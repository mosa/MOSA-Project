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
using System.IO;
using System.Text;

using Mosa.Runtime.CompilerFramework;
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

        public void Comment(string comment)
        {
            foreach (string line in comment.Split('\n', '\r'))
                _textWriter.WriteLine("; {0}", line);
            
        }

        public void Label(int label)
        {
            _textWriter.WriteLine("L_{0:x}:", label);    
        }

        public void Literal(int label, SigType type, object data)
        {
            _textWriter.WriteLine("C_{0:x}:\n\t\tdq\t{1}", label, WriteLiteral(type, data));
        }

        public void And(Operand dest, Operand src)
        {
            if (dest is ConstantOperand)
                throw new NotSupportedException(@"Constants are not allowed as destination.");
            _textWriter.WriteLine("\t\tand\t{0}, {1}", WriteOperand(dest), WriteOperand(src));

        }

        public void Add(Operand op1, Operand op2)
        {
            if (op1 is ConstantOperand)
                throw new NotSupportedException(@"Constants are not allowed as destination.");
            _textWriter.WriteLine("\t\tadd\t{0}, {1}", WriteOperand(op1), WriteOperand(op2));
            
        }

        public void Call(RuntimeMethod method)
        {
            // HACK: method.DeclaringType is not setup right now, just emit the method name.
            _textWriter.WriteLine(String.Format("\t\tcall\t{0}", method.Name));
        }

        public void Call(int label)
        {
            // HACK: method.DeclaringType is not setup right now, just emit the method name.
            _textWriter.WriteLine(String.Format("\t\tcall\tL_{0:x}", label));
        }

        public void Cli()
        {
            _textWriter.WriteLine("\t\tcli");
            
        }

        public void Cmp(Operand op1, Operand op2)
        {
            _textWriter.WriteLine("\t\tcmp\t{0}, {1}", WriteOperand(op2), WriteOperand(op1));
            
        }

        public void Int3()
        {
            _textWriter.WriteLine("\t\tint\t3");
            
        }

        public void Ja(int dest)
        {
            _textWriter.WriteLine("\t\tja\tL_{0:x}", dest);
        }

        public void Jae(int dest)
        {
            _textWriter.WriteLine("\t\tjae\tL_{0:x}", dest);  
        }

        public void Jb(int dest)
        {
            _textWriter.WriteLine("\t\tjb\tL_{0:x}", dest);            
        }

        public void Jbe(int dest)
        {
            _textWriter.WriteLine("\t\tjbe\tL_{0:x}", dest);
        }
        
        public void Je(int dest)
        {
            _textWriter.WriteLine("\t\tje\tL_{0:x}", dest);
        }

        public void Jg(int dest)
        {
            _textWriter.WriteLine("\t\tjg\tL_{0:x}", dest);
        }

        public void Jge(int dest)
        {
            _textWriter.WriteLine("\t\tjge\tL_{0:x}", dest);
        }

        public void Jl(int dest)
        {
            _textWriter.WriteLine("\t\tjl\tL_{0:x}", dest);
        }

        public void Jle(int dest)
        {
            _textWriter.WriteLine("\t\tjle\tL_{0:x}", dest);
        }

        public void Jne(int dest)
        {
            _textWriter.WriteLine("\t\tjne\tL_{0:x}", dest);
        }

        public void Jmp(int dest)
        {
            _textWriter.WriteLine("\t\tjmp\tL_{0:x}", dest);
            
        }

        public void Mul(Operand op1, Operand op2)
        {
            if (op1 is ConstantOperand)
                throw new NotSupportedException(@"Constants are not allowed as destination.");
            _textWriter.WriteLine("\t\timul\t{0}, {1}", WriteOperand(op1), WriteOperand(op2));
            
        }

        public void SseAdd(Operand op1, Operand op2)
        {
            if (op1 is ConstantOperand)
                throw new NotSupportedException(@"Constants are not allowed as destination.");
            _textWriter.WriteLine("\t\taddsd\t{0}, {1}", "xmm1", WriteOperand(op2));

        }

        public void SseSub(Operand op1, Operand op2)
        {
            if (op1 is ConstantOperand)
                throw new NotSupportedException(@"Constants are not allowed as destination.");
            _textWriter.WriteLine("\t\tsubsd\t{0}, {1}", "xmm1", WriteOperand(op2));

        }

        public void SseMul(Operand op1, Operand op2)
        {
            if (op1 is ConstantOperand)
                throw new NotSupportedException(@"Constants are not allowed as destination.");
            _textWriter.WriteLine("\t\tmulsd\t{0}, {1}", "xmm1", WriteOperand(op2));

        }

        public void SseDiv(Operand op1, Operand op2)
        {
            if (op1 is ConstantOperand)
                throw new NotSupportedException(@"Constants are not allowed as destination.");
            _textWriter.WriteLine("\t\tdivsd\t{0}, {1}", "xmm1", WriteOperand(op2));

        }

        public void Shl(Operand op1, Operand op2)
        {
            if (!(op1 is RegisterOperand))
                throw new NotSupportedException(@"Only registers allowed as destination.");

            _textWriter.WriteLine("\t\tshl\t{0}, {1}", WriteOperand(op1), WriteOperand(op2));

        }

        public void Shr(Operand op1, Operand op2)
        {
            if (!(op1 is RegisterOperand))
                throw new NotSupportedException(@"Only registers allowed as destination.");

            _textWriter.WriteLine("\t\tshr\t{0}, {1}", WriteOperand(op1), WriteOperand(op2));

        }

        public void Div(Operand op1, Operand op2)
        {
            if (op1 is ConstantOperand)
                throw new NotSupportedException(@"Constants are not allowed as destination.");
            _textWriter.WriteLine("\t\tidiv\t{0}, {1}", WriteOperand(op1), WriteOperand(op2));
            
        }

        public void Mov(Operand dest, Operand src)
        {
            if (dest is ConstantOperand)
                throw new NotSupportedException(@"Constants are not allowed as destination.");

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

        public void Nop()
        {
            _textWriter.WriteLine("\t\tnop");
            
        }

        public void Pop(Operand operand)
        {
            _textWriter.WriteLine("\t\tpop\t{0}", WriteOperand(operand));
          
        }

        public void Push(Operand operand)
        {
            _textWriter.WriteLine("\t\tpush\t{0}", WriteOperand(operand));
            
        }

        public void Ret()
        {
            _textWriter.WriteLine("\t\tret");
            _textWriter.Flush();   
        }

        public void Sti()
        {
            _textWriter.WriteLine("\t\tsti");
            
        }

        public void Sub(Operand dest, Operand src)
        {
            if (dest is ConstantOperand)
                throw new NotSupportedException(@"Constants are not allowed as destination.");
            _textWriter.WriteLine("\t\tsub\t{0}, {1}", WriteOperand(dest), WriteOperand(src));
            
        }

        public void In(Operand dest, Operand src)
        {
            if (!(dest is RegisterOperand))
                throw new NotSupportedException(@"Only registers allowed as destination");

            if (!(src is RegisterOperand || src is ConstantOperand))
                throw new NotSupportedException(@"Only registers and constants allowed as source");

            _textWriter.WriteLine("\t\tin\t{0}, {1}", WriteOperand(dest), WriteOperand(src));
            
        }

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
                if ((op as MemoryOperand).StackType == StackTypeCode.F)
                {
                    // Return the memorylocation in form of [register + offset]
                    if ((op as MemoryOperand).Offset.ToInt32() >= 0)
                        return ("[" + (op as MemoryOperand).Base.ToString() + " + " + (op as MemoryOperand).Offset.ToString() + "]");
                    else
                        return ("[" + (op as MemoryOperand).Base.ToString() + (op as MemoryOperand).Offset.ToString() + "]");
                }
                else
                {
                    // Return the memorylocation in form of [register + offset]
                    if ((op as MemoryOperand).Offset.ToInt32() >= 0)
                        return ("dword [" + (op as MemoryOperand).Base.ToString() + " + " + (op as MemoryOperand).Offset.ToString() + "]");
                    else
                        return ("dword [" + (op as MemoryOperand).Base.ToString() + (op as MemoryOperand).Offset.ToString() + "]");

                }
            }
            // Still here, so op is of an unknown or unsupported type.
            else
                throw new NotSupportedException(@"Unknown operand type");
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

                case Mosa.Runtime.Metadata.CilElementType.I2:
                    result = data.ToString();
                    break;

                case Mosa.Runtime.Metadata.CilElementType.R4:
                    result = ((float)data).ToString(System.Globalization.NumberFormatInfo.InvariantInfo);
                    break;

                case Mosa.Runtime.Metadata.CilElementType.R8:
                    result = ((double)data).ToString(System.Globalization.NumberFormatInfo.InvariantInfo);
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
