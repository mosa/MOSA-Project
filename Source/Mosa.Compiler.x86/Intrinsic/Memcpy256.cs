// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Intrinsic;

/// <summary>
/// Intrinsic Methods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x86.Intrinsic::Memcpy256")]
	private static void Memcpy256(Context context, Transform transform)
	{
		var dest = context.Operand1;
		var src = context.Operand2;

		var v0 = transform.VirtualRegisters.Allocate32();
		var v1 = transform.VirtualRegisters.Allocate32();

		context.SetInstruction(X86.MovupsLoad, v0, src, Operand.Constant32_0);
		context.AppendInstruction(X86.MovupsLoad, v1, src, Operand.Constant32_16);
		context.AppendInstruction(X86.MovupsStore, null, dest, Operand.Constant32_0, v0);
		context.AppendInstruction(X86.MovupsStore, null, dest, Operand.Constant32_16, v1);
	}
}
