// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Platform.ARMv6.Instructions;

namespace Mosa.Platform.ARMv6
{
	/// <summary>
	///
	/// </summary>
	public static class ARMv6
	{
		/// <summary>
		/// B instruction
		/// </summary>
		public static readonly B B = new B();

		/// <summary>
		/// Bl instruction
		/// </summary>
		public static readonly Bl Bl = new Bl();

		/// <summary>
		/// Blx instruction
		/// </summary>
		public static readonly Blx Blx = new Blx();

		/// <summary>
		/// Bx instruction
		/// </summary>
		public static readonly Bx Bx = new Bx();

		/// <summary>
		/// Adc instruction
		/// </summary>
		public static readonly Adc Adc = new Adc();

		/// <summary>
		/// Add instruction
		/// </summary>
		public static readonly Add Add = new Add();

		/// <summary>
		/// Adr instruction
		/// </summary>
		public static readonly Adr Adr = new Adr();

		/// <summary>
		/// And instruction
		/// </summary>
		public static readonly And And = new And();

		/// <summary>
		/// Bic instruction
		/// </summary>
		public static readonly Bic Bic = new Bic();

		/// <summary>
		/// Cmn instruction
		/// </summary>
		public static readonly Cmn Cmn = new Cmn();

		/// <summary>
		/// Cmp instruction
		/// </summary>
		public static readonly Cmp Cmp = new Cmp();

		/// <summary>
		/// Eor instruction
		/// </summary>
		public static readonly Eor Eor = new Eor();

		/// <summary>
		/// Mov instruction
		/// </summary>
		public static readonly Mov Mov = new Mov();

		/// <summary>
		/// Mvn instruction
		/// </summary>
		public static readonly Mvn Mvn = new Mvn();

		/// <summary>
		/// Orr instruction
		/// </summary>
		public static readonly Orr Orr = new Orr();

		/// <summary>
		/// Rsb instruction
		/// </summary>
		public static readonly Rsb Rsb = new Rsb();

		/// <summary>
		/// Rsc instruction
		/// </summary>
		public static readonly Rsc Rsc = new Rsc();

		/// <summary>
		/// Sbc instruction
		/// </summary>
		public static readonly Sbc Sbc = new Sbc();

		/// <summary>
		/// Sub instruction
		/// </summary>
		public static readonly Sub Sub = new Sub();

		/// <summary>
		/// Tst instruction
		/// </summary>
		public static readonly Tst Tst = new Tst();

		/// <summary>
		/// Teq instruction
		/// </summary>
		public static readonly Teq Teq = new Teq();

		/// <summary>
		/// Asr instruction
		/// </summary>
		public static readonly Asr Asr = new Asr();

		/// <summary>
		/// Lsl instruction
		/// </summary>
		public static readonly Lsl Lsl = new Lsl();

		/// <summary>
		/// Lsr instruction
		/// </summary>
		public static readonly Lsr Lsr = new Lsr();

		/// <summary>
		/// Ror instruction
		/// </summary>
		public static readonly Ror Ror = new Ror();

		/// <summary>
		/// Mul instruction
		/// </summary>
		public static readonly Mul Mul = new Mul();

		/// <summary>
		/// Sxtb instruction
		/// </summary>
		public static readonly Sxtb Sxtb = new Sxtb();

		/// <summary>
		/// Sxth instruction
		/// </summary>
		public static readonly Sxth Sxth = new Sxth();

		/// <summary>
		/// Uxtb instruction
		/// </summary>
		public static readonly Uxtb Uxtb = new Uxtb();

		/// <summary>
		/// Uxth instruction
		/// </summary>
		public static readonly Uxth Uxth = new Uxth();

		/// <summary>
		/// Rev instruction
		/// </summary>
		public static readonly Rev Rev = new Rev();

		/// <summary>
		/// Rev16 instruction
		/// </summary>
		public static readonly Rev16 Rev16 = new Rev16();

		/// <summary>
		/// Revsh instruction
		/// </summary>
		public static readonly Revsh Revsh = new Revsh();

		/// <summary>
		/// Mrs instruction
		/// </summary>
		public static readonly Mrs Mrs = new Mrs();

		/// <summary>
		/// Msr instruction
		/// </summary>
		public static readonly Msr Msr = new Msr();

		/// <summary>
		/// Ldr instruction
		/// </summary>
		public static readonly Ldr Ldr = new Ldr();

		/// <summary>
		/// Str instruction
		/// </summary>
		public static readonly Str Str = new Str();

		/// <summary>
		/// Strh instruction
		/// </summary>
		public static readonly Strh Strh = new Strh();

		/// <summary>
		/// Ldrh instruction
		/// </summary>
		public static readonly Ldrh Ldrh = new Ldrh();

		/// <summary>
		/// Ldrsh instruction
		/// </summary>
		public static readonly Ldrsh Ldrsh = new Ldrsh();

		/// <summary>
		/// Strb instruction
		/// </summary>
		public static readonly Strb Strb = new Strb();

		/// <summary>
		/// Ldrb instruction
		/// </summary>
		public static readonly Ldrb Ldrb = new Ldrb();

		/// <summary>
		/// Ldrsb instruction
		/// </summary>
		public static readonly Ldrsb Ldrsb = new Ldrsb();

		/// <summary>
		/// Ldm instruction
		/// </summary>
		public static readonly Ldm Ldm = new Ldm();

		/// <summary>
		/// Ldmia instruction
		/// </summary>
		public static readonly Ldmia Ldmia = new Ldmia();

		/// <summary>
		/// Ldmfd instruction
		/// </summary>
		public static readonly Ldmfd Ldmfd = new Ldmfd();

		/// <summary>
		/// Pop instruction
		/// </summary>
		public static readonly Pop Pop = new Pop();

		/// <summary>
		/// Push instruction
		/// </summary>
		public static readonly Push Push = new Push();

		/// <summary>
		/// Stm instruction
		/// </summary>
		public static readonly Stm Stm = new Stm();

		/// <summary>
		/// Stmia instruction
		/// </summary>
		public static readonly Stmia Stmia = new Stmia();

		/// <summary>
		/// Stmea instruction
		/// </summary>
		public static readonly Stmea Stmea = new Stmea();

		/// <summary>
		/// Dmb instruction
		/// </summary>
		public static readonly Dmb Dmb = new Dmb();

		/// <summary>
		/// Dsb instruction
		/// </summary>
		public static readonly Dsb Dsb = new Dsb();

		/// <summary>
		/// Isb instruction
		/// </summary>
		public static readonly Isb Isb = new Isb();

		/// <summary>
		/// Nop instruction
		/// </summary>
		public static readonly Nop Nop = new Nop();

		/// <summary>
		/// Sev instruction
		/// </summary>
		public static readonly Sev Sev = new Sev();

		/// <summary>
		/// Svc instruction
		/// </summary>
		public static readonly Svc Svc = new Svc();

		/// <summary>
		/// Wfe instruction
		/// </summary>
		public static readonly Wfe Wfe = new Wfe();

		/// <summary>
		/// Wfi instruction
		/// </summary>
		public static readonly Wfi Wfi = new Wfi();

		/// <summary>
		/// Yield instruction
		/// </summary>
		public static readonly Yield Yield = new Yield();

		/// <summary>
		/// Swi instruction
		/// </summary>
		public static readonly Swi Swi = new Swi();

		/// <summary>
		/// Bkpt instruction
		/// </summary>
		public static readonly Bkpt Bkpt = new Bkpt();
	}
}