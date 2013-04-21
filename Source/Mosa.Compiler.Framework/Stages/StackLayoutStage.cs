/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Calculates the layout of the stack of the method.
	/// </summary>
	public sealed class StackLayoutStage : BaseMethodCompilerStage, IMethodCompilerStage, IPipelineStage
	{
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
		}

		#region Internals

		/// <summary>
		/// Layouts the stack variables.
		/// </summary>
		private void LayoutStackVariables()
		{
			// assign increasing stack offsets to each variable
			methodCompiler.StackLayout.StackSize = LayoutVariables(methodCompiler.StackLayout.Stack, callingConvention, callingConvention.OffsetOfFirstLocal, 1);
		}

		/// <summary>
		/// Lays out all parameters of the method.
		/// </summary>
		/// <param name="compiler">The method compiler providing the parameters.</param>
		private void LayoutParameters(BaseMethodCompiler compiler)
		{
			List<Operand> paramOps = new List<Operand>();

			int offset = 0;

			if (compiler.Method.HasThis || compiler.Method.HasExplicitThis)
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
		private static int LayoutVariables(IList<Operand> locals, ICallingConvention callingConvention, int offsetOfFirst, int direction)
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

				operand.Offset = thisOffset;
			}

			return offset;
		}

		#endregion Internals
	}
}