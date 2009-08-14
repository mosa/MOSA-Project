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
    /// <summary>
    /// 
    /// </summary>
    public class LdftnInstruction : LoadInstruction
    {
        #region Data members

        /// <summary>
        /// The function loaded.
        /// </summary>
        private RuntimeMethod _function;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="LdftnInstruction"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        public LdftnInstruction(OpCode code)
            : base(code)
        {
            _function = Function;
            Debug.Assert(OpCode.Ldftn == code);
            if (OpCode.Ldftn != code)
                throw new ArgumentException(@"Invalid opcode.", @"code");
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets the function.
        /// </summary>
        /// <value>The function.</value>
        public RuntimeMethod Function
        {
            get { return _function; }
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
            // Decode the base class
            base.Decode(decoder);

            // Read the fn token
            TokenTypes token;
            decoder.Decode(out token);
            throw new NotImplementedException();
/*
            _function = MetadataMemberReference.FromToken(decoder.Metadata, token);

            // Setup the result
            _results[0] = CreateResultOperand(NativeTypeReference.NativeInt);
 */
        }

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        public sealed override void Visit<ArgType>(IILVisitor<ArgType> visitor, ArgType arg)
        {
            visitor.Ldftn(this, arg);
        }

        #endregion // Methods
    }
}
