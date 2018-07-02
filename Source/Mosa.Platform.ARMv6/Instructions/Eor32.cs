// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv6.Instructions
{
	/// <summary>
	/// Eor instruction: Bitwise Exclusive OR
	/// </summary>
	public class Eor32 : ARMv6Instruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Eor32"/>.
		/// </summary>
		public Eor32() :
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
			EmitDataProcessingInstruction(node, emitter, Bits.b0001);
		}

		public override string __emitter { get { return "EmitDataProcessingInstruction"; } }
		public override string __bits { get { return "b0001"; } }
		public override string __description { get { return "Bitwise Exclusive OR"; } }

		#endregion Methods
	}
}
