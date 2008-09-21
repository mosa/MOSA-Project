/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 *  Simon Wollwage (<mailto:rootnode@mosa-project.org>)
 */

using System;
using System.Collections.Generic;
using System.Text;
using Mosa.Platforms.x86.Instructions;
using Mosa.Platforms.x86.Instructions.Intrinsics;
using Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Platforms.x86
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="ArgType">The type of the rg type.</typeparam>
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

        void Cvtsi2ss(Cvtsi2ssInstruction instruction, ArgType arg);
        void Cvtsi2sd(Cvtsi2sdInstruction instruction, ArgType arg);

        #region Intrinsics
        /// <summary>
        /// Disable interrupts
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arguments</param>
        void Cli(CliInstruction instruction, ArgType arg);
        /// <summary>
        /// Clear Direction Flag
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arguments</param>
        void Cld(CldInstruction instruction, ArgType arg);
        /// <summary>
        /// Compare and exchange register - memory
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arguments</param>
        void CmpXchg(CmpXchgInstruction instruction, ArgType arg);
        /// <summary>
        /// Halts the machine
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arguments</param>
        void Hlt(HltInstruction instruction, ArgType arg);
        /// <summary>
        /// Read in from port
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arguments</param>
        void In(InInstruction instruction, ArgType arg);
        /// <summary>
        /// Call interrupt
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arguments</param>
        void Int(IntInstruction instruction, ArgType arg);
        /// <summary>
        /// Return from interrupt
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arguments</param>
        void Iretd(IretdInstruction instruction, ArgType arg);
        /// <summary>
        /// Load global descriptor table
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arguments</param>
        void Lgdt(LgdtInstruction instruction, ArgType arg);
        /// <summary>
        /// Load interrupt descriptor table
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arguments</param>
        void Lidt(LidtInstruction instruction, ArgType arg);
        /// <summary>
        /// Locks
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arguments</param>
        void Lock(LockIntruction instruction, ArgType arg);
        /// <summary>
        /// Output to port
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arguments</param>
        void Out(OutInstruction instruction, ArgType arg);
        /// <summary>
        /// Pause
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arguments</param>
        void Pause(PauseInstruction instruction, ArgType arg);
        /// <summary>
        /// Pop from the stack
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arguments</param>
        void Pop(Instructions.Intrinsics.PopInstruction instruction, ArgType arg);
        /// <summary>
        /// Pops All General-Purpose Registers
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arguments</param>
        void Popad(PopadInstruction instruction, ArgType arg);
        /// <summary>
        /// Pop Stack into EFLAGS Register
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arguments</param>
        void Popfd(PopfdInstruction instruction, ArgType arg);
        /// <summary>
        /// Push on the stack
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arguments</param>
        void Push(Instructions.Intrinsics.PushInstruction instruction, ArgType arg);
        /// <summary>
        /// Push All General-Purpose Registers
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arguments</param>
        void Pushad(PushadInstruction instruction, ArgType arg);
        /// <summary>
        /// Push EFLAGS Register onto the Stack
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arguments</param>
        void Pushfd(PushfdInstruction instruction, ArgType arg);
        /// <summary>
        /// Rdpmc
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arguments</param>
        void Rdpmc(RdpmcInstruction instruction, ArgType arg);
        /// <summary>
        /// Read time stamp counter
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arguments</param>
        void Rdtsc(RdtscInstruction instruction, ArgType arg);
        /// <summary>
        /// Repeat String Operation Prefix
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arguments</param>
        void Rep(RepInstruction instruction, ArgType arg);
        /// <summary>
        /// Enable interrupts
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arguments</param>
        void Sti(StiInstruction instruction, ArgType arg);
        /// <summary>
        /// Store String
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arguments</param>
        void Stosb(StosbInstruction instruction, ArgType arg);
        /// <summary>
        /// Store String
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arguments</param>
        void Stosd(StosdInstruction instruction, ArgType arg);
        /// <summary>
        /// Exchanges register/memory
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arguments</param>
        void Xchg(XchgInstruction instruction, ArgType arg);
        #endregion
    }
}
