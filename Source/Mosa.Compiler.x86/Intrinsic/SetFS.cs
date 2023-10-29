// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x86.Intrinsic::SetFS")]
	private static void SetFS(Context context, Transform transform)
	{
		context.SetInstruction(X86.MovStoreSeg32, transform.PhysicalRegisters.Allocate32(CPURegister.FS), context.Operand1);
	}
}
