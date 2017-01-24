// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 call instruction.
	/// </summary>
	public sealed class Call : X86Instruction
	{
		#region Data Member

		private static readonly LegacyOpCode RegCall = new LegacyOpCode(new byte[] { 0xFF }, 2);
		private static readonly byte[] CALL = new byte[] { 0xE8 };

		#endregion Data Member

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Call"/>.
		/// </summary>
		public Call() :
			base(0, 0)
		{
		}

		#endregion Construction

		#region Properties

		public override FlowControl FlowControl { get { return FlowControl.Call; } }

		#endregion Properties

		#region Methods

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="emitter">The emitter.</param>
		internal override void EmitLegacy(InstructionNode node, X86CodeEmitter emitter)
		{
			if (node.OperandCount == 0)
			{
				emitter.EmitRelativeBranch(CALL, node.BranchTargets[0].Label);
				return;
			}

			if (node.Operand1.IsSymbol)
			{
				emitter.WriteByte(0xE8);
				emitter.EmitCallSite(node.Operand1);
			}
			else
			{
				emitter.Emit(RegCall, node.Operand1);
			}
		}

		#endregion Methods
	}
}
