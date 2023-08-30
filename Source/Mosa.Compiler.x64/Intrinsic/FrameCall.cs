// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x64.Intrinsic::FrameCall")]
	private static void FrameCall(Context context, TransformContext transformContext)
	{
		context.SetInstruction(X64.Call, null, context.Operand1);
	}
}
