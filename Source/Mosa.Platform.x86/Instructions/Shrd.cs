// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 shrd instruction.
	/// </summary>
	public class Shrd : X86Instruction
	{
		#region Data Members

		private static readonly LegacyOpCode RM = new LegacyOpCode(new byte[] { 0x0F, 0xAD }, 4);
		private static readonly LegacyOpCode C = new LegacyOpCode(new byte[] { 0x0F, 0xAC }, 4);

		#endregion Data Members

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Shrd"/>.
		/// </summary>
		public Shrd() :
			base(1, 3)
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
