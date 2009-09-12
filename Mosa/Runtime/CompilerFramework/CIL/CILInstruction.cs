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
	public class CILInstruction : ICILInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="CILInstruction"/> class.
		/// </summary>
		public CILInstruction()
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
		public virtual void Decode(ref InstructionData instruction, OpCode opcode, IInstructionDecoder decoder)
		{
			/* Default implementation is to do nothing */
			instruction.Instruction = this;
		}

		/// <summary>
		/// Validates the specified instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="compiler">The compiler.</param>
		public virtual void Validate(ref InstructionData instruction, IMethodCompiler compiler)
		{
			/* Default implementation is to do nothing */
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public virtual string ToString(ref InstructionData instruction)
		{
			return base.ToString();
		}

		#endregion // ICILInstruction Overrides

		#region Operand Overrides

		#endregion // Operand Overrides
	}
}
