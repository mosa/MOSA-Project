/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using System;
using System.Collections.Generic;
//using System.Diagnostics;

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Operands;
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.TypeSystem;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class Set : IIntrinsicMethod
	{

		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		void IIntrinsicMethod.ReplaceIntrinsicCall(Context context, ITypeSystem typeSystem, IList<RuntimeParameter> parameters)
		{
			Operand dest = context.Operand1;
			Operand value = context.Operand2;

			RegisterOperand edx = new RegisterOperand(dest.Type, GeneralPurposeRegister.EDX);
			RegisterOperand eax = new RegisterOperand(value.Type, GeneralPurposeRegister.EAX);
			MemoryOperand memory = new MemoryOperand(new SigType(context.InvokeTarget.Signature.Parameters[1].Type), GeneralPurposeRegister.EDX, new IntPtr(0));

			context.SetInstruction(X86.Mov, edx, dest);
			context.AppendInstruction(X86.Mov, eax, value);
			context.AppendInstruction(X86.Mov, memory, eax);
		}

		#endregion // Methods

	}
}
