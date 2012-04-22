/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Platform.x86.Instructions;

namespace Mosa.Platform.x86
{
	/// <summary>
	/// 
	/// </summary>
	public static class X86
	{
		/// <summary>
		/// 
		/// </summary>
		public static readonly Nop Nop = new Nop();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Add Add = new Add();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Adc Adc = new Adc();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Break Break = new Break();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Cdq Cdq = new Cdq();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Cmp Cmp = new Cmp();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Comisd Comisd = new Comisd();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Comiss Comiss = new Comiss();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Cvtsd2ss Cvtsd2ss = new Cvtsd2ss();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Cvtsi2sd Cvtsi2sd = new Cvtsi2sd();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Cvtsi2ss Cvtsi2ss = new Cvtsi2ss();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Cvtss2sd Cvtss2sd = new Cvtss2sd();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Cvttsd2si Cvttsd2si = new Cvttsd2si();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Cvttss2si Cvttss2si = new Cvttss2si();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Dec Dec = new Dec();
		/// <summary>
		/// 
		/// </summary>
		public static readonly IDiv IDiv = new IDiv();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Div Div = new Div();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Inc Inc = new Inc();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Int Int = new Int();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Branch Branch = new Branch();
		/// <summary>
		/// 
		/// </summary>
		public static readonly And And = new And();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Not Not = new Not();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Or Or = new Or();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Xor Xor = new Xor();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Mov Mov = new Mov();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Mul Mul = new Mul();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Neg Neg = new Neg();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Sal Sal = new Sal();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Sar Sar = new Sar();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Sbb Sbb = new Sbb();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Setcc Setcc = new Setcc();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Shld Shld = new Shld();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Shl Shl = new Shl();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Shrd Shrd = new Shrd();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Shr Shr = new Shr();
		/// <summary>
		/// 
		/// </summary>
		public static readonly AddSS AddSS = new AddSS();
		/// <summary>
		/// 
		/// </summary>
		public static readonly AddSD AddSD = new AddSD();
		/// <summary>
		/// 
		/// </summary>
		public static readonly DivSS DivSS = new DivSS();
		/// <summary>
		/// 
		/// </summary>
		public static readonly DivSD DivSD = new DivSD();
		/// <summary>
		/// 
		/// </summary>
		public static readonly MulSS MulSS = new MulSS();
		/// <summary>
		/// 
		/// </summary>
		public static readonly MulSD MulSD = new MulSD();
		/// <summary>
		/// 
		/// </summary>
		public static readonly SubSS SubSS = new SubSS();
		/// <summary>
		/// 
		/// </summary>
		public static readonly SubSD SubSD = new SubSD();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Sub Sub = new Sub();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Ucomisd Ucomisd = new Ucomisd();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Ucomiss Ucomiss = new Ucomiss();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Jmp Jmp = new Jmp();
		/// <summary>
		/// 
		/// </summary>
		public static readonly FarJmp FarJmp = new FarJmp();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Movsx Movsx = new Movsx();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Movzx Movzx = new Movzx();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Movss Movss = new Movss();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Movsd Movsd = new Movsd();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Call Call = new Call();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Ret Ret = new Ret();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Leave Leave = new Leave();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Cld Cld = new Cld();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Cli Cli = new Cli();
		/// <summary>
		/// 
		/// </summary>
		public static readonly CmpXchg CmpXchg = new CmpXchg();
		/// <summary>
		/// 
		/// </summary>
		public static readonly CpuId CpuId = new CpuId();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Hlt Hlt = new Hlt();
		/// <summary>
		/// 
		/// </summary>
		public static readonly In In = new In();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Invlpg Invlpg = new Invlpg();
		/// <summary>
		/// 
		/// </summary>
		public static readonly IRetd IRetd = new IRetd();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Lgdt Lgdt = new Lgdt();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Lidt Lidt = new Lidt();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Out Out = new Out();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Pause Pause = new Pause();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Popad Popad = new Popad();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Popfd Popfd = new Popfd();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Pop Pop = new Pop();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Pushad Pushad = new Pushad();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Pushfd Pushfd = new Pushfd();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Push Push = new Push();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Rcr Rcr = new Rcr();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Rdmsr Rdmsr = new Rdmsr();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Rdpmc Rdpmc = new Rdpmc();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Rdtsc Rdtsc = new Rdtsc();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Rep Rep = new Rep();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Sti Sti = new Sti();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Stos Stos = new Stos();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Xchg Xchg = new Xchg();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Lea Lea = new Lea();
		/// <summary>
		/// 
		/// </summary>
		public static readonly RoundSS SseRound = new RoundSS();
		/// <summary>
		/// 
		/// </summary>
		public static readonly Lock Lock = new Lock();

	}
}

