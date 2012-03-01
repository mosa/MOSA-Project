/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework;

namespace Mosa.Platform.AVR32.OpCodes
{
	/// <summary>
	/// 
	/// </summary>
	public interface IAVR32Visitor : IVisitor
	{
		/// <summary>
		/// Adc instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Adc(Context context);

		/// <summary>
		/// Add instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Add(Context context);

		/// <summary>
		/// Adiw instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Adiw(Context context);

		/// <summary>
		/// And instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void And(Context context);

		/// <summary>
		/// Andi instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Andi(Context context);

		/// <summary>
		/// Asr instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Asr(Context context);

		/// <summary>
		/// Bclr instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Bclr(Context context);

		/// <summary>
		/// Bld instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Bld(Context context);

		/// <summary>
		/// Brbc instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Brbc(Context context);

		/// <summary>
		/// Brbs instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Brbs(Context context);

		/// <summary>
		/// Brcc instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Brcc(Context context);

		/// <summary>
		/// Brcs instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Brcs(Context context);

		/// <summary>
		/// Breq instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Breq(Context context);

		/// <summary>
		/// Brge instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Brge(Context context);

		/// <summary>
		/// Brhc instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Brhc(Context context);

		/// <summary>
		/// Brhs instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Brhs(Context context);

		/// <summary>
		/// Brid instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Brid(Context context);

		/// <summary>
		/// Brie instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Brie(Context context);

		/// <summary>
		/// Brlo instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Brlo(Context context);

		/// <summary>
		/// Brlt instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Brlt(Context context);

		/// <summary>
		/// Brmi instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Brmi(Context context);

		/// <summary>
		/// Brne instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Brne(Context context);

		/// <summary>
		/// Brpl instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Brpl(Context context);

		/// <summary>
		/// Brsh instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Brsh(Context context);

		/// <summary>
		/// Brtc instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Brtc(Context context);

		/// <summary>
		/// Brts instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Brts(Context context);

		/// <summary>
		/// Brvc instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Brvc(Context context);

		/// <summary>
		/// Brvs instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Brvs(Context context);

		/// <summary>
		/// Bset instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Bset(Context context);

		/// <summary>
		/// Bst instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Bst(Context context);

		/// <summary>
		/// Call instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Call(Context context);

		/// <summary>
		/// Cbi instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Cbi(Context context);

		/// <summary>
		/// Cbr instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Cbr(Context context);

		/// <summary>
		/// Clc instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Clc(Context context);

		/// <summary>
		/// Clh instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Clh(Context context);

		/// <summary>
		/// Cli instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Cli(Context context);

		/// <summary>
		/// Cln instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Cln(Context context);

		/// <summary>
		/// Clr instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Clr(Context context);

		/// <summary>
		/// Cls instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Cls(Context context);

		/// <summary>
		/// Clt instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Clt(Context context);

		/// <summary>
		/// Clv instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Clv(Context context);

		/// <summary>
		/// Clz instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Clz(Context context);

		/// <summary>
		/// Com instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Com(Context context);

		/// <summary>
		/// Cp instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Cp(Context context);

		/// <summary>
		/// Cpc instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Cpc(Context context);

		/// <summary>
		/// Cpi instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Cpi(Context context);

		/// <summary>
		/// Cpse instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Cpse(Context context);

		/// <summary>
		/// Dec instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Dec(Context context);

		/// <summary>
		/// Eor instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Eor(Context context);

		/// <summary>
		/// Icall instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Icall(Context context);

		/// <summary>
		/// Ijmp instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Ijmp(Context context);

		/// <summary>
		/// In instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void In(Context context);

		/// <summary>
		/// Inc instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Inc(Context context);

		/// <summary>
		/// Jmp instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Jmp(Context context);

		/// <summary>
		/// Ld instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Ld(Context context);

		/// <summary>
		/// Ldd instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Ldd(Context context);

		/// <summary>
		/// Ldi instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Ldi(Context context);

		/// <summary>
		/// Lds instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Lds(Context context);

		/// <summary>
		/// Lpm instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Lpm(Context context);

		/// <summary>
		/// Lsl instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Lsl(Context context);

		/// <summary>
		/// Lsr instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Lsr(Context context);

		/// <summary>
		/// Mov instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Mov(Context context);

		/// <summary>
		/// Mul instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Mul(Context context);

		/// <summary>
		/// Neg instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Neg(Context context);

		/// <summary>
		/// Nop instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Nop(Context context);

		/// <summary>
		/// Or instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Or(Context context);

		/// <summary>
		/// Ori instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Ori(Context context);

		/// <summary>
		/// Out instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Out(Context context);

		/// <summary>
		/// Pop instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Pop(Context context);

		/// <summary>
		/// Push instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Push(Context context);

		/// <summary>
		/// Rcall instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Rcall(Context context);

		/// <summary>
		/// Ret instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Ret(Context context);

		/// <summary>
		/// Reti instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Reti(Context context);

		/// <summary>
		/// Rjmp instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Rjmp(Context context);

		/// <summary>
		/// Rol instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Rol(Context context);

		/// <summary>
		/// Ror instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Ror(Context context);

		/// <summary>
		/// Sbc instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Sbc(Context context);

		/// <summary>
		/// Sbci instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Sbci(Context context);

		/// <summary>
		/// Sbi instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Sbi(Context context);

		/// <summary>
		/// Sbic instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Sbic(Context context);

		/// <summary>
		/// Sbis instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Sbis(Context context);

		/// <summary>
		/// Sbiw instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Sbiw(Context context);

		/// <summary>
		/// Sbr instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Sbr(Context context);

		/// <summary>
		/// Sbrc instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Sbrc(Context context);

		/// <summary>
		/// Sbrs instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Sbrs(Context context);

		/// <summary>
		/// Sec instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Sec(Context context);

		/// <summary>
		/// Seh instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Seh(Context context);

		/// <summary>
		/// Sei instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Sei(Context context);

		/// <summary>
		/// Sen instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Sen(Context context);

		/// <summary>
		/// Ser instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Ser(Context context);

		/// <summary>
		/// Ses instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Ses(Context context);

		/// <summary>
		/// Set instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Set(Context context);

		/// <summary>
		/// Sev instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Sev(Context context);

		/// <summary>
		/// Sez instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Sez(Context context);

		/// <summary>
		/// Sleep instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Sleep(Context context);

		/// <summary>
		/// St instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void St(Context context);

		/// <summary>
		/// Std instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Std(Context context);

		/// <summary>
		/// Sts instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Sts(Context context);

		/// <summary>
		/// Sub instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Sub(Context context);

		/// <summary>
		/// Subi instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Subi(Context context);

		/// <summary>
		/// Swap instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Swap(Context context);

		/// <summary>
		/// Tst instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Tst(Context context);

		/// <summary>
		/// Wdr instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Wdr(Context context);

	}
}
