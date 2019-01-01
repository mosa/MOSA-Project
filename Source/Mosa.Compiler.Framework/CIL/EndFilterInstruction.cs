// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// EndFilter Instruction
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.CIL.UnaryInstruction" />
	public sealed class EndFilterInstruction : UnaryInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="EndFilterInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public EndFilterInstruction(OpCode opcode)
			: base(opcode)
		{
		}

		#endregion Construction

		public override FlowControl FlowControl { get { return FlowControl.EndFilter; } }

		#region Methods

		/// <summary>
		/// Validates the instruction operands and creates a matching variable for the result.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="methodCompiler">The compiler.</param>
		public override void Resolve(Context context, MethodCompiler methodCompiler)
		{
			base.Resolve(context, methodCompiler);
		}

		#endregion Methods
	}
}
