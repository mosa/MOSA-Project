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

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Intermediate representation of the div instruction.
	/// </summary>
	public sealed class Div : X86Instruction
	{
		#region Data Members

		private static readonly OpCode opcode = new OpCode(new byte[] { 0xF7 }, 6);

		#endregion Data Members

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Div"/>.
		/// </summary>
		public Div() :
			base(2, 3)
		{
		}

		#endregion Construction

		#region Methods

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(Context context, MachineCodeEmitter emitter)
		{
			emitter.Emit(opcode, context.Operand3, null);
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Div(context);
		}

		#endregion Methods
	}
}