// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Intrinsic;

/// <summary>
/// Intrinsic Methods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x86.Intrinsic::Sqrtss")]
	private static void Sqrtss(Context context, Transform transform)
	{
		context.SetInstruction(X86.Sqrtss, context.Result, context.Operand1);
	}
}
