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
    /// Intermediate representation of the ldtoken IL instruction.
    /// </summary>
    public class LdtokenInstruction : LoadInstruction
    {
        #region Data members

        /// <summary>
        /// The token looked for.
        /// </summary>
        private TokenTypes _token;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="LdtokenInstruction"/>.
        /// </summary>
        /// <param name="code">The opcode, which must be ldtoken.</param>
        public LdtokenInstruction(OpCode code)
            : base(code)
        {
            Debug.Assert(OpCode.Ldtoken == code);
            if (OpCode.Ldtoken != code)
                throw new ArgumentException(@"Invalid opcode.", @"code");
        }

        #endregion // Construction

        #region Methods

        public sealed override void Decode(IInstructionDecoder decoder)
        {
            // Decode bases first
            base.Decode(decoder);

            // Retrieve the token from the code stream
            _token = decoder.DecodeToken();
            throw new NotImplementedException();
/*
            TypeReference typeRef;

            // Determine the result type...
            switch (TokenTypes.TableMask & _token)
            {
                case TokenTypes.TypeDef:
                    n = @"RuntimeTypeHandle";
                    break;

                case TokenTypes.TypeRef:
                    n = @"RuntimeTypeHandle";
                    break;

                case TokenTypes.TypeSpec:
                    n = @"RuntimeTypeHandle";
                    break;

                case TokenTypes.MethodDef:
                    n = @"RuntimeMethodHandle";
                    break;

                case TokenTypes.MemberRef:
                    // Field or Method
                    {
                        MemberReference memberRef = MetadataMemberReference.FromToken(decoder.Metadata, _token);
                        MemberDefinition memberDef = memberRef.Resolve();
                        if (memberDef is MethodDefinition)
                        {
                            n = @"RuntimeMethodHandle";
                        }
                        else if (memberDef is FieldDefinition)
                        {
                            n = @"RuntimeFieldHandle";
                        }
                        else
                        {
                            Debug.Assert(false, @"Failed to determine member reference type in ldtoken.");
                            throw new InvalidOperationException();
                        }
                    }
                    break;

                case TokenTypes.MethodSpec:
                    n = @"RuntimeMethodHandle";
                    break;

                case TokenTypes.Field:
                    n = @"RuntimeFieldHandle";
                    break;

                default:
                    throw new NotImplementedException();
            }

            typeRef = MetadataTypeReference.FromName(decoder.Metadata, @"System", n);
            if (null == typeRef)
                typeRef = MetadataTypeDefinition.FromName(decoder.Metadata, @"System", n);

            // Set the result
            Debug.Assert(null != typeRef, @"ldtoken: Failed to retrieve type reference.");
            _results[0] = CreateResultOperand(typeRef);
 */
        }

        public override string ToString()
        {
            return String.Format("{0} = ldtoken({1})", this.Results[0], _token);
        }

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        public sealed override void Visit<ArgType>(IILVisitor<ArgType> visitor, ArgType arg)
        {
            visitor.Ldtoken(this, arg);
        }

        #endregion // Methods
    }
}
