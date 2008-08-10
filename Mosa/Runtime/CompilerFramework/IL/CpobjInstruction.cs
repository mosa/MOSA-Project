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
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.CompilerFramework.IL
{
    public class CpobjInstruction : BinaryInstruction
    {
        #region Data members

        /// <summary>
        /// The verification token type.
        /// </summary>
        protected SigType _typeTok;

        #endregion // Data members

        #region Construction

        public CpobjInstruction(OpCode code)
            : base(code)
        {
            Debug.Assert(OpCode.Cpobj == code);
            if (OpCode.Cpobj != code)
                throw new ArgumentException(@"Invalid opcode.", @"code");
        }

        #endregion // Construction

        #region Properties

        public SigType TypeToken
        {
            get { return _typeTok; }
        }

        #endregion // Properties

        #region Methods

        public override void Decode(IInstructionDecoder decoder)
        {
            // Decode base first, it retrieves the
            // stack operands.
            base.Decode(decoder);

            // FIXME: Perform static assignment compatibility
            // verification on the three types.
        }

        public override string ToString()
        {
            Operand[] ops = this.Operands;
            return String.Format("{2} ; *{0} = *{1}", ops[0], ops[1], base.ToString());
        }

        public sealed override void Visit(IILVisitor visitor)
        {
            visitor.Cpobj(this);
        }

        #endregion // Methods
    }
}
