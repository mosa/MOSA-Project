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
    /// <summary>
    /// 
    /// </summary>
    public class NoPrefixInstruction : PrefixInstruction
    {
        #region Data members

        /// <summary>
        /// 
        /// </summary>
        private byte _noCheck;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="NoPrefixInstruction"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        public NoPrefixInstruction(OpCode code)
            : base(code)
        {
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets the no check.
        /// </summary>
        /// <value>The no check.</value>
        public byte NoCheck
        {
            get { return _noCheck; }
        }

        #endregion // Properties

        #region Methods

        /// <summary>
        /// Allows the instruction to decode any immediate operands.
        /// </summary>
        /// <param name="decoder">The instruction decoder, which holds the code stream.</param>
        /// <remarks>
        /// This method is used by instructions to retrieve immediate operands
        /// from the instruction stream.
        /// </remarks>
        public override void Decode(IInstructionDecoder decoder)
        {
            // Decode base class
            base.Decode(decoder);

            // Read the no check code
            decoder.Decode(out _noCheck);
        }

        #endregion // Methods
    }
}
