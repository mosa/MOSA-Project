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
	/// Intermediate representation of the idiv instruction.
	/// </summary>
	public sealed class IDiv : X86Instruction
	{
		#region Data Members

		private static readonly OpCode opcode = new OpCode(new byte[] { 0xF7 }, 7);

		#endregion Data Members

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="IDiv"/>.
		/// </summary>
		public IDiv() :
			base(2, 3)
		{
		}

		#endregion Construction

		#region Methods

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(InstructionNode node, MachineCodeEmitter emitter)
		{
			emitter.Emit(opcode, node.Operand3, null);
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.IDiv(context);
		}

		#endregion Methods
	}
}
