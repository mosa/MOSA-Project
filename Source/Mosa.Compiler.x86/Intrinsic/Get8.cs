// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Intrinsic;

/// <summary>
/// Intrinsic Methods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x86.Intrinsic::Get8")]
	private static void Get8(Context context, Transform transform)
	{
		context.SetInstruction(X86.MovzxLoad8, context.Result, context.Operand1, Operand.Constant32_0);
	}
}
