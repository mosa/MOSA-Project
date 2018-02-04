// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// Frame Call Ret U4
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.IIntrinsicPlatformMethod" />
	internal class FrameCallRetU4 : IIntrinsicPlatformMethod
	{
		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="methodCompiler">The method compiler.</param>
		void IIntrinsicPlatformMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			var result = context.Result;
			var methodAddress = context.Operand1;
			var newESP = context.Operand2;

			var eax = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EAX);

			context.SetInstruction(X86.CallReg, null, methodAddress);
			context.AppendInstruction(IRInstruction.Gen, eax);
			context.AppendInstruction(X86.Mov, result, eax);
		}

		#endregion Methods
	}
}
