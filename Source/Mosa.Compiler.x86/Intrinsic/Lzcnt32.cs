// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Intrinsic;

/// <summary>
/// Intrinsic Methods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x86.Intrinsic::Lzcnt32")]
	private static void Lzcnt32(Context context, Transform transform)
	{
		context.SetInstruction(X86.Lzcnt32, context.Result, context.Operand1);
	}
}
