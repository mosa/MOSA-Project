/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 compare-exchange instruction.
	/// </summary>
	public sealed class CmpXchg : X86Instruction
	{
		#region Data Member

		private static readonly OpCode RM_R = new OpCode(new byte[] { 0x0F, 0xB1 });

		#endregion //Data Member

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="CmpXchg"/>.
		/// </summary>
		public CmpXchg() :
			base(2, 3)
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
			return RM_R;
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.CmpXchg(context);
		}

		#endregion // Methods
	}
}
