/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 *  Simon Wollwage (<mailto:simon_wollwage@yahoo.co.jp>)
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
       
        public void Comment(string comment)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Comment(comment);
            });
        }

        public void Label(int label)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Label(label);
            });
        }

        public void Literal(int label, SigType type, object data)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Literal(label, type, data);
            });
        }

        public void And(Operand dest, Operand src)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.And(dest, src);
            });
        }

        public void Add(Operand op1, Operand op2)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Add(op1, op2);
            });
        }

        public void Call(RuntimeMethod method)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Call(method);
            });
        }

        public void Call(int label)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Call(label);
            });
        }

        public void Cli()
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Cli();
            });
        }

        public void Cmp(Operand op1, Operand op2)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Cmp(op1, op2);
            });
        }

        public void Div(Operand op1, Operand op2)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Div(op1, op2);
            });
        }

        public void Int3()
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Int3();
            });
        }

        public void Ja(int dest)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Ja(dest);
            });
        }

        public void Jae(int dest)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Jae(dest);
            });
        }

        public void Jb(int dest)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Jb(dest);
            });
        }

        public void Jbe(int dest)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Jbe(dest);
            });
        }

        public void Je(int dest)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Je(dest);
            });
        }

        public void Jg(int dest)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Jg(dest);
            });
        }

        public void Jge(int dest)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Jge(dest);
            });
        }

        public void Jl(int dest)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Jl(dest);
            });
        }

        public void Jle(int dest)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Jle(dest);
            });
        }

        public void Jne(int dest)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Jne(dest);
            });
        }

        public void Jmp(int dest)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Jmp(dest);
            });
        }

        public void Mul(Operand op1, Operand op2)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Mul(op1, op2);
            });
        }

        public void SseAdd(Operand op1, Operand op2)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.SseAdd(op1, op2);
            });
        }

        public void SseSub(Operand op1, Operand op2)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.SseSub(op1, op2);
            });
        }

        public void SseMul(Operand op1, Operand op2)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.SseMul(op1, op2);
            });
        }

        public void SseDiv(Operand op1, Operand op2)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.SseDiv(op1, op2);
            });
        }

        public void Shl(Operand op1, Operand op2)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Shl(op1, op2);
            });
        }

        public void Shr(Operand op1, Operand op2)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Shr(op1, op2);
            });
        }

        public void Mov(Operand dest, Operand src)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Mov(dest, src);
            });
        }

        public void Nop()
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Nop();
            });
        }

        public void Not(Operand dest)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Not(dest);
            });
        }

        public void Or(Operand dest, Operand src)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Or(dest, src);
            });
        }

        public void Pop(Operand operand)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Pop(operand);
            });
        }

        public void Push(Operand operand)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Push(operand);
            });
        }

        public void Ret()
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Ret();
            });
        }

        public void Sti()
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Sti();
            });
        }

        public void Sub(Operand dest, Operand src)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Sub(dest, src);
            });
        }

        #endregion // ICodeEmitter Members
    }
}
