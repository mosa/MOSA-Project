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
	/// Representations the x86 adc instruction.
	/// </summary>
	public sealed class Adc : TwoOperandInstruction
	{
		#region Data members

		private static readonly OpCode RM_C = new OpCode(new byte[] { 0x81 }, 2);
		private static readonly OpCode R_RM = new OpCode(new byte[] { 0x13 });
		private static readonly OpCode M_R = new OpCode(new byte[] { 0x11 });

		#endregion Data members

		#region Methods

		/// <summary>
		/// Computes the opcode.
		/// </summary>
		/// <param name="destination">The destination.</param>
		/// <param name="source">The source operand.</param>
		/// <param name="third">The third operand.</param>
		/// <returns></returns>
		protected override OpCode ComputeOpCode(Operand destination, Operand source, Operand third)
		{
			if (destination.IsRegister && third.IsConstant) return RM_C;
			if (destination.IsRegister && third.IsRegister) return R_RM;
			if (destination.IsRegister && third.IsMemoryAddress) return R_RM;
			if (destination.IsMemoryAddress && third.IsRegister) return M_R;
			if (destination.IsMemoryAddress && third.IsConstant) return RM_C;

			throw new ArgumentException(@"No opcode for operand type.");
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Adc(context);
		}

		#endregion Methods
	}
}