// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x64.Intrinsic::Int")]
	private static void Int(Context context, TransformContext transformContext)
	{
		Helper.FoldOperand1ToConstant(context);

		context.SetInstruction(X64.Int, context.Result, context.Operand1);
	}
}
