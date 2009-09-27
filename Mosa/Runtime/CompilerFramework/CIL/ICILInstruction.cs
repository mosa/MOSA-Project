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
		/// <param name="ctx">The context.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		void Decode(Context ctx, IInstructionDecoder decoder);

		/// <summary>
		/// Validates the instruction operands and creates a matching variable for the result.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="compiler">The compiler.</param>
		void Validate(Context ctx, IMethodCompiler compiler);

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		string ToString(Context ctx);

		/// <summary>
		/// Gets the default operand count of the instruction
		/// </summary>
		/// <value>The operand count.</value>
		byte DefaultOperandCount { get; }

		/// <summary>
		/// Gets the default result operand count of the instruction
		/// </summary>
		/// <value>The operand result count.</value>
		byte DefaultResultCount { get; }

		/// <summary>
		/// Gets the opcode.
		/// </summary>
		/// <value>The opcode.</value>
		OpCode OpCode { get; }

		/// <summary>
		/// Determines if the IL decoder pushes the results of this instruction onto the IL operand stack.
		/// </summary>
		/// <value><c>true</c> if [push result]; otherwise, <c>false</c>.</value>
		bool PushResult { get; }
	}
}
