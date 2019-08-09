// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;
using System;
using System.Diagnostics;

namespace Mosa.Platform.ARMv8A32.Stages
{
	/// <summary>
	/// Transforms IR instructions into their appropriate ARMv8A32.
	/// </summary>
	/// <remarks>
	/// This transformation stage transforms IR instructions into their equivalent ARMv8A32 sequences.
	/// </remarks>
	public sealed class IRTransformationStage : BaseTransformationStage
	{
		public const int LogicalLeft = 0b00;
		public const int LogicalRight = 0b01;
		public const int ArithShiftRight = 0b10;
		public const int RotateRight = 0b11;

		protected override void PopulateVisitationDictionary()
		{
			//AddVisitation(IRInstruction.AddFloatR4, AddFloatR4);
			//AddVisitation(IRInstruction.AddFloatR8, AddFloatR8);
			//AddVisitation(IRInstruction.AddressOf, AddressOf);
			AddVisitation(IRInstruction.Add32, Add32);
			AddVisitation(IRInstruction.AddCarryOut32, AddCarryOut32);
			AddVisitation(IRInstruction.AddWithCarry32, AddWithCarry32);

			AddVisitation(IRInstruction.ArithShiftRight32, ArithShiftRight32);

			//AddVisitation(IRInstruction.BitCopyFloatR4ToInt32, BitCopyFloatR4ToInt32);
			//AddVisitation(IRInstruction.BitCopyInt32ToFloatR4, BitCopyInt32ToFloatR4);
			//AddVisitation(IRInstruction.Break, Break);
			//AddVisitation(IRInstruction.CallDirect, CallDirect);
			//AddVisitation(IRInstruction.CompareFloatR4, CompareFloatR4);
			//AddVisitation(IRInstruction.CompareFloatR8, CompareFloatR8);
			//AddVisitation(IRInstruction.CompareInt32x32, CompareInt32x32);
			//AddVisitation(IRInstruction.CompareIntBranch32, CompareIntBranch32);
			//AddVisitation(IRInstruction.IfThenElse32, IfThenElse32);
			//AddVisitation(IRInstruction.LoadCompound, LoadCompound);
			//AddVisitation(IRInstruction.MoveCompound, MoveCompound);
			//AddVisitation(IRInstruction.StoreCompound, StoreCompound);
			//AddVisitation(IRInstruction.ConvertFloatR4ToFloatR8, ConvertFloatR4ToFloatR8);
			//AddVisitation(IRInstruction.ConvertFloatR8ToFloatR4, ConvertFloatR8ToFloatR4);
			//AddVisitation(IRInstruction.ConvertFloatR4ToInt32, ConvertFloatR4ToInt32);
			//AddVisitation(IRInstruction.ConvertFloatR8ToInt32, ConvertFloatR8ToInt32);
			//AddVisitation(IRInstruction.ConvertInt32ToFloatR4, ConvertInt32ToFloatR4);
			//AddVisitation(IRInstruction.ConvertInt32ToFloatR8, ConvertInt32ToFloatR8);
			//AddVisitation(IRInstruction.DivFloatR4, DivFloatR4);
			//AddVisitation(IRInstruction.DivFloatR8, DivFloatR8);
			//AddVisitation(IRInstruction.DivSigned32, DivSigned32);
			//AddVisitation(IRInstruction.DivUnsigned32, DivUnsigned32);
			//AddVisitation(IRInstruction.Jmp, Jmp);
			//AddVisitation(IRInstruction.LoadFloatR4, LoadFloatR4);
			//AddVisitation(IRInstruction.LoadFloatR8, LoadFloatR8);
			//AddVisitation(IRInstruction.LoadInt32, LoadInt32);
			//AddVisitation(IRInstruction.LoadSignExtend8x32, LoadSignExtend8x32);
			//AddVisitation(IRInstruction.LoadSignExtend16x32, LoadSignExtend16x32);
			//AddVisitation(IRInstruction.LoadZeroExtend8x32, LoadZeroExtend8x32);
			//AddVisitation(IRInstruction.LoadZeroExtend16x32, LoadZeroExtend16x32);
			//AddVisitation(IRInstruction.LoadParamFloatR4, LoadParamFloatR4);
			//AddVisitation(IRInstruction.LoadParamFloatR8, LoadParamFloatR8);
			//AddVisitation(IRInstruction.LoadParamInt32, LoadParamInt32);
			//AddVisitation(IRInstruction.LoadParamSignExtend8x32, LoadParamSignExtend8x32);
			//AddVisitation(IRInstruction.LoadParamSignExtend16x32, LoadParamSignExtend16x32);
			//AddVisitation(IRInstruction.LoadParamZeroExtend8x32, LoadParamZeroExtend8x32);
			//AddVisitation(IRInstruction.LoadParamZeroExtend16x32, LoadParamZeroExtend16x32);
			//AddVisitation(IRInstruction.LoadParamCompound, LoadParamCompound);
			AddVisitation(IRInstruction.LogicalAnd32, LogicalAnd32);
			AddVisitation(IRInstruction.LogicalNot32, LogicalNot32);
			AddVisitation(IRInstruction.LogicalOr32, LogicalOr32);
			AddVisitation(IRInstruction.LogicalXor32, LogicalXor32);

			//AddVisitation(IRInstruction.MoveFloatR4, MoveFloatR4);
			//AddVisitation(IRInstruction.MoveFloatR8, MoveFloatR8);
			AddVisitation(IRInstruction.MoveInt32, MoveInt32);

			//AddVisitation(IRInstruction.SignExtend8x32, SignExtend8x32);
			//AddVisitation(IRInstruction.SignExtend16x32, SignExtend16x32);
			//AddVisitation(IRInstruction.ZeroExtend8x32, ZeroExtend8x32);
			//AddVisitation(IRInstruction.ZeroExtend16x32, ZeroExtend16x32);
			//AddVisitation(IRInstruction.MulFloatR4, MulFloatR4);
			//AddVisitation(IRInstruction.MulFloatR8, MulFloatR8);
			//AddVisitation(IRInstruction.MulSigned32, MulSigned32);
			//AddVisitation(IRInstruction.MulUnsigned32, MulUnsigned32);
			//AddVisitation(IRInstruction.Nop, Nop);
			//AddVisitation(IRInstruction.RemSigned32, RemSigned32);
			//AddVisitation(IRInstruction.RemUnsigned32, RemUnsigned32);
			AddVisitation(IRInstruction.ShiftLeft32, ShiftLeft32);
			AddVisitation(IRInstruction.ShiftRight32, ShiftRight32);

			//AddVisitation(IRInstruction.StoreFloatR4, StoreFloatR4);
			//AddVisitation(IRInstruction.StoreFloatR8, StoreFloatR8);
			//AddVisitation(IRInstruction.StoreInt8, StoreInt8);
			//AddVisitation(IRInstruction.StoreInt16, StoreInt16);
			//AddVisitation(IRInstruction.StoreInt32, StoreInt32);
			//AddVisitation(IRInstruction.StoreParamFloatR4, StoreParamFloatR4);
			//AddVisitation(IRInstruction.StoreParamFloatR8, StoreParamFloatR8);
			//AddVisitation(IRInstruction.StoreParamInt8, StoreParamInt8);
			//AddVisitation(IRInstruction.StoreParamInt16, StoreParamInt16);
			//AddVisitation(IRInstruction.StoreParamInt32, StoreParamInt32);
			//AddVisitation(IRInstruction.StoreParamCompound, StoreParamCompound);
			//AddVisitation(IRInstruction.SubFloatR4, SubFloatR4);
			//AddVisitation(IRInstruction.SubFloatR8, SubFloatR8);
			//AddVisitation(IRInstruction.Sub32, Sub32);
			//AddVisitation(IRInstruction.SubCarryOut32, SubCarryOut32);
			//AddVisitation(IRInstruction.SubWithCarry32, SubWithCarry32);
			//AddVisitation(IRInstruction.Switch, Switch);
		}

		#region Visitation Methods

		private void Add32(Context context)
		{
			SwapFirstTwoOperandsIfFirstConstant(context);
			UpdateInstruction2(context, ARMv8A32.Add, ARMv8A32.AddImm);
		}

		private void AddCarryOut32(Context context)
		{
			var result2 = context.Result2;

			SwapFirstTwoOperandsIfFirstConstant(context);
			UpdateInstruction2(context, ARMv8A32.Add, ARMv8A32.AddImm, StatusRegister.Update);

			context.AppendInstruction(ARMv8A32.MovImm, ConditionCode.Carry, result2, CreateConstant(1));
			context.AppendInstruction(ARMv8A32.MovImm, ConditionCode.NoCarry, result2, CreateConstant(0));
		}

		private void AddWithCarry32(Context context)
		{
			var result = context.Result;
			var operand3 = context.Operand3;

			UpdateInstruction2(context, ARMv8A32.Add, ARMv8A32.AddImm);

			if (operand3.IsVirtualRegister)
			{
				context.AppendInstruction(ARMv8A32.Add, result, result, operand3);
			}
			else if (operand3.IsResolvedConstant)
			{
				context.AppendInstruction(ARMv8A32.AddImm, result, result, operand3);
			}
			else
			{
				throw new CompilerException("Error at {context} in {Method}");
			}
		}

		private void LogicalAnd32(Context context)
		{
			SwapFirstTwoOperandsIfFirstConstant(context);
			UpdateInstruction2(context, ARMv8A32.And, ARMv8A32.AndImm);
		}

		private void LogicalNot32(Context context)
		{
			UpdateInstruction1(context, ARMv8A32.Mvn, ARMv8A32.MvnImm);
		}

		private void LogicalOr32(Context context)
		{
			SwapFirstTwoOperandsIfFirstConstant(context);
			UpdateInstruction2(context, ARMv8A32.Orr, ARMv8A32.OrrImm);
		}

		private void LogicalXor32(Context context)
		{
			SwapFirstTwoOperandsIfFirstConstant(context);
			UpdateInstruction2(context, ARMv8A32.Eor, ARMv8A32.EorImm);
		}

		private void MoveInt32(Context context)
		{
			UpdateInstruction1(context, ARMv8A32.Mov, ARMv8A32.MovImm);
		}

		private void ShiftLeft32(Context context)
		{
			Shift(context, LogicalLeft);
		}

		private void ShiftRight32(Context context)
		{
			Shift(context, LogicalRight);
		}

		private void ArithShiftRight32(Context context)
		{
			Shift(context, ArithShiftRight);
		}

		#endregion Visitation Methods

		private void Shift(Context context, int ShiftType)
		{
			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			if (operand1.IsConstant)
			{
				operand1 = CreateImmediateOperand(context, operand1);
			}

			if (operand2.IsVirtualRegister)
			{
				context.SetInstruction(ARMv8A32.MovRegShift, result, operand1, operand2, CreateConstant(ShiftType));
			}
			else if (operand2.IsResolvedConstant)
			{
				context.SetInstruction(ARMv8A32.MovImmShift, result, operand1, operand2, CreateConstant(ShiftType));
			}
			else if (operand2.IsUnresolvedConstant)
			{
				var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

				context.SetInstruction(ARMv8A32.MovwImm, v1, operand2);
				context.AppendInstruction(ARMv8A32.MovtImm, v1, operand2);

				context.AppendInstruction(ARMv8A32.MovRegShift, result, operand1, v1, CreateConstant(ShiftType));
			}
			else
			{
				throw new CompilerException("Error at {context} in {Method}");
			}
		}

		private void UpdateInstruction1(Context context, BaseInstruction virtualInstruction, BaseInstruction immediateInstruction)
		{
			var result = context.Result;
			var operand1 = context.Operand1;

			if (operand1.IsConstant)
			{
				operand1 = CreateImmediateOperand(context, operand1);
			}

			if (operand1.IsVirtualRegister || operand1.IsCPURegister)
			{
				context.SetInstruction(virtualInstruction, result, operand1);
			}
			else if (operand1.IsResolvedConstant)
			{
				context.SetInstruction(immediateInstruction, result, operand1);
			}
			else
			{
				throw new CompilerException("Error at {context} in {Method}");
			}
		}

		private void UpdateInstruction2(Context context, BaseInstruction virtualInstruction, BaseInstruction immediateInstruction, StatusRegister statusRegister = StatusRegister.NotSet)
		{
			Debug.Assert(context.Operand1.IsVirtualRegister || context.Operand1.IsCPURegister);

			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			if (operand2.IsConstant)
			{
				operand2 = CreateImmediateOperand(context, operand2);
			}

			if (operand2.IsVirtualRegister || operand1.IsCPURegister)
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

		private Operand CreateImmediateOperand(Context context, Operand operand)
		{
			Debug.Assert(operand.IsConstant);

			if (operand.IsResolvedConstant)
			{
				if (ARMHelper.CalculateImmediateValue(operand.ConstantUnsignedInteger, out uint immediate, out _, out _))
				{
					if (operand.ConstantUnsignedLongInteger == immediate)
					{
						return operand;
					}

					return CreateConstant(immediate);
				}
				else
				{
					var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

					var before = context.InsertBefore();

					before.SetInstruction(ARMv8A32.MovwImm, v1, CreateConstant(operand.ConstantUnsignedInteger & 0xFFFF));
					before.AppendInstruction(ARMv8A32.MovtImm, v1, CreateConstant(operand.ConstantUnsignedInteger >> 16));

					return v1;
				}
			}
			else if (operand.IsUnresolvedConstant)
			{
				var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

				var before = context.InsertBefore();

				before.SetInstruction(ARMv8A32.MovwImm, v1, operand);
				before.AppendInstruction(ARMv8A32.MovtImm, v1, operand);

				return v1;
			}

			return operand;
		}
	}
}
