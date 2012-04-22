/*
 * (c) 2010 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <rootnode@mosa-project.org>
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
	public sealed class GetEIP : IIntrinsicMethod
	{
		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		void IIntrinsicMethod.ReplaceIntrinsicCall(Context context, ITypeSystem typeSystem, IList<RuntimeParameter> parameters)
		{
			Operand result = context.Result;
			SigType u4 = BuiltInSigType.UInt32;
			RegisterOperand eax = new RegisterOperand(u4, GeneralPurposeRegister.EAX);

			context.SetInstruction(X86.Pop, eax);
			context.AppendInstruction(X86.Add, eax, new RegisterOperand(u4, GeneralPurposeRegister.ESP));
			context.AppendInstruction(X86.Mov, eax, new MemoryOperand(u4, GeneralPurposeRegister.EAX, new IntPtr(0)));
			context.AppendInstruction(X86.Mov, result, eax);
			context.AppendInstruction(X86.Push, null, eax);
		}

		#endregion // Methods
	}
}
