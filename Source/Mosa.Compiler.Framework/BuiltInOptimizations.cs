// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework
{
	public static class BuiltInOptimizations
	{
		private static SimpleInstruction SimpleNopInstruction = new SimpleInstruction()
		{
			Instruction = IRInstruction.Nop
		};

		public static SimpleInstruction ConstantFoldingAndStrengthReductionInteger(InstructionNode node)
		{
			var operand = ConstantFoldingAndStrengthReductionIntegerToOperand(node);

			if (operand == null)
				return null;

			return new SimpleInstruction()
			{
				Instruction = GetMove(node.Result),
				Result = node.Result,
				Operand1 = operand
			};
		}

		public static SimpleInstruction StrengthReduction(InstructionNode node)
		{
			return StrengthReductionMultiplication(node)
				?? StrengthReductionDivision(node)
				?? StrengthReductionRemUnsignedModulus(node);
		}

		public static SimpleInstruction Simplification(InstructionNode node)
		{
			return FoldIfThenElse(node)
				?? SimplifyAddCarryOut32(node)
				?? SimplifyAddCarryOut64(node)
				?? SimplifySubCarryOut32(node)
				?? SimplifySubCarryOut64(node)
				?? PhiSimplication(node);
		}

		public static Operand ConstantFoldingAndStrengthReductionIntegerToOperand(InstructionNode node)
		{
			return ConstantFolding2Integer(node)
				?? ConstantFolding1Integer(node)
				?? StrengthReductionInteger(node)
				?? ConstantFolding3Integer(node);
		}

		private static Operand ConstantFolding1Integer(InstructionNode node)
		{
			if (node.OperandCount != 1 || node.ResultCount != 1)
				return null;

			if (!node.Operand1.IsResolvedConstant)
				return null;

			var instruction = node.Instruction;
			var result = node.Result;
			var op1 = node.Operand1;

			if (instruction == IRInstruction.GetLow64)
			{
				return ConstantOperand.Create(result.Type, (uint)op1.ConstantUnsigned64 & 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.GetHigh64)
			{
				return ConstantOperand.Create(result.Type, (uint)(op1.ConstantUnsigned64 >> 32));
			}
			else if (instruction == IRInstruction.LogicalNot32)
			{
				return ConstantOperand.Create(result.Type, ~((uint)op1.ConstantUnsigned64));
			}
			else if (instruction == IRInstruction.LogicalNot64)
			{
				return ConstantOperand.Create(result.Type, ~op1.ConstantUnsigned64);
			}
			else if (instruction == IRInstruction.Truncation64x32)
			{
				return ConstantOperand.Create(result.Type, (uint)op1.ConstantUnsigned64);
			}
			else if (instruction == IRInstruction.SignExtend8x32)
			{
				return ConstantOperand.Create(result.Type, SignExtend8x32((byte)op1.ConstantUnsigned64));
			}
			else if (instruction == IRInstruction.SignExtend16x32)
			{
				return ConstantOperand.Create(result.Type, SignExtend16x32((ushort)op1.ConstantUnsigned64));
			}
			else if (instruction == IRInstruction.SignExtend8x64)
			{
				return ConstantOperand.Create(result.Type, SignExtend8x64((byte)op1.ConstantUnsigned64));
			}
			else if (instruction == IRInstruction.SignExtend16x64)
			{
				return ConstantOperand.Create(result.Type, SignExtend16x64((ushort)op1.ConstantUnsigned64));
			}
			else if (instruction == IRInstruction.SignExtend32x64)
			{
				return ConstantOperand.Create(result.Type, SignExtend32x64((uint)op1.ConstantUnsigned64));
			}
			else if (instruction == IRInstruction.ZeroExtend8x32)
			{
				return ConstantOperand.Create(result.Type, (byte)op1.ConstantUnsigned64);
			}
			else if (instruction == IRInstruction.ZeroExtend16x32)
			{
				return ConstantOperand.Create(result.Type, (ushort)op1.ConstantUnsigned64);
			}
			else if (instruction == IRInstruction.ZeroExtend8x64)
			{
				return ConstantOperand.Create(result.Type, (byte)op1.ConstantUnsigned64);
			}
			else if (instruction == IRInstruction.ZeroExtend16x64)
			{
				return ConstantOperand.Create(result.Type, (ushort)op1.ConstantUnsigned64);
			}
			else if (instruction == IRInstruction.ZeroExtend32x64)
			{
				return ConstantOperand.Create(result.Type, (uint)op1.ConstantUnsigned64);
			}
			else if (instruction == IRInstruction.Convert32ToR4)
			{
				return ConstantOperand.Create(result.Type, (float)op1.ConstantSigned64);
			}
			else if (instruction == IRInstruction.Convert32ToR8)
			{
				return ConstantOperand.Create(result.Type, (double)op1.ConstantSigned64);
			}
			else if (instruction == IRInstruction.Convert64ToR4)
			{
				return ConstantOperand.Create(result.Type, (float)op1.ConstantSigned64);
			}
			else if (instruction == IRInstruction.Convert64ToR8)
			{
				return ConstantOperand.Create(result.Type, (double)op1.ConstantSigned64);
			}

			return null;
		}

		private static Operand ConstantFolding2Integer(InstructionNode node)
		{
			if (node.OperandCount != 2 || node.ResultCount != 1)
				return null;

			if (!node.Operand1.IsResolvedConstant || !node.Operand2.IsResolvedConstant)
				return null;

			var instruction = node.Instruction;
			var result = node.Result;
			var op1 = node.Operand1;
			var op2 = node.Operand2;

			if (instruction == IRInstruction.Add32)
			{
				return ConstantOperand.Create(result.Type, (op1.ConstantUnsigned64 + op2.ConstantUnsigned64) & 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.Add64)
			{
				return ConstantOperand.Create(result.Type, op1.ConstantUnsigned64 + op2.ConstantUnsigned64);
			}
			else if (instruction == IRInstruction.AddR4)
			{
				return ConstantOperand.Create(result.Type, (op1.ConstantFloat + op2.ConstantFloat));
			}
			else if (instruction == IRInstruction.AddR8)
			{
				return ConstantOperand.Create(result.Type, op1.ConstantDouble + op2.ConstantDouble);
			}
			else if (instruction == IRInstruction.Sub32)
			{
				return ConstantOperand.Create(result.Type, (op1.ConstantUnsigned64 - op2.ConstantUnsigned64) & 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.Sub64)
			{
				return ConstantOperand.Create(result.Type, op1.ConstantUnsigned64 - op2.ConstantUnsigned64);
			}
			else if (instruction == IRInstruction.SubR4)
			{
				return ConstantOperand.Create(result.Type, op1.ConstantFloat + op2.ConstantFloat);
			}
			else if (instruction == IRInstruction.SubR8)
			{
				return ConstantOperand.Create(result.Type, op1.ConstantFloat + op2.ConstantFloat);
			}
			else if (instruction == IRInstruction.LogicalAnd32)
			{
				return ConstantOperand.Create(result.Type, op1.ConstantUnsigned64 & op2.ConstantUnsigned64 & 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.LogicalAnd64)
			{
				return ConstantOperand.Create(result.Type, op1.ConstantUnsigned64 & op2.ConstantUnsigned64);
			}
			else if (instruction == IRInstruction.LogicalOr32)
			{
				return ConstantOperand.Create(result.Type, (op1.ConstantUnsigned64 | op2.ConstantUnsigned64) & 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.LogicalOr64)
			{
				return ConstantOperand.Create(result.Type, op1.ConstantUnsigned64 | op2.ConstantUnsigned64);
			}
			else if (instruction == IRInstruction.LogicalXor32)
			{
				return ConstantOperand.Create(result.Type, (op1.ConstantUnsigned64 ^ op2.ConstantUnsigned64) & 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.LogicalXor64)
			{
				return ConstantOperand.Create(result.Type, op1.ConstantUnsigned64 ^ op2.ConstantUnsigned64);
			}
			else if (instruction == IRInstruction.MulSigned32 || instruction == IRInstruction.MulUnsigned32)
			{
				return ConstantOperand.Create(result.Type, (op1.ConstantUnsigned64 * op2.ConstantUnsigned64) & 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.MulSigned64 || instruction == IRInstruction.MulUnsigned64)
			{
				return ConstantOperand.Create(result.Type, op1.ConstantUnsigned64 * op2.ConstantUnsigned64);
			}
			else if (instruction == IRInstruction.MulR4)
			{
				return ConstantOperand.Create(result.Type, op1.ConstantFloat * op2.ConstantFloat);
			}
			else if (instruction == IRInstruction.MulR8)
			{
				return ConstantOperand.Create(result.Type, op1.ConstantDouble * op2.ConstantDouble);
			}
			else if (instruction == IRInstruction.DivUnsigned32 && !op2.IsConstantZero)
			{
				return ConstantOperand.Create(result.Type, (op1.ConstantUnsigned64 / op2.ConstantUnsigned64) & 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.DivUnsigned64 && !op2.IsConstantZero)
			{
				return ConstantOperand.Create(result.Type, op1.ConstantUnsigned64 / op2.ConstantUnsigned64);
			}
			else if (instruction == IRInstruction.DivSigned32 && !op2.IsConstantZero)
			{
				return ConstantOperand.Create(result.Type, ((ulong)(op1.ConstantSigned64 / op2.ConstantSigned64)) & 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.DivSigned64 && !op2.IsConstantZero)
			{
				return ConstantOperand.Create(result.Type, (ulong)(op1.ConstantSigned64 / op2.ConstantSigned64));
			}
			else if (instruction == IRInstruction.DivR4 && !op2.IsConstantZero)
			{
				return ConstantOperand.Create(result.Type, op1.ConstantFloat / op2.ConstantFloat);
			}
			else if (instruction == IRInstruction.DivR8 && !op2.IsConstantZero)
			{
				return ConstantOperand.Create(result.Type, op1.ConstantDouble / op2.ConstantDouble);
			}

			//else if (instruction == IRInstruction.ArithShiftRight32)
			//{
			//	return ConstantOperand.Create(result.Type, ((ulong)(((long)op1.ConstantUnsigned64) >> (int)op2.ConstantUnsigned64)) & 0xFFFFFFFF);
			//}
			//else if (instruction == IRInstruction.ArithShiftRight64)
			//{
			//	return ConstantOperand.Create(result.Type, (ulong)(((long)op1.ConstantUnsigned64) >> (int)op2.ConstantUnsigned64));
			//}
			else if (instruction == IRInstruction.ShiftRight32)
			{
				return ConstantOperand.Create(result.Type, (op1.ConstantUnsigned64 >> (int)op2.ConstantUnsigned64) & 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.ShiftRight64)
			{
				return ConstantOperand.Create(result.Type, op1.ConstantUnsigned64 >> (int)op2.ConstantUnsigned64);
			}
			else if (instruction == IRInstruction.ShiftLeft32)
			{
				return ConstantOperand.Create(result.Type, (op1.ConstantUnsigned64 << (int)op2.ConstantUnsigned64) & 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.ShiftLeft64)
			{
				return ConstantOperand.Create(result.Type, op1.ConstantUnsigned64 << (int)op2.ConstantUnsigned64);
			}
			else if (instruction == IRInstruction.RemSigned32 && !op2.IsConstantZero)
			{
				return ConstantOperand.Create(result.Type, ((ulong)(op1.ConstantSigned64 % op2.ConstantSigned64)) & 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.RemSigned64 && !op2.IsConstantZero)
			{
				return ConstantOperand.Create(result.Type, (ulong)(op1.ConstantSigned64 % op2.ConstantSigned64));
			}
			else if (instruction == IRInstruction.RemUnsigned32 && !op2.IsConstantZero)
			{
				return ConstantOperand.Create(result.Type, (op1.ConstantUnsigned64 % op2.ConstantUnsigned64) & 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.RemUnsigned64 && !op2.IsConstantZero)
			{
				return ConstantOperand.Create(result.Type, op1.ConstantUnsigned64 % op2.ConstantUnsigned64);
			}
			else if (instruction == IRInstruction.RemR4 && !op2.IsConstantZero)
			{
				return ConstantOperand.Create(result.Type, op1.ConstantFloat % op2.ConstantFloat);
			}
			else if (instruction == IRInstruction.RemR8 && !op2.IsConstantZero)
			{
				return ConstantOperand.Create(result.Type, op1.ConstantDouble % op2.ConstantDouble);
			}
			else if (instruction == IRInstruction.To64)
			{
				return ConstantOperand.Create(result.Type, op2.ConstantUnsigned64 << 32 | op1.ConstantUnsigned64);
			}
			else if (instruction == IRInstruction.Compare32x32 || instruction == IRInstruction.Compare64x32 || instruction == IRInstruction.Compare64x64 || instruction == IRInstruction.Compare32x64)
			{
				bool compareResult = true;

				switch (node.ConditionCode)
				{
					case ConditionCode.Equal: compareResult = (op1.ConstantUnsigned64 == op2.ConstantUnsigned64); break;
					case ConditionCode.NotEqual: compareResult = (op1.ConstantUnsigned64 != op2.ConstantUnsigned64); break;
					case ConditionCode.GreaterOrEqual: compareResult = (op1.ConstantUnsigned64 >= op2.ConstantUnsigned64); break;
					case ConditionCode.GreaterThan: compareResult = (op1.ConstantUnsigned64 > op2.ConstantUnsigned64); break;
					case ConditionCode.LessOrEqual: compareResult = (op1.ConstantUnsigned64 <= op2.ConstantUnsigned64); break;
					case ConditionCode.LessThan: compareResult = (op1.ConstantUnsigned64 < op2.ConstantUnsigned64); break;

					case ConditionCode.UnsignedGreaterThan: compareResult = (op1.ConstantUnsigned64 > op2.ConstantUnsigned64); break;
					case ConditionCode.UnsignedGreaterOrEqual: compareResult = (op1.ConstantUnsigned64 >= op2.ConstantUnsigned64); break;
					case ConditionCode.UnsignedLessThan: compareResult = (op1.ConstantUnsigned64 < op2.ConstantUnsigned64); break;
					case ConditionCode.UnsignedLessOrEqual: compareResult = (op1.ConstantUnsigned64 <= op2.ConstantUnsigned64); break;

					// TODO: Add more
					default: return null;
				}

				return ConstantOperand.Create(result.Type, compareResult ? 1 : 0);
			}

			return null;
		}

		private static Operand ConstantFolding3Integer(InstructionNode node)
		{
			if (node.OperandCount != 3 || node.ResultCount != 1)
				return null;

			if (!node.Operand1.IsResolvedConstant || !node.Operand2.IsResolvedConstant || !node.Operand3.IsResolvedConstant)
				return null;

			var instruction = node.Instruction;
			var result = node.Result;
			var op1 = node.Operand1;
			var op2 = node.Operand2;
			var op3 = node.Operand3;

			if (instruction == IRInstruction.AddWithCarry32)
			{
				return ConstantOperand.Create(result.Type, (uint)(op1.ConstantUnsigned64 + op2.ConstantUnsigned64 + (op3.IsConstantZero ? 0u : 1u)));
			}
			else if (instruction == IRInstruction.SubWithCarry32)
			{
				return ConstantOperand.Create(result.Type, (uint)(op1.ConstantUnsigned64 - op2.ConstantUnsigned64 - (op3.IsConstantZero ? 0u : 1u)));
			}

			return null;
		}

		private static Operand StrengthReductionInteger(InstructionNode node)
		{
			if (node.OperandCount != 2 || node.ResultCount != 1)
				return null;

			var instruction = node.Instruction;
			var result = node.Result;
			var op1 = node.Operand1;
			var op2 = node.Operand2;

			if ((instruction == IRInstruction.Add32 || instruction == IRInstruction.Add64 || instruction == IRInstruction.AddR4 || instruction == IRInstruction.AddR8) && op1.IsConstantZero)
			{
				return op2;
			}
			else if ((instruction == IRInstruction.Add32 || instruction == IRInstruction.Add64 || instruction == IRInstruction.AddR4 || instruction == IRInstruction.AddR8) && op2.IsConstantZero)
			{
				return op1;
			}
			else if ((instruction == IRInstruction.Sub32 || instruction == IRInstruction.Sub64 || instruction == IRInstruction.SubR4 || instruction == IRInstruction.SubR8) && op2.IsConstantZero)
			{
				return op1;
			}
			else if ((instruction == IRInstruction.Sub32 || instruction == IRInstruction.Sub64) && (op1 == op2))
			{
				return ConstantOperand.Create(result.Type, 0);
			}
			else if (instruction == IRInstruction.SubR4 && op1 == op2)
			{
				return ConstantOperand.Create(result.Type, (float)0);
			}
			else if (instruction == IRInstruction.SubR8 && op1 == op2)
			{
				return ConstantOperand.Create(result.Type, (double)0);
			}
			else if ((instruction == IRInstruction.ShiftLeft32 || instruction == IRInstruction.ShiftLeft64 || instruction == IRInstruction.ShiftRight32 || instruction == IRInstruction.ShiftRight64) && op1.IsConstantZero)
			{
				return ConstantOperand.Create(result.Type, 0);
			}
			else if ((instruction == IRInstruction.ShiftLeft32 || instruction == IRInstruction.ShiftLeft64 || instruction == IRInstruction.ShiftRight32 || instruction == IRInstruction.ShiftRight64) && op2.IsConstantZero)
			{
				return op1;
			}

			//else if ((instruction == IRInstruction.ShiftLeft32 || instruction == IRInstruction.ShiftRight32) && op2.IsResolvedConstant && op2.ConstantUnsignedLongInteger >= 32)
			//{
			//	return ConstantOperand.Create(result.Type, 0);
			//}
			//else if ((instruction == IRInstruction.ShiftLeft64 || instruction == IRInstruction.ShiftRight64) && op2.IsResolvedConstant && op2.ConstantUnsignedLongInteger >= 64)
			//{
			//	return ConstantOperand.Create(result.Type, 0);
			//}
			else if ((instruction == IRInstruction.MulR4) && (op1.IsConstantZero || op2.IsConstantZero))
			{
				return ConstantOperand.Create(result.Type, 0.0f);
			}
			else if ((instruction == IRInstruction.MulR8) && (op1.IsConstantZero || op2.IsConstantZero))
			{
				return ConstantOperand.Create(result.Type, 0.0d);
			}
			else if ((instruction == IRInstruction.MulSigned32 || instruction == IRInstruction.MulUnsigned32 || instruction == IRInstruction.MulSigned64 || instruction == IRInstruction.MulUnsigned64) && (op1.IsConstantZero || op2.IsConstantZero))
			{
				return ConstantOperand.Create(result.Type, 0);
			}
			else if ((instruction == IRInstruction.MulSigned32 || instruction == IRInstruction.MulUnsigned32 || instruction == IRInstruction.MulSigned64 || instruction == IRInstruction.MulUnsigned64 || instruction == IRInstruction.MulR4 || instruction == IRInstruction.MulR8) && op1.IsConstantOne)
			{
				return op2;
			}
			else if ((instruction == IRInstruction.MulSigned32 || instruction == IRInstruction.MulUnsigned32 || instruction == IRInstruction.MulSigned64 || instruction == IRInstruction.MulUnsigned64 || instruction == IRInstruction.MulR4 || instruction == IRInstruction.MulR8) && op2.IsConstantOne)
			{
				return op1;
			}
			else if ((node.Instruction == IRInstruction.DivSigned32 || node.Instruction == IRInstruction.DivUnsigned32 || node.Instruction == IRInstruction.DivSigned64 || node.Instruction == IRInstruction.DivUnsigned64 || instruction == IRInstruction.DivR4 || instruction == IRInstruction.DivR8) && op2.IsConstantOne)
			{
				return op1;
			}
			else if ((node.Instruction == IRInstruction.DivSigned32 || node.Instruction == IRInstruction.DivUnsigned32 || node.Instruction == IRInstruction.DivSigned64 || node.Instruction == IRInstruction.DivUnsigned64 || instruction == IRInstruction.DivR4 || instruction == IRInstruction.DivR8) && op1.IsConstantZero)
			{
				return ConstantOperand.Create(result.Type, 0);
			}
			else if ((node.Instruction == IRInstruction.DivSigned32 || node.Instruction == IRInstruction.DivUnsigned32 || node.Instruction == IRInstruction.DivSigned64 || node.Instruction == IRInstruction.DivUnsigned64 || instruction == IRInstruction.DivR4 || instruction == IRInstruction.DivR8) && op1 == op2)
			{
				return ConstantOperand.Create(result.Type, 1);
			}
			else if ((node.Instruction == IRInstruction.RemUnsigned32 || node.Instruction == IRInstruction.RemUnsigned64 || instruction == IRInstruction.RemR4 || instruction == IRInstruction.RemR8) && op2.IsConstantOne)
			{
				return ConstantOperand.Create(result.Type, 0);
			}
			else if ((instruction == IRInstruction.LogicalAnd32 || instruction == IRInstruction.LogicalAnd64) && op1 == op2)
			{
				return op1;
			}
			else if ((instruction == IRInstruction.LogicalAnd32 || instruction == IRInstruction.LogicalAnd64) && (op1.IsConstantZero || op2.IsConstantZero))
			{
				return ConstantOperand.Create(result.Type, 0);
			}
			else if (instruction == IRInstruction.LogicalAnd32 && op1.IsConstant && op1.ConstantUnsigned32 == 0xFFFFFFFF)
			{
				return op2;
			}
			else if (instruction == IRInstruction.LogicalAnd64 && op1.IsConstant && op1.ConstantUnsigned64 == 0xFFFFFFFFFFFFFFFF)
			{
				return op2;
			}
			else if (instruction == IRInstruction.LogicalAnd32 && op2.IsConstant && op2.ConstantUnsigned32 == 0xFFFFFFFF)
			{
				return op1;
			}
			else if (instruction == IRInstruction.LogicalAnd64 && op2.IsConstant && op2.ConstantUnsigned64 == 0xFFFFFFFFFFFFFFFF)
			{
				return op1;
			}
			else if ((instruction == IRInstruction.LogicalOr32 || instruction == IRInstruction.LogicalOr64) && op1 == op2)
			{
				return op1;
			}
			else if ((instruction == IRInstruction.LogicalOr32 || instruction == IRInstruction.LogicalOr64) && op1.IsConstantZero)
			{
				return op2;
			}
			else if ((instruction == IRInstruction.LogicalOr32 || instruction == IRInstruction.LogicalOr64) && op2.IsConstantZero)
			{
				return op1;
			}
			else if (instruction == IRInstruction.LogicalOr32 && op1.IsConstant && op1.ConstantUnsigned32 == 0xFFFFFFFF)
			{
				return ConstantOperand.Create(result.Type, 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.LogicalOr64 && op1.IsConstant && op1.ConstantUnsigned64 == 0xFFFFFFFFFFFFFFFF)
			{
				return ConstantOperand.Create(result.Type, 0xFFFFFFFFFFFFFFFF);
			}
			else if (instruction == IRInstruction.LogicalOr32 && op2.IsConstant && op2.ConstantUnsigned32 == 0xFFFFFFFF)
			{
				return ConstantOperand.Create(result.Type, 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.LogicalOr64 && op2.IsConstant && op2.ConstantUnsigned64 == 0xFFFFFFFFFFFFFFFF)
			{
				return ConstantOperand.Create(result.Type, 0xFFFFFFFFFFFFFFFF);
			}
			else if ((instruction == IRInstruction.LogicalXor32 || instruction == IRInstruction.LogicalXor64) && op1 == op2)
			{
				return ConstantOperand.Create(result.Type, 0);
			}
			else if ((instruction == IRInstruction.LogicalXor32 || instruction == IRInstruction.LogicalXor64) && op1.IsConstantZero)
			{
				return op2;
			}
			else if ((instruction == IRInstruction.LogicalXor32 || instruction == IRInstruction.LogicalXor64) && op2.IsConstantZero)
			{
				return op1;
			}
			else if (instruction == IRInstruction.Compare32x32 || instruction == IRInstruction.Compare64x32 || instruction == IRInstruction.Compare64x64 || instruction == IRInstruction.Compare32x64)
			{
				var condition = node.ConditionCode;

				if (op1 == op2 && (condition == ConditionCode.Equal || condition == ConditionCode.GreaterOrEqual || condition == ConditionCode.UnsignedGreaterOrEqual || condition == ConditionCode.UnsignedLessOrEqual || condition == ConditionCode.LessOrEqual))
				{
					return ConstantOperand.Create(result.Type, true ? 1 : 0);
				}
				else if (op1 == op2 && (condition == ConditionCode.NotEqual || condition == ConditionCode.GreaterThan || condition == ConditionCode.LessThan || condition == ConditionCode.UnsignedGreaterThan || condition == ConditionCode.UnsignedLessThan))
				{
					return ConstantOperand.Create(result.Type, false ? 1 : 0);
				}
			}

			return null;
		}

		public static bool IsDeadCode(InstructionNode node)
		{
			if (node.ResultCount == 0 || node.ResultCount > 2)
				return false;

			if (!ValidateSSAForm(node.Result))
				return false;

			// special case - phi instruction references itself - this can be caused by optimizations
			if ((node.Result == node.Operand1) && (node.Instruction == IRInstruction.Phi32 || node.Instruction == IRInstruction.Phi64 || node.Instruction == IRInstruction.PhiR4 || node.Instruction == IRInstruction.PhiR8))
				return true;

			if (node.Result.Uses.Count != 0)
				return false;

			if (node.ResultCount == 2 && !ValidateSSAForm(node.Result2))
				return false;

			if (node.ResultCount == 2 && node.Result2.Uses.Count != 0)
				return false;

			if (node.Instruction == IRInstruction.CallDynamic
				|| node.Instruction == IRInstruction.CallInterface
				|| node.Instruction == IRInstruction.CallDirect
				|| node.Instruction == IRInstruction.CallStatic
				|| node.Instruction == IRInstruction.CallVirtual
				|| node.Instruction == IRInstruction.NewObject
				|| node.Instruction == IRInstruction.SetReturn32
				|| node.Instruction == IRInstruction.SetReturn64
				|| node.Instruction == IRInstruction.SetReturnR4
				|| node.Instruction == IRInstruction.SetReturnR8
				|| node.Instruction == IRInstruction.SetReturnCompound
				|| node.Instruction == IRInstruction.IntrinsicMethodCall)
				return false;

			return true;
		}

		public static SimpleInstruction PhiSimplication(InstructionNode node)
		{
			if (node.Instruction != IRInstruction.Phi32 && node.Instruction != IRInstruction.Phi64 && node.Instruction != IRInstruction.PhiR4 && node.Instruction != IRInstruction.PhiR8)
				return null;

			if (node.OperandCount == 1 && node.Result.IsInteger)
			{
				return new SimpleInstruction()
				{
					Instruction = GetMoveInteger(node.Result),
					Result = node.Result,
					Operand1 = node.Operand1
				};
			}

			return null;
		}

		public static SimpleInstruction StrengthReductionMultiplication(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.MulSigned32
				|| node.Instruction == IRInstruction.MulUnsigned32
				|| node.Instruction == IRInstruction.MulSigned64
				|| node.Instruction == IRInstruction.MulUnsigned64))
				return null;

			var result = node.Result;
			var op1 = node.Operand1;
			var op2 = node.Operand2;

			if (!op2.IsResolvedConstant)
				return null;

			if (BitTwiddling.IsPowerOfTwo(op2.ConstantUnsigned64))
			{
				uint shift = BitTwiddling.GetPowerOfTwo(op2.ConstantUnsigned64);

				if (shift < 32 || (shift < 64 && result.Is64BitInteger))
				{
					return new SimpleInstruction()
					{
						Instruction = Select(result.Is64BitInteger, IRInstruction.ShiftLeft32, IRInstruction.ShiftLeft64),
						Result = result,
						Operand1 = op1,
						Operand2 = ConstantOperand.Create(result.Type, (uint)shift)
					};
				}
			}

			return null;
		}

		public static SimpleInstruction StrengthReductionDivision(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.DivUnsigned32
				|| node.Instruction == IRInstruction.DivUnsigned64))
				return null;

			var result = node.Result;
			var op1 = node.Operand1;
			var op2 = node.Operand2;

			if (!op2.IsResolvedConstant)
				return null;

			if (op2.IsConstantZero || op2.IsVirtualRegister)
				return null;

			uint shift = BitTwiddling.GetPowerOfTwo(op2.ConstantUnsigned64);

			if (shift < 32 || (shift < 64 && result.Is64BitInteger))
			{
				return new SimpleInstruction()
				{
					Instruction = Select(result.Is64BitInteger, IRInstruction.ShiftRight32, IRInstruction.ShiftRight64),
					Result = result,
					Operand1 = op1,
					Operand2 = ConstantOperand.Create(result.Type, (uint)shift)
				};
			}

			return null;
		}

		public static SimpleInstruction StrengthReductionRemUnsignedModulus(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.RemUnsigned32
				|| node.Instruction == IRInstruction.RemUnsigned64))
				return null;

			var result = node.Result;
			var op1 = node.Operand1;
			var op2 = node.Operand2;

			if (!op2.IsResolvedConstant)
				return null;

			if (op2.ConstantUnsigned64 == 0)
				return null;

			if (!BitTwiddling.IsPowerOfTwo(op2.ConstantUnsigned64))
				return null;

			uint power = BitTwiddling.GetPowerOfTwo(op2.ConstantUnsigned64);

			var mask = (1 << (int)power) - 1;

			return new SimpleInstruction()
			{
				Instruction = Select(result.Is64BitInteger, IRInstruction.LogicalAnd32, IRInstruction.LogicalAnd64),
				Result = result,
				Operand1 = op1,
				Operand2 = ConstantOperand.Create(result.Type, (uint)mask)
			};
		}

		public static SimpleInstruction DeadCodeElimination(InstructionNode node)
		{
			if (!IsDeadCode(node))
				return null;

			return SimpleNopInstruction;
		}

		public static SimpleInstruction FoldIfThenElse(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.IfThenElse64
				|| node.Instruction == IRInstruction.IfThenElse32))
				return null;

			bool result = false;

			if (node.Operand2.IsVirtualRegister && node.Operand3.IsVirtualRegister && node.Operand2 == node.Operand3)
			{
				result = true;
			}
			else if (node.Operand1.IsResolvedConstant)
			{
				result = node.Operand1.ConstantUnsigned64 != 0;
			}
			else if (node.Operand2.IsResolvedConstant && node.Operand3.IsResolvedConstant)
			{
				result = node.Operand2.ConstantUnsigned64 == node.Operand3.ConstantUnsigned64;
			}
			else
			{
				return null;
			}

			return new SimpleInstruction()
			{
				Instruction = Select(node.Instruction == IRInstruction.IfThenElse32, IRInstruction.Move32, IRInstruction.Move64),
				Result = node.Result,
				Operand1 = result ? node.Operand2 : node.Operand3,
			};
		}

		public static SimpleInstruction SimplifyAddCarryOut32(InstructionNode node)
		{
			if (node.Instruction != IRInstruction.AddCarryOut32)
				return null;

			if (node.Result2.Uses.Count != 0)
				return null;

			return new SimpleInstruction()
			{
				Instruction = IRInstruction.Add32,
				Result = node.Result,
				Operand1 = node.Operand1,
				Operand2 = node.Operand2
			};
		}

		public static SimpleInstruction SimplifyAddCarryOut64(InstructionNode node)
		{
			if (node.Instruction != IRInstruction.AddCarryOut64)
				return null;

			if (node.Result2.Uses.Count != 0)
				return null;

			return new SimpleInstruction()
			{
				Instruction = IRInstruction.Add64,
				Result = node.Result,
				Operand1 = node.Operand1,
				Operand2 = node.Operand2
			};
		}

		public static SimpleInstruction SimplifySubCarryOut32(InstructionNode node)
		{
			if (node.Instruction != IRInstruction.SubCarryOut32)
				return null;

			if (node.Result2.Uses.Count != 0)
				return null;

			return new SimpleInstruction()
			{
				Instruction = IRInstruction.Sub32,
				Result = node.Result,
				Operand1 = node.Operand1,
				Operand2 = node.Operand2
			};
		}

		public static SimpleInstruction SimplifySubCarryOut64(InstructionNode node)
		{
			if (node.Instruction != IRInstruction.SubCarryOut64)
				return null;

			if (node.Result2.Uses.Count != 0)
				return null;

			return new SimpleInstruction()
			{
				Instruction = IRInstruction.Sub64,
				Result = node.Result,
				Operand1 = node.Operand1,
				Operand2 = node.Operand2
			};
		}

		#region Helpers

		private static uint SignExtend8x32(byte value)
		{
			return ((value & 0x80) == 0) ? value : (value | 0xFFFFFF00);
		}

		private static uint SignExtend16x32(ushort value)
		{
			return ((value & 0x8000) == 0) ? value : (value | 0xFFFF0000);
		}

		private static ulong SignExtend8x64(byte value)
		{
			return ((value & 0x80) == 0) ? value : (value | 0xFFFFFFFFFFFFFF00ul);
		}

		private static ulong SignExtend16x64(ushort value)
		{
			return ((value & 0x8000) == 0) ? value : (value | 0xFFFFFFFFFFFF0000ul);
		}

		private static ulong SignExtend32x64(uint value)
		{
			return ((value & 0x80000000) == 0) ? value : (value | 0xFFFFFFFF00000000ul);
		}

		private static bool ValidateSSAForm(Operand operand)
		{
			return operand.Definitions.Count == 1;
		}

		public static BaseInstruction Select(bool is64bit, BaseInstruction instruction32, BaseInstruction instruction64)
		{
			return is64bit ? instruction64 : instruction32;
		}

		private static BaseInstruction GetMoveInteger(Operand operand)
		{
			return operand.Is64BitInteger ? (BaseInstruction)IRInstruction.Move64 : IRInstruction.Move32;
		}

		private static BaseInstruction GetMove(Operand operand)
		{
			if (operand.IsR4)
				return IRInstruction.MoveR4;
			else if (operand.IsR8)
				return IRInstruction.MoveR8;
			else if (operand.Is64BitInteger)
				return IRInstruction.Move64;
			else
				return IRInstruction.Move32;
		}

		#endregion Helpers
	}
}
