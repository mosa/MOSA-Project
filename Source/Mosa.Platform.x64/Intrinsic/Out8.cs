// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Platform.x64.Intrinsic::Out8")]
	private static void Out8(Context context, TransformContext transformContext)
	{
		context.SetInstruction(X64.Out8, null, context.Operand1, context.Operand2);
	}
}
