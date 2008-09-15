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
using Mosa.Platforms.x86.Instructions;
using Mosa.Platforms.x86.Instructions.Intrinsics;
using Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Platforms.x86
{
    interface IX86InstructionVisitor<ArgType> : IIRVisitor<ArgType>
    {
        void Add(AddInstruction addInstruction, ArgType arg);
        void Adc(AdcInstruction adcInstruction, ArgType arg);
        
        void And(Instructions.LogicalAndInstruction andInstruction, ArgType arg);
        void Or(Instructions.LogicalOrInstruction orInstruction, ArgType arg);
        void Xor(Instructions.LogicalXorInstruction xorInstruction, ArgType arg);

        void Sub(SubInstruction subInstruction, ArgType arg);
        void Mul(MulInstruction mulInstruction, ArgType arg);
        void Div(DivInstruction divInstruction, ArgType arg);
        void SseAdd(SseAddInstruction addInstruction, ArgType arg);
        void SseSub(SseSubInstruction subInstruction, ArgType arg);
        void SseMul(SseMulInstruction mulInstruction, ArgType arg);
        void SseDiv(SseDivInstruction mulInstruction, ArgType arg);
        void Sar(SarInstruction shiftInstruction, ArgType arg);
        void Shl(ShlInstruction shiftInstruction, ArgType arg);
        void Shr(ShrInstruction shiftInstruction, ArgType arg);

        void Call(CallInstruction instruction, ArgType arg);

        #region Intrinsics
        void Cli(CliInstruction instruction, ArgType arg);
        void CmpXchg(CmpXchgInstruction instruction, ArgType arg);
        void In(InInstruction instruction, ArgType arg);
        void Int(IntInstruction instruction, ArgType arg);
        void Lgdt(LgdtInstruction instruction, ArgType arg);
        void Lidt(LditInstruction instruction, ArgType arg);
        void Lock(LockIntruction instruction, ArgType arg);
        void Out(OutInstruction instruction, ArgType arg);
        void Pop(Instructions.Intrinsics.PopInstruction instruction, ArgType arg);
        void Push(Instructions.Intrinsics.PushInstruction instruction, ArgType arg);
        void Sti(StiInstruction instruction, ArgType arg);
        void Xchg(XchgInstruction instruction, ArgType arg);
        #endregion
    }
}
