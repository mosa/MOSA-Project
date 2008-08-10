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
    public class StaticFieldOperand : MemoryOperand
    {
        #region Data members

        #endregion // Data members

        #region Construction

        public StaticFieldOperand(RuntimeField field) :
            base(field.Type, null, field.Address)
        {
        }

        #endregion // Construction

        #region MemoryOperand Overrides

        public override string ToString()
        {
            return base.ToString();
        }

        #endregion // MemoryOperand Overrides
    }
}
