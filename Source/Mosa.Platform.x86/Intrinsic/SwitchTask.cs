// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	///
	/// </summary>
	internal sealed class SwitchTask : IIntrinsicPlatformMethod
	{
		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		void IIntrinsicPlatformMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			Operand esp = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I4, GeneralPurposeRegister.ESP);

			context.SetInstruction(X86.Mov, esp, context.Operand1);
			context.AppendInstruction(X86.Popad);
			context.AppendInstruction(X86.Add, esp, esp, Operand.CreateConstant(methodCompiler.TypeSystem, 0x08));
			context.AppendInstruction(X86.Sti);
			context.AppendInstruction(X86.IRetd);
		}

		#endregion Methods
	}
}
