// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Platform.x64.Intrinsic::Blsr32")]
	private static void Blsr32(Context context, TransformContext transformContext)
	{
		context.SetInstruction(X64.Blsr32, context.Result, context.Operand1);
	}
}
