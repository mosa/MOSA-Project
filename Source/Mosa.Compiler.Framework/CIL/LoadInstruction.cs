// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	///
	/// </summary>
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

		public static Operand CreateResultOperand(IInstructionDecoder decoder, MosaType type)
		{
			return decoder.Compiler.CreateVirtualRegister(type.GetStackType());
		}
	}
}
