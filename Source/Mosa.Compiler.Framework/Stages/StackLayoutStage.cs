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
using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Stages
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
			if (methodCompiler.Compiler.PlugSystem.GetPlugMethod(methodCompiler.Method) != null)
				return;

			// Layout stack variables
			LayoutStackVariables();

			// Layout parameters
			LayoutParameters(methodCompiler);

			// Create a prologue instruction
			Context prologueCtx = new Context(instructionSet, basicBlocks.PrologueBlock).InsertBefore();
			prologueCtx.SetInstruction(IRInstruction.Prologue);
			prologueCtx.Label = -1;

			// Create an epilogue instruction
			Context epilogueCtx = new Context(instructionSet, basicBlocks.EpilogueBlock);
			epilogueCtx.AppendInstruction(IRInstruction.Epilogue);
			epilogueCtx.Label = Int32.MaxValue;
		}

		#region IStackLayoutStage Members

		int IStackLayoutProvider.LocalsSize
		{
			get { return localsSize; }
		}

		#endregion // IStackLayoutStage Members

		#region Internals

		/// <summary>
		/// Layouts the stack variables.
		/// </summary>
		private void LayoutStackVariables()
		{
			List<Operand> locals = GetActiveStackVariables();

			// Sort all found locals
			OrderVariables(locals, callingConvention);

			// Now we assign increasing stack offsets to each variable
			localsSize = LayoutVariables(locals, callingConvention, callingConvention.OffsetOfFirstLocal, 1);
		}

		/// <summary>
		/// Gets the active stack variables.
		/// </summary>
		/// <returns></returns>
		private List<Operand> GetActiveStackVariables()
		{
			// Allocate a list of locals
			List<Operand> locals = new List<Operand>();

			foreach (var operand in methodCompiler.StackLayout.Stack)
				if (operand.IsLocalVariable || operand.Uses.Count != 0 || operand.Definitions.Count != 0)
					locals.Add(operand);

			return locals;
		}

		/// <summary>
		/// Lays out all parameters of the method.
		/// </summary>
		/// <param name="compiler">The method compiler providing the parameters.</param>
		private void LayoutParameters(BaseMethodCompiler compiler)
		{
			List<Operand> paramOps = new List<Operand>();

			int offset = 0;

			if (compiler.Method.Signature.HasThis || compiler.Method.Signature.HasExplicitThis)
				++offset;

			for (int i = 0; i < compiler.Method.Parameters.Count + offset; ++i)
				paramOps.Add(compiler.GetParameterOperand(i));

			LayoutVariables(paramOps, callingConvention, callingConvention.OffsetOfFirstParameter, -1);
		}

		/// <summary>
		/// Performs a stack layout of all local variables in the list.
		/// </summary>
		/// <param name="locals">The enumerable holding all locals.</param>
		/// <param name="callingConvention">The cc.</param>
		/// <param name="offsetOfFirst">Specifies the offset of the first stack operand in the list.</param>
		/// <param name="direction">The direction.</param>
		/// <returns></returns>
		private static int LayoutVariables(List<Operand> locals, ICallingConvention callingConvention, int offsetOfFirst, int direction)
		{
			int offset = offsetOfFirst;

			foreach (Operand operand in locals)
			{
				// Does the offset fit the alignment requirement?
				int alignment;
				int size;
				int padding;
				int thisOffset;

				callingConvention.GetStackRequirements(operand, out size, out alignment);
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

				operand.Offset = new IntPtr(thisOffset);
			}

			return offset;
		}

		/// <summary>
		/// Sorts all local variables by their size requirements.
		/// </summary>
		/// <param name="locals">Holds all local variables to sort..</param>
		/// <param name="callingConvention">The calling convention used to determine size and alignment requirements.</param>
		private static void OrderVariables(List<Operand> locals, ICallingConvention callingConvention)
		{
			// Sort the list by stack size requirements - this moves equally sized operands closer together,
			// in the hope that this reduces padding on the stack to enforce HW alignment requirements.			 
			locals.Sort(delegate(Operand op1, Operand op2)
			{
				int size1, size2, alignment;
				callingConvention.GetStackRequirements(op1, out size1, out alignment);
				callingConvention.GetStackRequirements(op2, out size2, out alignment);
				return size2 - size1;
			});
		}

		#endregion // Internals
	}
}
