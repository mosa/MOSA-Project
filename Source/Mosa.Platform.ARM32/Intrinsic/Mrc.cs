// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Platform.ARM32.Intrinsic::Mrc")]
	private static void Mrc(Context context, TransformContext transformContext)
	{
		context.SetInstruction(ARM32.Mcr, context.Result, context.Operand1, context.Operand2, context.Operand3, context.Operand4, context.Operand5);
	}
}
