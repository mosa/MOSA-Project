﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	internal static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Platform.x64.Intrinsic::Memcpy256")]
		private static void Memcpy256(Context context, MethodCompiler methodCompiler)
		{
			var dest = context.Operand1;
			var src = context.Operand2;

			var v0 = methodCompiler.CreateVirtualRegister(methodCompiler.TypeSystem.BuiltIn.Void);
			var v1 = methodCompiler.CreateVirtualRegister(methodCompiler.TypeSystem.BuiltIn.Void);
			var offset16 = methodCompiler.CreateConstant(16);

			context.SetInstruction(X64.MovupsLoad, v0, dest, methodCompiler.ConstantZero64);
			context.AppendInstruction(X64.MovupsLoad, v1, dest, offset16);
			context.AppendInstruction(X64.MovupsStore, null, dest, methodCompiler.ConstantZero64, v0);
			context.AppendInstruction(X64.MovupsStore, null, dest, offset16, v1);
		}
	}
}
