// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Intrinsic;

/// <summary>
/// Intrinsic Methods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x64.Intrinsic::Memclr256")]
	private static void Memclr256(Context context, Transform transform)
	{
		var dest = context.Operand1;

		var v0 = transform.PhysicalRegisters.AllocateR8(CPURegister.XMM0);
		var offset16 = Operand.Constant64_16;

		context.SetInstruction(X64.PXor, v0, v0, v0);
		context.AppendInstruction(X64.MovupsStore, dest, Operand.Constant64_0, v0);
		context.AppendInstruction(X64.MovupsStore, dest, offset16, v0);
	}
}
