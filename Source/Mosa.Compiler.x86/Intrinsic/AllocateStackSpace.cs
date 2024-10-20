﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Intrinsic;

/// <summary>
/// Intrinsic Methods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x86.Intrinsic::AllocateStackSpace")]
	private static void AllocateStackSpace(Context context, Transform transform)
	{
		var result = context.Result;
		var size = context.Operand1;

		var esp = transform.PhysicalRegisters.Allocate32(CPURegister.ESP);

		context.SetInstruction(X86.Sub32, esp, esp, size);
		context.AppendInstruction(X86.Mov32, result, esp);
	}
}
