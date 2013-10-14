/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework;
using System;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 movCR instruction.
	/// </summary>
	public sealed class MovCR : X86Instruction
	{
		#region Data Members

		private static readonly OpCode R_CR = new OpCode(new byte[] { 0x0F, 0x20 });
		private static readonly OpCode CR_R = new OpCode(new byte[] { 0x0F, 0x22 });

		#endregion Data Members

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Mov"/>.
		/// </summary>
		public MovCR() :
			base(1, 1)
		{
		}

		#endregion Construction

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
			if (destination.Register is ControlRegister) return CR_R;
			if (source.Register is ControlRegister) return R_CR;

			throw new ArgumentException(@"No opcode for operand type. [" + destination + ", " + source + ")");
		}

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(Context context, MachineCodeEmitter emitter)
		{
			OpCode opCode = ComputeOpCode(context.Result, context.Operand1, null);

			if (context.Result.Register is ControlRegister)
			{
				emitter.Emit(opCode, context.Result, context.Operand1);
			}
			else
			{
				emitter.Emit(opCode, context.Operand1, context.Result);
			}
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			return; // none
		}

		#endregion Methods
	}
}