// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Nop
	/// </summary>
	/// <seealso cref="Mosa.Platform.x86.X86Instruction" />
	public class Nop : X86Instruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="Nop" /> class.
		/// </summary>
		public Nop()
			: base(0, 0)
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
			emitter.WriteByte(0x90);
		}

		#endregion Methods
	}
}
