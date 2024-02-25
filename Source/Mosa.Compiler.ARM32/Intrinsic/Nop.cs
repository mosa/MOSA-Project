// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Intrinsic;

/// <summary>
/// Intrinsic Methods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.ARM32.Intrinsic::Nop")]
	private static void Nop(Context context, Transform transform)
	{
		context.SetInstruction(ARM32.Nop);
	}
}
