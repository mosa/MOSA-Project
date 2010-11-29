/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <rootnode@mosa-project.org>
 */

using System;
using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.CompilerFramework.Operands;

namespace Mosa.Platform.X86.CPUx86
{
	/// <summary>
	/// Representations the x86 shrd instruction.
	/// </summary>
	public class ShldInstruction : ThreeOperandInstruction
	{
		#region Data Members

		private static readonly OpCode Register = new OpCode(new byte[] { 0x0F, 0xA5 });
		private static readonly OpCode Constant = new OpCode(new byte[] { 0x0F, 0xA4 });

		#endregion // Data Members

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
			if (third is RegisterOperand)
				return Register;
			if (third is ConstantOperand)
				return Constant;
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
			if (ctx.Operand2 is ConstantOperand)
			{
				ConstantOperand op = ctx.Operand2 as ConstantOperand;
				op = new ConstantOperand(new Mosa.Runtime.Metadata.Signatures.SigType(Mosa.Runtime.Metadata.CilElementType.U1), op.Value);
				emitter.Emit(opCode, ctx.Result, ctx.Operand1, op);
			}
			else
				emitter.Emit(opCode, ctx.Result, ctx.Operand1, ctx.Operand2);
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Shrd(context);
		}

		#endregion // Methods
	}
}
