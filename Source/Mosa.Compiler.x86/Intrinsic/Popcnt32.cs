// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x86.Intrinsic::Popcnt32")]
	private static void Popcnt32(Context context, TransformContext transformContext)
	{
		context.SetInstruction(X86.Popcnt32, context.Result, context.Operand1);
	}
}
