﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Platform.x64.Intrinsic::Get16")]
	private static void Get16(Context context, TransformContext transformContext)
	{
		context.SetInstruction(X64.MovzxLoad16, context.Result, context.Operand1, Operand.Constant32_0);
	}
}
