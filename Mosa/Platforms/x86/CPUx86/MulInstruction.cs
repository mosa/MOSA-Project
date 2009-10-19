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
        /// 
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="codeStream"></param>
        public override void Emit(Context ctx, System.IO.Stream codeStream)
        {
            OpCode opCode = ComputeOpCode(ctx.Result, ctx.Operand1, null);
            MachineCodeEmitter.Emit(codeStream, opCode, null, ctx.Operand1);
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
