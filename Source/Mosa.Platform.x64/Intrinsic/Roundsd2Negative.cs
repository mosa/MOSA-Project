// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Platform.x64.Intrinsic::Roundsd2Negative")]
	private static void Roundsd2Negative(Context context, MethodCompiler methodCompiler)
	{
		context.SetInstruction(X64.Roundsd, context.Result, context.Operand1, methodCompiler.CreateConstant((byte)1));
	}
}
