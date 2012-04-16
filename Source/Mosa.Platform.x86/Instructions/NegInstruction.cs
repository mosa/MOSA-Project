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
	/// Representations the x86 neg instruction.
	/// </summary>
	public sealed class NegInstruction : X86Instruction
	{
		#region Data Members

		private static readonly OpCode R = new OpCode(new byte[] { 0xF7 }, 3);
		private static readonly OpCode M = new OpCode(new byte[] { 0xF7 }, 3);

		#endregion // Data Members

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="DecInstruction"/>.
		/// </summary>
		public NegInstruction() :
			base(1, 1)
		{
		}

		#endregion // Construction

		#region Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="destination"></param>
		/// <param name="source"></param>
		/// <param name="third"></param>
		/// <returns></returns>
		protected override OpCode ComputeOpCode(Operand destination, Operand source, Operand third)
		{
			if (destination is RegisterOperand) return R;
			if (destination is MemoryOperand) return M;

			throw new ArgumentException(@"No opcode for operand type.");
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Neg(context);
		}

		#endregion // Methods
	}
}
