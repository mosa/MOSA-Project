/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Common;
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
			MethodCompiler.StackLayout.StackSize = LayoutVariables(MethodCompiler.StackLayout.Stack, CallingConvention, CallingConvention.OffsetOfFirstLocal, true);
		}

		/// <summary>
		/// Lays out all parameters of the method.
		/// </summary>
		private void LayoutParameters()
		{
			var parameters = new List<Operand>();

			int offset = 0;

			if (MethodCompiler.Method.HasThis || MethodCompiler.Method.HasExplicitThis)
				++offset;

			for (int i = 0; i < MethodCompiler.Method.Signature.Parameters.Count + offset; ++i)
			{
				var parameter = MethodCompiler.GetParameterOperand(i);

				parameters.Add(parameter);
			}

			LayoutVariables(parameters, CallingConvention, CallingConvention.OffsetOfFirstParameter, false);
		}

		/// <summary>
		/// Performs a stack layout of all local variables in the list.
		/// </summary>
		/// <param name="locals">The enumerable holding all locals.</param>
		/// <param name="callingConvention">The cc.</param>
		/// <param name="offsetOfFirst">Specifies the offset of the first stack operand in the list.</param>
		/// <param name="isLocalVariable">The direction.</param>
		/// <returns></returns>
		private int LayoutVariables(IList<Operand> locals, BaseCallingConvention callingConvention, int offsetOfFirst, bool isLocalVariable)
		{
			int offset = offsetOfFirst;

			foreach (var operand in locals)
			{
				bool skip = false;

				if (!operand.IsParameter && operand.Uses.Count == 0 && operand.Definitions.Count == 0)
				{
					if (operand.Low == null && operand.High == null)
					{
						skip = true;
					}
					else if (operand.Low.Uses.Count == 0 && operand.Low.Definitions.Count == 0 && operand.High.Uses.Count == 0 && operand.High.Definitions.Count == 0)
					{
						skip = true;
					}
				}

				if (skip)
				{
					operand.Displacement = 0;
					continue;
				}

				int size, alignment;
				Architecture.GetTypeRequirements(TypeLayout, operand.Type, out size, out alignment);
				if (isLocalVariable)
				{
					size = Alignment.AlignUp(size, alignment);
					offset = offset - size;
				}

				// adjust split children
				if (operand.Low != null)
				{
					operand.Low.Displacement = offset + (operand.Low.Displacement - operand.Displacement);
					operand.High.Displacement = offset + (operand.High.Displacement - operand.Displacement);
				}

				operand.Displacement = offset;

				if (!isLocalVariable)
				{
					size = Alignment.AlignUp(size, alignment);
					offset = offset + size;
				}
			}

			return offset;
		}

		#endregion Internals
	}
}