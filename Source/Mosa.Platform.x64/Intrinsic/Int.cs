// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Platform.x64.Intrinsic::Int")]
		private static void Int(Context context, MethodCompiler methodCompiler)
		{
			Helper.FoldOperand1ToConstant(context);

			context.SetInstruction(X64.Int, context.Result, context.Operand1);
		}
	}
}
