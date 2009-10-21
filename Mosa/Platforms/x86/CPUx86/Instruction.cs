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
		public static readonly JumpInstruction JumpInstruction = new JumpInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static readonly MovsxInstruction MovsxInstruction = new MovsxInstruction();
	}
}

