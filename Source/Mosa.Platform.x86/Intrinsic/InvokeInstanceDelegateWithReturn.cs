/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.TypeSystem;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class InvokeInstanceDelegateWithReturn : IIntrinsicMethod
	{

		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		void IIntrinsicMethod.ReplaceIntrinsicCall(Context context, ITypeSystem typeSystem, IList<RuntimeParameter> parameters)
		{
			var result = context.Result;
			var op1 = context.Operand1;
			var op2 = context.Operand2;

			var eax = Operand.CreateCPURegister(BuiltInSigType.IntPtr, GeneralPurposeRegister.EAX);
			var edx = Operand.CreateCPURegister(BuiltInSigType.IntPtr, GeneralPurposeRegister.EDX);
			var esp = Operand.CreateCPURegister(BuiltInSigType.IntPtr, GeneralPurposeRegister.ESP);
			var ebp = Operand.CreateCPURegister(BuiltInSigType.IntPtr, GeneralPurposeRegister.EBP);
			context.SetInstruction(X86.Sub, esp, Operand.CreateConstant(BuiltInSigType.IntPtr, parameters.Count * 4 + 4));
			context.AppendInstruction(X86.Mov, edx, esp);

			var size = parameters.Count * 4 + 4;
			foreach (var parameter in parameters)
			{
				context.AppendInstruction(X86.Mov, Operand.CreateMemoryAddress(BuiltInSigType.IntPtr, edx.Register, new IntPtr(size - 4)), Operand.CreateMemoryAddress(BuiltInSigType.IntPtr, ebp.Register, new IntPtr(size + 4)));
				size -= 4;
			}
			context.AppendInstruction(X86.Mov, Operand.CreateMemoryAddress(BuiltInSigType.IntPtr, edx.Register, new IntPtr(size - 4)), op1);

			context.AppendInstruction(X86.Mov, eax, op2);
			context.AppendInstruction(X86.Call, null, Operand.CreateCPURegister(BuiltInSigType.IntPtr, GeneralPurposeRegister.EAX));
			context.AppendInstruction(X86.Add, esp, Operand.CreateConstant(BuiltInSigType.IntPtr, parameters.Count * 4 + 4));
			context.AppendInstruction(X86.Mov, result, Operand.CreateCPURegister(result.Type, GeneralPurposeRegister.EAX));
		}

		#endregion // Methods

	}
}
