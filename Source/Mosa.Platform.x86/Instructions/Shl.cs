/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 shift left instruction.
	/// </summary>
	public sealed class Shl : X86Instruction
	{

		#region Data Members

		private static readonly OpCode C = new OpCode(new byte[] { 0xC1 }, 4);
		private static readonly OpCode C1 = new OpCode(new byte[] { 0xD1 }, 4);
		private static readonly OpCode RM = new OpCode(new byte[] { 0xD3 }, 4);

		#endregion
		
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Shl"/>.
		/// </summary>
		public Shl() :
			base(1, 2)
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
		/// <exception cref="System.ArgumentException"></exception>
		protected override OpCode ComputeOpCode(Operand destination, Operand source, Operand third)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(Context context, MachineCodeEmitter emitter)
		{
			if (context.Operand2.IsConstant)
			{
				if (context.Operand2.ValueAsLongInteger == 1)
					emitter.Emit(C1, context.Result, null);
				else
					emitter.Emit(C, context.Result, context.Operand2);
			}
			else
			{
				emitter.Emit(RM, context.Operand1, null);
			}
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
