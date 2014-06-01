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
	/// Representations a SyncCompareAndSwap Intrinsic
	/// </summary>
	public sealed class SyncCompareAndSwap : IIntrinsicPlatformMethod
	{
		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		void IIntrinsicPlatformMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			var pointer = context.Operand1;
			var oldval = context.Operand2;
			var newval = context.Operand3;
			var result = context.Result;
			
			Operand eax = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.U4, x86.GeneralPurposeRegister.EAX);
			Operand v1 = methodCompiler.CreateVirtualRegister(methodCompiler.TypeSystem.BuiltIn.U4);

			context.SetInstruction(X86.Mov, eax, oldval);
			context.SetInstruction(X86.Mov, v1, newval);
			context.AppendInstruction(X86.Lock);
			context.AppendInstruction(X86.CmpXchg, pointer, pointer, v1, eax);
			context.AppendInstruction(X86.Setcc, ConditionCode.Equal, result);
		}

		#endregion Methods
	}
}