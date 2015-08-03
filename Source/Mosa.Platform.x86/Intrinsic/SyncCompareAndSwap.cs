// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// Representations a SyncCompareAndSwap Intrinsic
	/// </summary>
	internal sealed class SyncCompareAndSwap : IIntrinsicPlatformMethod
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

			var eax = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.U4, x86.GeneralPurposeRegister.EAX);
			var v1 = methodCompiler.CreateVirtualRegister(methodCompiler.TypeSystem.BuiltIn.U4);
			var mem1 = Operand.CreateMemoryAddress(pointer.Type, pointer, 0);

			context.SetInstruction(X86.Mov, v1, newval);
			context.AppendInstruction(X86.Mov, eax, oldval);
			context.AppendInstruction(X86.Lock);
			context.AppendInstruction(X86.CmpXchg, mem1, mem1, v1, eax);
			context.AppendInstruction(X86.Setcc, ConditionCode.Equal, result);
		}

		#endregion Methods
	}
}