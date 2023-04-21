// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Stages;

/// <summary>
/// Calculates the layout of the stack of the method.
/// </summary>
public sealed class StackLayoutStage : BaseMethodCompilerStage
{
	protected override void Run()
	{
		// Layout stack variables
		var size = MethodCompiler.IsStackFrameRequired ? LayoutVariables(MethodCompiler.LocalStack, Architecture.OffsetOfFirstLocal) : 0;

		MethodCompiler.StackSize = size;
		MethodData.LocalMethodStackSize = -size;
		MethodCompiler.IsLocalStackFinalized = true;

		TraceStackLocals();
	}

	#region Internals

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
	/// <param name="localStack">The enumerable holding all locals.</param>
	/// <param name="offsetOfFirst">The offset of first.</param>
	///
	/// <returns></returns>
	private int LayoutVariables(LocalStack localStack, int offsetOfFirst)
	{
		var offset = offsetOfFirst;

		foreach (var local in localStack)
		{
			if (local.Uses.Count == 0)
				continue;

			var size = MethodCompiler.GetReferenceOrTypeSize(local.Type, true);

			offset -= (int)size;

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
