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
	/// Intermediate representation of the x86 not instruction.
	/// </summary>
	public sealed class Not : X86Instruction
	{
		#region Data Members

		private static readonly OpCode MR_8 = new OpCode(new byte[] { 0xF6 }, 2);
		private static readonly OpCode MR_16 = new OpCode(new byte[] { 0x66, 0xF7 }, 2);
		private static readonly OpCode MR = new OpCode(new byte[] { 0xF7 }, 2);

		#endregion // Data Members
		
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Not"/>.
		/// </summary>
		public Not() :
			base(1, 1)
		{
		}

		#endregion // Construction

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
			if ((destination.IsRegister) || (destination.IsMemoryAddress))
			{
				if (IsByte(destination)) return MR_8;
				if (IsChar(destination)) return MR_16;
				return MR;
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
			OpCode opCode = ComputeOpCode(context.Result, null, null);
			emitter.Emit(opCode, context.Result, null, null);
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Not(context);
		}

		#endregion // Methods
	}
}
