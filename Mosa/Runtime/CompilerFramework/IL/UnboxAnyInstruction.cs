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

namespace Mosa.Runtime.CompilerFramework.IL
{
    public class UnboxAnyInstruction : BoxingInstruction
    {
        #region Construction

        public UnboxAnyInstruction(OpCode code)
            : base(code)
        {
        }

        #endregion // Construction

        #region Methods

        public override string ToString()
        {
            return String.Format(@"{2} ; {0} = unbox.any({1})", this.Results[0], this.Operands[0], base.ToString());
        }

        public sealed override void Visit(IILVisitor visitor)
        {
            visitor.UnboxAny(this);
        }

        #endregion // Methods
    }
}
