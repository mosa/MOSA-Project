// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// Calli Instruction
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.CIL.InvokeInstruction" />
	public sealed class CalliInstruction : InvokeInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="CalliInstruction" /> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public CalliInstruction(OpCode opcode)
			: base(opcode)
		{
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the supported immediate metadata tokens in the instruction.
		/// </summary>
		/// <value></value>
		protected override InvokeInstruction.InvokeSupportFlags InvokeSupport
		{
			get { return InvokeSupportFlags.CallSite; }
		}

		#endregion Properties
	}
}
