// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;


namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::FrameCallRetU8")]
		private static void FrameCallRetU8(Context context, MethodCompiler methodCompiler)
		{
			var result = context.Result;
			var methodAddress = context.Operand1;

			var eax = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I4, CPURegister.EAX);
			var edx = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I4, CPURegister.EDX);

			methodCompiler.SplitLongOperand(result, out Operand op0L, out Operand op0H);

			context.SetInstruction(X86.Call, null, methodAddress);
			context.AppendInstruction(IRInstruction.Gen, eax);
			context.AppendInstruction(IRInstruction.Gen, edx);
			context.AppendInstruction(X86.Mov32, op0L, eax);
			context.AppendInstruction(X86.Mov32, op0H, edx);
		}
	}
}
