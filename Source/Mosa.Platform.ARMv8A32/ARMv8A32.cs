// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;
using Mosa.Platform.ARMv8A32.Instructions;

namespace Mosa.Platform.ARMv8A32;

/// <summary>
/// ARMv8A32 Instructions
/// </summary>
public static class ARMv8A32
{
	public static readonly BaseInstruction Adc = new Adc();
	public static readonly BaseInstruction AdcRegShift = new AdcRegShift();
	public static readonly BaseInstruction Add = new Add();
	public static readonly BaseInstruction AddRegShift = new AddRegShift();
	public static readonly BaseInstruction And = new And();
	public static readonly BaseInstruction AndRegShift = new AndRegShift();
	public static readonly BaseInstruction Bic = new Bic();
	public static readonly BaseInstruction BicRegShift = new BicRegShift();
	public static readonly BaseInstruction Cmn = new Cmn();
	public static readonly BaseInstruction CmnRegShift = new CmnRegShift();
	public static readonly BaseInstruction Cmp = new Cmp();
	public static readonly BaseInstruction CmpRegShift = new CmpRegShift();
	public static readonly BaseInstruction Eor = new Eor();
	public static readonly BaseInstruction EorRegShift = new EorRegShift();
	public static readonly BaseInstruction Mvn = new Mvn();
	public static readonly BaseInstruction MvnRegShift = new MvnRegShift();
	public static readonly BaseInstruction Orr = new Orr();
	public static readonly BaseInstruction OrrRegShift = new OrrRegShift();
	public static readonly BaseInstruction Rsb = new Rsb();
	public static readonly BaseInstruction RsbRegShift = new RsbRegShift();
	public static readonly BaseInstruction Rsc = new Rsc();
	public static readonly BaseInstruction RscRegShift = new RscRegShift();
	public static readonly BaseInstruction Sbc = new Sbc();
	public static readonly BaseInstruction SbcRegShift = new SbcRegShift();
	public static readonly BaseInstruction Sub = new Sub();
	public static readonly BaseInstruction SubRegShift = new SubRegShift();
	public static readonly BaseInstruction Teq = new Teq();
	public static readonly BaseInstruction TeqRegShift = new TeqRegShift();
	public static readonly BaseInstruction Tst = new Tst();
	public static readonly BaseInstruction TstRegShift = new TstRegShift();
	public static readonly BaseInstruction Bl = new Bl();
	public static readonly BaseInstruction B = new B();
	public static readonly BaseInstruction Bx = new Bx();
	public static readonly BaseInstruction Bkpt = new Bkpt();
	public static readonly BaseInstruction Dmb = new Dmb();
	public static readonly BaseInstruction Dsb = new Dsb();
	public static readonly BaseInstruction Isb = new Isb();
	public static readonly BaseInstruction Ldr32 = new Ldr32();
	public static readonly BaseInstruction Ldr8 = new Ldr8();
	public static readonly BaseInstruction Ldr16 = new Ldr16();
	public static readonly BaseInstruction LdrS16 = new LdrS16();
	public static readonly BaseInstruction LdrS8 = new LdrS8();
	public static readonly BaseInstruction Str32 = new Str32();
	public static readonly BaseInstruction Str8 = new Str8();
	public static readonly BaseInstruction Str16 = new Str16();
	public static readonly BaseInstruction StrS16 = new StrS16();
	public static readonly BaseInstruction StrS8 = new StrS8();
	public static readonly BaseInstruction Mul = new Mul();
	public static readonly BaseInstruction Mla = new Mla();
	public static readonly BaseInstruction UMull = new UMull();
	public static readonly BaseInstruction UMlal = new UMlal();
	public static readonly BaseInstruction SMull = new SMull();
	public static readonly BaseInstruction SMlal = new SMlal();
	public static readonly BaseInstruction Nop = new Nop();
	public static readonly BaseInstruction Mrs = new Mrs();
	public static readonly BaseInstruction Msr = new Msr();
	public static readonly BaseInstruction Ldm = new Ldm();
	public static readonly BaseInstruction Stm = new Stm();
	public static readonly BaseInstruction Pop = new Pop();
	public static readonly BaseInstruction Push = new Push();
	public static readonly BaseInstruction Rev = new Rev();
	public static readonly BaseInstruction Rev16 = new Rev16();
	public static readonly BaseInstruction Revsh = new Revsh();
	public static readonly BaseInstruction Sev = new Sev();
	public static readonly BaseInstruction Svc = new Svc();
	public static readonly BaseInstruction Swi = new Swi();
	public static readonly BaseInstruction Sxtb = new Sxtb();
	public static readonly BaseInstruction Sxth = new Sxth();
	public static readonly BaseInstruction Uxtb = new Uxtb();
	public static readonly BaseInstruction Uxth = new Uxth();
	public static readonly BaseInstruction Wfe = new Wfe();
	public static readonly BaseInstruction Wfi = new Wfi();
	public static readonly BaseInstruction Yield = new Yield();
	public static readonly BaseInstruction Movt = new Movt();
	public static readonly BaseInstruction Movw = new Movw();
	public static readonly BaseInstruction Mov = new Mov();
	public static readonly BaseInstruction MovRegShift = new MovRegShift();
	public static readonly BaseInstruction Lsl = new Lsl();
	public static readonly BaseInstruction Lsr = new Lsr();
	public static readonly BaseInstruction Asr = new Asr();
	public static readonly BaseInstruction Ror = new Ror();
	public static readonly BaseInstruction Adf = new Adf();
	public static readonly BaseInstruction Muf = new Muf();
	public static readonly BaseInstruction Suf = new Suf();
	public static readonly BaseInstruction Rsf = new Rsf();
	public static readonly BaseInstruction Dvr = new Dvr();
	public static readonly BaseInstruction Dvf = new Dvf();
	public static readonly BaseInstruction Rmf = new Rmf();
	public static readonly BaseInstruction Fml = new Fml();
	public static readonly BaseInstruction Fdv = new Fdv();
	public static readonly BaseInstruction Frd = new Frd();
	public static readonly BaseInstruction Mvf = new Mvf();
	public static readonly BaseInstruction Mnf = new Mnf();
	public static readonly BaseInstruction Abs = new Abs();
	public static readonly BaseInstruction Rnd = new Rnd();
	public static readonly BaseInstruction Sqt = new Sqt();
	public static readonly BaseInstruction Flt = new Flt();
	public static readonly BaseInstruction Fix = new Fix();
	public static readonly BaseInstruction Wfs = new Wfs();
	public static readonly BaseInstruction Rfs = new Rfs();
	public static readonly BaseInstruction Wfc = new Wfc();
	public static readonly BaseInstruction Rfc = new Rfc();
	public static readonly BaseInstruction Cmf = new Cmf();
	public static readonly BaseInstruction Cnf = new Cnf();
	public static readonly BaseInstruction Cmfe = new Cmfe();
	public static readonly BaseInstruction Cnfe = new Cnfe();
	public static readonly BaseInstruction Ldf = new Ldf();
	public static readonly BaseInstruction Stf = new Stf();
	public static readonly BaseInstruction Fmov = new Fmov();
}
