// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x86.Intrinsic::FrameCallRetU8")]
	private static void FrameCallRetU8(Context context, Transform transform)
	{
		var result = context.Result;
		var methodAddress = context.Operand1;

		var eax = transform.PhysicalRegisters.Allocate32(CPURegister.EAX);
		var edx = transform.PhysicalRegisters.Allocate32(CPURegister.EDX);

		context.SetInstruction(X86.Call, null, methodAddress);
		context.AppendInstruction(IRInstruction.Gen, eax);
		context.AppendInstruction(IRInstruction.Gen, edx);
		context.AppendInstruction(IRInstruction.To64, result, eax, edx);
	}
}
