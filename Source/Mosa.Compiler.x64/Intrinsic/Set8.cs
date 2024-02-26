// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Intrinsic;

/// <summary>
/// Intrinsic Methods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x64.Intrinsic::Set8")]
	private static void Set8(Context context, Transform transform)
	{
		context.SetInstruction(X64.MovStore8, null, context.Operand1, Operand.Constant32_0, context.Operand2);
	}
}
