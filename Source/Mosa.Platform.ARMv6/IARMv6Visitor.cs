/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv6
{
	/// <summary>
	///
	/// </summary>
	public interface IARMv6Visitor : IVisitor
	{
		/// <summary>
		/// B instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void B(Context context);

		/// <summary>
		/// Bl instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Bl(Context context);

		/// <summary>
		/// Blx instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Blx(Context context);

		/// <summary>
		/// Bx instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Bx(Context context);

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
		/// Adr instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Adr(Context context);

		/// <summary>
		/// And instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void And(Context context);

		/// <summary>
		/// Bic instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Bic(Context context);

		/// <summary>
		/// Cmn instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Cmn(Context context);

		/// <summary>
		/// Cmp instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Cmp(Context context);

		/// <summary>
		/// Eor instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Eor(Context context);

		/// <summary>
		/// Mov instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Mov(Context context);

		/// <summary>
		/// Mvn instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Mvn(Context context);

		/// <summary>
		/// Orr instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Orr(Context context);

		/// <summary>
		/// Rsb instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Rsb(Context context);

		/// <summary>
		/// Sbc instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Sbc(Context context);

		/// <summary>
		/// Sub instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Sub(Context context);

		/// <summary>
		/// Tst instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Tst(Context context);

		/// <summary>
		/// Asr instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Asr(Context context);

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
		/// Ror instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Ror(Context context);

		/// <summary>
		/// Mul instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Mul(Context context);

		/// <summary>
		/// Sxtb instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Sxtb(Context context);

		/// <summary>
		/// Sxth instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Sxth(Context context);

		/// <summary>
		/// Uxtb instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Uxtb(Context context);

		/// <summary>
		/// Uxth instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Uxth(Context context);

		/// <summary>
		/// Rev instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Rev(Context context);

		/// <summary>
		/// Rev16 instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Rev16(Context context);

		/// <summary>
		/// Revsh instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Revsh(Context context);

		/// <summary>
		/// Mrs instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Mrs(Context context);

		/// <summary>
		/// Msr instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Msr(Context context);

		/// <summary>
		/// Ldr instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Ldr(Context context);

		/// <summary>
		/// Str instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Str(Context context);

		/// <summary>
		/// Strh instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Strh(Context context);

		/// <summary>
		/// Ldrh instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Ldrh(Context context);

		/// <summary>
		/// Ldrsh instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Ldrsh(Context context);

		/// <summary>
		/// Strb instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Strb(Context context);

		/// <summary>
		/// Ldrb instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Ldrb(Context context);

		/// <summary>
		/// Ldrsb instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Ldrsb(Context context);

		/// <summary>
		/// Ldm instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Ldm(Context context);

		/// <summary>
		/// Ldmia instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Ldmia(Context context);

		/// <summary>
		/// Ldmfd instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Ldmfd(Context context);

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
		/// Stm instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Stm(Context context);

		/// <summary>
		/// Stmia instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Stmia(Context context);

		/// <summary>
		/// Stmea instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Stmea(Context context);

		/// <summary>
		/// Dmb instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Dmb(Context context);

		/// <summary>
		/// Dsb instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Dsb(Context context);

		/// <summary>
		/// Isb instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Isb(Context context);

		/// <summary>
		/// Nop instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Nop(Context context);

		/// <summary>
		/// Sev instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Sev(Context context);

		/// <summary>
		/// Svc instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Svc(Context context);

		/// <summary>
		/// Wfe instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Wfe(Context context);

		/// <summary>
		/// Wfi instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Wfi(Context context);

		/// <summary>
		/// Yield instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Yield(Context context);

		/// <summary>
		/// Swi instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Swi(Context context);

		/// <summary>
		/// Bkpt instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Bkpt(Context context);
	}
}