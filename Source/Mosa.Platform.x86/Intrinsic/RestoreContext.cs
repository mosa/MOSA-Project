// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	///
	/// </summary>
	internal sealed class RestoreContext : IIntrinsicPlatformMethod
	{
		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		void IIntrinsicPlatformMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			// Retrieve register context
			Operand edx = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.U4, GeneralPurposeRegister.EDX);
			//Operand ebp = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.U4, GeneralPurposeRegister.EBP);
			//Operand esp = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.U4, GeneralPurposeRegister.ESP);

			// Restore registers
			context.SetInstruction(X86.Mov, Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.U4, GeneralPurposeRegister.ESP), context.Operand1);

			// Jmp to EIP (stored in EDX)
			context.AppendInstruction(X86.Jmp, null, edx);

			//context.SetOperand(0, edx);
		}

		#endregion Methods
	}
}
