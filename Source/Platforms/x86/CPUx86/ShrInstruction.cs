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
using Mosa.Runtime.CompilerFramework.Operands;
using IR = Mosa.Runtime.CompilerFramework.IR;
using Mosa.Runtime.Metadata;

namespace Mosa.Platforms.x86.CPUx86
{
	/// <summary>
	/// Representations the x86 shift right instruction.
	/// </summary>
	public sealed class ShrInstruction : TwoOperandInstruction
	{
		#region Codes

		private static readonly OpCode R = new OpCode(new byte[] { 0xD3 }, 5);
		private static readonly OpCode M = new OpCode(new byte[] { 0xD3 }, 5);
		private static readonly OpCode R_C = new OpCode(new byte[] { 0xC1 }, 5);
		private static readonly OpCode M_C = new OpCode(new byte[] { 0xC1 }, 5);

		#endregion

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
			if ((destination is RegisterOperand) && (source is ConstantOperand)) return R_C;
			if ((destination is MemoryOperand) && (source is ConstantOperand)) return M_C;
			if (destination is RegisterOperand) return R;
			if (destination is MemoryOperand) return M;

			throw new ArgumentException(@"No opcode for operand type.");
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="emitter"></param>
        protected override void Emit(Context ctx, MachineCodeEmitter emitter)
        {
            OpCode opCode = ComputeOpCode(ctx.Result, ctx.Operand1, ctx.Operand2);
            if (ctx.Operand1 is ConstantOperand)
            {
                ConstantOperand op = ctx.Operand1 as ConstantOperand;
                op = new ConstantOperand(new Mosa.Runtime.Metadata.Signatures.SigType(CilElementType.U1), op.Value);
                emitter.Emit(opCode, ctx.Result, op);
            }
            else
                emitter.Emit(opCode, ctx.Operand1, null);
        }

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Shr(context);
		}

		#endregion // Methods
	}
}
