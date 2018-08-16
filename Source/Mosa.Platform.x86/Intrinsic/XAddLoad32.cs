// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// Representations a XchgLoad32 Intrinsic
	/// </summary>
	internal sealed class XAddLoad32 : IIntrinsicPlatformMethod
	{
		void IIntrinsicPlatformMethod.ReplaceIntrinsicCall(Context context, MethodCompiler methodCompiler)
		{
			var location = context.Operand1;
			var value = context.Operand2;
			var result = context.Result;

			var v1 = methodCompiler.CreateVirtualRegister(methodCompiler.TypeSystem.BuiltIn.U4);

			context.SetInstruction(X86.Mov32, v1, value);
			context.AppendInstruction(X86.Lock);
			context.AppendInstruction(X86.XAddLoad32, v1, location, methodCompiler.ConstantZero, v1);
			context.AppendInstruction(X86.Mov32, result, v1);
		}
	}
}
