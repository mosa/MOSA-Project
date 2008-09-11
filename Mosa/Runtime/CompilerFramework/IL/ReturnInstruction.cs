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
using System.Diagnostics;

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

        /// <summary>
        /// Determines flow behavior of this instruction.
        /// </summary>
        /// <value></value>
        /// <remarks>
        /// Knowledge of control flow is required for correct basic block
        /// building. Any instruction that alters the control flow must override
        /// this property and correctly identify its control flow modifications.
        /// </remarks>
        public override FlowControl FlowControl
        {
            get { return FlowControl.Return; }
        }

        /// <summary>
        /// Allows the instruction to decode any immediate operands.
        /// </summary>
        /// <param name="decoder">The instruction decoder, which holds the code stream.</param>
        /// <remarks>
        /// This method is used by instructions to retrieve immediate operands
        /// from the instruction stream.
        /// </remarks>
        public override void Decode(IInstructionDecoder decoder)
        {
            MethodSignature sig = decoder.Method.Signature;
            if (sig.ReturnType.Type == CilElementType.Void)
            {
                SetOperandCount(0, 0);
                return;
            }

            base.Decode(decoder);
        }

        /// <summary>
        /// Called by the intermediate to machine intermediate representation transformation
        /// to expand compound instructions into their basic instructions.
        /// </summary>
        /// <param name="methodCompiler">The executing method compiler.</param>
        /// <returns>
        /// The default expansion keeps the original instruction by
        /// returning the instruction itself. A derived class may return an
        /// IEnumerable&lt;Instruction&gt; to replace the instruction with a set of other
        /// instructions or null to remove the instruction itself from the stream.
        /// </returns>
        /// <remarks>
        /// If a derived class returns <see cref="Instruction.Empty"/> from this method, the
        /// instruction is essentially removed from the instruction stream.
        /// </remarks>
        public override object Expand(MethodCompilerBase methodCompiler)
        {
            IArchitecture arch = methodCompiler.Architecture;

            // Do we have an operand to return?
            if (0 != this.Operands.Length)
            {
                Debug.Assert(1 == this.Operands.Length, @"Can't return more than one operand.");
                ICallingConvention cc = methodCompiler.Architecture.GetCallingConvention(methodCompiler.Method.Signature.CallingConvention);
                List<Instruction> instructions = new List<Instruction>();
                Instruction[] resmove = cc.MoveReturnValue(arch, this.Operands[0]);
                if (null != resmove)
                    instructions.AddRange(resmove);
                instructions.Add(arch.CreateInstruction(typeof(BranchInstruction), OpCode.Br, new int[] { Int32.MaxValue }));
                return instructions.ToArray();
            }
            else
            {
                // HACK: Should really use the calling convention here
                // A ret jumps to the epilogue to leave
                return arch.CreateInstruction(typeof(BranchInstruction), OpCode.Br, new int[] { Int32.MaxValue });
            }
        }

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        public sealed override void Visit<ArgType>(IILVisitor<ArgType> visitor, ArgType arg)
        {
            visitor.Ret(this, arg);
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
