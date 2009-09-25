/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Text;
using Mosa.Runtime.CompilerFramework;

namespace Mosa.Platforms.x86.CPUx86
{
	/// <summary>
	/// 
	/// </summary>
	public interface IX86Visitor
	{
		/// <summary>
		/// Adds the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		void Add(Context context);
		/// <summary>
		/// Adcs the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		void Adc(Context context);
		/// <summary>
		/// Ands the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		void And(Context context);
		/// <summary>
		/// Ors the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		void Or(Context context);
		/// <summary>
		/// Xors the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		void Xor(Context context);
		/// <summary>
		/// Subs the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		void Sub(Context context);
		/// <summary>
		/// SBBs the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		void Sbb(Context context);
		/// <summary>
		/// Muls the specified context.
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
		/// Divs the specified context.
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
		/// Sars the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		void Sar(Context context);
		/// <summary>
		/// Sals the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		void Sal(Context context);
		/// <summary>
		/// SHLs the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		void Shl(Context context);
		/// <summary>
		/// SHRs the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		void Shr(Context context);
		/// <summary>
		/// RCRs the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		void Rcr(Context context);
		/// <summary>
		/// Cvtsi2sses the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		void Cvtsi2ss(Context context);
		/// <summary>
		/// Cvtsi2sds the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		void Cvtsi2sd(Context context);
		/// <summary>
		/// CVTSD2SSs the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		void Cvtsd2ss(Context context);
		/// <summary>
		/// CVTSS2SDs the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		void Cvtss2sd(Context context);
		/// <summary>
		/// CMPs the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		void Cmp(Context context);
		/// <summary>
		/// Setccs the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		void Setcc(Context context);
		/// <summary>
		/// CDQs the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		void Cdq(Context context);
		/// <summary>
		/// SHLDs the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		void Shld(Context context);
		/// <summary>
		/// SHRDs the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		void Shrd(Context context);
		/// <summary>
		/// Comisds the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		void Comisd(Context context);
		/// <summary>
		/// Comisses the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		void Comiss(Context context);
		/// <summary>
		/// Ucomisds the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		void Ucomisd(Context context);
		/// <summary>
		/// Ucomisses the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		void Ucomiss(Context context);
		/// <summary>
		/// JNSs the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		void Jns(Context context);

		#region Intrinsics

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
		/// Invlpgs the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		void Invlpg(Context context);
		/// <summary>
		/// Read in from port
		/// </summary>
		/// <param name="context">The context.</param>
		void In(Context context);
		/// <summary>
		/// Incs the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		void Inc(Context context);
		/// <summary>
		/// Decs the specified context.
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
		/// No operation
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

		#endregion
	}
}
