/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */


using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Operands;
using Mosa.Compiler.Metadata;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Intermediate representation of the SSE addition instruction.
	/// </summary>
	public sealed class SseAddInstruction : TwoOperandInstruction
	{
		#region Data Members

		private static readonly OpCode R4 = new OpCode(new byte[] { 0xF3, 0x0F, 0x58 });
		private static readonly OpCode R8 = new OpCode(new byte[] { 0xF2, 0x0F, 0x58 });

		#endregion // Data Members

		#region Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="destination"></param>
		/// <param name="source"></param>
		/// <param name="third"></param>
		/// <returns></returns>
		protected override OpCode ComputeOpCode(Operand destination, Operand source, Operand third)
		{
			if (source.Type.Type == CilElementType.R4)
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
			visitor.SseAdd(context);
		}

		#endregion

	}
}
