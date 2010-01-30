/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using System;
using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.CompilerFramework.Operands;

namespace Mosa.Platforms.x86.CPUx86
{
    /// <summary>
    /// Representations the x86 xchg instruction.
    /// </summary>
    public sealed class XchgInstruction : TwoOperandInstruction
	{
		#region Data Members

		private static readonly OpCode R_M = new OpCode(new byte[] { 0x87 });
        private static readonly OpCode R_R = new OpCode(new byte[] { 0x87 });
        private static readonly OpCode M_R = new OpCode(new byte[] { 0x87 });

		#endregion // Data Members

		#region Properties

		/// <summary>
		/// Gets the instruction latency.
		/// </summary>
		/// <value>The latency.</value>
		public override int Latency { get { return 2; } }

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
            if ((destination is RegisterOperand) && (source is MemoryOperand)) return R_M;
            if ((destination is RegisterOperand) && (source is RegisterOperand)) return R_R;
            if ((destination is MemoryOperand) && (source is RegisterOperand)) return M_R;
            
			throw new ArgumentException(@"No opcode for operand type."); 
        }

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Xchg(context);
		}

        #endregion // Methods
    }
}
