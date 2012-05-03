/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Pascal Delprat (pdelprat) <pascal.delprat@online.fr>
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Operands;
using System;

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
			if (context.Result is RegisterOperand && context.Operand1 is RegisterOperand && context.Operand2 is RegisterOperand)
			{
				RegisterOperand destination = context.Result as RegisterOperand;
				RegisterOperand firstSource = context.Operand1 as RegisterOperand;
				RegisterOperand secondSource = context.Operand2 as RegisterOperand;
				emitter.EmitThreeRegistersUnshifted(0x24, (byte)firstSource.Register.RegisterCode, (byte)secondSource.Register.RegisterCode, (byte)destination.Register.RegisterCode);
			}
			else
				if (context.Result is RegisterOperand && context.Operand1 is RegisterOperand && context.Operand2 is ConstantOperand)
				{
					RegisterOperand destination = context.Result as RegisterOperand;
					RegisterOperand source = context.Operand1 as RegisterOperand;
					ConstantOperand immediate = context.Operand2 as ConstantOperand;
					int value = 0;
					if (IsConstantBetween(immediate, -128, 127, out value))
					{
						emitter.EmitTwoRegisterOperandsWithK8Immediate(0x00, (byte)source.Register.RegisterCode, (byte)destination.Register.RegisterCode, (sbyte)value);
					}
					else
						throw new OverflowException();
				}
				else
					if (context.Result is RegisterOperand && context.Operand1 is RegisterOperand)
					{
						RegisterOperand destination = context.Result as RegisterOperand;
						RegisterOperand source = context.Operand1 as RegisterOperand;
						emitter.EmitTwoRegisterInstructions(0x13, (byte)source.Register.RegisterCode, (byte)destination.Register.RegisterCode);
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
