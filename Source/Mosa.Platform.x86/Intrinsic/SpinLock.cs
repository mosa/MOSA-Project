/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
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
			// Create operands
			Operand pointer = Operand.CreateMemoryAddress(context.Operand2.Type, context.Operand2, 0);
			Operand notused = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.U4, x86.GeneralPurposeRegister.EAX);
			Operand register = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.U4, x86.GeneralPurposeRegister.EBX);//methodCompiler.CreateVirtualRegister(methodCompiler.TypeSystem.BuiltIn.U4);
			Operand @return = context.Result;

			// Test to acquire lock, if we do acquire the lock then set the lock and return true, otherwise return false
			context.SetInstruction(X86.Xor, notused, notused, notused);
			context.AppendInstruction(X86.Xor, register, register, register);
			context.AppendInstruction(X86.Inc, register);
			context.AppendInstruction(X86.Lock);
			context.AppendInstruction(X86.CmpXchg, null, pointer, register);
			context.AppendInstruction(X86.Setcc, ConditionCode.Equal, @return);
		}

		#endregion Methods
	}
}