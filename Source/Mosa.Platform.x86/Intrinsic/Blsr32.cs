// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::Blsr32")]
	private static void Blsr32(Context context, MethodCompiler methodCompiler)
	{
		context.SetInstruction(X86.Blsr32, context.Result, context.Operand1);
	}
}
