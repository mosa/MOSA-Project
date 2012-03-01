/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Platform.AVR32.OpCodes;

namespace Mosa.Platform.AVR32
{
	/// <summary>
	/// 
	/// </summary>
	public static class Instruction
	{
		/// <summary>
		/// 
		/// </summary>
		public static readonly AdcInstruction AdcInstruction = new AdcInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly AddInstruction AddInstruction = new AddInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly AdiwInstruction AdiwInstruction = new AdiwInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly AndInstruction AndInstruction = new AndInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly AndiInstruction AndiInstruction = new AndiInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly AsrInstruction AsrInstruction = new AsrInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly BclrInstruction BclrInstruction = new BclrInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly BldInstruction BldInstruction = new BldInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly BrbcInstruction BrbcInstruction = new BrbcInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly BrbsInstruction BrbsInstruction = new BrbsInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly BrccInstruction BrccInstruction = new BrccInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly BrcsInstruction BrcsInstruction = new BrcsInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly BreqInstruction BreqInstruction = new BreqInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly BrgeInstruction BrgeInstruction = new BrgeInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly BrhcInstruction BrhcInstruction = new BrhcInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly BrhsInstruction BrhsInstruction = new BrhsInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly BridInstruction BridInstruction = new BridInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly BrieInstruction BrieInstruction = new BrieInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly BrloInstruction BrloInstruction = new BrloInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly BrltInstruction BrltInstruction = new BrltInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly BrmiInstruction BrmiInstruction = new BrmiInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly BrneInstruction BrneInstruction = new BrneInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly BrplInstruction BrplInstruction = new BrplInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly BrshInstruction BrshInstruction = new BrshInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly BrtcInstruction BrtcInstruction = new BrtcInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly BrtsInstruction BrtsInstruction = new BrtsInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly BrvcInstruction BrvcInstruction = new BrvcInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly BrvsInstruction BrvsInstruction = new BrvsInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly BsetInstruction BsetInstruction = new BsetInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly BstInstruction BstInstruction = new BstInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly CallInstruction CallInstruction = new CallInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly CbiInstruction CbiInstruction = new CbiInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly CbrInstruction CbrInstruction = new CbrInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly ClcInstruction ClcInstruction = new ClcInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly ClhInstruction ClhInstruction = new ClhInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly CliInstruction CliInstruction = new CliInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly ClnInstruction ClnInstruction = new ClnInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly ClrInstruction ClrInstruction = new ClrInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly ClsInstruction ClsInstruction = new ClsInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly CltInstruction CltInstruction = new CltInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly ClvInstruction ClvInstruction = new ClvInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly ClzInstruction ClzInstruction = new ClzInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly ComInstruction ComInstruction = new ComInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly CpInstruction CpInstruction = new CpInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly CpcInstruction CpcInstruction = new CpcInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly CpiInstruction CpiInstruction = new CpiInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly CpseInstruction CpseInstruction = new CpseInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly DecInstruction DecInstruction = new DecInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly EorInstruction EorInstruction = new EorInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly IcallInstruction IcallInstruction = new IcallInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly IjmpInstruction IjmpInstruction = new IjmpInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly InInstruction InInstruction = new InInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly IncInstruction IncInstruction = new IncInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly JmpInstruction JmpInstruction = new JmpInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly LdInstruction LdInstruction = new LdInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly LddInstruction LddInstruction = new LddInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly LdiInstruction LdiInstruction = new LdiInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly LdsInstruction LdsInstruction = new LdsInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly LpmInstruction LpmInstruction = new LpmInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly LslInstruction LslInstruction = new LslInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly LsrInstruction LsrInstruction = new LsrInstruction();

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
		public static readonly NopInstruction NopInstruction = new NopInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly OrInstruction OrInstruction = new OrInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly OriInstruction OriInstruction = new OriInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly OutInstruction OutInstruction = new OutInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly PopInstruction PopInstruction = new PopInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly PushInstruction PushInstruction = new PushInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly RcallInstruction RcallInstruction = new RcallInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly RetInstruction RetInstruction = new RetInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly RetiInstruction RetiInstruction = new RetiInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly RjmpInstruction RjmpInstruction = new RjmpInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly RolInstruction RolInstruction = new RolInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly RorInstruction RorInstruction = new RorInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly SbcInstruction SbcInstruction = new SbcInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly SbciInstruction SbciInstruction = new SbciInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly SbiInstruction SbiInstruction = new SbiInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly SbicInstruction SbicInstruction = new SbicInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly SbisInstruction SbisInstruction = new SbisInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly SbiwInstruction SbiwInstruction = new SbiwInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly SbrInstruction SbrInstruction = new SbrInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly SbrcInstruction SbrcInstruction = new SbrcInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly SbrsInstruction SbrsInstruction = new SbrsInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly SecInstruction SecInstruction = new SecInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly SehInstruction SehInstruction = new SehInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly SeiInstruction SeiInstruction = new SeiInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly SenInstruction SenInstruction = new SenInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly SerInstruction SerInstruction = new SerInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly SesInstruction SesInstruction = new SesInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly SetInstruction SetInstruction = new SetInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly SevInstruction SevInstruction = new SevInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly SezInstruction SezInstruction = new SezInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly SleepInstruction SleepInstruction = new SleepInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly StInstruction StInstruction = new StInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly StdInstruction StdInstruction = new StdInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly StsInstruction StsInstruction = new StsInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly SubInstruction SubInstruction = new SubInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly SubiInstruction SubiInstruction = new SubiInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly SwapInstruction SwapInstruction = new SwapInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly TstInstruction TstInstruction = new TstInstruction();

		/// <summary>
		/// 
		/// </summary>
		public static readonly WdrInstruction WdrInstruction = new WdrInstruction();

	}
}

