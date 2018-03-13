// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// FrameCallRetR8
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.IIntrinsicPlatformMethod" />
	internal class FrameCallRetR8 : IIntrinsicPlatformMethod
	{
		void IIntrinsicPlatformMethod.ReplaceIntrinsicCall(Context context, MethodCompiler methodCompiler)
		{
			var result = context.Result;
			var methodAddress = context.Operand1;
			var newESP = context.Operand2;

			var eax = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EAX);
			var edx = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EDX);
			var mmx1 = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I4, SSE2Register.XMM0);

			methodCompiler.SplitLongOperand(result, out Operand op0L, out Operand op0H);

			context.SetInstruction(X86.CallReg, null, methodAddress);
			context.AppendInstruction(IRInstruction.Gen, mmx1);

			context.AppendInstruction(X86.Movd, eax, mmx1);
			context.AppendInstruction(X86.Pextrd, edx, mmx1, Operand.CreateConstant(methodCompiler.TypeSystem.BuiltIn.U1, 1));

			context.AppendInstruction(X86.Mov32, op0L, eax);
			context.AppendInstruction(X86.Mov32, op0H, edx);
		}
	}
}
