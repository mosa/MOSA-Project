// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Intrinsic;

/// <summary>
/// Intrinsic Methods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x64.Intrinsic::Sqrtsd")]
	private static void Sqrtsd(Context context, Transform transform)
	{
		context.SetInstruction(X64.Sqrtsd, context.Result, context.Operand1);
	}
}
