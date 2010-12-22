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
using System.Text;
using System.Diagnostics;
using IR = Mosa.Runtime.CompilerFramework.IR;

using Mosa.Runtime.CompilerFramework;


namespace Mosa.Platform.x86.CPUx86
{
	/// <summary>
	/// Representations the x86 pushfd instruction.
	/// </summary>
	public sealed class PushfdInstruction : BaseInstruction
	{

		#region Methods

		/// <summary>
		/// 
		/// </summary>
		/// <param name="destination"></param>
		/// <param name="source"></param>
		/// <param name="third"></param>
		/// <returns></returns>
		protected override OpCode ComputeOpCode(Runtime.CompilerFramework.Operands.Operand destination, Runtime.CompilerFramework.Operands.Operand source, Runtime.CompilerFramework.Operands.Operand third)
		{
			return new OpCode(new byte[] { 0x9C });
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Pushfd(context);
		}

		#endregion // Methods
	}
}