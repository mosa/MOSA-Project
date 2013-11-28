﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework;
using System;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 or instruction.
	/// </summary>
	public sealed class Or : TwoOperandInstruction
	{
		#region Data Members

		private static readonly OpCode R_C = new OpCode(new byte[] { 0x81 }, 1);
		private static readonly OpCode M_C = new OpCode(new byte[] { 0x81 }, 1);
		private static readonly OpCode R_M = new OpCode(new byte[] { 0x0B });
		private static readonly OpCode R_R = new OpCode(new byte[] { 0x0B });
		private static readonly OpCode M_R = new OpCode(new byte[] { 0x09 });

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
			if (destination.IsRegister && third.IsConstant) return R_C;
			if (destination.IsRegister && third.IsMemoryAddress) return R_M;
			if (destination.IsRegister && third.IsRegister) return R_R;
			if (destination.IsMemoryAddress && third.IsRegister) return M_R;
			if (destination.IsMemoryAddress && third.IsConstant) return M_C;

			throw new ArgumentException(@"No opcode for operand type.");
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