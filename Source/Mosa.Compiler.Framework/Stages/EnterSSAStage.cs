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
using Mosa.Compiler.Framework.IR;

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
		protected PhiPlacementStage phiPlacementStage;

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
				EnterSSA(headBlock);

			ssaOperands = null;
		}

		/// <summary>
		/// Enters the SSA.
		/// </summary>
		/// <param name="headBlock">The head block.</param>
		protected void EnterSSA(BasicBlock headBlock)
		{
			var dominanceCalculation = methodCompiler.Pipeline.FindFirst<DominanceCalculationStage>().GetDominanceProvider(headBlock);
			var variables = new Dictionary<Operand, Stack<int>>();

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
			{
				foreach (var op in methodCompiler.LocalVariables)
				{
					if (!variables.ContainsKey(op))
						variables[op] = new Stack<int>();

					variables[op].Push(0);
				}
			}

			if (headBlock.NextBlocks.Count > 0)
				RenameVariables(headBlock.NextBlocks[0], dominanceCalculation, variables);
		}

		public Operand CreateSsaOperand(Operand operand, int version)
		{
			Operand[] ssaArray;
			Operand ssaOperand;

			if (!ssaOperands.TryGetValue(operand, out ssaArray))
			{
				ssaArray = new Operand[Math.Max(4, version + 1)];
				ssaOperand = Operand.CreateSSA(operand, version);
				ssaArray[version] = ssaOperand;
				ssaOperands.Add(operand, ssaArray);
			}
			else
			{
				if (version >= ssaArray.Length)
				{
					ssaOperand = Operand.CreateSSA(operand, version);

					Operand[] newSsaArray = new Operand[ssaArray.Length * ssaArray.Length];
					ssaArray.CopyTo(newSsaArray, 0);
					newSsaArray[version] = ssaOperand;
					ssaOperands.Remove(operand);
					ssaOperands.Add(operand, newSsaArray);
				}
				else
				{
					ssaOperand = ssaArray[version];
					if (ssaOperand == null)
					{
						ssaOperand = Operand.CreateSSA(operand, version);
						ssaArray[version] = ssaOperand;
					}
				}
			}

			return ssaOperand;
		}

		/// <summary>
		/// Renames the variables.
		/// </summary>
		/// <param name="block">The block.</param>
		/// <param name="dominanceCalculation">The dominance calculation.</param>
		/// <param name="variables">The variables.</param>
		private void RenameVariables(BasicBlock block, IDominanceProvider dominanceCalculation, Dictionary<Operand, Stack<int>> variables)
		{
			for (var context = new Context(instructionSet, block); !context.EndOfInstruction; context.GotoNext())
			{
				if (!(context.Instruction is Phi))
				{
					for (var i = 0; i < context.OperandCount; ++i)
					{
						var op = context.GetOperand(i);

						if (op == null || !op.IsStackLocal)
							continue;

						if (!variables.ContainsKey(op))
							throw new Exception(op.ToString() + " is not in dictionary [block = " + block + "]");

						var index = variables[op].Peek();
						context.SetOperand(i, CreateSsaOperand(context.GetOperand(i), index));
					}
				}

				if (PhiPlacementStage.IsAssignmentToStackVariable(context))
				{
					var op = context.Result;
					var index = variables[op].Count;
					context.SetResult(CreateSsaOperand(op, index));
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
						context.SetOperand(j, CreateSsaOperand(context.GetOperand(j), index));
					}
				}
			}

			foreach (var s in dominanceCalculation.GetChildren(block))
			{
				RenameVariables(s, dominanceCalculation, variables);
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
			return (context.Result != null && context.Result.IsStackLocal);
		}

	}
}
