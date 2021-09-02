﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Platform.Intel;

namespace Mosa.Platform.x64.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Platform.x64.Intrinsic::SetCR2")]
		private static void SetCR2(Context context, MethodCompiler methodCompiler)
		{
			Operand operand1 = context.Operand1;

			Operand eax = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.U4, GeneralPurposeRegister.EAX);
			Operand cr = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.U4, ControlRegister.CR2);

			context.SetInstruction(X64.Mov64, eax, operand1);
			context.AppendInstruction(X64.MovCRStore64, null, cr, eax);
		}
	}
}
