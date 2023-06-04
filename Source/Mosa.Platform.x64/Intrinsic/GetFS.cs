﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Platform.x64.Intrinsic::GetFS")]
	private static void GetFS(Context context, TransformContext transformContext)
	{
		context.SetInstruction(X64.MovLoadSeg64, context.Result, Operand.CreateCPURegister64(CPURegister.FS));
	}
}
