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
			Operand operand1 = context.Operand1;
			Operand operand2 = context.Operand2;

			Operand esp = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I4, GeneralPurposeRegister.ESP);

			context.SetInstruction(X86.Push, null, esp);
			context.AppendInstruction(X86.Mov, esp, operand2);
			context.AppendInstruction(X86.Call, null, operand1);
			context.AppendInstruction(X86.Pop, esp);
		}

		#endregion Methods
	}
}
