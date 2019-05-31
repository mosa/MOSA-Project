// Copyright (c) MOSA Project. Licensed under the New BSD License.

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
			if (!MethodCompiler.IsStackFrameRequired)
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
			MethodData.LocalMethodStackSize = -size;

			TraceStackLocals();
		}

		private void TraceStackLocals()
		{
			var trace = CreateTraceLog("Stack Local");

			if (trace == null)
				return;

			foreach (var local in MethodCompiler.LocalStack)
			{
				if (local.Uses.Count == 0)
					continue;

				trace.Log($"{local}: offset = {local.Offset}");
			}
		}

		/// <summary>
		/// Performs a stack layout of all local variables in the list.
		/// </summary>
		/// <param name="locals">The enumerable holding all locals.</param>
		/// <param name="offsetOfFirst">The offset of first.</param>
		///
		/// <returns></returns>
		private int LayoutVariables(List<Operand> locals, int offsetOfFirst)
		{
			int offset = offsetOfFirst;

			foreach (var local in locals)
			{
				if (local.Uses.Count == 0)
					continue;

				var size = GetTypeSize(local.Type, true);

				offset -= size;

				local.Offset = offset;
				local.IsResolved = true;

				if (local.Low != null)
				{
					local.Low.Offset = offset;
					local.Low.IsResolved = true;
				}
				if (local.High != null)
				{
					local.High.Offset = offset + 4;
					local.High.IsResolved = true;
				}
			}

			return offset;
		}

		#endregion Internals
	}
}
