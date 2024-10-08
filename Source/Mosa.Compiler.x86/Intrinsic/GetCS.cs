﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Intrinsic;

/// <summary>
/// Intrinsic Methods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x86.Intrinsic::GetCS")]
	private static void GetCS(Context context, Transform transform)
	{
		context.SetInstruction(X86.MovLoadSeg32, context.Result, transform.PhysicalRegisters.Allocate32(CPURegister.CS));
	}
}
