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

using Mosa.Runtime.CompilerFramework;

namespace Mosa.Platforms.x86
{
    interface IX86InstructionVisitor : IInstructionVisitor
    {
        void Add(AddInstruction addInstruction);
        void Sub(SubInstruction subInstruction);
        void Mul(MulInstruction mulInstruction);
        void Div(DivInstruction divInstruction);
        void SseAdd(SseAddInstruction addInstruction);
        void SseSub(SseSubInstruction subInstruction);
        void SseMul(SseMulInstruction mulInstruction);
        void SseDiv(SseDivInstruction mulInstruction);
        void Shift(ShiftInstruction shiftInstruction);
        void Cli(CliInstruction instruction);
        void Ldit(LditInstruction instruction);
        void Sti(StiInstruction instruction);
    }
}
