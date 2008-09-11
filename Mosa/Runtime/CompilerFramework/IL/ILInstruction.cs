/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Text;
using Mosa.Runtime.Metadata;
using System.Diagnostics;
//using Mosa.Runtime.Metadata.Signatures;


namespace Mosa.Runtime.CompilerFramework.IL
{
	/// <summary>
	/// Represents an instruction in internal representation.
	/// </summary>
	/// <remarks>
	/// During compilation of a method its CIL code is transformed into
	/// a simpler internal representation consisting of instances of this
	/// structure. The internal representation is a very simple, but strongly
	/// typed three address code. All optimizations are done using this 
	/// internal representation, which is emitted to machine specific native
	/// code in the final compilation phase.
	/// <para/>
	/// Three address code instructions ussually take the form of a = b op c. This
	/// class represents a by Destination, b by First, c by Second and op by Code. The
	/// use of the three operands is specific to the IR code used and documented
	/// with the operand.
	/// </remarks>
	public abstract class ILInstruction : Instruction
    {
        #region Data members

        /// <summary>
        /// Stores the opcode of the instruction.
        /// </summary>
		protected OpCode _code;

        /// <summary>
        /// Holds the prefix of the instruction.
        /// </summary>
        protected PrefixInstruction _prefix;

        #endregion // Data members

		#region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="ILInstruction"/>.
        /// </summary>
        /// <param name="op">The opcode of the instruction.</param>
		protected ILInstruction(OpCode op)
		{
			_code = op;
		}

        protected ILInstruction(OpCode op, int operandCount) : 
            base(operandCount)
        {
            _code = op;
        }

        protected ILInstruction(OpCode op, int operandCount, int resultCount) :
            base(operandCount, resultCount)
        {
            _code = op;
        }
        
        #endregion // Construction

		#region Properties

        /// <summary>
        /// Retrieves the opcode of the instruction.
        /// </summary>
		public OpCode Code 
        { 
            get { return _code; } 
        }

        /// <summary>
        /// Gets/Sets the prefix instruction of the instruction.
        /// </summary>
        public PrefixInstruction Prefix
        {
            get { return _prefix; }
            set { _prefix = value; }
        }

        /// <summary>
        /// Determines if the IL decoder pushes the results of this instruction onto the IL operand stack.
        /// </summary>
        /// <remarks>
        /// Some instructions do have a result in three-address code, such as the Store and Load 
        /// instructions, where the target is a result. However these instructions may not modify the stack.
        /// In the case that no stack modification is wished, this property should be overriden to indicate
        /// that.
        /// </remarks>
        public virtual bool PushResult
        {
            get { return true; }
        }

        #endregion // Properties

        #region Methods

        /// <summary>
        /// Allows the instruction to decode any immediate operands.
        /// </summary>
        /// <param name="decoder">The instruction decoder, which holds the code stream.</param>
        /// <remarks>
        /// This method is used by instructions to retrieve immediate operands
        /// from the instruction stream.
        /// </remarks>
        public virtual void Decode(IInstructionDecoder decoder)
        {
            /* Default implementation is to do nothing */
        }

        #endregion // Methods

        #region Instruction Overrides

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        public override void Visit<ArgType>(IInstructionVisitor<ArgType> visitor, ArgType arg)
        {
            IILVisitor<ArgType> ilv = visitor as IILVisitor<ArgType>;
            Debug.Assert(null != ilv);
            if (null != ilv)
                Visit(ilv, arg);
            else
                base.Visit(visitor, arg);
        }

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        public abstract void Visit<ArgType>(IILVisitor<ArgType> visitor, ArgType arg);

        #endregion // Instruction Overrides

        #region Object overrides

        /// <summary>
        /// Returns a formatted representation of the opcode.
        /// </summary>
        /// <returns>The code as a string value.</returns>
        public override string ToString()
        {
            return "IL " + _code.ToString().ToLower().Replace('_', '.');
        }

        #endregion // Object overrides
    }
}
