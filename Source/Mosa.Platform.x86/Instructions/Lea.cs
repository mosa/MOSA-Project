/*
 * (c) 2009 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <rootnode@mosa-project.org>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	///
	/// </summary>
	public sealed class Lea : TwoOperandInstruction
	{
		#region Data Members

		private static readonly OpCode opcode = new OpCode(new byte[] { 0x8D });

		#endregion Data Members

		#region Methods

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="node">The context.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(InstructionNode node, MachineCodeEmitter emitter)
		{
			emitter.Emit(opcode, node.Result, node.Operand1);
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Lea(context);
		}

		#endregion Methods
	}
}
