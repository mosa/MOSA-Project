// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::Lidt")]
		private static void Lidt(Context context, MethodCompiler methodCompiler)
		{
			//Helper.FoldOperand1ToConstant(context);

			context.SetInstruction(X86.Lidt, null, context.Operand1);
		}
	}
}
