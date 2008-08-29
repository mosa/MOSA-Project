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
    public class StlocInstruction : UnaryInstruction, IStoreInstruction
    {
        #region Construction

        public StlocInstruction(OpCode code)
            : base(code, 1)
        {
        }

        public StlocInstruction(Operand target, Operand value) :
            base(OpCode.Stloc, 1)
        {
            SetResult(0, target);
            SetOperand(0, value);
        }

        #endregion // Construction

        #region UnaryInstruction Overrides

        /// <summary>
        /// Stloc has a result, but doesn't push it on the stack.
        /// </summary>
        public override bool PushResult
        {
            get { return false; }
        }

        public override void Decode(IInstructionDecoder decoder)
        {
            ushort locIdx;

            // Decode base classes first
            base.Decode(decoder);

            // Destination depends on the opcode
            switch (_code)
            {
                case OpCode.Stloc:
                    locIdx = decoder.DecodeUInt16();
                    break;

                case OpCode.Stloc_s:
                    locIdx = decoder.DecodeByte();
                    break;

                case OpCode.Stloc_0:
                    locIdx = 0;
                    break;

                case OpCode.Stloc_1:
                    locIdx = 1;
                    break;

                case OpCode.Stloc_2:
                    locIdx = 2;
                    break;

                case OpCode.Stloc_3:
                    locIdx = 3;
                    break;

                default:
                    throw new NotImplementedException();
            }

            SetResult(0, decoder.GetLocalOperand(locIdx));
        }

        public override object Expand(MethodCompilerBase methodCompiler)
        {
            IArchitecture arch = methodCompiler.Architecture;
            return arch.CreateInstruction(typeof(IR.MoveInstruction), this.Results[0], this.Operands[0]);
        }

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        public sealed override void Visit<ArgType>(IILVisitor<ArgType> visitor, ArgType arg)
        {
            visitor.Stloc(this, arg);
        }

        public override string ToString()
        {
            return String.Format("{0} ; {1} = {2}", base.ToString(), this.Results[0], this.Operands[0]);
        }

        #endregion // UnaryInstruction Overrides
    }
}
