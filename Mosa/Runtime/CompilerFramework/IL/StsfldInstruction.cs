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
using System.Diagnostics;

using Mosa.Runtime.Metadata;
using Mosa.Runtime.Vm;

namespace Mosa.Runtime.CompilerFramework.IL
{
    /// <summary>
    /// Intermediate representation of the CIL stsfld operation.
    /// </summary>
    public class StsfldInstruction : UnaryInstruction
    {
        #region Data members

        /// <summary>
        /// Contains the VM representation of the static field to store to.
        /// </summary>
        private RuntimeField field;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="StsfldInstruction"/> class.
        /// </summary>
        /// <param name="code">The opcode of the unary instruction.</param>
        public StsfldInstruction(OpCode code)
            : base(code)
        {
            Debug.Assert(OpCode.Stsfld == code);
            if (OpCode.Stsfld != code)
                throw new ArgumentException(@"Invalid opcode.", @"code");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StsfldInstruction"/> class.
        /// </summary>
        /// <param name="target">The target field.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="target"/> or <paramref name="value"/> is null.</exception>
        public StsfldInstruction(RuntimeField target, Operand value) :
            base(OpCode.Stsfld, 1)
        {
            if (target == null)
                throw new ArgumentNullException(@"target");
            if (value == null)
                throw new ArgumentNullException(@"value");

            this.field = target;
            SetOperand(0, value);
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets the field.
        /// </summary>
        /// <value>The field.</value>
        public RuntimeField Field
        {
            get { return this.field; }
        }

        #endregion // Properties

        #region Methods

        /// <summary>
        /// Allows the instruction to decode any immediate operands.
        /// </summary>
        /// <param name="decoder">The instruction decoder, which holds the code stream.</param>
        /// <remarks>
        /// This method is used by instructions to retrieve immediate operands
        /// From the instruction stream.
        /// </remarks>
        public override void Decode(IInstructionDecoder decoder)
        {
            // Decode the base class
            base.Decode(decoder);

            // Read the field From the code
            TokenTypes token;
            decoder.Decode(out token);
            this.field = RuntimeBase.Instance.TypeLoader.GetField(decoder.Compiler.Assembly, token);
            Debug.Assert((this.field.Attributes & FieldAttributes.Static) == FieldAttributes.Static, @"Static field access on non-static field.");
        }

        /// <summary>
        /// Returns a formatted representation of the opcode.
        /// </summary>
        /// <returns>The code as a string value.</returns>
        public override string ToString()
        {
            return String.Format("IL stsfld ; {0}.{1} = {2}", this.field.DeclaringType.FullName, this.field.Name, this.Operands[0]);
        }

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        public sealed override void Visit<ArgType>(IILVisitor<ArgType> visitor, ArgType arg)
        {
            visitor.Stsfld(this, arg);
        }

        #endregion // Methods
    }
}
