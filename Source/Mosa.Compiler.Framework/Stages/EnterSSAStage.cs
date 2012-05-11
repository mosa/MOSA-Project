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
		private class AssignmentInformation
		{
			public List<BasicBlock> AssigningBlocks = new List<BasicBlock>();
			public Operand Operand;

			public AssignmentInformation(Operand operand)
			{
				this.Operand = operand;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private class VariableInformation
		{
			public int Count = 0;
			public Stack<int> Stack = new Stack<int>();
		}

		/// <summary>
		/// 
		/// </summary>
		private Dictionary<string, Operand> oldLefHandSide = new Dictionary<string,Operand>();
		/// <summary>
		/// 
		/// </summary>
		private Dictionary<string, VariableInformation> variableInformation = new Dictionary<string, VariableInformation>();
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
			if (AreExceptions)
				return;

			foreach (var block in this.basicBlocks)
				if (block.NextBlocks.Count == 0 && block.PreviousBlocks.Count == 0)
					return;

			this.dominanceCalculation = this.methodCompiler.Pipeline.FindFirst<DominanceCalculationStage>().DominanceProvider;
			this.phiPlacementStage = this.methodCompiler.Pipeline.FindFirst<PhiPlacementStage>();

			var numberOfParameters = this.methodCompiler.Method.Parameters.Count;
			if (this.methodCompiler.Method.Signature.HasThis)
				++numberOfParameters;

			foreach (var name in this.phiPlacementStage.Assignments.Keys)
				this.variableInformation[name] = new VariableInformation();

			for (var i = 0; i < numberOfParameters; ++i)
			{
				var op = methodCompiler.GetParameterOperand(i);
				var name = NameForOperand(op);
				this.variableInformation[name].Stack.Push(0);
				this.variableInformation[name].Count = 1;
			}

			for (var i = 0; methodCompiler.LocalVariables != null && i < methodCompiler.LocalVariables.Length; ++i)
			{
				var op = methodCompiler.LocalVariables[i];
				var name = NameForOperand(op);
				if (!this.variableInformation.ContainsKey(name))
					this.variableInformation[name] = new VariableInformation();
				this.variableInformation[name].Stack.Push(0);
				this.variableInformation[name].Count = 1;
			}
			this.RenameVariables(basicBlocks.PrologueBlock.NextBlocks[0]);
			Debug.WriteLine("ESSA: " + this.methodCompiler.Method.FullName);
		}

		/// <summary>
		/// Renames the variables.
		/// </summary>
		/// <param name="block">The block.</param>
		private void RenameVariables(BasicBlock block)
		{
			for (var context = new Context(this.instructionSet, block); !context.EndOfInstruction; context.GotoNext())
			{
				if (!(context.Instruction is Phi))
				{
					for (var i = 0; i < context.OperandCount; ++i)
					{
						var op = context.GetOperand(i);
						if (!(op is StackOperand))
							continue;
						var name = NameForOperand(context.GetOperand(i));
						if (!this.variableInformation.ContainsKey(name))
							throw new Exception(name + " is not in dictionary [block = " + block + "]");
						var index = this.variableInformation[name].Stack.Peek();
						context.SetOperand(i, new SsaOperand(context.GetOperand(i), index));
					}
				}

				if (PhiPlacementStage.IsAssignmentToStackVariable(context))
				{
					var name = NameForOperand(context.Result);
					var index = this.variableInformation[name].Count;
					context.SetResult(new SsaOperand(context.Result, index));
					this.variableInformation[name].Stack.Push(index);
					++this.variableInformation[name].Count;
				}
			}

			foreach (var s in block.NextBlocks)
			{
				var j = this.WhichPredecessor(s, block);
				for (var context = new Context(this.instructionSet, s); !context.EndOfInstruction; context.GotoNext())
				{
					if (!(context.Instruction is Phi))
						continue;
					var name = NameForOperand(context.GetOperand(j));
					if (this.variableInformation[name].Stack.Count > 0)
					{
						var index = this.variableInformation[name].Stack.Peek();
						context.SetOperand(j, new SsaOperand(context.GetOperand(j), index));
					}
				}
			}

			foreach (var s in this.dominanceCalculation.GetChildren(block))
			{
				this.RenameVariables(s);
			}

			for (var context = new Context(this.instructionSet, block); !context.EndOfInstruction; context.GotoNext())
			{
				if (PhiPlacementStage.IsAssignmentToStackVariable(context))
				{
					var instName = context.Label + "." + context.Index;
					var op = this.oldLefHandSide[instName];
					var name = NameForOperand(op);
					this.variableInformation[name].Stack.Pop();
				}
			}
		}

		/// <summary>
		/// Whiches the predecessor.
		/// </summary>
		/// <param name="y">The y.</param>
		/// <param name="x">The x.</param>
		/// <returns></returns>
		private int WhichPredecessor(BasicBlock y, BasicBlock x)
		{
			for (var i = 0; i < y.PreviousBlocks.Count; ++i)
				if (y.PreviousBlocks[i].Sequence == x.Sequence)
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
			var op = context.Result;
			if (op == null)
				return false;

			return (op is StackOperand);
		}

		/// <summary>
		/// Names for operand.
		/// </summary>
		/// <param name="operand">The operand.</param>
		/// <returns></returns>
		private string NameForOperand(Operand operand)
		{
			return PhiPlacementStage.NameForOperand(operand);
		}
	}
}
