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

using Mosa.Runtime.CompilerFramework;
using IR = Mosa.Runtime.CompilerFramework.IR;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Platforms.x86.Instructions
{
    sealed class LiteralInstruction : IR.LiteralInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="LiteralInstruction"/>.
        /// </summary>
        /// <param name="label">The label of the literal.</param>
        /// <param name="type">The literal type.</param>
        /// <param name="data">The literal data.</param>
        public LiteralInstruction(int label, SigType type, object data) :
            base(label, type, data)
        {
        }

        #endregion // Construction

        #region IR.LiteralInstruction Overrides

        public override Operand CreateOperand()
        {
            /* HACK: 
             * Position independent code on x86 requires EIP relative addressing, which
             * unfortunately isn't available. We try to work around this limitation by
             * storing the EIP of the first instruction on the stack, however this isn't
             * enough. So PIC with Literals doesn't work for now.
             */
            //return new LabelOperand(this.Type, GeneralPurposeRegister.EBP, -8, this.Label);
            return new LabelOperand(this.Type, null, 0, this.Label);
        }

        #endregion // IR.LiteralInstruction Overrides
    }
}
