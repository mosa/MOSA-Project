/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.TinyCPUSimulator.x86.Opcodes;

namespace Mosa.TinyCPUSimulator.x86
{
	public static class Opcode
	{
		public static BaseX86Opcode Add = new Add();
		public static BaseX86Opcode Adc = new Adc();
		public static BaseX86Opcode And = new And();
		public static BaseX86Opcode Mov = new Mov();
		public static BaseX86Opcode Jo = new Jo();
		public static BaseX86Opcode Jno = new Jno();
		public static BaseX86Opcode Jc = new Jc();
		public static BaseX86Opcode Jb = new Jb();
		public static BaseX86Opcode Jae = new Jae();
		public static BaseX86Opcode Jnc = new Jnc();
		public static BaseX86Opcode Je = new Je();
		public static BaseX86Opcode Jz = new Jz();
		public static BaseX86Opcode Jne = new Jne();
		public static BaseX86Opcode Jnz = new Jnz();
		public static BaseX86Opcode Jbe = new Jbe();
		public static BaseX86Opcode Ja = new Ja();
		public static BaseX86Opcode Js = new Js();
		public static BaseX86Opcode Jns = new Jns();
		public static BaseX86Opcode Jp = new Jp();
		public static BaseX86Opcode Jnp = new Jnp();
		public static BaseX86Opcode Jl = new Jl();
		public static BaseX86Opcode Jge = new Jge();
		public static BaseX86Opcode Jle = new Jle();
		public static BaseX86Opcode Jg = new Jg();
		public static BaseX86Opcode Dec = new Dec();
		public static BaseX86Opcode Inc = new Inc();
		public static BaseX86Opcode Jmp = new Jmp();
		public static BaseX86Opcode Sub = new Sub();
		public static BaseX86Opcode Pop = new Pop();
		public static BaseX86Opcode Push = new Push();
		public static BaseX86Opcode Call = new Call();
		public static BaseX86Opcode Cmp = new Cmp();
		public static BaseX86Opcode Movzx = new Movzx();
		public static BaseX86Opcode Xor = new Xor();
		public static BaseX86Opcode Ret = new Ret();
		public static BaseX86Opcode Movsx = new Movsx();
		public static BaseX86Opcode Cli = new Cli();
		public static BaseX86Opcode Sti = new Sti();
		public static BaseX86Opcode Mul = new Mul();
		public static BaseX86Opcode Popad = new Pushad();
		public static BaseX86Opcode Pushad = new Pushad();
		public static BaseX86Opcode Iretd = new Iretd();
		public static BaseX86Opcode Or = new Or();
		public static BaseX86Opcode Seta = new Seta();
		public static BaseX86Opcode Setc = new Setc();
		public static BaseX86Opcode Setg = new Setg();
		public static BaseX86Opcode Setl = new Setl();
		public static BaseX86Opcode Setnc = new Setnc();
		public static BaseX86Opcode Setnz = new Setnz();
		public static BaseX86Opcode Setpo = new Setpo();
		public static BaseX86Opcode Setz = new Setz();
		public static BaseX86Opcode Stc = new Stc();
		public static BaseX86Opcode Cmc = new Cmc();
		public static BaseX86Opcode Nop = new Nop();
		public static BaseX86Opcode Neg = new Neg();
		public static BaseX86Opcode Not = new Not();
		public static BaseX86Opcode Hlt = new Hlt();
		public static BaseX86Opcode Xchg = new Xchg();
		public static BaseX86Opcode Lidt = new Lidt();
		public static BaseX86Opcode Lgdt = new Lgdt();
		public static BaseX86Opcode In = new In();
		public static BaseX86Opcode Out = new Out();
		public static BaseX86Opcode Cdq = new Cdq();
		public static BaseX86Opcode Lea = new Lea();
		public static BaseX86Opcode Sar = new Sar();
		public static BaseX86Opcode Shr = new Shr();
		public static BaseX86Opcode Shl = new Shl();
		public static BaseX86Opcode Sal = new Sal();
		public static BaseX86Opcode Shrd = new Shrd();
		public static BaseX86Opcode Shld = new Shld();
		public static BaseX86Opcode Div = new Div();
		public static BaseX86Opcode Sbb = new Sbb();
		public static BaseX86Opcode Rcl = new Rcl();
		public static BaseX86Opcode Rcr = new Rcr();
		public static BaseX86Opcode Idiv = new Idiv();
		public static BaseX86Opcode Imul = new Imul();
		public static BaseX86Opcode Cpuid = new Cpuid();
		public static BaseX86Opcode Addsd = new Addsd();
		public static BaseX86Opcode Addss = new Addss();
		public static BaseX86Opcode Subsd = new Subsd();
		public static BaseX86Opcode Subss = new Subss();
		public static BaseX86Opcode Movsd = new Movsd();
		public static BaseX86Opcode Movss = new Movss();
		public static BaseX86Opcode Cvtsd2ss = new Cvtsd2ss();
		public static BaseX86Opcode Cvtsi2sd = new Cvtsi2sd();
		public static BaseX86Opcode Cvtsi2ss = new Cvtsi2ss();
		public static BaseX86Opcode Cvtss2sd = new Cvtss2sd();
		public static BaseX86Opcode Cvttsd2si = new Cvttsd2si();
		public static BaseX86Opcode Cvttss2si = new Cvttss2si();
		public static BaseX86Opcode Mulss = new Mulss();
		public static BaseX86Opcode Mulsd = new Mulsd();
		public static BaseX86Opcode Divss = new Divss();
		public static BaseX86Opcode Divsd = new Divsd();
		public static BaseX86Opcode Popfd = new Popfd();
		public static BaseX86Opcode Pushfd = new Pushfd();
		public static BaseX86Opcode Comisd = new Comisd();
		public static BaseX86Opcode Comiss = new Comiss();
		public static BaseX86Opcode Setp = new Setp();
		public static BaseX86Opcode Setnp = new Setnp();
		public static BaseX86Opcode Setpe = new Setpe();
		public static BaseX86Opcode Sete = new Sete();
		public static BaseX86Opcode Setle = new Setle();
		public static BaseX86Opcode Setge = new Setge();
		public static BaseX86Opcode Setne = new Setne();
		public static BaseX86Opcode Setbe = new Setbe();
		public static BaseX86Opcode Ucomiss = new Ucomiss();
		public static BaseX86Opcode Ucomisd = new Ucomisd();
		public static BaseX86Opcode Fld = new Fld();
		public static BaseX86Opcode Roundss = new Roundss();
		public static BaseX86Opcode Roundsd = new Roundsd();
		public static BaseX86Opcode FarJmp = new FarJmp();
		public static BaseX86Opcode Test = new Test();

		public static BaseX86Opcode InternalBreak = new InternalBreak();
	}
}