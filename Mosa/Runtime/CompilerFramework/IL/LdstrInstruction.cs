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
    public class LdstrInstruction : LoadInstruction
    {
        #region Data members

        /// <summary>
        /// The string value, which is loaded.
        /// </summary>
        private string _value;

        #endregion // Data members

        #region Construction

        public LdstrInstruction(OpCode code)
            : base(code)
        {
            Debug.Assert(OpCode.Ldstr == code);
            if (OpCode.Ldstr != code)
                throw new ArgumentException(@"Invalid opcode.");
        }

        #endregion // Construction

        #region Methods

        /// <summary>
        /// Initializes the arithmetic instruction.
        /// </summary>
        /// <param name="decoder">The decoder to initialize from.</param>
        public override void Decode(IInstructionDecoder decoder)
        {
            // Decode base classes first
            base.Decode(decoder);

            // Load the string value, it's a token
            IMetadataProvider metadata = decoder.Metadata;
            TokenTypes token = TokenTypes.UserString | decoder.DecodeToken();
            metadata.Read(token, out _value);

            // Set the result
            SetResult(0, CreateResultOperand(decoder.Architecture, new SigType(CilElementType.String)));
        }

        public override string ToString()
        {
            return String.Format("{0} = \"{1}\"", this.Results[0], _value);
        }

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        public sealed override void Visit<ArgType>(IILVisitor<ArgType> visitor, ArgType arg)
        {
            visitor.Ldstr(this, arg);
        }

        #endregion // Methods
    }
}
