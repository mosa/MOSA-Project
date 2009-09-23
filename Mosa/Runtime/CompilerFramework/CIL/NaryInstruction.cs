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
	public class NaryInstruction : BaseInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="NaryInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		/// <param name="operandCount">The operand count.</param>
		public NaryInstruction(OpCode opcode, byte operandCount)
			: base(opcode, operandCount)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NaryInstruction"/> class.
		/// </summary>
		/// <param name="code">The code.</param>
		/// <param name="operandCount">The operand count.</param>
		/// <param name="resultCount">The result count.</param>
		protected NaryInstruction(OpCode code, byte operandCount, byte resultCount)
			: base(code, operandCount, resultCount)
		{
		}

		#endregion // Construction

		#region Methods

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString(ref InstructionData instruction)
		{
			StringBuilder builder = new StringBuilder();
			builder.AppendFormat("{0}", _opcode);

			if (instruction.OperandCount != 0) {
				builder.Append(' ');

				if (instruction.OperandCount == 1)
					builder.AppendFormat("{0}, ", instruction.Operand1);

				if (instruction.OperandCount == 2)
					builder.AppendFormat("{0}, ", instruction.Operand2);

				if (instruction.OperandCount == 3)
					builder.AppendFormat("{0}, ", instruction.Operand3);

				builder.Remove(builder.Length - 2, 2);
			}
			return builder.ToString();
		}

		#endregion Methods
	}
}
