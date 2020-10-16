// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// End Finally Instruction
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.CIL.BaseCILInstruction" />
	public sealed class EndFinallyInstruction : BaseCILInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="EndFinallyInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public EndFinallyInstruction(OpCode opcode)
			: base(opcode, 0)
		{
		}

		#endregion Construction

		public override FlowControl FlowControl { get { return FlowControl.EndFinally; } }
	}
}
