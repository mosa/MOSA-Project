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

using Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Platforms.x86
{
    interface IX86InstructionVisitor<ArgType> : IIRVisitor<ArgType>
    {
        void Add(AddInstruction addInstruction, ArgType arg);
        void Adc(AdcInstruction adcInstruction, ArgType arg);
        
        void And(LogicalAndInstruction andInstruction, ArgType arg);
        void Or(LogicalOrInstruction orInstruction, ArgType arg);
        void Xor(LogicalXorInstruction xorInstruction, ArgType arg);

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
        void Ldit(LditInstruction instruction, ArgType arg);

        void Cli(CliInstruction instruction, ArgType arg);
        void Sti(StiInstruction instruction, ArgType arg);
        void Int(IntInstruction instruction, ArgType arg);

        void Call(CallInstruction instruction, ArgType arg);
    }
}
