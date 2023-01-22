// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Platform.x64.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	internal static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Compiler.Platform.x64.Intrinsic::Set16")]
		private static void Set16(Context context, MethodCompiler methodCompiler)
		{
			context.SetInstruction(X64.MovStore16, null, context.Operand1, methodCompiler.ConstantZero32, context.Operand2);
		}
	}
}
