// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x86.Intrinsic::Memclr256")]
	private static void Memclr256(Context context, Transform transform)
	{
		var dest = context.Operand1;

		var v0 = Operand.CreateCPURegisterNativeInteger(CPURegister.XMM0, transform.Is32BitPlatform);
		var offset16 = Operand.Constant32_16;

		context.SetInstruction(X86.PXor, v0, v0, v0);
		context.AppendInstruction(X86.MovupsStore, dest, Operand.Constant32_0, v0);
		context.AppendInstruction(X86.MovupsStore, dest, offset16, v0);
	}
}
