/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework;
using System.Diagnostics;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	///
	/// </summary>
	public abstract class TwoOperandInstruction : X86Instruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="TwoOperandInstruction"/>.
		/// </summary>
		protected TwoOperandInstruction() :
			base(1, 2)
		{
		}

		#endregion Construction

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="node">The context.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(InstructionNode node, MachineCodeEmitter emitter)
		{
			Debug.Assert(node.Result == node.Operand1, node.ToString());
			OpCode opCode = ComputeOpCode(node.Result, node.Operand1, node.Operand2);
			emitter.Emit(opCode, node.Result, node.Operand2);
		}
	}
}
