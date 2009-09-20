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
		/// Holds the default number of operands for this instruction.
		/// </summary>
		protected byte _operandDefaultCount;

		/// <summary>
		/// Holds the default number of operand results for this instruction.
		/// </summary>
		protected byte _resultDefaultCount;

		#endregion // Data members

		#region Properties

		/// <summary>
		/// Gets the op code.
		/// </summary>
		/// <value>The op code.</value>
		public OpCode OpCode { get { return _opcode; } }

		/// <summary>
		/// Gets the default operand count of the instruction
		/// </summary>
		/// <value>The operand count.</value>
		protected byte DefaultOperandCount { get { return _operandDefaultCount; } }

		/// <summary>
		/// Gets the default result operand count of the instruction
		/// </summary>
		/// <value>The operand result count.</value>
		protected byte DefaultResultCount { get { return _resultDefaultCount; } }

		#endregion // Properties

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="CILInstruction"/> class.
		/// </summary>
		/// <param name="opCode">The op code.</param>
		public CILInstruction(OpCode opCode)
		{
			this._opcode = opCode;
			_operandDefaultCount = 0;
			_resultDefaultCount = 0;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CILInstruction"/> class.
		/// </summary>
		/// <param name="opCode">The op code.</param>
		/// <param name="operandCount">The operand count.</param>
		public CILInstruction(OpCode opCode, byte operandCount)
			: this(opCode)
		{
			_operandDefaultCount = operandCount;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CILInstruction"/> class.
		/// </summary>
		/// <param name="opCode">The op code.</param>
		/// <param name="operandCount">The operand count.</param>
		/// <param name="resultCount">The result count.</param>
		public CILInstruction(OpCode opCode, byte operandCount, byte resultCount)
			: this(opCode, operandCount)
		{
			_resultDefaultCount = resultCount;
		}

		#endregion // Construction

		#region Methods

		/// <summary>
		/// Decodes the specified instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// 
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		public virtual void Decode(ref InstructionData instruction, IInstructionDecoder decoder)
		{
			instruction.Instruction = this;
			instruction.OperandCount = DefaultOperandCount;
			instruction.ResultCount = DefaultResultCount;
			instruction.Ignore = false;
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

		/// <summary>
		/// Determines flow behavior of this instruction.
		/// </summary>
		/// <remarks>
		/// Knowledge of control flow is required for correct basic block
		/// building. Any instruction that alters the control flow must override
		/// this property and correctly identify its control flow modifications.
		/// </remarks>
		public virtual FlowControl FlowControl
		{
			get { return FlowControl.Next; }
		}

		#endregion Methods

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
