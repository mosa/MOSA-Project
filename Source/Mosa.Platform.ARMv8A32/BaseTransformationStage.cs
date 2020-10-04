// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Platform;
using System.Diagnostics;

namespace Mosa.Platform.ARMv8A32
{
	/// <summary>
	/// BaseTransformationStage
	/// </summary>
	public abstract class BaseTransformationStage : BasePlatformTransformationStage
	{
		protected override string Platform { get { return "ARMv8A32"; } }

		#region Helper Methods

		protected void SwapFirstTwoOperandsIfFirstConstant(Context context)
		{
			var operand1 = context.Operand1;

			if (operand1.IsConstant && context.Instruction.IsCommutative)
			{
				var operand2 = context.Operand2;

				context.Operand1 = operand2;
				context.Operand2 = operand1;
			}
		}

		protected void TransformLoadInstruction(Context context, BaseInstruction loadUp, BaseInstruction loadUpImm, BaseInstruction loadDownImm, Operand result, Operand baseOperand, Operand offsetOperand)
		{
			BaseInstruction instruction;

			baseOperand = MoveConstantToRegister(context, baseOperand);

			if (offsetOperand.IsResolvedConstant)
			{
				if (offsetOperand.ConstantUnsigned64 >= 0 && offsetOperand.ConstantSigned32 <= (1 << 13))
				{
					instruction = loadUpImm;
				}
				else if (offsetOperand.ConstantUnsigned64 < 0 && -offsetOperand.ConstantSigned32 <= (1 << 13))
				{
					instruction = loadDownImm;
					offsetOperand = CreateConstant((uint)-offsetOperand.ConstantSigned32);
				}
				else
				{
					instruction = loadDownImm;
					offsetOperand = MoveConstantToRegister(context, offsetOperand);
				}
			}
			else if (offsetOperand.IsUnresolvedConstant)
			{
				instruction = loadUp;
				offsetOperand = MoveConstantToRegister(context, offsetOperand);
			}
			else
			{
				instruction = loadUp;
			}

			context.SetInstruction(instruction, ConditionCode.Always, result, baseOperand, offsetOperand);
		}

		protected void TransformStoreInstruction(Context context, BaseInstruction storeUp, BaseInstruction storeUpImm, BaseInstruction loadDownImm, Operand baseOperand, Operand offsetOperand, Operand sourceOperand)
		{
			BaseInstruction instruction;

			baseOperand = MoveConstantToRegister(context, baseOperand);
			sourceOperand = MoveConstantToRegister(context, sourceOperand);

			if (offsetOperand.IsResolvedConstant)
			{
				if (offsetOperand.ConstantUnsigned64 >= 0 && offsetOperand.ConstantSigned32 <= (1 << 13))
				{
					instruction = storeUpImm;
				}
				else if (offsetOperand.ConstantUnsigned64 < 0 && -offsetOperand.ConstantSigned32 <= (1 << 13))
				{
					instruction = loadDownImm;
					offsetOperand = CreateConstant((uint)-offsetOperand.ConstantSigned32);
				}
				else
				{
					instruction = loadDownImm;
					offsetOperand = MoveConstantToRegister(context, offsetOperand);
				}
			}
			else if (offsetOperand.IsUnresolvedConstant)
			{
				instruction = storeUp;
				offsetOperand = MoveConstantToRegister(context, offsetOperand);
			}
			else
			{
				instruction = storeUp;
			}

			context.SetInstruction(instruction, ConditionCode.Always, null, baseOperand, offsetOperand, sourceOperand);
		}

		protected Operand CreateImmediateOperand(Context context, Operand operand)
		{
			if (operand.IsVirtualRegister || operand.IsCPURegister)
				return operand;

			if (operand.IsResolvedConstant && ARMHelper.CalculateRotatedImmediateValue(operand.ConstantUnsigned32, out uint immediate, out _, out _))
			{
				if (operand.ConstantUnsigned64 == immediate)
				{
					return operand;
				}

				return CreateConstant(immediate);
			}

			return MoveConstantToRegister(context, operand);
		}

		protected Operand MoveConstantToRegister(Context context, Operand operand)
		{
			if (operand.IsVirtualRegister || operand.IsCPURegister)
				return operand;

			if (operand.IsResolvedConstant)
			{
				var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

				var before = context.InsertBefore();

				if (operand.ConstantUnsigned32 <= 0xFFFF)
				{
					before.SetInstruction(ARMv8A32.MovImm, v1, operand);

					return v1;
				}

				if (ARMHelper.CalculateRotatedImmediateValue(operand.ConstantUnsigned32, out uint immediate, out byte rotation4, out byte imm8))
				{
					before.SetInstruction(ARMv8A32.MovImm, v1, CreateConstant(immediate));

					return v1;
				}

				before.SetInstruction(ARMv8A32.Movw, v1, CreateConstant(operand.ConstantUnsigned32 & 0xFFFF));
				before.AppendInstruction(ARMv8A32.Movt, v1, v1, CreateConstant(operand.ConstantUnsigned32 >> 16));

				return v1;
			}
			else if (operand.IsUnresolvedConstant)
			{
				var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

				var before = context.InsertBefore();

				before.SetInstruction(ARMv8A32.Movw, v1, operand);
				before.AppendInstruction(ARMv8A32.Movt, v1, v1, operand);

				return v1;
			}

			throw new CompilerException("Error at {context} in {Method}");
		}

		private Operand ConvertFloatToImm(Operand operand)
		{
			if (operand.IsCPURegister || operand.IsVirtualRegister)
				return operand;

			if (operand.IsR4)
			{
				var value = operand.ConstantFloat;

				switch (value)
				{
					case 0.0f: return CreateConstant(0b1000);
					case 1.0f: return CreateConstant(0b1001);
					case 2.0f: return CreateConstant(0b1010);
					case 3.0f: return CreateConstant(0b1011);
					case 4.0f: return CreateConstant(0b1100);
					case 5.0f: return CreateConstant(0b1101);
					case 0.5f: return CreateConstant(0b1110);
					case 10.0f: return CreateConstant(0b1111);
				}
			}
			else if (operand.IsR4)
			{
				var value = operand.ConstantDouble;

				switch (value)
				{
					case 0.0d: return CreateConstant(0b1000);
					case 1.0d: return CreateConstant(0b1001);
					case 2.0d: return CreateConstant(0b1010);
					case 3.0d: return CreateConstant(0b1011);
					case 4.0d: return CreateConstant(0b1100);
					case 5.0d: return CreateConstant(0b1101);
					case 0.5d: return CreateConstant(0b1110);
					case 10.0d: return CreateConstant(0b1111);
				}
			}

			return operand;
		}

		protected void TransformInstructionXXX(Context context, BaseInstruction virtualInstruction, BaseInstruction immediateInstruction, Operand result, StatusRegister statusRegister, Operand operand1, Operand operand2)
		{
			// TODO!!!!!

			// AddFloatR4 result, operand1, operand2

			// TODO: (across all float instructions)
			// if operand1 is constant
			// if resolved & specific constant, then AdfImm
			// else if resolved & non-specific constant, then LoadConstant, adf
			// else if unresolved, throw not implemented

			if (operand1.IsConstant)
			{
				if (virtualInstruction.IsCommutative && !operand2.IsConstant)
				{
					var temp = operand1;
					operand1 = operand2;
					operand2 = temp;
				}
				else
				{
					operand1 = MoveConstantToRegister(context, operand1);
				}
			}

			if (operand2.IsConstant)
			{
				operand2 = CreateImmediateOperand(context, operand2);
			}

			Debug.Assert(operand1.IsVirtualRegister || operand1.IsCPURegister);

			if (operand2.IsVirtualRegister || operand2.IsCPURegister)
			{
				context.SetInstruction(virtualInstruction, statusRegister, result, operand1, operand2);
			}
			else if (operand2.IsResolvedConstant)
			{
				context.SetInstruction(immediateInstruction, statusRegister, result, operand1, operand2);
			}
			else
			{
				throw new CompilerException("Error at {context} in {Method}");
			}
		}

		#endregion Helper Methods
	}
}
