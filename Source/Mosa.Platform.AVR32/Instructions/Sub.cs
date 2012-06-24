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
	/// Sub Instruction
	/// Supported Format:
	///     sub Rd, Rs
	///     sub Rd, imm (8 bits)
	///     sub Rd, imm (21 bits)
	///     sub Rd, Rs, imm (16 bits)
	/// </summary>
	public class Sub : AVR32Instruction
	{
		#region Methods

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(Context context, MachineCodeEmitter emitter)
		{
			if (context.Result.IsRegister && context.Operand1.IsConstant)
			{
				int value = 0;

				if (context.Result.Register.RegisterCode == GeneralPurposeRegister.SP.Index)
				{
					if (IsConstantBetween(context.Operand1, -512, 508, out value))
					{
						emitter.EmitK8immediateAndSingleRegister(0x00, (sbyte)(value >> 2), (byte)context.Result.Register.RegisterCode); // sub Sp, Imm (k8)
					}
					else if (IsConstantBetween(context.Operand1, -1048576, 1048575, out value))
					{
						emitter.EmitRegisterOrConditionCodeAndK21(0x01, (byte)context.Result.Register.RegisterCode, value); // sub Sp, Imm (k21)
					}
					else
						throw new OverflowException();
				}
				else
				{
					if (IsConstantBetween(context.Operand1, -128, 127, out value))
					{
						emitter.EmitK8immediateAndSingleRegister(0x00, (sbyte)value, (byte)context.Result.Register.RegisterCode); // sub Rd, Imm (k8)
					}
					else if (IsConstantBetween(context.Operand1, -1048576, 1048575, out value))
					{
						emitter.EmitRegisterOrConditionCodeAndK21(0x01, (byte)context.Result.Register.RegisterCode, value); // sub Rd, Imm (k21)
					}
					else
						throw new OverflowException();
				}
			}
			else if (context.Result.IsRegister && context.Operand1.IsRegister && context.Operand2.IsConstant)
			{
				int value = 0;

				if (IsConstantBetween(context.Operand2, -32768, 32767, out value))
				{
					emitter.EmitTwoRegistersAndK16(0x0C, (byte)context.Operand1.Register.RegisterCode, (byte)context.Result.Register.RegisterCode, (short)value); // sub Rd, Rs, Imm (k16)
				}
				else
					throw new OverflowException();
			}
			else if ((context.Result.IsRegister) && (context.Operand1.IsRegister))
			{
				emitter.EmitTwoRegisterInstructions(0x01, (byte)context.Result.Register.RegisterCode, (byte)context.Operand1.Register.RegisterCode); // sub Rd, Rs
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
			visitor.Sub(context);
		}

		#endregion // Methods

	}
}
