// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::Sqrtss")]
		private static void Sqrtss(Context context, MethodCompiler methodCompiler)
		{
			context.SetInstruction(X86.Sqrtss, context.Result, context.Operand1);
		}
	}
}
