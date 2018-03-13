// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// Representations the x86 BochsDebug instruction.
	/// </summary>
	internal sealed class BochsDebug : IIntrinsicPlatformMethod
	{
		void IIntrinsicPlatformMethod.ReplaceIntrinsicCall(Context context, MethodCompiler methodCompiler)
		{
			// xchg	bx, bx
			var bx = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.U2, GeneralPurposeRegister.EBX);
			context.SetInstruction2(X86.Xchg32, bx, bx, bx, bx);
		}
	}
}
