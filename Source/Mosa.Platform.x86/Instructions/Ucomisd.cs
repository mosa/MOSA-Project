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
using System.Diagnostics;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Intermediate representation for the x86 ucomisd instruction.
	/// </summary>
	public class Ucomisd : X86Instruction
	{
		#region Data Members

		private static readonly OpCode opcode = new OpCode(new byte[] { 0x66, 0x0F, 0x2E });

		#endregion Data Members

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Cmp"/>.
		/// </summary>
		public Ucomisd() :
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
			Debug.Assert(source.IsRegister);

			return opcode;
		}

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(InstructionNode node, MachineCodeEmitter emitter)
		{
			Debug.Assert(node.Result == null);

			OpCode opCode = ComputeOpCode(null, node.Operand1, node.Operand2);
			emitter.Emit(opCode, node.Operand1, node.Operand2);
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Ucomisd(context);
		}

		#endregion Methods
	}
}
