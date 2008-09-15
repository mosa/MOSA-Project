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

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Vm;

namespace Mosa.Platforms.x86
{
    /// <summary>
    /// Forwards calls to the code emitter interface to multiple implementations.
    /// </summary>
    public sealed class MultiplexingCodeEmitter : ICodeEmitter, IDisposable
    {
        #region Data members

        /// <summary>
        /// The real list of code emitters the multiplexer forwards to.
        /// </summary>
        private List<ICodeEmitter> _emitters;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiplexingCodeEmitter"/>.
        /// </summary>
        public MultiplexingCodeEmitter()
        {
            _emitters = new List<ICodeEmitter>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiplexingCodeEmitter"/>.
        /// </summary>
        /// <param name="collection">The collection of code emitters to add to the multiplexer.</param>
        public MultiplexingCodeEmitter(IEnumerable<ICodeEmitter> collection)
        {
            _emitters = new List<ICodeEmitter>(collection);
        }

        #endregion // Construction

        #region IDisposable Members

        /// <summary>
        /// Forwards the dispose call to all code emitters and clears their list.
        /// </summary>
        public void Dispose()
        {
            foreach (IDisposable d in _emitters)
                d.Dispose();

            _emitters.Clear();
        }

        #endregion // IDisposable Members

        #region Properties

        /// <summary>
        /// Returns the list of code emitters this multiplexer forwards to.
        /// </summary>
        public List<ICodeEmitter> Emitters
        {
            get { return _emitters; }
        }

        #endregion // Properties

        #region ICodeEmitter Members
        /// <summary>
        /// Emits a comment into the code stream.
        /// </summary>
        /// <param name="comment">The comment to emit.</param>
        public void Comment(string comment)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Comment(comment);
            });
        }

        /// <summary>
        /// Emits a label into the code stream.
        /// </summary>
        /// <param name="label">The label name to emit.</param>
        public void Label(int label)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Label(label);
            });
        }

        /// <summary>
        /// Emits a literal constant into the code stream.
        /// </summary>
        /// <param name="label">The label to apply to the data.</param>
        /// <param name="type">The type of the literal.</param>
        /// <param name="data">The data to emit.</param>
        public void Literal(int label, SigType type, object data)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Literal(label, type, data);
            });
        }

        /// <summary>
        /// Emits an AND instruction.
        /// </summary>
        /// <param name="dest">The destination operand of the instruction.</param>
        /// <param name="src">The source operand of the instruction.</param>
        public void And(Operand dest, Operand src)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.And(dest, src);
            });
        }

        /// <summary>
        /// Emits an Add instruction.
        /// </summary>
        /// <param name="op1">The first operand and destination of the instruction.</param>
        /// <param name="op2">The second operand.</param>
        public void Add(Operand op1, Operand op2)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Add(op1, op2);
            });
        }

        /// <summary>
        /// Emits an Adc instruction.
        /// </summary>
        /// <param name="op1">The first operand and destination of the instruction.</param>
        /// <param name="op2">The second operand.</param>
        public void Adc(Operand op1, Operand op2)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Adc(op1, op2);
            });
        }

        /// <summary>
        /// Emits a Call instruction
        /// </summary>
        /// <param name="method">The method to be called.</param>
        /// <remarks>
        /// This only invokes the platform call, it does not push arguments, spill and
        /// save registers or handle the return value.
        /// </remarks>
        public void Call(RuntimeMethod method)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Call(method);
            });
        }

        /// <summary>
        /// Emits a CALL instruction to the given label.
        /// </summary>
        /// <param name="label">The label to be called.</param>
        /// <remarks>
        /// This only invokes the platform call, it does not push arguments, spill and
        /// save registers or handle the return value.
        /// </remarks>
        public void Call(int label)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Call(label);
            });
        }

        /// <summary>
        /// Clears DF flag and EFLAGS
        /// </summary>
        public void Cld()
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Cld();
            });
        }

        /// <summary>
        /// Emits a disable interrupts instruction.
        /// </summary>
        public void Cli()
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Cli();
            });
        }

        /// <summary>
        /// Emits a comparison instruction.
        /// </summary>
        /// <param name="op1"></param>
        /// <param name="op2"></param>
        public void Cmp(Operand op1, Operand op2)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Cmp(op1, op2);
            });
        }

        /// <summary>
        /// Compares and exchanges both values
        /// </summary>
        /// <param name="op1">First operand</param>
        /// <param name="op2">Second operand</param>
        public void CmpXchg(Operand op1, Operand op2)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.CmpXchg(op1, op2);
            });
        }

        /// <summary>
        /// Emits a div instruction.
        /// </summary>
        /// <param name="op1">The first operand and destination of the instruction.</param>
        /// <param name="op2">The second operand.</param>
        public void Div(Operand op1, Operand op2)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Div(op1, op2);
            });
        }

        /// <summary>
        /// Halts the machine
        /// </summary>
        public void Hlt()
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Hlt();
            });
        }

        /// <summary>
        /// Reads in from the port at src and stores into dest
        /// </summary>
        /// <param name="dest">The destination operand</param>
        /// <param name="src">The source operand</param>
        public void In(Operand dest, Operand src)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.In(dest, src);
            });
        }

        void ICodeEmitter.Int(byte interrupt)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Int(interrupt);
            });
        }

        void ICodeEmitter.Int3()
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Int3();
            });
        }

        void ICodeEmitter.IntO()
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.IntO();
            });
        }

        /// <summary>
        /// Returns from an interrupt
        /// </summary>
        public void Iretd()
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Iretd();
            });
        }

        /// <summary>
        /// Emits a conditional jump above.
        /// </summary>
        /// <param name="dest">The target label of the jump.</param>
        public void Ja(int dest)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Ja(dest);
            });
        }

        /// <summary>
        /// Emits a conditional jump above or equal.
        /// </summary>
        /// <param name="dest">The target label of the jump.</param>
        public void Jae(int dest)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Jae(dest);
            });
        }

        /// <summary>
        /// Emits a conditional jump below.
        /// </summary>
        /// <param name="dest">The target label of the jump.</param>
        public void Jb(int dest)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Jb(dest);
            });
        }

        /// <summary>
        /// Emits a conditional jump below or equal.
        /// </summary>
        /// <param name="dest">The target label of the jump.</param>
        public void Jbe(int dest)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Jbe(dest);
            });
        }

        /// <summary>
        /// Emits a conditional jump equal.
        /// </summary>
        /// <param name="dest">The target label of the jump.</param>
        public void Je(int dest)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Je(dest);
            });
        }

        /// <summary>
        /// Emits a conditional jump greater than.
        /// </summary>
        /// <param name="dest">The target label of the jump.</param>
        public void Jg(int dest)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Jg(dest);
            });
        }

        /// <summary>
        /// Emits a conditional jump greater than or equal.
        /// </summary>
        /// <param name="dest">The target label of the jump.</param>
        public void Jge(int dest)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Jge(dest);
            });
        }

        /// <summary>
        /// Emits a conditional jump less than.
        /// </summary>
        /// <param name="dest">The target label of the jump.</param>
        public void Jl(int dest)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Jl(dest);
            });
        }

        /// <summary>
        /// Emits a conditional jump less than or equal.
        /// </summary>
        /// <param name="dest">The target label of the jump.</param>
        public void Jle(int dest)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Jle(dest);
            });
        }

        /// <summary>
        /// Emits a conditional jump not equal.
        /// </summary>
        /// <param name="dest">The target label of the jump.</param>
        public void Jne(int dest)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Jne(dest);
            });
        }

        /// <summary>
        /// Emits a jump instruction.
        /// </summary>
        /// <param name="dest">The target label of the jump.</param>
        public void Jmp(int dest)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Jmp(dest);
            });
        }

        void ICodeEmitter.Lea(Operand dest, Operand op)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Lea(dest, op);
            });
        }

        /// <summary>
        /// Loads the global descriptor table register
        /// </summary>
        /// <param name="src">Source to load from</param>
        public void Lgdt(Operand src)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Lgdt(src);
            });
        }

        /// <summary>
        /// Loads the global interrupt table register
        /// </summary>
        /// <param name="src">Source to load from</param>
        public void Lidt(Operand src)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Lidt(src);
            });
        }

        /// <summary>
        /// Asserts LOCK# signal for duration of
        /// the accompanying instruction.
        /// </summary>
        public void Lock()
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Lock();
            });
        }

        /// <summary>
        /// Emits a mul instruction.
        /// </summary>
        /// <param name="op1">The first operand and destination of the instruction.</param>
        /// <param name="op2">The second operand.</param>
        public void Mul(Operand op1, Operand op2)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Mul(op1, op2);
            });
        }

        /// <summary>
        /// Emits a addsd instruction.
        /// </summary>
        /// <param name="op1">The first operand and destination of the instruction.</param>
        /// <param name="op2">The second operand.</param>
        public void SseAdd(Operand op1, Operand op2)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.SseAdd(op1, op2);
            });
        }

        /// <summary>
        /// Emits a subsd instruction.
        /// </summary>
        /// <param name="op1">The first operand and destination of the instruction.</param>
        /// <param name="op2">The second operand.</param>
        public void SseSub(Operand op1, Operand op2)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.SseSub(op1, op2);
            });
        }

        /// <summary>
        /// Emits a mulsd instruction.
        /// </summary>
        /// <param name="op1">The first operand and destination of the instruction.</param>
        /// <param name="op2">The second operand.</param>
        public void SseMul(Operand op1, Operand op2)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.SseMul(op1, op2);
            });
        }

        /// <summary>
        /// Emits a divsd instruction.
        /// </summary>
        /// <param name="op1">The first operand and destination of the instruction.</param>
        /// <param name="op2">The second operand.</param>
        public void SseDiv(Operand op1, Operand op2)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.SseDiv(op1, op2);
            });
        }

        /// <summary>
        /// Shifts the value in register op1 by op2 bits to the left
        /// </summary>
        /// <param name="op1">The first operand and destination of the instruction.</param>
        /// <param name="op2">The second operand.</param>
        public void Sar(Operand op1, Operand op2)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Sar(op1, op2);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="op1"></param>
        /// <param name="op2"></param>
        public void Shl(Operand op1, Operand op2)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Shl(op1, op2);
            });
        }

        /// <summary>
        /// Shifts the value in register op1 by op2 bits to the right
        /// </summary>
        /// <param name="op1">The first operand and destination of the instruction.</param>
        /// <param name="op2">The second operand.</param>
        public void Shr(Operand op1, Operand op2)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Shr(op1, op2);
            });
        }

        /// <summary>
        /// Emits a mov instruction.
        /// </summary>
        /// <param name="dest">The destination of the move.</param>
        /// <param name="src">The source of the move.</param>
        public void Mov(Operand dest, Operand src)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Mov(dest, src);
            });
        }

        /// <summary>
        /// Emits a mov sign extend instruction.
        /// </summary>
        /// <param name="dest">The destination register.</param>
        /// <param name="src">The source register.</param>
        public void Movsx(Operand dest, Operand src)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Movsx(dest, src);
            });
        }

        /// <summary>
        /// Emits a mov zero extend instruction.
        /// </summary>
        /// <param name="dest">The destination register.</param>
        /// <param name="src">The source register.</param>
        public void Movzx(Operand dest, Operand src)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Movzx(dest, src);
            });
        }

        /// <summary>
        /// Emits a nop instructions.
        /// </summary>
        public void Nop()
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Nop();
            });
        }

        /// <summary>
        /// Emits an NOT instruction.
        /// </summary>
        /// <param name="dest">The destination operand of the instruction.</param>
        public void Not(Operand dest)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Not(dest);
            });
        }

        /// <summary>
        /// Emits an OR instruction.
        /// </summary>
        /// <param name="dest">The destination operand of the instruction.</param>
        /// <param name="src">The source operand of the instruction.</param>
        public void Or(Operand dest, Operand src)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Or(dest, src);
            });
        }

        /// <summary>
        /// Outputs the value in src to the port in b
        /// </summary>
        /// <param name="dest">The destination port.</param>
        /// <param name="src">The value.</param>
        public void Out(Operand dest, Operand src)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Out(dest, src);
            });
        }

        /// <summary>
        /// Pops the stack's top value into the given operand
        /// </summary>
        /// <param name="operand">The operand to push.</param>
        public void Pop(Operand operand)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Pop(operand);
            });
        }

        /// <summary>
        /// Pops the stack's top values into the general purpose registers
        /// </summary>
        public void Popad()
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Popad();
            });
        }

        /// <summary>
        /// Pop Stack into EFLAGS Register
        /// </summary>
        public void Popfd()
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Popfd();
            });
        }

        /// <summary>
        /// Pops the top-most value from the stack into the given operand.
        /// </summary>
        /// <param name="operand">The operand to pop.</param>
        public void Push(Operand operand)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Push(operand);
            });
        }

        /// <summary>
        /// Pushes all general purpose registers
        /// </summary>
        public void Pushad()
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Pushad();
            });
        }

        /// <summary>
        /// Push EFLAGS Register onto the Stack
        /// </summary>
        public void Pushfd()
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Pushfd();
            });
        }

        /// <summary>
        /// Emits a return instruction.
        /// </summary>
        public void Ret()
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Ret();
            });
        }

        /// <summary>
        /// Emits a enable interrupts instruction.
        /// </summary>
        public void Sti()
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Sti();
            });
        }

        /// <summary>
        /// Stores a string
        /// </summary>
        /// <param name="dest">The destination operand.</param>
        public void Stos(Operand dest)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Stos(dest);
            });
        }

        /// <summary>
        /// Subtracts src from dest and stores the result in dest. (dest -= src)
        /// </summary>
        /// <param name="dest">The destination operand.</param>
        /// <param name="src">The source operand.</param>
        public void Sub(Operand dest, Operand src)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Sub(dest, src);
            });
        }

        /// <summary>
        /// Exchange Register/Memory with a register
        /// </summary>
        /// <param name="dest">The destination operand of the instruction.</param>
        /// <param name="src">The source operand of the instruction.</param>
        public void Xchg(Operand dest, Operand src)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Xchg(dest, src);
            });
        }

        /// <summary>
        /// Emits an Xor instruction.
        /// </summary>
        /// <param name="dest">The destination operand of the instruction.</param>
        /// <param name="src">The source operand of the instruction.</param>
        public void Xor(Operand dest, Operand src)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Xor(dest, src);
            });
        }

        /// <summary>
        /// </summary>
        /// <param name="code"></param>
        public void Setcc(Mosa.Runtime.CompilerFramework.IL.OpCode code)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Setcc(code);
            });
        }

        #endregion // ICodeEmitter Members
    }
}
