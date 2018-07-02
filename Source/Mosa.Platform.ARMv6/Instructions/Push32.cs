// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv6.Instructions
{
	/// <summary>
	/// Push instruction: Push multiple registers onto the stack
	/// This instruction decrements the base register before the memory access and updates the base register.
	/// </summary>
	public class Push32 : ARMv6Instruction
	{
		public override string __description { get { return "Push multiple registers onto the stack"; } }
		public override string __description2 { get { return "This instruction decrements the base register before the memory access and updates the base register."; } }

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Push32"/>.
		/// </summary>
		public Push32() :
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
		protected override void Emit(InstructionNode node, ARMv6CodeEmitter emitter)
		{
			// TODO
		}

		#endregion Methods
	}
}
