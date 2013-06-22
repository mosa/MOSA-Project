/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
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
			LayoutParameters();
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
		private void LayoutParameters()
		{
			List<Operand> parameters = new List<Operand>();

			int offset = 0;

			if (methodCompiler.Method.HasThis || methodCompiler.Method.HasExplicitThis)
				++offset;

			for (int i = 0; i < methodCompiler.Method.Parameters.Count + offset; ++i)
			{
				parameters.Add(methodCompiler.GetParameterOperand(i));
			}

			LayoutVariables(parameters, callingConvention, callingConvention.OffsetOfFirstParameter, -1);
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

				long existing = operand.Offset;
				operand.Offset = thisOffset;

				// adjust split children
				if (operand.Low != null)
				{
					operand.Low.Offset = thisOffset + (operand.Low.Offset - existing);
				}

				if (operand.High != null)
				{
					operand.High.Offset = thisOffset + (operand.High.Offset - existing);
				}
			}

			return offset;
		}

		#endregion Internals
	}
}