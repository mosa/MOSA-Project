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
using Mosa.Runtime.Vm;

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// An operand used to reference object data fields.
    /// </summary>
    public class ObjectFieldOperand : MemoryOperand
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectFieldOperand"/> class.
        /// </summary>
        /// <param name="objectInstance">The operand, representing the object instance.</param>
        /// <param name="field">The referenced field.</param>
        public ObjectFieldOperand(Operand objectInstance, RuntimeField field) :
            base(field.Type, null, IntPtr.Zero)
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
