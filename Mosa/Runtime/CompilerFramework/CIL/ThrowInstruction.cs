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

namespace Mosa.Runtime.CompilerFramework.CIL
{
	/// <summary>
	/// 
	/// </summary>
	public class ThrowInstruction : CILInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="ThrowInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public ThrowInstruction(OpCode opcode)
			: base(opcode)
		{
		}

		#endregion // Construction

		#region ICILInstruction Overrides

		/// <summary>
		/// Decodes the specified instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="opcode">The opcode of the load.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		public override void Decode(ref InstructionData instruction, OpCode opcode, IInstructionDecoder decoder)
		{
			base.Decode(ref instruction, opcode, decoder);
		}

		#endregion // ICILInstruction Overrides

	}
}
