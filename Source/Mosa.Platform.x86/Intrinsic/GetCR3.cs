﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::GetCR3")]
	private static void GetCR3(Context context, MethodCompiler methodCompiler)
	{
		context.SetInstruction(X86.MovCRLoad32, context.Result, Operand.CreateCPURegister32(CPURegister.CR3));
	}
}
