/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using Mosa.Runtime.CompilerFramework;

namespace Mosa.Platforms.x86.CPUx86
{
	/// <summary>
	/// Representations the x86 int instruction.
	/// </summary>
	public sealed class DecInstruction : OneOperandInstruction
    {
        #region Data Members

	    private static readonly OpCode Dec_8 = new OpCode(new byte[] { 0xFE }, 1);
        private static readonly OpCode Dec_16 = new OpCode(new byte[] { 0x66, 0xFF }, 1);
        private static readonly OpCode Dec_32 = new OpCode(new byte[] { 0xFF }, 1);

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
            if (IsByte(destination)) return Dec_8;
            if (IsShort(destination) || IsChar(destination)) return Dec_16;
            if (IsInt(destination)) return Dec_32;

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
