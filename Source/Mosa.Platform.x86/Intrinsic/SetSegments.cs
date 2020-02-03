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
		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::SetSegments")]
		private static void SetSegments(Context context, MethodCompiler methodCompiler)
		{
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var operand3 = context.Operand3;
			var operand4 = context.GetOperand(3);
			var operand5 = context.GetOperand(4);

			var ds = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I2, SegmentRegister.DS);
			var es = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I2, SegmentRegister.ES);
			var fs = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I2, SegmentRegister.FS);
			var gs = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I2, SegmentRegister.GS);
			var ss = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I2, SegmentRegister.SS);

			context.SetInstruction(X86.MovStoreSeg32, ds, operand1);
			context.AppendInstruction(X86.MovStoreSeg32, es, operand2);
			context.AppendInstruction(X86.MovStoreSeg32, fs, operand3);
			context.AppendInstruction(X86.MovStoreSeg32, gs, operand4);
			context.AppendInstruction(X86.MovStoreSeg32, ss, operand5);
		}
	}
}
