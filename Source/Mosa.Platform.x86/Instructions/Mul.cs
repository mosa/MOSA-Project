// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Intermediate representation of the mul instruction.
	/// </summary>
	public sealed class Mul : X86Instruction
	{
		#region Data Members

		private static readonly LegacyOpCode opcode = new LegacyOpCode(new byte[] { 0xF7 }, 4);

		#endregion Data Members

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Mul"/>.
		/// </summary>
		public Mul() :
			base(2, 2)
		{
		}

		#endregion Construction

		#region Methods

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="emitter">The emitter.</param>
		internal override void EmitLegacy(InstructionNode node, X86CodeEmitter emitter)
		{
			emitter.Emit(opcode, node.Operand2);
		}

		#endregion Methods
	}
}
