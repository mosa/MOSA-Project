/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using Mosa.Runtime.CompilerFramework;

namespace Mosa.Platforms.x86.CPUx86
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class AddInstruction : TwoOperandInstruction
	{

		#region Data Members

		private static readonly OpCode R_C = new OpCode(new byte[] { 0x81 }, 0);
		private static readonly OpCode R_R = new OpCode(new byte[] { 0x03 });
		private static readonly OpCode R_M = new OpCode(new byte[] { 0x03 });
		private static readonly OpCode M_R = new OpCode(new byte[] { 0x01 });
		private static readonly OpCode R_M_U8 = new OpCode(new byte[] { 0x02 }); // Add r/m8 to r8
		private static readonly OpCode M_R_U8 = new OpCode(new byte[] { 0x00 }); // Add r8 to r/m8

		#endregion

		#region Properties

		/// <summary>
		/// Gets the instruction latency.
		/// </summary>
		/// <value>The latency.</value>
		public override int Latency { get { return 1; } }

		#endregion // Properties

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
			if ((destination is RegisterOperand) && (source is ConstantOperand))
				return R_C;

			if ((destination is RegisterOperand) && (source is RegisterOperand))
				return R_R;

			if ((destination is RegisterOperand) && (source is MemoryOperand))
				return IsUnsignedByte(destination) ? R_M_U8 : R_M;

			if ((destination is MemoryOperand) && (source is RegisterOperand))
            {
				if (IsByte(destination) || IsByte(source))
                    return M_R_U8;
				if (IsChar(destination) || IsChar(source) || IsShort(destination) || IsShort(source))
                    return M_R_U8;
                return M_R;
            }

		    throw new ArgumentException(@"No opcode for operand type.");
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString(Context context)
		{
			return String.Format(@"X86.add {0}, {1} ; {0} += {1}", context.Operand1, context.Operand2);
		}
		
		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Add(context);
		}

		#endregion // Methods

	}
}
