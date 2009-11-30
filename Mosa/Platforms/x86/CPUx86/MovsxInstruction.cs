/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.CompilerFramework.Operands;
using Mosa.Runtime.Metadata;

namespace Mosa.Platforms.x86.CPUx86
{
	/// <summary>
	/// Representations the x86 Movsx instruction.
	/// </summary>
	public sealed class MovsxInstruction : TwoOperandInstruction
	{
		#region Data Members

		private static readonly OpCode R_X8 = new OpCode(new byte[] { 0x0F, 0xBE });
		private static readonly OpCode R_X16 = new OpCode(new byte[] { 0x0F, 0xBF });

		#endregion

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
			if (!(destination is RegisterOperand))
				throw new ArgumentException(@"Destination must be RegisterOperand.", @"destination");
			if (source is ConstantOperand)
				throw new ArgumentException(@"Source must not be ConstantOperand.", @"source");

			switch (source.Type.Type) {
				case CilElementType.U1: goto case CilElementType.I1;
				case CilElementType.I1: {
						if ((destination is RegisterOperand) && (source is RegisterOperand)) return R_X8;
						if ((destination is RegisterOperand) && (source is MemoryOperand)) return R_X8;
					}
					break;
                case CilElementType.Char: goto case CilElementType.U2;
				case CilElementType.U2: goto case CilElementType.I2;
				case CilElementType.I2:
					if ((destination is RegisterOperand) && (source is RegisterOperand)) return R_X16;
					if ((destination is RegisterOperand) && (source is MemoryOperand)) return R_X16;
					break;
				default:
					//Emit(destination, source, X86.Move(destination, source));
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
			visitor.Movsx(context);
		}

		#endregion
	}
}
