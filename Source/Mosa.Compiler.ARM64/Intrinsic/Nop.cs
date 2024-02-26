// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM64.Intrinsic;

/// <summary>
/// Intrinsic Methods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.ARM64.Intrinsic::Nop")]
	private static void Nop(Context context, Transform transform)
	{
		context.SetInstruction(ARM64.Nop); // TODO
	}
}
