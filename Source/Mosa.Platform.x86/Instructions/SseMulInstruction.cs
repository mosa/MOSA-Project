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
	/// Intermediate representation of the SSE multiplication operation.
	/// </summary>
	public sealed class SseMulInstruction : TwoOperandInstruction
	{
		#region Data Members

		private static readonly OpCode F = new OpCode(new byte[] { 0xF3, 0x0F, 0x59 });
		private static readonly OpCode I = new OpCode(new byte[] { 0xF2, 0x0F, 0x59 });

		#endregion // Data Members

		#region Methods
		/// <summary>
		/// Computes the opcode.
		/// </summary>
		/// <param name="destination">The destination operand.</param>
		/// <param name="source">The source operand.</param>
		/// <param name="third">The third operand.</param>
		/// <returns></returns>
		protected override OpCode ComputeOpCode(Operand destination, Operand source, Operand third)
		{
			if (source.Type.Type == CilElementType.R4)
				return F;

			return I;
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.SseMul(context);
		}

		#endregion // Methods
	}
}
