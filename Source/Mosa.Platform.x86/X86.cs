// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Platform.x86.Instructions;

namespace Mosa.Platform.x86
{
	public static class X86
	{
		public static readonly Nop Nop = new Nop();

		public static readonly Add Add = new Add();

		public static readonly Adc Adc = new Adc();

		public static readonly Break Break = new Break();

		public static readonly Cdq Cdq = new Cdq();

		public static readonly Cmp Cmp = new Cmp();

		public static readonly Cmovcc Cmovcc = new Cmovcc();

		public static readonly Comisd Comisd = new Comisd();

		public static readonly Comiss Comiss = new Comiss();

		public static readonly Cvtsd2ss Cvtsd2ss = new Cvtsd2ss();

		public static readonly Cvtsi2sd Cvtsi2sd = new Cvtsi2sd();

		public static readonly Cvtsi2ss Cvtsi2ss = new Cvtsi2ss();

		public static readonly Cvtss2sd Cvtss2sd = new Cvtss2sd();

		public static readonly Cvttsd2si Cvttsd2si = new Cvttsd2si();

		public static readonly Cvttss2si Cvttss2si = new Cvttss2si();

		public static readonly Dec Dec = new Dec();

		public static readonly IDiv IDiv = new IDiv();

		public static readonly Div Div = new Div();

		public static readonly Inc Inc = new Inc();

		public static readonly Int Int = new Int();

		public static readonly Branch Branch = new Branch();

		public static readonly And And = new And();

		public static readonly Not Not = new Not();

		public static readonly Or Or = new Or();

		public static readonly Xor Xor = new Xor();

		public static readonly PXor PXor = new PXor();

		public static readonly Mov Mov = new Mov();

		public static readonly MovStore MovStore = new MovStore();

		public static readonly MovLoad MovLoad = new MovLoad();

		public static readonly MovzxLoad MovzxLoad = new MovzxLoad();

		public static readonly MovsxLoad MovsxLoad = new MovsxLoad();

		public static readonly MovCR MovCR = new MovCR();

		public static readonly Mul Mul = new Mul();

		public static readonly IMul IMul = new IMul();

		public static readonly Neg Neg = new Neg();

		public static readonly Sar Sar = new Sar();

		public static readonly Sbb Sbb = new Sbb();

		public static readonly Setcc Setcc = new Setcc();

		public static readonly Shld Shld = new Shld();

		public static readonly Shl Shl = new Shl();

		public static readonly Shrd Shrd = new Shrd();

		public static readonly Shr Shr = new Shr();

		public static readonly Addss Addss = new Addss();

		public static readonly Addsd Addsd = new Addsd();

		public static readonly Divss Divss = new Divss();

		public static readonly Divsd Divsd = new Divsd();

		public static readonly Mulss Mulss = new Mulss();

		public static readonly Mulsd Mulsd = new Mulsd();

		public static readonly Subss Subss = new Subss();

		public static readonly Subsd Subsd = new Subsd();

		public static readonly Sub Sub = new Sub();

		public static readonly Ucomisd Ucomisd = new Ucomisd();

		public static readonly Ucomiss Ucomiss = new Ucomiss();

		public static readonly Jmp Jmp = new Jmp();

		public static readonly FarJmp FarJmp = new FarJmp();

		public static readonly Movsx Movsx = new Movsx();

		public static readonly Movzx Movzx = new Movzx();

		public static readonly Movss Movss = new Movss();

		public static readonly MovssLoad MovssLoad = new MovssLoad();

		public static readonly MovssStore MovssStore = new MovssStore();

		public static readonly Movsd Movsd = new Movsd();

		public static readonly MovsdLoad MovsdLoad = new MovsdLoad();

		public static readonly MovsdStore MovsdStore = new MovsdStore();

		public static readonly Movups Movups = new Movups();

		public static readonly MovupsLoad MovupsLoad = new MovupsLoad();

		public static readonly MovupsStore MovupsStore = new MovupsStore();

		public static readonly Movaps Movaps = new Movaps();

		public static readonly MovapsLoad MovapsLoad = new MovapsLoad();

		public static readonly Call Call = new Call();

		public static readonly Ret Ret = new Ret();

		public static readonly Leave Leave = new Leave();

		public static readonly Cld Cld = new Cld();

		public static readonly Cli Cli = new Cli();

		public static readonly CmpXchg CmpXchg = new CmpXchg();

		public static readonly CpuId CpuId = new CpuId();

		public static readonly Hlt Hlt = new Hlt();

		public static readonly In In = new In();

		public static readonly Invlpg Invlpg = new Invlpg();

		public static readonly IRetd IRetd = new IRetd();

		public static readonly Lgdt Lgdt = new Lgdt();

		public static readonly Lidt Lidt = new Lidt();

		public static readonly Out Out = new Out();

		public static readonly Pause Pause = new Pause();

		public static readonly Popad Popad = new Popad();

		public static readonly Popfd Popfd = new Popfd();

		public static readonly Pop Pop = new Pop();

		public static readonly Pushad Pushad = new Pushad();

		public static readonly Pushfd Pushfd = new Pushfd();

		public static readonly Push Push = new Push();

		public static readonly Rcr Rcr = new Rcr();

		public static readonly Rdmsr Rdmsr = new Rdmsr();

		public static readonly Rdpmc Rdpmc = new Rdpmc();

		public static readonly Rdtsc Rdtsc = new Rdtsc();

		public static readonly Rep Rep = new Rep();

		public static readonly Sti Sti = new Sti();

		public static readonly Stos Stos = new Stos();

		public static readonly Xchg Xchg = new Xchg();

		public static readonly Lea Lea = new Lea();

		public static readonly Roundss Roundss = new Roundss();

		public static readonly Roundsd Roundsd = new Roundsd();

		public static readonly Lock Lock = new Lock();

		public static readonly Fld Fld = new Fld();

		public static readonly Test Test = new Test();

		public static readonly Bts Bts = new Bts();

		public static readonly Btr Btr = new Btr();

		public static readonly Pextrd Pextrd = new Pextrd();

		public static readonly Movd Movd = new Movd();
	}
}
