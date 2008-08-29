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
    /// Represents a load of a parameter value.
    /// </summary>
    public class LdargInstruction : LoadInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="LdlocInstruction"/>.
        /// </summary>
        /// <param name="code">The opcode of the load.</param>
        public LdargInstruction(OpCode code)
            : base(code)
        {
            // ParameterOperand loads are stack operations, which are not required
            // in a register based vm.
            _ignore = true;
        }

        #endregion // Construction

        #region Methods

        public override void Decode(IInstructionDecoder decoder)
        {
            ushort argIdx;

            // Opcode specific handling
            switch (_code)
            {
                case OpCode.Ldarg:
                    argIdx = decoder.DecodeUInt16();
                    break;

                case OpCode.Ldarg_s:
                    argIdx = decoder.DecodeByte();
                    break;

                case OpCode.Ldarg_0:
                    argIdx = 0;
                    break;

                case OpCode.Ldarg_1:
                    argIdx = 1;
                    break;

                case OpCode.Ldarg_2:
                    argIdx = 2;
                    break;

                case OpCode.Ldarg_3:
                    argIdx = 3;
                    break;

                default:
                    throw new NotImplementedException();
            }

            // Push the loaded value onto the evaluation stack
            SetResult(0, decoder.GetParameterOperand(argIdx));
        }

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        public sealed override void Visit<ArgType>(IILVisitor<ArgType> visitor, ArgType arg)
        {
            visitor.Ldarg(this, arg);
        }

        #endregion // Methods
    }
}
