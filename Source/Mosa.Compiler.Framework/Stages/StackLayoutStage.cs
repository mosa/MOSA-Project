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
			int size = LayoutVariables(MethodCompiler.LocalStack, Architecture.OffsetOfFirstLocal);

			MethodCompiler.StackSize = size;
			MethodCompiler.MethodData.LocalMethodStackSize = -size;

			TraceStackLocals();
		}

		private void TraceStackLocals()
		{
			var trace = CreateTraceLog("Stack Local");

			if (!trace.Active)
				return;

			foreach (var local in MethodCompiler.LocalStack)
			{
				trace.Log(local + ": offset = " + local.Offset.ToString());
			}
		}

		/// <summary>
		/// Performs a stack layout of all local variables in the list.
		/// </summary>
		/// <param name="locals">The enumerable holding all locals.</param>
		/// <param name="offsetOfFirst">The offset of first.</param>
		///
		/// <returns></returns>
		private int LayoutVariables(IList<Operand> locals, int offsetOfFirst)
		{
			int offset = offsetOfFirst;

			foreach (var operand in locals)
			{
				var size = GetTypeSize(operand.Type, true);

				offset -= size;

				operand.Offset = offset;
				operand.IsResolved = true;
			}

			return offset;
		}

		#endregion Internals
	}
}
