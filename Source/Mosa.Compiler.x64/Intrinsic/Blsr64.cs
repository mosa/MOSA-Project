// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Intrinsic;

/// <summary>
/// Intrinsic Methods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x64.Intrinsic::Blsr64")]
	private static void Blsr64(Context context, Transform transform)
	{
		context.SetInstruction(X64.Blsr64, context.Result, context.Operand1);
	}
}
