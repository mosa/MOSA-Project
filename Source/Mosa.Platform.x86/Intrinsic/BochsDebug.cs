/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// Representations the x86 BochsDebug instruction.
	/// </summary>
	public sealed class BochsDebug : IIntrinsicPlatformMethod
	{
		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		void IIntrinsicPlatformMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			// xchg	bx, bx
			var bx = Operand.CreateCPURegister(BuiltInSigType.UInt16, GeneralPurposeRegister.EBX);
			context.SetInstruction2(X86.Xchg, bx, bx, bx, bx);
		}

		#endregion Methods
	}
}