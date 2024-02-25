// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Intrinsic;

/// <summary>
/// Intrinsic Methods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x64.Intrinsic::Get16")]
	private static void Get16(Context context, Transform transform)
	{
		context.SetInstruction(X64.MovzxLoad16, context.Result, context.Operand1, Operand.Constant32_0);
	}
}
