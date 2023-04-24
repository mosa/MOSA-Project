// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Platform.x64.Intrinsic::Roundsd2Positive")]
	private static void Roundsd2Positive(Context context, MethodCompiler methodCompiler)
	{
		context.SetInstruction(X64.Roundsd, context.Result, context.Operand1, Operand.Constant64_2);
	}
}
