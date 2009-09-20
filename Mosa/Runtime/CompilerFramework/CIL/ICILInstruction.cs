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
		/// Determines flow behavior of this instruction.
		/// </summary>
		/// <value>The flow control.</value>
		/// <remarks>
		/// Knowledge of control flow is required for correct basic block
		/// building. Any instruction that alters the control flow must override
		/// this property and correctly identify its control flow modifications.
		/// </remarks>
		FlowControl FlowControl { get; }

		/// <summary>
		/// Decodes the specified CIL instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		void Decode(ref InstructionData instruction, IInstructionDecoder decoder);

		/// <summary>
		/// Validates the instruction operands and creates a matching variable for the result.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="compiler">The compiler.</param>
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
