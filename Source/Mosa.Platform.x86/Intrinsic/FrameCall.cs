// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	///
	/// </summary>
	internal class FrameCall : IIntrinsicPlatformMethod
	{
		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		void IIntrinsicPlatformMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			Operand methodAddress = context.Operand1;
			Operand newESP = context.Operand2;

			Operand esp = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I4, GeneralPurposeRegister.ESP);
			Operand edx = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EDX);

			context.AppendInstruction(X86.Call, null, methodAddress);
		}

		#endregion Methods
	}
}
