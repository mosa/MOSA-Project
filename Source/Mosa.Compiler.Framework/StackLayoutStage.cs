/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using Mosa.Compiler.Framework.Operands;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Calculates the layout of the stack of the method.
	/// </summary>
	public sealed class StackLayoutStage : BaseMethodCompilerStage, IMethodCompilerStage, IStackLayoutProvider, IPipelineStage
	{

		#region Data members

		/// <summary>
		/// Holds the total stack requirements of local variables of the compiled method.
		/// </summary>
		private int localsSize;

		#endregion // Data members

		/// <summary>
		/// Runs the specified method compiler.
		/// </summary>
		void IMethodCompilerStage.Run()
		{
			if (methodCompiler.PlugSystem != null)
				if (methodCompiler.PlugSystem.GetPlugMethod(this.methodCompiler.Method) != null)
					return;

			List<StackOperand> locals = CollectLocalVariablesFromIL();

			// Iterate and collect locals from all blocks
			foreach (BasicBlock block in basicBlocks)
			{
				CollectLocalVariables(locals, block);
			}

			// Sort all found locals
			OrderVariables(locals, callingConvention);

			// Now we assign increasing stack offsets to each variable
			localsSize = LayoutVariables(locals, callingConvention, callingConvention.OffsetOfFirstLocal, 1);

			// Layout parameters
			LayoutParameters(methodCompiler);

			// Create a prologue instruction
			Context prologueCtx = new Context(instructionSet, FindBlock(-1)).InsertBefore();
			prologueCtx.SetInstruction(IR.Instruction.PrologueInstruction);
			prologueCtx.Label = -1;

			// Create an epilogue instruction
			Context epilogueCtx = new Context(instructionSet, FindBlock(Int32.MaxValue));
			epilogueCtx.AppendInstruction(IR.Instruction.EpilogueInstruction);
			epilogueCtx.Label = Int32.MaxValue;
		}

		#region IStackLayoutStage Members

		int IStackLayoutProvider.LocalsSize
		{
			get { return localsSize; }
		}

		#endregion // IStackLayoutStage Members

		#region Internals

		private List<StackOperand> CollectLocalVariablesFromIL()
		{
			// Allocate a list of locals
			List<StackOperand> locals = new List<StackOperand>();

			var localVariables = methodCompiler.LocalVariables;

			if (localVariables == null)
				return locals;

			foreach (var localVariable in localVariables)
				locals.Add(localVariable as StackOperand);

			return locals;
		}

		/// <summary>
		/// Collects all local variables assignments into a list.
		/// </summary>
		/// <param name="locals">Holds all locals found by the stage.</param>
		/// <param name="block">The block.</param>
		private void CollectLocalVariables(List<StackOperand> locals, BasicBlock block)
		{
			for (Context ctx = new Context(instructionSet, block); !ctx.EndOfInstruction; ctx.GotoNext())
			{
				// Does this instruction define a new stack variable?
				foreach (Operand op in ctx.Results)
				{
					// The instruction list may not be in SSA form, so we have to check existence again here unfortunately.
					// FIXME: Allow us to detect the state of blocks
					LocalVariableOperand lvop = op as LocalVariableOperand;
					if (lvop != null && !locals.Contains(lvop))
						locals.Add(lvop);
				}
			}
		}

		/// <summary>
		/// Lays out all parameters of the method.
		/// </summary>
		/// <param name="compiler">The method compiler providing the parameters.</param>
		private void LayoutParameters(IMethodCompiler compiler)
		{
			List<StackOperand> paramOps = new List<StackOperand>();

			int offset = 0;

			if (compiler.Method.Signature.HasThis || compiler.Method.Signature.HasExplicitThis)
				++offset;

			for (int i = 0; i < compiler.Method.Parameters.Count + offset; ++i)
				paramOps.Add((StackOperand)compiler.GetParameterOperand(i));

			LayoutVariables(paramOps, callingConvention, callingConvention.OffsetOfFirstParameter, -1);
		}

		/// <summary>
		/// Performs a stack layout of all local variables in the list.
		/// </summary>
		/// <param name="locals">The enumerable holding all locals.</param>
		/// <param name="cc">The cc.</param>
		/// <param name="offsetOfFirst">Specifies the offset of the first stack operand in the list.</param>
		/// <param name="direction">The direction.</param>
		/// <returns></returns>
		private static int LayoutVariables(IEnumerable<StackOperand> locals, ICallingConvention cc, int offsetOfFirst, int direction)
		{
			int offset = offsetOfFirst;

			foreach (StackOperand lvo in locals)
			{
				// Does the offset fit the alignment requirement?
				int alignment;
				int size;
				int padding;
				int thisOffset;

				cc.GetStackRequirements(lvo, out size, out alignment);
				if (direction == 1)
				{
					padding = (offset % alignment);
					offset -= (padding + size);
					thisOffset = offset;
				}
				else
				{
					padding = (offset % alignment);
					if (padding != 0)
						padding = alignment - padding;

					thisOffset = offset;
					offset += (padding + size);
				}

				lvo.Offset = new IntPtr(thisOffset);
			}

			return offset;
		}

		/// <summary>
		/// Sorts all local variables by their size requirements.
		/// </summary>
		/// <param name="locals">Holds all local variables to sort..</param>
		/// <param name="cc">The calling convention used to determine size and alignment requirements.</param>
		private static void OrderVariables(List<StackOperand> locals, ICallingConvention cc)
		{
			// Sort the list by stack size requirements - this moves equally sized operands closer together,
			// in the hope that this reduces padding on the stack to enforce HW alignment requirements.			 
			locals.Sort(delegate(StackOperand op1, StackOperand op2)
			{
				int size1, size2, alignment;
				cc.GetStackRequirements(op1, out size1, out alignment);
				cc.GetStackRequirements(op2, out size2, out alignment);
				return size2 - size1;
			});
		}

		#endregion // Internals
	}
}
