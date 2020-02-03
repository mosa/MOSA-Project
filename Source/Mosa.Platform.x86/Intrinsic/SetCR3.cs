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
		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::SetCR3")]
		private static void SetCR3(Context context, MethodCompiler methodCompiler)
		{
			Operand operand1 = context.Operand1;

			Operand eax = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.U4, GeneralPurposeRegister.EAX);
			Operand cr = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.U4, ControlRegister.CR3);

			context.SetInstruction(X86.Mov32, eax, operand1);
			context.AppendInstruction(X86.MovCRStore32, null, cr, eax);
		}
	}
}
