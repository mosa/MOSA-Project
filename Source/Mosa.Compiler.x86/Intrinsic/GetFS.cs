// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x86.Intrinsic::GetFS")]
	private static void GetFS(Context context, TransformContext transformContext)
	{
		context.SetInstruction(X86.MovLoadSeg32, context.Result, Operand.CreateCPURegister32(CPURegister.FS));
	}
}
