// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Platform.x64.Instructions;

namespace Mosa.Platform.x64
{
	/// <summary>
	/// X64 Instructions
	/// </summary>
	public static class X64
	{
		public static readonly Adc32 Adc32 = new Adc32();
		public static readonly AdcConst32 AdcConst32 = new AdcConst32();
		public static readonly Adc64 Adc64 = new Adc64();
		public static readonly AdcConst64 AdcConst64 = new AdcConst64();
		public static readonly Add32 Add32 = new Add32();
		public static readonly AddConst32 AddConst32 = new AddConst32();
		public static readonly Add64 Add64 = new Add64();
		public static readonly AddConst64 AddConst64 = new AddConst64();
		public static readonly Addsd Addsd = new Addsd();
		public static readonly Addss Addss = new Addss();
		public static readonly And32 And32 = new And32();
		public static readonly AndConst32 AndConst32 = new AndConst32();
		public static readonly And64 And64 = new And64();
		public static readonly AndConst64 AndConst64 = new AndConst64();
		public static readonly Break Break = new Break();
		public static readonly Btr32 Btr32 = new Btr32();
		public static readonly BtrConst32 BtrConst32 = new BtrConst32();
		public static readonly Btr64 Btr64 = new Btr64();
		public static readonly BtrConst64 BtrConst64 = new BtrConst64();
		public static readonly Bt32 Bt32 = new Bt32();
		public static readonly BtConst32 BtConst32 = new BtConst32();
		public static readonly Bt64 Bt64 = new Bt64();
		public static readonly BtConst64 BtConst64 = new BtConst64();
		public static readonly Bts32 Bts32 = new Bts32();
		public static readonly BtsConst32 BtsConst32 = new BtsConst32();
		public static readonly Bts64 Bts64 = new Bts64();
		public static readonly BtsConst64 BtsConst64 = new BtsConst64();
		public static readonly Call Call = new Call();
		public static readonly CallStatic CallStatic = new CallStatic();
		public static readonly CallReg CallReg = new CallReg();
		public static readonly Cdq Cdq = new Cdq();
		public static readonly Cli Cli = new Cli();
		public static readonly Cmp32 Cmp32 = new Cmp32();
		public static readonly CmpConst32 CmpConst32 = new CmpConst32();
		public static readonly Cmp64 Cmp64 = new Cmp64();
		public static readonly CmpConst64 CmpConst64 = new CmpConst64();
		public static readonly CmpXChgLoad32 CmpXChgLoad32 = new CmpXChgLoad32();
		public static readonly CmpXChgLoad64 CmpXChgLoad64 = new CmpXChgLoad64();
		public static readonly Comisd Comisd = new Comisd();
		public static readonly Comiss Comiss = new Comiss();
		public static readonly CpuId CpuId = new CpuId();
		public static readonly Cvtsd2ss Cvtsd2ss = new Cvtsd2ss();
		public static readonly Cvtsi2sd Cvtsi2sd = new Cvtsi2sd();
		public static readonly Cvtsi2ss Cvtsi2ss = new Cvtsi2ss();
		public static readonly Cvtss2sd Cvtss2sd = new Cvtss2sd();
		public static readonly Cvttsd2si Cvttsd2si = new Cvttsd2si();
		public static readonly Cvttss2si Cvttss2si = new Cvttss2si();
		public static readonly Dec32 Dec32 = new Dec32();
		public static readonly Dec64 Dec64 = new Dec64();
		public static readonly Div32 Div32 = new Div32();
		public static readonly Div64 Div64 = new Div64();
		public static readonly Divsd Divsd = new Divsd();
		public static readonly Divss Divss = new Divss();
		public static readonly JmpFar JmpFar = new JmpFar();
		public static readonly Hlt Hlt = new Hlt();
		public static readonly IDiv32 IDiv32 = new IDiv32();
		public static readonly IDiv64 IDiv64 = new IDiv64();
		public static readonly IMul32 IMul32 = new IMul32();
		public static readonly IMul64 IMul64 = new IMul64();
		public static readonly In8 In8 = new In8();
		public static readonly In16 In16 = new In16();
		public static readonly In32 In32 = new In32();
		public static readonly InConst8 InConst8 = new InConst8();
		public static readonly InConst16 InConst16 = new InConst16();
		public static readonly InConst32 InConst32 = new InConst32();
		public static readonly Inc32 Inc32 = new Inc32();
		public static readonly Inc64 Inc64 = new Inc64();
		public static readonly Int Int = new Int();
		public static readonly Invlpg Invlpg = new Invlpg();
		public static readonly IRetd IRetd = new IRetd();
		public static readonly Jmp Jmp = new Jmp();
		public static readonly JmpStatic JmpStatic = new JmpStatic();
		public static readonly JmpReg JmpReg = new JmpReg();
		public static readonly Lea32 Lea32 = new Lea32();
		public static readonly Lea64 Lea64 = new Lea64();
		public static readonly Leave Leave = new Leave();
		public static readonly Lgdt Lgdt = new Lgdt();
		public static readonly Lidt Lidt = new Lidt();
		public static readonly Lock Lock = new Lock();
		public static readonly MovLoadSeg32 MovLoadSeg32 = new MovLoadSeg32();
		public static readonly MovLoadSeg64 MovLoadSeg64 = new MovLoadSeg64();
		public static readonly MovStoreSeg32 MovStoreSeg32 = new MovStoreSeg32();
		public static readonly MovStoreSeg64 MovStoreSeg64 = new MovStoreSeg64();
		public static readonly Mov64 Mov64 = new Mov64();
		public static readonly MovConst64 MovConst64 = new MovConst64();
		public static readonly Movaps Movaps = new Movaps();
		public static readonly MovapsLoad MovapsLoad = new MovapsLoad();
		public static readonly MovCRLoad32 MovCRLoad32 = new MovCRLoad32();
		public static readonly MovCRLoad64 MovCRLoad64 = new MovCRLoad64();
		public static readonly MovCRStore32 MovCRStore32 = new MovCRStore32();
		public static readonly MovCRStore64 MovCRStore64 = new MovCRStore64();
		public static readonly Movd Movd = new Movd();
		public static readonly MovLoad8 MovLoad8 = new MovLoad8();
		public static readonly MovLoad16 MovLoad16 = new MovLoad16();
		public static readonly MovLoad32 MovLoad32 = new MovLoad32();
		public static readonly MovLoad64 MovLoad64 = new MovLoad64();
		public static readonly Movsd Movsd = new Movsd();
		public static readonly MovsdLoad MovsdLoad = new MovsdLoad();
		public static readonly MovsdStore MovsdStore = new MovsdStore();
		public static readonly Movss Movss = new Movss();
		public static readonly MovssLoad MovssLoad = new MovssLoad();
		public static readonly MovssStore MovssStore = new MovssStore();
		public static readonly MovStore8 MovStore8 = new MovStore8();
		public static readonly MovStore16 MovStore16 = new MovStore16();
		public static readonly MovStore32 MovStore32 = new MovStore32();
		public static readonly MovStore64 MovStore64 = new MovStore64();
		public static readonly Movsx8To64 Movsx8To64 = new Movsx8To64();
		public static readonly Movsx16To64 Movsx16To64 = new Movsx16To64();
		public static readonly MovsxLoad8 MovsxLoad8 = new MovsxLoad8();
		public static readonly MovsxLoad16 MovsxLoad16 = new MovsxLoad16();
		public static readonly MovsxLoad32 MovsxLoad32 = new MovsxLoad32();
		public static readonly Movups Movups = new Movups();
		public static readonly MovupsLoad MovupsLoad = new MovupsLoad();
		public static readonly MovupsStore MovupsStore = new MovupsStore();
		public static readonly Movzx8To64 Movzx8To64 = new Movzx8To64();
		public static readonly Movzx16To64 Movzx16To64 = new Movzx16To64();
		public static readonly MovzxLoad8 MovzxLoad8 = new MovzxLoad8();
		public static readonly MovzxLoad16 MovzxLoad16 = new MovzxLoad16();
		public static readonly MovzxLoad32 MovzxLoad32 = new MovzxLoad32();
		public static readonly Mul32 Mul32 = new Mul32();
		public static readonly Mul64 Mul64 = new Mul64();
		public static readonly Mulsd Mulsd = new Mulsd();
		public static readonly Mulss Mulss = new Mulss();
		public static readonly Neg32 Neg32 = new Neg32();
		public static readonly Neg64 Neg64 = new Neg64();
		public static readonly Nop Nop = new Nop();
		public static readonly Not32 Not32 = new Not32();
		public static readonly Not64 Not64 = new Not64();
		public static readonly Or32 Or32 = new Or32();
		public static readonly OrConst32 OrConst32 = new OrConst32();
		public static readonly Or64 Or64 = new Or64();
		public static readonly OrConst64 OrConst64 = new OrConst64();
		public static readonly Out8 Out8 = new Out8();
		public static readonly Out16 Out16 = new Out16();
		public static readonly Out32 Out32 = new Out32();
		public static readonly OutConst8 OutConst8 = new OutConst8();
		public static readonly OutConst16 OutConst16 = new OutConst16();
		public static readonly OutConst32 OutConst32 = new OutConst32();
		public static readonly Pause Pause = new Pause();
		public static readonly Pextrd Pextrd = new Pextrd();
		public static readonly Pop64 Pop64 = new Pop64();
		public static readonly Popad Popad = new Popad();
		public static readonly Push64 Push64 = new Push64();
		public static readonly PushConst64 PushConst64 = new PushConst64();
		public static readonly Pushad Pushad = new Pushad();
		public static readonly PXor PXor = new PXor();
		public static readonly Rcr32 Rcr32 = new Rcr32();
		public static readonly RcrConst32 RcrConst32 = new RcrConst32();
		public static readonly RcrConstOne32 RcrConstOne32 = new RcrConstOne32();
		public static readonly Rcr64 Rcr64 = new Rcr64();
		public static readonly RcrConst64 RcrConst64 = new RcrConst64();
		public static readonly RcrConstOne64 RcrConstOne64 = new RcrConstOne64();
		public static readonly Rep Rep = new Rep();
		public static readonly Ret Ret = new Ret();
		public static readonly Roundsd Roundsd = new Roundsd();
		public static readonly Roundss Roundss = new Roundss();
		public static readonly Sar32 Sar32 = new Sar32();
		public static readonly SarConst32 SarConst32 = new SarConst32();
		public static readonly SarConstOne32 SarConstOne32 = new SarConstOne32();
		public static readonly Sar64 Sar64 = new Sar64();
		public static readonly SarConst64 SarConst64 = new SarConst64();
		public static readonly SarConstOne64 SarConstOne64 = new SarConstOne64();
		public static readonly Sbb32 Sbb32 = new Sbb32();
		public static readonly SbbConst32 SbbConst32 = new SbbConst32();
		public static readonly Sbb64 Sbb64 = new Sbb64();
		public static readonly SbbConst64 SbbConst64 = new SbbConst64();
		public static readonly Shl32 Shl32 = new Shl32();
		public static readonly ShlConst32 ShlConst32 = new ShlConst32();
		public static readonly ShlConstOne32 ShlConstOne32 = new ShlConstOne32();
		public static readonly Shl64 Shl64 = new Shl64();
		public static readonly ShlConst64 ShlConst64 = new ShlConst64();
		public static readonly ShlConstOne64 ShlConstOne64 = new ShlConstOne64();
		public static readonly Shld32 Shld32 = new Shld32();
		public static readonly ShldConst32 ShldConst32 = new ShldConst32();
		public static readonly Shld64 Shld64 = new Shld64();
		public static readonly ShldConst64 ShldConst64 = new ShldConst64();
		public static readonly Shr32 Shr32 = new Shr32();
		public static readonly ShrConst32 ShrConst32 = new ShrConst32();
		public static readonly ShrConstOne32 ShrConstOne32 = new ShrConstOne32();
		public static readonly Shr64 Shr64 = new Shr64();
		public static readonly ShrConst64 ShrConst64 = new ShrConst64();
		public static readonly ShrConstOne64 ShrConstOne64 = new ShrConstOne64();
		public static readonly Shrd32 Shrd32 = new Shrd32();
		public static readonly ShrdConst32 ShrdConst32 = new ShrdConst32();
		public static readonly Shrd64 Shrd64 = new Shrd64();
		public static readonly ShrdConst64 ShrdConst64 = new ShrdConst64();
		public static readonly Sti Sti = new Sti();
		public static readonly Stos Stos = new Stos();
		public static readonly Sub32 Sub32 = new Sub32();
		public static readonly SubConst32 SubConst32 = new SubConst32();
		public static readonly Sub64 Sub64 = new Sub64();
		public static readonly SubConst64 SubConst64 = new SubConst64();
		public static readonly Subsd Subsd = new Subsd();
		public static readonly Subss Subss = new Subss();
		public static readonly Test32 Test32 = new Test32();
		public static readonly TestConst32 TestConst32 = new TestConst32();
		public static readonly Test64 Test64 = new Test64();
		public static readonly TestConst64 TestConst64 = new TestConst64();
		public static readonly Ucomisd Ucomisd = new Ucomisd();
		public static readonly Ucomiss Ucomiss = new Ucomiss();
		public static readonly XAddLoad32 XAddLoad32 = new XAddLoad32();
		public static readonly XAddLoad64 XAddLoad64 = new XAddLoad64();
		public static readonly XChg32 XChg32 = new XChg32();
		public static readonly XChg64 XChg64 = new XChg64();
		public static readonly XChgLoad32 XChgLoad32 = new XChgLoad32();
		public static readonly XChgLoad64 XChgLoad64 = new XChgLoad64();
		public static readonly Xor32 Xor32 = new Xor32();
		public static readonly XorConst32 XorConst32 = new XorConst32();
		public static readonly Xor64 Xor64 = new Xor64();
		public static readonly XorConst64 XorConst64 = new XorConst64();
		public static readonly BranchOverflow BranchOverflow = new BranchOverflow();
		public static readonly BranchNoOverflow BranchNoOverflow = new BranchNoOverflow();
		public static readonly BranchCarry BranchCarry = new BranchCarry();
		public static readonly BranchUnsignedLessThan BranchUnsignedLessThan = new BranchUnsignedLessThan();
		public static readonly BranchUnsignedGreaterOrEqual BranchUnsignedGreaterOrEqual = new BranchUnsignedGreaterOrEqual();
		public static readonly BranchNoCarry BranchNoCarry = new BranchNoCarry();
		public static readonly BranchEqual BranchEqual = new BranchEqual();
		public static readonly BranchZero BranchZero = new BranchZero();
		public static readonly BranchNotEqual BranchNotEqual = new BranchNotEqual();
		public static readonly BranchNotZero BranchNotZero = new BranchNotZero();
		public static readonly BranchUnsignedLessOrEqual BranchUnsignedLessOrEqual = new BranchUnsignedLessOrEqual();
		public static readonly BranchUnsignedGreaterThan BranchUnsignedGreaterThan = new BranchUnsignedGreaterThan();
		public static readonly BranchSigned BranchSigned = new BranchSigned();
		public static readonly BranchNotSigned BranchNotSigned = new BranchNotSigned();
		public static readonly BranchParity BranchParity = new BranchParity();
		public static readonly BranchNoParity BranchNoParity = new BranchNoParity();
		public static readonly BranchLessThan BranchLessThan = new BranchLessThan();
		public static readonly BranchGreaterOrEqual BranchGreaterOrEqual = new BranchGreaterOrEqual();
		public static readonly BranchLessOrEqual BranchLessOrEqual = new BranchLessOrEqual();
		public static readonly BranchGreaterThan BranchGreaterThan = new BranchGreaterThan();
		public static readonly SetByteIfOverflow SetByteIfOverflow = new SetByteIfOverflow();
		public static readonly SetByteIfNoOverflow SetByteIfNoOverflow = new SetByteIfNoOverflow();
		public static readonly SetByteIfCarry SetByteIfCarry = new SetByteIfCarry();
		public static readonly SetByteIfUnsignedLessThan SetByteIfUnsignedLessThan = new SetByteIfUnsignedLessThan();
		public static readonly SetByteIfUnsignedGreaterOrEqual SetByteIfUnsignedGreaterOrEqual = new SetByteIfUnsignedGreaterOrEqual();
		public static readonly SetByteIfNoCarry SetByteIfNoCarry = new SetByteIfNoCarry();
		public static readonly SetByteIfEqual SetByteIfEqual = new SetByteIfEqual();
		public static readonly SetByteIfZero SetByteIfZero = new SetByteIfZero();
		public static readonly SetByteIfNotEqual SetByteIfNotEqual = new SetByteIfNotEqual();
		public static readonly SetByteIfNotZero SetByteIfNotZero = new SetByteIfNotZero();
		public static readonly SetByteIfUnsignedLessOrEqual SetByteIfUnsignedLessOrEqual = new SetByteIfUnsignedLessOrEqual();
		public static readonly SetByteIfUnsignedGreaterThan SetByteIfUnsignedGreaterThan = new SetByteIfUnsignedGreaterThan();
		public static readonly SetByteIfSigned SetByteIfSigned = new SetByteIfSigned();
		public static readonly SetByteIfNotSigned SetByteIfNotSigned = new SetByteIfNotSigned();
		public static readonly SetByteIfParity SetByteIfParity = new SetByteIfParity();
		public static readonly SetByteIfNoParity SetByteIfNoParity = new SetByteIfNoParity();
		public static readonly SetByteIfLessThan SetByteIfLessThan = new SetByteIfLessThan();
		public static readonly SetByteIfGreaterOrEqual SetByteIfGreaterOrEqual = new SetByteIfGreaterOrEqual();
		public static readonly SetByteIfLessOrEqual SetByteIfLessOrEqual = new SetByteIfLessOrEqual();
		public static readonly SetByteIfGreaterThan SetByteIfGreaterThan = new SetByteIfGreaterThan();
		public static readonly CMovOverflow64 CMovOverflow64 = new CMovOverflow64();
		public static readonly CMovNoOverflow64 CMovNoOverflow64 = new CMovNoOverflow64();
		public static readonly CMovCarry64 CMovCarry64 = new CMovCarry64();
		public static readonly CMovUnsignedLessThan64 CMovUnsignedLessThan64 = new CMovUnsignedLessThan64();
		public static readonly CMovUnsignedGreaterOrEqual64 CMovUnsignedGreaterOrEqual64 = new CMovUnsignedGreaterOrEqual64();
		public static readonly CMovNoCarry64 CMovNoCarry64 = new CMovNoCarry64();
		public static readonly CMovEqual64 CMovEqual64 = new CMovEqual64();
		public static readonly CMovZero64 CMovZero64 = new CMovZero64();
		public static readonly CMovNotEqual64 CMovNotEqual64 = new CMovNotEqual64();
		public static readonly CMovNotZero64 CMovNotZero64 = new CMovNotZero64();
		public static readonly CMovUnsignedLessOrEqual64 CMovUnsignedLessOrEqual64 = new CMovUnsignedLessOrEqual64();
		public static readonly CMovUnsignedGreaterThan64 CMovUnsignedGreaterThan64 = new CMovUnsignedGreaterThan64();
		public static readonly CMovSigned64 CMovSigned64 = new CMovSigned64();
		public static readonly CMovNotSigned64 CMovNotSigned64 = new CMovNotSigned64();
		public static readonly CMovParity64 CMovParity64 = new CMovParity64();
		public static readonly CMovNoParity64 CMovNoParity64 = new CMovNoParity64();
		public static readonly CMovLessThan64 CMovLessThan64 = new CMovLessThan64();
		public static readonly CMovGreaterOrEqual64 CMovGreaterOrEqual64 = new CMovGreaterOrEqual64();
		public static readonly CMovLessOrEqual64 CMovLessOrEqual64 = new CMovLessOrEqual64();
		public static readonly CMovGreaterThan64 CMovGreaterThan64 = new CMovGreaterThan64();
	}
}
