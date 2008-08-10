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
using System.Diagnostics;

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Vm;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Platforms.x86
{
    /// <summary>
    /// An x86 machine code emitter.
    /// </summary>
    public sealed class MachineCodeEmitter : ICodeEmitter, IDisposable
    {
        #region Types

        struct Patch
        {
            public Patch(int label, long position)
            {
                this.label = label;
                this.position = position;
            }

            public int label;
            public long position;
        }

        #endregion // Types

        #region Data members

        /// <summary>
        /// The stream used to write machine code bytes to.
        /// </summary>
        private Stream _codeStream;

        /// <summary>
        /// Patches we need to perform.
        /// </summary>
        private List<Patch> _patches = new List<Patch>();

        /// <summary>
        /// List of labels that were emitted.
        /// </summary>
        private Dictionary<int, long> _labels = new Dictionary<int, long>();

        /// <summary>
        /// List of literal patches we need to perform.
        /// </summary>
        private List<Patch> _literals = new List<Patch>();

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="MachineCodeEmitter"/>.
        /// </summary>
        /// <param name="codeStream">The stream the machine code is written to.</param>
        public MachineCodeEmitter(Stream codeStream)
        {
            Debug.Assert(null != codeStream, @"MachineCodeEmitter needs a code stream.");
            if (null == codeStream)
                throw new ArgumentNullException(@"codeStream");

            _codeStream = codeStream;
        }

        #endregion // Construction

        #region IDisposable Members

        /// <summary>
        /// Completes emitting the code of a method.
        /// </summary>
        public void Dispose()
        {
            // Flush the stream - we're not responsible for disposing it, as it belongs
            // to another component that gave it to the code generator.
            _codeStream.Flush();
        }

        #endregion // IDisposable Members

        #region ICodeEmitter Members

        public void Comment(string comment)
        {
            /*
             * The machine code emitter does not support comments. They're simply ignored.
             * 
             */
        }

        public void Label(int label)
        {
            /*
             * FIXME: Labels are used to resolve branches inside a procedure. Branches outside
             * of procedures are handled differently, t.b.d. 
             * 
             * So we store the current instruction offset with the label info to be able to 
             * resolve later backward jumps to this location.
             *
             * Additionally there may have been forward branches to this location, which have not
             * been resolved yet, so we need to scan these and resolve them to the current
             * instruction offset.
             * 
             */

            // Save the current position
            long currentPosition = _codeStream.Position;
            // Relative branch offset
            int relOffset;

            // Check if this label has forward references on it...
            _patches.RemoveAll(delegate(Patch p)
            {
                if (p.label == label)
                {
                    _codeStream.Position = p.position;
                    relOffset = (int)currentPosition - ((int)p.position + 4);
                    WriteImmediate(relOffset);
                    return true;
                }

                return false;
            });

            // Add this label to the label list, so we can resolve the jump later on
            _labels.Add(label, currentPosition);

            // Reset the position
            _codeStream.Position = currentPosition;

            // HACK: The machine code emitter needs to replace FP constants
            // with an EIP relative address, but these are not possible on x86,
            // so we store the EIP via a call in the right place on the stack
            //if (0 == label)
            //{
            //    // FIXME: This code doesn't need to be emitted if there are no
            //    // large constants used.
            //    _codeStream.WriteByte(0xE8);
            //    WriteImmediate(0);

            //    SigType i4 = new SigType(CilElementType.I4);

            //    RegisterOperand eax = new RegisterOperand(i4, GeneralPurposeRegister.EAX);
            //    Pop(eax);

            //    MemoryOperand mo = new MemoryOperand(i4, GeneralPurposeRegister.EBP, new IntPtr(-8));
            //    Mov(mo, eax);
            //}
        }

        public void Literal(int label, SigType type, object data)
        {
            // Save the current position
            long currentPosition = _codeStream.Position;
            // Relative branch offset
            int relOffset;
            // Flag, if we should really emit the literal (only if the literal is used!)
            bool emit = false;
            // Byte representation of the literal
            byte[] bytes;

            // Check if this label has forward references on it...
            emit = (0 != _literals.RemoveAll(delegate(Patch p)
            {
                if (p.label == label)
                {
                    _codeStream.Position = p.position;
                    // HACK: We can't do PIC right now
                    //relOffset = (int)currentPosition - ((int)p.position + 4);
                    relOffset = (int)currentPosition;
                    WriteImmediate(relOffset);
                    return true;
                }

                return false;
            }));

            if (true == emit)
            {
                _codeStream.Position = currentPosition;
                switch (type.Type)
                {
                    case CilElementType.I8:
                        bytes = BitConverter.GetBytes((long)data);
                        break;

                    case CilElementType.U8:
                        bytes = BitConverter.GetBytes((ulong)data);
                        break;

                    case CilElementType.R4:
                        bytes = BitConverter.GetBytes((double)data);
                        break;

                    case CilElementType.R8:
                        bytes = BitConverter.GetBytes((double)data);
                        break;

                    default:
                        throw new NotImplementedException();
                }

                Emit(bytes, bytes.Length);
            }
        }

        public void Add(Operand dest, Operand src)
        {
            // Write the opcode byte
            if (src is ConstantOperand)
                Emit(0x81, 0, dest, src);
            else
                Emit(0x03, 0, dest, src);
        }

        public void Call(RuntimeMethod method)
        {
            _codeStream.WriteByte(0xE8);

            // Calculate the relative call destination
            int relOffset = method.Address.ToInt32() - ((int)_codeStream.Position + 4);
            WriteImmediate(relOffset);
        }

        public void Call(int label)
        {
            _codeStream.WriteByte(0xE8);
            EmitRelativeBranchTarget(label);
        }

        public void Cli()
        {
            Emit(new byte[] { (byte)(0xFA) }, 1);
        }

        public void Cmp(Operand op1, Operand op2)
        {
            Emit(0x81, 7, op2, op1);
        }

        public void In(Operand dest, Operand src)
        {
            // TODO   
        }

        public void Out(Operand dest, Operand src)
        {
            // TODO   
        }

        public void Int3()
        {
            // FIXME: Use a code table for the instructions, so we don't allocate things all the time
            Emit(new byte[] { 0xCC });
        }

        public void Jae(int dest)
        {
            Emit(new byte[] { 0x0F, 0x83 }, 2);
            EmitRelativeBranchTarget(dest);
        }

        public void Jb(int dest)
        {
            Emit(new byte[] { 0x0F, 0x82 }, 2);
            EmitRelativeBranchTarget(dest);
        }

        public void Jmp(int dest)
        {
            _codeStream.WriteByte(0xE9);
            EmitRelativeBranchTarget(dest);
        }

        public void Nop()
        {
            Emit(new byte[] { 0x90 });
        }

        public void Mul(Operand dest, Operand src)
        {
            // Write the opcode byte
            Debug.Assert(dest is RegisterOperand && ((RegisterOperand)dest).Register is GeneralPurposeRegister && ((GeneralPurposeRegister)((RegisterOperand)dest).Register).RegisterCode == GeneralPurposeRegister.EAX.RegisterCode);
            Emit(0xF7, 4, src, null);
        }

        public void SseAdd(Operand dest, Operand src)
        {
            // Write the opcode byte
            Debug.Assert(dest is RegisterOperand && ((RegisterOperand)dest).Register is GeneralPurposeRegister && ((GeneralPurposeRegister)((RegisterOperand)dest).Register).RegisterCode == GeneralPurposeRegister.EAX.RegisterCode);
            // FIXME: Insert correct opcode here
            byte[] code = { 0xF2, 0x0F };
            Emit(code);
            Emit(0x58, 1, src, null);
        }

        public void SseSub(Operand dest, Operand src)
        {
            // Write the opcode byte
            Debug.Assert(dest is RegisterOperand && ((RegisterOperand)dest).Register is GeneralPurposeRegister && ((GeneralPurposeRegister)((RegisterOperand)dest).Register).RegisterCode == GeneralPurposeRegister.EAX.RegisterCode);
            // FIXME: Insert correct opcode here
            Emit(0xD8, 1, src, null);
        }

        public void SseMul(Operand dest, Operand src)
        {
            // Write the opcode byte
            //Debug.Assert(dest is RegisterOperand && ((RegisterOperand)dest).Register is SSE2Register && ((SSE2Register)((RegisterOperand)dest).Register).RegisterCode == SSE2Register.XMM0.RegisterCode);
            // HACK: Until we get a real register allocator (EAX == XMM0)
            Debug.Assert(dest is RegisterOperand && ((RegisterOperand)dest).Register is GeneralPurposeRegister && ((GeneralPurposeRegister)((RegisterOperand)dest).Register).RegisterCode == GeneralPurposeRegister.EAX.RegisterCode);
            // FIXME: Insert correct opcode here
            byte[] code = { 0xF2, 0x0F };
            Emit(code);
            Emit(0x59, 0, dest, src);
        }

        public void SseDiv(Operand dest, Operand src)
        {
            // Write the opcode byte
            Debug.Assert(dest is RegisterOperand && ((RegisterOperand)dest).Register is GeneralPurposeRegister && ((GeneralPurposeRegister)((RegisterOperand)dest).Register).RegisterCode == GeneralPurposeRegister.EAX.RegisterCode);
            // FIXME: Insert correct opcode here
            Emit(0xD8, 1, src, null);
        }

        public void Shl(Operand dest, Operand src)
        {
            // Write the opcode byte
            Debug.Assert(dest is RegisterOperand && (src is ConstantOperand || src is MemoryOperand));
            Emit(0xD1, 4, src, null);
        }

        public void Shr(Operand dest, Operand src)
        {
            // Write the opcode byte
            Debug.Assert(dest is RegisterOperand && (src is ConstantOperand || src is MemoryOperand));
            Emit(0xD1, 5, src, null);
        }

        public void Div(Operand dest, Operand src)
        {
            // Write the opcode byte
            Debug.Assert(dest is RegisterOperand && ((RegisterOperand)dest).Register is GeneralPurposeRegister && ((GeneralPurposeRegister)((RegisterOperand)dest).Register).RegisterCode == GeneralPurposeRegister.EAX.RegisterCode);
            Emit(0xF6, 6, src, null);
        }

        public void Mov(Operand dest, Operand src)
        {
            if (dest.StackType != StackTypeCode.F && src.StackType != StackTypeCode.F)
            {
                if (src is ConstantOperand)
                {
                    Emit(0xC7, 0, dest, src);
                }
                else if (dest is RegisterOperand)
                {
                    RegisterOperand ro = (RegisterOperand)dest;
                    Emit(0x8B, 0, dest, src);
                }
                else if (dest is MemoryOperand && src is RegisterOperand)
                {
                    Emit(0x89, 0, dest, src);
                }
                else if (dest is MemoryOperand && src is MemoryOperand)
                {
                    throw new NotSupportedException(@"Move from Memory to Memory not supported.");
                }
            }
            else if (src is LabelOperand)
            {
                // Move to a register only
                Debug.Assert(dest is RegisterOperand);
                byte[] code = new byte[] { 0xF2, 0x0F };
                Emit(code);
                Emit(0x10, 0, dest, src);
            }
            else if (dest is MemoryOperand && src is MemoryOperand)
            {
                throw new NotSupportedException(@"Move from Memory to Memory not supported.");
            }
            else
            {
                byte[] base_op_code = { 0xF2, 0x0F };

                if (dest is RegisterOperand)
                {
                    Emit(base_op_code);
                    Emit(0x10, 0, new RegisterOperand(new Mosa.Runtime.Metadata.Signatures.SigType(CilElementType.R8), SSE2Register.XMM0), src);
                }
                else if (dest is MemoryOperand)
                {
                    Emit(base_op_code);
                    Emit(0x11, 0, dest, new RegisterOperand(new Mosa.Runtime.Metadata.Signatures.SigType(CilElementType.R8), SSE2Register.XMM0));
                }
            }
        }

        public void Pop(Operand operand)
        {
            if (operand is RegisterOperand)
            {
                RegisterOperand ro = (RegisterOperand)operand;
                Emit(new byte[] { (byte)(0x58 + ro.Register.RegisterCode) }, 1);
            }
            else
            {
                Emit(0x8F, 0, operand);
            }
        }

        public void Push(Operand operand)
        {
            if (operand is ConstantOperand)
            {
                _codeStream.WriteByte(0x68);
                WriteImmediate((int)((ConstantOperand)operand).Value);
            }
            else if (operand is RegisterOperand)
            {
                RegisterOperand ro = (RegisterOperand)operand;
                Emit(new byte[] { (byte)(0x50 + ro.Register.RegisterCode) }, 1);
            }
            else
                Emit(0xFF, 6, operand);
        }

        public void Ret()
        {
            Emit(new byte[] { 0xC3 });
        }

        public void Sti()
        {
            Emit(new byte[] { (byte)(0xFB) }, 1);
        }

        public void Sub(Operand dest, Operand src)
        {
            // Write the opcode byte
            if (src is ConstantOperand)
                Emit(0x81, 5, dest, src);
            else if (dest is RegisterOperand)
                Emit(0x2B, 0, dest, src);
            else if (dest is MemoryOperand && src is RegisterOperand)
                Emit(0x29, 0, dest, src);
            else
                throw new NotImplementedException();
        }

        #endregion // ICodeEmitter Members

        #region Internals

        private byte CalculateModRM(byte reg, Operand dest, Operand src)
        {
            byte mod = 0, rm = 0;
            if (0 == reg)
            {
                if (dest is RegisterOperand)
                {
                    mod = 3;
                    RegisterOperand ro = (RegisterOperand)dest;
                    reg = (byte)ro.Register.RegisterCode;

                    if (src is RegisterOperand)
                    {
                        ro = (RegisterOperand)src;
                        rm = (byte)ro.Register.RegisterCode;
                    }
                }
                else if (dest is StackOperand)
                {
                    // [EBP+displacement]
                    mod = 2;
                    rm = 5;
                }
                else if (dest is ObjectFieldOperand)
                {
                    // [base+displacement]
                    mod = 2;
                    rm = 1;
                }
                else
                {
                    // [displacement]
                }
            }

            return (byte)(((mod & 3) << 6) | ((reg & 7) << 3) | (rm & 7));
        }

        private void EmitRelativeBranchTarget(int label)
        {
            // The relative offset of the label
            int relOffset = 0;
            // The position in the code stream of the label
            long position;

            // Did we see the label?
            if (true == _labels.TryGetValue(label, out position))
            {
                // Yes, calculate the relative offset
                relOffset = (int)position - ((int)_codeStream.Position + 4);
            }
            else
            {
                // Forward jump, we can't resolve yet - store a patch
                _patches.Add(new Patch(label, _codeStream.Position));
            }

            // Emit the relative jump offset (zero if we don't know it yet!)
            WriteImmediate(relOffset);
        }

        private void Emit(byte opcode, byte regField, Operand dest)
        {
            List<byte> code = new List<byte>();

            // Write the opcode
            code.Add(opcode);

            // Add the ModR/M byte
            code.Add(CalculateModRM(regField, dest, null));

            // Add the displacement of the destination
            AddDisplacementOrImmediate(code, dest, false);

            Emit(code.ToArray());
        }

        /// <summary>
        /// Builds the Scaled Index Base byte.
        /// </summary>
        /// <param name="param">The memory address to encode</param>
        /// <returns>The built SIB byte</returns>
        private static byte BuildSIBByte(MemoryOperand operand)
        {
            byte SIB = 0;
            //SIB |= (byte)((byte)param.Scale << 6);
            //SIB |= (byte)(param.BaseRegister.Index << 3);
            //SIB |= param.IndexRegister.Index;
            return SIB;
        }

        private void Emit(byte opcode, byte? regField, Operand dest, Operand src)
        {
            byte? sib = null;
            IntPtr? displacement = null;
            List<byte> code = new List<byte>();

            // Write the opcode
            code.Add(opcode);
            /*
                        // Add the ModR/M byte
                        code.Add(CalculateModRM(regField, dest, src));

                        // Add the displacement of the destination
                        AddDisplacementOrImmediate(code, dest, false);
            
                        // Add the immediate bytes
                        AddDisplacementOrImmediate(code, src, true);
            */
            MemoryOperand memAddrOp = null;

            //#region ModR/M byte & SIB byte
            if (null != dest)
            {
                byte modRM = 0;
                if (dest is RegisterOperand && src is RegisterOperand)
                {
                    modRM |= 3 << 6;
                    modRM |= (byte)(((RegisterOperand)dest).Register.RegisterCode << 3);
                    modRM |= (byte)((RegisterOperand)src).Register.RegisterCode;
                }
                else
                {
                    if (regField != null)
                    {
                        modRM |= (byte)(regField.Value << 3);
                    }
                    RegisterOperand registerOp = null;
                    if (dest is MemoryOperand)
                    {
                        memAddrOp = (MemoryOperand)dest;
                        registerOp = src as RegisterOperand;

                        displacement = memAddrOp.Offset;
                        if (null != memAddrOp.Base && displacement != IntPtr.Zero)
                        {
                            modRM |= (byte)((2 << 6) | memAddrOp.Base.RegisterCode);
                        }
                        else
                        {
                            modRM |= 5;
                        }

                        /* FIXME
                                                if (memAddrOp.Base != null && (memAddrOp.IndexRegister != null && (byte)memAddrOp.Scale > 0))
                                                {
                                                    SIBByte = BuildSIBByte(memAddrOp);
                                                    modRM |= 3;
                                                }
                                                else 
                         */
                        if (null != registerOp)
                            modRM |= (byte)(registerOp.Register.RegisterCode << 3);
                    }
                    else if (dest is RegisterOperand)
                    {
                        registerOp = (RegisterOperand)dest;
                        memAddrOp = src as MemoryOperand;

                        if (null == memAddrOp)
                        {
                            modRM |= 0xC0;
                        }
                        else
                        {
                            displacement = memAddrOp.Offset;
                            if (null != memAddrOp.Base && displacement != IntPtr.Zero)
                            {
                                modRM |= (byte)((2 << 6) | memAddrOp.Base.RegisterCode);
                            }
                            else
                            {
                                modRM |= 5;
                            }

                            /* FIXME
                            if (memAddrOp.Base != null && (memAddrOp.IndexRegister != null && (byte)memAddrOp.Scale > 0))
                            {
                                SIBByte = BuildSIBByte(memAddrOp);
                                modRM |= 3;
                            }
                            else 
*/
                        }

                        if (null != registerOp)
                            modRM |= (byte)registerOp.Register.RegisterCode;
                    }

#if FALSE
                    if (dest is MemoryOperand)
                        memAddrOp = (MemoryOperand)dest;
                    else if (src is MemoryOperand)
                        memAddrOp = (MemoryOperand)src;

                    if (src is RegisterOperand)
                        registerOp = (RegisterOperand)src;
                    else if (dest is RegisterOperand)
                        registerOp = (RegisterOperand)dest;


                    if (memAddrOp != null)
                    {
                        if (memAddrOp.Offset != null)
                        {
                            displacement = memAddrOp.Offset;
                            modRM |= 2 << 6;
                            if (memAddrOp.Base == null) // && memAddrOp.IndexRegister == null)
                            {
                                // displacement only
                                modRM |= 5;
                            }
                        }
/* FIXME
                        if (memAddrOp.Base != null && (memAddrOp.IndexRegister != null && (byte)memAddrOp.Scale > 0))
                        {
                            SIBByte = BuildSIBByte(memAddrOp);
                            modRM |= 3;
                        }
                        else 
 */
                        { 
                            modRM |= (byte)memAddrOp.Base.RegisterCode; 
                        }
                    }
                    if (null != registerOp)
                        modRM |= (byte)(registerOp.Register.RegisterCode << 3);
#endif // #if FALSE
                }
                code.Add(modRM);
            }

            if (sib != null)
            {
                code.Add(sib.Value);
            }

            /* Add displacement */
            if (null != displacement)
            {
                LabelOperand label = src as LabelOperand;
                if (null != label)
                {
                    // HACK: PIC and FP won't work for now, have to really fix this for moveable 
                    // jitted code though
                    displacement = IntPtr.Zero;
                    _literals.Add(new Patch(label.Label, _codeStream.Position + code.Count));
                }

                code.AddRange(BitConverter.GetBytes(displacement.Value.ToInt32()));
            }

            /* Add immediate bytes */
            if (dest is ConstantOperand)
                AddDisplacementOrImmediate(code, dest, true);
            if (src is ConstantOperand)
                AddDisplacementOrImmediate(code, src, true);

            Emit(code.ToArray(), code.Count);
        }

        private void AddDisplacementOrImmediate(List<byte> code, Operand op, bool immediate)
        {
            if (op is LocalVariableOperand)
            {
                // Add the displacement
                StackOperand so = (StackOperand)op;
                byte[] disp = BitConverter.GetBytes(so.Offset.ToInt32());
                //Array.Reverse(disp);
                code.AddRange(disp);
            }
            else if (op is LabelOperand)
            {
                _literals.Add(new Patch((op as LabelOperand).Label, _codeStream.Position + code.Count));
                code.AddRange(new byte[4]);
            }
            else if (op is MemoryOperand)
            {
                // Add the displacement
                MemoryOperand mo = (MemoryOperand)op;
                byte[] disp = BitConverter.GetBytes(mo.Offset.ToInt32());
                //Array.Reverse(disp);
                code.AddRange(disp);
            }
            else if (true == immediate && op is ConstantOperand)
            {
                // Add the immediate
                ConstantOperand co = (ConstantOperand)op;
                switch (op.Type.Type)
                {
                    case CilElementType.I:
                        code.AddRange(BitConverter.GetBytes((int)co.Value));
                        break;

                    case CilElementType.I4:
                        code.AddRange(BitConverter.GetBytes((int)co.Value));
                        break;

                    case CilElementType.I2:
                        code.AddRange(BitConverter.GetBytes((short)co.Value));
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            else if (op is RegisterOperand)
            {
                // Nothing to do...
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private void Emit(byte[] code)
        {
            Emit(code, code.Length);
        }

        private void Emit(byte[] code, int length)
        {
            _codeStream.Write(code, 0, length);
        }

        private void WriteImmediate(int value)
        {
            byte[] imm = BitConverter.GetBytes(value);
            _codeStream.Write(imm, 0, imm.Length);
        }

        #endregion // Internals
    }
}
