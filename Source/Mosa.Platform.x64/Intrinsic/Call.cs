// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Platform.x64.Intrinsic::Call")]
	private static void Call(Context context, MethodCompiler methodCompiler)
	{
		context.SetInstruction(X64.Call, null, context.Operand1);
	}
}
