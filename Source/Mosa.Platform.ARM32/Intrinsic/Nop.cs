// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Platform.ARM32.Intrinsic::Nop")]
	private static void Nop(Context context, TransformContext transformContext)
	{
		context.SetInstruction(ARM32.Nop);
	}
}
