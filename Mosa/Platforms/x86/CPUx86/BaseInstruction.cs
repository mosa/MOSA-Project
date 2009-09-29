/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
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
		protected BaseInstruction()
		{
			_operandDefaultCount = 0;
			_resultDefaultCount = 0;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseInstruction"/> class.
		/// </summary>
		/// <param name="operandCount">The operand count.</param>
		private BaseInstruction(byte operandCount)
			: this()
		{
			_operandDefaultCount = operandCount;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseInstruction"/> class.
		/// </summary>
		/// <param name="operandCount">The operand count.</param>
		/// <param name="resultCount">The result count.</param>
		protected BaseInstruction(byte operandCount, byte resultCount)
			: this(operandCount)
		{
			_resultDefaultCount = resultCount;
		}

		#endregion // Construction

		#region IPlatformInstruction Overrides

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstOperand"></param>
        /// <param name="secondOperand"></param>
        /// <param name="thirdOperand"></param>
        /// <returns></returns>
	    protected virtual OpCode ComputeOpCode(Operand firstOperand, Operand secondOperand, Operand thirdOperand)
	    {
	        throw new NotSupportedException();
	    }

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="codeStream">The code stream.</param>
		public virtual void Emit(Context ctx, System.IO.Stream codeStream)
		{
		    OpCode opCode = ComputeOpCode(ctx.Result, ctx.Operand1, ctx.Operand2);
            MachineCodeEmitter.Emit(codeStream, opCode, ctx.Result, ctx.Operand1, ctx.Operand2);
		}

		/// <summary>
		/// Gets the instruction latency.
		/// </summary>
		/// <value>The latency.</value>
		public virtual int Latency { get { return -1; } }

		#endregion // IPlatformInstruction Overrides

		#region Operand Overrides

		/// <summary>
		/// Returns a string representation of <see cref="ConstantOperand"/>.
		/// </summary>
		/// <returns>A string representation of the operand.</returns>
		public override string ToString()
		{
			return "X86." + GetType();
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

        #region Typesizes
        /// <summary>
        /// Check if the given operand is an unsigned byte
        /// </summary>
        /// <param name="operand">The operand to check</param>
        /// <returns>True if it is an unsigned byte</returns>
        public static bool IsUnsignedByte(Operand operand)
        {
            return (operand.Type.Type == Runtime.Metadata.CilElementType.U1);
        }

        /// <summary>
        /// Check if the given operand is a signed byte
        /// </summary>
        /// <param name="operand">The operand to check</param>
        /// <returns>True if it is a signed byte</returns>
        public static bool IsSignedByte(Operand operand)
        {
            return (operand.Type.Type == Runtime.Metadata.CilElementType.I1);
        }

        /// <summary>
        /// Check if the given operand is an unsigned short
        /// </summary>
        /// <param name="operand">The operand to check</param>
        /// <returns>True if it is an unsigned short</returns>
        public static bool IsUnsignedShort(Operand operand)
        {
            return (operand.Type.Type == Runtime.Metadata.CilElementType.U2);
        }

        /// <summary>
        /// Check if the given operand is a signed short
        /// </summary>
        /// <param name="operand">The operand to check</param>
        /// <returns>True if it is a signed short</returns>
        public static bool IsSignedShort(Operand operand)
        {
            return (operand.Type.Type == Runtime.Metadata.CilElementType.I2);
        }

        /// <summary>
        /// Check if the given operand is an unsigned integer
        /// </summary>
        /// <param name="operand">The operand to check</param>
        /// <returns>True if it is an unsigned integer</returns>
        public static bool IsUnsignedInt(Operand operand)
        {
            return (operand.Type.Type == Runtime.Metadata.CilElementType.U4);
        }

        /// <summary>
        /// Check if the given operand is a signed integer
        /// </summary>
        /// <param name="operand">The operand to check</param>
        /// <returns>True if it is a signed integer</returns>
        public static bool IsSignedInt(Operand operand)
        {
            return (operand.Type.Type == Runtime.Metadata.CilElementType.I4);
        }

        /// <summary>
        /// Check if the given operand is an unsigned long
        /// </summary>
        /// <param name="operand">The operand to check</param>
        /// <returns>True if it is an unsigned long</returns>
        public static bool IsUnsignedLong(Operand operand)
        {
            return (operand.Type.Type == Runtime.Metadata.CilElementType.U8);
        }

        /// <summary>
        /// Check if the given operand is a signed long
        /// </summary>
        /// <param name="operand">The operand to check</param>
        /// <returns>True if it is a signed long</returns>
        public static bool IsSignedLong(Operand operand)
        {
            return (operand.Type.Type == Runtime.Metadata.CilElementType.I8);
        }

        /// <summary>
        /// Check if the given operand is a byte
        /// </summary>
        /// <param name="operand">The operand to check</param>
        /// <returns>True if it is a byte</returns>
        public static bool IsByte(Operand operand)
        {
            return IsUnsignedByte(operand) || IsSignedByte(operand);
        }

        /// <summary>
        /// Check if the given operand is a short
        /// </summary>
        /// <param name="operand">The operand to check</param>
        /// <returns>True if it is a short</returns>
        public static bool IsShort(Operand operand)
        {
            return IsUnsignedShort(operand) || IsSignedShort(operand);
        }

        /// <summary>
        /// Check if the given operand is a char
        /// </summary>
        /// <param name="operand">The operand to check</param>
        /// <returns>True if it is a char</returns>
        public static bool IsChar(Operand operand)
        {
            return operand.Type.Type == Runtime.Metadata.CilElementType.Char;
        }

        /// <summary>
        /// Check if the given operand is an integer
        /// </summary>
        /// <param name="operand">The operand to check</param>
        /// <returns>True if it is an integer</returns>
        public static bool IsInt(Operand operand)
        {
            return IsUnsignedInt(operand) || IsSignedInt(operand);
        }

        /// <summary>
        /// Check if the given operand is a long
        /// </summary>
        /// <param name="operand">The operand to check</param>
        /// <returns>True if it is a long</returns>
        public static bool IsLong(Operand operand)
        {
            return IsUnsignedLong(operand) || IsSignedLong(operand);
        }
        #endregion
    }
}
