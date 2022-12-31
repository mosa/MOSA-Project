// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;
using System.Diagnostics;

namespace Mosa.Platform.ARMv8A32
{
	public static class ARMv8A32TransformHelper
	{
		#region Helper Methods

		public static void ExpandMul(Context context, TransformContext transform)
		{
			transform.SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			transform.SplitLongOperand(context.Operand1, out var op1L, out var op1H);
			transform.SplitLongOperand(context.Operand2, out var op2L, out var op2H);

			var v1 = transform.AllocateVirtualRegister32();
			var v2 = transform.AllocateVirtualRegister32();

			op1L = MoveConstantToRegister(context, op1L, transform);
			op1H = MoveConstantToRegister(context, op1H, transform);
			op2L = MoveConstantToRegister(context, op2L, transform);
			op2H = MoveConstantToRegister(context, op2H, transform);

			//umull		low, v1 <= op1l, op2l
			//mla		v2, <= op1l, op2h, v1
			//mla		high, <= op1h, op2l, v2

			context.SetInstruction2(ARMv8A32.UMull, v1, resultLow, op1L, op2L);
			context.AppendInstruction(ARMv8A32.Mla, v2, op1L, op2H, v1);
			context.AppendInstruction(ARMv8A32.Mla, resultHigh, op1H, op2L, v2);
		}

		public static void MoveConstantRightForComparison(Context context)
		{
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			if (operand2.IsConstant || operand1.IsVirtualRegister)
				return;

			// Move constant to the right
			context.Operand1 = operand2;
			context.Operand2 = operand1;
			context.ConditionCode = context.ConditionCode.GetReverse();
		}

		public static void Translate(Context context, BaseInstruction instruction, bool allowImmediate, TransformContext transform)
		{
			var result = context.Result;
			var operand1 = context.Operand1;

			if (context.OperandCount == 1)
			{
				operand1 = operand1.IsFloatingPoint
					? MoveConstantToFloatRegisterOrImmediate(context, operand1, allowImmediate, transform)
					: MoveConstantToRegisterOrImmediate(context, operand1, allowImmediate, transform);

				context.SetInstruction(instruction, result, operand1);
			}
			else if (context.OperandCount == 2)
			{
				var operand2 = context.Operand2;

				operand1 = MoveConstantToRegister(context, operand1, transform);

				operand2 = operand2.IsFloatingPoint
					? MoveConstantToFloatRegisterOrImmediate(context, operand2, allowImmediate, transform)
					: MoveConstantToRegisterOrImmediate(context, operand2, allowImmediate, transform);

				context.SetInstruction(instruction, result, operand1, operand2);
			}
		}

		public static void MoveConstantRight(Context context, TransformContext transform)
		{
			Debug.Assert(context.OperandCount == 2);
			Debug.Assert(context.Instruction.IsCommutative);

			var operand1 = context.Operand1;

			if (operand1.IsConstant && context.Instruction.IsCommutative)
			{
				var operand2 = context.Operand2;

				context.Operand1 = operand2;
				context.Operand2 = operand1;
			}
		}

		public static void TransformLoad(Context context, BaseInstruction loadInstruction, Operand result, Operand baseOperand, Operand offsetOperand, TransformContext transform)
		{
			baseOperand = MoveConstantToRegister(context, baseOperand, transform);
			bool upDirection = true;

			if (offsetOperand.IsResolvedConstant)
			{
				if (offsetOperand.ConstantUnsigned64 >= 0 && offsetOperand.ConstantSigned32 <= 0xFFF)
				{
					// Nothing
				}
				else if (offsetOperand.ConstantUnsigned64 < 0 && -offsetOperand.ConstantSigned32 <= 0xFFF)
				{
					upDirection = false;
					offsetOperand = transform.CreateConstant32(-offsetOperand.ConstantSigned32);
				}
				else
				{
					offsetOperand = MoveConstantToRegister(context, offsetOperand, transform);
				}
			}
			else if (offsetOperand.IsUnresolvedConstant)
			{
				offsetOperand = MoveConstantToRegister(context, offsetOperand, transform);
			}

			context.SetInstruction(loadInstruction, upDirection ? StatusRegister.UpDirection : StatusRegister.DownDirection, result, baseOperand, offsetOperand);
		}

		public static void TransformStore(Context context, BaseInstruction storeInstruction, Operand baseOperand, Operand offsetOperand, Operand sourceOperand, TransformContext transform)
		{
			baseOperand = MoveConstantToRegister(context, baseOperand, transform);
			sourceOperand = MoveConstantToRegister(context, sourceOperand, transform);

			bool upDirection = true;

			if (offsetOperand.IsResolvedConstant)
			{
				if (offsetOperand.ConstantUnsigned64 >= 0 && offsetOperand.ConstantSigned32 <= 0xFFF)
				{
					// Nothing
				}
				else if (offsetOperand.ConstantUnsigned64 < 0 && -offsetOperand.ConstantSigned32 <= 0xFFF)
				{
					offsetOperand = transform.CreateConstant32(-offsetOperand.ConstantSigned32);
				}
				else
				{
					upDirection = false;
					offsetOperand = MoveConstantToRegister(context, offsetOperand, transform);
				}
			}
			else if (offsetOperand.IsUnresolvedConstant)
			{
				offsetOperand = MoveConstantToRegister(context, offsetOperand, transform);
			}

			context.SetInstruction(storeInstruction, upDirection ? StatusRegister.UpDirection : StatusRegister.DownDirection, null, baseOperand, offsetOperand, sourceOperand);
		}

		public static Operand MoveConstantToRegister(Context context, Operand operand, TransformContext transform)
		{
			return MoveConstantToRegisterOrImmediate(context, operand, false, transform);
		}

		public static Operand MoveConstantToRegisterOrImmediate(Context context, Operand operand, TransformContext transform)
		{
			return MoveConstantToRegisterOrImmediate(context, operand, true, transform);
		}

		public static Operand MoveConstantToRegisterOrImmediate(Context context, Operand operand, bool allowImmediate, TransformContext transform)
		{
			if (operand.IsVirtualRegister || operand.IsCPURegister)
				return operand;

			if (operand.IsResolvedConstant)
			{
				if (ARMHelper.CalculateRotatedImmediateValue(operand.ConstantUnsigned32, out uint immediate, out byte _, out byte _))
				{
					var constant = transform.CreateConstant32(immediate);

					if (allowImmediate)
						return constant;

					var before = context.InsertBefore();
					var v1 = transform.AllocateVirtualRegister32();
					before.SetInstruction(ARMv8A32.Mov, v1, constant);

					return v1;
				}

				if (ARMHelper.CalculateRotatedImmediateValue(~operand.ConstantUnsigned32, out uint immediate2, out byte _, out byte _))
				{
					var constant = transform.CreateConstant32(immediate);

					var before = context.InsertBefore();
					var v1 = transform.AllocateVirtualRegister32();
					before.SetInstruction(ARMv8A32.Mov, v1, constant);

					return v1;
				}

				{
					var before = context.InsertBefore();

					var v1 = transform.AllocateVirtualRegister32();
					before.SetInstruction(ARMv8A32.Movw, v1, transform.CreateConstant32(operand.ConstantUnsigned32 & 0xFFFF));
					before.AppendInstruction(ARMv8A32.Movt, v1, v1, transform.CreateConstant32(operand.ConstantUnsigned32 >> 16));

					return v1;
				}
			}
			else if (operand.IsUnresolvedConstant)
			{
				var before = context.InsertBefore();
				var v1 = transform.AllocateVirtualRegister32();
				before.SetInstruction(ARMv8A32.Movw, v1, operand);
				before.AppendInstruction(ARMv8A32.Movt, v1, v1, operand);

				return v1;
			}

			throw new CompilerException("Error at {context} in {Method}");
		}

		public static Operand MoveConstantToFloatRegisterOrImmediate(Context context, Operand operand, TransformContext transform)
		{
			return MoveConstantToFloatRegisterOrImmediate(context, operand, true, transform);
		}

		public static Operand MoveConstantToFloatRegister(Context context, Operand operand, TransformContext transform)
		{
			return MoveConstantToFloatRegisterOrImmediate(context, operand, false, transform);
		}

		public static Operand MoveConstantToFloatRegisterOrImmediate(Context context, Operand operand, bool allowImmediate, TransformContext transform)
		{
			if (operand.IsVirtualRegister || operand.IsCPURegister)
				return operand;

			if (allowImmediate)
			{
				var immediate = ConvertFloatToImm(operand, transform);

				if (immediate != null)
					return immediate;
			}

			// FUTURE: Load float bits (not double) into integer register, than fmov them into the floating point register (saves a memory load)

			var v1 = operand.IsR4 ? transform.AllocateVirtualRegisterR4() : transform.AllocateVirtualRegisterR8();
			var symbol = operand.IsR4 ? transform.Linker.GetConstantSymbol((float)operand.ConstantUnsigned64) : transform.Linker.GetConstantSymbol((double)operand.ConstantUnsigned64);
			var label = Operand.CreateLabel(v1.Type, symbol.Name);

			context.InsertBefore().SetInstruction(ARMv8A32.Ldf, v1, label, transform.Constant32_0);

			return v1;
		}

		public static Operand ConvertFloatToImm(Operand operand, TransformContext transform)
		{
			if (operand.IsCPURegister || operand.IsVirtualRegister || operand.IsUnresolvedConstant)
				return operand;

			if (operand.IsR4)
			{
				var value = operand.ConstantFloat;

				switch (value)
				{
					case 0.0f: return transform.CreateConstant32(0b1000);
					case 1.0f: return transform.CreateConstant32(0b1001);
					case 2.0f: return transform.CreateConstant32(0b1010);
					case 3.0f: return transform.CreateConstant32(0b1011);
					case 4.0f: return transform.CreateConstant32(0b1100);
					case 5.0f: return transform.CreateConstant32(0b1101);
					case 0.5f: return transform.CreateConstant32(0b1110);
					case 10.0f: return transform.CreateConstant32(0b1111);
				}
			}
			else if (operand.IsR4)
			{
				var value = operand.ConstantDouble;

				switch (value)
				{
					case 0.0d: return transform.CreateConstant32(0b1000);
					case 1.0d: return transform.CreateConstant32(0b1001);
					case 2.0d: return transform.CreateConstant32(0b1010);
					case 3.0d: return transform.CreateConstant32(0b1011);
					case 4.0d: return transform.CreateConstant32(0b1100);
					case 5.0d: return transform.CreateConstant32(0b1101);
					case 0.5d: return transform.CreateConstant32(0b1110);
					case 10.0d: return transform.CreateConstant32(0b1111);
				}
			}

			return null;
		}

		#endregion Helper Methods
	}
}
