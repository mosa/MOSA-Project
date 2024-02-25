// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Intrinsic;

/// <summary>
/// Intrinsic Methods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.ARM32.Intrinsic::Mcr")]
	private static void Mcr(Context context, Transform transform)
	{
		context.SetInstruction(ARM32.Mcr, context.Result, context.Operand1, context.Operand2, context.Operand3, context.Operand4, context.Operand5);
	}
}
