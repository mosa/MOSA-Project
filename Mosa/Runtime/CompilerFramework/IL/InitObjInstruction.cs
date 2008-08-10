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
    public class InitObjInstruction : UnaryInstruction
    {
        #region Data members

        /// <summary>
        /// The type reference of the value to initialize.
        /// </summary>
        protected SigType _typeRef;

        #endregion // Data members

        #region Construction

        public InitObjInstruction(OpCode code)
            : base(code)
        {
            Debug.Assert(OpCode.InitObj == code);
            if (OpCode.InitObj != code)
                throw new ArgumentException(@"Invalid opcode.", @"code");
        }

        #endregion // Construction

        #region Properties

        public SigType Type
        {
            get { return _typeRef; }
        }

        #endregion // Properties

        #region Methods

        public override void Decode(IInstructionDecoder decoder)
        {
            // Decode the base class first
            base.Decode(decoder);

            // Retrieve the type reference
            TokenTypes token = decoder.DecodeToken();
            throw new NotImplementedException();
            //_typeRef = MetadataTypeReference.FromToken(decoder.Metadata, token);
        }

        public sealed override void Visit(IILVisitor visitor)
        {
            visitor.InitObj(this);
        }

        #endregion // Methods
    }
}
