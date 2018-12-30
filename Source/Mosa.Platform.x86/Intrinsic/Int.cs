// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// Representations the x86 int instruction.
	/// </summary>
	internal sealed class Int : IIntrinsicPlatformMethod
	{
		void IIntrinsicMethod.ReplaceIntrinsicCall(Context context, MethodCompiler methodCompiler)
		{
			Helper.FoldOperand1ToConstant(context);

			context.SetInstruction(X86.Int, context.Result, context.Operand1);
		}
	}
}
