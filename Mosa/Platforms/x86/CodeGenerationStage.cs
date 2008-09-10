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
    sealed class CodeGenerator : CodeGenerationStage<int>, IX86InstructionVisitor<int>, IL.IILVisitor<int>, IR.IIrVisitor<int>
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
            mce.Emitters.Add(new MachineCodeEmitter(_compiler, _codeStream, _compiler.Linker));
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

        void IX86InstructionVisitor<int>.Add(AddInstruction instruction, int arg)
        {
            _emitter.Add(instruction.Results[0], instruction.Operands[1]);
        }

        void IX86InstructionVisitor<int>.Adc(AdcInstruction instruction, int arg)
        {
            _emitter.Adc(instruction.Results[0], instruction.Operands[1]);
        }

        void IX86InstructionVisitor<int>.Sub(SubInstruction instruction, int arg)
        {
            _emitter.Sub(instruction.Results[0], instruction.Operands[1]);
        }

        void IX86InstructionVisitor<int>.Mul(MulInstruction instruction, int arg)
        {
            if (instruction.Results[0].StackType == StackTypeCode.Int64 || instruction.Operands[1].StackType == StackTypeCode.Int64)
                MulI8(instruction.Results[0], instruction.Operands[1]);
            else
                _emitter.Mul(instruction.Results[0], instruction.Operands[1]);
        }

        void IX86InstructionVisitor<int>.Div(DivInstruction instruction, int arg)
        {
            _emitter.Div(instruction.Results[0], instruction.Operands[0]);
        }

        void IX86InstructionVisitor<int>.SseAdd(SseAddInstruction instruction, int arg)
        {
            _emitter.SseAdd(instruction.Results[0], instruction.Operands[1]);
        }

        void IX86InstructionVisitor<int>.SseSub(SseSubInstruction instruction, int arg)
        {
            _emitter.SseSub(instruction.Results[0], instruction.Operands[1]);
        }

        void IX86InstructionVisitor<int>.SseMul(SseMulInstruction instruction, int arg)
        {
            _emitter.SseMul(instruction.Results[0], instruction.Operands[1]);
        }

        void IX86InstructionVisitor<int>.SseDiv(SseDivInstruction instruction, int arg)
        {
            _emitter.SseDiv(instruction.Results[0], instruction.Operands[1]);
        }

        void IX86InstructionVisitor<int>.Shift(x86.ShiftInstruction instruction, int arg)
        {
            if (instruction.Code == IL.OpCode.Shl)
                _emitter.Shl(instruction.Results[0], instruction.Operands[1]);
            else
                _emitter.Shr(instruction.Results[0], instruction.Operands[1]);
        }

        void IX86InstructionVisitor<int>.Ldit(LditInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IX86InstructionVisitor<int>.Cli(CliInstruction instruction, int arg)
        {
            _emitter.Cli();
        }

        void IX86InstructionVisitor<int>.Sti(StiInstruction instruction, int arg)
        {
            _emitter.Sti();
        }

        void IX86InstructionVisitor<int>.Call(CallInstruction instruction, int arg)
        {
            _emitter.Call(instruction.InvokeTarget);
        }

        #endregion // IX86InstructionVisitor Members

        #region IILVisitor Members

        void IL.IILVisitor<int>.Nop(IL.NopInstruction instruction, int arg)
        {
            _emitter.Nop();
        }

        void IL.IILVisitor<int>.Break(IL.BreakInstruction instruction, int arg)
        {
            _emitter.Int3();
        }

        void IL.IILVisitor<int>.Ldarg(IL.LdargInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Ldarga(IL.LdargaInstruction instruction, int arg)
        {
            _emitter.Mov(instruction.Results[0], new RegisterOperand(new SigType(CilElementType.Ptr), GeneralPurposeRegister.EBP));
            _emitter.Add(instruction.Results[0], new ConstantOperand(new SigType(CilElementType.Ptr), instruction.Offset));
        }

        void IL.IILVisitor<int>.Ldloc(IL.LdlocInstruction instruction, int arg)
        {
            //throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Ldloca(IL.LdlocaInstruction instruction, int arg)
        {
            _emitter.Mov(instruction.Results[0], new RegisterOperand(instruction.Results[0].Type, GeneralPurposeRegister.EBP));
            _emitter.Add(instruction.Results[0], new ConstantOperand(instruction.Results[0].Type, instruction.Offset));
        }

        void IL.IILVisitor<int>.Ldc(IL.LdcInstruction instruction, int arg)
        {
            //throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Ldobj(IL.LdobjInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Ldstr(IL.LdstrInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Ldfld(IL.LdfldInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Ldflda(IL.LdfldaInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Ldsfld(IL.LdsfldInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Ldsflda(IL.LdsfldaInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Ldftn(IL.LdftnInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Ldvirtftn(IL.LdvirtftnInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Ldtoken(IL.LdtokenInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Stloc(IL.StlocInstruction instruction, int arg)
        {
            // Should never happen, the StlocInstruction expands itself into an IR.MoveInstruction
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Starg(IL.StargInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Stobj(IL.StobjInstruction instruction, int arg)
        {
            //throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Stfld(IL.StfldInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Stsfld(IL.StsfldInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Dup(IL.DupInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Pop(IL.PopInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Jmp(IL.JumpInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Call(IL.CallInstruction instruction, int arg)
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

        void IL.IILVisitor<int>.Calli(IL.CalliInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Ret(IL.ReturnInstruction instruction, int arg)
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

        void IL.IILVisitor<int>.Branch(IL.BranchInstruction instruction, int arg)
        {
            _emitter.Jmp(instruction.BranchTargets[0]);

            //int rel = CalculateRelativeBranchTarget(instruction.BranchTargets[0], 5, new JmpRewrite(_emitter.Jmp));
            //_emitter.Jmp(new ConstantOperand(new SigType(CilElementType.I4), rel));
        }

        void IL.IILVisitor<int>.UnaryBranch(IL.UnaryBranchInstruction instruction, int arg)
        {
            //throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.BinaryBranch(IL.BinaryBranchInstruction instruction, int arg)
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

        void IL.IILVisitor<int>.Switch(IL.SwitchInstruction instruction, int arg)
        {
            for (int i = 0; i < instruction.BranchTargets.Length; i++)
            {
                _emitter.Cmp(instruction.Operands[0], new ConstantOperand(new SigType(CilElementType.I), i));
                _emitter.Je(instruction.BranchTargets[i]);
            }
            //throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Add(IL.AddInstruction instruction, int arg)
        {
            _emitter.Add(instruction.Results[0], instruction.Operands[1]);
        }

        void IL.IILVisitor<int>.Div(IL.DivInstruction instruction, int arg)
        {
            _emitter.Div(instruction.Results[0], instruction.Operands[1]);
        }

        void IL.IILVisitor<int>.Mul(IL.MulInstruction instruction, int arg)
        {
            _emitter.Mul(instruction.Results[0], instruction.Operands[1]);
        }

        void IL.IILVisitor<int>.Rem(IL.RemInstruction instruction, int arg)
        {
        }

        void IL.IILVisitor<int>.Sub(IL.SubInstruction instruction, int arg)
        {
        }

        void IL.IILVisitor<int>.BinaryLogic(IL.BinaryLogicInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Shift(IL.ShiftInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Neg(IL.NegInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Not(IL.NotInstruction instruction, int arg)
        {
            _emitter.Not(instruction.Operands[0]);
        }

        void IL.IILVisitor<int>.Conversion(IL.ConversionInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Callvirt(IL.CallvirtInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Cpobj(IL.CpobjInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Newobj(IL.NewobjInstruction instruction, int arg)
        {
            UnsupportedInstruction();
        }

        void IL.IILVisitor<int>.Castclass(IL.CastclassInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Isinst(IL.IsInstInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Unbox(IL.UnboxInstruction instruction, int arg)
        {
            UnsupportedInstruction();
        }

        void IL.IILVisitor<int>.Throw(IL.ThrowInstruction instruction, int arg)
        {
            UnsupportedInstruction();
        }

        void IL.IILVisitor<int>.Box(IL.BoxInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Newarr(IL.NewarrInstruction instruction, int arg)
        {
            UnsupportedInstruction();
        }

        void IL.IILVisitor<int>.Ldlen(IL.LdlenInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Ldelema(IL.LdelemaInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Ldelem(IL.LdelemInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Stelem(IL.StelemInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.UnboxAny(IL.UnboxAnyInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Refanyval(IL.RefanyvalInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.UnaryArithmetic(IL.UnaryArithmeticInstruction instruction, int arg)
        {
            //throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Mkrefany(IL.MkrefanyInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.ArithmeticOverflow(IL.ArithmeticOverflowInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Endfinally(IL.EndfinallyInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Leave(IL.LeaveInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Arglist(IL.ArglistInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.BinaryComparison(IL.BinaryComparisonInstruction instruction, int arg)
        {
            _emitter.Cmp(instruction.First, instruction.Second);
            _emitter.Setcc(instruction.Code);
        }

        void IL.IILVisitor<int>.Localalloc(IL.LocalallocInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Endfilter(IL.EndfilterInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.InitObj(IL.InitObjInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Cpblk(IL.CpblkInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Initblk(IL.InitblkInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Prefix(IL.PrefixInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Rethrow(IL.RethrowInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Sizeof(IL.SizeofInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Refanytype(IL.RefanytypeInstruction instruction, int arg)
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

        private void MulI8(Operand dest, Operand src)
        {
            RegisterOperand eax = new RegisterOperand(new SigType(CilElementType.I), GeneralPurposeRegister.EAX);
            RegisterOperand ebx = new RegisterOperand(new SigType(CilElementType.I), GeneralPurposeRegister.EBX);
            RegisterOperand ecx = new RegisterOperand(new SigType(CilElementType.I), GeneralPurposeRegister.ECX);
            RegisterOperand edx = new RegisterOperand(new SigType(CilElementType.I), GeneralPurposeRegister.EDX);

            _emitter.Push(eax);
            _emitter.Push(ebx);
            _emitter.Push(ecx);
            _emitter.Push(edx);

            //_emitter.Mov(eax, HIWORD(A));
            //_emitter.Mov(ecx, HIWORD(B));
            _emitter.Or(ecx, eax);
            //_emitter.Mov(ecx, LOWORD(B));

            // jnz short hard

            //_emitter.Mov(eax, LOWORD(A));
            _emitter.Mul(eax, ecx); 

            // ret 16

            // hard:

            _emitter.Push(ebx);

            _emitter.Mul(eax, ecx);
            _emitter.Mov(ebx, eax);
            //_emitter.Mov(eax, LOWORD(A2));
            //_emitter.Mul(null, HIWORD(B2));
            _emitter.Add(ebx, eax);

            //_emitter.Mov(eax, LOWORD(A2));
            _emitter.Mul(eax, ecx);
            _emitter.Add(edx, ebx);

            _emitter.Pop(ebx);

            // ret 16

            _emitter.Pop(edx);
            _emitter.Pop(ecx);
            _emitter.Pop(ebx);
            _emitter.Pop(eax);
        }

        #endregion // Internal Helpers

        #region IIrVisitor Members

        void IR.IIrVisitor<int>.Visit(IR.EpilogueInstruction instruction, int arg)
        {
            // Epilogue instruction should have been expanded in a stage before ours.
            throw new NotSupportedException();
        }

        void IR.IIrVisitor<int>.Visit(IR.LiteralInstruction instruction, int arg)
        {
            _emitter.Literal(instruction.Label, instruction.Type, instruction.Data);
        }

        void IR.IIrVisitor<int>.Visit(IR.LogicalAndInstruction instruction, int arg)
        {
            _emitter.And(instruction.Destination, instruction.Operand2);
        }

        void IR.IIrVisitor<int>.Visit(IR.LogicalOrInstruction instruction, int arg)
        {
            _emitter.Or(instruction.Destination, instruction.Operand2);
        }

        void IR.IIrVisitor<int>.Visit(IR.LogicalXorInstruction instruction, int arg)
        {
            _emitter.Xor(instruction.Destination, instruction.Operand2);
        }

        void IR.IIrVisitor<int>.Visit(IR.LogicalNotInstruction instruction, int arg)
        {
            _emitter.Not(instruction.Destination);
        }

        void IR.IIrVisitor<int>.Visit(IR.MoveInstruction instruction, int arg)
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
                        tmp = new RegisterOperand(src.Type, MMXRegister.MM0);
                        _emitter.Mov(tmp, src);
                        _emitter.Mov(dst, tmp);
                        break;

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

        void IR.IIrVisitor<int>.Visit(IR.PhiInstruction instruction, int arg)
        {
            throw new NotSupportedException(@"PHI functions should've been removed by the LeaveSSA stage.");
        }

        void IR.IIrVisitor<int>.Visit(IR.PopInstruction instruction, int arg)
        {
            _emitter.Pop(instruction.Destination);
        }

        void IR.IIrVisitor<int>.Visit(IR.PrologueInstruction instruction, int arg)
        {
            // Prologue instruction should have been expanded in a stage before ours.
            throw new NotSupportedException();
        }

        void IR.IIrVisitor<int>.Visit(IR.PushInstruction instruction, int arg)
        {
            _emitter.Push(instruction.Source);
        }

        void IR.IIrVisitor<int>.Visit(IR.ReturnInstruction instruction, int arg)
        {
            _emitter.Ret();
        }

        void IR.IIrVisitor<int>.Visit(IR.SignExtendedMoveInstruction instruction, int arg)
        {
            switch (instruction.Destination.Type.Type)
            {
                case CilElementType.I1:
                    _emitter.Movsx(instruction.Destination, instruction.Source);
                    break;

                case CilElementType.I2: goto case CilElementType.I1;

                case CilElementType.I4: goto case CilElementType.I1;

                case CilElementType.I8:
                    _emitter.Mov(instruction.Destination, instruction.Source);
                    break;

                case CilElementType.R4:
                    _emitter.Mov(instruction.Destination, instruction.Source);
                    break;

                default:
                    throw new NotSupportedException();
            }
        }

        void IR.IIrVisitor<int>.Visit(IR.ZeroExtendedMoveInstruction instruction, int arg)
        {
            switch (instruction.Destination.Type.Type)
            {
                case CilElementType.I1:
                    _emitter.Movzx(instruction.Destination, instruction.Source);
                    break;

                case CilElementType.I2: goto case CilElementType.I1;

                case CilElementType.I4: goto case CilElementType.I1;

                case CilElementType.I8:
                    throw new NotImplementedException();

                default:
                    throw new NotSupportedException();
            }
        }

        #endregion // IIrVisitor Members
    }
}
