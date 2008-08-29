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

namespace Mosa.Runtime.CompilerFramework.IL
{
    /// <summary>
    /// Intermediate representation of the newarr IL instruction.
    /// </summary>
    public class NewarrInstruction : UnaryInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="NewarrInstruction"/>.
        /// </summary>
        /// <param name="code">The opcode, which must be OpCode.Newarr.</param>
        public NewarrInstruction(OpCode code)
            : base(code, 1)
        {
            Debug.Assert(OpCode.Newarr == code);
            if (OpCode.Newarr != code)
                throw new ArgumentException(@"Invalid opcode.", @"code");
        }

        #endregion // Construction

        #region Methods

        public override void Decode(IInstructionDecoder decoder)
        {
            // Decode the base first
            base.Decode(decoder);

            // Read the type specification
            TokenTypes arrayEType;
            decoder.Decode(out arrayEType);
            throw new NotImplementedException();
/*
            TypeReference eType = MetadataTypeReference.FromToken(decoder.Metadata, arrayEType);

            // FIXME: If _operands[0] is an integral constant, we can infer the maximum size of the array
            // and instantiate an ArrayTypeSpecification with max. sizes. This way we could eliminate bounds
            // checks in an optimization stage later on, if we find that a value never exceeds the array 
            // bounds.

            // Build a type specification
            ArrayTypeSpecification typeRef = new ArrayTypeSpecification(eType);
            _results[0] = CreateResultOperand(typeRef);
 */ 
        }

        public override string ToString()
        {
            throw new NotImplementedException();
//            TypeSpecification typeSpec = (TypeSpecification)_results[0].Type;
//            return String.Format(@"{0} = new {1}[{2}]", _results[0], typeSpec.ElementType, _operands[0]);
        }

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        public sealed override void Visit<ArgType>(IILVisitor<ArgType> visitor, ArgType arg)
        {
            visitor.Newarr(this, arg);
        }

        #endregion // Methods
    }
}
