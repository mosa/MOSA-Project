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
using System.Diagnostics;

namespace Mosa.Runtime.CompilerFramework.IL
{
    public class UnalignedPrefixInstruction : PrefixInstruction
    {
        #region Data members

        /// <summary>
        /// Alignment value.
        /// </summary>
        private byte _alignment;

        #endregion // Data members

        #region Construction

        public UnalignedPrefixInstruction(OpCode code)
            : base(code)
        {
            Debug.Assert(OpCode.PreUnaligned == code);
            if (OpCode.PreUnaligned != code)
                throw new ArgumentException(@"code");
        }

        #endregion // Construction

        #region Properties

        public byte Alignment
        {
            get { return _alignment; }
        }

        #endregion // Properties

        #region Methods

        public override void Decode(IInstructionDecoder decoder)
        {
            base.Decode(decoder);
            _alignment = decoder.DecodeByte();
        }

        #endregion // Methods
    }
}
