﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Platform.Intel;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::SetCR0")]
		private static void SetCR0(Context context, MethodCompiler methodCompiler)
		{
			Operand operand1 = context.Operand1;

			Operand eax = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.U4, CPURegister.EAX);
			Operand cr = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.U4, CPURegister.CR0);

			context.SetInstruction(X86.Mov32, eax, operand1);
			context.AppendInstruction(X86.MovCRStore32, null, cr, eax);
		}
	}
}
