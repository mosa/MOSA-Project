// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x64.Intrinsic::Roundsd2Negative")]
	private static void Roundsd2Negative(Context context, Transform transform)
	{
		context.SetInstruction(X64.Roundsd, context.Result, context.Operand1, Operand.Constant64_1);
	}
}
