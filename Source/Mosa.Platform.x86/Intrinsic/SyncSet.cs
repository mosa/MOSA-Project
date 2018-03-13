// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Platform.x86.Stages;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// LockSet
	/// </summary>
	internal sealed class SyncSet : IIntrinsicPlatformMethod
	{
		void IIntrinsicPlatformMethod.ReplaceIntrinsicCall(Context context, MethodCompiler methodCompiler)
		{
			context.SetInstruction(X86.MovStore32, null, context.Operand1, methodCompiler.ConstantZero, context.Operand2);
		}
	}
}
