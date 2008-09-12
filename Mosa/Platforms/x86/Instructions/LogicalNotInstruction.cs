/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:rootnode@mosa-project.org>)
 */

using System;
using System.Collections.Generic;
using System.Text;

using Mosa.Runtime.CompilerFramework;
using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Platforms.x86
{
    /// <summary>
    /// 
    /// </summary>
    sealed class LogicalNotInstruction : IR.LogicalNotInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicalNotInstruction"/> class.
        /// </summary>
        public LogicalNotInstruction()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicalNotInstruction"/> class.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="op1">The op1.</param>
        public LogicalNotInstruction(Operand result, Operand op1) :
            base(result, op1)
        {
        }

        #endregion // Construction

        #region IR.LogicalOrInstruction Overrides

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
            // Three -> Two conversion
            IArchitecture arch = methodCompiler.Architecture;
            RegisterOperand eax = new RegisterOperand(this.Operand0.Type, GeneralPurposeRegister.EAX);
            Operand op1 = this.Operand1;
            this.Operand1 = eax;

            return new Instruction[] {
                arch.CreateInstruction(typeof(IR.MoveInstruction), eax, op1),
                this,
                arch.CreateInstruction(typeof(IR.MoveInstruction), this.Operand0, eax)
            };
        }

        #endregion // IR.LogicalAndInstruction Overrides
    }
}
