// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Intrinsic;

/// <summary>
/// Intrinsic Methods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x86.Intrinsic::SetEAX")]
	private static void SetEAX(Context context, Transform transform)
	{
		context.SetInstruction(X86.Mov32, transform.PhysicalRegisters.Allocate32(CPURegister.EAX), context.Operand1);
	}
}
