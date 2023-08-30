// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Platform.x64.Intrinsic::Roundss2Negative")]
	private static void Roundss2Negative(Context context, TransformContext transformContext)
	{
		context.SetInstruction(X64.Roundss, context.Result, context.Operand1, Operand.Constant64_1);
	}
}
