/*
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
	/// Representations the x86 movss instruction.
	/// </summary>
	public sealed class Movss : TwoOperandInstruction
	{

		#region Data Members

		private static readonly OpCode R = new OpCode(new byte[] { 0xF3, 0x0F, 0x10 });
		private static readonly OpCode M_R = new OpCode(new byte[] { 0xF3, 0x0F, 0x11 });

		#endregion

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
			if ((destination.IsMemoryAddress) && (source.IsRegister)) return M_R;
			if ((destination.IsRegister) && (source.IsMemoryAddress)) return R;
			if ((destination.IsRegister) && (source.IsRegister)) return R;

			throw new ArgumentException(@"No opcode for operand type. [" + destination.GetType() + ", " + source.GetType() + ")");
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Movss(context);
		}

		#endregion
	}
}
