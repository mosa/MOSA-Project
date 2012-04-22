/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Operands;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 compare-exchange instruction.
	/// </summary>
	/// <remarks>
	/// This instruction compares the value of Operand0 and Operand1. If they are
	/// equal, Operand0 is set to the value of Operand2.
	/// </remarks>
	public sealed class CmpXchg : ThreeOperandInstruction
	{
		#region Data Member

		private static readonly OpCode RM_R = new OpCode(new byte[] { 0x0F, 0xB1 });

		#endregion //Data Member

		#region Methods

		/// <summary>
		/// Gets the additional output registers.
		/// </summary>
		public override RegisterBitmap AdditionalOutputRegisters { get { return new RegisterBitmap(GeneralPurposeRegister.EAX); } }

		/// <summary>
		/// Gets the additional input registers.
		/// </summary>
		public override RegisterBitmap AdditionalInputRegisters { get { return new RegisterBitmap(GeneralPurposeRegister.EAX); } }

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
