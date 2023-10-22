// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x86.Intrinsic::Set8")]
	private static void Set8(Context context, Transform transform)
	{
		context.SetInstruction(X86.MovStore8, null, context.Operand1, Operand.Constant32_0, context.Operand2);
	}
}
