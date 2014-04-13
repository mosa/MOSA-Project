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
			Operand const1 = Operand.CreateConstantSignedInt(methodCompiler.TypeSystem, 0x1);
			Operand pointer = Operand.CreateMemoryAddress(context.Operand2.Type, context.Operand2, 0);

			// Test to acquire lock, if cant acquire jump to top, if we do then continue normal execution
			context.SetInstruction(X86.Test, null, pointer, const1);
			context.AppendInstruction(X86.Branch, ConditionCode.NotZero, context.BasicBlock);
			context.AppendInstruction(X86.Mov, pointer, const1);
		}

		#endregion Methods
	}
}