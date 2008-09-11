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
        /// 
        /// </summary>
        /// <param name="comment"></param>
        public void Comment(string comment)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Comment(comment);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        public void Label(int label)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Label(label);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <param name="type"></param>
        /// <param name="data"></param>
        public void Literal(int label, SigType type, object data)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Literal(label, type, data);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="src"></param>
        public void And(Operand dest, Operand src)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.And(dest, src);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="op1"></param>
        /// <param name="op2"></param>
        public void Add(Operand op1, Operand op2)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Add(op1, op2);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="op1"></param>
        /// <param name="op2"></param>
        public void Adc(Operand op1, Operand op2)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Adc(op1, op2);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        public void Call(RuntimeMethod method)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Call(method);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        public void Call(int label)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Call(label);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        public void Cli()
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Cli();
            });
        }

        /// <summary>
        /// 
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
        /// 
        /// </summary>
        /// <param name="op1"></param>
        /// <param name="op2"></param>
        public void Div(Operand op1, Operand op2)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Div(op1, op2);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        public void Int3()
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Int3();
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dest"></param>
        public void Ja(int dest)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Ja(dest);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dest"></param>
        public void Jae(int dest)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Jae(dest);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dest"></param>
        public void Jb(int dest)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Jb(dest);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dest"></param>
        public void Jbe(int dest)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Jbe(dest);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dest"></param>
        public void Je(int dest)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Je(dest);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dest"></param>
        public void Jg(int dest)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Jg(dest);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dest"></param>
        public void Jge(int dest)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Jge(dest);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dest"></param>
        public void Jl(int dest)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Jl(dest);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dest"></param>
        public void Jle(int dest)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Jle(dest);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dest"></param>
        public void Jne(int dest)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Jne(dest);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dest"></param>
        public void Jmp(int dest)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Jmp(dest);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="op1"></param>
        /// <param name="op2"></param>
        public void Mul(Operand op1, Operand op2)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Mul(op1, op2);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="op1"></param>
        /// <param name="op2"></param>
        public void SseAdd(Operand op1, Operand op2)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.SseAdd(op1, op2);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="op1"></param>
        /// <param name="op2"></param>
        public void SseSub(Operand op1, Operand op2)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.SseSub(op1, op2);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="op1"></param>
        /// <param name="op2"></param>
        public void SseMul(Operand op1, Operand op2)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.SseMul(op1, op2);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="op1"></param>
        /// <param name="op2"></param>
        public void SseDiv(Operand op1, Operand op2)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.SseDiv(op1, op2);
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
        /// 
        /// </summary>
        /// <param name="op1"></param>
        /// <param name="op2"></param>
        public void Shr(Operand op1, Operand op2)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Shr(op1, op2);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="src"></param>
        public void Mov(Operand dest, Operand src)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Mov(dest, src);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="src"></param>
        public void Movsx(Operand dest, Operand src)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Movsx(dest, src);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="src"></param>
        public void Movzx(Operand dest, Operand src)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Movzx(dest, src);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        public void Nop()
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Nop();
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dest"></param>
        public void Not(Operand dest)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Not(dest);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="src"></param>
        public void Or(Operand dest, Operand src)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Or(dest, src);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operand"></param>
        public void Pop(Operand operand)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Pop(operand);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operand"></param>
        public void Push(Operand operand)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Push(operand);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        public void Ret()
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Ret();
            });
        }

        /// <summary>
        /// 
        /// </summary>
        public void Sti()
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Sti();
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="src"></param>
        public void Sub(Operand dest, Operand src)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Sub(dest, src);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="src"></param>
        public void Xor(Operand dest, Operand src)
        {
            _emitters.ForEach(delegate(ICodeEmitter emitter)
            {
                emitter.Xor(dest, src);
            });
        }

        /// <summary>
        /// 
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
