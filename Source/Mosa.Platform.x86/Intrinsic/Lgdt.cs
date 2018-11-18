// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Platform.Intel;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// Representations the x86 Lgdt instruction.
	/// </summary>
	internal sealed class Lgdt : IIntrinsicPlatformMethod
	{
		void IIntrinsicPlatformMethod.ReplaceIntrinsicCall(Context context, MethodCompiler methodCompiler)
		{
			//Helper.FoldOperand1ToConstant(context);

			var constantx10 = methodCompiler.CreateConstant(0x10);

			Operand eax = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EAX);
			Operand ds = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I2, SegmentRegister.DS);
			Operand es = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I2, SegmentRegister.ES);
			Operand fs = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I2, SegmentRegister.FS);
			Operand gs = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I2, SegmentRegister.GS);
			Operand ss = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I2, SegmentRegister.SS);

			context.SetInstruction(X86.Lgdt, null, context.Operand1);
			context.AppendInstruction(X86.Mov32, eax, constantx10);
			context.AppendInstruction(X86.MovStoreSeg32, ds, eax);
			context.AppendInstruction(X86.MovStoreSeg32, es, eax);
			context.AppendInstruction(X86.MovStoreSeg32, fs, eax);
			context.AppendInstruction(X86.MovStoreSeg32, gs, eax);
			context.AppendInstruction(X86.MovStoreSeg32, ss, eax);
			context.AppendInstruction(X86.JmpFar);
		}
	}
}
