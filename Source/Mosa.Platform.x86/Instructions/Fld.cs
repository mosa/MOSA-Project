// Copyright (c) MOSA Project. Licensed under the New BSD License.

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

			if (source.IsR4)
				return m32fp;
			else
				return m64fp;
		}

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(InstructionNode node, MachineCodeEmitter emitter)
		{
			OpCode code = ComputeOpCode(node.Result, node.Operand1, node.Operand2);
			emitter.Emit(code, node.Operand1, null);
		}

		#endregion Methods
	}
}
