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
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.TypeSystem;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class Set : IIntrinsicPlatformMethod
	{

		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		void IIntrinsicPlatformMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			Operand dest = context.Operand1;
			Operand value = context.Operand2;

			Operand edx = methodCompiler.CreateVirtualRegister(dest.Type);
			Operand eax = methodCompiler.CreateVirtualRegister(value.Type);
            Operand memory = Operand.CreateMemoryAddress(new SigType(context.InvokeTarget.SigParameters[1].Type), edx, 0);

			context.SetInstruction(X86.Mov, edx, dest);
			context.AppendInstruction(X86.Mov, eax, value);
			context.AppendInstruction(X86.Mov, memory, eax);
		}

		#endregion // Methods

	}
}
