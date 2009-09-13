/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Mosa.Runtime.CompilerFramework.CIL
{
	/// <summary>
	/// 
	/// </summary>
	public class CILInstruction : ICILInstruction
	{
		#region Data members

		/// <summary>
		/// Holds the CIL opcode
		/// </summary>
		protected OpCode _opcode;

		/// <summary>
		/// Holds the number of operands for this instruction.
		/// </summary>
		public int _operandCount;

		/// <summary>
		/// Holds the number of operand results for this instruction.
		/// </summary>
		public int _resultCount;

		#endregion // Data members

		#region Properties

		/// <summary>
		/// Gets the operand count of the instruction
		/// </summary>
		/// <value>The operand count.</value>
		public int OperandCount { get { return _operandCount; } }

		/// <summary>
		/// Gets the result operand count of the instruction
		/// </summary>
		/// <value>The operand result count.</value>
		public int ResultsCount { get { return _resultCount; } }

		#endregion // Properties

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="CILInstruction"/> class.
		/// </summary>
		/// <param name="opCode">The op code.</param>
		public CILInstruction(OpCode opCode)
		{
			this._opcode = opCode;
			_operandCount = 0;
			_resultCount = 0;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CILInstruction"/> class.
		/// </summary>
		/// <param name="opCode">The op code.</param>
		/// <param name="operandCount">The operand count.</param>
		public CILInstruction(OpCode opCode, int operandCount)
		{
			this._opcode = opCode;
			_operandCount = operandCount;
			_resultCount = 0;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CILInstruction"/> class.
		/// </summary>
		/// <param name="opCode">The op code.</param>
		/// <param name="operandCount">The operand count.</param>
		/// <param name="resultCount">The result count.</param>
		public CILInstruction(OpCode opCode, int operandCount, int resultCount)
		{
			this._opcode = opCode;
			_operandCount = operandCount;
			_resultCount = resultCount;
		}

		#endregion // Construction

		#region ICILInstruction Overrides

		/// <summary>
		/// Decodes the specified instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// 
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		public virtual void Decode(ref InstructionData instruction, IInstructionDecoder decoder)
		{
			instruction.Instruction = this;
		}

		/// <summary>
		/// Validates the specified instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="compiler">The compiler.</param>
		public virtual void Validate(ref InstructionData instruction, IMethodCompiler compiler)
		{
			/* Default implementation is to do nothing */
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public virtual string ToString(ref InstructionData instruction)
		{
			return ToString();
		}

		#endregion // ICILInstruction Overrides

		#region  Overrides

		/// <summary>
		/// Returns a string representation of <see cref="ConstantOperand"/>.
		/// </summary>
		/// <returns>A string representation of the operand.</returns>
		public override string ToString()
		{
			return "CIL: " + _opcode.ToString();
		}

		#endregion //  Overrides
	}
}
