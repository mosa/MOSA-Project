/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Pascal Delprat (pdelprat) <pascal.delprat@online.fr>  
 */

using Mosa.Platform.AVR32.Instructions;

namespace Mosa.Platform.AVR32
{
	/// <summary>
	/// 
	/// </summary>
	public static class AVR32
	{
		/// <summary>
		/// 
		/// </summary>
		public static readonly Adc Adc = new Adc();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Add Add = new Add();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Adiw Adiw = new Adiw();

		/// <summary>
		/// 
		/// </summary>
		public static readonly And And = new And();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Andi Andi = new Andi();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Asr Asr = new Asr();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Bclr Bclr = new Bclr();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Bld Bld = new Bld();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Branch Branch = new Branch();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Bset Bset = new Bset();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Bst Bst = new Bst();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Call Call = new Call();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Cbi Cbi = new Cbi();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Cbr Cbr = new Cbr();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Clc Clc = new Clc();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Clh Clh = new Clh();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Cli Cli = new Cli();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Cln Cln = new Cln();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Clr Clr = new Clr();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Cls Cls = new Cls();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Clt Clt = new Clt();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Clv Clv = new Clv();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Clz Clz = new Clz();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Com Com = new Com();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Cp Cp = new Cp();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Cpc Cpc = new Cpc();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Cpi Cpi = new Cpi();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Cpse Cpse = new Cpse();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Dec Dec = new Dec();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Eor Eor = new Eor();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Icall Icall = new Icall();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Ijmp Ijmp = new Ijmp();

		/// <summary>
		/// 
		/// </summary>
		public static readonly In In = new In();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Inc Inc = new Inc();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Jmp Jmp = new Jmp();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Ld Ld = new Ld();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Ldd Ldd = new Ldd();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Ldi Ldi = new Ldi();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Lds Lds = new Lds();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Lpm Lpm = new Lpm();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Lsl Lsl = new Lsl();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Lsr Lsr = new Lsr();

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
		public static readonly Nop Nop = new Nop();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Or Or = new Or();

		/// <summary>
		/// 
		/// </summary>
		//public static readonly Orh Ori = new Orh();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Out Out = new Out();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Pop Pop = new Pop();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Push Push = new Push();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Rcall Rcall = new Rcall();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Ret Ret = new Ret();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Reti Reti = new Reti();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Rjmp Rjmp = new Rjmp();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Rol Rol = new Rol();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Ror Ror = new Ror();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Sbc Sbc = new Sbc();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Sbci Sbci = new Sbci();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Sbi Sbi = new Sbi();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Sbic Sbic = new Sbic();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Sbis Sbis = new Sbis();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Sbiw Sbiw = new Sbiw();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Sbr Sbr = new Sbr();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Sbrc Sbrc = new Sbrc();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Sbrs Sbrs = new Sbrs();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Sec Sec = new Sec();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Seh Seh = new Seh();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Sei Sei = new Sei();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Sen Sen = new Sen();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Ser Ser = new Ser();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Ses Ses = new Ses();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Set Set = new Set();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Sev Sev = new Sev();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Sez Sez = new Sez();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Sleep Sleep = new Sleep();

		/// <summary>
		/// 
		/// </summary>
		public static readonly St St = new St();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Std Std = new Std();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Sts Sts = new Sts();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Sub Sub = new Sub();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Subi Subi = new Subi();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Swap Swap = new Swap();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Tst Tst = new Tst();

		/// <summary>
		/// 
		/// </summary>
		public static readonly Wdr Wdr = new Wdr();

	}
}

