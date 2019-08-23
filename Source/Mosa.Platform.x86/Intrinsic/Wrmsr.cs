// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic:WrMSR")]
		private static void WrMSR(Context context, MethodCompiler methodCompiler)
		{
			context.SetInstruction(X86.WrMSR, context.Result, context.Operand1, context.Operand2);
		}
	}
}
