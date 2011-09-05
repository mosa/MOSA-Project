/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Runtime.CompilerFramework;

namespace Mosa.Platform.x86.CPUx86
{
	/// <summary>
	/// 
	/// </summary>
	public interface IX86Visitor : IVisitor
	{
		/// <summary>
		/// Adds instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Add(Context context);
		/// <summary>
		/// Adcs instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Adc(Context context);
		/// <summary>
		/// Ands instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void And(Context context);
		/// <summary>
		/// Cmp instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Call(Context context);
		/// <summary>
		/// Directs the compare.
		/// </summary>
		/// <param name="context">The context.</param>
		void DirectCompare(Context context);
		/// <summary>
		/// Or instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Cmp(Context context);
		/// <summary>
		/// Or instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Or(Context context);
		/// <summary>
		/// Xor instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Xor(Context context);
		/// <summary>
		/// Subs instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Sub(Context context);
		/// <summary>
		/// SBBs instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Sbb(Context context);
		/// <summary>
		/// Muls instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Mul(Context context);
		/// <summary>
		/// Directs the multiplication.
		/// </summary>
		/// <param name="context">The context.</param>
		void DirectMultiplication(Context context);
		/// <summary>
		/// Directs the division.
		/// </summary>
		/// <param name="context">The context.</param>
		void DirectDivision(Context context);
		/// <summary>
		/// Divs instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Div(Context context);
		/// <summary>
		/// Us the div.
		/// </summary>
		/// <param name="context">The context.</param>
		void UDiv(Context context);
		/// <summary>
		/// Sses the add.
		/// </summary>
		/// <param name="context">The context.</param>
		void SseAdd(Context context);
		/// <summary>
		/// Sses the sub.
		/// </summary>
		/// <param name="context">The context.</param>
		void SseSub(Context context);
		/// <summary>
		/// Sses the mul.
		/// </summary>
		/// <param name="context">The context.</param>
		void SseMul(Context context);
		/// <summary>
		/// Sses the div.
		/// </summary>
		/// <param name="context">The context.</param>
		void SseDiv(Context context);
		/// <summary>
		/// Sars instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Sar(Context context);
		/// <summary>
		/// Sals instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Sal(Context context);
		/// <summary>
		/// SHLs instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Shl(Context context);
		/// <summary>
		/// SHRs instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Shr(Context context);
		/// <summary>
		/// RCRs instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Rcr(Context context);
		/// <summary>
		/// Cvtsi2sses instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Cvtsi2ss(Context context);
		/// <summary>
		/// Cvtsi2sds instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Cvtsi2sd(Context context);
		/// <summary>
		/// CVTSD2SSs instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Cvtsd2ss(Context context);
		/// <summary>
		/// CVTSS2SDs instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Cvtss2sd(Context context);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		void Cvttsd2si(Context context);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		void Cvttss2si(Context context);
		/// <summary>
		/// Setccs instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Setcc(Context context);
		/// <summary>
		/// CDQs instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Cdq(Context context);
		/// <summary>
		/// SHLDs instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Shld(Context context);
		/// <summary>
		/// SHRDs instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Shrd(Context context);
		/// <summary>
		/// Comisds instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Comisd(Context context);
		/// <summary>
		/// Comisses instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Comiss(Context context);
		/// <summary>
		/// Ucomisds instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Ucomisd(Context context);
		/// <summary>
		/// Ucomisses instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Ucomiss(Context context);
		/// <summary>
		/// JNSs instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Jns(Context context);
		/// <summary>
		/// X86 branch instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Branch(Context context);
		/// <summary>
		/// Jumps instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Jump(Context context);
		/// <summary>
		/// Emits xchg bx, bx to force bochs into debug mode
		/// </summary>
		/// <param name="context">The context.</param>
		void BochsDebug(Context context);
		/// <summary>
		/// Disable interrupts
		/// </summary>
		/// <param name="context">The context.</param>
		void Cli(Context context);
		/// <summary>
		/// Clear Direction Flag
		/// </summary>
		/// <param name="context">The context.</param>
		void Cld(Context context);
		/// <summary>
		/// Compare and exchange register - memory
		/// </summary>
		/// <param name="context">The context.</param>
		void CmpXchg(Context context);
		/// <summary>
		/// Retrieves the CPU ID
		/// </summary>
		/// <param name="context">The context.</param>
		void CpuId(Context context);
		/// <summary>
		/// Cpus the id eax.
		/// </summary>
		/// <param name="context">The context.</param>
		void CpuIdEax(Context context);
		/// <summary>
		/// Cpus the id ebx.
		/// </summary>
		/// <param name="context">The context.</param>
		void CpuIdEbx(Context context);
		/// <summary>
		/// Cpus the id ecx.
		/// </summary>
		/// <param name="context">The context.</param>
		void CpuIdEcx(Context context);
		/// <summary>
		/// Cpus the id edx.
		/// </summary>
		/// <param name="context">The context.</param>
		void CpuIdEdx(Context context);
		/// <summary>
		/// Halts the machine
		/// </summary>
		/// <param name="context">The context.</param>
		void Hlt(Context context);
		/// <summary>
		/// Invlpgs instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Invlpg(Context context);
		/// <summary>
		/// Read in from port
		/// </summary>
		/// <param name="context">The context.</param>
		void In(Context context);
		/// <summary>
		/// Incs instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Inc(Context context);
		/// <summary>
		/// Decs instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Dec(Context context);
		/// <summary>
		/// Call interrupt
		/// </summary>
		/// <param name="context">The context.</param>
		void Int(Context context);
		/// <summary>
		/// Return from interrupt
		/// </summary>
		/// <param name="context">The context.</param>
		void Iretd(Context context);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		void Lea(Context context);
		/// <summary>
		/// Load global descriptor table
		/// </summary>
		/// <param name="context">The context.</param>
		void Lgdt(Context context);
		/// <summary>
		/// Load interrupt descriptor table
		/// </summary>
		/// <param name="context">The context.</param>
		void Lidt(Context context);
		/// <summary>
		/// Locks
		/// </summary>
		/// <param name="context">The context.</param>
		void Lock(Context context);
		/// <summary>
		/// Negate with Two-Complement
		/// </summary>
		/// <param name="context">The context.</param>
		void Neg(Context context);
		/// <summary>
		/// Move 
		/// </summary>
		/// <param name="context">The context.</param>
		void Mov(Context context);
		/// <summary>
		/// Move with Sign-Extension
		/// </summary>
		/// <param name="context">The context.</param>
		void Movsx(Context context);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		void Movss(Context context);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		void Movsd(Context context);
		/// <summary>
		/// Move with Zero-Extension
		/// </summary>
		/// <param name="context">The context.</param>
		void Movzx(Context context);
		/// <summary>
		/// Output to port
		/// </summary>
		/// <param name="context">The context.</param>
		void Nop(Context context);
		/// <summary>
		/// Output to port
		/// </summary>
		/// <param name="context">The context.</param>
		void Out(Context context);
		/// <summary>
		/// Pause
		/// </summary>
		/// <param name="context">The context.</param>
		void Pause(Context context);
		/// <summary>
		/// Pop from the stack
		/// </summary>
		/// <param name="context">The context.</param>
		void Pop(Context context);
		/// <summary>
		/// Pops All General-Purpose Registers
		/// </summary>
		/// <param name="context">The context.</param>
		void Popad(Context context);
		/// <summary>
		/// Pop Stack into EFLAGS Register
		/// </summary>
		/// <param name="context">The context.</param>
		void Popfd(Context context);
		/// <summary>
		/// Push on the stack
		/// </summary>
		/// <param name="context">The context.</param>
		void Push(Context context);
		/// <summary>
		/// Push All General-Purpose Registers
		/// </summary>
		/// <param name="context">The context.</param>
		void Pushad(Context context);
		/// <summary>
		/// Push EFLAGS Register onto the Stack
		/// </summary>
		/// <param name="context">The context.</param>
		void Pushfd(Context context);
		/// <summary>
		/// Read time stamp counter
		/// </summary>
		/// <param name="context">The context.</param>
		void Rdmsr(Context context);
		/// <summary>
		/// Read from model specific register
		/// </summary>
		/// <param name="context">The context.</param>
		void Rdpmc(Context context);
		/// <summary>
		/// Read time stamp counter
		/// </summary>
		/// <param name="context">The context.</param>
		void Rdtsc(Context context);
		/// <summary>
		/// Repeat String Operation Prefix
		/// </summary>
		/// <param name="context">The context.</param>
		void Rep(Context context);
		/// <summary>
		/// Enable interrupts
		/// </summary>
		/// <param name="context">The context.</param>
		void Sti(Context context);
		/// <summary>
		/// Store String
		/// </summary>
		/// <param name="context">The context.</param>
		void Stosb(Context context);
		/// <summary>
		/// Store String
		/// </summary>
		/// <param name="context">The context.</param>
		void Stosd(Context context);
		/// <summary>
		/// Exchanges register/memory
		/// </summary>
		/// <param name="context">The context.</param>
		void Xchg(Context context);
		/// <summary>
		/// Not operation
		/// </summary>
		/// <param name="context">The context.</param>
		void Not(Context context);
		/// <summary>
		/// Sses round instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void SseRound(Context context);
	}
}
