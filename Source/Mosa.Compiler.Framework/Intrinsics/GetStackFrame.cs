// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Intrinsics;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Runtime.Intrinsic::GetStackFrame")]
	private static void GetStackFrame(Context context, Transform transform)
	{
		context.SetInstruction(transform.MoveInstruction, context.Result, transform.Compiler.StackFrame);
	}
}
