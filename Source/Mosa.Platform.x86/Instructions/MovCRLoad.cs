// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 MovCRLoad instruction.
	/// </summary>
	public sealed class MovCRLoad : X86Instruction
	{
		#region Data Members

		private static readonly LegacyOpCode R_CR = new LegacyOpCode(new byte[] { 0x0F, 0x20 });

		#endregion Data Members

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="MovCRLoad" />.
		/// </summary>
		public MovCRLoad() :
			base(1, 1)
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
			emitter.Emit(R_CR, node.Operand1, node.Result);
		}

		#endregion Methods
	}
}
