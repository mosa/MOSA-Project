// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;
using Mosa.Platform.Intel;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// FrameCallRetU4
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.IIntrinsicPlatformMethod" />
	internal class FrameCallRetU4 : IIntrinsicPlatformMethod
	{
		void IIntrinsicPlatformMethod.ReplaceIntrinsicCall(Context context, MethodCompiler methodCompiler)
		{
			var result = context.Result;
			var methodAddress = context.Operand1;
			var newESP = context.Operand2;

			var eax = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EAX);

			context.SetInstruction(X86.Call, null, methodAddress);
			context.AppendInstruction(IRInstruction.Gen, eax);
			context.AppendInstruction(X86.Mov32, result, eax);
		}
	}
}
