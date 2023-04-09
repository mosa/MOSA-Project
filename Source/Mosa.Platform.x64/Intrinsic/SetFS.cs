// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Platform.x64.Intrinsic::SetFS")]
	private static void SetFS(Context context, MethodCompiler methodCompiler)
	{
		context.SetInstruction(X64.MovStoreSeg64, Operand.CreateCPURegister64(CPURegister.FS), context.Operand1);
	}
}
