// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Intrinsics;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Runtime.Intrinsic::GetDelegateMethodAddress")]
	private static void GetDelegateMethodAddress(Context context, Transform transform)
	{
		context.SetInstruction(transform.LoadInstruction, context.Result, context.Operand1, transform.ConstantZero);
	}
}
