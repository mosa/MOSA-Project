/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <rootnode@mosa-project.org>
 */

using Mosa.Compiler.Framework.IR;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	///
	/// </summary>
	public sealed class EnterSSAStage : BaseMethodCompilerStage, IMethodCompilerStage, IPipelineStage
	{
		private PhiPlacementStage phiPlacementStage;
		private Dictionary<Operand, Stack<int>> variables;
		private Dictionary<Operand, int> counts;

		private Dictionary<Operand, Operand[]> ssaOperands = new Dictionary<Operand, Operand[]>();

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void IMethodCompilerStage.Run()
		{
			// Method is empty - must be a plugged method
			if (basicBlocks.HeadBlocks.Count == 0)
				return;

			phiPlacementStage = methodCompiler.Pipeline.FindFirst<PhiPlacementStage>();

			foreach (var headBlock in basicBlocks.HeadBlocks)
			{
				EnterSSA(headBlock);
			}

			ssaOperands = null;
		}

		/// <summary>
		/// Enters the SSA.
		/// </summary>
		/// <param name="headBlock">The head block.</param>
		private void EnterSSA(BasicBlock headBlock)
		{
			var analysis = methodCompiler.DominanceAnalysis.GetDominanceAnalysis(headBlock);

			variables = new Dictionary<Operand, Stack<int>>();
			counts = new Dictionary<Operand, int>();

			foreach (var op in phiPlacementStage.Assignments.Keys)
			{
				AddToAssignments(op);
			}

			foreach (var op in methodCompiler.Parameters)
			{
				AddToAssignments(op);
			}

			foreach (var op in methodCompiler.LocalVariables)
			{
				if (op.Uses.Count != 0)
				{
					AddToAssignments(op);
				}
			}

			if (headBlock.NextBlocks.Count > 0)
			{
				RenameVariables(headBlock.NextBlocks[0], analysis);
			}

			// Clean up
			analysis = null;
			variables = null;
			counts = null;
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
		/// <param name="dominanceProvider">The dominance provider.</param>
		private void RenameVariables(BasicBlock block, IDominanceAnalysis dominanceProvider)
		{
			for (var context = new Context(instructionSet, block); !context.IsBlockEndInstruction; context.GotoNext())
			{
				if (!(context.Instruction is Phi))
				{
					for (var i = 0; i < context.OperandCount; ++i)
					{
						var op = context.GetOperand(i);

						if (op == null || !op.IsVirtualRegister)
							continue;

						//Debug.Assert(variables.ContainsKey(op), op.ToString() + " is not in dictionary [block = " + block + "]");

						var index = variables[op].Peek();
						context.SetOperand(i, GetSSAOperand(op, index));
					}
				}

				if (!context.IsEmpty && context.Result != null && context.Result.IsVirtualRegister)
				{
					var op = context.Result;
					var index = counts[op];
					context.SetResult(GetSSAOperand(op, index));
					variables[op].Push(index);
					counts[op] = index + 1;
				}
			}

			foreach (var s in block.NextBlocks)
			{
				var j = WhichPredecessor(s, block); // ???

				for (var context = new Context(instructionSet, s); !context.IsBlockEndInstruction; context.GotoNext())
				{
					if (!(context.Instruction is Phi))
						continue;

					var op = context.GetOperand(j);

					if (variables[op].Count > 0)
					{
						var index = variables[op].Peek();
						context.SetOperand(j, GetSSAOperand(context.GetOperand(j), index));
					}
				}
			}

			foreach (var s in dominanceProvider.GetChildren(block))
			{
				RenameVariables(s, dominanceProvider);
			}

			for (var context = new Context(instructionSet, block); !context.IsBlockEndInstruction; context.GotoNext())
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