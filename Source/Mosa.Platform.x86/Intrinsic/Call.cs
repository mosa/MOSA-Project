// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::Call")]
		private static void Call(Context context, MethodCompiler methodCompiler)
		{
			context.SetInstruction(X86.Call, null, context.Operand1);
		}
	}
}
