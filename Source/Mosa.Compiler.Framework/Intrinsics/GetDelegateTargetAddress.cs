// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Intrinsics;

/// <summary>
/// Intrinsic Methods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Runtime.Intrinsic::GetDelegateTargetAddress")]
	private static void GetDelegateTargetAddress(Context context, Transform transform)
	{
		context.SetInstruction(transform.LoadInstruction, context.Result, context.Operand1, Operand.CreateConstant32(transform.Architecture.NativePointerSize));
	}
}
