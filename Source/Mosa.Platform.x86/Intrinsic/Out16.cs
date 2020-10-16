// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::Out16")]
		private static void Out16(Context context, MethodCompiler methodCompiler)
		{
			context.SetInstruction(X86.Out16, null, context.Operand1, context.Operand2);
		}
	}
}
