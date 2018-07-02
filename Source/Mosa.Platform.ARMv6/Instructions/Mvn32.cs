// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv6.Instructions
{
	/// <summary>
	/// Mvn instruction: Bitwise NOT
	/// Has only one operand. ARMv6-M does not support any immediate or shift options.
	/// </summary>
	public class Mvn32 : ARMv6Instruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Mvn32"/>.
		/// </summary>
		public Mvn32() :
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
			EmitDataProcessingInstruction(node, emitter, Bits.b1111);
		}

		public override string __emitter { get { return "EmitDataProcessingInstruction"; } }
		public override string __bits { get { return "b1111"; } }
		public override string __description { get { return "Bitwise NOT"; } }
		public override string __description2 { get { return "Has only one operand. ARMv6-M does not support any immediate or shift options."; } }

		#endregion Methods
	}
}
