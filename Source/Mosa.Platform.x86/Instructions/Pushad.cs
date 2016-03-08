// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 cli instruction.
	/// </summary>
	public sealed class Pushad : X86Instruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Pushad"/>.
		/// </summary>
		public Pushad() :
			base(0, 0)
		{
		}

		#endregion Construction

		#region Methods

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(InstructionNode node, MachineCodeEmitter emitter)
		{
			emitter.WriteByte(0x60);
		}

		#endregion Methods
	}
}
