// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// Representations a CmpXchgLoad32 Intrinsic
	/// </summary>
	internal sealed class CmpXChgLoad32 : IIntrinsicPlatformMethod
	{
		void IIntrinsicPlatformMethod.ReplaceIntrinsicCall(Context context, MethodCompiler methodCompiler)
		{
			var location = context.Operand1;
			var value = context.Operand2;
			var comparand = context.Operand3;
			var result = context.Result;

			var eax = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.U4, GeneralPurposeRegister.EAX);
			var v1 = methodCompiler.CreateVirtualRegister(methodCompiler.TypeSystem.BuiltIn.U4);

			context.SetInstruction(X86.Mov32, eax, value);
			context.AppendInstruction(X86.Mov32, v1, comparand);
			context.AppendInstruction(X86.Lock);
			context.AppendInstruction(X86.CmpXChgLoad32, eax, eax, location, methodCompiler.ConstantZero, v1);
			context.AppendInstruction(X86.Mov32, result, eax);
		}
	}
}
