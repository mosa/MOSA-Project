// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 popad instruction.
	/// </summary>
	public sealed class Popad : X86Instruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Popad"/>.
		/// </summary>
		public Popad() :
			base(0, 0)
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
			emitter.WriteByte(0x61);
		}

		#endregion Methods
	}
}
