/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Pascal Delprat (pdelprat) <pascal.delprat@online.fr>
 */

using System;
using Mosa.Compiler.Framework;

namespace Mosa.Platform.AVR32.Instructions
{
	/// <summary>
	/// Mul instruction
	/// Supported format:
	///     mul Rd, Rs
	///     mul Rd, Rx, Ry
	///     mul Rd, Rs, imm 8 bits
	/// </summary>
	public class Mul : AVR32Instruction
	{

		#region Methods

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(Context context, MachineCodeEmitter emitter)
		{
			if (context.Result.IsRegister && context.Operand1.IsRegister && context.Operand2.IsRegister)
			{
				emitter.EmitThreeRegistersUnshifted(0x24, (byte)context.Operand1.Register.RegisterCode, (byte)context.Operand2.Register.RegisterCode, (byte)context.Result.Register.RegisterCode);
			}
			else if (context.Result.IsRegister && context.Operand1.IsRegister && context.Operand2.IsConstant)
			{
				int value = 0;
				if (IsConstantBetween(context.Operand2, -128, 127, out value))
				{
					emitter.EmitTwoRegisterOperandsWithK8Immediate(0x00, (byte)context.Operand1.Register.RegisterCode, (byte)context.Result.Register.RegisterCode, (sbyte)value);
				}
				else
					throw new OverflowException();
			}
			else if (context.Result.IsRegister && context.Operand1.IsRegister)
			{
				emitter.EmitTwoRegisterInstructions(0x13, (byte)context.Operand1.Register.RegisterCode, (byte)context.Result.Register.RegisterCode);
			}
			else
			{
				throw new Exception("Not supported combination of operands");
			}
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IAVR32Visitor visitor, Context context)
		{
			visitor.Mul(context);
		}

		#endregion // Methods

	}
}
