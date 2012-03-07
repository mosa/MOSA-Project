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

namespace Mosa.Platform.AVR32.Instructions
{


	/// <summary>
	/// Cp Instruction
	/// Supported Format :
	///     cp.w Rd, Rs
	///     cp.w Rd, imm 6 bits
	///     cp.w Rd, imm 21 bits
	/// </summary>
	public class CpInstruction : BaseInstruction
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
			if (context.Result is RegisterOperand && context.Operand1 is RegisterOperand)
			{
				RegisterOperand destination = context.Result as RegisterOperand;
				RegisterOperand source = context.Operand1 as RegisterOperand;

				emitter.EmitTwoRegisterInstructions(0x03, (byte)source.Register.RegisterCode, (byte)destination.Register.RegisterCode); // cp.w Rd, Rs
			}
			else
				if (context.Result is RegisterOperand && context.Operand1 is ConstantOperand)
				{
					RegisterOperand destination = context.Result as RegisterOperand;
					ConstantOperand source = context.Operand1 as ConstantOperand;

					int value = ComputeValue(source);

					if (IsBetween(value, -32, 31))
					{
						emitter.EmitK6immediateAndSingleRegister((sbyte)value, (byte)destination.Register.RegisterCode); // cp.w Rd, imm 6 bits
					}
					else
						if (IsBetween(value, -1048576, 1048575))
						{
							emitter.EmitRegisterOrConditionCodeAndK21((byte)0x02, (byte)destination.Register.RegisterCode, value); // cp.w Rd, imm 21 bits
						}

				}
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
