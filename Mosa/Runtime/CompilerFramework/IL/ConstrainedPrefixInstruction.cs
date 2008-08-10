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
    public class ConstrainedPrefixInstruction : PrefixInstruction
    {
        #region Data members

        /// <summary>
        /// The constrained type.
        /// </summary>
        private SigType _constraint;

        #endregion // Data members

        #region Construction

        public ConstrainedPrefixInstruction(OpCode code)
            : base(code)
        {
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

        public override void Decode(IInstructionDecoder decoder)
        {
            // Allow the base class to decode
            base.Decode(decoder);

            // Retrieve the type token
            TokenTypes token = decoder.DecodeToken();
            throw new NotImplementedException();
/*
            _constraint = MetadataTypeReference.FromToken(decoder.Metadata, token);
            Debug.Assert(null != _constraint);
 */
        }

        #endregion // Methods
    }
}
