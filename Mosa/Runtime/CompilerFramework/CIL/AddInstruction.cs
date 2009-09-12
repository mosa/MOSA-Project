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
	public class AddInstruction : ArithmeticInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="AddInstruction"/> class.
		/// </summary>
		public AddInstruction() : base(OpCode.Add)
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
			Debug.Assert(OpCode.Add == opcode, @"Wrong opcode for NopInstruction.");
			if (OpCode.Add != opcode)
				throw new ArgumentException(@"Wrong opcode.", @"code");
		}

		#endregion // ICILInstruction Overrides

		#region Operand Overrides

		/// <summary>
		/// Returns a string representation of <see cref="ConstantOperand"/>.
		/// </summary>
		/// <returns>A string representation of the operand.</returns>
		public override string ToString()
		{
			return "CIL add";
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString(ref InstructionData instruction)
		{
			return String.Format("{0} ; {1} = {2} + {3}", base.ToString(), instruction.Result, instruction.Operand1, instruction.Operand2);
		}

		#endregion // Operand Overrides
	}
}
