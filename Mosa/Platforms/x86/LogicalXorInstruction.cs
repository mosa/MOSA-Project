/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:simon_wollwage@yahoo.co.jp>)
 */

using System;
using System.Collections.Generic;
using System.Text;

using Mosa.Runtime.CompilerFramework;
using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Platforms.x86
{
    sealed class LogicalXorInstruction : IR.LogicalXorInstruction
    {
        #region Construction

        public LogicalXorInstruction()
        {
        }

        public LogicalXorInstruction(Operand result, Operand op1, Operand op2) :
            base(result, op1, op2)
        {
        }

        #endregion // Construction

        #region IR.LogicalOrInstruction Overrides

        public override object Expand(MethodCompilerBase methodCompiler)
        {
            // Three -> Two conversion
            IArchitecture arch = methodCompiler.Architecture;
            RegisterOperand eax = new RegisterOperand(this.Destination.Type, GeneralPurposeRegister.EAX);
            Operand op1 = this.Operand1;
            this.Operand1 = eax;

            return new Instruction[] {
                arch.CreateInstruction(typeof(IR.MoveInstruction), eax, op1),
                this,
                arch.CreateInstruction(typeof(IR.MoveInstruction), this.Destination, eax)
            };
        }

        #endregion // IR.LogicalXorInstruction Overrides
    }
}
