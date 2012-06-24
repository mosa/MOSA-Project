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

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 out instruction.
	/// </summary>
	public sealed class Out : X86Instruction
	{
		#region Data Members

		private static readonly OpCode C_R_8 = new OpCode(new byte[] { 0xE6 });
		private static readonly OpCode R_R_8 = new OpCode(new byte[] { 0xEE });
		private static readonly OpCode C_R_32 = new OpCode(new byte[] { 0xE7 });
		private static readonly OpCode R_R_32 = new OpCode(new byte[] { 0xEF });

		#endregion // Data Members

		/// <summary>
		/// Initializes a new instance of <see cref="Out"/>.
		/// </summary>
		public Out() :
			base(2, 0)
		{
		}

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
			// FIXME: This method is not called. 
			if (IsByte(third))
			{
				if ((source.IsConstant) && (third.IsRegister)) return C_R_8;
				if ((source.IsRegister) && (third.IsRegister)) return R_R_8;
			}
			else
			{
				if ((source.IsConstant) && (third.IsRegister)) return C_R_32;
				if ((source.IsRegister) && (third.IsRegister)) return R_R_32;
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
			visitor.Out(context);
		}

		#endregion // Methods
	}
}
