// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Intrinsics;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Runtime.Intrinsic::GetObjectFromAddress")]
	private static void GetObjectFromAddress(Context context, MethodCompiler methodCompiler)
	{
		context.SetInstruction(IRInstruction.MoveObject, context.Result, context.Operand1);
	}
}
