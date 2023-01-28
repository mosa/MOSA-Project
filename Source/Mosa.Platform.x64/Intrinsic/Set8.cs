// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Platform.x64.Intrinsic::Set8")]
	private static void Set8(Context context, MethodCompiler methodCompiler)
	{
		context.SetInstruction(X64.MovStore8, null, context.Operand1, methodCompiler.Constant32_0, context.Operand2);
	}
}
