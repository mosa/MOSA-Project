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
using Mosa.Runtime.CompilerFramework;

namespace Mosa.Platforms.x86.CPUx86
{
	/// <summary>
	/// 
	/// </summary>
	public class BaseInstruction : IPlatformInstruction
	{
		#region Data members

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
		/// Gets the default operand count of the instruction
		/// </summary>
		/// <value>The operand count.</value>
		public byte DefaultOperandCount { get { return _operandDefaultCount; } }

		/// <summary>
		/// Gets the default result operand count of the instruction
		/// </summary>
		/// <value>The operand result count.</value>
		public byte DefaultResultCount { get { return _resultDefaultCount; } }

		#endregion // Properties

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseInstruction"/> class.
		/// </summary>
		public BaseInstruction()
		{
			_operandDefaultCount = 0;
			_resultDefaultCount = 0;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseInstruction"/> class.
		/// </summary>
		/// <param name="operandCount">The operand count.</param>
		public BaseInstruction(byte operandCount)
			: this()
		{
			_operandDefaultCount = operandCount;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseInstruction"/> class.
		/// </summary>
		/// <param name="operandCount">The operand count.</param>
		/// <param name="resultCount">The result count.</param>
		public BaseInstruction(byte operandCount, byte resultCount)
			: this(operandCount)
		{
			_resultDefaultCount = resultCount;
		}

		#endregion // Construction

		#region IPlatformInstruction Overrides

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="codeStream">The code stream.</param>
		public virtual void Emit(ref InstructionData instruction, System.IO.Stream codeStream)
		{
		}

		#endregion // IPlatformInstruction Overrides

		#region Operand Overrides

		/// <summary>
		/// Returns a string representation of <see cref="ConstantOperand"/>.
		/// </summary>
		/// <returns>A string representation of the operand.</returns>
		public override string ToString()
		{
			return "X86." + this.GetType().ToString();
		}

		#endregion // Operand Overrides

		#region Methods

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public virtual string ToString(Context context)
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

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="vistor">The vistor.</param>
		/// <param name="context">The context.</param>
		public virtual void Visit(IX86Visitor vistor, Context context)
		{
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="vistor">The vistor.</param>
		/// <param name="context">The context.</param>
		public void Visit(IVisitor vistor, Context context)
		{
			if (vistor is IX86Visitor)
				Visit(vistor as IX86Visitor, context);
		}

		#endregion //  Overrides
	}
}
