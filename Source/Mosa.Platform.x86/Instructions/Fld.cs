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
using System.Diagnostics;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 FldSt instruction.
	/// </summary>
	public sealed class Fld : X86Instruction
	{
		#region Data Members

		private static readonly OpCode m32fp = new OpCode(new byte[] { 0xD9 }, 0);
		private static readonly OpCode m64fp = new OpCode(new byte[] { 0xDD }, 0);

		#endregion Data Members

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Fld"/>.
		/// </summary>
		public Fld() :
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
			Debug.Assert(source.IsMemoryAddress);

			// TODO: support 32 bit too

			return m64fp;
		}

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(Context context, MachineCodeEmitter emitter)
		{
			OpCode code = ComputeOpCode(context.Result, context.Operand1, context.Operand2);
			emitter.Emit(code, context.Operand1, null);
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			return; 
		}

		#endregion Methods
	}
}