/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.TypeSystem;
using Mosa.Platform.x86;
using Mosa.TinyCPUSimulator.Adaptor;
using Mosa.TinyCPUSimulator.x86.Emulate;
using System.Collections.Generic;

namespace Mosa.TinyCPUSimulator.x86.Adaptor
{
	public class SimAdapter : BaseSetup<CPUx86>, ISimAdapter
	{
		public SimAdapter()
		{
			Initialize();
		}

		public override void Initialize()
		{
		}

		SimCPU ISimAdapter.SimCPU { get { return this.CPU; } }

		SimMonitor ISimAdapter.Monitor { get { return this.Monitor; } }

		void ISimAdapter.Reset()
		{
			CPU.Reset();
			//CPU.Execute();
		}

		void ISimAdapter.Execute()
		{
			CPU.Execute();
		}

		SimInstruction ISimAdapter.Convert(Context context, RuntimeMethod method, BasicBlocks basicBlocks, byte opcodeSize)
		{
			X86Instruction x86Instruction = context.Instruction as X86Instruction;

			if (x86Instruction == null)
				return null;

			BaseOpcode opcode = ConvertToOpcode(x86Instruction, context.ConditionCode);

			if (opcode == null)
				return null;

			var operands = GetOperands(context, method, basicBlocks);

			AdjustInstructionOperands(opcode, operands);

			SimInstruction instruction = null;

			switch (operands.Count)
			{
				case 0: instruction = new SimInstruction(opcode, opcodeSize); break;
				case 1: instruction = new SimInstruction(opcode, opcodeSize, operands[0]); break;
				case 2: instruction = new SimInstruction(opcode, opcodeSize, operands[0], operands[1]); break;
				case 3: instruction = new SimInstruction(opcode, opcodeSize, operands[0], operands[1], operands[2]); break;
				case 4: instruction = new SimInstruction(opcode, opcodeSize, operands[0], operands[1], operands[2], operands[3]); break;
				default: break;
			}

			return instruction;
		}

		void ISimAdapter.AddInstruction(ulong address, SimInstruction instruction)
		{
			Add(address, instruction);
		}

		void ISimAdapter.SetLabel(string label, ulong address)
		{
			CPU.SetLabel(label, address);
		}

		private List<SimOperand> GetOperands(Context context, RuntimeMethod method, BasicBlocks basicBlocks)
		{
			List<SimOperand> operands = new List<SimOperand>();

			if (context.ResultCount != 0)
				operands.Add(ConvertToOpcodeOperand(context.Result));

			foreach (var operand in context.Operands)
				operands.Add(ConvertToOpcodeOperand(operand));

			if (context.BranchTargets != null)
			{
				foreach (var target in context.BranchTargets)
				{
					var block = basicBlocks.GetByLabel(target);

					operands.Add(CreateLabel(32, block.ToString() + ":" + method.FullName));
				}
			}

			return operands;
		}

		private static void AdjustInstructionOperands(BaseOpcode opcode, List<SimOperand> operands)
		{
			if (IsSecondOperandDuplicate(opcode))
			{
				operands.RemoveAt(1);
			}

			if (opcode == Opcode.Mul) { operands.RemoveAt(0); }
			else if (opcode == Opcode.Div) { operands.RemoveAt(0); operands.RemoveAt(0); }
			else if (opcode == Opcode.Idiv) { operands.RemoveAt(0); operands.RemoveAt(0); }
			else if (opcode == Opcode.Cdq) { operands.Clear(); }
			else if (opcode == Opcode.Xchg) { operands.RemoveAt(2); }
			else if (opcode == Opcode.Imul)
			{
				if (operands[0] == operands[1]) operands.RemoveAt(0);
			}
		}

		private SimOperand ConvertToOpcodeOperand(Operand operand)
		{
			int size = GetSize(operand.Type);

			if (operand.IsConstant)
			{
				return CreateImmediate((ulong)operand.ValueAsLongInteger, size);
			}
			else if (operand.IsRegister)
			{
				return CreateOperand(AdjustRegisterSize(ConvertToRegister(operand.Register), size));
			}
			else if (operand.IsLabel)
			{
				if (operand.IsMemoryAddress)
					return CreateMemoryAddressLabel(size, operand.Name);
				else
					return CreateLabel(size, operand.Name);
			}
			else if (operand.IsRuntimeMember)
			{
				if (operand.IsMemoryAddress)
					return CreateMemoryAddressLabel(size, ((operand.RuntimeMember) as RuntimeField).FullName);
				else
					return CreateLabel(size, ((operand.RuntimeMember) as RuntimeField).FullName);
			}
			else if (operand.IsMemoryAddress)
			{
				if (operand.OffsetBase != null && operand.OffsetBase.IsConstant)
				{
					return CreateMemoryAddressOperand(size, (ulong)operand.OffsetBase.ValueAsLongInteger);
				}
				else
				{
					return CreateMemoryAddressOperand(size, ConvertToRegister(operand.EffectiveOffsetBase), null, 0, (int)operand.Displacement);
				}
			}
			else if (operand.IsSymbol)
			{
				if (operand.IsMemoryAddress)
					return CreateMemoryAddressLabel(size, operand.Name);
				else
					return CreateLabel(size, operand.Name);
			}
			return null;
		}

		private SimRegister ConvertToRegister(Mosa.Compiler.Framework.Register register)
		{
			if (register == Mosa.Platform.x86.GeneralPurposeRegister.EAX) return CPU.EAX;
			if (register == Mosa.Platform.x86.GeneralPurposeRegister.EBX) return CPU.EBX;
			if (register == Mosa.Platform.x86.GeneralPurposeRegister.ECX) return CPU.ECX;
			if (register == Mosa.Platform.x86.GeneralPurposeRegister.EDX) return CPU.EDX;
			if (register == Mosa.Platform.x86.GeneralPurposeRegister.ESI) return CPU.ESI;
			if (register == Mosa.Platform.x86.GeneralPurposeRegister.EDI) return CPU.EDI;
			if (register == Mosa.Platform.x86.GeneralPurposeRegister.EBP) return CPU.EBP;
			if (register == Mosa.Platform.x86.GeneralPurposeRegister.ESP) return CPU.ESP;

			if (register == Mosa.Platform.x86.SSE2Register.XMM0) return CPU.XMM0;
			if (register == Mosa.Platform.x86.SSE2Register.XMM1) return CPU.XMM1;
			if (register == Mosa.Platform.x86.SSE2Register.XMM2) return CPU.XMM2;
			if (register == Mosa.Platform.x86.SSE2Register.XMM3) return CPU.XMM3;
			if (register == Mosa.Platform.x86.SSE2Register.XMM4) return CPU.XMM4;
			if (register == Mosa.Platform.x86.SSE2Register.XMM5) return CPU.XMM5;
			if (register == Mosa.Platform.x86.SSE2Register.XMM6) return CPU.XMM6;
			if (register == Mosa.Platform.x86.SSE2Register.XMM7) return CPU.XMM7;

			if (register == Mosa.Platform.x86.ControlRegister.CR0) return CPU.CR0;
			if (register == Mosa.Platform.x86.ControlRegister.CR2) return CPU.CR2;
			if (register == Mosa.Platform.x86.ControlRegister.CR3) return CPU.CR3;
			if (register == Mosa.Platform.x86.ControlRegister.CR4) return CPU.CR4;

			if (register == Mosa.Platform.x86.SegmentRegister.CS) return CPU.CS;
			if (register == Mosa.Platform.x86.SegmentRegister.DS) return CPU.DS;
			if (register == Mosa.Platform.x86.SegmentRegister.ES) return CPU.ES;
			if (register == Mosa.Platform.x86.SegmentRegister.FS) return CPU.FS;
			if (register == Mosa.Platform.x86.SegmentRegister.GS) return CPU.GS;
			if (register == Mosa.Platform.x86.SegmentRegister.SS) return CPU.SS;

			return null;
		}

		private SimRegister AdjustRegisterSize(SimRegister register, int size)
		{
			if (size == 16)
			{
				if (register == CPU.EAX) return CPU.AX;
				if (register == CPU.EBX) return CPU.BX;
				if (register == CPU.ECX) return CPU.CX;
				if (register == CPU.EDX) return CPU.DX;
				if (register == CPU.EDI) return CPU.SI;
				if (register == CPU.EDI) return CPU.DI;
			}
			else if (size == 8)
			{
				if (register == CPU.EAX) return CPU.AL;
				if (register == CPU.EBX) return CPU.BL;
				if (register == CPU.ECX) return CPU.CL;
				if (register == CPU.EDX) return CPU.DL;
			}
			return register;
		}

		private static int GetSize(SigType sigType)
		{
			switch (sigType.Type)
			{
				case CilElementType.U1: return 8;
				case CilElementType.U2: return 16;
				case CilElementType.U4: return 32;
				case CilElementType.U8: return 64;
				case CilElementType.I1: return 8;
				case CilElementType.I2: return 16;
				case CilElementType.I4: return 32;
				case CilElementType.I8: return 64;
				case CilElementType.R4: return 32;
				case CilElementType.R8: return 64;
				case CilElementType.Boolean: return 8;
				case CilElementType.Char: return 16;
				case CilElementType.Ptr: return 32;
				case CilElementType.I: return 32;
				case CilElementType.U: return 32;
				default: return 32;
			}
		}

		private static bool IsSecondOperandDuplicate(BaseOpcode opcode)
		{
			if (opcode == Opcode.Adc) return true;
			if (opcode == Opcode.Add) return true;
			if (opcode == Opcode.Addsd) return true;
			if (opcode == Opcode.Addss) return true;
			if (opcode == Opcode.And) return true;
			//if (opcode == Opcode.Cld) return true;
			//if (opcode == Opcode.CmpXchg) return true;
			if (opcode == Opcode.Comisd) return true;
			if (opcode == Opcode.Comiss) return true;
			if (opcode == Opcode.Cvtsd2ss) return true;
			if (opcode == Opcode.Cvtsi2sd) return true;
			if (opcode == Opcode.Cvtsi2ss) return true;
			if (opcode == Opcode.Cvtss2sd) return true;
			if (opcode == Opcode.Cvttsd2si) return true;
			if (opcode == Opcode.Cvttss2si) return true;
			if (opcode == Opcode.Dec) return true;
			if (opcode == Opcode.Div) return true;
			if (opcode == Opcode.Divsd) return true;
			if (opcode == Opcode.Divss) return true;
			//if (opcode == Opcode.FarJmp) return true;
			//if (opcode == Opcode.Fld) return true;
			if (opcode == Opcode.Idiv) return true;
			if (opcode == Opcode.Imul) return false;
			if (opcode == Opcode.Inc) return true;
			if (opcode == Opcode.Lea) return true;
			if (opcode == Opcode.Mul) return true;
			if (opcode == Opcode.Mulsd) return true;
			if (opcode == Opcode.Mulss) return true;
			if (opcode == Opcode.Not) return true;
			if (opcode == Opcode.Or) return true;
			if (opcode == Opcode.Rcr) return true;
			//if (opcode == Opcode.Rdmsr) return true;
			//if (opcode == Opcode.Rdpmc) return true;
			//if (opcode == Opcode.Rdtsc) return true;
			//if (opcode == Opcode.Rep) return true;
			//if (opcode == Opcode.Roundsd) return true;
			//if (opcode == Opcode.Roundss) return true;
			if (opcode == Opcode.Sar) return true;
			if (opcode == Opcode.Sbb) return true;
			if (opcode == Opcode.Shl) return true;
			if (opcode == Opcode.Shld) return true;
			if (opcode == Opcode.Shr) return true;
			if (opcode == Opcode.Shrd) return true;
			//if (opcode == Opcode.Stos) return true;
			if (opcode == Opcode.Sub) return true;
			if (opcode == Opcode.Subsd) return true;
			if (opcode == Opcode.Subss) return true;
			//if (opcode == Opcode.Ucomisd) return true;
			//if (opcode == Opcode.Ucomiss) return true;
			//if (opcode == Opcode.Xchg) return true;
			if (opcode == Opcode.Xor) return true;
			if (opcode == Opcode.Ucomisd) return true;
			if (opcode == Opcode.Ucomiss) return true;
			if (opcode == Opcode.Neg) return true;

			return false;
		}

		private static BaseOpcode ConvertToOpcode(X86Instruction instruction, ConditionCode conditionCode)
		{
			if (instruction == X86.Adc) return Opcode.Adc;
			if (instruction == X86.Add) return Opcode.Add;
			if (instruction == X86.Addsd) return Opcode.Addsd;
			if (instruction == X86.Addss) return Opcode.Addss;
			if (instruction == X86.And) return Opcode.And;
			//if (instruction == X86.Break) return Opcode.Break;
			if (instruction == X86.Call) return Opcode.Call;
			if (instruction == X86.Cdq) return Opcode.Cdq;
			//if (instruction == X86.Cld) return Opcode.Cld;
			if (instruction == X86.Cli) return Opcode.Cli;
			//if (instruction == X86.Cmov) return Opcode.Cmov;
			if (instruction == X86.Cmp) return Opcode.Cmp;
			//if (instruction == X86.CmpXchg) return Opcode.CmpXchg;
			if (instruction == X86.Comisd) return Opcode.Comisd;
			if (instruction == X86.Comiss) return Opcode.Comiss;
			if (instruction == X86.CpuId) return Opcode.Cpuid;
			if (instruction == X86.Cvtsd2ss) return Opcode.Cvtsd2ss;
			if (instruction == X86.Cvtsi2sd) return Opcode.Cvtsi2sd;
			if (instruction == X86.Cvtsi2ss) return Opcode.Cvtsi2ss;
			if (instruction == X86.Cvtss2sd) return Opcode.Cvtss2sd;
			if (instruction == X86.Cvttsd2si) return Opcode.Cvttsd2si;
			if (instruction == X86.Cvttss2si) return Opcode.Cvttss2si;
			if (instruction == X86.Dec) return Opcode.Dec;
			if (instruction == X86.Div) return Opcode.Div;
			if (instruction == X86.Divsd) return Opcode.Divsd;
			if (instruction == X86.Divss) return Opcode.Divss;
			if (instruction == X86.FarJmp) return Opcode.FarJmp;
			if (instruction == X86.Fld) return Opcode.Fld;
			if (instruction == X86.Hlt) return Opcode.Hlt;
			if (instruction == X86.IDiv) return Opcode.Idiv;
			if (instruction == X86.IMul) return Opcode.Imul;
			if (instruction == X86.In) return Opcode.In;
			if (instruction == X86.Inc) return Opcode.Inc;
			//if (instruction == X86.Int) return Opcode.Int;
			//if (instruction == X86.Invlpg) return Opcode.Invlpg;
			if (instruction == X86.IRetd) return Opcode.Iretd;
			if (instruction == X86.Jmp) return Opcode.Jmp;
			if (instruction == X86.Lea) return Opcode.Lea;
			//if (instruction == X86.Leave) return Opcode.Leave;
			if (instruction == X86.Lgdt) return Opcode.Lgdt;
			if (instruction == X86.Lidt) return Opcode.Lidt;
			//if (instruction == X86.Lock) return Opcode.Lock;
			if (instruction == X86.Mov) return Opcode.Mov;
			if (instruction == X86.Movsd) return Opcode.Movsd;
			if (instruction == X86.Movss) return Opcode.Movss;
			if (instruction == X86.Movsx) return Opcode.Movsx;
			if (instruction == X86.Movzx) return Opcode.Movzx;
			if (instruction == X86.Mul) return Opcode.Mul;
			if (instruction == X86.Mulsd) return Opcode.Mulsd;
			if (instruction == X86.Mulss) return Opcode.Mulss;
			if (instruction == X86.Neg) return Opcode.Neg;
			if (instruction == X86.Nop) return Opcode.Nop;
			if (instruction == X86.Not) return Opcode.Not;
			if (instruction == X86.Or) return Opcode.Or;
			if (instruction == X86.Out) return Opcode.Out;
			//if (instruction == X86.Pause) return Opcode.Pause;
			if (instruction == X86.Pop) return Opcode.Pop;
			if (instruction == X86.Popad) return Opcode.Popad;
			if (instruction == X86.Popfd) return Opcode.Popfd;
			if (instruction == X86.Push) return Opcode.Push;
			if (instruction == X86.Pushad) return Opcode.Pushad;
			if (instruction == X86.Pushfd) return Opcode.Pushfd;
			if (instruction == X86.Rcr) return Opcode.Rcr;
			//if (instruction == X86.Rdmsr) return Opcode.Rdmsr;
			//if (instruction == X86.Rdpmc) return Opcode.Rdpmc;
			//if (instruction == X86.Rdtsc) return Opcode.Rdtsc;
			//if (instruction == X86.Rep) return Opcode.Rep;
			if (instruction == X86.Ret) return Opcode.Ret;
			if (instruction == X86.Roundsd) return Opcode.Roundsd;
			if (instruction == X86.Roundss) return Opcode.Roundss;
			if (instruction == X86.Sar) return Opcode.Sar;
			if (instruction == X86.Sbb) return Opcode.Sbb;
			if (instruction == X86.Shl) return Opcode.Shl;
			if (instruction == X86.Shld) return Opcode.Shld;
			if (instruction == X86.Shr) return Opcode.Shr;
			if (instruction == X86.Shrd) return Opcode.Shrd;
			if (instruction == X86.Sti) return Opcode.Sti;
			//if (instruction == X86.Stos) return Opcode.Stos;
			if (instruction == X86.Sub) return Opcode.Sub;
			if (instruction == X86.Subsd) return Opcode.Subsd;
			if (instruction == X86.Subss) return Opcode.Subss;
			//if (instruction == X86.Ucomisd) return Opcode.Ucomisd;
			//if (instruction == X86.Ucomiss) return Opcode.Ucomiss;
			if (instruction == X86.Xchg) return Opcode.Xchg;
			if (instruction == X86.Xor) return Opcode.Xor;
			if (instruction == X86.MovCR) return Opcode.Mov;
			if (instruction == X86.Ucomisd) return Opcode.Ucomisd;
			if (instruction == X86.Ucomiss) return Opcode.Ucomiss;

			if (instruction == X86.Setcc) return ConvertSetInstruction(conditionCode);
			if (instruction == X86.Branch) return ConvertBranchInstruction(conditionCode);

			if (instruction == X86.Break) return Opcode.InternalBreak;

			return null;
		}

		private static BaseOpcode ConvertBranchInstruction(ConditionCode conditionCode)
		{
			switch (conditionCode)
			{
				case ConditionCode.Equal: return Opcode.Je;
				case ConditionCode.NotEqual: return Opcode.Jne;
				case ConditionCode.Zero: return Opcode.Jz;
				case ConditionCode.NotZero: return Opcode.Jnz;
				case ConditionCode.GreaterOrEqual: return Opcode.Jge;
				case ConditionCode.GreaterThan: return Opcode.Jg;
				case ConditionCode.LessOrEqual: return Opcode.Jle;
				case ConditionCode.LessThan: return Opcode.Jl;
				case ConditionCode.UnsignedGreaterOrEqual: return Opcode.Jae;
				case ConditionCode.UnsignedGreaterThan: return Opcode.Ja;
				case ConditionCode.UnsignedLessOrEqual: return Opcode.Jbe;
				case ConditionCode.UnsignedLessThan: return Opcode.Jb;
				case ConditionCode.Signed: return Opcode.Js;
				case ConditionCode.NotSigned: return Opcode.Jns;
				case ConditionCode.Carry: return Opcode.Jc;
				case ConditionCode.NoCarry: return Opcode.Jnc;
				case ConditionCode.Overflow: return Opcode.Jo;
				case ConditionCode.NoOverflow: return Opcode.Jno;
				case ConditionCode.Parity: return Opcode.Jp;
				case ConditionCode.NoParity: return Opcode.Jnp;
				default: return null;
			}
		}

		private static BaseOpcode ConvertSetInstruction(ConditionCode conditionCode)
		{
			switch (conditionCode)
			{
				case ConditionCode.Equal: return Opcode.Sete;
				case ConditionCode.LessThan: return Opcode.Setl;
				case ConditionCode.LessOrEqual: return Opcode.Setle;
				case ConditionCode.GreaterOrEqual: return Opcode.Setge;
				case ConditionCode.GreaterThan: return Opcode.Setg;
				case ConditionCode.NotEqual: return Opcode.Setne;
				case ConditionCode.UnsignedGreaterOrEqual: return Opcode.Setnc;
				case ConditionCode.UnsignedGreaterThan: return Opcode.Seta;
				case ConditionCode.UnsignedLessOrEqual: return Opcode.Setbe;
				case ConditionCode.UnsignedLessThan: return Opcode.Setc;
				case ConditionCode.Parity: return Opcode.Setp;
				case ConditionCode.NoParity: return Opcode.Setnp;
				case ConditionCode.NoCarry: return Opcode.Setnc;
				case ConditionCode.Carry: return Opcode.Setc;
				case ConditionCode.Zero: return Opcode.Setz;
				case ConditionCode.NotZero: return Opcode.Setnz;
				default: return null;
			}
		}
	}
}