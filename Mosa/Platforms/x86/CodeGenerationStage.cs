/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 *  Phil Garcia (<mailto:phil@thinkedge.com>)
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

using Mosa.Runtime.CompilerFramework;
using IL = Mosa.Runtime.CompilerFramework.IL;
using IR = Mosa.Runtime.CompilerFramework.IR;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Vm;

namespace Mosa.Platforms.x86
{

	/// <summary>
    /// Generates appropriate x86 code using code emitters.
	/// </summary>
    /// <remarks>
    /// This code generation stage translates the three-address code used in the intermediate
    /// representation into an appropriate two-address code used by the x86 processor. We could
    /// attempt to do this in a seperate compilation stage, however this would create a large
    /// number of additional intermediate representation classes. There's potential to optimized
    /// register usage though. This should clearly be done after the results of this approach
    /// have been validated.
    /// </remarks>
    sealed class CodeGenerator : CodeGenerationStage, IX86InstructionVisitor, IL.IILVisitor, IR.IIrVisitor
    {
        #region Data members

        /// <summary>
        /// The emitter used by the code generator.
        /// </summary>
        private ICodeEmitter _emitter;

        #endregion // Data members

        #region Construction

        public CodeGenerator()
		{
		}

		#endregion // Construction

        #region Methods

        protected override void BeginGenerate()
        {
            FileInfo t = new FileInfo(String.Format("{0}.asm", this._compiler.Method.Name));
            StreamWriter textwriter = t.CreateText();
            MultiplexingCodeEmitter mce = new MultiplexingCodeEmitter();
            mce.Emitters.Add(new AsmCodeEmitter(textwriter));
            mce.Emitters.Add(new MachineCodeEmitter(_codeStream, _compiler.Linker));
            _emitter = mce;

            //_emitter = new MachineCodeEmitter(_codeStream);
        }

        protected override void EndGenerate()
        {
            _emitter.Dispose();
            _emitter = null;
        }

        protected override void BlockStart(BasicBlock block)
        {
            // Emit the block label
            _emitter.Label(block.Label);
        }

        #endregion // Methods

        #region IX86InstructionVisitor Members

        void IX86InstructionVisitor.Add(AddInstruction instruction)
        {
            _emitter.Add(instruction.Results[0], instruction.Operands[1]);
        }

        void IX86InstructionVisitor.Sub(SubInstruction instruction)
        {
            _emitter.Sub(instruction.Results[0], instruction.Operands[1]);
        }

        void IX86InstructionVisitor.Mul(MulInstruction instruction)
        {
            _emitter.Mul(instruction.Results[0], instruction.Operands[1]);
        }

        void IX86InstructionVisitor.Div(DivInstruction instruction)
        {
            _emitter.Div(instruction.Results[0], instruction.Operands[0]);
        }

        void IX86InstructionVisitor.SseAdd(SseAddInstruction instruction)
        {
            _emitter.SseAdd(instruction.Results[0], instruction.Operands[1]);
        }

        void IX86InstructionVisitor.SseSub(SseSubInstruction instruction)
        {
            _emitter.SseSub(instruction.Results[0], instruction.Operands[1]);
        }

        void IX86InstructionVisitor.SseMul(SseMulInstruction instruction)
        {
            _emitter.SseMul(instruction.Results[0], instruction.Operands[1]);
        }

        void IX86InstructionVisitor.SseDiv(SseDivInstruction instruction)
        {
            _emitter.SseDiv(instruction.Results[0], instruction.Operands[1]);
        }

        void IX86InstructionVisitor.Shift(x86.ShiftInstruction instruction)
        {
            if (instruction.Code == IL.OpCode.Shl)
                _emitter.Shl(instruction.Results[0], instruction.Operands[1]);
            else
                _emitter.Shr(instruction.Results[0], instruction.Operands[1]);
        }

        void IX86InstructionVisitor.Ldit(LditInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IX86InstructionVisitor.Cli(CliInstruction instruction)
        {
            _emitter.Cli();
        }

        void IX86InstructionVisitor.Sti(StiInstruction instruction)
        {
            _emitter.Sti();
        }

        void IX86InstructionVisitor.Call(CallInstruction instruction)
        {
            _emitter.Call(instruction.InvokeTarget);
        }

        #endregion // IX86InstructionVisitor Members

        #region IILVisitor Members

        void IL.IILVisitor.Nop(IL.NopInstruction instruction)
        {
            _emitter.Nop();
        }

        void IL.IILVisitor.Break(IL.BreakInstruction instruction)
        {
            _emitter.Int3();
        }

        void IL.IILVisitor.Ldarg(IL.LdargInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Ldarga(IL.LdargaInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Ldloc(IL.LdlocInstruction instruction)
        {
            //throw new NotImplementedException();
        }

        void IL.IILVisitor.Ldloca(IL.LdlocaInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Ldc(IL.LdcInstruction instruction)
        {
            //throw new NotImplementedException();
        }

        void IL.IILVisitor.Ldobj(IL.LdobjInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Ldstr(IL.LdstrInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Ldfld(IL.LdfldInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Ldflda(IL.LdfldaInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Ldsfld(IL.LdsfldInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Ldsflda(IL.LdsfldaInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Ldftn(IL.LdftnInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Ldvirtftn(IL.LdvirtftnInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Ldtoken(IL.LdtokenInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Stloc(IL.StlocInstruction instruction)
        {
            // Should never happen, the StlocInstruction expands itself into an IR.MoveInstruction
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Starg(IL.StargInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Stobj(IL.StobjInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Stfld(IL.StfldInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Stsfld(IL.StsfldInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Dup(IL.DupInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Pop(IL.PopInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Jmp(IL.JumpInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Call(IL.CallInstruction instruction)
        {
            // Move the this pointer to the right place, if this is an object instance
            RuntimeMethod method = instruction.InvokeTarget;
            if (true == method.Signature.HasThis)
                _emitter.Mov(new RegisterOperand(new SigType(Mosa.Runtime.Metadata.CilElementType.Object), GeneralPurposeRegister.ECX), instruction.ThisReference);
            
            /*
             * HINT: Microsoft seems not to use vtables/itables in .NET v2/v3/v3.5 anymore. They allocate
             * trampolines for virtual calls and rewrite them without indirect lookups if the object type
             * changes. This way they don't perform indirect lookups and the performance is drastically
             * improved.
             * 
             */

            // Do we need to emit a call with vtable lookup?
            Debug.Assert(MethodAttributes.Virtual != (MethodAttributes.Virtual & method.Attributes), @"call to a virtual function?");

            // A static call to the right address :)
            _emitter.Call(method);
        }

        void IL.IILVisitor.Calli(IL.CalliInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Ret(IL.ReturnInstruction instruction)
        {
            //_emitter.Ret();
            bool eax = false;
            if (0 != instruction.Operands.Length && null != instruction.Operands[0])
            {
                Operand retval = instruction.Operands[0];
                if (true == retval.IsRegister)
                {
                    // Do not move, if return value is already in EAX
                    RegisterOperand rop = (RegisterOperand)retval;
                    if (true == System.Object.ReferenceEquals(rop.Register, GeneralPurposeRegister.EAX))
                        eax = true;
                }

                if (false == eax)
                    _emitter.Mov(new RegisterOperand(new SigType(CilElementType.I), GeneralPurposeRegister.EAX), retval);
            }
        }

        void IL.IILVisitor.Branch(IL.BranchInstruction instruction)
        {
            _emitter.Jmp(instruction.BranchTargets[0]);

            //int rel = CalculateRelativeBranchTarget(instruction.BranchTargets[0], 5, new JmpRewrite(_emitter.Jmp));
            //_emitter.Jmp(new ConstantOperand(new SigType(CilElementType.I4), rel));
        }

        void IL.IILVisitor.UnaryBranch(IL.UnaryBranchInstruction instruction)
        {
            //throw new NotImplementedException();
        }

        void IL.IILVisitor.BinaryBranch(IL.BinaryBranchInstruction instruction)
        {
            bool swap = instruction.First is ConstantOperand;
            int[] targets = instruction.BranchTargets;
            if (true == swap)
            {
                int tmp = targets[0];
                targets[0] = targets[1];
                targets[1] = tmp;

                _emitter.Cmp(instruction.Second, instruction.First);
                switch (instruction.Code)
                {
                    // Signed
                    case IL.OpCode.Beq_s: _emitter.Jne(targets[0]); break;
                    case IL.OpCode.Bge_s: _emitter.Jl(targets[0]); break;
                    case IL.OpCode.Bgt_s: _emitter.Jle(targets[0]); break;
                    case IL.OpCode.Ble_s: _emitter.Jg(targets[0]); break;
                    case IL.OpCode.Blt_s: _emitter.Jge(targets[0]); break;

                    // Unsigned
                    case IL.OpCode.Bne_un_s: _emitter.Je(targets[0]); break;
                    case IL.OpCode.Bge_un_s: _emitter.Jb(targets[0]); break;
                    case IL.OpCode.Bgt_un_s: _emitter.Jbe(targets[0]); break;
                    case IL.OpCode.Ble_un_s: _emitter.Ja(targets[0]); break;
                    case IL.OpCode.Blt_un_s: _emitter.Jae(targets[0]); break;

                    // Long form signed
                    case IL.OpCode.Beq: goto case IL.OpCode.Beq_s;
                    case IL.OpCode.Bge: goto case IL.OpCode.Bge_s;
                    case IL.OpCode.Bgt: goto case IL.OpCode.Bgt_s;
                    case IL.OpCode.Ble: goto case IL.OpCode.Ble_s;
                    case IL.OpCode.Blt: goto case IL.OpCode.Blt_s;

                    // Long form unsigned
                    case IL.OpCode.Bne_un: goto case IL.OpCode.Bne_un_s;
                    case IL.OpCode.Bge_un: goto case IL.OpCode.Bge_un_s;
                    case IL.OpCode.Bgt_un: goto case IL.OpCode.Bgt_un_s;
                    case IL.OpCode.Ble_un: goto case IL.OpCode.Ble_un_s;
                    case IL.OpCode.Blt_un: goto case IL.OpCode.Blt_un_s;

                    default:
                        throw new NotImplementedException();
                }
            }
            else
            {
                _emitter.Cmp(instruction.First, instruction.Second);
                switch (instruction.Code)
                {
                    // Signed
                    case IL.OpCode.Beq_s: _emitter.Je(targets[0]); break;
                    case IL.OpCode.Bge_s: _emitter.Jge(targets[0]); break;
                    case IL.OpCode.Bgt_s: _emitter.Jg(targets[0]); break;
                    case IL.OpCode.Ble_s: _emitter.Jle(targets[0]); break;
                    case IL.OpCode.Blt_s: _emitter.Jl(targets[0]); break;

                    // Unsigned
                    case IL.OpCode.Bne_un_s: _emitter.Jne(targets[0]); break;
                    case IL.OpCode.Bge_un_s: _emitter.Jae(targets[0]); break;
                    case IL.OpCode.Bgt_un_s: _emitter.Ja(targets[0]); break;
                    case IL.OpCode.Ble_un_s: _emitter.Jbe(targets[0]); break;
                    case IL.OpCode.Blt_un_s: _emitter.Jb(targets[0]); break;

                    // Long form signed
                    case IL.OpCode.Beq: goto case IL.OpCode.Beq_s;
                    case IL.OpCode.Bge: goto case IL.OpCode.Bge_s;
                    case IL.OpCode.Bgt: goto case IL.OpCode.Bgt_s;
                    case IL.OpCode.Ble: goto case IL.OpCode.Ble_s;
                    case IL.OpCode.Blt: goto case IL.OpCode.Blt_s;

                    // Long form unsigned
                    case IL.OpCode.Bne_un: goto case IL.OpCode.Bne_un_s;
                    case IL.OpCode.Bge_un: goto case IL.OpCode.Bge_un_s;
                    case IL.OpCode.Bgt_un: goto case IL.OpCode.Bgt_un_s;
                    case IL.OpCode.Ble_un: goto case IL.OpCode.Ble_un_s;
                    case IL.OpCode.Blt_un: goto case IL.OpCode.Blt_un_s;

                    default:
                        throw new NotImplementedException();
                }
            }

            // Emit a regular jump for the second case
            _emitter.Jmp(targets[1]);
        }

        void IL.IILVisitor.Switch(IL.SwitchInstruction instruction)
        {
            foreach (int target in instruction.BranchTargets)
                _emitter.Jmp(target);
            //throw new NotImplementedException();
        }

        void IL.IILVisitor.Add(IL.AddInstruction instruction)
        {
            _emitter.Add(instruction.Results[0], instruction.Operands[1]);
        }

        void IL.IILVisitor.Div(IL.DivInstruction instruction)
        {
            _emitter.Div(instruction.Results[0], instruction.Operands[1]);
        }

        void IL.IILVisitor.Mul(IL.MulInstruction instruction)
        {
            _emitter.Mul(instruction.Results[0], instruction.Operands[1]);
        }

        void IL.IILVisitor.Rem(IL.RemInstruction instruction)
        {
        }

        void IL.IILVisitor.Sub(IL.SubInstruction instruction)
        {
        }

        void IL.IILVisitor.BinaryLogic(IL.BinaryLogicInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Shift(IL.ShiftInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Neg(IL.NegInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Not(IL.NotInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Conversion(IL.ConversionInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Callvirt(IL.CallvirtInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Cpobj(IL.CpobjInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Newobj(IL.NewobjInstruction instruction)
        {
            UnsupportedInstruction();
        }

        void IL.IILVisitor.Castclass(IL.CastclassInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Isinst(IL.IsInstInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Unbox(IL.UnboxInstruction instruction)
        {
            UnsupportedInstruction();
        }

        void IL.IILVisitor.Throw(IL.ThrowInstruction instruction)
        {
            UnsupportedInstruction();
        }

        void IL.IILVisitor.Box(IL.BoxInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Newarr(IL.NewarrInstruction instruction)
        {
            UnsupportedInstruction();
        }

        void IL.IILVisitor.Ldlen(IL.LdlenInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Ldelema(IL.LdelemaInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Ldelem(IL.LdelemInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Stelem(IL.StelemInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.UnboxAny(IL.UnboxAnyInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Refanyval(IL.RefanyvalInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.UnaryArithmetic(IL.UnaryArithmeticInstruction instruction)
        {
            //throw new NotImplementedException();
        }

        void IL.IILVisitor.Mkrefany(IL.MkrefanyInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.ArithmeticOverflow(IL.ArithmeticOverflowInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Endfinally(IL.EndfinallyInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Leave(IL.LeaveInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Arglist(IL.ArglistInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.BinaryComparison(IL.BinaryComparisonInstruction instruction)
        {
            //throw new NotImplementedException();
        }

        void IL.IILVisitor.Localalloc(IL.LocalallocInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Endfilter(IL.EndfilterInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.InitObj(IL.InitObjInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Cpblk(IL.CpblkInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Initblk(IL.InitblkInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Prefix(IL.PrefixInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Rethrow(IL.RethrowInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Sizeof(IL.SizeofInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Refanytype(IL.RefanytypeInstruction instruction)
        {
            throw new NotImplementedException();
        }

        #endregion // IILVisitor Members

        #region x86 Encoding Methods

        protected override void EmitInstructions()
        {
            base.EmitInstructions();

            /* FIXME: 
             * - We really need to emit proper x86 code
             * - This stub just allows me to finish the cltester
             * 
             * Hard coded x86 instruction sequence below is:
             * MOV EAX, 8
             * ADD EAX, 8
             * RET
             */
/*
            byte[] code = new byte[] { 0xB8, 0x08, 0x00, 0x00, 0x00, 0x05, 0x08, 0x00, 0x00, 0x00, 0xC3 };
            _codeStream.Write(code, 0, code.Length);
 */
        }

        private void Encode(byte[] code, byte? regFieldOpCode, Operand operand1, Operand operand2, Operand immediate)
        {

/*
            byte? SIBByte = null;
            ValueType displacement = null;

            List<byte> byteStream = new List<byte>();

            #region Prefixes
            if ((prefixes & PrefixFlags.LOCK) == PrefixFlags.LOCK) { byteStream.Add(0xF0); }

            if ((prefixes & PrefixFlags.REP_REPZ) == PrefixFlags.REP_REPZ) { byteStream.Add(0xF3); }
            else if ((prefixes & PrefixFlags.REPNE_REPNZ) == PrefixFlags.REPNE_REPNZ) { byteStream.Add(0xF2); }

            if ((operand1 != null && (operand1 is Memory16Bit || operand1 is Register16Bit)) ||
                (immediateData != null && (immediateData is short || immediateData is ushort)) ||
                sizeOverride)
            { byteStream.Add(0x66); }
            #endregion

            byteStream.AddRange(opCode);

            #region ModR/M byte & SIB byte
            if (operand1 != null)
            {
                byte modRM = 0;
                if (operand1 is RegisterBase && operand2 is RegisterBase)
                {
                    modRM |= 3 << 6;
                    modRM |= (byte)(((RegisterBase)operand2).Index << 3);
                    modRM |= ((RegisterBase)operand1).Index;
                }
                else
                {
                    if (regFieldOpCode != null) { modRM |= (byte)(regFieldOpCode.Value << 3); }

                    MemoryBase memAddrOp;
                    RegisterBase registerOp;
                    if (operand1 is MemoryBase)
                    {
                        memAddrOp = (MemoryBase)operand1;
                        registerOp = (RegisterBase)operand2;
                    }
                    else
                    {
                        memAddrOp = (MemoryBase)operand2;
                        registerOp = (RegisterBase)operand1;
                    }
                    if (memAddrOp != null)
                    {
                        if (memAddrOp.Displacement != null)
                        {
                            displacement = memAddrOp.Displacement;
                            modRM |= 2 << 6;
                            if (memAddrOp.BaseRegister == null && memAddrOp.IndexRegister == null)
                            {
                                // displacement only
                                modRM |= 5;
                            }
                        }
                        if (memAddrOp.BaseRegister != null && (memAddrOp.IndexRegister != null && (byte)memAddrOp.Scale > 0))
                        {
                            SIBByte = BuildSIBByte(memAddrOp);
                            modRM |= 3;
                        }
                        else { modRM |= memAddrOp.BaseRegister.Index; }
                    }
                    modRM |= (byte)(registerOp.Index << 3);
                }
                byteStream.Add(modRM);
            }

            if (SIBByte != null) { byteStream.Add(SIBByte.Value); }

            if (displacement != null)
            {
                if (displacement is byte) { byteStream.Add((byte)displacement); }
                else if (displacement is short) { byteStream.AddRange(BitConverter.GetBytes((short)displacement)); }
                else if (displacement is int) { byteStream.AddRange(BitConverter.GetBytes((int)displacement)); }
            }
            #endregion

            #region Immediate data
            if (immediateData != null)
            {
                ValueType data = immediateData;

                #region IP-relative address calculation
                if ((prefixes & PrefixFlags.RELATIVE_ADDRESS) == PrefixFlags.RELATIVE_ADDRESS)
                {
                    if (data is byte) { data = (byte)data - (IpPosition + byteStream.Count + 1); }
                    else if (data is short) { data = (short)data - (IpPosition + byteStream.Count + 2); }
                    else if (data is int) { data = (int)data - (IpPosition + byteStream.Count + 4); }
                }
                #endregion

                if (data is byte) { byteStream.Add((byte)data); }
                else
                {
                    if (data is short) { byteStream.AddRange(BitConverter.GetBytes((ushort)data)); }
                    else if (data is int) { byteStream.AddRange(BitConverter.GetBytes((int)data)); }
                }
            }
            #endregion
 */ 
        }

        #endregion // x86 Encoding Methods

        #region Internal Helpers

        private void UnsupportedInstruction()
        {
            throw new NotSupportedException(@"Instruction can not be emitted and requires appropriate expansion and runtime support.");
        }

        #endregion // Internal Helpers

        #region IIrVisitor Members

        void IR.IIrVisitor.Visit(IR.EpilogueInstruction instruction)
        {
            // Epilogue instruction should have been expanded in a stage before ours.
            throw new NotSupportedException();
        }

        void IR.IIrVisitor.Visit(IR.LiteralInstruction instruction)
        {
            _emitter.Literal(instruction.Label, instruction.Type, instruction.Data);
        }

        void IR.IIrVisitor.Visit(IR.LogicalAndInstruction instruction)
        {
            _emitter.And(instruction.Destination, instruction.Operand2);
        }

        void IR.IIrVisitor.Visit(IR.LogicalOrInstruction instruction)
        {
            _emitter.Or(instruction.Destination, instruction.Operand2);
        }

        void IR.IIrVisitor.Visit(IR.LogicalXorInstruction instruction)
        {
            _emitter.Xor(instruction.Destination, instruction.Operand2);
        }

        void IR.IIrVisitor.Visit(IR.LogicalNotInstruction instruction)
        {
            _emitter.Not(instruction.Destination);
        }

        void IR.IIrVisitor.Visit(IR.MoveInstruction instruction)
        {
            Operand dst = instruction.Results[0], src = instruction.Operands[0];

            // FIXME: This should actually be expanded somewhere else
            if (src is LabelOperand)
            {
                switch (src.Type.Type)
                {
                    case CilElementType.R4:
                        goto case CilElementType.R8;

                    case CilElementType.R8:
                        Operand tmp = new RegisterOperand(src.Type, SSE2Register.XMM0);
                        _emitter.Mov(tmp, src);
                        _emitter.Mov(dst, tmp);
                        break;

                    case CilElementType.I8:
                        goto case CilElementType.U8;

                    case CilElementType.U8:
                        throw new NotImplementedException();

                    case CilElementType.Object:
                        throw new NotImplementedException();
                }
            }
            else
            {
                _emitter.Mov(dst, src);
            }
        }

        void IR.IIrVisitor.Visit(IR.PopInstruction instruction)
        {
            _emitter.Pop(instruction.Destination);
        }

        void IR.IIrVisitor.Visit(IR.PrologueInstruction instruction)
        {
            // Prologue instruction should have been expanded in a stage before ours.
            throw new NotSupportedException();
        }

        void IR.IIrVisitor.Visit(IR.PushInstruction instruction)
        {
            _emitter.Push(instruction.Source);
        }

        void IR.IIrVisitor.Visit(IR.ReturnInstruction instruction)
        {
            _emitter.Ret();
        }

        #endregion // IIrVisitor Members
    }
}
