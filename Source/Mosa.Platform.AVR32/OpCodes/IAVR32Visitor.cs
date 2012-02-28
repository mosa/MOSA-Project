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
		/// Compare and exchange register - memory
		/// </summary>
		/// <param name="context">The context.</param>
		void CmpXchg(Context context);
		/// <summary>
		/// In instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void In(Context context);
		/// <summary>
		/// Incs instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Inc(Context context);
		/// <summary>
		/// Dec instruction
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
		/// Lea instruction
		/// </summary>
		/// <param name="context">The context.</param>
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
		/// Push on the stack
		/// </summary>
		/// <param name="context">The context.</param>
		void Push(Context context);
		/// <summary>
		/// Read time stamp counter
		/// </summary>
		/// <param name="context">The context.</param>
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
	}
}
