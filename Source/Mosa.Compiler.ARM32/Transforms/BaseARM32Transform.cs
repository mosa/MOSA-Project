// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms
{
	public abstract class BaseARM32Transform : BaseTransform
	{
		public BaseARM32Transform(BaseInstruction instruction, TransformType type, bool log = false)
			: base(instruction, type, log)
		{ }

		#region Helpers

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

		public static void Translate(Transform transform, Context context, BaseInstruction instruction, bool allowImmediate)
		{
			var result = context.Result;
			var operand1 = context.Operand1;

			if (context.OperandCount == 1)
			{
				operand1 = operand1.IsFloatingPoint
					? MoveConstantToFloatRegisterOrImmediate(transform, context, operand1, allowImmediate)
					: MoveConstantToRegisterOrImmediate(transform, context, operand1, allowImmediate);

				context.SetInstruction(instruction, result, operand1);
			}
			else if (context.OperandCount == 2)
			{
				var operand2 = context.Operand2;

				operand1 = MoveConstantToRegister(transform, context, operand1);

				operand2 = operand2.IsFloatingPoint
					? MoveConstantToFloatRegisterOrImmediate(transform, context, operand2, allowImmediate)
					: MoveConstantToRegisterOrImmediate(transform, context, operand2, allowImmediate);

				context.SetInstruction(instruction, result, operand1, operand2);
			}
		}

		public static void TransformLoad(Transform transform, Context context, BaseInstruction loadInstruction, Operand result, Operand baseOperand, Operand offsetOperand)
		{
			baseOperand = MoveConstantToRegister(transform, context, baseOperand);
			offsetOperand = LimitOffsetToRange(transform, context, offsetOperand, 12, out var upDirection);

			context.SetInstruction(loadInstruction, upDirection ? InstructionOption.UpDirection : InstructionOption.None, result, baseOperand, offsetOperand);
		}

		public static void TransformStore(Transform transform, Context context, BaseInstruction storeInstruction, Operand baseOperand, Operand offsetOperand, Operand sourceOperand)
		{
			sourceOperand = MoveConstantToRegister(transform, context, sourceOperand);
			baseOperand = MoveConstantToRegister(transform, context, baseOperand);
			offsetOperand = LimitOffsetToRange(transform, context, offsetOperand, 12, out var upDirection);

			context.SetInstruction(storeInstruction, upDirection ? InstructionOption.UpDirection : InstructionOption.None, null, baseOperand, offsetOperand, sourceOperand);
		}

		public static void TransformFloatingPointLoad(Transform transform, Context context, BaseInstruction loadInstruction, Operand result, Operand baseOperand, Operand offsetOperand)
		{
			if (baseOperand.IsConstant && offsetOperand.IsVirtualRegister)
			{
				// swap
				var tmp = baseOperand;
				baseOperand = offsetOperand;
				offsetOperand = tmp;
			}

			baseOperand = MoveConstantToRegister(transform, context, baseOperand);
			offsetOperand = LimitOffsetToRange(transform, context, offsetOperand, 8, out var upDirection);

			ConformBaseOffsetToContant(transform, context, ref baseOperand, ref offsetOperand);

			context.SetInstruction(loadInstruction, upDirection ? InstructionOption.UpDirection : InstructionOption.None, result, baseOperand, offsetOperand);
		}

		public static void TransformFloatingPointStore(Transform transform, Context context, BaseInstruction storeInstruction, Operand baseOperand, Operand offsetOperand, Operand sourceOperand)
		{
			if (baseOperand.IsConstant && offsetOperand.IsVirtualRegister)
			{
				// swap
				var tmp = baseOperand;
				baseOperand = offsetOperand;
				offsetOperand = tmp;
			}

			sourceOperand = MoveConstantToFloatRegister(transform, context, sourceOperand);
			baseOperand = MoveConstantToRegister(transform, context, baseOperand);
			offsetOperand = LimitOffsetToRange(transform, context, offsetOperand, 8, out var upDirection);

			ConformBaseOffsetToContant(transform, context, ref baseOperand, ref offsetOperand);

			context.SetInstruction(storeInstruction, upDirection ? InstructionOption.UpDirection : InstructionOption.None, null, baseOperand, offsetOperand, sourceOperand);
		}

		public static Operand MoveConstantToRegister(Transform transform, Context context, Operand operand)
		{
			return MoveConstantToRegisterOrImmediate(transform, context, operand, false);
		}

		public static Operand MoveConstantToRegisterOrImmediate(Transform transform, Context context, Operand operand)
		{
			return MoveConstantToRegisterOrImmediate(transform, context, operand, true);
		}

		public static Operand MoveConstantToRegisterOrImmediate(Transform transform, Context context, Operand operand, bool allowImmediate)
		{
			if (operand.IsVirtualRegister || operand.IsPhysicalRegister)
				return operand;

			if (operand.IsResolvedConstant)
			{
				if (ARMHelper.CalculateRotatedImmediateValue(operand.ConstantUnsigned32, out uint immediate, out byte _, out byte _))
				{
					var constant = Operand.CreateConstant32(immediate);

					if (allowImmediate)
						return constant;

					var before = context.InsertBefore();
					var v1 = transform.VirtualRegisters.Allocate32();
					before.SetInstruction(ARM32.Mov, v1, constant);

					return v1;
				}

				if (ARMHelper.CalculateRotatedImmediateValue(~operand.ConstantUnsigned32, out uint immediate2, out byte _, out byte _))
				{
					var constant = Operand.CreateConstant32(immediate);

					var before = context.InsertBefore();
					var v1 = transform.VirtualRegisters.Allocate32();
					before.SetInstruction(ARM32.Mov, v1, constant);

					return v1;
				}

				{
					var before = context.InsertBefore();

					var v1 = transform.VirtualRegisters.Allocate32();

					var low = operand.ConstantUnsigned32 & 0xFFFF;
					var high = operand.ConstantUnsigned32 >> 16;

					before.SetInstruction(ARM32.Movw, v1, Operand.CreateConstant32(low));

					if (high != 0)
					{
						before.AppendInstruction(ARM32.Movt, v1, v1, Operand.CreateConstant32(high));
					}

					return v1;
				}
			}
			else if (operand.IsUnresolvedConstant)
			{
				var before = context.InsertBefore();

				var v1 = transform.VirtualRegisters.Allocate32();

				before.SetInstruction(ARM32.Movw, v1, operand);
				before.AppendInstruction(ARM32.Movt, v1, v1, operand);

				return v1;
			}

			throw new CompilerException("Error at {context} in {Method}");
		}

		public static Operand MoveConstantToFloatRegisterOrImmediate(Transform transform, Context context, Operand operand)
		{
			return MoveConstantToFloatRegisterOrImmediate(transform, context, operand, true);
		}

		public static Operand MoveConstantToFloatRegister(Transform transform, Context context, Operand operand)
		{
			return MoveConstantToFloatRegisterOrImmediate(transform, context, operand, false);
		}

		public static Operand MoveConstantToFloatRegisterOrImmediate(Transform transform, Context context, Operand operand, bool allowImmediate)
		{
			if (operand.IsVirtualRegister || operand.IsPhysicalRegister)
				return operand;

			if (allowImmediate)
			{
				var immediate = ConvertFloatToImm(transform, operand);

				if (immediate != null)
					return immediate;
			}

			// FUTURE: Load float bits (not double) into integer register, than fmov them into the floating point register (saves a memory load)

			var v1 = operand.IsR4
				? transform.VirtualRegisters.AllocateR4()
				: transform.VirtualRegisters.AllocateR8();

			var symbol = operand.IsR4
				? transform.Linker.GetConstantSymbol(operand.ConstantFloat)
				: transform.Linker.GetConstantSymbol(operand.ConstantDouble);

			var label = operand.IsR4
				? Operand.CreateLabelR4(symbol.Name)
				: Operand.CreateLabelR8(symbol.Name);

			var source = MoveConstantToRegister(transform, context, label);

			context.InsertBefore().SetInstruction(ARM32.VLdr, v1, source, Operand.Constant32_0);

			return v1;
		}

		public static Operand ConvertFloatToImm(Transform transform, Operand operand)
		{
			if (operand.IsPhysicalRegister || operand.IsVirtualRegister || operand.IsUnresolvedConstant)
				return operand;

			if (operand.IsR4)
			{
				var value = operand.ConstantFloat;

				switch (value)
				{
					case 0.0f: return Operand.Constant32_0b1000;
						//case 1.0f: return Operand.Constant32_0b1001;
						//case 2.0f: return Operand.Constant32_0b1010;
						//case 3.0f: return Operand.Constant32_0b1011;
						//case 4.0f: return Operand.Constant32_0b1100;
						//case 5.0f: return Operand.Constant32_0b1101;
						//case 0.5f: return Operand.Constant32_0b1110;
						//case 10.0f: return Operand.Constant32_0b1111;
				}
			}
			else if (operand.IsR4)
			{
				var value = operand.ConstantDouble;

				switch (value)
				{
					case 0.0d: return Operand.Constant32_0b1000;
						//case 1.0d: return Operand.Constant32_0b1001;
						//case 2.0d: return Operand.Constant32_0b1010;
						//case 3.0d: return Operand.Constant32_0b1011;
						//case 4.0d: return Operand.Constant32_0b1100;
						//case 5.0d: return Operand.Constant32_0b1101;
						//case 0.5d: return Operand.Constant32_0b1110;
						//case 10.0d: return Operand.Constant32_0b1111;
				}
			}

			return null;
		}

		public static void MoveConstantRight(Transform transform, Context context)
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

		private static Operand LimitOffsetToRange(Transform transform, Context context, Operand offsetOperand, int bits, out bool upDirection)
		{
			var mask = 1 << (bits + 1) - 1;

			// bits == 4 then mask == 0xFFF

			upDirection = true;

			if (offsetOperand.IsResolvedConstant)
			{
				if (offsetOperand.ConstantUnsigned64 >= 0 && offsetOperand.ConstantSigned32 <= mask)
				{
					// Nothing
				}
				else if (offsetOperand.ConstantUnsigned64 < 0 && -offsetOperand.ConstantSigned32 <= mask)
				{
					offsetOperand = Operand.CreateConstant32(-offsetOperand.ConstantSigned32);
				}
				else
				{
					upDirection = false;
					offsetOperand = MoveConstantToRegister(transform, context, offsetOperand);
				}
			}
			else if (offsetOperand.IsUnresolvedConstant)
			{
				offsetOperand = MoveConstantToRegister(transform, context, offsetOperand);
			}

			return offsetOperand;
		}

		private static void ConformBaseOffsetToContant(Transform transform, Context context, ref Operand baseOperand, ref Operand offsetOperand)
		{
			if (offsetOperand.IsVirtualRegister)
			{
				var before = context.InsertBefore();
				var v1 = transform.VirtualRegisters.Allocate32();
				before.SetInstruction(ARM32.Add, v1, baseOperand, offsetOperand);

				baseOperand = v1;
				offsetOperand = Operand.Constant32_0;
			}
		}

		#endregion Helpers
	}
}
