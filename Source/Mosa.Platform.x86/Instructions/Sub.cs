/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Intermediate representation of the sub instruction.
	/// </summary>
	public sealed class Sub : X86Instruction
	{

		#region Data Members

		private static readonly OpCode O_C = new OpCode(new byte[] { 0x81 }, 5);
		private static readonly OpCode R_O = new OpCode(new byte[] { 0x2B });
		private static readonly OpCode R_O_16 = new OpCode(new byte[] { 0x66, 0x2B });
		private static readonly OpCode M_R = new OpCode(new byte[] { 0x29 });

		#endregion
		
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Sub"/>.
		/// </summary>
		public Sub() :
			base(1, 2)
		{
		}

		#endregion // Construction

		#region Methods

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Sub(context);
		}

		/// <summary>
		/// Computes the opcode.
		/// </summary>
		/// <param name="destination">The destination operand.</param>
		/// <param name="source">The source operand.</param>
		/// <param name="third">The third operand.</param>
		/// <returns></returns>
		protected override OpCode ComputeOpCode(Operand destination, Operand source, Operand third)
		{
			if (third.IsConstant)
				return O_C;

			if (destination.IsRegister)
			{
				if (IsChar(third))
					return R_O_16;
				else
					return R_O;
			}
			if ((destination.IsMemoryAddress) && (third.IsRegister)) return M_R;

			throw new ArgumentException(@"No opcode for operand type.");
		}

		#endregion // Methods
	}
}
