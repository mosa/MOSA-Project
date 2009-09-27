/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Mosa.Platforms.x86.CPUx86
{
	/// <summary>
	/// 
	/// </summary>
	public static class Instruction
	{
		/// <summary>
		/// 
		/// </summary>
		public static NopInstruction NopInstruction = new NopInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction AddInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AdcInstruction AdcInstruction = new AdcInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static CdqInstruction CdqInstruction = new CdqInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static CmpInstruction CmpInstruction = new CmpInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static ComisdInstruction ComisdInstruction = new ComisdInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static ComissInstruction ComissInstruction = new ComissInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static Cvtsd2ssInstruction Cvtsd2ssInstruction = new Cvtsd2ssInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static Cvtsi2sdInstruction Cvtsi2sdInstruction = new Cvtsi2sdInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static Cvtsi2ssInstruction Cvtsi2ssInstruction = new Cvtsi2ssInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static Cvtss2sdInstruction Cvtss2sdInstruction = new Cvtss2sdInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static DecInstruction DecInstruction = new DecInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static DirectDivisionInstruction DirectDivisionInstruction = new DirectDivisionInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static DirectMultiplicationInstruction DirectMultiplicationInstruction = new DirectMultiplicationInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static DivInstruction DivInstruction = new DivInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static EpilogueInstruction EpilogueInstruction = new EpilogueInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static IncInstruction IncInstruction = new IncInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static IntInstruction IntInstruction = new IntInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static JnsBranchInstruction JnsBranchInstruction = new JnsBranchInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static LiteralInstruction LiteralInstruction = new LiteralInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static LogicalAndInstruction LogicalAndInstruction = new LogicalAndInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static LogicalNotInstruction LogicalNotInstruction = new LogicalNotInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static LogicalOrInstruction LogicalOrInstruction = new LogicalOrInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static LogicalXorInstruction LogicalXorInstruction = new LogicalXorInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static MoveInstruction MoveInstruction = new MoveInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static MulInstruction MulInstruction = new MulInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static NegInstruction NegInstruction = new NegInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static PrologueInstruction PrologueInstruction = new PrologueInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static SalInstruction SalInstruction = new SalInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static SarInstruction SarInstruction = new SarInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static SbbInstruction SbbInstruction = new SbbInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static SetccInstruction SetccInstruction = new SetccInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static ShldInstruction ShldInstruction = new ShldInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static ShlInstruction ShlInstruction = new ShlInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static ShrdInstruction ShrdInstruction = new ShrdInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static ShrInstruction ShrInstruction = new ShrInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static SseAddInstruction SseAddInstruction = new SseAddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static SseDivInstruction SseDivInstruction = new SseDivInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static SseMulInstruction SseMulInstruction = new SseMulInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static SseSubInstruction SseSubInstruction = new SseSubInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static SubInstruction SubInstruction = new SubInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static UcomisdInstruction UcomisdInstruction = new UcomisdInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static UcomissInstruction UcomissInstruction = new UcomissInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static Intrinsics.BochsDebug BochsDebug = new Intrinsics.BochsDebug();
		/// <summary>
		/// 
		/// </summary>
		public static Intrinsics.CldInstruction CldInstruction = new Intrinsics.CldInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static Intrinsics.CliInstruction CliInstruction = new Intrinsics.CliInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static Intrinsics.CmpXchgInstruction CmpXchgInstruction = new Intrinsics.CmpXchgInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static Intrinsics.CpuIdEaxInstruction CpuIdEaxInstruction = new Intrinsics.CpuIdEaxInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static Intrinsics.CpuIdEbxInstruction CpuIdEbxInstruction = new Intrinsics.CpuIdEbxInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static Intrinsics.CpuIdEcxInstruction CpuIdEcxInstruction = new Intrinsics.CpuIdEcxInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static Intrinsics.CpuIdEdxInstruction CpuIdEdxInstruction = new Intrinsics.CpuIdEdxInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static Intrinsics.CpuIdInstruction CpuIdInstruction = new Intrinsics.CpuIdInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static Intrinsics.HltInstruction HltInstruction = new Intrinsics.HltInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static Intrinsics.InInstruction InInstruction = new Intrinsics.InInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static Intrinsics.InvlpgInstruction InvlpgInstruction = new Intrinsics.InvlpgInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static Intrinsics.IretdInstruction IretdInstruction = new Intrinsics.IretdInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static Intrinsics.LgdtInstruction LgdtInstruction = new Intrinsics.LgdtInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static Intrinsics.LidtInstruction LidtInstruction = new Intrinsics.LidtInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static Intrinsics.OutInstruction OutInstruction = new Intrinsics.OutInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static Intrinsics.PauseInstruction PauseInstruction = new Intrinsics.PauseInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static Intrinsics.PopadInstruction PopadInstruction = new Intrinsics.PopadInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static Intrinsics.PopfdInstruction PopfdInstruction = new Intrinsics.PopfdInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static Intrinsics.PopInstruction PopInstruction = new Intrinsics.PopInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static Intrinsics.PushadInstruction PushadInstruction = new Intrinsics.PushadInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static Intrinsics.PushfdInstruction PushfdInstruction = new Intrinsics.PushfdInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static Intrinsics.PushInstruction PushInstruction = new Intrinsics.PushInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static RcrInstruction RcrInstruction = new RcrInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static Intrinsics.RdmsrInstruction RdmsrInstruction = new Intrinsics.RdmsrInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static Intrinsics.RdpmcInstruction RdpmcInstruction = new Intrinsics.RdpmcInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static Intrinsics.RdtscInstruction RdtscInstruction = new Intrinsics.RdtscInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static Intrinsics.RepInstruction RepInstruction = new Intrinsics.RepInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static Intrinsics.StiInstruction StiInstruction = new Intrinsics.StiInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static Intrinsics.StosbInstruction StosbInstrucion = new Intrinsics.StosbInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static Intrinsics.StosdInstruction StosdInstruction = new Intrinsics.StosdInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static Intrinsics.XchgInstruction XchgInstruction = new Intrinsics.XchgInstruction();
	}
}

