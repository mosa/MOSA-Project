// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.Analysis;
using Mosa.Compiler.Framework.IR;
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

		private Dictionary<Operand, Operand[]> ssaOperands = new Dictionary<Operand, Operand[]>();

		private Dictionary<BasicBlock, BaseDominanceAnalysis> blockAnalysis = new Dictionary<BasicBlock, BaseDominanceAnalysis>();

		private Dictionary<Operand, List<BasicBlock>> assignments = new Dictionary<Operand, List<BasicBlock>>();

		protected override void Run()
		{
			// Method is empty - must be a plugged method
			if (BasicBlocks.HeadBlocks.Count == 0)
				return;

			if (HasProtectedRegions)
				return;

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
			UpdateCounter("EnterSSA.IRInstructions", instructionCount);

			// Clean up
			variables = null;
			counts = null;
			ssaOperands = null;
			assignments = null;
			blockAnalysis = null;
		}

		private void EnterSSA()
		{
			foreach (var headBlock in BasicBlocks.HeadBlocks)
			{
				EnterSSA(headBlock);
			}
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
				ssaOperand = Operand.CreateSSA(operand, version);
				ssaArray[version] = ssaOperand;
			}

			return ssaOperand;
		}

		/// <summary>
		/// Renames the variables.
		/// </summary>
		/// <param name="block">The block.</param>
		/// <param name="dominanceAnalysis">The dominance analysis.</param>
		private void RenameVariables(BasicBlock block, BaseDominanceAnalysis dominanceAnalysis)
		{
			for (var node = block.First; !node.IsBlockEndInstruction; node = node.Next)
			{
				if (node.Instruction != IRInstruction.Phi)
				{
					for (var i = 0; i < node.OperandCount; ++i)
					{
						var op = node.GetOperand(i);

						if (op == null || !op.IsVirtualRegister)
							continue;

						Debug.Assert(variables.ContainsKey(op), op + " is not in dictionary [block = " + block + "]");

						var version = variables[op].Peek();
						node.SetOperand(i, GetSSAOperand(op, version));
					}
				}

				if (!node.IsEmpty && node.Result != null && node.Result.IsVirtualRegister)
				{
					var op = node.Result;
					var index = counts[op];
					node.Result = GetSSAOperand(op, index);
					variables[op].Push(index);
					counts[op] = index + 1;
				}
			}

			foreach (var s in block.NextBlocks)
			{
				// index does not change between this stage and PhiPlacementStage since the block list order does not change
				var index = WhichPredecessor(s, block);

				for (var context = new Context(s); !context.IsBlockEndInstruction; context.GotoNext())
				{
					if (context.Instruction != IRInstruction.Phi)
						continue;

					Debug.Assert(context.OperandCount == context.Block.PreviousBlocks.Count);

					var op = context.GetOperand(index);

					if (variables[op].Count > 0)
					{
						var version = variables[op].Peek();
						context.SetOperand(index, GetSSAOperand(context.GetOperand(index), version));
					}
				}
			}

			foreach (var s in dominanceAnalysis.GetChildren(block))
			{
				RenameVariables(s, dominanceAnalysis);
			}

			for (var context = new Context(block); !context.IsBlockEndInstruction; context.GotoNext())
			{
				if (!context.IsEmpty && context.Result != null && context.Result.IsVirtualRegister)
				{
					var op = context.Result.SSAParent;
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
				for (var context = new Context(block); !context.IsBlockEndInstruction; context.GotoNext())
				{
					if (context.IsEmpty)
						continue;

					instructionCount++;

					if (context.Result == null)
						continue;

					if (context.Result.IsVirtualRegister)
					{
						AddToAssignments(context.Result, block);
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

			//var sourceBlocks = new BasicBlock[block.PreviousBlocks.Count];
			var sourceBlocks = new List<BasicBlock>(block.PreviousBlocks.Count);
			context.PhiBlocks = sourceBlocks;

			for (var i = 0; i < block.PreviousBlocks.Count; i++)
			{
				context.SetOperand(i, variable);
				sourceBlocks.Add(block.PreviousBlocks[i]);
			}

			context.OperandCount = block.PreviousBlocks.Count;

			Debug.Assert(context.OperandCount == context.Block.PreviousBlocks.Count);
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
