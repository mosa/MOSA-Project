// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x86.Intrinsic::Out32")]
	private static void Out32(Context context, TransformContext transformContext)
	{
		context.SetInstruction(X86.Out32, null, context.Operand1, context.Operand2);
	}
}
