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
using System.Diagnostics;
using System.Text;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.CompilerFramework.IR
{
    /// <summary>
    /// Used to represent labelled literal data in the instruction stream.
    /// </summary>
    public abstract class LiteralInstruction : Instruction
    {
        #region Data members

        /// <summary>
        /// The literal data to embed in the instruction stream.
        /// </summary>
        private object _data;

        /// <summary>
        /// Contains the label to apply to the data.
        /// </summary>
        private int _label;

        /// <summary>
        /// Holds the signature type of the literal data.
        /// </summary>
        private SigType _type;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of LiteralInstruction.
        /// </summary>
        /// <param name="label">The label used to identify the literal in code.</param>
        /// <param name="type">The signature type of the literal data.</param>
        /// <param name="data">The data to embed along with the code stream.</param>
        protected LiteralInstruction(int label, SigType type, object data)
        {
            if (null == type)
                throw new ArgumentNullException(@"type");
            if (null == data)
                throw new ArgumentNullException(@"data");

            _label = label;
            _type = type;
            _data = data;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets an object, that represents the data to embed in the instruction stream.
        /// </summary>
        /// <value>The data.</value>
        public object Data
        {
            get { return _data; }
        }

        /// <summary>
        /// Gets the label to apply to the data.
        /// </summary>
        /// <value>The label.</value>
        public int Label
        {
            get { return _label; }
        }

        /// <summary>
        /// Gets the signature type of the data to embed.
        /// </summary>
        /// <value>The type.</value>
        public SigType Type
        {
            get { return _type; }
        }

        #endregion // Properties

        #region Methods

        /// <summary>
        /// Creates an operand for use in instructions that reference this literal.
        /// </summary>
        /// <returns>An operand used to reference this literal.</returns>
        public abstract Operand CreateOperand();

        #endregion // Methods

        #region Instruction Overrides

        /// <summary>
        /// Gibt einen <see cref="T:System.String"/> zurück, der den aktuellen <see cref="T:System.Object"/> darstellt.
        /// </summary>
        /// <returns>
        /// Ein <see cref="T:System.String"/>, der den aktuellen <see cref="T:System.Object"/> darstellt.
        /// </returns>
        public override string ToString()
        {
            return String.Format("IR literal {0} {1} ; L_{2}", _type, _data, _label);
        }

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        public override void Visit<ArgType>(IInstructionVisitor<ArgType> visitor, ArgType arg)
        {
            IIRVisitor<ArgType> irv = visitor as IIRVisitor<ArgType>;
            if (null == irv)
                throw new ArgumentException(@"Doesn't implement IIRVisitor interface.", @"visitor");

            irv.Visit(this, arg);
        }

        #endregion // Instruction Overrides
    }
}
