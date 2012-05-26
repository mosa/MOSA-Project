/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <rootnode@mosa-project.org>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Framework.Operands;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// 
	/// </summary>
	public class EnterSSAStage : BaseMethodCompilerStage, IMethodCompilerStage, IPipelineStage
	{

		/// <summary>
		/// 
		/// </summary>
		private Dictionary<string, Operand> oldLefHandSide = new Dictionary<string, Operand>();
		/// <summary>
		/// 
		/// </summary>
		private Dictionary<Operand, Stack<int>> variables = new Dictionary<Operand, Stack<int>>();
		/// <summary>
		/// 
		/// </summary>
		private IDominanceProvider dominanceCalculation;
		/// <summary>
		/// 
		/// </summary>
		private PhiPlacementStage phiPlacementStage;

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void IMethodCompilerStage.Run()
		{
			if (HasExceptionOrFinally)
				return;

			if (methodCompiler.PlugSystem != null)
				if (methodCompiler.PlugSystem.GetPlugMethod(methodCompiler.Method) != null)
					return;

			dominanceCalculation = methodCompiler.Pipeline.FindFirst<DominanceCalculationStage>().DominanceProvider;
			phiPlacementStage = methodCompiler.Pipeline.FindFirst<PhiPlacementStage>();

			foreach (var op in phiPlacementStage.Assignments.Keys)
			{
				variables[op] = new Stack<int>();
			}

			foreach (var op in methodCompiler.Parameters)
			{
				if (!variables.ContainsKey(op))
					variables[op] = new Stack<int>();

				variables[op].Push(0);
			}

			if (methodCompiler.LocalVariables != null)
				foreach (var op in methodCompiler.LocalVariables)
				{
					if (!variables.ContainsKey(op))
						variables[op] = new Stack<int>();

					variables[op].Push(0);
				}

			RenameVariables(basicBlocks.PrologueBlock.NextBlocks[0]);
		}

		/// <summary>
		/// Renames the variables.
		/// </summary>
		/// <param name="block">The block.</param>
		private void RenameVariables(BasicBlock block)
		{
			for (var context = new Context(instructionSet, block); !context.EndOfInstruction; context.GotoNext())
			{
				if (!(context.Instruction is Phi))
				{
					for (var i = 0; i < context.OperandCount; ++i)
					{
						var op = context.GetOperand(i);

						if (!(op is StackOperand))
							continue;

						if (!variables.ContainsKey(op))
							throw new Exception(op.ToString() + " is not in dictionary [block = " + block + "]");

						var index = variables[op].Peek();
						context.SetOperand(i, new SsaOperand(context.GetOperand(i), index));
					}
				}

				if (PhiPlacementStage.IsAssignmentToStackVariable(context))
				{
					var op = context.Result;
					var index = variables[op].Count;
					context.SetResult(new SsaOperand(op, index));
					variables[op].Push(index);
				}
			}

			foreach (var s in block.NextBlocks)
			{
				var j = WhichPredecessor(s, block);
				for (var context = new Context(instructionSet, s); !context.EndOfInstruction; context.GotoNext())
				{
					if (!(context.Instruction is Phi))
						continue;

					var op = context.GetOperand(j);

					if (variables[op].Count > 0)
					{
						var index = variables[op].Peek();
						context.SetOperand(j, new SsaOperand(context.GetOperand(j), index));
					}
				}
			}

			foreach (var s in dominanceCalculation.GetChildren(block))
			{
				RenameVariables(s);
			}

			for (var context = new Context(instructionSet, block); !context.EndOfInstruction; context.GotoNext())
			{
				if (PhiPlacementStage.IsAssignmentToStackVariable(context))
				{
					var instName = context.Label + "." + context.Index;
					var op = oldLefHandSide[instName];
					variables[op].Pop();
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

		/// <summary>
		/// Determines whether the specified context is assignment.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns>
		///   <c>true</c> if the specified context is assignment; otherwise, <c>false</c>.
		/// </returns>
		private bool IsAssignment(Context context)
		{
			if (context.Result == null)
				return false;

			return (context.Result is StackOperand);
		}

	}
}
