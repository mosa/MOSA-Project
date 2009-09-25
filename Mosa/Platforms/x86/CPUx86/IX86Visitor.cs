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
		void Add(Context ctx);
		void Adc(Context ctx);

		void And(Context ctx);
		void Or(Context ctx);
		void Xor(Context ctx);

		void Sub(Context ctx);
		void Sbb(Context ctx);
		void Mul(Context ctx);
		void DirectMultiplication(Context ctx);
		void DirectDivision(Context ctx);
		void Div(Context ctx);
		void UDiv(Context ctx);
		void SseAdd(Context ctx);
		void SseSub(Context ctx);
		void SseMul(Context ctx);
		void SseDiv(Context ctx);
		void Sar(Context ctx);
		void Sal(Context ctx);
		void Shl(Context ctx);
		void Shr(Context ctx);
		void Rcr(Context ctx);

		void Cvtsi2ss(Context ctx);
		void Cvtsi2sd(Context ctx);
		void Cvtsd2ss(Context ctx);
		void Cvtss2sd(Context ctx);

		void Cmp(Context ctx);
		void Setcc(Context ctx);
		void Cdq(Context ctx);

		void Shld(Context ctx);
		void Shrd(Context ctx);

		void Comisd(Context ctx);
		void Comiss(Context ctx);
		void Ucomisd(Context ctx);
		void Ucomiss(Context ctx);

		void Jns(Context ctx);

		#region Intrinsics

		/// <summary>
		/// Emits xchg bx, bx to force bochs into debug mode
		/// </summary>
		/// <param name="instruction">The instruction.</param>

		void BochsDebug(Context ctx);
		/// <summary>
		/// Disable interrupts
		/// </summary>
		/// <param name="instruction">The instruction.</param>

		void Cli(Context ctx);
		/// <summary>
		/// Clear Direction Flag
		/// </summary>
		/// <param name="instruction">The instruction.</param>

		void Cld(Context ctx);
		/// <summary>
		/// Compare and exchange register - memory
		/// </summary>
		/// <param name="instruction">The instruction.</param>

		void CmpXchg(Context ctx);
		/// <summary>
		/// Retrieves the CPU ID
		/// </summary>
		/// <param name="instruction">The instruction.</param>

		void CpuId(Context ctx);
		void CpuIdEax(Context ctx);
		void CpuIdEbx(Context ctx);
		void CpuIdEcx(Context ctx);
		void CpuIdEdx(Context ctx);
		/// <summary>
		/// Halts the machine
		/// </summary>
		/// <param name="instruction">The instruction.</param>

		void Hlt(Context ctx);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="instruction"></param>
		/// <param name="arg"></param>
		void Invlpg(Context ctx);
		/// <summary>
		/// Read in from port
		/// </summary>
		/// <param name="instruction">The instruction.</param>

		void In(Context ctx);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="instruction"></param>
		/// <param name="arg"></param>
		void Inc(Context ctx);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="instruction"></param>
		/// <param name="arg"></param>
		void Dec(Context ctx);
		/// <summary>
		/// Call interrupt
		/// </summary>
		/// <param name="instruction">The instruction.</param>

		void Int(Context ctx);
		/// <summary>
		/// Return from interrupt
		/// </summary>
		/// <param name="instruction">The instruction.</param>

		void Iretd(Context ctx);
		/// <summary>
		/// Load global descriptor table
		/// </summary>
		/// <param name="instruction">The instruction.</param>

		void Lgdt(Context ctx);
		/// <summary>
		/// Load interrupt descriptor table
		/// </summary>
		/// <param name="instruction">The instruction.</param>

		void Lidt(Context ctx);
		/// <summary>
		/// Locks
		/// </summary>
		/// <param name="instruction">The instruction.</param>

		void Lock(Context ctx);
		/// <summary>
		/// Negate with Two-Complement
		/// </summary>
		/// <param name="instruction">The instruction.</param>

		void Neg(Context ctx);
		/// <summary>
		/// No operation
		/// </summary>
		/// <param name="instruction">The instruction.</param>

		void Nop(Context ctx);
		/// <summary>
		/// Output to port
		/// </summary>
		/// <param name="instruction">The instruction.</param>

		void Out(Context ctx);
		/// <summary>
		/// Pause
		/// </summary>
		/// <param name="instruction">The instruction.</param>

		void Pause(Context ctx);
		/// <summary>
		/// Pop from the stack
		/// </summary>
		/// <param name="instruction">The instruction.</param>

		void Pop(Context ctx);
		/// <summary>
		/// Pops All General-Purpose Registers
		/// </summary>
		/// <param name="instruction">The instruction.</param>

		void Popad(Context ctx);
		/// <summary>
		/// Pop Stack into EFLAGS Register
		/// </summary>
		/// <param name="instruction">The instruction.</param>

		void Popfd(Context ctx);
		/// <summary>
		/// Push on the stack
		/// </summary>
		/// <param name="instruction">The instruction.</param>

		void Push(Context ctx);
		/// <summary>
		/// Push All General-Purpose Registers
		/// </summary>
		/// <param name="instruction">The instruction.</param>

		void Pushad(Context ctx);
		/// <summary>
		/// Push EFLAGS Register onto the Stack
		/// </summary>
		/// <param name="instruction">The instruction.</param>

		void Pushfd(Context ctx);
		/// <summary>
		/// Read time stamp counter
		/// </summary>
		/// <param name="instruction">The instruction.</param>

		void Rdmsr(Context ctx);
		/// <summary>
		/// Read from model specific register
		/// </summary>
		/// <param name="instruction">The instruction.</param>

		void Rdpmc(Context ctx);
		/// <summary>
		/// Read time stamp counter
		/// </summary>
		/// <param name="instruction">The instruction.</param>

		void Rdtsc(Context ctx);
		/// <summary>
		/// Repeat String Operation Prefix
		/// </summary>
		/// <param name="instruction">The instruction.</param>

		void Rep(Context ctx);
		/// <summary>
		/// Enable interrupts
		/// </summary>
		/// <param name="instruction">The instruction.</param>

		void Sti(Context ctx);
		/// <summary>
		/// Store String
		/// </summary>
		/// <param name="instruction">The instruction.</param>

		void Stosb(Context ctx);
		/// <summary>
		/// Store String
		/// </summary>
		/// <param name="instruction">The instruction.</param>

		void Stosd(Context ctx);
		/// <summary>
		/// Exchanges register/memory
		/// </summary>
		/// <param name="instruction">The instruction.</param>

		void Xchg(Context ctx);
		#endregion
	}
}
