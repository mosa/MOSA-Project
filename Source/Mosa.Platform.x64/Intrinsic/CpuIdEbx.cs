﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Platform.Intel;

namespace Mosa.Platform.x64.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	internal static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Platform.x64.Intrinsic::CpuIdEbx")]
		private static void CpuIdEbx(Context context, MethodCompiler methodCompiler)
		{
			var result = context.Result;
			var operand = context.Operand1;
			var eax = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I4, CPURegister.R1);
			var ecx = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I4, CPURegister.R1);
			var reg = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I4, CPURegister.R3);

			context.SetInstruction(X64.Mov64, eax, operand);
			context.AppendInstruction(X64.Mov64, ecx, methodCompiler.ConstantZero32);
			context.AppendInstruction(X64.CpuId, eax, eax);
			context.AppendInstruction(X64.Mov64, result, reg);
		}
	}
}
