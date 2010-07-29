/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;

using Mosa.Runtime;
using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.CompilerFramework.Operands;

namespace Mosa.Platforms.x86.Intrinsic
{
	/// <summary>
	/// This instruction is used to test intrinsic calls.
	/// </summary>
	public sealed class Test : IIntrinsicMethod
	{
	
		#region Methods

		/// <summary>
		/// Replaces the instrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		public void ReplaceIntrinsicCall(Context context, RuntimeBase runtime)
		{
			context.SetInstruction(CPUx86.Instruction.MovInstruction, context.Result, context.Operand1);
		}

		#endregion
	}
}
