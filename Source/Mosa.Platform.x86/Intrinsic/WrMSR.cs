// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::WrMSR")]
		private static void WrMSR(Context context, MethodCompiler methodCompiler)
		{
			methodCompiler.SplitLongOperand(context.Operand2, out Operand resultLow, out Operand resultHigh);

			context.SetInstruction(X86.WrMSR, null, context.Operand1, resultLow, resultHigh);
		}
	}
}
