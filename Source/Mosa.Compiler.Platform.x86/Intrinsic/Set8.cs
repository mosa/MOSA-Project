// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Platform.x86.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	internal static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Compiler.Platform.x86.Intrinsic::Set8")]
		private static void Set8(Context context, MethodCompiler methodCompiler)
		{
			context.SetInstruction(X86.MovStore8, null, context.Operand1, methodCompiler.ConstantZero32, context.Operand2);
		}
	}
}
