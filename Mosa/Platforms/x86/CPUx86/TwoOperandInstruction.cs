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
	/// Abstract base class for x86 instructions with two operands.
    /// </summary>
    /// <remarks>
    /// The <see cref="TwoOperandInstruction"/> is the base class for
    /// x86 instructions using two operands. It provides properties to
    /// easily access the individual operands.
    /// </remarks>
    public abstract class TwoOperandInstruction : BaseInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="ThreeOperandInstruction"/>.
        /// </summary>
        protected TwoOperandInstruction() :
            base(1, 1)
        {
        }

        #endregion // Construction


		/// <summary>
		/// Computes the opcode.
		/// </summary>
		/// <param name="destination">The destination operand.</param>
		/// <param name="source">The source operand.</param>
		/// <param name="third">The third operand.</param>
		/// <returns></returns>
        protected override OpCode ComputeOpCode(Operand destination, Operand source, Operand third)
        {
            throw new NotSupportedException();
        }

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="codeStream">The code stream.</param>
        public override void Emit(Context ctx, System.IO.Stream codeStream)
        {
            OpCode opCode = ComputeOpCode(ctx.Result, ctx.Operand1, null);
            MachineCodeEmitter.Emit(codeStream, opCode, ctx.Result, ctx.Operand1);
        }
    }
}
