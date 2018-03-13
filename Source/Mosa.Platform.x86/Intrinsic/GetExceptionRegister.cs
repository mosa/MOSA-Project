// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// GetExceptionRegister
	/// </summary>
	internal class GetExceptionRegister : IIntrinsicPlatformMethod
	{
		void IIntrinsicPlatformMethod.ReplaceIntrinsicCall(Context context, MethodCompiler methodCompiler)
		{
			context.SetInstruction(X86.Mov32, context.Result, Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.Object, methodCompiler.Architecture.ExceptionRegister));
		}
	}
}
