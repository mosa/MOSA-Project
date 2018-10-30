// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// Representations the x86 Lidt instruction.
	/// </summary>
	internal sealed class Lidt : IIntrinsicPlatformMethod
	{
		void IIntrinsicPlatformMethod.ReplaceIntrinsicCall(Context context, MethodCompiler methodCompiler)
		{
			//Helper.FoldOperand1ToConstant(context);

			context.SetInstruction(X86.Lidt, null, context.Operand1);
		}
	}
}
