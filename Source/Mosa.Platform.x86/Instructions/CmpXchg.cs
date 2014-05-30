/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework;
using System;
using System.Diagnostics;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 compare-exchange instruction.
	/// </summary>
	public sealed class CmpXchg : X86Instruction
	{
		#region Data Member

		private static readonly OpCode RM_R = new OpCode(new byte[] { 0x0F, 0xB1 });

		#endregion Data Member

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="CmpXchg"/>.
		/// </summary>
		public CmpXchg() :
			base(0, 2)
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
			if ((source.IsRegister || source.IsMemoryAddress) && third.IsRegister) return RM_R;

			throw new ArgumentException(String.Format(@"x86.CmpXchg: No opcode for operand types {0} and {1}.", source, third));
		}

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(Context context, MachineCodeEmitter emitter)
		{
			Debug.Assert(context.Result == null);

			OpCode opCode = ComputeOpCode(null, context.Operand1, context.Operand2);
			emitter.Emit(opCode, context.Operand1, context.Operand2);
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.CmpXchg(context);
		}

		#endregion Methods
	}
}