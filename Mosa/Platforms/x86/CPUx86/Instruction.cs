/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

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
        public static readonly NopInstruction NopInstruction = new NopInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly AddInstruction AddInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly AdcInstruction AdcInstruction = new AdcInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly CdqInstruction CdqInstruction = new CdqInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly CmpInstruction CmpInstruction = new CmpInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly ComisdInstruction ComisdInstruction = new ComisdInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly ComissInstruction ComissInstruction = new ComissInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly Cvtsd2ssInstruction Cvtsd2ssInstruction = new Cvtsd2ssInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly Cvtsi2sdInstruction Cvtsi2sdInstruction = new Cvtsi2sdInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly Cvtsi2ssInstruction Cvtsi2ssInstruction = new Cvtsi2ssInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly Cvtss2sdInstruction Cvtss2sdInstruction = new Cvtss2sdInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly DecInstruction DecInstruction = new DecInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly DirectDivisionInstruction DirectDivisionInstruction = new DirectDivisionInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly DirectMultiplicationInstruction DirectMultiplicationInstruction = new DirectMultiplicationInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly DivInstruction DivInstruction = new DivInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly IncInstruction IncInstruction = new IncInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly IntInstruction IntInstruction = new IntInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly JnsBranchInstruction JnsBranchInstruction = new JnsBranchInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly LiteralInstruction LiteralInstruction = new LiteralInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly LogicalAndInstruction LogicalAndInstruction = new LogicalAndInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly LogicalNotInstruction LogicalNotInstruction = new LogicalNotInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly LogicalOrInstruction LogicalOrInstruction = new LogicalOrInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly LogicalXorInstruction LogicalXorInstruction = new LogicalXorInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly MoveInstruction MoveInstruction = new MoveInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly MulInstruction MulInstruction = new MulInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly NegInstruction NegInstruction = new NegInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly SalInstruction SalInstruction = new SalInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly SarInstruction SarInstruction = new SarInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly SbbInstruction SbbInstruction = new SbbInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly SetccInstruction SetccInstruction = new SetccInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly ShldInstruction ShldInstruction = new ShldInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly ShlInstruction ShlInstruction = new ShlInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly ShrdInstruction ShrdInstruction = new ShrdInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly ShrInstruction ShrInstruction = new ShrInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly SseAddInstruction SseAddInstruction = new SseAddInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly SseDivInstruction SseDivInstruction = new SseDivInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly SseMulInstruction SseMulInstruction = new SseMulInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly SseSubInstruction SseSubInstruction = new SseSubInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly SubInstruction SubInstruction = new SubInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly UcomisdInstruction UcomisdInstruction = new UcomisdInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly UcomissInstruction UcomissInstruction = new UcomissInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly Intrinsics.BochsDebug BochsDebug = new Intrinsics.BochsDebug();
		/// <summary>
		/// 
		/// </summary>
        public static readonly Intrinsics.CldInstruction CldInstruction = new Intrinsics.CldInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly Intrinsics.CliInstruction CliInstruction = new Intrinsics.CliInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly Intrinsics.CmpXchgInstruction CmpXchgInstruction = new Intrinsics.CmpXchgInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly Intrinsics.CpuIdEaxInstruction CpuIdEaxInstruction = new Intrinsics.CpuIdEaxInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly Intrinsics.CpuIdEbxInstruction CpuIdEbxInstruction = new Intrinsics.CpuIdEbxInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly Intrinsics.CpuIdEcxInstruction CpuIdEcxInstruction = new Intrinsics.CpuIdEcxInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly Intrinsics.CpuIdEdxInstruction CpuIdEdxInstruction = new Intrinsics.CpuIdEdxInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly Intrinsics.CpuIdInstruction CpuIdInstruction = new Intrinsics.CpuIdInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly Intrinsics.HltInstruction HltInstruction = new Intrinsics.HltInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly Intrinsics.InInstruction InInstruction = new Intrinsics.InInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly Intrinsics.InvlpgInstruction InvlpgInstruction = new Intrinsics.InvlpgInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly Intrinsics.IretdInstruction IretdInstruction = new Intrinsics.IretdInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly Intrinsics.LgdtInstruction LgdtInstruction = new Intrinsics.LgdtInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly Intrinsics.LidtInstruction LidtInstruction = new Intrinsics.LidtInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly Intrinsics.OutInstruction OutInstruction = new Intrinsics.OutInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly Intrinsics.PauseInstruction PauseInstruction = new Intrinsics.PauseInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly Intrinsics.PopadInstruction PopadInstruction = new Intrinsics.PopadInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly Intrinsics.PopfdInstruction PopfdInstruction = new Intrinsics.PopfdInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly Intrinsics.PopInstruction PopInstruction = new Intrinsics.PopInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly Intrinsics.PushadInstruction PushadInstruction = new Intrinsics.PushadInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly Intrinsics.PushfdInstruction PushfdInstruction = new Intrinsics.PushfdInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly Intrinsics.PushInstruction PushInstruction = new Intrinsics.PushInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly Intrinsics.RcrInstruction RcrInstruction = new Intrinsics.RcrInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly Intrinsics.RdmsrInstruction RdmsrInstruction = new Intrinsics.RdmsrInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly Intrinsics.RdpmcInstruction RdpmcInstruction = new Intrinsics.RdpmcInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly Intrinsics.RdtscInstruction RdtscInstruction = new Intrinsics.RdtscInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly Intrinsics.RepInstruction RepInstruction = new Intrinsics.RepInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly Intrinsics.StiInstruction StiInstruction = new Intrinsics.StiInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly Intrinsics.StosbInstruction StosbInstrucion = new Intrinsics.StosbInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly Intrinsics.StosdInstruction StosdInstruction = new Intrinsics.StosdInstruction();
		/// <summary>
		/// 
		/// </summary>
        public static readonly Intrinsics.XchgInstruction XchgInstruction = new Intrinsics.XchgInstruction();
	}
}

