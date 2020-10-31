// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.Analysis;
using Mosa.Compiler.Framework.Trace;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Enter SSA Stage
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.BaseMethodCompilerStage" />
	public sealed class EnterSSAStage : BaseMethodCompilerStage
	{
		private Dictionary<Operand, Stack<int>> stack;
		private Dictionary<Operand, int> counters;
		private Dictionary<Operand, Operand[]> ssaOperands;
		private Dictionary<BasicBlock, SimpleFastDominance> blockAnalysis;
		private Dictionary<Operand, List<BasicBlock>> assignments;
		private Dictionary<Operand, Operand> parentOperand;
		private List<Context> phiInstructions;

		private TraceLog trace;

		protected override void Setup()
		{
			ssaOperands = new Dictionary<Operand, Operand[]>();
			blockAnalysis = new Dictionary<BasicBlock, SimpleFastDominance>();
			assignments = new Dictionary<Operand, List<BasicBlock>>();
			parentOperand = new Dictionary<Operand, Operand>();
			phiInstructions = new List<Context>();
		}

		protected override void Run()
		{
			if (!HasCode)
				return;

			if (HasProtectedRegions)
				return;

			trace = CreateTraceLog(8);

			foreach (var headBlock in BasicBlocks.HeadBlocks)
			{
				var analysis = new SimpleFastDominance(BasicBlocks, headBlock);
				blockAnalysis.Add(headBlock, analysis);
			}

			CollectAssignments2();

			PlacePhiFunctionsMinimal();

			EnterSSA();

			RemoveUselessPhiInstructions();

			MethodCompiler.IsInSSAForm = true;
		}

		protected override void Finish()
		{
			// Clean up
			phiInstructions.Clear();
			trace = null;
			stack = null;
			counters = null;
			ssaOperands = null;
			assignments = null;
			blockAnalysis = null;
			parentOperand = null;
		}

		private void EnterSSA()
		{
			foreach (var headBlock in BasicBlocks.HeadBlocks)
			{
				EnterSSA(headBlock);
			}
		}

		private void EnterSSA(BasicBlock headBlock)
		{
			stack = new Dictionary<Operand, Stack<int>>();
			counters = new Dictionary<Operand, int>();

			foreach (var op in assignments.Keys)
			{
				AddToAssignments(op);
			}

			if (headBlock.NextBlocks.Count > 0)
			{
				RenameVariables(headBlock.NextBlocks[0], blockAnalysis[headBlock]);
			}
		}

		private void AddToAssignments(Operand operand)
		{
			if (!stack.ContainsKey(operand))
			{
				stack[operand] = new Stack<int>();
				counters.Add(operand, 0);
				stack[operand].Push(0);

				if (!ssaOperands.ContainsKey(operand))
				{
					ssaOperands.Add(operand, new Operand[operand.Definitions.Count + 1]);
				}
			}
		}

		private Operand GetSSAOperand(Operand operand, int version)
		{
			var ssaArray = ssaOperands[operand];
			var ssaOperand = ssaArray[version];

			if (ssaOperand == null)
			{
				ssaOperand = AllocateVirtualRegister(operand);
				ssaArray[version] = ssaOperand;

				parentOperand.Add(ssaOperand, operand);
			}

			return ssaOperand;
		}

		private void RenameVariables(BasicBlock headBlock, SimpleFastDominance dominanceAnalysis)
		{
			var worklist = new Stack<BasicBlock>();

			worklist.Push(headBlock);

			while (worklist.Count != 0)
			{
				var block = worklist.Pop();

				if (block != null)
				{
					trace?.Log($"Processing: {block}");

					UpdateOperands(block);
					UpdatePHIs(block);

					worklist.Push(block);
					worklist.Push(null);

					trace?.Log($"  >Pushed: {block} (Return)");

					// Repeat for all children of the dominance block, if any
					var children = dominanceAnalysis.GetChildren(block);
					if (children != null && children.Count > 0)
					{
						foreach (var s in children)
						{
							worklist.Push(s);

							trace?.Log($"  >Pushed: {s}");
						}
					}
				}
				else
				{
					block = worklist.Pop();

					trace?.Log($"Processing: {block} (Back)");
					UpdateResultOperands(block);
				}
			}
		}

		private void UpdateOperands(BasicBlock block)
		{
			for (var node = block.First.Next; !node.IsBlockEndInstruction; node = node.Next)
			{
				if (node.IsEmptyOrNop)
					continue;

				if (!IsPhiInstruction(node.Instruction))
				{
					for (var i = 0; i < node.OperandCount; ++i)
					{
						var op = node.GetOperand(i);

						if (!op.IsVirtualRegister)
							continue;

						var version = stack[op].Peek();
						node.SetOperand(i, GetSSAOperand(op, version));
					}
				}

				if (node.Result?.IsVirtualRegister == true)
				{
					var op = node.Result;
					var index = counters[op];
					node.Result = GetSSAOperand(op, index);
					stack[op].Push(index);
					counters[op] = index + 1;
				}

				if (node.Result2?.IsVirtualRegister == true)
				{
					var op = node.Result2;
					var index = counters[op];
					node.Result2 = GetSSAOperand(op, index);
					stack[op].Push(index);
					counters[op] = index + 1;
				}
			}
		}

		private void UpdatePHIs(BasicBlock block)
		{
			// Update PHIs in successor blocks
			foreach (var s in block.NextBlocks)
			{
				var index = WhichPredecessor(s, block);

				for (var node = s.First.Next; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmptyOrNop)
						continue;

					if (!IsPhiInstruction(node.Instruction))
						break;

					var op = node.GetOperand(index);

					if (stack[op].Count > 0)
					{
						var version = stack[op].Peek();
						var ssaOperand = GetSSAOperand(node.GetOperand(index), version);
						node.SetOperand(index, ssaOperand);
					}
				}
			}
		}

		private void UpdateResultOperands(BasicBlock block)
		{
			// Update Result Operands in current block
			for (var node = block.First.Next; !node.IsBlockEndInstruction; node = node.Next)
			{
				if (node.IsEmptyOrNop || node.ResultCount == 0)
					continue;

				if (node.Result.IsVirtualRegister == true)
				{
					var op = parentOperand[node.Result];
					stack[op].Pop();
				}

				if (node.Result2?.IsVirtualRegister == true)
				{
					var op = parentOperand[node.Result2];
					stack[op].Pop();
				}
			}
		}

		private int WhichPredecessor(BasicBlock y, BasicBlock x)
		{
			for (var i = 0; i < y.PreviousBlocks.Count; ++i)
			{
				if (y.PreviousBlocks[i] == x)
				{
					return i;
				}
			}

			return -1;
		}

		private void CollectAssignments2()
		{
			foreach (var operand in MethodCompiler.VirtualRegisters)
			{
				//if (operand.Definitions.Count <= 1)
				//continue;

				var blocks = new List<BasicBlock>(operand.Definitions.Count);
				assignments.Add(operand, blocks);

				foreach (var def in operand.Definitions)
				{
					blocks.AddIfNew(def.Block);
				}
			}
		}

		private Context InsertPhiInstruction(BasicBlock block, Operand variable)
		{
			//trace?.Log($"     Phi: {variable} into {block}");

			var context = new Context(block);

			if (variable.IsReferenceType)
				context.AppendInstruction(IRInstruction.PhiObject, variable);
			else if (variable.IsR4)
				context.AppendInstruction(IRInstruction.PhiR4, variable);
			else if (variable.IsR8)
				context.AppendInstruction(IRInstruction.PhiR8, variable);
			else if (variable.IsInteger64)
				context.AppendInstruction(IRInstruction.Phi64, variable);
			else
				context.AppendInstruction(IRInstruction.Phi32, variable);

			var sourceBlocks = new List<BasicBlock>(block.PreviousBlocks.Count);
			context.PhiBlocks = sourceBlocks;

			for (var i = 0; i < block.PreviousBlocks.Count; i++)
			{
				context.SetOperand(i, variable);
				sourceBlocks.Add(block.PreviousBlocks[i]);
			}

			context.OperandCount = block.PreviousBlocks.Count;

			phiInstructions.Add(context);

			return context;
		}

		private void PlacePhiFunctionsMinimal()
		{
			foreach (var headBlock in BasicBlocks.HeadBlocks)
			{
				PlacePhiFunctionsMinimal(headBlock);
			}
		}

		private void PlacePhiFunctionsMinimal(BasicBlock headBlock)
		{
			var analysis = blockAnalysis[headBlock];

			foreach (var t in assignments)
			{
				var operand = t.Key;
				var blocks = t.Value;

				//trace?.Log($"Operand {operand}");

				if (blocks.Count < 2)
					continue;

				foreach (var block in analysis.IteratedDominanceFrontier(blocks))
				{
					InsertPhiInstruction(block, operand);
				}

				//trace?.Log($"Operand {operand} defined in {ToString(blocks)}");
				//trace?.Log("");

				//var workList = new Stack<BasicBlock>(blocks);
				//var everOnWorkList = new HashSet<BasicBlock>(blocks);
				//var alreadyHasPhiFunction = new HashSet<BasicBlock>();

				//while (workList.Count != 0)
				//{
				//	var node = workList.Pop();

				//	var df = analysis.GetDominanceFrontier(node);

				//	if (df != null)
				//	{
				//		//trace?.Log($" DF ( {node} ) -> {ToString(df)}");

				//		foreach (var d in df)
				//		{
				//			if (!alreadyHasPhiFunction.Contains(d))
				//			{
				//				InsertPhiInstruction(d, operand);
				//				alreadyHasPhiFunction.Add(d);

				//				if (!everOnWorkList.Contains(d))
				//				{
				//					workList.Push(d);
				//					everOnWorkList.Add(d);
				//				}
				//			}
				//		}
				//	}
				//}

				//trace?.Log("");
			}
		}

		private void RemoveUselessPhiInstructions()
		{
			foreach (var context in phiInstructions)
			{
				if (context.Result == context.Operand1)
				{
					context.Empty();
				}
			}
		}

		private static string ToString(List<BasicBlock> blocks)
		{
			var sb = new StringBuilder();

			foreach (var block in blocks)
			{
				sb.Append(block);
				sb.Append(" ");
			}

			if (blocks.Count != 0)
				sb.Length -= 1;

			return sb.ToString();
		}
	}
}
