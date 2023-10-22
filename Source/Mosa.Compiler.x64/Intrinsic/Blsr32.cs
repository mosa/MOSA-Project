// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x64.Intrinsic::Blsr32")]
	private static void Blsr32(Context context, Transform transform)
	{
		context.SetInstruction(X64.Blsr32, context.Result, context.Operand1);
	}
}
