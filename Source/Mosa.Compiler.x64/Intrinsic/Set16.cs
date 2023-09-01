// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x64.Intrinsic::Set16")]
	private static void Set16(Context context, TransformContext transformContext)
	{
		context.SetInstruction(X64.MovStore16, null, context.Operand1, Operand.Constant32_0, context.Operand2);
	}
}
