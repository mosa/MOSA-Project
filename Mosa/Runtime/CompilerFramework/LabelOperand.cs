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
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// An operand, which represents a label in the program data.
    /// </summary>
    public class LabelOperand : MemoryOperand
    {
        #region Data members

        /// <summary>
        /// Holds the label.
        /// </summary>
        private int _label;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="LabelOperand"/>.
        /// </summary>
        /// <param name="type">The signature type of the operand data.</param>
        /// <param name="baseRegister">The base register for label offsets.</param>
        /// <param name="offset">The default offset from the base register.</param>
        /// <param name="label">The additional offset as indicated by a label.</param>
        public LabelOperand(SigType type, Register baseRegister, int offset, int label) :
            base(type, baseRegister, new IntPtr(offset))
        {
            _label = label;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets the label of the operand.
        /// </summary>
        /// <value>The label.</value>
        public int Label
        {
            get { return _label; }
        }

        #endregion // Properties

        #region Object Overrides

        /// <summary>
        /// Returns a string representation of <see cref="Operand"/>.
        /// </summary>
        /// <returns>A string representation of the operand.</returns>
        public override string ToString()
        {
            return base.ToString().Replace("[", String.Format("[L_{0}, ", _label));
        }

        #endregion // Object Overrides
    }
}
