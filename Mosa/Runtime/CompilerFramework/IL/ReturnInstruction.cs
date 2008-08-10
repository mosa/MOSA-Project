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
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.CompilerFramework.IL
{
    /// <summary>
    /// Intermediate representation of the CIL ret instruction.
    /// </summary>
    public class ReturnInstruction : UnaryInstruction, IBranchInstruction
    {
        #region Static data members

        /// <summary>
        /// Offset of the epilogue block to jump to.
        /// </summary>
        private static readonly int[] s_lastBlock = new int[] { Int32.MaxValue };

        #endregion // Static data members

        #region Data members

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="ReturnInstruction"/>.
        /// </summary>
        /// <param name="code">The opcode of the return instruction.</param>
        public ReturnInstruction(OpCode code)
            : base(code)
        {
            if (OpCode.Ret != code)
                throw new ArgumentException(@"Invalid opcode.", @"code");
        }

        #endregion // Construction

        #region UnaryInstruction overrides

        public override FlowControl FlowControl
        {
            get { return FlowControl.Return; }
        }

        public override void Decode(IInstructionDecoder decoder)
        {
            MethodSignature sig = decoder.Method.Signature;
            if (sig.ReturnType.Type == CilElementType.Void)
            {
                //_operands = ILInstruction.NoOperands;
                return;
            }

            base.Decode(decoder);
        }

        public override object Expand(MethodCompilerBase methodCompiler)
        {
            // A ret jumps to the epilogue to leave
            return methodCompiler.Architecture.CreateInstruction(typeof(BranchInstruction), OpCode.Br, new int[] { Int32.MaxValue });
        }

        public sealed override void Visit(IILVisitor visitor)
        {
            visitor.Ret(this);
        }

        #endregion // UnaryInstruction overrides

        #region IBranchInstruction Members

        bool IBranchInstruction.IsConditional
        {
            get { return false; }
        }

        int[] IBranchInstruction.BranchTargets
        {
            get
            {
                return s_lastBlock;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion // IBranchInstruction Members
    }
}
