// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using System;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 shld instruction.
	/// </summary>
	public class Shld : X86Instruction
	{
		#region Data Members

		private static readonly OpCode RM = new OpCode(new byte[] { 0x0F, 0xA5 });
		private static readonly OpCode C = new OpCode(new byte[] { 0x0F, 0xA4 });

		#endregion Data Members

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Shld"/>.
		/// </summary>
		public Shld() :
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
			throw new NotSupportedException();
		}

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(InstructionNode node, MachineCodeEmitter emitter)
		{
			if (node.Operand3.IsConstant)
			{
				emitter.Emit(C, node.Operand2, node.Result, node.Operand3);
			}
			else
			{
				emitter.Emit(RM, node.Operand2, node.Result);
			}
		}

		#endregion Methods
	}
}
