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
using Mosa.Compiler.Metadata;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 Movzx instruction.
	/// </summary>
	public sealed class Movzx : TwoOperandInstruction
	{

		#region Data Members

		private static readonly OpCode R_X8 = new OpCode(new byte[] { 0x0F, 0xB6 });
		private static readonly OpCode R_X16 = new OpCode(new byte[] { 0x0F, 0xB7 });

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
			if (!(destination.IsRegister))
				throw new ArgumentException(@"Destination must be RegisterOperand.", @"destination");
			if (source.IsConstant)
				throw new ArgumentException(@"Source must not be ConstantOperand.", @"source");

			switch (source.Type.Type)
			{
				case CilElementType.U1: goto case CilElementType.I1;
				case CilElementType.I1:
					{
						if ((destination.IsRegister) && (source.IsRegister)) return R_X8;
						if ((destination.IsRegister) && (source.IsMemoryAddress)) return R_X8;
					}
					break;
				case CilElementType.Char: goto case CilElementType.U2;
				case CilElementType.U2: goto case CilElementType.I2;
				case CilElementType.I2:
					if ((destination.IsRegister) && (source.IsRegister)) return R_X16;
					if ((destination.IsRegister) && (source.IsMemoryAddress)) return R_X16;
					break;
				case CilElementType.Boolean: goto case CilElementType.I1;
				default:

					break;
			}

			throw new ArgumentException(@"No opcode for operand type. [" + destination.GetType() + ", " + source.GetType() + ")");
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Movzx(context);
		}

		#endregion
	}
}
