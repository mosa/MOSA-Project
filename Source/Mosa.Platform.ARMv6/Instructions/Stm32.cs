// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv6.Instructions
{
	/// <summary>
	/// Stm instruction: Store Multiple
	/// </summary>
	public class Stm32 : ARMv6Instruction
	{
		public override string __description { get { return "Store Multiple"; } }

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Stm32"/>.
		/// </summary>
		public Stm32() :
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
