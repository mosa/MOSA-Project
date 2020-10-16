// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Platform.Intel;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	internal static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::CmpXChgLoad32")]
		private static void CmpXChgLoad32(Context context, MethodCompiler methodCompiler)
		{
			var location = context.Operand1;
			var value = context.Operand2;
			var comparand = context.Operand3;
			var result = context.Result;

			var eax = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.U4, GeneralPurposeRegister.EAX);
			var v1 = methodCompiler.CreateVirtualRegister(methodCompiler.TypeSystem.BuiltIn.U4);

			context.SetInstruction(X86.Mov32, eax, comparand);
			context.AppendInstruction(X86.Mov32, v1, value);
			context.AppendInstruction(X86.Lock);
			context.AppendInstruction(X86.CmpXChgLoad32, eax, eax, location, methodCompiler.ConstantZero32, v1);
			context.AppendInstruction(X86.Mov32, result, eax);
		}
	}
}
