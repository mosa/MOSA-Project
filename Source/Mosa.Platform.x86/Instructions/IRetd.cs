// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 iretd instruction.
	/// </summary>
	public sealed class IRetd : X86Instruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="IRetd"/>.
		/// </summary>
		public IRetd() :
			base(0, 0)
		{
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the flow control.
		/// </summary>
		/// <value>The flow control.</value>
		public override FlowControl FlowControl { get { return FlowControl.Return; } }

		#endregion Properties

		#region Methods

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="emitter">The emitter.</param>
		internal override void EmitLegacy(InstructionNode node, X86CodeEmitter emitter)
		{
			emitter.WriteByte(0xCF);
		}

		#endregion Methods
	}
}
