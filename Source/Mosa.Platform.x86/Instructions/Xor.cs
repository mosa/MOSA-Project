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
	/// Representations the x86 xor instruction.
	/// </summary>
	public sealed class Xor : TwoOperandInstruction
	{
		#region Data Members

		private static readonly OpCode R_C = new OpCode(new byte[] { 0x81 }, 6);
		private static readonly OpCode R_M = new OpCode(new byte[] { 0x33 });
		private static readonly OpCode R_R = new OpCode(new byte[] { 0x33 });
		private static readonly OpCode M_R = new OpCode(new byte[] { 0x31 });
		private static readonly OpCode M_C = new OpCode(new byte[] { 0x81 }, 6);

		#endregion // Data Members

		#region Methods

		public override RegisterBitmap GetInputRegisters(Context context)
		{
			// Special case when the input register does not matter, example: XOR EAX, EAX. 

			if (context.Result.IsCPURegister && context.Operand1.IsCPURegister && context.Result.Register == context.Operand1.Register)
				return NoRegisters;

			return base.GetInputRegisters(context);
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
			if ((destination.IsRegister) && (source.IsConstant)) return R_C;
			if ((destination.IsRegister) && (source.IsMemoryAddress)) return R_M;
			if ((destination.IsRegister) && (source.IsRegister)) return R_R;
			if ((destination.IsMemoryAddress) && (source.IsRegister)) return M_R;
			if ((destination.IsMemoryAddress) && (source.IsConstant)) return M_C;

			throw new ArgumentException(@"No opcode for operand type.");
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Xor(context);
		}

		#endregion // Methods
	}
}
