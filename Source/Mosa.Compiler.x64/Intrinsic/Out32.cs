// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x64.Intrinsic::Out32")]
	private static void Out32(Context context, Transform transform)
	{
		context.SetInstruction(X64.Out32, null, context.Operand1, context.Operand2);
	}
}
