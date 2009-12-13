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
		/// 
		public static readonly BreakInstruction BreakInstruction = new BreakInstruction();
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
        public static readonly DirectCompareInstruction DirectCompareInstruction = new DirectCompareInstruction();
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
        public static readonly Cvttsd2siInstruction Cvttsd2siInstruction = new Cvttsd2siInstruction();
        /// <summary>
        /// 
        /// </summary>
        public static readonly Cvttss2siInstruction Cvttss2siInstruction = new Cvttss2siInstruction();
        /// <summary>
        /// 
        /// </summary>
        public static readonly DebugInstruction DebugInstruction = new DebugInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly DecInstruction DecInstruction = new DecInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly DivInstruction DivInstruction = new DivInstruction();
        /// <summary>
        /// 
        /// </summary>
        public static readonly DirectDivisionInstruction DirectDivisionInstruction = new DirectDivisionInstruction();
        /// <summary>
        /// 
        /// </summary>
        public static readonly UDivInstruction UDivInstruction = new UDivInstruction();
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
        public static readonly BranchInstruction BranchInstruction = new BranchInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly LiteralInstruction LiteralInstruction = new LiteralInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly AndInstruction AndInstruction = new AndInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly NotInstruction NotInstruction = new NotInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly OrInstruction OrInstruction = new OrInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly XorInstruction XorInstruction = new XorInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly MovInstruction MovInstruction = new MovInstruction();
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
		public static readonly JmpInstruction JmpInstruction = new JmpInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly MovsxInstruction MovsxInstruction = new MovsxInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly MovzxInstruction MovzxInstruction = new MovzxInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly MovssInstruction MovssInstruction = new MovssInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly MovsdInstruction MovsdInstruction = new MovsdInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly CallInstruction CallInstruction = new CallInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly RetInstruction RetInstruction = new RetInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly CldInstruction CldInstruction = new CldInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly CliInstruction CliInstruction = new CliInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly CmpXchgInstruction CmpXchgInstruction = new CmpXchgInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly CpuIdEaxInstruction CpuIdEaxInstruction = new CpuIdEaxInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly CpuIdEbxInstruction CpuIdEbxInstruction = new CpuIdEbxInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly CpuIdEcxInstruction CpuIdEcxInstruction = new CpuIdEcxInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly CpuIdEdxInstruction CpuIdEdxInstruction = new CpuIdEdxInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly CpuIdInstruction CpuIdInstruction = new CpuIdInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly HltInstruction HltInstruction = new HltInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly InInstruction InInstruction = new InInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly InvlpgInstruction InvlpgInstruction = new InvlpgInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly SetRC0Instruction SetRC0Instruction = new SetRC0Instruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly SetRC1Instruction SetRC1Instruction = new SetRC1Instruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly IretdInstruction IretdInstruction = new IretdInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly LgdtInstruction LgdtInstruction = new LgdtInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly LidtInstruction LidtInstruction = new LidtInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly OutInstruction OutInstruction = new OutInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly PauseInstruction PauseInstruction = new PauseInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly PopadInstruction PopadInstruction = new PopadInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly PopfdInstruction PopfdInstruction = new PopfdInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly PopInstruction PopInstruction = new PopInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly PushadInstruction PushadInstruction = new PushadInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly PushfdInstruction PushfdInstruction = new PushfdInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly PushInstruction PushInstruction = new PushInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly RcrInstruction RcrInstruction = new RcrInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly RdmsrInstruction RdmsrInstruction = new RdmsrInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly RdpmcInstruction RdpmcInstruction = new RdpmcInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly RdtscInstruction RdtscInstruction = new RdtscInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly RepInstruction RepInstruction = new RepInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly StiInstruction StiInstruction = new StiInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly StosbInstruction StosbInstrucion = new StosbInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly StosdInstruction StosdInstruction = new StosdInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly XchgInstruction XchgInstruction = new XchgInstruction();
        /// <summary>
        /// 
        /// </summary>
        public static readonly LeaInstruction LeaInstruction = new LeaInstruction();
		
	}
}

