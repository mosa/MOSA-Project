using System;
using System.Collections.Generic;
using System.Text;
using Mosa.Runtime.CompilerFramework.Operands;
using Mosa.Runtime.CompilerFramework;

namespace Mosa.Platform.X86.CPUx86
{
	/// <summary>
	/// 
	/// </summary>
	public class SseRoundInstruction : BaseInstruction
	{
		// ROUNDSS
		private static readonly OpCode R4 = new OpCode(new byte[] { 0x66, 0x0F, 0x3A, 0x0A });

		// ROUNDSD
		private static readonly OpCode R8 = new OpCode(new byte[] { 0x66, 0x0F, 0x3A, 0x0B });

		/// <summary>
		/// Gets the instruction latency.
		/// </summary>
		/// <value>The latency.</value>
		public override int Latency { get { return 3; } }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="destination"></param>
		/// <param name="source"></param>
		/// <param name="third"></param>
		/// <returns></returns>
		protected override OpCode ComputeOpCode(Operand destination, Operand source, Operand third)
		{
			if (source.Type.Type == Mosa.Runtime.Metadata.CilElementType.R4)
				return R4;

			return R8;
		}
		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.SseRound(context);
		}
	}
}
