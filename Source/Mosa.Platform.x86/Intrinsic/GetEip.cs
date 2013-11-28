/*
 * (c) 2010 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <rootnode@mosa-project.org>
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	///
	/// </summary>
	public sealed class GetEIP : IIntrinsicPlatformMethod
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
			Operand eax = methodCompiler.CreateVirtualRegister(BuiltInSigType.UInt32);

			context.AppendInstruction(X86.Add, eax, eax, Operand.CreateCPURegister(BuiltInSigType.UInt32, GeneralPurposeRegister.ESP));
			context.AppendInstruction(X86.Mov, eax, Operand.CreateMemoryAddress(BuiltInSigType.UInt32, eax, 0));
			context.AppendInstruction(X86.Mov, result, eax);
		}

		#endregion Methods
	}
}