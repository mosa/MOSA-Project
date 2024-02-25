// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Intrinsics;

/// <summary>
/// Intrinsic Methods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Runtime.Intrinsic::GetExceptionRegister")]
	private static void GetExceptionRegister(Context context, Transform transform)
	{
		context.SetInstruction(transform.MoveInstruction, context.Result, transform.ExceptionRegister);
	}
}
