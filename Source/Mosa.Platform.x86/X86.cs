// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;
using Mosa.Compiler.x86.Instructions;

namespace Mosa.Compiler.x86;

/// <summary>
/// X86 Instructions
/// </summary>
public static class X86
{
	public static readonly BaseInstruction Adc32 = new Adc32();
	public static readonly BaseInstruction Add32 = new Add32();
	public static readonly BaseInstruction Addsd = new Addsd();
	public static readonly BaseInstruction Addss = new Addss();
	public static readonly BaseInstruction And32 = new And32();
	public static readonly BaseInstruction AndN32 = new AndN32();
	public static readonly BaseInstruction Break = new Break();
	public static readonly BaseInstruction Btr32 = new Btr32();
	public static readonly BaseInstruction Bt32 = new Bt32();
	public static readonly BaseInstruction Bts32 = new Bts32();
	public static readonly BaseInstruction Call = new Call();
	public static readonly BaseInstruction Cdq32 = new Cdq32();
	public static readonly BaseInstruction Cli = new Cli();
	public static readonly BaseInstruction Cmp32 = new Cmp32();
	public static readonly BaseInstruction CmpXChgLoad32 = new CmpXChgLoad32();
	public static readonly BaseInstruction Comisd = new Comisd();
	public static readonly BaseInstruction Comiss = new Comiss();
	public static readonly BaseInstruction CpuId = new CpuId();
	public static readonly BaseInstruction Cvtsd2ss = new Cvtsd2ss();
	public static readonly BaseInstruction Cvtsi2sd32 = new Cvtsi2sd32();
	public static readonly BaseInstruction Cvtsi2ss32 = new Cvtsi2ss32();
	public static readonly BaseInstruction Cvtss2sd = new Cvtss2sd();
	public static readonly BaseInstruction Cvttsd2si32 = new Cvttsd2si32();
	public static readonly BaseInstruction Cvttss2si32 = new Cvttss2si32();
	public static readonly BaseInstruction Dec32 = new Dec32();
	public static readonly BaseInstruction Div32 = new Div32();
	public static readonly BaseInstruction Divsd = new Divsd();
	public static readonly BaseInstruction Divss = new Divss();
	public static readonly BaseInstruction JmpFar = new JmpFar();
	public static readonly BaseInstruction Hlt = new Hlt();
	public static readonly BaseInstruction IDiv32 = new IDiv32();
	public static readonly BaseInstruction IMul32 = new IMul32();
	public static readonly BaseInstruction IMul1o32 = new IMul1o32();
	public static readonly BaseInstruction IMul3o32 = new IMul3o32();
	public static readonly BaseInstruction In8 = new In8();
	public static readonly BaseInstruction In16 = new In16();
	public static readonly BaseInstruction In32 = new In32();
	public static readonly BaseInstruction Inc32 = new Inc32();
	public static readonly BaseInstruction Int = new Int();
	public static readonly BaseInstruction Invlpg = new Invlpg();
	public static readonly BaseInstruction IRetd = new IRetd();
	public static readonly BaseInstruction Jmp = new Jmp();
	public static readonly BaseInstruction JmpExternal = new JmpExternal();
	public static readonly BaseInstruction Lea32 = new Lea32();
	public static readonly BaseInstruction Leave = new Leave();
	public static readonly BaseInstruction Lgdt = new Lgdt();
	public static readonly BaseInstruction Lidt = new Lidt();
	public static readonly BaseInstruction Lock = new Lock();
	public static readonly BaseInstruction MovLoadSeg32 = new MovLoadSeg32();
	public static readonly BaseInstruction MovStoreSeg32 = new MovStoreSeg32();
	public static readonly BaseInstruction Mov32 = new Mov32();
	public static readonly BaseInstruction Movaps = new Movaps();
	public static readonly BaseInstruction MovapsLoad = new MovapsLoad();
	public static readonly BaseInstruction MovCRLoad32 = new MovCRLoad32();
	public static readonly BaseInstruction MovCRStore32 = new MovCRStore32();
	public static readonly BaseInstruction Movdssi32 = new Movdssi32();
	public static readonly BaseInstruction Movdi32ss = new Movdi32ss();
	public static readonly BaseInstruction MovLoad8 = new MovLoad8();
	public static readonly BaseInstruction MovLoad16 = new MovLoad16();
	public static readonly BaseInstruction MovLoad32 = new MovLoad32();
	public static readonly BaseInstruction Movsd = new Movsd();
	public static readonly BaseInstruction MovsdLoad = new MovsdLoad();
	public static readonly BaseInstruction MovsdStore = new MovsdStore();
	public static readonly BaseInstruction Movss = new Movss();
	public static readonly BaseInstruction MovssLoad = new MovssLoad();
	public static readonly BaseInstruction MovssStore = new MovssStore();
	public static readonly BaseInstruction MovStore8 = new MovStore8();
	public static readonly BaseInstruction MovStore16 = new MovStore16();
	public static readonly BaseInstruction MovStore32 = new MovStore32();
	public static readonly BaseInstruction Movsx8To32 = new Movsx8To32();
	public static readonly BaseInstruction Movsx16To32 = new Movsx16To32();
	public static readonly BaseInstruction MovsxLoad8 = new MovsxLoad8();
	public static readonly BaseInstruction MovsxLoad16 = new MovsxLoad16();
	public static readonly BaseInstruction Movups = new Movups();
	public static readonly BaseInstruction MovupsLoad = new MovupsLoad();
	public static readonly BaseInstruction MovupsStore = new MovupsStore();
	public static readonly BaseInstruction Movzx8To32 = new Movzx8To32();
	public static readonly BaseInstruction Movzx16To32 = new Movzx16To32();
	public static readonly BaseInstruction MovzxLoad8 = new MovzxLoad8();
	public static readonly BaseInstruction MovzxLoad16 = new MovzxLoad16();
	public static readonly BaseInstruction Mul32 = new Mul32();
	public static readonly BaseInstruction Mulsd = new Mulsd();
	public static readonly BaseInstruction Mulss = new Mulss();
	public static readonly BaseInstruction Neg32 = new Neg32();
	public static readonly BaseInstruction Nop = new Nop();
	public static readonly BaseInstruction Not32 = new Not32();
	public static readonly BaseInstruction Or32 = new Or32();
	public static readonly BaseInstruction Out8 = new Out8();
	public static readonly BaseInstruction Out16 = new Out16();
	public static readonly BaseInstruction Out32 = new Out32();
	public static readonly BaseInstruction Pause = new Pause();
	public static readonly BaseInstruction Pextrd32 = new Pextrd32();
	public static readonly BaseInstruction Pop32 = new Pop32();
	public static readonly BaseInstruction Popad = new Popad();
	public static readonly BaseInstruction Push32 = new Push32();
	public static readonly BaseInstruction Pushad = new Pushad();
	public static readonly BaseInstruction PXor = new PXor();
	public static readonly BaseInstruction Rcr32 = new Rcr32();
	public static readonly BaseInstruction Ret = new Ret();
	public static readonly BaseInstruction Roundsd = new Roundsd();
	public static readonly BaseInstruction Roundss = new Roundss();
	public static readonly BaseInstruction Sar32 = new Sar32();
	public static readonly BaseInstruction Sbb32 = new Sbb32();
	public static readonly BaseInstruction Shl32 = new Shl32();
	public static readonly BaseInstruction Shld32 = new Shld32();
	public static readonly BaseInstruction Shr32 = new Shr32();
	public static readonly BaseInstruction Shrd32 = new Shrd32();
	public static readonly BaseInstruction Sti = new Sti();
	public static readonly BaseInstruction Stos = new Stos();
	public static readonly BaseInstruction Sqrtss = new Sqrtss();
	public static readonly BaseInstruction Sqrtsd = new Sqrtsd();
	public static readonly BaseInstruction Sub32 = new Sub32();
	public static readonly BaseInstruction Subsd = new Subsd();
	public static readonly BaseInstruction Subss = new Subss();
	public static readonly BaseInstruction Test32 = new Test32();
	public static readonly BaseInstruction Ucomisd = new Ucomisd();
	public static readonly BaseInstruction Ucomiss = new Ucomiss();
	public static readonly BaseInstruction XAddLoad32 = new XAddLoad32();
	public static readonly BaseInstruction XChg32 = new XChg32();
	public static readonly BaseInstruction XChgLoad32 = new XChgLoad32();
	public static readonly BaseInstruction Xor32 = new Xor32();
	public static readonly BaseInstruction Branch = new Branch();
	public static readonly BaseInstruction Setcc = new Setcc();
	public static readonly BaseInstruction CMov32 = new CMov32();
	public static readonly BaseInstruction BochsDebug = new BochsDebug();
	public static readonly BaseInstruction RdMSR = new RdMSR();
	public static readonly BaseInstruction WrMSR = new WrMSR();
	public static readonly BaseInstruction Blsr32 = new Blsr32();
	public static readonly BaseInstruction Popcnt32 = new Popcnt32();
	public static readonly BaseInstruction Lzcnt32 = new Lzcnt32();
	public static readonly BaseInstruction Tzcnt32 = new Tzcnt32();
	public static readonly BaseInstruction Xorps = new Xorps();
}
