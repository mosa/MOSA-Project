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
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// A temporary stack local operand used to capture results from CIL operations.
    /// </summary>
    /// <remarks>
    /// Temporaries have a stack slot assigned to them however these, see VariableOperand and
    /// ParameterOperand can be optimized by a register allocator and move to registers.
    /// </remarks>
    public sealed class TemporaryOperand : StackOperand
    {
        #region Data members

        /// <summary>
        /// Stores the label of the instruction, that created the temporary operand.
        /// </summary>
        private int _label;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the virtual register.
        /// </summary>
        /// <param name="label">The virtual register number.</param>
        /// <param name="typeRef">The type reference of the virtual register.</param>
        /// <param name="register">The stack base register.</param>
        /// <param name="offset">The offset.</param>
        public TemporaryOperand(int label, SigType typeRef, Register register, int offset)
            : base(typeRef, register, offset)
        {
            _label = label;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Retrieves the id of the virtual register.
        /// </summary>
        public int Id { get { return _label; } }

        #endregion // Properties

        #region Methods

        /// <summary>
        /// Returns a string representation of <see cref="Operand"/>.
        /// </summary>
        /// <returns>A string representation of the operand.</returns>
        public override string ToString()
        {
            return String.Format("L_{0} {1}", _label, base.ToString());
        }

        #endregion // Methods

        #region ICloneable Members

        /// <summary>
        /// Clones an operand.
        /// </summary>
        /// <returns>A new instance of TemporaryOperand.</returns>
        public override object Clone()
        {
            return new TemporaryOperand(_label, _type, base.Base, base.Offset.ToInt32());
        }

        #endregion // ICloneable Members        
    }
}
