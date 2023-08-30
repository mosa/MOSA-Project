// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Platform.x64.Intrinsic::Get8")]
	private static void Get8(Context context, TransformContext transformContext)
	{
		context.SetInstruction(X64.MovzxLoad8, context.Result, context.Operand1, Operand.Constant32_0);
	}
}
