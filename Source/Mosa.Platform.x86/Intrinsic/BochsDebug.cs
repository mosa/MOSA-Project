// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::BochsDebug")]
	private static void BochsDebug(Context context, TransformContext transformContext)
	{
		context.SetInstruction(X86.BochsDebug);
	}
}
