// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::Roundsd2Positive")]
		private static void Roundsd2Positive(Context context, MethodCompiler methodCompiler)
		{
			context.SetInstruction(X86.Roundsd, context.Result, context.Operand1, methodCompiler.CreateConstant((byte)2));
		}
	}
}
