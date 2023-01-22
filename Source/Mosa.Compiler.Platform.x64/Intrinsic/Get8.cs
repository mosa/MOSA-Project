// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Platform.x64.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	internal static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Compiler.Platform.x64.Intrinsic::Get8")]
		private static void Get8(Context context, MethodCompiler methodCompiler)
		{
			context.SetInstruction(X64.MovzxLoad8, context.Result, context.Operand1, methodCompiler.ConstantZero32);
		}
	}
}
