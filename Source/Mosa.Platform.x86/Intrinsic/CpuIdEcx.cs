// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	internal static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::CpuIdECX")]
		private static void CpuIdECX(Context context, MethodCompiler methodCompiler)
		{
			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			var eax = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I4, CPURegister.EAX);
			var ebx = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I4, CPURegister.EBX);
			var ecx = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I4, CPURegister.ECX);
			var edx = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I4, CPURegister.EDX);

			context.SetInstruction(X86.Mov32, eax, operand1);
			context.AppendInstruction(X86.Mov32, ecx, operand2); context.AppendInstruction(X86.Mov32, ecx, methodCompiler.ConstantZero32);
			context.AppendInstruction(X86.CpuId, eax, eax, ecx);
			context.AppendInstruction(IRInstruction.Gen, eax, ebx, ecx, edx);
			context.AppendInstruction(X86.Mov32, result, ecx);
		}
	}
}
