﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Intrinsics;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Runtime.Intrinsic::GetAssemblyListTable")]
	private static void GetAssemblyListTable(Context context, MethodCompiler methodCompiler)
	{
		var move = methodCompiler.Is32BitPlatform ? (BaseInstruction)IRInstruction.Move32 : IRInstruction.Move64;

		context.SetInstruction(move, context.Result, Operand.CreateLabel(Metadata.AssembliesTable, methodCompiler.Is32BitPlatform));
	}
}
