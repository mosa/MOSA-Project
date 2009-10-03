/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using Mosa.Runtime.CompilerFramework;

namespace Mosa.Platforms.x86.CPUx86
{
	/// <summary>
	/// Intermediate representation of the x86 int instruction.
	/// </summary>
	public sealed class DecInstruction : OneOperandInstruction
    {
        #region Data Members
	    private static readonly OpCode R = new OpCode(new byte[] { 0xFF }, 1);
	    private static readonly OpCode M = new OpCode(new byte[] { 0xFF }, 1);
        #endregion

        #region Properties
        /// <summary>
		/// Gets the instruction latency.
		/// </summary>
		/// <value>The latency.</value>
		public override int Latency { get { return 1; } }

		#endregion // Properties

		#region OneOperandInstruction Overrides
		/// <summary>
		/// Computes the op code.
		/// </summary>
		/// <param name="dest">The destination.</param>
		/// <param name="src">The source.</param>
		/// <param name="third">The third.</param>
		/// <returns></returns>
        protected override OpCode ComputeOpCode(Operand dest, Operand src, Operand third)
        {
            if (dest is RegisterOperand) return R;
            if (dest is MemoryOperand) return M;
            throw new ArgumentException(@"No opcode for operand type.");
        }

		/// <summary>
		/// Returns a string representation of the instruction.
		/// </summary>
		/// <returns>
		/// A string representation of the instruction in intermediate form.
		/// </returns>
		public override string ToString(Context context)
		{
			return String.Format(@"x86 dec {0}", context.Operand1);
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
		#endregion // OneOperandInstruction Overrides
	}
}
