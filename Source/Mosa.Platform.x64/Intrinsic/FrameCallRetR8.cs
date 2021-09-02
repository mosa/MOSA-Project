// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Platform.Intel;

namespace Mosa.Platform.x64.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Platform.x64.Intrinsic::FrameCallRetR8")]
		private static void FrameCallRetR8(Context context, MethodCompiler methodCompiler)
		{
			var result = context.Result;
			var methodAddress = context.Operand1;

			var eax = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I8, GeneralPurposeRegister.EAX);
			var edx = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I8, GeneralPurposeRegister.EDX);
			var xmm0 = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I8, SSE2Register.XMM0);

			methodCompiler.SplitLongOperand(result, out Operand op0L, out Operand op0H);

			context.SetInstruction(X64.Call, null, methodAddress);
			context.AppendInstruction(IRInstruction.Gen, xmm0);

			//context.AppendInstruction(X64.Movdi64ss, eax, xmm0);        // FIXME: X64
			context.AppendInstruction(X64.Pextrd64, edx, xmm0, methodCompiler.CreateConstant((byte)1));

			context.AppendInstruction(X64.Mov64, op0L, eax);
			context.AppendInstruction(X64.Mov64, op0H, edx);
		}
	}
}
