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
			if (context.Result.IsRegister && context.Operand1.IsRegister)
			{
				emitter.EmitTwoRegisterInstructions(0x03, (byte)context.Operand1.Register.RegisterCode, (byte)context.Result.Register.RegisterCode); // cp.w Rd, Rs
			}
			else if (context.Result.IsRegister && context.Operand1.IsConstant)
			{
				int value = 0;

				if (IsConstantBetween(context.Operand1, -32, 31, out value))
				{
					emitter.EmitK6immediateAndSingleRegister((sbyte)value, (byte)context.Result.Register.RegisterCode); // cp.w Rd, imm 6 bits
				}
				else if (IsConstantBetween(context.Operand1, -1048576, 1048575, out value))
				{
					emitter.EmitRegisterOrConditionCodeAndK21((byte)0x02, (byte)context.Result.Register.RegisterCode, value); // cp.w Rd, imm 21 bits
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
