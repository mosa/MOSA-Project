// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Platform.x64.Intrinsic::Get64")]
	private static void Get64(Context context, MethodCompiler methodCompiler)
	{
		context.SetInstruction(X64.MovLoad64, context.Result, context.Operand1, methodCompiler.Constant32_0);
	}
}
