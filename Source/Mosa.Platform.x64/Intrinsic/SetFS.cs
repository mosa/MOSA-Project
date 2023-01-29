// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;


namespace Mosa.Platform.x64.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Platform.x64.Intrinsic::SetFS")]
	private static void SetFS(Context context, MethodCompiler methodCompiler)
	{
		context.SetInstruction(X64.MovStoreSeg64, Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.U8, CPURegister.FS), context.Operand1);
	}
}
