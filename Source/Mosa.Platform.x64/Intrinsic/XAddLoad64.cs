﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	internal static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Platform.x64.Intrinsic::XAddLoad64")]
		private static void XAddLoad64(Context context, MethodCompiler methodCompiler)
		{
			var location = context.Operand1;
			var value = context.Operand2;
			var result = context.Result;

			var v1 = methodCompiler.CreateVirtualRegister(methodCompiler.TypeSystem.BuiltIn.U8);

			context.SetInstruction(X64.Mov64, v1, value);
			context.AppendInstruction(X64.Lock);
			context.AppendInstruction(X64.XAddLoad32, v1, location, methodCompiler.ConstantZero32, v1);
			context.AppendInstruction(X64.Mov64, result, v1);
		}
	}
}
