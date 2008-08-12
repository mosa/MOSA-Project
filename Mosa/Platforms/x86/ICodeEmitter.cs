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
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Vm;

namespace Mosa.Platforms.x86
{
    /// <summary>
    /// Interface of x86 code emitters.
    /// </summary>
    /// <remarks>
    /// A code emitter emits the actual code to reflect specific x86 instructions. There's a set of emitters
    /// available: An <see cref="AsmCodeEmitter"/>, that emits raw assembly text that can be assembled by
    /// NASM, a <see cref="MachineCodeEmitter"/> that emits the raw opcode bytes into a stream of the
    /// respective x86 instructions and the <see cref="MultiplexingCodeEmitter"/>, that allows the use of
    /// multiple emitters at the same time.
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
        /// <param name="type">The type of the literal.</param>
        /// <param name="data">The data to emit.</param>
        void Literal(int label, SigType type, object data); 

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
        /// <param name="method">The label to be called.</param>
        /// <remarks>
        /// This only invokes the platform call, it does not push arguments, spill and
        /// save registers or handle the return value.
        /// </remarks>
        void Call(int label);

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
        /// Emits a breakpoint instruction.
        /// </summary>
        void Int3();

        /// <summary>
        /// Emits a jump instruction.
        /// </summary>
        /// <param name="dest">The target label of the jump.</param>
        void Jmp(int dest);

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
        /// Emits a conditional jump greater then.
        /// </summary>
        /// <param name="dest">The target label of the jump.</param>
        void Jg(int dest);

        /// <summary>
        /// Emits a conditional jump greater then or equal.
        /// </summary>
        /// <param name="dest">The target label of the jump.</param>
        void Jge(int dest);

        /// <summary>
        /// Emits a conditional jump less then.
        /// </summary>
        /// <param name="dest">The target label of the jump.</param>
        void Jl(int dest);

        /// <summary>
        /// Emits a conditional jump less then or equal.
        /// </summary>
        /// <param name="dest">The target label of the jump.</param>
        void Jle(int dest);

        /// <summary>
        /// Emits a conditional jump not equal.
        /// </summary>
        /// <param name="dest">The target label of the jump.</param>
        void Jne(int dest);

        /// <summary>
        /// Emits a mul instruction.
        /// </summary>
        /// <param name="op1">The first operand and destination of the instruction.</param>
        /// <param name="op2">The second operand.</param>
        void Mul(Operand op1, Operand op2);

        /// <summary>
        /// Emits an NOT instruction.
        /// </summary>
        /// <param name="dest">The destination operand of the instruction.</param>
        /// <param name="src">The source operand of the instruction.</param>
        void Not(Operand dest);

        /// <summary>
        /// Emits an OR instruction.
        /// </summary>
        /// <param name="dest">The destination operand of the instruction.</param>
        /// <param name="src">The source operand of the instruction.</param>
        void Or(Operand dest, Operand src);

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
        /// Shifts the value in register op1 by op2 bits to the left
        /// </summary>
        /// <param name="op1">The first operand and destination of the instruction.</param>
        /// <param name="op2">The second operand.</param>
        void Shl(Operand op1, Operand op2);

        /// <summary>
        /// Shifts the value in register op1 by op2 bits to the right
        /// </summary>
        /// <param name="op1">The first operand and destination of the instruction.</param>
        /// <param name="op2">The second operand.</param>
        void Shr(Operand op1, Operand op2);

        /// <summary>
        /// Emits a div instruction.
        /// </summary>
        /// <param name="op1">The first operand and destination of the instruction.</param>
        /// <param name="op2">The second operand.</param>
        void Div(Operand op1, Operand op2);

        /// <summary>
        /// Emits a mov instruction.
        /// </summary>
        /// <param name="dest">The destination of the move.</param>
        /// <param name="op">The source of the move.</param>
        void Mov(Operand dest, Operand src);

        /// <summary>
        /// Emits a nop instructions.
        /// </summary>
        void Nop();

        /// <summary>
        /// Pushes the given operand on the stack.
        /// </summary>
        /// <param name="operand">The operand to push.</param>
        void Pop(Operand operand);

        /// <summary>
        /// Pops the top-most value from the stack into the given operand.
        /// </summary>
        /// <param name="operand">The operand to pop.</param>
        void Push(Operand operand);

        /// <summary>
        /// Emits a return instruction.
        /// </summary>
        void Ret();

        /// <summary>
        /// Emits a enable interrupts instruction.
        /// </summary>
        void Sti();

        /// <summary>
        /// Subtracts src from dest and stores the result in dest. (dest -= src)
        /// </summary>
        /// <param name="dest">The destination operand.</param>
        /// <param name="src">The source operand.</param>
        void Sub(Operand dest, Operand src);

        #endregion // x86 instructions
    }
}
