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
			Operand const0 = Operand.CreateConstantSignedInt(methodCompiler.TypeSystem, 0x0);
			Operand const1 = Operand.CreateConstantSignedInt(methodCompiler.TypeSystem, 0x1);
			Operand pointer = Operand.CreateMemoryAddress(context.Operand2.Type, context.Operand2, 0);
			Operand eax = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.U4, x86.GeneralPurposeRegister.EAX);
			Operand register = methodCompiler.CreateVirtualRegister(methodCompiler.TypeSystem.BuiltIn.U4);
			Operand @return = context.Result;

			// Test to acquire lock, if we do acquire the lock then set the lock and return true, otherwise return false
			context.SetInstruction(X86.Mov, register, const1);
			context.AppendInstruction(X86.Mov, @return, const0);
			context.AppendInstruction(X86.Mov, eax, const0);
			context.AppendInstruction(X86.Lock);
			context.AppendInstruction(X86.CmpXchg, pointer, pointer, register, eax);
			context.AppendInstruction(X86.Setcc, ConditionCode.Equal, @return);
		}

		#endregion Methods
	}
}