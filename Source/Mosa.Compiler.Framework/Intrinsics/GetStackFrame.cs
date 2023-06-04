// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Intrinsics;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Runtime.Intrinsic::GetStackFrame")]
	private static void GetStackFrame(Context context, TransformContext transformContext)
	{
		context.SetInstruction(transformContext.MoveInstruction, context.Result, transformContext.Compiler.StackFrame);
	}
}
