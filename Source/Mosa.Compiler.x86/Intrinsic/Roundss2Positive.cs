﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x86.Intrinsic::Roundss2Positive")]
	private static void Roundss2Positive(Context context, Transform transform)
	{
		context.SetInstruction(X86.Roundss, context.Result, context.Operand1, Operand.Constant32_2);
	}
}
