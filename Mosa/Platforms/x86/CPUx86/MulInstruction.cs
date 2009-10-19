/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Mosa.Runtime.CompilerFramework;
using IR = Mosa.Runtime.CompilerFramework.IR;
using Mosa.Runtime.Metadata;

namespace Mosa.Platforms.x86.CPUx86
{
    /// <summary>
    /// Intermediate representation of the mul instruction.
    /// </summary>
    public sealed class MulInstruction : TwoOperandInstruction
    {
		#region Data Members

		private static readonly OpCode R = new OpCode(new byte[] { 0xF7 }, 4);
		private static readonly OpCode M = new OpCode(new byte[] { 0xF7 }, 4);

		#endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="MulInstruction"/> class.
        /// </summary>
        public MulInstruction()
        {
        }

        #endregion // Construction

		#region Properties

		/// <summary>
		/// Gets the instruction latency.
		/// </summary>
		/// <value>The latency.</value>
		public override int Latency { get { return 4; } }

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
			if (destination == null) return R;
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
			visitor.Mul(context);
		}

        #endregion // Methods
    }
}
