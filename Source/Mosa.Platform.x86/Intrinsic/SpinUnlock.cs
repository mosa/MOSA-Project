/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// Representations a spin unlock
	/// </summary>
	public sealed class SpinUnlock : IIntrinsicPlatformMethod
	{
		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		void IIntrinsicPlatformMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			// Create constant operand and pointer to variable containing the lock
			Operand const0 = Operand.CreateConstantSignedInt(methodCompiler.TypeSystem, 0x0);
			Operand locked = Operand.CreateMemoryAddress(methodCompiler.TypeSystem.BuiltIn.Pointer, context.Operand1, 0);

			// Set the variable locked to 0 signifying that the lock is free
			context.SetInstruction(X86.Mov, locked, const0);
		}

		#endregion Methods
	}
}