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
    /// <summary>
    /// 
    /// </summary>
    public class ConstrainedPrefixInstruction : PrefixInstruction
    {
        #region Data members

        /// <summary>
        /// The constrained type.
        /// </summary>
        private SigType _constraint;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstrainedPrefixInstruction"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        public ConstrainedPrefixInstruction(OpCode code)
            : base(code)
        {
            _constraint = null;
            _constraint = Constraint;
            Debug.Assert(OpCode.PreConstrained == code);
            if (OpCode.PreConstrained != code)
                throw new ArgumentException(@"code");
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Retrieves the constraint of this instruction.
        /// </summary>
        public SigType Constraint
        {
            get { return _constraint; }
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
            // Allow the base class to decode
            base.Decode(decoder);

            // Retrieve the type token
            TokenTypes token;
            decoder.Decode(out token);
            throw new NotImplementedException();
/*
            _constraint = MetadataTypeReference.FromToken(decoder.Metadata, token);
            Debug.Assert(null != _constraint);
 */
        }

        #endregion // Methods
    }
}
