/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using System;
using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Intermediate representation of the arithmetic shift right instruction.
	/// </summary>
	public sealed class Sal : TwoOperandInstruction
	{

		#region Data Members

		private static readonly OpCode RM = new OpCode(new byte[] { 0xD3 }, 4);
		private static readonly OpCode RMC = new OpCode(new byte[] { 0xC1 }, 4);

		#endregion

		#region Methods

		/// <summary>
		/// Computes the opcode.
		/// </summary>
		/// <param name="destination">The destination operand.</param>
		/// <param name="source">The source operand.</param>
		/// <param name="third">The third operand.</param>
		/// <returns></returns>
		protected override OpCode ComputeOpCode(Operand destination, Operand source, Operand third)
		{
			if ((destination.IsRegister || destination.IsMemoryAddress) && (source.IsConstant)) return RMC;
			if (destination.IsRegister || destination.IsMemoryAddress) return RM;

			throw new ArgumentException(@"No opcode for operand type.");
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Sal(context);
		}

		#endregion // Methods
	}
}
