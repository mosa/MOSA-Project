/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Operands;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 int instruction.
	/// </summary>
	public sealed class Dec : X86Instruction
	{

		#region Data Members

		private static readonly OpCode DEC8 = new OpCode(new byte[] { 0xFE }, 1);
		private static readonly OpCode DEC16 = new OpCode(new byte[] { 0x66, 0xFF }, 1);
		private static readonly OpCode DEC32 = new OpCode(new byte[] { 0xFF }, 1);

		#endregion

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Dec"/>.
		/// </summary>
		public Dec() :
			base(0, 1)
		{
		}

		#endregion // Construction

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
			if (IsByte(destination)) return DEC8;
			if (IsShort(destination) || IsChar(destination)) return DEC16;
			if (IsInt(destination)) return DEC32;

			throw new ArgumentException(@"No opcode for operand type.");
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Dec(context);
		}

		#endregion // Methods
	}
}
