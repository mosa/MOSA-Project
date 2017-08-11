// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// Load Instruction
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.CIL.BaseCILInstruction" />
	public class LoadInstruction : BaseCILInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LoadInstruction"/> class.
		/// </summary>
		/// <param name="opCode">The op code.</param>
		public LoadInstruction(OpCode opCode)
			: base(opCode, 1, 1)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LoadInstruction"/> class.
		/// </summary>
		/// <param name="code">The code.</param>
		/// <param name="operandCount">The number of operands of the load.</param>
		protected LoadInstruction(OpCode code, byte operandCount)
			: base(code, operandCount, 1)
		{
		}

		#endregion Construction
	}
}
