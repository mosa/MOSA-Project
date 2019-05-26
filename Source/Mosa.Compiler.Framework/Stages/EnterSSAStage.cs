// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.Analysis;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Framework.Trace;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Enter SSA Stage
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.BaseMethodCompilerStage" />
	public sealed class EnterSSAStage : BaseMethodCompilerStage
	{
		private Dictionary<Operand, Stack<int>> variables;
		private Dictionary<Operand, int> counts;
		private Dictionary<Operand, Operand[]> ssaOperands;
		private Dictionary<BasicBlock, SimpleFastDominance> blockAnalysis;
		private Dictionary<Operand, List<BasicBlock>> assignments;
		private Dictionary<Operand, Operand> parentOperand;

		private Counter InstructionCount = new Counter("EnterSSAStage.IRInstructions");

		private TraceLog trace;

		protected override void Initialize()
		{
			Register(InstructionCount);
		}

		protected override void Setup()
		{
			ssaOperands = new Dictionary<Operand, Operand[]>();
			blockAnalysis = new Dictionary<BasicBlock, SimpleFastDominance>();
			assignments = new Dictionary<Operand, List<BasicBlock>>();
			parentOperand = new Dictionary<Operand, Operand>();
		}

		protected override void Run()
		{
			// Method is empty - must be a plugged method
			if (BasicBlocks.HeadBlocks.Count == 0)
				return;

			if (HasProtectedRegions)
				return;

			trace = CreateTraceLog(8);

			foreach (var headBlock in BasicBlocks.HeadBlocks)
			{
				var analysis = new SimpleFastDominance(BasicBlocks, headBlock);
				blockAnalysis.Add(headBlock, analysis);
			}

			CollectAssignments();

			PlacePhiFunctionsMinimal();

			EnterSSA();
		}

		protected override void Finish()
		{
			// Clean up
			trace = null;
			variables = null;
			counts = null;
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

			MethodCompiler.IsInSSAForm = true;
		}

		/// <summary>
		/// Enters the SSA.
		/// </summary>
		/// <param name="headBlock">The head block.</param>
		private void EnterSSA(BasicBlock headBlock)
		{
			var analysis = blockAnalysis[headBlock];

			variables = new Dictionary<Operand, Stack<int>>();
			counts = new Dictionary<Operand, int>();

			foreach (var op in assignments.Keys)
			{
				AddToAssignments(op);
			}

			if (headBlock.NextBlocks.Count > 0)
			{
				RenameVariables(headBlock.NextBlocks[0], analysis);
			}
		}

		/// <summary>
		/// Adds to assignments.
		/// </summary>
		/// <param name="operand">The operand.</param>
		private void AddToAssignments(Operand operand)
		{
			if (!variables.ContainsKey(operand))
			{
				variables[operand] = new Stack<int>();
				counts.Add(operand, 0);
				variables[operand].Push(0);

				if (!ssaOperands.ContainsKey(operand))
				{
					ssaOperands.Add(operand, new Operand[operand.Definitions.Count + 1]);
				}
			}
		}

		/// <summary>
		/// Gets the SSA operand.
		/// </summary>
		/// <param name="operand">The operand.</param>
		/// <param name="version">The version.</param>
		/// <returns></returns>
		private Operand GetSSAOperand(Operand operand, int version)
		{
			var ssaArray = ssaOperands[operand];
			var ssaOperand = ssaArray[version];

			if (ssaOperand == null)
			{
				ssaOperand = version == 0 ? operand : AllocateVirtualRegister(operand.Type);
				ssaArray[version] = ssaOperand;

				parentOperand.Add(ssaOperand, operand);
			}

			return ssaOperand;
		}

		/// <summary>
		/// Renames the variables.
		/// </summary>
		/// <param name="block">The block.</param>
		/// <param name="dominanceAnalysis">The dominance analysis.</param>
		private void RenameVariables2(BasicBlock block, SimpleFastDominance dominanceAnalysis)
		{
			trace?.Log($"Processing: {block}");

			UpdateOperands(block);
			UpdatePHIs(block);

			// Repeat for all children of the dominance block, if any
			var children = dominanceAnalysis.GetChildren(block);
			if (children != null && children.Count != 0)
			{
				foreach (var s in children)
				{
					RenameVariables2(s, dominanceAnalysis);
				}
			}

			UpdateResultOperands(block);
		}

		/// <summary>
		/// Renames the variables.
		/// </summary>
		/// <param name="headBlock">The head block.</param>
		/// <param name="dominanceAnalysis">The dominance analysis.</param>
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
			// Update Operands in current block
			for (var node = block.First.Next; !node.IsBlockEndInstruction; node = node.Next)
			{
				if (node.IsEmptyOrNop)
					continue;

				if (node.Instruction != IRInstruction.Phi)
				{
					for (var i = 0; i < node.OperandCount; ++i)
					{
						var op = node.GetOperand(i);

						if (op == null || !op.IsVirtualRegister)
							continue;

						if (!variables.ContainsKey(op))
							AllStopWithException("");

						Debug.Assert(variables.ContainsKey(op), $"{op} is not in dictionary [block = {block}]");

						var version = variables[op].Peek();
						node.SetOperand(i, GetSSAOperand(op, version));
					}
				}

				if (node.Result?.IsVirtualRegister == true)
				{
					var op = node.Result;

					Debug.Assert(counts.ContainsKey(op), $"{op} is not in counts");

					var index = counts[op];
					node.Result = GetSSAOperand(op, index);
					variables[op].Push(index);
					counts[op] = index + 1;
				}

				if (node.Result2?.IsVirtualRegister == true)
				{
					var op = node.Result2;

					Debug.Assert(counts.ContainsKey(op), $"{op} is not in counts");

					var index = counts[op];
					node.Result2 = GetSSAOperand(op, index);
					variables[op].Push(index);
					counts[op] = index + 1;
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

					if (node.Instruction != IRInstruction.Phi)
						break;

					Debug.Assert(node.OperandCount == node.Block.PreviousBlocks.Count);

					var op = node.GetOperand(index);

					if (variables[op].Count > 0)
					{
						var version = variables[op].Peek();
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

				if (node.Result?.IsVirtualRegister == true)
				{
					//var op = node.Result.SSAParent;
					var op = parentOperand[node.Result];
					var index = variables[op].Pop();
				}

				if (node.Result2?.IsVirtualRegister == true)
				{
					//var op = node.Result2.SSAParent;
					var op = parentOperand[node.Result2];
					var index = variables[op].Pop();
				}
			}
		}

		/// <summary>
		/// Which the predecessor.
		/// </summary>
		/// <param name="y">The y.</param>
		/// <param name="x">The x.</param>
		/// <returns></returns>
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

		/// <summary>
		/// Collects the assignments.
		/// </summary>
		private void CollectAssignments()
		{
			foreach (var block in BasicBlocks)
			{
				for (var node = block.AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmpty)
						continue;

					InstructionCount++;

					if (node.Result?.IsVirtualRegister == true)
					{
						AddToAssignments(node.Result, block);
					}

					if (node.Result2?.IsVirtualRegister == true)
					{
						AddToAssignments(node.Result2, block);
					}
				}
			}
		}

		/// <summary>
		/// Adds to assignments.
		/// </summary>
		/// <param name="operand">The operand.</param>
		/// <param name="block">The block.</param>
		private void AddToAssignments(Operand operand, BasicBlock block)
		{
			if (!assignments.TryGetValue(operand, out List<BasicBlock> blocks))
			{
				blocks = new List<BasicBlock>();
				assignments.Add(operand, blocks);
			}

			blocks.AddIfNew(block);
		}

		/// <summary>
		/// Inserts the phi instruction.
		/// </summary>
		/// <param name="block">The block.</param>
		/// <param name="variable">The variable.</param>
		private void InsertPhiInstruction(BasicBlock block, Operand variable)
		{
			var context = new Context(block);
			context.AppendInstruction(IRInstruction.Phi, variable);

			var sourceBlocks = new List<BasicBlock>(block.PreviousBlocks.Count);
			context.PhiBlocks = sourceBlocks;

			for (var i = 0; i < block.PreviousBlocks.Count; i++)
			{
				context.SetOperand(i, variable);
				sourceBlocks.Add(block.PreviousBlocks[i]);
			}

			context.OperandCount = block.PreviousBlocks.Count;

			//Debug.Assert(context.OperandCount == context.Block.PreviousBlocks.Count);
		}

		/// <summary>
		/// Places the phi functions minimal.
		/// </summary>
		private void PlacePhiFunctionsMinimal()
		{
			foreach (var headBlock in BasicBlocks.HeadBlocks)
			{
				PlacePhiFunctionsMinimal(headBlock);
			}
		}

		/// <summary>
		/// Places the phi functions minimal.
		/// </summary>
		/// <param name="headBlock">The head block.</param>
		private void PlacePhiFunctionsMinimal(BasicBlock headBlock)
		{
			var analysis = blockAnalysis[headBlock];

			foreach (var t in assignments)
			{
				var blocks = t.Value;

				if (blocks.Count < 2)
					continue;

				blocks.AddIfNew(headBlock);

				foreach (var n in analysis.IteratedDominanceFrontier(blocks))
				{
					InsertPhiInstruction(n, t.Key);
				}
			}
		}
	}
}
