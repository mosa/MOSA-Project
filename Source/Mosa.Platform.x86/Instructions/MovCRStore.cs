// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using System;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 MovCRStore instruction.
	/// </summary>
	public sealed class MovCRStore : X86Instruction
	{
		#region Data Members

		private static readonly LegacyOpCode R_CR = new LegacyOpCode(new byte[] { 0x0F, 0x20 });
		private static readonly LegacyOpCode CR_R = new LegacyOpCode(new byte[] { 0x0F, 0x22 });

		#endregion Data Members

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="MovCRStore" />.
		/// </summary>
		public MovCRStore() :
			base(0, 2)
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
			emitter.Emit(CR_R, node.Operand1, node.Operand2);
		}

		#endregion Methods
	}
}
