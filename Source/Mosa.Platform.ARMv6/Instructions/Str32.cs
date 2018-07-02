// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv6.Instructions
{
	/// <summary>
	/// Str instruction: Store 32-bit word
	/// Load and store instructions
	/// </summary>
	public class Str32 : ARMv6Instruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Str32"/>.
		/// </summary>
		public Str32() :
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
			EmitMemoryStore(node, emitter);
		}

		public override string __emitter { get { return "EmitMemoryStore"; } }
		public override string __description { get { return "Store 32-bit word"; } }

		#endregion Methods
	}
}
