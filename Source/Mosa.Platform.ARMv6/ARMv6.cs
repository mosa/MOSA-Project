// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Platform.ARMv6.Instructions;

namespace Mosa.Platform.ARMv6
{
	public static class ARMv6
	{
		public static readonly Adc Adc = new Adc();
		public static readonly Add Add = new Add();
		public static readonly Adr Adr = new Adr();
		public static readonly And And = new And();
		public static readonly Asr Asr = new Asr();
		public static readonly B B = new B();
		public static readonly Bic Bic = new Bic();
		public static readonly Bkpt Bkpt = new Bkpt();
		public static readonly Bl Bl = new Bl();
		public static readonly Blx Blx = new Blx();
		public static readonly Bx Bx = new Bx();
		public static readonly Cmn Cmn = new Cmn();
		public static readonly Cmp Cmp = new Cmp();
		public static readonly Dmb Dmb = new Dmb();
		public static readonly Dsb Dsb = new Dsb();
		public static readonly Eor Eor = new Eor();
		public static readonly Isb Isb = new Isb();
		public static readonly Ldm Ldm = new Ldm();
		public static readonly Ldmfd Ldmfd = new Ldmfd();
		public static readonly Ldmia Ldmia = new Ldmia();
		public static readonly Ldr Ldr = new Ldr();
		public static readonly Ldrb Ldrb = new Ldrb();
		public static readonly Ldrh Ldrh = new Ldrh();
		public static readonly Ldrsb Ldrsb = new Ldrsb();
		public static readonly Ldrsh Ldrsh = new Ldrsh();
		public static readonly Lsl Lsl = new Lsl();
		public static readonly Lsr Lsr = new Lsr();
		public static readonly Mov Mov = new Mov();
		public static readonly Mrs Mrs = new Mrs();
		public static readonly Msr Msr = new Msr();
		public static readonly Mul Mul = new Mul();
		public static readonly Mvn Mvn = new Mvn();
		public static readonly Nop Nop = new Nop();
		public static readonly Orr Orr = new Orr();
		public static readonly Pop Pop = new Pop();
		public static readonly Push Push = new Push();
		public static readonly Rev Rev = new Rev();
		public static readonly Rev16 Rev16 = new Rev16();
		public static readonly Revsh Revsh = new Revsh();
		public static readonly Ror Ror = new Ror();
		public static readonly Rsb Rsb = new Rsb();
		public static readonly Rsc Rsc = new Rsc();
		public static readonly Sbc Sbc = new Sbc();
		public static readonly Sev Sev = new Sev();
		public static readonly Stm Stm = new Stm();
		public static readonly Stmea Stmea = new Stmea();
		public static readonly Stmia Stmia = new Stmia();
		public static readonly Str Str = new Str();
		public static readonly Strb Strb = new Strb();
		public static readonly Strh Strh = new Strh();
		public static readonly Sub Sub = new Sub();
		public static readonly Svc Svc = new Svc();
		public static readonly Swi Swi = new Swi();
		public static readonly Sxtb Sxtb = new Sxtb();
		public static readonly Sxth Sxth = new Sxth();
		public static readonly Teq Teq = new Teq();
		public static readonly Tst Tst = new Tst();
		public static readonly Uxtb Uxtb = new Uxtb();
		public static readonly Uxth Uxth = new Uxth();
		public static readonly Wfe Wfe = new Wfe();
		public static readonly Wfi Wfi = new Wfi();
		public static readonly Yield Yield = new Yield();
	}
}
