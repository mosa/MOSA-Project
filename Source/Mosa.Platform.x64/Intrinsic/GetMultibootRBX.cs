﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Platform;

namespace Mosa.Platform.x64.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Platform.x64.Intrinsic::GetMultibootRBX")]
	private static void GetMultibootRBX(Context context, MethodCompiler methodCompiler)
	{
		var MultibootEBX = Operand.CreateLabel(BaseMultibootStage.MultibootEBX, methodCompiler.Is32BitPlatform);

		context.SetInstruction(IRInstruction.Load64, context.Result, MultibootEBX, Operand.Constant32_0);
	}
}
