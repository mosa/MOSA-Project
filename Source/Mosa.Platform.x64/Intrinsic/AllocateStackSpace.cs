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
		[IntrinsicMethod("Mosa.Platform.x64.Intrinsic::AllocateStackSpace")]
		private static void AllocateStackSpace(Context context, MethodCompiler methodCompiler)
		{
			Operand result = context.Result;
			Operand size = context.Operand1;

			Operand esp = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I8, GeneralPurposeRegister.ESP);

			context.SetInstruction(X64.Sub64, esp, esp, size);
			context.AppendInstruction(X64.Mov64, result, esp);
		}
	}
}
