/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// Representations the x86 Lidt instruction.
	/// </summary>
	public sealed class Lidt : IIntrinsicPlatformMethod
	{
		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		void IIntrinsicPlatformMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			//context.SetInstruction(X86.Mov, Operand.CreateCPURegister(BuiltInSigType.Ptr, GeneralPurposeRegister.EAX), context.Operand1);
			//context.AppendInstruction(X86.Lidt, null, Operand.CreateMemoryAddress(BuiltInSigType.Ptr, GeneralPurposeRegister.EAX, 0));

			context.SetInstruction(X86.Lidt, null, Operand.CreateMemoryAddress(BuiltInSigType.Ptr, context.Operand1, 0));

		}

		#endregion Methods
	}
}