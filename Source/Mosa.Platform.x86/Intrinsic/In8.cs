// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// Representations the x86 in instruction.
	/// </summary>
	internal sealed class In8 : IIntrinsicPlatformMethod
	{
		void IIntrinsicPlatformMethod.ReplaceIntrinsicCall(Context context, MethodCompiler methodCompiler)
		{
			Operand v1 = methodCompiler.CreateVirtualRegister(methodCompiler.TypeSystem.BuiltIn.U4);

			var result = context.Result;

			context.SetInstruction(X86.In8, v1, context.Operand1);
			context.AppendInstruction(X86.Movzx8To32, result, v1);

			//context.SetInstruction(X86.In8, result, context.Operand1);
		}
	}
}
