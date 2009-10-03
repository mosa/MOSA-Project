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
		/// Computes the op code.
		/// </summary>
		/// <param name="dest">The destination.</param>
		/// <param name="src">The source.</param>
		/// <param name="thirdOperand">The third operand.</param>
		/// <returns></returns>
        protected override OpCode ComputeOpCode(Operand dest, Operand src, Operand thirdOperand)
        {
			if ((dest is RegisterOperand) && (src is ConstantOperand))
				return R_C;

			if ((dest is RegisterOperand) && (src is RegisterOperand))
				return R_R;

            if ((dest is RegisterOperand) && (src is MemoryOperand))
            {
                return IsUnsignedByte(dest) ? R_M_U8 : R_M;
            }

		    if ((dest is MemoryOperand) && (src is RegisterOperand))
            {
                if (IsByte(dest) || IsByte(src))
                    return M_R_U8;
                if (IsChar(dest) || IsChar(src) || IsShort(dest) || IsShort(src))
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
			return String.Format(@"x86.add {0}, {1} ; {0} += {1}", context.Operand1, context.Operand2);
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
