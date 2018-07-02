// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv6.Instructions
{
	/// <summary>
	/// Ldr instruction: Load 32-bit word
	/// Load and store instructions
	/// </summary>
	public class Ldr32 : ARMv6Instruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Ldr32"/>.
		/// </summary>
		public Ldr32() :
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
			EmitMemoryLoad(node, emitter);
		}

		public override string __emitter { get { return "EmitMemoryLoad"; } }
		public override string __description { get { return "Load 32-bit word"; } }

		#endregion Methods
	}
}
