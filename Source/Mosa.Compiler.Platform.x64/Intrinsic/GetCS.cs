﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Platform.x64.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Compiler.Platform.x64.Intrinsic::GetCS")]
		private static void GetCS(Context context, MethodCompiler methodCompiler)
		{
			context.SetInstruction(X64.MovLoadSeg64, context.Result, Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.U8, CPURegister.CS));
		}
	}
}
