// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 Bts instruction.
	/// </summary>
	public sealed class Bts : TwoOperandInstruction
	{
		#region Data Members

		private static readonly OpCode RM_C = new OpCode(new byte[] { 0x0F, 0xAB }, 5);
		private static readonly OpCode RM_R = new OpCode(new byte[] { 0x0F, 0xBA });

		#endregion Data Members

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
			if (third.IsConstant)
				return RM_C;
			else
				return RM_R;
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Or(context);
		}

		#endregion Methods
	}
}