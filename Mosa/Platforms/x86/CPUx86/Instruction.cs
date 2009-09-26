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
		public static DirectMultiplicationInstruction DirectMultiplicationInstruction = new DirectMultiplicationInstruction();
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
		public static AddInstruction AdcInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction CdqInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction CmpInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction ComisdInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction ComissInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction Cvtsd2ssInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction Cvtsi2sdInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction Cvtsi2ssInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction Cvtss2sdInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction DecInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction DirectDivision = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction DirectMultiplication = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction DivInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction EpilogueInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction IncInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction IntInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction JnsBranchInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction LiteralInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction LogicalAndInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction LogicalNotInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction LogicalOrInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction LogicalXorInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction MoveInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction MulInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction NegInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction PrologueInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction SalInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction SarInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction SbbInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction SetccInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction ShldInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction ShlInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction ShrdInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction ShrInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction SseAddInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction SseDivInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction SseMulInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction SseSubInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction SubInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction UcomisdInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction UcomissInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction UDivInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction BochsDebug = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction CldInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction CliInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction CmpXchgInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction CpuIdEaxInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction CpuIdEbxInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction CpuIdEcxInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction CpuIdEdxInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction CpuIdInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction HltInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction InInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction InvlpgInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction IretdInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction LgdtInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction LidtInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction LockInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction OutInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction PauseInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction PopadInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction PopfdInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction PopInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction PushadInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction PushfdInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction PushInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction RcrInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction RdmsrInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction RdpmcInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction RdtscInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction RepInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction StiInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction StosbInstrucions = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction StosdInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction XchgInstruction = new AddInstruction();
		/// <summary>
		/// 
		/// </summary>
		public static AddInstruction DirectDivisionInstruction = new AddInstruction();

	}
}

