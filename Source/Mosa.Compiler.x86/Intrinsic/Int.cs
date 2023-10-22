// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x86.Intrinsic::Int")]
	private static void Int(Context context, Transform transform)
	{
		Helper.FoldOperand1ToConstant(context);

		context.SetInstruction(X86.Int, context.Result, context.Operand1);
	}
}
