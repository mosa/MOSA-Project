// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Platform.x64.Intrinsic::GetCR4")]
	private static void GetCR4(Context context, MethodCompiler methodCompiler)
	{
		context.SetInstruction(X64.MovCRLoad64, context.Result, Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.U8, CPURegister.CR4));
	}
}
