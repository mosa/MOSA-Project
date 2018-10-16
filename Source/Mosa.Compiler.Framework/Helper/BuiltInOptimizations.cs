// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Helper
{
	public static class BuiltInOptimizations
	{
		public static Operand ConstantFoldingAndStrengthReductionIntegerToOperand(InstructionNode node)
		{
			return ConstantFolding2Integer(node)
				?? ConstantFolding1Integer(node)
				?? StrengthReductionInteger(node)
				?? ConstantFolding3Integer(node);
		}

		public static SimpleInstruction StrengthReductionAndSimplification(InstructionNode node)
		{
			return ArithmeticSimplificationMultiplication(node)
				?? ArithmeticSimplificationDivision(node)
				?? ArithmeticSimplificationRemUnsignedModulus(node)				
				?? PhiSimplication(node);
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
				return ConstantOperand.Create(result.Type, (uint)op1.ConstantUnsignedLongInteger & 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.GetHigh64)
			{
				return ConstantOperand.Create(result.Type, (uint)(op1.ConstantUnsignedLongInteger >> 32));
			}
			else if (instruction == IRInstruction.LogicalNot32)
			{
				return ConstantOperand.Create(result.Type, ~((uint)op1.ConstantUnsignedLongInteger));
			}
			else if (instruction == IRInstruction.LogicalNot64)
			{
				return ConstantOperand.Create(result.Type, ~((ulong)op1.ConstantUnsignedLongInteger));
			}
			else if (instruction == IRInstruction.Truncation64x32)
			{
				return ConstantOperand.Create(result.Type, (uint)op1.ConstantUnsignedLongInteger);
			}
			else if (instruction == IRInstruction.SignExtend8x32)
			{
				return ConstantOperand.Create(result.Type, SignExtend8x32((byte)op1.ConstantUnsignedLongInteger));
			}
			else if (instruction == IRInstruction.SignExtend16x32)
			{
				return ConstantOperand.Create(result.Type, SignExtend16x32((ushort)op1.ConstantUnsignedLongInteger));
			}
			else if (instruction == IRInstruction.SignExtend8x64)
			{
				return ConstantOperand.Create(result.Type, SignExtend8x64((byte)op1.ConstantUnsignedLongInteger));
			}
			else if (instruction == IRInstruction.SignExtend16x64)
			{
				return ConstantOperand.Create(result.Type, SignExtend16x64((ushort)op1.ConstantUnsignedLongInteger));
			}
			else if (instruction == IRInstruction.SignExtend32x64)
			{
				return ConstantOperand.Create(result.Type, SignExtend32x64((uint)op1.ConstantUnsignedLongInteger));
			}
			else if (instruction == IRInstruction.ZeroExtend8x32)
			{
				return ConstantOperand.Create(result.Type, (byte)op1.ConstantUnsignedLongInteger);
			}
			else if (instruction == IRInstruction.ZeroExtend16x32)
			{
				return ConstantOperand.Create(result.Type, (ushort)op1.ConstantUnsignedLongInteger);
			}
			else if (instruction == IRInstruction.ZeroExtend8x64)
			{
				return ConstantOperand.Create(result.Type, (byte)op1.ConstantUnsignedLongInteger);
			}
			else if (instruction == IRInstruction.ZeroExtend16x64)
			{
				return ConstantOperand.Create(result.Type, (ushort)op1.ConstantUnsignedLongInteger);
			}
			else if (instruction == IRInstruction.ZeroExtend32x64)
			{
				return ConstantOperand.Create(result.Type, (uint)op1.ConstantUnsignedLongInteger);
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
				return ConstantOperand.Create(result.Type, (op1.ConstantUnsignedLongInteger + op2.ConstantUnsignedLongInteger) & 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.Add64)
			{
				return ConstantOperand.Create(result.Type, op1.ConstantUnsignedLongInteger + op2.ConstantUnsignedLongInteger);
			}
			else if (instruction == IRInstruction.Sub32)
			{
				return ConstantOperand.Create(result.Type, (op1.ConstantUnsignedLongInteger - op2.ConstantUnsignedLongInteger) & 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.Sub64)
			{
				return ConstantOperand.Create(result.Type, op1.ConstantUnsignedLongInteger - op2.ConstantUnsignedLongInteger);
			}
			else if (instruction == IRInstruction.LogicalAnd32)
			{
				return ConstantOperand.Create(result.Type, op1.ConstantUnsignedLongInteger & op2.ConstantUnsignedLongInteger & 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.LogicalAnd64)
			{
				return ConstantOperand.Create(result.Type, op1.ConstantUnsignedLongInteger & op2.ConstantUnsignedLongInteger);
			}
			else if (instruction == IRInstruction.LogicalOr32)
			{
				return ConstantOperand.Create(result.Type, (op1.ConstantUnsignedLongInteger | op2.ConstantUnsignedLongInteger) & 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.LogicalOr64)
			{
				return ConstantOperand.Create(result.Type, op1.ConstantUnsignedLongInteger | op2.ConstantUnsignedLongInteger);
			}
			else if (instruction == IRInstruction.LogicalXor32)
			{
				return ConstantOperand.Create(result.Type, (op1.ConstantUnsignedLongInteger ^ op2.ConstantUnsignedLongInteger) & 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.LogicalXor64)
			{
				return ConstantOperand.Create(result.Type, op1.ConstantUnsignedLongInteger ^ op2.ConstantUnsignedLongInteger);
			}
			else if (instruction == IRInstruction.MulSigned32 || instruction == IRInstruction.MulUnsigned32)
			{
				return ConstantOperand.Create(result.Type, (op1.ConstantUnsignedLongInteger * op2.ConstantUnsignedLongInteger) & 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.MulSigned64 || instruction == IRInstruction.MulUnsigned64)
			{
				return ConstantOperand.Create(result.Type, op1.ConstantUnsignedLongInteger * op2.ConstantUnsignedLongInteger);
			}
			else if (instruction == IRInstruction.DivUnsigned32 && !op2.IsConstantZero)
			{
				return ConstantOperand.Create(result.Type, (op1.ConstantUnsignedLongInteger / op2.ConstantUnsignedLongInteger) & 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.DivUnsigned64 && !op2.IsConstantZero)
			{
				return ConstantOperand.Create(result.Type, op1.ConstantUnsignedLongInteger / op2.ConstantUnsignedLongInteger);
			}
			else if (instruction == IRInstruction.DivSigned32 && !op2.IsConstantZero)
			{
				return ConstantOperand.Create(result.Type, ((ulong)(op1.ConstantSignedLongInteger / op2.ConstantSignedLongInteger)) & 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.DivSigned64 && !op2.IsConstantZero)
			{
				return ConstantOperand.Create(result.Type, (ulong)(op1.ConstantSignedLongInteger / op2.ConstantSignedLongInteger));
			}
			else if (instruction == IRInstruction.ArithShiftRight32)
			{
				return ConstantOperand.Create(result.Type, ((ulong)(((long)op1.ConstantUnsignedLongInteger) >> (int)op2.ConstantUnsignedLongInteger)) & 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.ArithShiftRight64)
			{
				return ConstantOperand.Create(result.Type, (ulong)(((long)op1.ConstantUnsignedLongInteger) >> (int)op2.ConstantUnsignedLongInteger));
			}
			else if (instruction == IRInstruction.ShiftRight32)
			{
				return ConstantOperand.Create(result.Type, (op1.ConstantUnsignedLongInteger >> (int)op2.ConstantUnsignedLongInteger) & 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.ShiftRight64)
			{
				return ConstantOperand.Create(result.Type, op1.ConstantUnsignedLongInteger >> (int)op2.ConstantUnsignedLongInteger);
			}
			else if (instruction == IRInstruction.ShiftLeft32)
			{
				return ConstantOperand.Create(result.Type, (op1.ConstantUnsignedLongInteger << (int)op2.ConstantUnsignedLongInteger) & 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.ShiftLeft64)
			{
				return ConstantOperand.Create(result.Type, op1.ConstantUnsignedLongInteger << (int)op2.ConstantUnsignedLongInteger);
			}
			else if (instruction == IRInstruction.RemSigned32 && !op2.IsConstantZero)
			{
				return ConstantOperand.Create(result.Type, ((ulong)(op1.ConstantSignedLongInteger % op2.ConstantSignedLongInteger)) & 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.RemSigned64 && !op2.IsConstantZero)
			{
				return ConstantOperand.Create(result.Type, (ulong)(op1.ConstantSignedLongInteger % op2.ConstantSignedLongInteger));
			}
			else if (instruction == IRInstruction.RemUnsigned32 && !op2.IsConstantZero)
			{
				return ConstantOperand.Create(result.Type, (op1.ConstantUnsignedLongInteger % op2.ConstantUnsignedLongInteger) & 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.RemUnsigned64 && !op2.IsConstantZero)
			{
				return ConstantOperand.Create(result.Type, op1.ConstantUnsignedLongInteger % op2.ConstantUnsignedLongInteger);
			}
			else if (instruction == IRInstruction.To64)
			{
				return ConstantOperand.Create(result.Type, op2.ConstantUnsignedLongInteger << 32 | op1.ConstantUnsignedLongInteger);
			}
			else if (instruction == IRInstruction.CompareInt32x32 || instruction == IRInstruction.CompareInt64x32 || instruction == IRInstruction.CompareInt64x64)
			{
				bool compareResult = true;

				switch (node.ConditionCode)
				{
					case ConditionCode.Equal: compareResult = (op1.ConstantUnsignedLongInteger == op2.ConstantUnsignedLongInteger); break;
					case ConditionCode.NotEqual: compareResult = (op1.ConstantUnsignedLongInteger != op2.ConstantUnsignedLongInteger); break;
					case ConditionCode.GreaterOrEqual: compareResult = (op1.ConstantUnsignedLongInteger >= op2.ConstantUnsignedLongInteger); break;
					case ConditionCode.GreaterThan: compareResult = (op1.ConstantUnsignedLongInteger > op2.ConstantUnsignedLongInteger); break;
					case ConditionCode.LessOrEqual: compareResult = (op1.ConstantUnsignedLongInteger <= op2.ConstantUnsignedLongInteger); break;
					case ConditionCode.LessThan: compareResult = (op1.ConstantUnsignedLongInteger < op2.ConstantUnsignedLongInteger); break;

					case ConditionCode.UnsignedGreaterThan: compareResult = (op1.ConstantUnsignedLongInteger > op2.ConstantUnsignedLongInteger); break;
					case ConditionCode.UnsignedGreaterOrEqual: compareResult = (op1.ConstantUnsignedLongInteger >= op2.ConstantUnsignedLongInteger); break;
					case ConditionCode.UnsignedLessThan: compareResult = (op1.ConstantUnsignedLongInteger < op2.ConstantUnsignedLongInteger); break;
					case ConditionCode.UnsignedLessOrEqual: compareResult = (op1.ConstantUnsignedLongInteger <= op2.ConstantUnsignedLongInteger); break;

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
				return ConstantOperand.Create(result.Type, (uint)(op1.ConstantUnsignedLongInteger + op2.ConstantUnsignedLongInteger + (op3.IsConstantZero ? 0u : 1u)));
			}
			else if (instruction == IRInstruction.SubWithCarry32)
			{
				return ConstantOperand.Create(result.Type, (uint)(op1.ConstantUnsignedLongInteger - op2.ConstantUnsignedLongInteger - (op3.IsConstantZero ? 0u : 1u)));
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

			if ((instruction == IRInstruction.Add32 || instruction == IRInstruction.Add64) && op1.IsConstantZero)
			{
				return op2;
			}
			else if ((instruction == IRInstruction.Add32 || instruction == IRInstruction.Add64) && op2.IsConstantZero)
			{
				return op1;
			}
			else if ((instruction == IRInstruction.Sub32 || instruction == IRInstruction.Sub64) && op2.IsConstantZero)
			{
				return op1;
			}
			else if ((instruction == IRInstruction.Sub32 || instruction == IRInstruction.Sub64) && (op1 == op2))
			{
				return ConstantOperand.Create(result.Type, 0);
			}
			else if ((instruction == IRInstruction.ShiftLeft32 || instruction == IRInstruction.ShiftLeft64 || instruction == IRInstruction.ShiftRight32 || instruction == IRInstruction.ShiftRight64) && op1.IsConstantZero)
			{
				return ConstantOperand.Create(result.Type, 0);
			}
			else if ((instruction == IRInstruction.ShiftLeft32 || instruction == IRInstruction.ShiftLeft64 || instruction == IRInstruction.ShiftRight32 || instruction == IRInstruction.ShiftRight64) && op2.IsConstantZero)
			{
				return op1;
			}
			else if ((instruction == IRInstruction.ShiftLeft32 || instruction == IRInstruction.ShiftRight32) && op2.IsResolvedConstant && op2.ConstantUnsignedLongInteger >= 32)
			{
				return ConstantOperand.Create(result.Type, 0);
			}
			else if ((instruction == IRInstruction.ShiftLeft64 || instruction == IRInstruction.ShiftRight64) && op2.IsResolvedConstant && op2.ConstantUnsignedLongInteger >= 64)
			{
				return ConstantOperand.Create(result.Type, 0);
			}
			else if ((instruction == IRInstruction.MulSigned32 || instruction == IRInstruction.MulUnsigned32 || instruction == IRInstruction.MulSigned64 || instruction == IRInstruction.MulUnsigned64) && (op1.IsConstantZero || op2.IsConstantZero))
			{
				return ConstantOperand.Create(result.Type, 0);
			}
			else if ((instruction == IRInstruction.MulSigned32 || instruction == IRInstruction.MulUnsigned32 || instruction == IRInstruction.MulSigned64 || instruction == IRInstruction.MulUnsigned64) && op1.IsConstantOne)
			{
				return op2;
			}
			else if ((instruction == IRInstruction.MulSigned32 || instruction == IRInstruction.MulUnsigned32 || instruction == IRInstruction.MulSigned64 || instruction == IRInstruction.MulUnsigned64) && op2.IsConstantOne)
			{
				return op1;
			}
			else if ((node.Instruction == IRInstruction.DivSigned32 || node.Instruction == IRInstruction.DivUnsigned32 || node.Instruction == IRInstruction.DivSigned64 || node.Instruction == IRInstruction.DivUnsigned64) && op2.IsConstantOne)
			{
				return op1;
			}
			else if ((node.Instruction == IRInstruction.DivSigned32 || node.Instruction == IRInstruction.DivUnsigned32 || node.Instruction == IRInstruction.DivSigned64 || node.Instruction == IRInstruction.DivUnsigned64) && op1.IsConstantZero)
			{
				return ConstantOperand.Create(result.Type, 0);
			}
			else if ((node.Instruction == IRInstruction.DivSigned32 || node.Instruction == IRInstruction.DivUnsigned32 || node.Instruction == IRInstruction.DivSigned64 || node.Instruction == IRInstruction.DivUnsigned64) && op1 == op2)
			{
				return ConstantOperand.Create(result.Type, 1);
			}
			else if ((node.Instruction == IRInstruction.RemUnsigned32 || node.Instruction == IRInstruction.RemUnsigned64) && op2.IsConstantOne)
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
			else if (instruction == IRInstruction.LogicalAnd32 && op1.IsConstant && op1.ConstantUnsignedInteger == 0xFFFFFFFF)
			{
				return op2;
			}
			else if (instruction == IRInstruction.LogicalAnd64 && op1.IsConstant && op1.ConstantUnsignedLongInteger == 0xFFFFFFFFFFFFFFFF)
			{
				return op2;
			}
			else if (instruction == IRInstruction.LogicalAnd32 && op2.IsConstant && op2.ConstantUnsignedInteger == 0xFFFFFFFF)
			{
				return op1;
			}
			else if (instruction == IRInstruction.LogicalAnd64 && op2.IsConstant && op2.ConstantUnsignedLongInteger == 0xFFFFFFFFFFFFFFFF)
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
			else if (instruction == IRInstruction.LogicalOr32 && op1.IsConstant && op1.ConstantUnsignedInteger == 0xFFFFFFFF)
			{
				return ConstantOperand.Create(result.Type, 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.LogicalOr64 && op1.IsConstant && op1.ConstantUnsignedLongInteger == 0xFFFFFFFFFFFFFFFF)
			{
				return ConstantOperand.Create(result.Type, 0xFFFFFFFFFFFFFFFF);
			}
			else if (instruction == IRInstruction.LogicalOr32 && op2.IsConstant && op2.ConstantUnsignedInteger == 0xFFFFFFFF)
			{
				return ConstantOperand.Create(result.Type, 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.LogicalOr64 && op2.IsConstant && op2.ConstantUnsignedLongInteger == 0xFFFFFFFFFFFFFFFF)
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
			else if (instruction == IRInstruction.CompareInt32x32 || instruction == IRInstruction.CompareInt64x32 || instruction == IRInstruction.CompareInt64x64)
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
			if (node.Instruction != IRInstruction.Phi)
				return null;

			if (node.OperandCount == 1 && node.Result.IsInteger)
			{
				return new SimpleInstruction(GetMoveInteger(node.Result), node.Result, node.Operand1);
			}

			return null;
		}

		public static SimpleInstruction ArithmeticSimplificationMultiplication(InstructionNode node)
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

			if (IsPowerOfTwo(op2.ConstantUnsignedLongInteger))
			{
				int shift = GetPowerOfTwo(op2.ConstantUnsignedLongInteger);

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

		public static SimpleInstruction ArithmeticSimplificationDivision(InstructionNode node)
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

			if ((node.Instruction == IRInstruction.DivUnsigned32 || node.Instruction == IRInstruction.DivUnsigned64) && IsPowerOfTwo(op2.ConstantUnsignedLongInteger))
			{
				int shift = GetPowerOfTwo(op2.ConstantUnsignedLongInteger);

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
			}

			return null;
		}

		public static SimpleInstruction ArithmeticSimplificationRemUnsignedModulus(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.RemUnsigned32
				|| node.Instruction == IRInstruction.RemUnsigned64))
				return null;

			var result = node.Result;
			var op1 = node.Operand1;
			var op2 = node.Operand2;

			if (!op2.IsResolvedConstant)
				return null;

			if (op2.ConstantUnsignedLongInteger == 0)
				return null;

			if (!IsPowerOfTwo(op2.ConstantUnsignedLongInteger))
				return null;

			int power = GetPowerOfTwo(op2.ConstantUnsignedLongInteger);

			var mask = (1 << power) - 1;

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

			return new SimpleInstruction()
			{
				Instruction = IRInstruction.Nop
			};
		}

		public static SimpleInstruction ConstantFoldingAndStrengthReductionInteger(InstructionNode node)
		{
			var operand = ConstantFoldingAndStrengthReductionIntegerToOperand(node);

			if (operand == null)
				return null;

			return new SimpleInstruction()
			{
				Instruction = GetMoveInteger(node.Result),
				Result = node.Result,
				Operand1 = operand
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

		private static bool IsPowerOfTwo(ulong n)
		{
			return (n & (n - 1)) == 0;
		}

		private static int GetPowerOfTwo(ulong n)
		{
			int bits = 0;
			while (n > 0)
			{
				bits++;
				n >>= 1;
			}

			return bits - 1;
		}

		private static bool ValidateSSAForm(Operand operand)
		{
			return operand.Definitions.Count == 1;
		}

		public static BaseInstruction Select(bool is64bit, BaseInstruction instruction32, BaseInstruction instruction64)
		{
			return !is64bit ? instruction32 : instruction64;
		}

		private static BaseInstruction GetMoveInteger(Operand operand)
		{
			return operand.Is64BitInteger ? (BaseInstruction)IRInstruction.MoveInt64 : IRInstruction.MoveInt32;
		}

		#endregion Helpers
	}
}
