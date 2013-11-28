/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	///
	/// </summary>
	public sealed class SwitchTask : IIntrinsicPlatformMethod
	{
		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		void IIntrinsicPlatformMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			Operand esp = Operand.CreateCPURegister(BuiltInSigType.Int32, GeneralPurposeRegister.ESP);

			context.SetInstruction(X86.Mov, esp, context.Operand1);
			context.AppendInstruction(X86.Popad);
			context.AppendInstruction(X86.Add, esp, esp, Operand.CreateConstantSignedInt(0x08));
			context.AppendInstruction(X86.Sti);
			context.AppendInstruction(X86.IRetd);
		}

		#endregion Methods
	}
}