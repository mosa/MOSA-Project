/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// Representations a spin lock
	/// </summary>
	public sealed class SpinLock : IIntrinsicPlatformMethod
	{
		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		void IIntrinsicPlatformMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			// Create constant operand and get pointer to lock variable
			Operand const0 = Operand.CreateConstantSignedInt(methodCompiler.TypeSystem, 0x0);
			Operand const1 = Operand.CreateConstantSignedInt(methodCompiler.TypeSystem, 0x1);
			Operand pointer = Operand.CreateMemoryAddress(context.Operand2.Type, context.Operand2, 0);
			Operand @return = context.Result;

			// Test to acquire lock, if we do acquire the lock then set the lock and return true, otherwise return false
			context.SetInstruction(X86.Mov, @return, const0);
			context.AppendInstruction(X86.Test, null, pointer, const1);
			context.AppendInstruction(X86.Cmov, ConditionCode.Zero, pointer, const1);
			context.AppendInstruction(X86.Cmov, ConditionCode.Zero, @return, const1);
		}

		#endregion Methods
	}
}