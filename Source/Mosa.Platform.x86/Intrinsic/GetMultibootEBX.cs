﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Platform.x86.CompilerStages;

namespace Mosa.Platform.x86.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::GetMultibootEBX")]
	private static void GetMultibootEBX(Context context, MethodCompiler methodCompiler)
	{
		var MultibootEBX = Operand.CreateLabel(MultibootV1Stage.MultibootEBX, methodCompiler.Is32BitPlatform);

		context.SetInstruction(IRInstruction.Load32, context.Result, MultibootEBX, Operand.Constant32_0);
	}
}
