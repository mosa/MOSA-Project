// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv6.Instructions
{
	/// <summary>
	/// Adr instruction: Form PC-relative Address
	/// First operand is the PC. Second operand is an immediate constant.
	/// </summary>
	public class Adr : ARMv6Instruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Adr"/>.
		/// </summary>
		public Adr() :
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
		protected override void Emit(InstructionNode node, MachineCodeEmitter emitter)
		{
			// TODO
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IARMv6Visitor visitor, Context context)
		{
			visitor.Adr(context);
		}

		#endregion Methods
	}
}
