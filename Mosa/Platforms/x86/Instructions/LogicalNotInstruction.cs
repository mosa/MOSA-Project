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
    sealed class LogicalNotInstruction : IR.LogicalNotInstruction
    {
        #region Construction

        public LogicalNotInstruction()
        {
        }

        public LogicalNotInstruction(Operand result, Operand op1) :
            base(result, op1)
        {
        }

        #endregion // Construction

        #region IR.LogicalOrInstruction Overrides

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
