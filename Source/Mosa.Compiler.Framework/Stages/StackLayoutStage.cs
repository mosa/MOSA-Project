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
	public sealed class StackLayoutStage : BaseMethodCompilerStage
	{
		protected override void Run()
		{
			if (MethodCompiler.Compiler.PlugSystem.GetPlugMethod(MethodCompiler.Method) != null)
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
			MethodCompiler.StackLayout.StackSize = LayoutVariables(MethodCompiler.StackLayout.Stack, CallingConvention, CallingConvention.OffsetOfFirstLocal, 1);
		}

		/// <summary>
		/// Lays out all parameters of the method.
		/// </summary>
		private void LayoutParameters()
		{
			List<Operand> parameters = new List<Operand>();

			int offset = 0;

			if (MethodCompiler.Method.HasThis || MethodCompiler.Method.HasExplicitThis)
				++offset;

			for (int i = 0; i < MethodCompiler.Method.Signature.Parameters.Count + offset; ++i)
			{
				var parameter = MethodCompiler.GetParameterOperand(i);

				parameters.Add(parameter);
			}

			LayoutVariables(parameters, CallingConvention, CallingConvention.OffsetOfFirstParameter, -1);
		}

		/// <summary>
		/// Performs a stack layout of all local variables in the list.
		/// </summary>
		/// <param name="locals">The enumerable holding all locals.</param>
		/// <param name="callingConvention">The cc.</param>
		/// <param name="offsetOfFirst">Specifies the offset of the first stack operand in the list.</param>
		/// <param name="direction">The direction.</param>
		/// <returns></returns>
		private int LayoutVariables(IList<Operand> locals, BaseCallingConvention callingConvention, int offsetOfFirst, int direction)
		{
			int offset = offsetOfFirst;

			foreach (var operand in locals)
			{
				if (!operand.IsParameter && operand.IsStackLocal && operand.Uses.Count == 0 && operand.Definitions.Count == 0)
				{
					operand.Displacement = 0;
				}
				else
				{
					// Does the offset fit the alignment requirement?
					int alignment = 0;
					int size = 0;
					int padding = 0;
					int thisOffset = 0;

					callingConvention.GetStackRequirements(TypeLayout, operand, out size, out alignment);
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

					long existing = operand.Displacement;
					operand.Displacement = thisOffset;

					// adjust split children
					if (operand.Low != null)
					{
						operand.Low.Displacement = thisOffset + (operand.Low.Displacement - existing);
					}

					if (operand.High != null)
					{
						operand.High.Displacement = thisOffset + (operand.High.Displacement - existing);
					}
				}
			}

			return offset;
		}

		#endregion Internals
	}
}