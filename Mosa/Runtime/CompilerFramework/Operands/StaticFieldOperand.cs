/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.Text;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Vm;

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// 
    /// </summary>
    public class StaticFieldOperand : MemoryOperand
    {
        #region Data members

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="StaticFieldOperand"/> class.
        /// </summary>
        /// <param name="field">The field.</param>
        public StaticFieldOperand(RuntimeField field) :
            base(field.Type, null, field.Address)
        {
        }

        #endregion // Construction

        #region MemoryOperand Overrides

        /// <summary>
        /// Returns a string representation of <see cref="Operand"/>.
        /// </summary>
        /// <returns>A string representation of the operand.</returns>
        public override string ToString()
        {
            return base.ToString();
        }

        #endregion // MemoryOperand Overrides
    }
}
