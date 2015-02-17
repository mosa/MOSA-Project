/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <rootnode@mosa-project.org>
 */

using Mosa.Compiler.Framework.Analysis;
using Mosa.Compiler.Framework.IR;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	///
	/// </summary>
	public sealed class EnterSSAStage : BaseMethodCompilerStage
	{
		private PhiPlacementStage phiPlacementStage;
		private Dictionary<Operand, Stack<int>> variables;
		private Dictionary<Operand, int> counts;

		private Dictionary<Operand, Operand[]> ssaOperands = new Dictionary<Operand, Operand[]>();

		protected override void Run()
		{
			// Method is empty - must be a plugged method
			if (BasicBlocks.HeadBlocks.Count == 0)
				return;

			if (HasProtectedRegions)
				return;

			phiPlacementStage = MethodCompiler.Pipeline.FindFirst<PhiPlacementStage>();

			foreach (var headBlock in BasicBlocks.HeadBlocks)
			{
				EnterSSA(headBlock);
			}

			ssaOperands = null;
		}

		protected override void Finish()
		{
			// Clean up
			variables = null;
			counts = null;
		}

		/// <summary>
		/// Enters the SSA.
		/// </summary>
		/// <param name="headBlock">The head block.</param>
		private void EnterSSA(BasicBlock headBlock)
		{
			var analysis = MethodCompiler.DominanceAnalysis.GetDominanceAnalysis(headBlock);

			variables = new Dictionary<Operand, Stack<int>>();
			counts = new Dictionary<Operand, int>();

			foreach (var op in phiPlacementStage.Assignments.Keys)
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
		/// <param name="dominance">The dominance provider.</param>
		private void RenameVariables(BasicBlock block, IDominanceAnalysis dominance)
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

						Debug.Assert(variables.ContainsKey(op), op.ToString() + " is not in dictionary [block = " + block + "]");

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

					Debug.Assert(context.OperandCount == context.BasicBlock.PreviousBlocks.Count);

					var op = context.GetOperand(index);

					if (variables[op].Count > 0)
					{
						var version = variables[op].Peek();
						context.SetOperand(index, GetSSAOperand(context.GetOperand(index), version));
					}
				}
			}

			foreach (var s in dominance.GetChildren(block))
			{
				RenameVariables(s, dominance);
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
				if (y.PreviousBlocks[i] == x)
					return i;
			return -1;
		}
	}
}
