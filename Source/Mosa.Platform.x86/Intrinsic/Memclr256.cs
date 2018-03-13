// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// Memclr256
	/// </summary>
	internal sealed class Memclr256 : IIntrinsicPlatformMethod
	{
		void IIntrinsicPlatformMethod.ReplaceIntrinsicCall(Context context, MethodCompiler methodCompiler)
		{
			var dest = context.Operand1;

			var v0 = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.Void, SSE2Register.XMM0);
			var zero = Operand.CreateConstant(0, methodCompiler.TypeSystem);
			var offset16 = Operand.CreateConstant(16, methodCompiler.TypeSystem);

			context.SetInstruction(X86.PXor, v0, v0, v0);
			context.AppendInstruction(X86.MovupsStore, dest, zero, v0);
			context.AppendInstruction(X86.MovupsStore, dest, offset16, v0);
		}
	}
}
