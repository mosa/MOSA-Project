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
    public class NoPrefixInstruction : PrefixInstruction
    {
        #region Data members

        private byte _noCheck;

        #endregion // Data members

        #region Construction

        public NoPrefixInstruction(OpCode code)
            : base(code)
        {
        }

        #endregion // Construction

        #region Properties

        public byte NoCheck
        {
            get { return _noCheck; }
        }

        #endregion // Properties

        #region Methods

        public override void Decode(IInstructionDecoder decoder)
        {
            // Decode base class
            base.Decode(decoder);

            // Read the no check code
            _noCheck = decoder.DecodeByte();
        }

        #endregion // Methods
    }
}
