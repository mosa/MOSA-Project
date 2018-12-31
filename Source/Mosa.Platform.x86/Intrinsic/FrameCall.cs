// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic:FrameCall")]
		private static void FrameCall(Context context, MethodCompiler methodCompiler)
		{
			var methodAddress = context.Operand1;
			var newESP = context.Operand2;

			context.SetInstruction(X86.Call, null, methodAddress);
		}
	}
}
