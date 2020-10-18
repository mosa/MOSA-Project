// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
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
		protected override void PopulateVisitationDictionary()
		{
			AddVisitation(IRInstruction.AddR4, AddR4);
			AddVisitation(IRInstruction.AddR8, AddR8);
			AddVisitation(IRInstruction.AddressOf, AddressOf);
			AddVisitation(IRInstruction.Add32, Add32);
			AddVisitation(IRInstruction.AddCarryOut32, AddCarryOut32);
			AddVisitation(IRInstruction.AddCarryIn32, AddCarryIn32);
			AddVisitation(IRInstruction.ArithShiftRight32, ArithShiftRight32);
			AddVisitation(IRInstruction.CallDirect, CallDirect);
			AddVisitation(IRInstruction.CompareR4, CompareR4);
			AddVisitation(IRInstruction.CompareR8, CompareR8);
			AddVisitation(IRInstruction.Compare32x32, Compare32x32);
			AddVisitation(IRInstruction.BranchCompare32, BranchCompare32);
			AddVisitation(IRInstruction.IfThenElse32, IfThenElse32);
			AddVisitation(IRInstruction.ConvertR4To32, ConvertR4ToInt32);
			AddVisitation(IRInstruction.ConvertR8To32, ConvertR8ToInt32);
			AddVisitation(IRInstruction.Convert32ToR4, ConvertInt32ToR4);
			AddVisitation(IRInstruction.Convert32ToR8, ConvertInt32ToR8);
			AddVisitation(IRInstruction.DivR4, DivR4);
			AddVisitation(IRInstruction.DivR8, DivR8);
			AddVisitation(IRInstruction.Jmp, Jmp);
			AddVisitation(IRInstruction.LoadR4, LoadR4);
			AddVisitation(IRInstruction.LoadR8, LoadR8);
			AddVisitation(IRInstruction.Load32, Load32);
			AddVisitation(IRInstruction.LoadSignExtend8x32, LoadSignExtend8x32);
			AddVisitation(IRInstruction.LoadSignExtend16x32, LoadSignExtend16x32);
			AddVisitation(IRInstruction.LoadZeroExtend8x32, LoadZeroExtend8x32);
			AddVisitation(IRInstruction.LoadZeroExtend16x32, LoadZeroExtend16x32);
			AddVisitation(IRInstruction.LoadParamR4, LoadParamR4);
			AddVisitation(IRInstruction.LoadParamR8, LoadParamR8);
			AddVisitation(IRInstruction.LoadParam32, LoadParam32);
			AddVisitation(IRInstruction.LoadParamSignExtend8x32, LoadParamSignExtend8x32);
			AddVisitation(IRInstruction.LoadParamSignExtend16x32, LoadParamSignExtend16x32);
			AddVisitation(IRInstruction.LoadParamZeroExtend8x32, LoadParamZeroExtend8x32);
			AddVisitation(IRInstruction.LoadParamZeroExtend16x32, LoadParamZeroExtend16x32);
			AddVisitation(IRInstruction.And32, And32);
			AddVisitation(IRInstruction.Not32, Not32);
			AddVisitation(IRInstruction.Or32, Or32);
			AddVisitation(IRInstruction.Xor32, Xor32);
			AddVisitation(IRInstruction.MoveR4, MoveR4);
			AddVisitation(IRInstruction.MoveR8, MoveR8);
			AddVisitation(IRInstruction.Move32, MoveInt32);
			AddVisitation(IRInstruction.SignExtend8x32, SignExtend8x32);
			AddVisitation(IRInstruction.SignExtend16x32, SignExtend16x32);
			AddVisitation(IRInstruction.ZeroExtend8x32, ZeroExtend8x32);
			AddVisitation(IRInstruction.ZeroExtend16x32, ZeroExtend16x32);
			AddVisitation(IRInstruction.MulR4, MulR4);
			AddVisitation(IRInstruction.MulR8, MulR8);
			AddVisitation(IRInstruction.MulSigned32, MulSigned32);
			AddVisitation(IRInstruction.MulUnsigned32, MulUnsigned32);
			AddVisitation(IRInstruction.Nop, Nop);
			AddVisitation(IRInstruction.ShiftLeft32, ShiftLeft32);
			AddVisitation(IRInstruction.ShiftRight32, ShiftRight32);
			AddVisitation(IRInstruction.StoreR4, StoreR4);
			AddVisitation(IRInstruction.StoreR8, StoreR8);
			AddVisitation(IRInstruction.Store8, StoreInt8);
			AddVisitation(IRInstruction.Store16, StoreInt16);
			AddVisitation(IRInstruction.Store32, StoreInt32);
			AddVisitation(IRInstruction.StoreParamR4, StoreParamR4);
			AddVisitation(IRInstruction.StoreParamR8, StoreParamR8);
			AddVisitation(IRInstruction.StoreParam8, StoreParamInt8);
			AddVisitation(IRInstruction.StoreParam16, StoreParamInt16);
			AddVisitation(IRInstruction.StoreParam32, StoreParamInt32);
			AddVisitation(IRInstruction.SubR4, SubR4);
			AddVisitation(IRInstruction.SubR8, SubR8);
			AddVisitation(IRInstruction.Sub32, Sub32);
			AddVisitation(IRInstruction.SubCarryOut32, SubCarryOut32);
			AddVisitation(IRInstruction.SubCarryIn32, SubCarryIn32);

			//AddVisitation(IRInstruction.Switch, Switch);
			AddVisitation(IRInstruction.ZeroExtend16x32, ZeroExtend16x32);
			AddVisitation(IRInstruction.ZeroExtend8x32, ZeroExtend8x32);
		}

		#region Visitation Methods

		private void Add32(Context context)
		{
			MoveConstantRight(context);

			Translate(context, ARMv8A32.Add, true);
		}

		private void AddCarryIn32(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var operand3 = context.Operand3;

			operand1 = MoveConstantToRegister(context, operand1);
			operand2 = MoveConstantToRegisterOrImmediate(context, operand2);
			operand3 = MoveConstantToRegisterOrImmediate(context, operand3);

			var v1 = AllocateVirtualRegister32();

			context.SetInstruction(ARMv8A32.Add, v1, operand1, operand2);
			context.AppendInstruction(ARMv8A32.Add, result, v1, operand3);
		}

		private void AddCarryOut32(Context context)
		{
			var result = context.Result;
			var result2 = context.Result2;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			operand1 = MoveConstantToRegister(context, operand1);
			operand2 = MoveConstantToRegisterOrImmediate(context, operand2);

			context.SetInstruction(ARMv8A32.Add, StatusRegister.Set, result, operand1, operand2);
			context.AppendInstruction(ARMv8A32.Mov, ConditionCode.Carry, result2, Constant_1);
			context.AppendInstruction(ARMv8A32.Mov, ConditionCode.NoCarry, result2, Constant_0);
		}

		private void AddR4(Context context)
		{
			MoveConstantRight(context);

			Translate(context, ARMv8A32.Adf, true);
		}

		private void AddR8(Context context)
		{
			MoveConstantRight(context);

			Translate(context, ARMv8A32.Adf, true);
		}

		private void AddressOf(Context context)
		{
			Debug.Assert(context.Operand1.IsOnStack || context.Operand1.IsStaticField);

			var result = context.Result;
			var operand1 = context.Operand1;

			operand1 = MoveConstantToRegisterOrImmediate(context, operand1);

			if (operand1.IsStaticField)
			{
				context.SetInstruction(ARMv8A32.Mov, result, operand1);
			}
			else if (operand1.IsStackLocal)
			{
				context.SetInstruction(ARMv8A32.Add, result, StackFrame, operand1);
			}
			else if (context.Operand1.IsUnresolvedConstant)
			{
				var offset = MoveConstantToRegister(context, operand1);

				context.SetInstruction(ARMv8A32.Add, result, StackFrame, offset);
			}
			else
			{
				var offset = CreateConstant(context.Operand1.Offset);

				offset = MoveConstantToRegisterOrImmediate(context, offset);

				context.SetInstruction(ARMv8A32.Add, result, StackFrame, offset);
			}
		}

		private void And32(Context context)
		{
			MoveConstantRight(context);

			Translate(context, ARMv8A32.And, true);
		}

		private void ArithShiftRight32(Context context)
		{
			Translate(context, ARMv8A32.Asr, true);
		}

		private void CallDirect(Context context)
		{
			var operand1 = context.Operand1;

			if (operand1.IsCPURegister || operand1.IsVirtualRegister || operand1.IsResolvedConstant)
			{
				operand1 = MoveConstantToRegister(context, operand1);

				context.SetInstruction(ARMv8A32.Add, LinkRegister, ProgramCounter, Constant_4);
				context.AppendInstruction(ARMv8A32.Mov, ProgramCounter, operand1);
			}
			else
			{
				context.SetInstruction(ARMv8A32.Bl, operand1);
			}
		}

		private void Compare32x32(Context context)
		{
			MoveConstantRightForComparison(context);

			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var condition = context.ConditionCode;

			operand1 = MoveConstantToRegister(context, operand1);
			operand2 = MoveConstantToRegisterOrImmediate(context, operand2);

			context.SetInstruction(ARMv8A32.Cmp, condition, null, operand1, operand2);
			context.AppendInstruction(ARMv8A32.Mov, condition, result, Constant_1);
			context.AppendInstruction(ARMv8A32.Mov, condition.GetOpposite(), result, ConstantZero);
		}

		private void BranchCompare32(Context context)
		{
			MoveConstantRightForComparison(context);

			var target = context.BranchTargets[0];
			var condition = context.ConditionCode;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			operand1 = MoveConstantToRegister(context, operand1);
			operand2 = MoveConstantToRegisterOrImmediate(context, operand2);

			context.SetInstruction(ARMv8A32.Cmp, null, operand1, operand2);
			context.AppendInstruction(ARMv8A32.B, condition, target);
		}

		private void CompareR4(Context context)
		{
			MoveConstantRightForComparison(context);

			var result = context.Result;
			var condition = context.ConditionCode;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			operand1 = MoveConstantToFloatRegister(context, operand1);
			operand2 = MoveConstantToFloatRegisterOrImmediate(context, operand2);

			context.SetInstruction(ARMv8A32.Cmf, null, operand1, operand2);
			context.AppendInstruction(ARMv8A32.Mov, condition, result, Constant_0);
			context.AppendInstruction(ARMv8A32.Mov, condition.GetOpposite(), result, Constant_1);
		}

		private void CompareR8(Context context)
		{
			CompareR4(context);
		}

		private void ConvertInt32ToR4(Context context)
		{
			Translate(context, ARMv8A32.Flt, false);
		}

		private void ConvertInt32ToR8(Context context)
		{
			Translate(context, ARMv8A32.Flt, false);
		}

		private void ConvertR4ToInt32(Context context)
		{
			Translate(context, ARMv8A32.Fix, true);
		}

		private void ConvertR8ToInt32(Context context)
		{
			Translate(context, ARMv8A32.Fix, true);
		}

		private void DivR4(Context context)
		{
			Translate(context, ARMv8A32.Dvf, true);
		}

		private void DivR8(Context context)
		{
			Translate(context, ARMv8A32.Dvf, true);
		}

		private void IfThenElse32(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var operand3 = context.Operand3;

			operand1 = MoveConstantToRegister(context, operand1);
			operand2 = MoveConstantToRegisterOrImmediate(context, operand2);
			operand3 = MoveConstantToRegisterOrImmediate(context, operand3);

			context.SetInstruction(ARMv8A32.Cmp, StatusRegister.Set, null, operand1, Constant_0);
			context.AppendInstruction(ARMv8A32.Mov, ConditionCode.Equal, result, operand2);
			context.AppendInstruction(ARMv8A32.Mov, ConditionCode.NotEqual, result, operand3);
		}

		private void Jmp(Context context)
		{
			context.ReplaceInstruction(ARMv8A32.B);
			context.ConditionCode = ConditionCode.Always;
		}

		private void Load32(Context context)
		{
			Debug.Assert(!context.Result.IsR4);
			Debug.Assert(!context.Result.IsR8);

			LoadStore.OrderOperands(context, MethodCompiler);

			TransformLoad(context, ARMv8A32.Ldr32, context.Result, context.Operand1, context.Operand2);
		}

		private void LoadParam32(Context context)
		{
			Debug.Assert(!context.Result.IsR4);
			Debug.Assert(!context.Result.IsR8);

			TransformLoad(context, ARMv8A32.Ldr32, context.Result, StackFrame, context.Operand1);
		}

		private void LoadParamR4(Context context)
		{
			Debug.Assert(context.Operand1.IsConstant);

			TransformLoad(context, ARMv8A32.Ldf, context.Result, StackFrame, context.Operand1);
		}

		private void LoadParamR8(Context context)
		{
			Debug.Assert(context.Operand1.IsConstant);

			TransformLoad(context, ARMv8A32.Ldf, context.Result, StackFrame, context.Operand1);
		}

		private void LoadParamSignExtend16x32(Context context)
		{
			Debug.Assert(!context.Result.IsR4);
			Debug.Assert(!context.Result.IsR8);

			TransformLoad(context, ARMv8A32.LdrS16, context.Result, StackFrame, context.Operand1);
		}

		private void LoadParamSignExtend8x32(Context context)
		{
			Debug.Assert(!context.Result.IsR4);
			Debug.Assert(!context.Result.IsR8);

			TransformLoad(context, ARMv8A32.LdrS8, context.Result, StackFrame, context.Operand1);
		}

		private void LoadParamZeroExtend16x32(Context context)
		{
			Debug.Assert(!context.Result.IsR4);
			Debug.Assert(!context.Result.IsR8);

			TransformLoad(context, ARMv8A32.Ldr16, context.Result, StackFrame, context.Operand1);
		}

		private void LoadParamZeroExtend8x32(Context context)
		{
			Debug.Assert(!context.Result.IsR4);
			Debug.Assert(!context.Result.IsR8);

			TransformLoad(context, ARMv8A32.Ldr8, context.Result, StackFrame, context.Operand1);
		}

		private void LoadR4(Context context)
		{
			LoadStore.OrderOperands(context, MethodCompiler);

			TransformLoad(context, ARMv8A32.Ldf, context.Result, context.Operand1, context.Operand2);
		}

		private void LoadR8(Context context)
		{
			LoadR4(context);
		}

		private void LoadSignExtend16x32(Context context)
		{
			Debug.Assert(!context.Result.IsR4);
			Debug.Assert(!context.Result.IsR8);

			LoadStore.OrderOperands(context, MethodCompiler);

			TransformLoad(context, ARMv8A32.LdrS16, context.Result, StackFrame, context.Operand1);
		}

		private void LoadSignExtend8x32(Context context)
		{
			Debug.Assert(!context.Result.IsR4);
			Debug.Assert(!context.Result.IsR8);

			LoadStore.OrderOperands(context, MethodCompiler);

			TransformLoad(context, ARMv8A32.LdrS8, context.Result, StackFrame, context.Operand1);
		}

		private void LoadZeroExtend16x32(Context context)
		{
			Debug.Assert(!context.Result.IsR4);
			Debug.Assert(!context.Result.IsR8);

			LoadStore.OrderOperands(context, MethodCompiler);

			TransformLoad(context, ARMv8A32.Ldr16, context.Result, StackFrame, context.Operand1);
		}

		private void LoadZeroExtend8x32(Context context)
		{
			Debug.Assert(!context.Result.IsR4);
			Debug.Assert(!context.Result.IsR8);

			LoadStore.OrderOperands(context, MethodCompiler);

			TransformLoad(context, ARMv8A32.Ldr8, context.Result, StackFrame, context.Operand1);
		}

		private void MoveInt32(Context context)
		{
			Translate(context, ARMv8A32.Mov, true);
		}

		private void MoveR4(Context context)
		{
			Translate(context, ARMv8A32.Mvf, true);
		}

		private void MoveR8(Context context)
		{
			Translate(context, ARMv8A32.Mvf, true);
		}

		private void MulR4(Context context)
		{
			MoveConstantRight(context);

			Translate(context, ARMv8A32.Muf, true);
		}

		private void MulR8(Context context)
		{
			MoveConstantRight(context);

			Translate(context, ARMv8A32.Muf, true);
		}

		private void MulSigned32(Context context)
		{
			MoveConstantRight(context);

			Translate(context, ARMv8A32.Mul, false);
		}

		private void MulUnsigned32(Context context)
		{
			MoveConstantRight(context);

			Translate(context, ARMv8A32.Mul, false);
		}

		private void Nop(Context context)
		{
			context.Empty();
		}

		private void Not32(Context context)
		{
			Translate(context, ARMv8A32.Mvn, true);
		}

		private void Or32(Context context)
		{
			MoveConstantRight(context);

			Translate(context, ARMv8A32.Orr, true);
		}

		private void ShiftLeft32(Context context)
		{
			Translate(context, ARMv8A32.Lsl, true);
		}

		private void ShiftRight32(Context context)
		{
			Translate(context, ARMv8A32.Lsr, true);
		}

		private void SignExtend16x32(Context context)
		{
			Translate(context, ARMv8A32.Sxth, false);
		}

		private void SignExtend8x32(Context context)
		{
			Translate(context, ARMv8A32.Sxth, false);
		}

		private void StoreInt16(Context context)
		{
			TransformStore(context, ARMv8A32.Str16, context.Operand1, context.Operand2, context.Operand3);
		}

		private void StoreInt32(Context context)
		{
			TransformStore(context, ARMv8A32.Str32, context.Operand1, context.Operand2, context.Operand3);
		}

		private void StoreInt8(Context context)
		{
			TransformStore(context, ARMv8A32.Str8, context.Operand1, context.Operand2, context.Operand3);
		}

		private void StoreParamInt16(Context context)
		{
			TransformStore(context, ARMv8A32.Str16, StackFrame, context.Operand1, context.Operand2);
		}

		private void StoreParamInt32(Context context)
		{
			TransformStore(context, ARMv8A32.Str32, StackFrame, context.Operand1, context.Operand2);
		}

		private void StoreParamInt8(Context context)
		{
			TransformStore(context, ARMv8A32.Str8, StackFrame, context.Operand1, context.Operand2);
		}

		private void StoreParamR4(Context context)
		{
			TransformStore(context, ARMv8A32.Stf, StackFrame, context.Operand1, context.Operand2);
		}

		private void StoreParamR8(Context context)
		{
			TransformStore(context, ARMv8A32.Stf, StackFrame, context.Operand1, context.Operand2);
		}

		private void StoreR4(Context context)
		{
			TransformStore(context, ARMv8A32.Stf, context.Operand1, context.Operand2, context.Operand3);
		}

		private void StoreR8(Context context)
		{
			TransformStore(context, ARMv8A32.Stf, context.Operand1, context.Operand2, context.Operand3);
		}

		private void Sub32(Context context)
		{
			Translate(context, ARMv8A32.Sub, true);
		}

		private void SubCarryIn32(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var operand3 = context.Operand3;

			operand1 = MoveConstantToRegister(context, operand1);
			operand2 = MoveConstantToRegisterOrImmediate(context, operand2);
			operand3 = MoveConstantToRegisterOrImmediate(context, operand3);

			var v1 = AllocateVirtualRegister32();

			context.SetInstruction(ARMv8A32.Sub, v1, operand1, operand2);
			context.AppendInstruction(ARMv8A32.Sub, result, v1, operand3);
		}

		private void SubCarryOut32(Context context)
		{
			var result = context.Result;
			var result2 = context.Result2;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			operand1 = MoveConstantToRegister(context, operand1);
			operand2 = MoveConstantToRegisterOrImmediate(context, operand2);

			context.SetInstruction(ARMv8A32.Sub, StatusRegister.Set, result, operand1, operand2);
			context.AppendInstruction(ARMv8A32.Mov, ConditionCode.Carry, result2, Constant_1);
			context.AppendInstruction(ARMv8A32.Mov, ConditionCode.NoCarry, result2, Constant_0);
		}

		private void SubR4(Context context)
		{
			Translate(context, ARMv8A32.Suf, true);
		}

		private void SubR8(Context context)
		{
			Translate(context, ARMv8A32.Suf, true);
		}

		private void Xor32(Context context)
		{
			MoveConstantRight(context);

			Translate(context, ARMv8A32.Eor, true);
		}

		private void ZeroExtend16x32(Context context)
		{
			Translate(context, ARMv8A32.Uxth, false);
		}

		private void ZeroExtend8x32(Context context)
		{
			Translate(context, ARMv8A32.Uxtb, false);
		}

		#endregion Visitation Methods

		#region Helper Methods

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

		private void Translate(Context context, BaseInstruction instruction, bool allowImmediate)
		{
			var result = context.Result;
			var operand1 = context.Operand1;

			if (context.OperandCount == 1)
			{
				operand1 = operand1.IsFloatingPoint
					? MoveConstantToFloatRegisterOrImmediate(context, operand1, allowImmediate)
					: MoveConstantToRegisterOrImmediate(context, operand1, allowImmediate);

				context.SetInstruction(instruction, result, operand1);
			}
			else if (context.OperandCount == 2)
			{
				var operand2 = context.Operand2;

				operand1 = MoveConstantToRegister(context, operand1);

				operand2 = operand2.IsFloatingPoint
					? MoveConstantToFloatRegisterOrImmediate(context, operand2, allowImmediate)
					: MoveConstantToRegisterOrImmediate(context, operand2, allowImmediate);

				context.SetInstruction(instruction, result, operand1, operand2);
			}
		}

		#endregion Helper Methods
	}
}
