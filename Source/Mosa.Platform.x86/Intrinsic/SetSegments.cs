// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Platform.Intel;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic:SetSegments")]
		private static void SetSegments(Context context, MethodCompiler methodCompiler)
		{
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var operand3 = context.Operand3;
			var operand4 = context.GetOperand(3);
			var operand5 = context.GetOperand(4);

			var EAX = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EAX);

			var ds = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I2, SegmentRegister.DS);
			var es = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I2, SegmentRegister.ES);
			var fs = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I2, SegmentRegister.FS);
			var gs = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I2, SegmentRegister.GS);
			var ss = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I2, SegmentRegister.SS);

			context.SetInstruction(X86.Mov32, EAX, operand1);
			context.AppendInstruction(X86.MovStoreSeg32, ds, EAX);
			context.AppendInstruction(X86.Mov32, EAX, operand2);
			context.AppendInstruction(X86.MovStoreSeg32, es, EAX);
			context.AppendInstruction(X86.Mov32, EAX, operand3);
			context.AppendInstruction(X86.MovStoreSeg32, fs, EAX);
			context.AppendInstruction(X86.Mov32, EAX, operand4);
			context.AppendInstruction(X86.MovStoreSeg32, gs, EAX);
			context.AppendInstruction(X86.Mov32, EAX, operand5);
			context.AppendInstruction(X86.MovStoreSeg32, ss, EAX);
		}
	}
}
