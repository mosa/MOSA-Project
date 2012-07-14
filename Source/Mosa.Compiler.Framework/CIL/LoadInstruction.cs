/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Michael Fröhlich (aka grover, Michael Ruck) <sharpos@michaelruck.de>
 *  
 */

using Mosa.Compiler.Metadata.Signatures;

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

		#endregion // Construction

		public static Operand CreateResultOperand(IInstructionDecoder decoder, StackTypeCode operandType, SigType operandSigType)
		{
			if (operandType == StackTypeCode.O || operandType == StackTypeCode.Ptr || operandType == StackTypeCode.F)
			{
				return decoder.Compiler.CreateVirtualRegister(operandSigType);
			}
			else
			{
				return decoder.Compiler.CreateVirtualRegister(Operand.SigTypeFromStackType(operandType));
			}
		}
	}
}
