// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::FrameCallRetU4")]
	private static void FrameCallRetU4(Context context, TransformContext transformContext)
	{
		var result = context.Result;
		var methodAddress = context.Operand1;

		var eax = Operand.CreateCPURegister32(CPURegister.EAX);

		context.SetInstruction(X86.Call, null, methodAddress);
		context.AppendInstruction(IRInstruction.Gen, eax);
		context.AppendInstruction(X86.Mov32, result, eax);
	}
}
