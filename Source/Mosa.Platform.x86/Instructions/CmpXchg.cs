// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using System;

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
			base(1, 3)
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
			if ((destination.IsRegister || destination.IsMemoryAddress) && source.IsRegister) return RM_R;

			throw new ArgumentException(String.Format(@"x86.CmpXchg: No opcode for operand types {0} and {1}.", source, third));
		}

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(InstructionNode node, MachineCodeEmitter emitter)
		{
			OpCode opCode = ComputeOpCode(node.Operand1, node.Operand2, node.Operand3);
			emitter.Emit(opCode, node.Operand1, node.Operand2, node.Operand3);
		}

		#endregion Methods
	}
}
