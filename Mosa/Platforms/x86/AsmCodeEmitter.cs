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
using System.IO;
using System.Text;

using Mosa.Runtime.CompilerFramework;
using IR = Mosa.Runtime.CompilerFramework.IR;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Vm;

namespace Mosa.Platforms.x86
{
    /// <summary>
    /// An x86 code emitter, which emits the generated instructions to an asm file.
    /// </summary>
    /// <remarks>
    /// This code emitter generates ASM text, which can be processed by NASM. Another
    /// purpose to use this code emitter is to create logs of the generated code.
    /// </remarks>
    public sealed class AsmCodeEmitter : ICodeEmitter, IDisposable
    {
        #region Data members

        /// <summary>
        /// The text writer used to emit the ASM text.
        /// </summary>
        private TextWriter _textWriter;

        private Dictionary<double, string> _doubleMap = new Dictionary<double, string>();

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="AsmCodeEmitter"/>.
        /// </summary>
        /// <param name="textWriter">The text writer to write the ASM text into.</param>
        public AsmCodeEmitter(TextWriter textWriter)
        {
            if (null == textWriter)
                throw new ArgumentNullException(@"textWriter");

            _textWriter = textWriter;
        }

        #endregion // Construction

        #region IDisposable Members

        /// <summary>
        /// Flushes and releases the text writer.
        /// </summary>
        public void Dispose()
        {
            // Flush and release the text writer
            _textWriter.Flush();

            // HACK: This is wrong, but only this allows us to do a clean compilation
            _textWriter.Dispose();
            _textWriter = null;
        }

        #endregion // IDisposable Members

        #region Properties

        /// <summary>
        /// Retrieves the text writer used by the <see cref="AsmCodeEmitter"/>.
        /// </summary>
        public TextWriter Writer
        {
            get { return _textWriter; }
        }

        #endregion // Properties

        #region ICodeEmitter Members
        /// <summary>
        /// Emits a comment into the code stream.
        /// </summary>
        /// <param name="comment">The comment to emit.</param>
        public void Comment(string comment)
        {
            foreach (string line in comment.Split('\n', '\r'))
                _textWriter.WriteLine("; {0}", line);
            
        }

        /// <summary>
        /// Emits a label into the code stream.
        /// </summary>
        /// <param name="label">The label name to emit.</param>
        public void Label(int label)
        {
            _textWriter.WriteLine("L_{0:x}:", label);    
        }

        /// <summary>
        /// Emits a literal constant into the code stream.
        /// </summary>
        /// <param name="label">The label to apply to the data.</param>
        /// <param name="type">The type of the literal.</param>
        /// <param name="data">The data to emit.</param>
        public void Literal(int label, SigType type, object data)
        {
            _textWriter.WriteLine("C_{0:x}:\n\t\tdq\t{1}", label, WriteLiteral(type, data));
        }

        /// <summary>
        /// Emits an AND instruction.
        /// </summary>
        /// <param name="dest">The destination operand of the instruction.</param>
        /// <param name="src">The source operand of the instruction.</param>
        public void And(Operand dest, Operand src)
        {
            if (dest is ConstantOperand)
                throw new NotSupportedException(@"Constants are not allowed as destination.");
            _textWriter.WriteLine("\t\tand\t{0}, {1}", WriteOperand(dest), WriteOperand(src));

        }

        /// <summary>
        /// Emits an Add instruction.
        /// </summary>
        /// <param name="op1">The first operand and destination of the instruction.</param>
        /// <param name="op2">The second operand.</param>
        public void Add(Operand op1, Operand op2)
        {
            if (op1 is ConstantOperand)
                throw new NotSupportedException(@"Constants are not allowed as destination.");
            _textWriter.WriteLine("\t\tadd\t{0}, {1}", WriteOperand(op1), WriteOperand(op2));
            
        }

        /// <summary>
        /// Emits an Adc instruction.
        /// </summary>
        /// <param name="op1">The first operand and destination of the instruction.</param>
        /// <param name="op2">The second operand.</param>
        public void Adc(Operand op1, Operand op2)
        {
            if (op1 is ConstantOperand)
                throw new NotSupportedException(@"Constants are not allowed as destination.");
            _textWriter.WriteLine("\t\tadc\t{0}, {1}", WriteOperand(op1), WriteOperand(op2));

        }

        void ICodeEmitter.Cdq()
        {
            _textWriter.WriteLine("\t\tcdq");
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
            // HACK: method.DeclaringType is not setup right now, just emit the method name.
            _textWriter.WriteLine(String.Format("\t\tcall\t{0}", method.Name));
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
            // HACK: method.DeclaringType is not setup right now, just emit the method name.
            _textWriter.WriteLine(String.Format("\t\tcall\tL_{0:x}", label));
        }

        /// <summary>
        /// Clears DF flag and EFLAGS
        /// </summary>
        public void Cld()
        {
            _textWriter.WriteLine("\t\tcld");
        }

        /// <summary>
        /// Emits a disable interrupts instruction.
        /// </summary>
        public void Cli()
        {
            _textWriter.WriteLine("\t\tcli");
            
        }

        /// <summary>
        /// Emits a comparison instruction.
        /// </summary>
        /// <param name="op1"></param>
        /// <param name="op2"></param>
        public void Cmp(Operand op1, Operand op2)
        {
            _textWriter.WriteLine("\t\tcmp\t{0}, {1}", WriteOperand(op2), WriteOperand(op1));
            
        }

        /// <summary>
        /// Compares and exchanges both values
        /// </summary>
        /// <param name="op1">First operand</param>
        /// <param name="op2">Second operand</param>
        public void CmpXchg(Operand op1, Operand op2)
        {
            _textWriter.WriteLine("\t\tcmpxchg\t{0}, {1}", WriteOperand(op2), WriteOperand(op1));

        }

        void ICodeEmitter.Cvtsd2ss(Operand op1, Operand op2)
        {
            _textWriter.WriteLine("\t\tcvtsd2ss\t{0}, {1}", WriteOperand(op1), WriteOperand(op2));
        }

        void ICodeEmitter.Cvtsi2sd(Operand op1, Operand op2)
        {
            _textWriter.WriteLine("\t\tcvtsi2sd\t{0}, {1}", WriteOperand(op1), WriteOperand(op2));
        }

        void ICodeEmitter.Cvtsi2ss(Operand op1, Operand op2)
        {
            _textWriter.WriteLine("\t\tcvtsi2ss\t{0}, {1}", WriteOperand(op1), WriteOperand(op2));
        }

        void ICodeEmitter.Cvtss2sd(Operand op1, Operand op2)
        {
            _textWriter.WriteLine("\t\tcvtss2sd\t{0}, {1}", WriteOperand(op1), WriteOperand(op2));
        }

        void ICodeEmitter.Cvttsd2si(Operand op1, Operand op2)
        {
            _textWriter.WriteLine("\t\tcvttsd2si\t{0}, {1}", WriteOperand(op1), WriteOperand(op2));
        }

        void ICodeEmitter.Cvttss2si(Operand op1, Operand op2)
        {
            _textWriter.WriteLine("\t\tcvttss2si\t{0}, {1}", WriteOperand(op1), WriteOperand(op2));
        }

        /// <summary>
        /// Emits an interrupt instruction.
        /// </summary>
        void ICodeEmitter.Int(byte interrupt)
        {
            _textWriter.WriteLine("\t\tint\t{0}", interrupt);
        }

        /// <summary>
        /// Emits a breakpoint instruction.
        /// </summary>
        void ICodeEmitter.Int3()
        {
            _textWriter.WriteLine("\t\tint\t3");    
        }

        /// <summary>
        /// Emits an overflow interrupt instruction.
        /// </summary>
        void ICodeEmitter.IntO()
        {
            _textWriter.WriteLine("\t\tinto");
        }

        /// <summary>
        /// Invalidate Internal Caches
        /// </summary>
        void ICodeEmitter.Invd()
        {
            _textWriter.WriteLine("\t\tinvd");
        }

        /// <summary>
        /// Returns from an interrupt
        /// </summary>
        public void Iretd()
        {
            _textWriter.WriteLine("\t\tiretd");

        }

        /// <summary>
        /// Emits a conditional jump above.
        /// </summary>
        /// <param name="dest">The target label of the jump.</param>
        public void Ja(int dest)
        {
            _textWriter.WriteLine("\t\tja\tL_{0:x}", dest);
        }

        /// <summary>
        /// Emits a conditional jump above or equal.
        /// </summary>
        /// <param name="dest">The target label of the jump.</param>
        public void Jae(int dest)
        {
            _textWriter.WriteLine("\t\tjae\tL_{0:x}", dest);  
        }

        /// <summary>
        /// Emits a conditional jump below.
        /// </summary>
        /// <param name="dest">The target label of the jump.</param>
        public void Jb(int dest)
        {
            _textWriter.WriteLine("\t\tjb\tL_{0:x}", dest);            
        }

        /// <summary>
        /// Emits a conditional jump below or equal.
        /// </summary>
        /// <param name="dest">The target label of the jump.</param>
        public void Jbe(int dest)
        {
            _textWriter.WriteLine("\t\tjbe\tL_{0:x}", dest);
        }

        /// <summary>
        /// Emits a conditional jump equal.
        /// </summary>
        /// <param name="dest">The target label of the jump.</param>
        public void Je(int dest)
        {
            _textWriter.WriteLine("\t\tje\tL_{0:x}", dest);
        }

        /// <summary>
        /// Emits a conditional jump greater than.
        /// </summary>
        /// <param name="dest">The target label of the jump.</param>
        public void Jg(int dest)
        {
            _textWriter.WriteLine("\t\tjg\tL_{0:x}", dest);
        }

        /// <summary>
        /// Emits a conditional jump greater than or equal.
        /// </summary>
        /// <param name="dest">The target label of the jump.</param>
        public void Jge(int dest)
        {
            _textWriter.WriteLine("\t\tjge\tL_{0:x}", dest);
        }

        /// <summary>
        /// Emits a conditional jump less than.
        /// </summary>
        /// <param name="dest">The target label of the jump.</param>
        public void Jl(int dest)
        {
            _textWriter.WriteLine("\t\tjl\tL_{0:x}", dest);
        }

        /// <summary>
        /// Emits a conditional jump less than or equal.
        /// </summary>
        /// <param name="dest">The target label of the jump.</param>
        public void Jle(int dest)
        {
            _textWriter.WriteLine("\t\tjle\tL_{0:x}", dest);
        }

        /// <summary>
        /// Emits a conditional jump not equal.
        /// </summary>
        /// <param name="dest">The target label of the jump.</param>
        public void Jne(int dest)
        {
            _textWriter.WriteLine("\t\tjne\tL_{0:x}", dest);
        }

        /// <summary>
        /// Emits a jump instruction.
        /// </summary>
        /// <param name="dest">The target label of the jump.</param>
        public void Jmp(int dest)
        {
            _textWriter.WriteLine("\t\tjmp\tL_{0:x}", dest);
            
        }

        void ICodeEmitter.Lea(Operand dest, Operand op)
        {
            MemoryOperand mop = (MemoryOperand)op;
            
            Register baseReg = mop.Base;
            IntPtr offset = mop.Offset;

            _textWriter.WriteLine("\t\tlea\t{0}, [{1}+{2}]", WriteOperand(dest), baseReg, offset);
        }

        /// <summary>
        /// Load Fence
        /// </summary>
        public void Lfence()
        {
            _textWriter.WriteLine("\t\tlfence");
        }

        /// <summary>
        /// Loads the global descriptor table register
        /// </summary>
        /// <param name="src">Source to load from</param>
        public void Lgdt(Operand src)
        {
            if (!(src is MemoryOperand))
                throw new NotSupportedException(@"Constants or registers are not allowed as source.");
            _textWriter.WriteLine("\t\tlgdt\t{0}", WriteOperand(src));
        }

        /// <summary>
        /// Loads the global interrupt table register
        /// </summary>
        /// <param name="src">Source to load from</param>
        public void Lidt(Operand src)
        {
            if (!(src is MemoryOperand))
                throw new NotSupportedException(@"Constants or registers are not allowed as source.");
            _textWriter.WriteLine("\t\tlidt\t{0}", WriteOperand(src));
        }

        /// <summary>
        /// Load Local Descriptor Table Register
        /// </summary>
        /// <param name="src">Source to load from</param>
        public void Lldt(Operand src)
        {
            _textWriter.WriteLine("\t\tlldt\t{0}", WriteOperand(src));
        }

        /// <summary>
        /// Load Machine Status Word
        /// </summary>
        /// <param name="src">Source to load from</param>
        public void Lmsw(Operand src)
        {
            _textWriter.WriteLine("\t\tlmsw\t{0}", WriteOperand(src));
        }

        /// <summary>
        /// Asserts LOCK# signal for duration of
        /// the accompanying instruction.
        /// </summary>
        public void Lock()
        {
            _textWriter.WriteLine("\t\tlock");
        }

        /// <summary>
        /// Memory Fence
        /// </summary>
        public void Mfence()
        {
            _textWriter.WriteLine("\t\tmfence");
        }

        /// <summary>
        /// Emits a mul instruction.
        /// </summary>
        /// <param name="op1">The first operand and destination of the instruction.</param>
        /// <param name="op2">The second operand.</param>
        public void Mul(Operand op1, Operand op2)
        {
            if (op1 is ConstantOperand)
                throw new NotSupportedException(@"Constants are not allowed as destination.");
            _textWriter.WriteLine("\t\timul\t{0}, {1}", WriteOperand(op1), WriteOperand(op2));
            
        }

        /// <summary>
        /// Monitor Wait
        /// </summary>
        public void Mwait()
        {
            _textWriter.WriteLine("\t\tmwait");
        }

        /// <summary>
        /// Emits a addsd instruction.
        /// </summary>
        /// <param name="op1">The first operand and destination of the instruction.</param>
        /// <param name="op2">The second operand.</param>
        public void SseAdd(Operand op1, Operand op2)
        {
            if (op1 is ConstantOperand)
                throw new NotSupportedException(@"Constants are not allowed as destination.");
            _textWriter.WriteLine("\t\taddsd\t{0}, {1}", "xmm1", WriteOperand(op2));

        }

        /// <summary>
        /// Emits a subsd instruction.
        /// </summary>
        /// <param name="op1">The first operand and destination of the instruction.</param>
        /// <param name="op2">The second operand.</param>
        public void SseSub(Operand op1, Operand op2)
        {
            if (op1 is ConstantOperand)
                throw new NotSupportedException(@"Constants are not allowed as destination.");
            _textWriter.WriteLine("\t\tsubsd\t{0}, {1}", "xmm1", WriteOperand(op2));

        }

        /// <summary>
        /// Emits a mulsd instruction.
        /// </summary>
        /// <param name="op1">The first operand and destination of the instruction.</param>
        /// <param name="op2">The second operand.</param>
        public void SseMul(Operand op1, Operand op2)
        {
            if (op1 is ConstantOperand)
                throw new NotSupportedException(@"Constants are not allowed as destination.");
            _textWriter.WriteLine("\t\tmulsd\t{0}, {1}", "xmm1", WriteOperand(op2));

        }

        /// <summary>
        /// Emits a divsd instruction.
        /// </summary>
        /// <param name="op1">The first operand and destination of the instruction.</param>
        /// <param name="op2">The second operand.</param>
        public void SseDiv(Operand op1, Operand op2)
        {
            if (op1 is ConstantOperand)
                throw new NotSupportedException(@"Constants are not allowed as destination.");
            _textWriter.WriteLine("\t\tdivsd\t{0}, {1}", "xmm1", WriteOperand(op2));

        }

        void ICodeEmitter.Sar(Operand op1, Operand op2)
        {
            _textWriter.WriteLine("\t\tsar\t{0}, {1}", WriteOperand(op1), WriteOperand(op2));
        }

        /// <summary>
        /// Shifts the value in register op1 by op2 bits to the left
        /// </summary>
        /// <param name="op1">The first operand and destination of the instruction.</param>
        /// <param name="op2">The second operand.</param>
        public void Shl(Operand op1, Operand op2)
        {
            if (!(op1 is RegisterOperand))
                throw new NotSupportedException(@"Only registers allowed as destination.");

            _textWriter.WriteLine("\t\tshl\t{0}, {1}", WriteOperand(op1), WriteOperand(op2));
        }

        void ICodeEmitter.Shld(Operand dst, Operand src, Operand count)
        {
            _textWriter.WriteLine("\t\tshld\t{0}, {1}, {2}", WriteOperand(dst), WriteOperand(src), WriteOperand(count));
        }

        /// <summary>
        /// Shifts the value in register op1 by op2 bits to the right
        /// </summary>
        /// <param name="op1">The first operand and destination of the instruction.</param>
        /// <param name="op2">The second operand.</param>
        public void Shr(Operand op1, Operand op2)
        {
            if (!(op1 is RegisterOperand))
                throw new NotSupportedException(@"Only registers allowed as destination.");

            _textWriter.WriteLine("\t\tshr\t{0}, {1}", WriteOperand(op1), WriteOperand(op2));

        }

        void ICodeEmitter.Shrd(Operand dst, Operand src, Operand count)
        {
            _textWriter.WriteLine("\t\tshrd\t{0}, {1}, {2}", WriteOperand(dst), WriteOperand(src), WriteOperand(count));
        }

        /// <summary>
        /// Emits a unsigned div instruction.
        /// </summary>
        /// <param name="op1">The first operand and destination of the instruction.</param>
        /// <param name="op2">The second operand.</param>
        public void Div(Operand op1, Operand op2)
        {
            if (op1 is ConstantOperand)
                throw new NotSupportedException(@"Constants are not allowed as destination.");
            _textWriter.WriteLine("\t\tdiv\t{0}, {1}", WriteOperand(op1), WriteOperand(op2));

        }

        /// <summary>
        /// Emits a signed div instruction.
        /// </summary>
        /// <param name="op1">The first operand and destination of the instruction.</param>
        /// <param name="op2">The second operand.</param>
        public void IDiv(Operand op1, Operand op2)
        {
            if (op1 is ConstantOperand)
                throw new NotSupportedException(@"Constants are not allowed as destination.");
            _textWriter.WriteLine("\t\tidiv\t{0}, {1}", WriteOperand(op1), WriteOperand(op2));
            
        }

        /// <summary>
        /// Emits a mov instruction.
        /// </summary>
        /// <param name="dest">The destination of the move.</param>
        /// <param name="src">The source of the move.</param>
        public void Mov(Operand dest, Operand src)
        {
            if (dest is ConstantOperand && !(src is ConstantOperand))
            {
                Operand tmp = dest;
                dest = src;
                src = tmp;
            }

            if (src is ConstantOperand && src.StackType == StackTypeCode.F)
            {
                if (dest is RegisterOperand)
                    _textWriter.WriteLine("\t\tmovsd\t{0}, {1}", "xmm1", "[cd" + WriteOperand(src) + "]");
                else if (dest is MemoryOperand)
                {
                    _textWriter.WriteLine("\t\tmovsd\t{0}, {1}", "xmm1", "[cd" + WriteOperand(src) + "]");
                    _textWriter.WriteLine("\t\tmovsd\t{0}, {1}", WriteOperand(dest), "xmm1");
                }
            }
            else if (src is MemoryOperand && src.StackType == StackTypeCode.F)
            {
                if (dest is RegisterOperand)
                    _textWriter.WriteLine("\t\tmovsd\t{0}, {1}", "xmm1", WriteOperand(src));
                else if (dest is MemoryOperand)
                {
                    _textWriter.WriteLine("\t\tmovsd\t{0}, {1}", "xmm1", WriteOperand(src));
                    _textWriter.WriteLine("\t\tmovsd\t{0}, {1}", WriteOperand(dest), "xmm1");
                }
            }
            else if (src is RegisterOperand && dest is MemoryOperand && dest.StackType == StackTypeCode.F)
            {
                _textWriter.WriteLine("\t\tmovsd\t{0}, {1}", WriteOperand(dest), "xmm1");
            }
            else
                _textWriter.WriteLine("\t\tmov\t{0}, {1}", WriteOperand(dest), WriteOperand(src));
        }

        void ICodeEmitter.Movss(Operand dest, Operand src)
        {
            _textWriter.WriteLine("\t\tmovss\t{0}, {1}", WriteOperand(dest), WriteOperand(src));
        }

        void ICodeEmitter.Movsd(Operand dest, Operand src)
        {
            _textWriter.WriteLine("\t\tmovsd\t{0}, {1}", WriteOperand(dest), WriteOperand(src));
        }

        /// <summary>
        /// Emits a mov sign extend instruction.
        /// </summary>
        /// <param name="dest">The destination register.</param>
        /// <param name="src">The source register.</param>
        public void Movsx(Operand dest, Operand src)
        {
            if (!(dest is RegisterOperand))
                throw new NotSupportedException(@"Only register destination supported.");
            if (src is ConstantOperand)
                throw new NotSupportedException(@"Source must be memory or register.");

            _textWriter.WriteLine("\t\tmovsx\t{0}, {1}", WriteOperand(dest), WriteOperand(src));
        }

        /// <summary>
        /// Emits a mov zero extend instruction.
        /// </summary>
        /// <param name="dest">The destination register.</param>
        /// <param name="src">The source register.</param>
        public void Movzx(Operand dest, Operand src)
        {
            if (!(dest is RegisterOperand))
                throw new NotSupportedException(@"Only register destination supported.");
            if (src is ConstantOperand)
                throw new NotSupportedException(@"Source must be memory or register.");

            _textWriter.WriteLine("\t\tmovzx\t{0}, {1}", WriteOperand(dest), WriteOperand(src));
        }

        /// <summary>
        /// Emits a nop instructions.
        /// </summary>
        public void Nop()
        {
            _textWriter.WriteLine("\t\tnop");
            
        }

        /// <summary>
        /// Emits an NOT instruction.
        /// </summary>
        /// <param name="dest">The destination operand of the instruction.</param>
        public void Not(Operand dest)
        {
            if (dest is ConstantOperand)
                throw new NotSupportedException(@"Constants are not allowed as destination.");
            _textWriter.WriteLine("\t\tnot\t{0}", WriteOperand(dest));

        }

        /// <summary>
        /// Emits an OR instruction.
        /// </summary>
        /// <param name="dest">The destination operand of the instruction.</param>
        /// <param name="src">The source operand of the instruction.</param>
        public void Or(Operand dest, Operand src)
        {
            if (dest is ConstantOperand)
                throw new NotSupportedException(@"Constants are not allowed as destination.");
            _textWriter.WriteLine("\t\tor\t{0}, {1}", WriteOperand(dest), WriteOperand(src));

        }

        /// <summary>
        /// Pauses the machine.
        /// </summary>
        public void Pause()
        {
            _textWriter.WriteLine("\t\tpause");
        }

        /// <summary>
        /// Pushes the given operand on the stack.
        /// </summary>
        /// <param name="operand">The operand to push.</param>
        public void Pop(Operand operand)
        {
            _textWriter.WriteLine("\t\tpop\t{0}", WriteOperand(operand));
        }

        /// <summary>
        /// Pops the stack's top values into the general purpose registers
        /// </summary>
        public void Popad()
        {
            _textWriter.WriteLine("\t\tpopad");
        }

        /// <summary>
        /// Pop Stack into EFLAGS Register
        /// </summary>
        public void Popfd()
        {
            _textWriter.WriteLine("\t\tpopfd");
        }

        /// <summary>
        /// Pops the top-most value from the stack into the given operand.
        /// </summary>
        /// <param name="operand">The operand to pop.</param>
        public void Push(Operand operand)
        {
            _textWriter.WriteLine("\t\tpush\t{0}", WriteOperand(operand));
            
        }

        /// <summary>
        /// Pushes all general purpose registers
        /// </summary>
        public void Pushad()
        {
            _textWriter.WriteLine("\t\tpushad");

        }

        /// <summary>
        /// Push EFLAGS Register onto the Stack
        /// </summary>
        public void Pushfd()
        {
            _textWriter.WriteLine("\t\tpushfd");

        }

        /// <summary>
        /// Read MSR specified by ECX into
        /// EDX:EAX. (MSR: Model sepcific register)
        /// </summary>
        public void Rdmsr()
        {
            _textWriter.WriteLine("\t\trdmsr");
        }

        /// <summary>
        /// Reads performance monitor counter
        /// </summary>
        public void Rdpmc()
        {
            _textWriter.WriteLine("\t\trdpmc");
        }

        /// <summary>
        /// Reads the timestamp counter
        /// </summary>
        public void Rdtsc()
        {
            _textWriter.WriteLine("\t\trdtsc");
        }

        void ICodeEmitter.Rep()
        {
            _textWriter.WriteLine("\t\trep");
        }

        /// <summary>
        /// Emits a return instruction.
        /// </summary>
        public void Ret()
        {
            _textWriter.WriteLine("\t\tret");
            _textWriter.Flush();   
        }

        /// <summary>
        /// Store fence
        /// </summary>
        public void Sfence()
        {
            _textWriter.WriteLine("\t\tsfence");
        }

        /// <summary>
        /// Store global descriptor table to dest
        /// </summary>
        /// <param name="dest">Destination to save to</param>
        public void Sgdt(Operand dest)
        {
            _textWriter.WriteLine("\t\tsgdt {0}", WriteOperand(dest));
        }

        /// <summary>
        /// Store interrupt descriptor table to dest
        /// </summary>
        /// <param name="dest">Destination to save to</param>
        public void Sidt(Operand dest)
        {
            _textWriter.WriteLine("\t\tsidt {0}", WriteOperand(dest));
        }

        /// <summary>
        /// Store Local Descriptor Table Register
        /// </summary>
        /// <param name="dest">The destination operand</param>
        public void Sldt(Operand dest)
        {
            _textWriter.WriteLine("\t\tsldt {0}", WriteOperand(dest));
        }

        /// <summary>
        /// Store Machine Status Word
        /// </summary>
        /// <param name="dest">The destination operand</param>
        public void Smsw(Operand dest)
        {
            _textWriter.WriteLine("\t\tsmsw {0}", WriteOperand(dest));
        }

        /// <summary>
        /// Emits a enable interrupts instruction.
        /// </summary>
        public void Sti()
        {
            _textWriter.WriteLine("\t\tsti");
        }

        /// <summary>
        /// Store MXCSR Register State
        /// </summary>
        /// <param name="dest">The destination operand</param>
        public void StmXcsr(Operand dest)
        {
            _textWriter.WriteLine("\t\tstmxcsr {0}", WriteOperand(dest));
        }

        /// <summary>
        /// Stores a string
        /// </summary>
        public void Stosb()
        {
            _textWriter.WriteLine("\t\tstosb");
        }

        /// <summary>
        /// Stores a string
        /// </summary>
        public void Stosd()
        {
            _textWriter.WriteLine("\t\tstosd");
        }

        /// <summary>
        /// Subtracts src from dest and stores the result in dest. (dest -= src)
        /// </summary>
        /// <param name="dest">The destination operand.</param>
        /// <param name="src">The source operand.</param>
        public void Sub(Operand dest, Operand src)
        {
            if (dest is ConstantOperand)
                throw new NotSupportedException(@"Constants are not allowed as destination.");
            _textWriter.WriteLine("\t\tsub\t{0}, {1}", WriteOperand(dest), WriteOperand(src));
            
        }

        /// <summary>
        /// Subtracts src from dest and stores the result in dest. (dest -= src)
        /// </summary>
        /// <param name="dest">The destination operand.</param>
        /// <param name="src">The source operand.</param>
        public void Sbb(Operand dest, Operand src)
        {
            if (dest is ConstantOperand)
                throw new NotSupportedException(@"Constants are not allowed as destination.");
            _textWriter.WriteLine("\t\tsbb\t{0}, {1}", WriteOperand(dest), WriteOperand(src));

        }

        /// <summary>
        /// Halts the machine
        /// </summary>
        public void Hlt()
        {
            _textWriter.WriteLine("\t\thlt");
        }

        /// <summary>
        /// Reads in from the port at src and stores into dest
        /// </summary>
        /// <param name="dest">The destination operand</param>
        /// <param name="src">The source operand</param>
        public void In(Operand dest, Operand src)
        {
            if (!(dest is RegisterOperand))
                throw new NotSupportedException(@"Only registers allowed as destination");

            if (!(src is RegisterOperand || src is ConstantOperand))
                throw new NotSupportedException(@"Only registers and constants allowed as source");

            _textWriter.WriteLine("\t\tin\t{0}, {1}", WriteOperand(dest), WriteOperand(src));
            
        }

        /// <summary>
        /// Outputs the value in src to the port in b
        /// </summary>
        /// <param name="dest">The destination port.</param>
        /// <param name="src">The value.</param>
        public void Out(Operand dest, Operand src)
        {
            // Copies the value from the second operand (source operand) to the I/O port 
            // ConstantOperandspecified with the destination operand (first operand).

            if (!(src is RegisterOperand))
                throw new NotSupportedException(@"Only registers allowed as source");

            if (!(dest is RegisterOperand || dest is ConstantOperand))
                throw new NotSupportedException(@"Only registers and constants allowed as destination");

            _textWriter.WriteLine("\t\tout\t{0}, {1}", WriteOperand(dest), WriteOperand(src));
            
        }

        /// <summary>
        /// Write Back and Invalidate Cache
        /// </summary>
        public void Wbinvd()
        {
            _textWriter.WriteLine("\t\twbinvd");
        }

        /// <summary>
        /// Write to Model Specific Register
        /// </summary>
        public void Wrmsr()
        {
            _textWriter.WriteLine("\t\twrmsr");
        }
        /// <summary>
        /// Exchange Register/Memory with a register
        /// </summary>
        /// <param name="dest">The destination operand of the instruction.</param>
        /// <param name="src">The source operand of the instruction.</param>
        public void Xchg(Operand dest, Operand src)
        {
            if (!(dest is MemoryOperand) || (dest is RegisterOperand))
                throw new NotSupportedException(@"Destination has to be register or memory.");
            _textWriter.WriteLine("\t\txchg\t{0}, {1}", WriteOperand(dest), WriteOperand(src));
        }

        /// <summary>
        /// Get Value of Extended Control Register
        /// </summary>
        public void Xgetbv()
        {
            _textWriter.WriteLine("\t\txgetbv");
        }

        /// <summary>
        /// Emits an Xor instruction.
        /// </summary>
        /// <param name="dest">The destination operand of the instruction.</param>
        /// <param name="src">The source operand of the instruction.</param>
        public void Xor(Operand dest, Operand src)
        {
            if (dest is ConstantOperand)
                throw new NotSupportedException(@"Constants are not allowed as destination.");
            _textWriter.WriteLine("\t\txor\t{0}, {1}", WriteOperand(dest), WriteOperand(src));

        }

        /// <summary>
        /// Save Processor Extended States
        /// </summary>
        /// <param name="dest">The destination operand</param>
        public void Xsave(Operand dest)
        {
            _textWriter.WriteLine("\t\txsave {0}", WriteOperand(dest));
        }

        /// <summary>
        /// Set Extended Control Register
        /// </summary>
        public void Xsetbv()
        {
            _textWriter.WriteLine("\t\txsetbv");
        }

        void ICodeEmitter.Setcc(Operand dest, IR.ConditionCode code)
        {
            _textWriter.WriteLine("\t\tset{0}\t{1}", Instructions.SetccInstruction.GetConditionString(code), WriteOperand(dest));
        }

        void ICodeEmitter.Comisd(Operand op1, Operand op2)
        {
            _textWriter.WriteLine("\t\tcomisd\t{0}, {1}", WriteOperand(op1), WriteOperand(op2));
        }

        void ICodeEmitter.Ucomisd(Operand op1, Operand op2)
        {
            _textWriter.WriteLine("\t\tucomisd\t{0}, {1}", WriteOperand(op1), WriteOperand(op2));
        }

        void ICodeEmitter.Comiss(Operand op1, Operand op2)
        {
            _textWriter.WriteLine("\t\tcomiss\t{0}, {1}", WriteOperand(op1), WriteOperand(op2));
        }

        void ICodeEmitter.Ucomiss(Operand op1, Operand op2)
        {
            _textWriter.WriteLine("\t\tucomiss\t{0}, {1}", WriteOperand(op1), WriteOperand(op2));
        }

        #endregion // ICodeEmitter Members

        #region Internals

        /// <summary>
        /// Checks the operand's type and returns the corresponding x86 representation
        /// as a string.
        /// </summary>
        /// <param name="op">The operand that is to be converted to a string.</param>
        /// <returns>The operand's x86 string representation.</returns>
        private string WriteOperand(Operand op)
        {
            // Check if op is a ConstantOperand
            if (op is ConstantOperand)
            {
                ConstantOperand co = (ConstantOperand)op;
                return WriteLiteral(co.Type, co.Value);
            }
            // Check if op is a RegisterOperand
            else if (op is RegisterOperand)
                // Return the register's name
                return (op as RegisterOperand).Register.ToString();
            // Check if op is a LabelOperand
            else if (op is LabelOperand)
                // FIXME: Operand is a label, emit correct NASM syntax for this case
                return String.Format("C_{0:x}", (op as LabelOperand).Label);
            // Check if op is a MemoryOperand
            else if (op is MemoryOperand)
            {
                MemoryOperand mop = op as MemoryOperand;
                if (mop.StackType == StackTypeCode.F)
                {
                    // Return the memorylocation in form of [register + offset]
                    if (mop.Offset.ToInt32() >= 0)
                    {
                        if (mop.Base != null)
                        {
                            return ("[" + mop.Base.ToString() + " + " + mop.Offset.ToString() + "]");
                        }
                        else
                        {
                            return ("[" + mop.Offset.ToString() + "]");
                        }
                    }
                    else
                    {
                        return ("[" + mop.Base.ToString() + "]");
                    }
                }
                else
                {
                    // Return the memorylocation in form of [register + offset]
                    if (mop.Offset.ToInt32() >= 0)
                    {
                        if (mop.Base != null)
                        {
                            return ("dword [" + (op as MemoryOperand).Base.ToString() + " + " + (op as MemoryOperand).Offset.ToString() + "]");
                        }
                        else
                        {
                            return ("dword [" + (op as MemoryOperand).Offset.ToString() + "]");
                        }
                    }
                    else
                    {
                        return ("dword [" + (op as MemoryOperand).Base.ToString() + "]");
                    }
                }
            }
            // Still here, so op is of an unknown or unsupported type.
            else
                return "";// throw new NotSupportedException(@"Unknown operand type");
        }

        private string WriteLiteral(SigType type, object data)
        {
            string result;

            // Convert value from decimal to hexadecimal
            switch (type.Type)
            {
                case Mosa.Runtime.Metadata.CilElementType.I:
                    goto case Mosa.Runtime.Metadata.CilElementType.I4;

                case Mosa.Runtime.Metadata.CilElementType.I4:
                    result = data.ToString();
                    break;

                case Mosa.Runtime.Metadata.CilElementType.I2: goto case Mosa.Runtime.Metadata.CilElementType.I4;
                case Mosa.Runtime.Metadata.CilElementType.I1: goto case Mosa.Runtime.Metadata.CilElementType.I4;

                case Mosa.Runtime.Metadata.CilElementType.I8:
                    result = ((long)data).ToString();
                    break;

                case Mosa.Runtime.Metadata.CilElementType.R4:
                    //result = ((float)data).ToString(System.Globalization.NumberFormatInfo.InvariantInfo);
                    result = data.ToString();
                    break;

                case Mosa.Runtime.Metadata.CilElementType.R8:
                    //result = ((double)data).ToString(System.Globalization.NumberFormatInfo.InvariantInfo);
                    result = data.ToString();
                    break;

                default:
                    throw new NotImplementedException();
            }

            // Return the literal as a string
            return result;
        }

        #endregion // Internals
    }
}
