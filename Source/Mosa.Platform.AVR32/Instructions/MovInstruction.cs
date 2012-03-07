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
using Mosa.Compiler.Metadata;
using System;
using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Platform.AVR32.Instructions
{
	/// <summary>
	/// Mov Instruction
	/// Supported Format:
	///     mov Rd, imm (8 bits)
	///     mov Rd, imm (21 bits)
	///     mov Rd, Rs
	/// </summary>
	public class MovInstruction : BaseInstruction
	{

		#region Methods

		/// <summary>
		/// Convert operand to int32
		/// </summary>
		/// <param name="op"></param>
		/// <returns></returns>
		protected int ComputeValue(ConstantOperand op)
		{
			switch (op.Type.Type)
			{
				case CilElementType.I:
					try
					{
						if (op.Value is Token)
						{
							return ((Token)op.Value).ToInt32();
						}
						else
						{
							return Convert.ToInt32(op.Value);
						}
					}
					catch (OverflowException)
					{
						// TODO: Exception
					}
					break;
				case CilElementType.I1:
				case CilElementType.I2:
					return (int)op.Value;
				case CilElementType.I4:
					goto case CilElementType.I;
				case CilElementType.U1:
					return (int)op.Value;
				case CilElementType.Char:
				case CilElementType.U2:
				case CilElementType.Ptr:
				case CilElementType.U4:
				case CilElementType.I8:
				case CilElementType.U8:
				case CilElementType.R4:
				case CilElementType.R8:
					goto default;
				case CilElementType.Object:
					goto case CilElementType.I;
				default:
					throw new NotSupportedException(String.Format(@"CilElementType.{0} is not supported.", op.Type.Type));
			}
			return 0;
		}

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(Context context, MachineCodeEmitter emitter)
		{		
			if (context.Result is RegisterOperand && context.Operand1 is ConstantOperand)
			{
				RegisterOperand reg = context.Result as RegisterOperand;
				ConstantOperand op = context.Operand1 as ConstantOperand;

				int value = ComputeValue(op);

				if (IsBetween(value, -128, 127))
				{
					emitter.EmitK8immediateAndSingleRegister(0x01, (sbyte)value, (byte)reg.Register.RegisterCode); // mov Rd, Imm (k8)
				}
				else
					if (IsBetween(value, -1048576, 1048575))
					{
						emitter.EmitRegisterOrConditionCodeAndK21(0x03, (byte)reg.Register.RegisterCode, value); // mov Rd, Imm (k21)
					}
					else
						throw new OverflowException();
			}

			if ((context.Result is RegisterOperand) && (context.Operand1 is RegisterOperand))
			{
				RegisterOperand destination = context.Result as RegisterOperand;
				RegisterOperand source = context.Operand1  as RegisterOperand;

				emitter.EmitTwoRegisterInstructions(0x09, (byte)source.Register.RegisterCode, (byte)destination.Register.RegisterCode); // mov Rd, Rs
			}
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IAVR32Visitor visitor, Context context)
		{
			visitor.Mov(context);
		}

		#endregion // Methods

	}
}
