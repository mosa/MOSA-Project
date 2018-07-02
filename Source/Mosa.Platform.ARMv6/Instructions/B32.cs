// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv6.Instructions
{
	/// <summary>
	/// B instruction: Branch to target address
	/// </summary>
	public class B32 : ARMv6Instruction
	{
		public override string __description { get { return "Branch to target address"; } }

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="B32"/>.
		/// </summary>
		public B32() :
			base(1, 3)
		{
		}

		#endregion Construction

		#region Methods

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(InstructionNode node, ARMv6CodeEmitter emitter)
		{
			// TODO
		}

		#endregion Methods
	}
}
