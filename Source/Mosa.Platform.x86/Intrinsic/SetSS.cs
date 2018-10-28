// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// SetSS
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.IIntrinsicPlatformMethod" />
	internal class SetSS : IIntrinsicPlatformMethod
	{
		void IIntrinsicPlatformMethod.ReplaceIntrinsicCall(Context context, MethodCompiler methodCompiler)
		{
			context.SetInstruction(X86.MovStoreSeg32, Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.U4, SegmentRegister.SS), context.Operand1);
		}
	}
}
