// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using System;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 movaps instruction.
	/// </summary>
	public sealed class MovAPS : X86Instruction
	{
		#region Data Members

		private static readonly OpCode R_RM = new OpCode(new byte[] { 0x0F, 0x28 });
		private static readonly OpCode RM_R = new OpCode(new byte[] { 0x0F, 0x29 });

		#endregion Data Members

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="MovAPS"/>.
		/// </summary>
		public MovAPS() :
			base(1, 1)
		{
		}

		#endregion Construction

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
			if (destination.IsRegister) return R_RM;
			if (destination.IsMemoryAddress) return RM_R;

			throw new ArgumentException(@"No opcode for operand type. [" + destination.GetType() + ", " + source.GetType() + ")");
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.MovAPS(context);
		}

		#endregion Methods
	}
}