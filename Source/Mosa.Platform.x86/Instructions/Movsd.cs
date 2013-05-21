﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 movsd instruction.
	/// </summary>
	public sealed class Movsd : TwoOperandInstruction
	{
		#region Data Members

		private static readonly OpCode R_M = new OpCode(new byte[] { 0xF2, 0x0F, 0x10 });
		private static readonly OpCode R_R = new OpCode(new byte[] { 0xF2, 0x0F, 0x10 });
		private static readonly OpCode M_R = new OpCode(new byte[] { 0xF2, 0x0F, 0x11 });

		#endregion Data Members

		#region Methods

		/// <summary>
		/// Gets a value indicating whether [result is input].
		/// </summary>
		/// <value>
		///   <c>true</c> if [result is input]; otherwise, <c>false</c>.
		/// </value>
		public override bool ResultIsInput { get { return false; } }

		/// <summary>
		/// Computes the opcode.
		/// </summary>
		/// <param name="destination">The destination operand.</param>
		/// <param name="source">The source operand.</param>
		/// <param name="third">The third operand.</param>
		/// <returns></returns>
		protected override OpCode ComputeOpCode(Operand destination, Operand source, Operand third)
		{
			if ((destination.IsRegister) && (source.IsMemoryAddress)) return R_M;
			if ((destination.IsRegister) && (source.IsRegister)) return R_R;
			if ((destination.IsMemoryAddress) && (source.IsRegister)) return M_R;

			throw new ArgumentException(@"No opcode for operand type. [" + destination.GetType() + ", " + source.GetType() + ")");
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Movsd(context);
		}

		#endregion Methods
	}
}