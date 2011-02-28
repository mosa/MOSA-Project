/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using System;

using Mosa.Runtime;
using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.CompilerFramework.Operands;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.TypeSystem;
using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// Representations a jump to the global interrupt handler.
	/// </summary>
	public sealed class JumpGlobalInterruptHandler : IIntrinsicMethod
	{
		#region Methods

		/// <summary>
		/// Replaces the instrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		public void ReplaceIntrinsicCall(Context context, ITypeSystem typeSystem)
		{
			//			context.SetInstruction(IR.Instruction.JmpInstruction, );
		}

		#endregion // Methods
	}
}
