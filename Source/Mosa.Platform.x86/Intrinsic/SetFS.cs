﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::SetFS")]
	private static void SetFS(Context context, MethodCompiler methodCompiler)
	{
		context.SetInstruction(X86.MovStoreSeg32, Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.U4, CPURegister.FS), context.Operand1);
	}
}
