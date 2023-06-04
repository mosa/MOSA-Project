﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::Out8")]
	private static void Out8(Context context, TransformContext transformContext)
	{
		context.SetInstruction(X86.Out8, null, context.Operand1, context.Operand2);
	}
}
