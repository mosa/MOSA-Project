/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using Mosa.Runtime.CompilerFramework;
using IR2 = Mosa.Runtime.CompilerFramework.IR2;

namespace Mosa.Platforms.x86.CPUx86
{
	/// <summary>
	/// Visitor interface for instructions of the intermediate representation.
	/// </summary>
	public class CPUx86CombinedWithIRStage : IR2.IRStage, IX86Visitor, IVisitor
	{

		#region Methods

		/// <summary>
		/// Adds the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void Add(Context context) { }
		/// <summary>
		/// Adcs the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void Adc(Context context) { }
		/// <summary>
		/// Ands the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void And(Context context) { }
		/// <summary>
		/// Ors the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void Or(Context context) { }
		/// <summary>
		/// Xors the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void Xor(Context context) { }
		/// <summary>
		/// Subs the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void Sub(Context context) { }
		/// <summary>
		/// SBBs the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void Sbb(Context context) { }
		/// <summary>
		/// Muls the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void Mul(Context context) { }
		/// <summary>
		/// Directs the multiplication.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void DirectMultiplication(Context context) { }
		/// <summary>
		/// Directs the division.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void DirectDivision(Context context) { }
		/// <summary>
		/// Divs the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void Div(Context context) { }
		/// <summary>
		/// Us the div.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void UDiv(Context context) { }
		/// <summary>
		/// Sses the add.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void SseAdd(Context context) { }
		/// <summary>
		/// Sses the sub.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void SseSub(Context context) { }
		/// <summary>
		/// Sses the mul.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void SseMul(Context context) { }
		/// <summary>
		/// Sses the div.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void SseDiv(Context context) { }
		/// <summary>
		/// Sars the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void Sar(Context context) { }
		/// <summary>
		/// Sals the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void Sal(Context context) { }
		/// <summary>
		/// SHLs the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void Shl(Context context) { }
		/// <summary>
		/// SHRs the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void Shr(Context context) { }
		/// <summary>
		/// RCRs the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void Rcr(Context context) { }
		/// <summary>
		/// Cvtsi2sses the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void Cvtsi2ss(Context context) { }
		/// <summary>
		/// Cvtsi2sds the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void Cvtsi2sd(Context context) { }
		/// <summary>
		/// CVTSD2SSs the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void Cvtsd2ss(Context context) { }
		/// <summary>
		/// CVTSS2SDs the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void Cvtss2sd(Context context) { }
		/// <summary>
		/// CMPs the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void Cmp(Context context) { }
		/// <summary>
		/// Setccs the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void Setcc(Context context) { }
		/// <summary>
		/// CDQs the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void Cdq(Context context) { }
		/// <summary>
		/// SHLDs the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void Shld(Context context) { }
		/// <summary>
		/// SHRDs the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void Shrd(Context context) { }
		/// <summary>
		/// Comisds the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void Comisd(Context context) { }
		/// <summary>
		/// Comisses the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void Comiss(Context context) { }
		/// <summary>
		/// Ucomisds the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void Ucomisd(Context context) { }
		/// <summary>
		/// Ucomisses the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void Ucomiss(Context context) { }
		/// <summary>
		/// JNSs the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void Jns(Context context) { }

		#endregion // Methods

		#region Intrinsics

		/// <summary>
		/// Emits xchg bx, bx to force bochs into debug mode
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void BochsDebug(Context context) { }
		/// <summary>
		/// Disable interrupts
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void Cli(Context context) { }
		/// <summary>
		/// Clear Direction Flag
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void Cld(Context context) { }
		/// <summary>
		/// Compare and exchange register - memory
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void CmpXchg(Context context) { }
		/// <summary>
		/// Retrieves the CPU ID
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void CpuId(Context context) { }
		/// <summary>
		/// Cpus the id eax.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void CpuIdEax(Context context) { }
		/// <summary>
		/// Cpus the id ebx.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void CpuIdEbx(Context context) { }
		/// <summary>
		/// Cpus the id ecx.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void CpuIdEcx(Context context) { }
		/// <summary>
		/// Cpus the id edx.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void CpuIdEdx(Context context) { }
		/// <summary>
		/// Halts the machine
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void Hlt(Context context) { }
		/// <summary>
		/// Invlpgs the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void Invlpg(Context context) { }
		/// <summary>
		/// Read in from port
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void In(Context context) { }
		/// <summary>
		/// Incs the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void Inc(Context context) { }
		/// <summary>
		/// Decs the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void Dec(Context context) { }
		/// <summary>
		/// Call interrupt
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void Int(Context context) { }
		/// <summary>
		/// Return from interrupt
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void Iretd(Context context) { }
		/// <summary>
		/// Load global descriptor table
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void Lgdt(Context context) { }
		/// <summary>
		/// Load interrupt descriptor table
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void Lidt(Context context) { }
		/// <summary>
		/// Locks
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void Lock(Context context) { }
		/// <summary>
		/// Negate with Two-Complement
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void Neg(Context context) { }
		/// <summary>
		/// No operation
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void Nop(Context context) { }
		/// <summary>
		/// Output to port
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void Out(Context context) { }
		/// <summary>
		/// Pause
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void Pause(Context context) { }
		/// <summary>
		/// Pop from the stack
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void Pop(Context context) { }
		/// <summary>
		/// Pops All General-Purpose Registers
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void Popad(Context context) { }
		/// <summary>
		/// Pop Stack into EFLAGS Register
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void Popfd(Context context) { }
		/// <summary>
		/// Push on the stack
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void Push(Context context) { }
		/// <summary>
		/// Push All General-Purpose Registers
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void Pushad(Context context) { }
		/// <summary>
		/// Push EFLAGS Register onto the Stack
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void Pushfd(Context context) { }
		/// <summary>
		/// Read time stamp counter
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void Rdmsr(Context context) { }
		/// <summary>
		/// Read from model specific register
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void Rdpmc(Context context) { }
		/// <summary>
		/// Read time stamp counter
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void Rdtsc(Context context) { }
		/// <summary>
		/// Repeat String Operation Prefix
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void Rep(Context context) { }
		/// <summary>
		/// Enable interrupts
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void Sti(Context context) { }
		/// <summary>
		/// Store String
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void Stosb(Context context) { }
		/// <summary>
		/// Store String
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void Stosd(Context context) { }
		/// <summary>
		/// Exchanges register/memory
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void Xchg(Context context) { }

		#endregion // Intrinsics

	}
}
