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
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Vm;

namespace Mosa.Runtime.CompilerFramework.IL
{
    public class LdftnInstruction : LoadInstruction
    {
        #region Data members

        /// <summary>
        /// The function loaded.
        /// </summary>
        private RuntimeMethod _function;

        #endregion // Data members

        #region Construction

        public LdftnInstruction(OpCode code)
            : base(code)
        {
            Debug.Assert(OpCode.Ldftn == code);
            if (OpCode.Ldftn != code)
                throw new ArgumentException(@"Invalid opcode.", @"code");
        }

        #endregion // Construction

        #region Properties

        public RuntimeMethod Function
        {
            get { return _function; }
        }

        #endregion // Properties

        #region Methods

        public override void Decode(IInstructionDecoder decoder)
        {
            // Decode the base class
            base.Decode(decoder);

            // Read the fn token
            TokenTypes token = decoder.DecodeToken();
            throw new NotImplementedException();
/*
            _function = MetadataMemberReference.FromToken(decoder.Metadata, token);

            // Setup the result
            _results[0] = CreateResultOperand(NativeTypeReference.NativeInt);
 */
        }

        public sealed override void Visit(IILVisitor visitor)
        {
            visitor.Ldftn(this);
        }

        #endregion // Methods
    }
}
