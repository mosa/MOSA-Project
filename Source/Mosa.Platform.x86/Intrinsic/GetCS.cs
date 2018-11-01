// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Platform.Intel;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// GetCS
	/// </summary>
	public class GetCS : IIntrinsicPlatformMethod
	{
		void IIntrinsicPlatformMethod.ReplaceIntrinsicCall(Context context, MethodCompiler methodCompiler)
		{
			context.SetInstruction(X86.MovLoadSeg32, context.Result, Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.U4, SegmentRegister.CS));
		}
	}
}
