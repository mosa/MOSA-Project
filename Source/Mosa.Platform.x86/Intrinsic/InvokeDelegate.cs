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
using Mosa.Compiler.Framework.Operands;
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.TypeSystem;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class InvokeDelegate : IIntrinsicMethod
	{

		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		void IIntrinsicMethod.ReplaceIntrinsicCall(Context context, ITypeSystem typeSystem, IList<RuntimeParameter> parameters)
		{
			//var result = context.Result;
			//var op1 = context.Operand1;
			var op2 = context.Operand2;

			var eax = new RegisterOperand(BuiltInSigType.IntPtr, GeneralPurposeRegister.EAX);
			var edx = new RegisterOperand(BuiltInSigType.IntPtr, GeneralPurposeRegister.EDX);
			var esp = new RegisterOperand(BuiltInSigType.IntPtr, GeneralPurposeRegister.ESP);
			var ebp = new RegisterOperand(BuiltInSigType.IntPtr, GeneralPurposeRegister.EBP);
			context.SetInstruction(X86.Sub, esp, new ConstantOperand(BuiltInSigType.IntPtr, parameters.Count * 4));
			context.AppendInstruction(X86.Mov, edx, esp);

			var size = parameters.Count * 4;
			foreach (var parameter in parameters)
			{
				context.AppendInstruction(X86.Mov, new MemoryOperand(BuiltInSigType.IntPtr, edx.Register, new IntPtr(size - 4)), new MemoryOperand(BuiltInSigType.IntPtr, ebp.Register, new IntPtr(size + 8)));
				size -= 4;
			}

			context.AppendInstruction(X86.Mov, eax, op2);
			context.AppendInstruction(X86.Call, null, new RegisterOperand(BuiltInSigType.IntPtr, GeneralPurposeRegister.EAX));
			context.AppendInstruction(X86.Add, esp, new ConstantOperand(BuiltInSigType.IntPtr, parameters.Count * 4));
		}

		#endregion // Methods

	}
}
