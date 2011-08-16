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

using Mosa.Runtime;
using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.TypeSystem;
using Mosa.Runtime.CompilerFramework.Operands;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// Representations the x86 cli instruction.
	/// </summary>
	public sealed class InvokeDelegateWithReturn : IIntrinsicMethod
	{

		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		public void ReplaceIntrinsicCall(Context context, ITypeSystem typeSystem)
		{
			context.SetInstruction(CPUx86.Instruction.CallPointerInstruction, context.Result, context.Operand2);
			context.AppendInstruction(CPUx86.Instruction.MovInstruction, context.Result, new RegisterOperand(context.Result.Type, GeneralPurposeRegister.EAX));
		}

		#endregion // Methods

	}
}
