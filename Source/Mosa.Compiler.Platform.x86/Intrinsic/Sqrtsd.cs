﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Platform.x86.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Compiler.Platform.x86.Intrinsic::Sqrtsd")]
		private static void Sqrtsd(Context context, MethodCompiler methodCompiler)
		{
			context.SetInstruction(X86.Sqrtsd, context.Result, context.Operand1);
		}
	}
}
