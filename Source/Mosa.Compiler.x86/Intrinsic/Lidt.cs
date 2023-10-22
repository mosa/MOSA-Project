// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x86.Intrinsic::Lidt")]
	private static void Lidt(Context context, Transform transform)
	{
		//Helper.FoldOperand1ToConstant(context);

		context.SetInstruction(X86.Lidt, null, context.Operand1);
	}
}
