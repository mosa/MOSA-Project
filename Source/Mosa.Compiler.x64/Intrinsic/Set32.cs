// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x64.Intrinsic::Set32")]
	private static void Set32(Context context, TransformContext transformContext)
	{
		context.SetInstruction(X64.MovStore32, null, context.Operand1, Operand.Constant32_0, context.Operand2);
	}
}
