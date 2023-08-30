// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::FrameCall")]
	private static void FrameCall(Context context, TransformContext transformContext)
	{
		context.SetInstruction(X86.Call, null, context.Operand1);
	}
}
