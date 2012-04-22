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

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 in instruction.
	/// </summary>
	public sealed class In : TwoOperandInstruction
	{
		#region Data Members

		private static readonly OpCode R_C_8 = new OpCode(new byte[] { 0xE4 });
		private static readonly OpCode R_R_8 = new OpCode(new byte[] { 0xEC });
		private static readonly OpCode R_C_32 = new OpCode(new byte[] { 0xE5 });
		private static readonly OpCode R_R_32 = new OpCode(new byte[] { 0xED });
		private static readonly OpCode opcode = new OpCode(new byte[] { 0xEC });

		#endregion // Data Members

		#region Methods

		/// <summary>
		/// Gets the additional output registers.
		/// </summary>
		public override RegisterBitmap AdditionalOutputRegisters { get { return new RegisterBitmap(GeneralPurposeRegister.EAX); } }

		/// <summary>
		/// Computes the op code.
		/// </summary>
		/// <param name="destination">The destination.</param>
		/// <param name="source">The source.</param>
		/// <param name="empty">The empty.</param>
		/// <returns></returns>
		protected override OpCode ComputeOpCode(Operand destination, Operand source, Operand empty)
		{
			// FIXME: This method is not called. 
			if (IsByte(source))
			{
				if ((destination is RegisterOperand) && (source is ConstantOperand)) return R_C_8;
				if ((destination is RegisterOperand) && (source is RegisterOperand)) return R_R_8;
			}
			else
			{
				if ((destination is RegisterOperand) && (source is ConstantOperand)) return R_C_32;
				if ((destination is RegisterOperand) && (source is RegisterOperand)) return R_R_32;
			}

			throw new ArgumentException(@"No opcode for operand type.");
		}

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(Context context, MachineCodeEmitter emitter)
		{
			// FIXME: Incoming operands are incorrect. This method ignores them.
			emitter.Emit(R_R_8, null, null);
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.In(context);
		}

		#endregion // Methods
	}
}
