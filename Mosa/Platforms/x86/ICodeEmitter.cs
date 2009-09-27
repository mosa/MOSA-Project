/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 *  Scott Balmos (<mailto:sbalmos@fastmail.fm>)
 */

using System;
using System.Collections.Generic;
using System.Text;

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Vm;
using IR2 = Mosa.Runtime.CompilerFramework.IR2;

namespace Mosa.Platforms.x86
{
    /// <summary>
    /// Interface of x86 code emitters.
    /// </summary>
    /// <remarks>
    /// A code emitter emits the actual code to reflect specific x86 instructions. 
    /// <see cref="MachineCodeEmitter"/> that emits the raw opcode bytes into a stream of the
    /// respective x86 instructions.
    /// </remarks>
    public interface ICodeEmitter : IDisposable
    {
        /// <summary>
        /// Emits a comment into the code stream.
        /// </summary>
        /// <param name="comment">The comment to emit.</param>
        void Comment(string comment);

        /// <summary>
        /// Emits a label into the code stream.
        /// </summary>
        /// <param name="label">The label name to emit.</param>
        void Label(int label);

		/// <summary>
		/// Emits a literal constant into the code stream.
		/// </summary>
		/// <param name="label">The label to apply to the data.</param>
		/// <param name="LiteralData">The literal data.</param>
		void Literal(int label, IR2.LiteralData LiteralData); 

        #region x86 instructions

        /// <summary>
        /// Emits an AND instruction.
        /// </summary>
        /// <param name="dest">The destination operand of the instruction.</param>
        /// <param name="src">The source operand of the instruction.</param>
        void And(Operand dest, Operand src);

        /// <summary>
        /// Emits an Add instruction.
        /// </summary>
        /// <param name="op1">The first operand and destination of the instruction.</param>
        /// <param name="op2">The second operand.</param>
        void Add(Operand op1, Operand op2);

        /// <summary>
        /// Emits an Adc instruction.
        /// </summary>
        /// <param name="op1">The first operand and destination of the instruction.</param>
        /// <param name="op2">The second operand.</param>
        void Adc(Operand op1, Operand op2);

        /// <summary>
        /// Emits a cdq instruction.
        /// </summary>
        void Cdq();

        /// <summary>
        /// Emits a Call instruction
        /// </summary>
        /// <param name="method">The method to be called.</param>
        /// <remarks>
        /// This only invokes the platform call, it does not push arguments, spill and
        /// save registers or handle the return value.
        /// </remarks>
        void Call(RuntimeMethod method);

        /// <summary>
        /// Emits a CALL instruction to the given label.
        /// </summary>
        /// <param name="label">The label to be called.</param>
        /// <remarks>
        /// This only invokes the platform call, it does not push arguments, spill and
        /// save registers or handle the return value.
        /// </remarks>
        void Call(int label);

        /// <summary>
        /// Clears DF flag and EFLAGS
        /// </summary>
        void Cld();

        /// <summary>
        /// Emits a disable interrupts instruction.
        /// </summary>
        void Cli();

        /// <summary>
        /// Emits a comparison instruction.
        /// </summary>
        /// <param name="op1"></param>
        /// <param name="op2"></param>
        void Cmp(Operand op1, Operand op2);

        /// <summary>
        /// Emits an interrupt instruction.
        /// </summary>
        /// <param name="op1">First operand</param>
        /// <param name="op2">Second operand</param>
        void CmpXchg(Operand op1, Operand op2);

        /// <summary>
        /// Performs an ordered comparison of two single-precision floating point operands.
        /// </summary>
        /// <param name="op1">The first fp operand.</param>
        /// <param name="op2">The second fp operand.</param>
        void Comiss(Operand op1, Operand op2);

        /// <summary>
        /// Performs an ordered comparison of two floating point operands.
        /// </summary>
        /// <param name="op1">The first fp operand.</param>
        /// <param name="op2">The second fp operand.</param>
        void Comisd(Operand op1, Operand op2);

        /// <summary>
        /// Performs an unordered comparison of two single-precision floating point operands.
        /// </summary>
        /// <param name="op1">The first fp operand.</param>
        /// <param name="op2">The second fp operand.</param>
        void Ucomiss(Operand op1, Operand op2);

        /// <summary>
        /// Performs an unordered comparison of two floating point operands.
        /// </summary>
        /// <param name="op1">The first fp operand.</param>
        /// <param name="op2">The second fp operand.</param>
        void Ucomisd(Operand op1, Operand op2);
        
        /// <summary>
        /// Retrieves the CPU ID
        /// </summary>
        /// <param name="dest">The destination base memory address</param>
        /// <param name="function">The CPUID function to execute</param>
        void CpuId(Operand dest, Operand function);
        
        /// <summary>
        /// Converts the signed integer to a double precision floating point value.
        /// </summary>
        /// <param name="destination">The destination operand, which receives the converted double precision floating point value.</param>
        /// <param name="operand">The source operand, which holds the signed integer value.</param>
        void Cvtsi2sd(Operand destination, Operand operand);

        /// <summary>
        /// Converts the signed integer to a single precision floating point value.
        /// </summary>
        /// <param name="destination">The destination operand, which receives the converted single precision floating point value.</param>
        /// <param name="operand">The source operand, which holds the signed integer value.</param>
        void Cvtsi2ss(Operand destination, Operand operand);

        /// <summary>
        /// Converts the float to a double precision floating point value.
        /// </summary>
        /// <param name="destination">The destination operand, which receives the converted double precision floating point value.</param>
        /// <param name="operand">The source operand, which holds the floating point value.</param>
        void Cvtss2sd(Operand destination, Operand operand);

        /// <summary>
        /// Converts the double precision floating point value in <paramref name="source"/> to a signed integer and stores it in <paramref name="destination"/>.
        /// </summary>
        /// <param name="destination">The destination operand, which receives the converted signed integer.</param>
        /// <param name="operand">The source operand, which holds the floating point value.</param>
        void Cvttsd2si(Operand destination, Operand operand);

        /// <summary>
        /// Converts the single precision floating point value in <paramref name="source"/> to a signed integer and stores it in <paramref name="destination"/>.
        /// </summary>
        /// <param name="destination">The destination operand, which receives the converted signed integer.</param>
        /// <param name="operand">The source operand, which holds the floating point value.</param>
        void Cvttss2si(Operand destination, Operand operand);

        /// <summary>
        /// Halts the machine
        /// </summary>
        void Hlt();

        /// <summary>
        /// Reads in from the port at src and stores into dest
        /// </summary>
        /// <param name="src">The source operand</param>
        void In(Operand src);

        /// <summary>
        /// Emits a raise interrupt instruction.
        /// </summary>
        /// <param name="interrupt">Contains the interrupt to execute.</param>
        void Int(byte interrupt);

        /// <summary>
        /// Emits a breakpoint instruction.
        /// </summary>
        void Int3();

        /// <summary>
        /// Emits an overflow interrupt instruction.
        /// </summary>
        void IntO();

        /// <summary>
        /// Invalidate Internal Caches
        /// </summary>
        void Invd();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="op"></param>
        void Invlpg(Operand op);

        /// <summary>
        /// Returns from an interrupt.
        /// </summary>
        void Iretd();

        /// <summary>
        /// Emits a jump instruction.
        /// </summary>
        /// <param name="dest">The target label of the jump.</param>
        void Jmp(int dest);

        /// <summary>
        /// Emits a jump instruction.
        /// </summary>
        /// <param name="dest">The target label of the jump.</param>
        void Jns(int dest);

        /// <summary>
        /// Emits a conditional jump above.
        /// </summary>
        /// <param name="dest">The target label of the jump.</param>
        void Ja(int dest);

        /// <summary>
        /// Emits a conditional jump above or equal.
        /// </summary>
        /// <param name="dest">The target label of the jump.</param>
        void Jae(int dest);

        /// <summary>
        /// Emits a conditional jump below.
        /// </summary>
        /// <param name="dest">The target label of the jump.</param>
        void Jb(int dest);

        /// <summary>
        /// Emits a conditional jump below or equal.
        /// </summary>
        /// <param name="dest">The target label of the jump.</param>
        void Jbe(int dest);

        /// <summary>
        /// Emits a conditional jump equal.
        /// </summary>
        /// <param name="dest">The target label of the jump.</param>
        void Je(int dest);

        /// <summary>
        /// Emits a conditional jump greater than.
        /// </summary>
        /// <param name="dest">The target label of the jump.</param>
        void Jg(int dest);

        /// <summary>
        /// Emits a conditional jump greater than or equal.
        /// </summary>
        /// <param name="dest">The target label of the jump.</param>
        void Jge(int dest);

        /// <summary>
        /// Emits a conditional jump less than.
        /// </summary>
        /// <param name="dest">The target label of the jump.</param>
        void Jl(int dest);

        /// <summary>
        /// Emits a conditional jump less than or equal.
        /// </summary>
        /// <param name="dest">The target label of the jump.</param>
        void Jle(int dest);

        /// <summary>
        /// Emits a conditional jump not equal.
        /// </summary>
        /// <param name="dest">The target label of the jump.</param>
        void Jne(int dest);

        /// <summary>
        /// Emits a lea instruction.
        /// </summary>
        /// <param name="dest">The destination of the instruction.</param>
        /// <param name="op">The operand to retrieve the address of.</param>
        void Lea(Operand dest, Operand op);

        /// <summary>
        /// Load Fence
        /// </summary>
        void Lfence();

        /// <summary>
        /// Loads the global descriptor table.
        /// </summary>
        /// <param name="src">Source to load from</param>
        void Lgdt(Operand src);

        /// <summary>
        /// Loads the global interrupt table register
        /// </summary>
        /// <param name="src">Source to load from</param>
        void Lidt(Operand src);

        /// <summary>
        /// Load Local Descriptor Table Register
        /// </summary>
        /// <param name="src">The source operand.</param>
        void Lldt(Operand src);

        /// <summary>
        /// Load Machine Status Word
        /// </summary>
        /// <param name="src">Source to load from</param>
        void Lmsw(Operand src);
        
        /// <summary>
        /// Asserts LOCK# signal for duration of 
        /// the accompanying instruction.
        /// </summary>
        void Lock();

        /// <summary>
        /// Memory Fence
        /// </summary>
        void Mfence();

        /// <summary>
        /// Emits a mul instruction.
        /// </summary>
        /// <param name="op1">The first operand and destination of the instruction.</param>
        /// <param name="op2">The second operand.</param>
        void Mul(Operand op1, Operand op2);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="op"></param>
        void DirectMultiplication(Operand op);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="op"></param>
        void DirectDivision(Operand op);

        /// <summary>
        /// Monitor Wait
        /// </summary>
        void Mwait();

        /// <summary>
        /// Emits a neg instruction.
        /// </summary>
        /// <param name="op">Contains the operand to negate.</param>
        void Neg(Operand op);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="op"></param>
        void Dec(Operand op);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="op"></param>
        void Inc(Operand op);

        /// <summary>
        /// Emits an NOT instruction.
        /// </summary>
        /// <param name="dest">The destination operand of the instruction.</param>
        void Not(Operand dest);

        /// <summary>
        /// Emits an OR instruction.
        /// </summary>
        /// <param name="dest">The destination operand of the instruction.</param>
        /// <param name="src">The source operand of the instruction.</param>
        void Or(Operand dest, Operand src);

        /// <summary>
        /// Outputs the value in src to the port in b
        /// </summary>
        /// <param name="dest">The destination port.</param>
        /// <param name="src">The value.</param>
        void Out(Operand dest, Operand src);

        /// <summary>
        /// Rotates the value in register op1 by op2 bits to the right
        /// </summary>
        /// <param name="op1">The first operand and destination of the instruction.</param>
        /// <param name="op2">The second operand.</param>
        void Rcr(Operand op1, Operand op2);

        /// <summary>
        /// Shifts the value in the register op1 by op2 bits to the right, keeping the sign of the original value.
        /// </summary>
        /// <param name="op1">The first operand and destination of the instruction.</param>
        /// <param name="op2">The second operand.</param>
        void Sar(Operand op1, Operand op2);

        /// <summary>
        /// Shifts the value in the register op1 by op2 bits to the left, keeping the sign of the original value.
        /// </summary>
        /// <param name="op1">The first operand and destination of the instruction.</param>
        /// <param name="op2">The second operand.</param>
        void Sal(Operand op1, Operand op2);

        /// <summary>
        /// Shifts the value in register op1 by op2 bits to the left
        /// </summary>
        /// <param name="op1">The first operand and destination of the instruction.</param>
        /// <param name="op2">The second operand.</param>
        void Shl(Operand op1, Operand op2);

        /// <summary>
        /// Shifts the value in register op1 by op2 bits to the left
        /// </summary>
        /// <param name="dst">The first operand and destination of the instruction.</param>
        /// <param name="src">The second operand.</param>
        /// <param name="count">The count operand.</param>
        void Shld(Operand dst, Operand src, Operand count);

        /// <summary>
        /// Shifts the value in register op1 by op2 bits to the right
        /// </summary>
        /// <param name="op1">The first operand and destination of the instruction.</param>
        /// <param name="op2">The second operand.</param>
        void Shr(Operand op1, Operand op2);

        /// <summary>
        /// Shifts the value in register op1 by op2 bits to the right
        /// </summary>
        /// <param name="dst">The first operand and destination of the instruction.</param>
        /// <param name="src">The second operand.</param>
        /// <param name="count">The count operand.</param>
        void Shrd(Operand dst, Operand src, Operand count);

        /// <summary>
        /// Emits a addsd instruction.
        /// </summary>
        /// <param name="op1">The first operand and destination of the instruction.</param>
        /// <param name="op2">The second operand.</param>
        void SseAdd(Operand op1, Operand op2);

        /// <summary>
        /// Emits a subsd instruction.
        /// </summary>
        /// <param name="op1">The first operand and destination of the instruction.</param>
        /// <param name="op2">The second operand.</param>
        void SseSub(Operand op1, Operand op2);

        /// <summary>
        /// Emits a mulsd instruction.
        /// </summary>
        /// <param name="op1">The first operand and destination of the instruction.</param>
        /// <param name="op2">The second operand.</param>
        void SseMul(Operand op1, Operand op2);

        /// <summary>
        /// Emits a divsd instruction.
        /// </summary>
        /// <param name="op1">The first operand and destination of the instruction.</param>
        /// <param name="op2">The second operand.</param>
        void SseDiv(Operand op1, Operand op2);

        /// <summary>
        /// Emits a unsigned div instruction.
        /// </summary>
        /// <param name="op1">The first operand and destination of the instruction.</param>
        /// <param name="op2">The second operand.</param>
        void Div(Operand op1, Operand op2);

        /// <summary>
        /// Emits a signed div instruction.
        /// </summary>
        /// <param name="op1">The first operand and destination of the instruction.</param>
        /// <param name="op2">The second operand.</param>
        void IDiv(Operand op1, Operand op2);

        /// <summary>
        /// Emits a mov instruction.
        /// </summary>
        /// <param name="dest">The destination of the move.</param>
        /// <param name="src">The source of the move.</param>
        void Mov(Operand dest, Operand src);

        /// <summary>
        /// Emits a mov sign extend instruction.
        /// </summary>
        /// <param name="dest">The destination register.</param>
        /// <param name="src">The source register.</param>
        void Movsx(Operand dest, Operand src);

        /// <summary>
        /// Emits a mov zero extend instruction.
        /// </summary>
        /// <param name="dest">The destination register.</param>
        /// <param name="src">The source register.</param>
        void Movzx(Operand dest, Operand src);

        /// <summary>
        /// Emits a nop instructions.
        /// </summary>
        void Nop();

        /// <summary>
        /// Pauses the machine.
        /// </summary>
        void Pause();

        /// <summary>
        /// Pops the stack's top value into the given operand
        /// </summary>
        /// <param name="operand">The operand to pop into.</param>
        void Pop(Operand operand);

        /// <summary>
        /// Pops the stack's top values into the general purpose registers
        /// </summary>
        void Popad();

        /// <summary>
        /// Pop Stack into EFLAGS Register
        /// </summary>
        void Popfd();

        /// <summary>
        /// Pops the top-most value from the stack into the given operand.
        /// </summary>
        /// <param name="operand">The operand to pop.</param>
        void Push(Operand operand);

        /// <summary>
        /// Pushes all general purpose registers
        /// </summary>
        void Pushad();

        /// <summary>
        /// Push EFLAGS Register onto the Stack
        /// </summary>
        void Pushfd();

        /// <summary>
        /// Read MSR specified by ECX into 
        /// EDX:EAX. (MSR: Model sepcific register)
        /// </summary>
        void Rdmsr();

        /// <summary>
        /// Reads performance monitor counter
        /// </summary>
        void Rdpmc();

        /// <summary>
        /// Reads the timestamp counter
        /// </summary>
        void Rdtsc();

        /// <summary>
        /// Emits a repeat prefix.
        /// </summary>
        void Rep();

        /// <summary>
        /// Emits a return instruction.
        /// </summary>
        void Ret();

        /// <summary>
        /// Subtracts src from dest and stores the result in dest. (dest -= src)
        /// </summary>
        /// <param name="dest">The destination operand.</param>
        /// <param name="src">The source operand.</param>
        void Sbb(Operand dest, Operand src);

        /// <summary>
        /// Store fence
        /// </summary>
        void Sfence();

        /// <summary>
        /// Store global descriptor table to dest
        /// </summary>
        /// <param name="dest">Destination to save to</param>
        void Sgdt(Operand dest);

        /// <summary>
        /// Store interrupt descriptor table to dest
        /// </summary>
        /// <param name="dest">Destination to save to</param>
        void Sidt(Operand dest);

        /// <summary>
        /// Store Local Descriptor Table Register
        /// </summary>
        /// <param name="dest">The destination operand</param>
        void Sldt(Operand dest);

        /// <summary>
        /// Store Machine Status Word
        /// </summary>
        /// <param name="dest">The destination operand</param>
        void Smsw(Operand dest);

        /// <summary>
        /// Emits a enable interrupts instruction.
        /// </summary>
        void Sti();

        /// <summary>
        /// Store MXCSR Register State
        /// </summary>
        /// <param name="dest">The destination operand</param>
        void StmXcsr(Operand dest);

        /// <summary>
        /// Stores a byte string
        /// </summary>
        void Stosb();

        /// <summary>
        /// Stores a dword string
        /// </summary>
        void Stosd();

        /// <summary>
        /// Subtracts src from dest and stores the result in dest. (dest -= src)
        /// </summary>
        /// <param name="dest">The destination operand.</param>
        /// <param name="src">The source operand.</param>
        void Sub(Operand dest, Operand src);

        /// <summary>
        /// Write Back and Invalidate Cache
        /// </summary>
        void Wbinvd();

        /// <summary>
        /// Write to Model Specific Register
        /// </summary>
        void Wrmsr();

        /// <summary>
        /// Exchange Register/Memory with a register
        /// </summary>
        /// <param name="dest">The destination operand of the instruction.</param>
        /// <param name="src">The source operand of the instruction.</param>
        void Xchg(Operand dest, Operand src);

        /// <summary>
        /// Get Value of Extended Control Register
        /// </summary>
        void Xgetbv();

        /// <summary>
        /// Emits an Xor instruction.
        /// </summary>
        /// <param name="dest">The destination operand of the instruction.</param>
        /// <param name="src">The source operand of the instruction.</param>
        void Xor(Operand dest, Operand src);

        /// <summary>
        /// Save Processor Extended States
        /// </summary>
        /// <param name="dest">The destination operand</param>
        void Xsave(Operand dest);

        /// <summary>
        /// Set Extended Control Register
        /// </summary>
        void Xsetbv();

        /// <summary>
        /// Set the destination operand (memory or register) to 0 or 1 depending on the condition outcome.
        /// </summary>
        /// <param name="destination">The destination of the comparison result.</param>
        /// <param name="code">The condition code to test.</param>
        void Setcc(Operand destination, IR2.ConditionCode code);

        #endregion // x86 instructions

        /// <summary>
        /// Converts the double-precision fp value to a single-precision fp value.
        /// </summary>
        /// <param name="dst">The destination operand.</param>
        /// <param name="src">The source operand.</param>
        void Cvtsd2ss(Operand dst, Operand src);

        /// <summary>
        /// Moves a single precision fp value from src to dst.
        /// </summary>
        /// <param name="dst">The destination operand.</param>
        /// <param name="src">The source operand.</param>
        void Movss(Operand dst, Operand src);

        /// <summary>
        /// Moves a double precision fp value from src to dst.
        /// </summary>
        /// <param name="dst">The destination operand.</param>
        /// <param name="src">The source operand.</param>
        void Movsd(Operand dst, Operand src);
    }
}

