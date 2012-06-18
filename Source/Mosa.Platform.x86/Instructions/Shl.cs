/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using System;

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Operands;
using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 shift left instruction.
	/// </summary>
	public sealed class Shl : TwoOperandInstruction
	{

		#region Data Members

		private static readonly OpCode R_C = new OpCode(new byte[] { 0xC1 }, 4);
		private static readonly OpCode M_C = new OpCode(new byte[] { 0xC1 }, 4);
		private static readonly OpCode R = new OpCode(new byte[] { 0xD3 }, 4);
		private static readonly OpCode M = new OpCode(new byte[] { 0xD3 }, 4);

		#endregion

		#region Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="destination"></param>
		/// <param name="source"></param>
		/// <param name="third"></param>
		/// <returns></returns>
		protected override OpCode ComputeOpCode(Operand destination, Operand source, Operand third)
		{
			if (destination is RegisterOperand && source is ConstantOperand) return R_C;
			if (destination is MemoryOperand && source is ConstantOperand) return M_C;
			if (destination is RegisterOperand) return R;
			if (destination is MemoryOperand) return M;

			throw new ArgumentException(@"No opcode for operand type.");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="emitter"></param>
		protected override void Emit(Context context, MachineCodeEmitter emitter)
		{
			OpCode opCode = ComputeOpCode(context.Result, context.Operand1, context.Operand2);
			if (context.Operand1 is ConstantOperand)
			{
				ConstantOperand op = context.Operand1 as ConstantOperand;
				op = new ConstantOperand(BuiltInSigType.Byte, op.Value);
				emitter.Emit(opCode, context.Result, op);
			}
			else
				emitter.Emit(opCode, context.Operand1, null);
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Shl(context);
		}

		#endregion // Methods
	}
}
