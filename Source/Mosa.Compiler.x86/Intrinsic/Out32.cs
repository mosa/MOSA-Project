// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Intrinsic;

/// <summary>
/// Intrinsic Methods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x86.Intrinsic::Out32")]
	private static void Out32(Context context, Transform transform)
	{
		context.SetInstruction(X86.Out32, null, context.Operand1, context.Operand2);
	}
}
