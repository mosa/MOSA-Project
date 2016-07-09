// Copyright (c) MOSA Project. Licensed under the New BSD License.

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
			if (IsPlugged)
				return;

			// Layout stack variables
			LayoutStackVariables();
		}

		#region Internals

		/// <summary>
		/// Layouts the stack variables.
		/// </summary>
		private void LayoutStackVariables()
		{
			// assign increasing stack offsets to each variable
			int size = LayoutVariables(MethodCompiler.LocalStack, CallingConvention, CallingConvention.OffsetOfFirstLocal, true);

			MethodCompiler.StackSize = size;
			MethodCompiler.TypeLayout.SetMethodStackSize(MethodCompiler.Method, -size);

			TraceStackLocals();
		}

		private void TraceStackLocals()
		{
			var trace = CreateTraceLog("Stack Local");

			if (!trace.Active)
				return;

			foreach (var local in MethodCompiler.LocalStack)
			{
				trace.Log(local.ToString() + ": displacement = " + local.Offset.ToString());
			}
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
				if (!operand.IsParameter && operand.Uses.Count == 0 && operand.Definitions.Count == 0)
				{
					bool skip = false;

					if (operand.Low == null && operand.High == null)
					{
						skip = true;
					}
					else if (operand.Low.Uses.Count == 0 && operand.Low.Definitions.Count == 0 && operand.High.Uses.Count == 0 && operand.High.Definitions.Count == 0)
					{
						skip = true;
					}

					if (skip)
					{
						operand.Offset = 0;
						continue;
					}
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
					operand.Low.Offset = offset + (operand.Low.Offset - operand.Offset);
					operand.High.Offset = offset + (operand.High.Offset - operand.Offset);
				}

				operand.Offset = offset;

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
