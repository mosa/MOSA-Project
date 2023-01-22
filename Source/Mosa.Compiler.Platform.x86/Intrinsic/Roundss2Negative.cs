// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Platform.x86.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Compiler.Platform.x86.Intrinsic::Roundss2Negative")]
		private static void Roundss2Negative(Context context, MethodCompiler methodCompiler)
		{
			context.SetInstruction(X86.Roundss, context.Result, context.Operand1, methodCompiler.CreateConstant((byte)1));
		}
	}
}
