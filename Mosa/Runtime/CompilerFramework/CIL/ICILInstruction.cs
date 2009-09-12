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
using System.Text;
using Mosa.Runtime.CompilerFramework;

namespace Mosa.Runtime.CompilerFramework.CIL
{
	/// <summary>
	/// Interface to a CIL instruction
	/// </summary>
	public interface ICILInstruction : IInstruction
	{
		/// <summary>
		/// Decodes the specified CIL instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="opcode">The opcode of the load.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		void Decode(ref InstructionData instruction, OpCode opcode, IInstructionDecoder decoder);

		/// <summary>
		/// Validates the instruction operands and creates a matching variable for the result.
		/// </summary>
		void Validate(ref InstructionData instruction, IMethodCompiler compiler);

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		string ToString(ref InstructionData instruction);
	}
}
