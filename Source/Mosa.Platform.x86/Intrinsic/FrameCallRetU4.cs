// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	///
	/// </summary>
	internal class FrameCallRetU4 : IIntrinsicPlatformMethod
	{
		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		void IIntrinsicPlatformMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			Operand result = context.Result;
			Operand methodAddress = context.Operand1;
			Operand newESP = context.Operand2;

			Operand esp = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I4, GeneralPurposeRegister.ESP);
			Operand eax = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EAX);
			Operand edx = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EDX);

			context.AppendInstruction(X86.Call, null, methodAddress);
			context.AppendInstruction(IRInstruction.Gen, eax);
			context.AppendInstruction(X86.Mov, result, eax);
		}

		#endregion Methods
	}
}
