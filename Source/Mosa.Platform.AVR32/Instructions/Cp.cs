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
using Mosa.Compiler.Framework.Operands;

namespace Mosa.Platform.AVR32.Instructions
{
	/// <summary>
	/// Cp Instruction
	/// Supported Format :
	///     cp.w Rd, Rs
	///     cp.w Rd, imm 6 bits
	///     cp.w Rd, imm 21 bits
	/// </summary>
	public class Cp : AVR32Instruction
	{

		#region Methods

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(Context context, MachineCodeEmitter emitter)
		{
			if (context.Result is DefinedRegisterOperand && context.Operand1 is DefinedRegisterOperand)
			{
				DefinedRegisterOperand destination = context.Result as DefinedRegisterOperand;
				DefinedRegisterOperand source = context.Operand1 as DefinedRegisterOperand;

				emitter.EmitTwoRegisterInstructions(0x03, (byte)source.Register.RegisterCode, (byte)destination.Register.RegisterCode); // cp.w Rd, Rs
			}
			else
				if (context.Result is DefinedRegisterOperand && context.Operand1 is ConstantOperand)
				{
					DefinedRegisterOperand destination = context.Result as DefinedRegisterOperand;
					ConstantOperand immediate = context.Operand1 as ConstantOperand;

					int value = 0;

					if (IsConstantBetween(immediate, -32, 31, out value))
					{
						emitter.EmitK6immediateAndSingleRegister((sbyte)value, (byte)destination.Register.RegisterCode); // cp.w Rd, imm 6 bits
					}
					else
						if (IsConstantBetween(immediate, -1048576, 1048575, out value))
						{
							emitter.EmitRegisterOrConditionCodeAndK21((byte)0x02, (byte)destination.Register.RegisterCode, value); // cp.w Rd, imm 21 bits
						}
						else
							throw new OverflowException();

				}
				else
					throw new Exception("Not supported combination of operands");
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IAVR32Visitor visitor, Context context)
		{
			visitor.Cp(context);
		}

		#endregion // Methods

	}
}
