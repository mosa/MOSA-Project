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
    /// <summary>
    /// 
    /// </summary>
    public class UnalignedPrefixInstruction : PrefixInstruction
    {
        #region Data members

        /// <summary>
        /// Alignment value.
        /// </summary>
        private byte _alignment;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="UnalignedPrefixInstruction"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        public UnalignedPrefixInstruction(OpCode code)
            : base(code)
        {
            Debug.Assert(OpCode.PreUnaligned == code);
            if (OpCode.PreUnaligned != code)
                throw new ArgumentException(@"code");
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets the alignment.
        /// </summary>
        /// <value>The alignment.</value>
        public byte Alignment
        {
            get { return _alignment; }
        }

        #endregion // Properties

        #region Methods

        /// <summary>
        /// Allows the instruction to decode any immediate operands.
        /// </summary>
        /// <param name="decoder">The instruction decoder, which holds the code stream.</param>
        /// <remarks>
        /// This method is used by instructions to retrieve immediate operands
        /// From the instruction stream.
        /// </remarks>
        public override void Decode(IInstructionDecoder decoder)
        {
            base.Decode(decoder);
            decoder.Decode(out _alignment);
        }

        #endregion // Methods
    }
}
