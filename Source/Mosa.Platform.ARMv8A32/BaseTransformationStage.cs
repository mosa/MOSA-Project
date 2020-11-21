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

		protected Operand Constant_0;
		protected Operand Constant_1;
		protected Operand Constant_2;
		protected Operand Constant_3;
		protected Operand Constant_4;
		protected Operand Constant_16;
		protected Operand Constant_24;
		protected Operand Constant_31;
		protected Operand Constant_32;
		protected Operand Constant_64;

		protected Operand Constant_1F;

		protected Operand LSL;
		protected Operand LSR;
		protected Operand ASR;
		protected Operand ROR;

		protected override void Setup()
		{
			Constant_0 = CreateConstant32(1);
			Constant_1 = CreateConstant32(1);
			Constant_2 = CreateConstant32(2);
			Constant_3 = CreateConstant32(3);
			Constant_4 = CreateConstant32(4);
			Constant_16 = CreateConstant32(16);
			Constant_24 = CreateConstant32(16);
			Constant_31 = CreateConstant32(31);
			Constant_32 = CreateConstant32(32);
			Constant_64 = CreateConstant32(64);
			Constant_1F = Constant_31;

			LSL = Constant_0;
			LSR = Constant_1;
			ASR = Constant_2;
			ROR = Constant_3;
		}

		#region Helper Methods

		protected static void MoveConstantRight(Context context)
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

		protected void TransformLoad(Context context, BaseInstruction loadInstruction, Operand result, Operand baseOperand, Operand offsetOperand)
		{
			baseOperand = MoveConstantToRegister(context, baseOperand);
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
					offsetOperand = CreateConstant32(-offsetOperand.ConstantSigned32);
				}
				else
				{
					offsetOperand = MoveConstantToRegister(context, offsetOperand);
				}
			}
			else if (offsetOperand.IsUnresolvedConstant)
			{
				offsetOperand = MoveConstantToRegister(context, offsetOperand);
			}

			context.SetInstruction(loadInstruction, upDirection ? StatusRegister.UpDirection : StatusRegister.DownDirection, result, baseOperand, offsetOperand);
		}

		protected void TransformStore(Context context, BaseInstruction storeInstruction, Operand baseOperand, Operand offsetOperand, Operand sourceOperand)
		{
			baseOperand = MoveConstantToRegister(context, baseOperand);
			sourceOperand = MoveConstantToRegister(context, sourceOperand);

			bool upDirection = true;

			if (offsetOperand.IsResolvedConstant)
			{
				if (offsetOperand.ConstantUnsigned64 >= 0 && offsetOperand.ConstantSigned32 <= 0xFFF)
				{
					// Nothing
				}
				else if (offsetOperand.ConstantUnsigned64 < 0 && -offsetOperand.ConstantSigned32 <= 0xFFF)
				{
					offsetOperand = CreateConstant32(-offsetOperand.ConstantSigned32);
				}
				else
				{
					upDirection = false;
					offsetOperand = MoveConstantToRegister(context, offsetOperand);
				}
			}
			else if (offsetOperand.IsUnresolvedConstant)
			{
				offsetOperand = MoveConstantToRegister(context, offsetOperand);
			}

			context.SetInstruction(storeInstruction, upDirection ? StatusRegister.UpDirection : StatusRegister.DownDirection, null, baseOperand, offsetOperand, sourceOperand);
		}

		protected Operand MoveConstantToRegister(Context context, Operand operand)
		{
			return MoveConstantToRegisterOrImmediate(context, operand, false);
		}

		protected Operand MoveConstantToRegisterOrImmediate(Context context, Operand operand)
		{
			return MoveConstantToRegisterOrImmediate(context, operand, true);
		}

		protected Operand MoveConstantToRegisterOrImmediate(Context context, Operand operand, bool allowImmediate)
		{
			if (operand.IsVirtualRegister || operand.IsCPURegister)
				return operand;

			if (operand.IsResolvedConstant)
			{
				if (ARMHelper.CalculateRotatedImmediateValue(operand.ConstantUnsigned32, out uint immediate, out byte _, out byte _))
				{
					var constant = CreateConstant32(immediate);

					if (allowImmediate)
						return constant;

					var before = context.InsertBefore();
					var v1 = AllocateVirtualRegisterI32();
					before.SetInstruction(ARMv8A32.Mov, v1, constant);

					return v1;
				}

				if (ARMHelper.CalculateRotatedImmediateValue(~operand.ConstantUnsigned32, out uint immediate2, out byte _, out byte _))
				{
					var constant = CreateConstant32(immediate);

					var before = context.InsertBefore();
					var v1 = AllocateVirtualRegisterI32();
					before.SetInstruction(ARMv8A32.Mov, v1, constant);

					return v1;
				}

				{
					var before = context.InsertBefore();

					var v1 = AllocateVirtualRegisterI32();
					before.SetInstruction(ARMv8A32.Movw, v1, CreateConstant32(operand.ConstantUnsigned32 & 0xFFFF));
					before.AppendInstruction(ARMv8A32.Movt, v1, v1, CreateConstant32(operand.ConstantUnsigned32 >> 16));

					return v1;
				}
			}
			else if (operand.IsUnresolvedConstant)
			{
				var before = context.InsertBefore();
				var v1 = AllocateVirtualRegisterI32();
				before.SetInstruction(ARMv8A32.Movw, v1, operand);
				before.AppendInstruction(ARMv8A32.Movt, v1, v1, operand);

				return v1;
			}

			throw new CompilerException("Error at {context} in {Method}");
		}

		protected Operand MoveConstantToFloatRegisterOrImmediate(Context context, Operand operand)
		{
			return MoveConstantToFloatRegisterOrImmediate(context, operand, true);
		}

		protected Operand MoveConstantToFloatRegister(Context context, Operand operand)
		{
			return MoveConstantToFloatRegisterOrImmediate(context, operand, false);
		}

		protected Operand MoveConstantToFloatRegisterOrImmediate(Context context, Operand operand, bool allowImmediate)
		{
			if (operand.IsVirtualRegister || operand.IsCPURegister)
				return operand;

			if (allowImmediate)
			{
				var immediate = ConvertFloatToImm(operand);

				if (immediate != null)
					return immediate;
			}

			// FUTURE: Load float bits (not double) into integer register, than fmov them into the floating point register (saves a memory load)

			var v1 = operand.IsR4 ? AllocateVirtualRegisterR4() : AllocateVirtualRegisterR8();

			var symbol = operand.IsR4 ? Linker.GetConstantSymbol((float)operand.ConstantUnsigned64) : Linker.GetConstantSymbol((double)operand.ConstantUnsigned64);

			var label = Operand.CreateLabel(v1.Type, symbol.Name);

			context.InsertBefore().SetInstruction(ARMv8A32.Ldf, v1, label, Constant_0);

			return v1;
		}

		private Operand ConvertFloatToImm(Operand operand)
		{
			if (operand.IsCPURegister || operand.IsVirtualRegister || operand.IsUnresolvedConstant)
				return operand;

			if (operand.IsR4)
			{
				var value = operand.ConstantFloat;

				switch (value)
				{
					case 0.0f: return CreateConstant32(0b1000);
					case 1.0f: return CreateConstant32(0b1001);
					case 2.0f: return CreateConstant32(0b1010);
					case 3.0f: return CreateConstant32(0b1011);
					case 4.0f: return CreateConstant32(0b1100);
					case 5.0f: return CreateConstant32(0b1101);
					case 0.5f: return CreateConstant32(0b1110);
					case 10.0f: return CreateConstant32(0b1111);
				}
			}
			else if (operand.IsR4)
			{
				var value = operand.ConstantDouble;

				switch (value)
				{
					case 0.0d: return CreateConstant32(0b1000);
					case 1.0d: return CreateConstant32(0b1001);
					case 2.0d: return CreateConstant32(0b1010);
					case 3.0d: return CreateConstant32(0b1011);
					case 4.0d: return CreateConstant32(0b1100);
					case 5.0d: return CreateConstant32(0b1101);
					case 0.5d: return CreateConstant32(0b1110);
					case 10.0d: return CreateConstant32(0b1111);
				}
			}

			return null;
		}

		#endregion Helper Methods
	}
}
