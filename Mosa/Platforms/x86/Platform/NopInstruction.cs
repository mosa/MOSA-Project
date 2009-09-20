/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Mosa.Runtime.CompilerFramework;

namespace Mosa.Platforms.x86.Platform
{
	/// <summary>
	/// 
	/// </summary>
	public class NopInstruction : IPlatformInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="NopInstruction"/> class.
		/// </summary>
		public NopInstruction()
		{
		}

		#endregion // Construction

		#region IPlatformInstruction Overrides

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="codeStream">The code stream.</param>
		public void Emit(ref InstructionData instruction, System.IO.Stream codeStream)
		{
			codeStream.WriteByte(0x90);
		}

		#endregion // IPlatformInstruction Overrides

		#region Operand Overrides

		/// <summary>
		/// Returns a string representation of <see cref="ConstantOperand"/>.
		/// </summary>
		/// <returns>A string representation of the operand.</returns>
		public override string ToString()
		{
			return "CIL nop";
		}

		#endregion // Operand Overrides

		#region HACK // FIXME PG

		// Needs to derive from base class with implements these default methods:

		/// <summary>
		/// Gets the flow control.
		/// </summary>
		/// <value>The flow control.</value>
		public FlowControl FlowControl { get { return FlowControl.Next; } }


		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="vistor">The vistor.</param>
		/// <param name="context">The context.</param>
		public void Visit(IVisitor vistor, Context context)
		{
		}

		#endregion
	}
}
