// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 cdq instruction.
	/// </summary>
	public sealed class Cdq : X86Instruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="Cdq" /> class.
		/// </summary>
		public Cdq()
			: base(2, 1)
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
			emitter.WriteByte(0x99);
		}

		#endregion Methods
	}
}
