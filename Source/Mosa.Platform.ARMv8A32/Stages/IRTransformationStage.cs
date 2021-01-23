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
			AddVisitation(IRInstruction.Branch32, Branch32);
			AddVisitation(IRInstruction.IfThenElse32, IfThenElse32);
			AddVisitation(IRInstruction.ConvertR4ToI32, ConvertR4ToI32);
			AddVisitation(IRInstruction.ConvertR8ToI32, ConvertR8ToI32);
			AddVisitation(IRInstruction.ConvertI32ToR4, ConvertI32ToR4);
			AddVisitation(IRInstruction.ConvertI32ToR8, ConvertI32ToR8);

			//AddVisitation(IRInstruction.ConvertR4ToU32, ConvertR4ToU32);
			//AddVisitation(IRInstruction.ConvertR8ToU32, ConvertR8ToU32);
			//AddVisitation(IRInstruction.ConvertU32ToR4, ConvertU32ToR4);
			//AddVisitation(IRInstruction.ConvertU32ToR8, ConvertU32ToR8);

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

			// 64-bit Transforms

			AddVisitation(IRInstruction.Add64, Add64);

			//AddVisitation(IRInstruction.BitCopyR8To64, BitCopyR8To64);
			//AddVisitation(IRInstruction.BitCopy32ToR4, BitCopy64ToR8);
			AddVisitation(IRInstruction.ArithShiftRight64, ArithShiftRight64);
			AddVisitation(IRInstruction.Call, Call);

			//AddVisitation(IRInstruction.Compare32x64, Compare32x64);
			//AddVisitation(IRInstruction.Compare64x32, Compare64x32);
			//AddVisitation(IRInstruction.Compare64x64, Compare64x64);
			//AddVisitation(IRInstruction.Branch64, Branch64);
			//AddVisitation(IRInstruction.ConvertR4ToI64, ConvertR4ToI64);
			//AddVisitation(IRInstruction.ConvertR8ToI64, ConvertR8ToI64);
			//AddVisitation(IRInstruction.ConvertI64ToR4, ConvertI64ToR4);
			//AddVisitation(IRInstruction.ConvertI64ToR8, ConvertI64ToR8);

			//AddVisitation(IRInstruction.ConvertR4ToI64, ConvertR4ToI64);
			//AddVisitation(IRInstruction.ConvertR8ToI64, ConvertR8ToI64);
			//AddVisitation(IRInstruction.ConvertU64ToR4, ConvertU64ToR4);
			//AddVisitation(IRInstruction.ConvertU64ToR8, ConvertU64ToR8);

			AddVisitation(IRInstruction.IfThenElse64, IfThenElse64);
			AddVisitation(IRInstruction.Load64, Load64);
			AddVisitation(IRInstruction.LoadParam64, LoadParam64);
			AddVisitation(IRInstruction.LoadParamSignExtend16x64, LoadParamSignExtend16x64);
			AddVisitation(IRInstruction.LoadParamSignExtend32x64, LoadParamSignExtend32x64);
			AddVisitation(IRInstruction.LoadParamSignExtend8x64, LoadParamSignExtend8x64);
			AddVisitation(IRInstruction.LoadParamZeroExtend16x64, LoadParamZeroExtended16x64);
			AddVisitation(IRInstruction.LoadParamZeroExtend32x64, LoadParamZeroExtended32x64);
			AddVisitation(IRInstruction.LoadParamZeroExtend8x64, LoadParamZeroExtended8x64);
			AddVisitation(IRInstruction.And64, And64);
			AddVisitation(IRInstruction.Not64, Not64);
			AddVisitation(IRInstruction.Or64, Or64);
			AddVisitation(IRInstruction.Xor64, Xor64);
			AddVisitation(IRInstruction.Move64, Move64);
			AddVisitation(IRInstruction.MulSigned64, MulSigned64);
			AddVisitation(IRInstruction.MulUnsigned64, MulUnsigned64);
			AddVisitation(IRInstruction.ShiftLeft64, ShiftLeft64);
			AddVisitation(IRInstruction.ShiftRight64, ShiftRight64);
			AddVisitation(IRInstruction.SignExtend16x64, SignExtend16x64);
			AddVisitation(IRInstruction.SignExtend32x64, SignExtend32x64);
			AddVisitation(IRInstruction.SignExtend8x64, SignExtend8x64);
			AddVisitation(IRInstruction.GetHigh32, GetHigh32);
			AddVisitation(IRInstruction.GetLow32, GetLow32);
			AddVisitation(IRInstruction.Store64, Store64);
			AddVisitation(IRInstruction.StoreParam64, StoreParam64);
			AddVisitation(IRInstruction.Sub64, Sub64);
			AddVisitation(IRInstruction.To64, To64);
			AddVisitation(IRInstruction.Truncate64x32, Truncate64x32);
			AddVisitation(IRInstruction.ZeroExtend16x64, ZeroExtended16x64);
			AddVisitation(IRInstruction.ZeroExtend32x64, ZeroExtended32x64);
			AddVisitation(IRInstruction.ZeroExtend8x64, ZeroExtended8x64);
		}

		#region 32-bit Transformations

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

			var v1 = AllocateVirtualRegisterI32();

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
				var offset = CreateConstant32(context.Operand1.Offset);

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
			context.AppendInstruction(ARMv8A32.Mov, condition.GetOpposite(), result, ConstantZero32);
		}

		private void Branch32(Context context)
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

		private void ConvertI32ToR4(Context context)
		{
			Translate(context, ARMv8A32.Flt, false);
		}

		private void ConvertI32ToR8(Context context)
		{
			Translate(context, ARMv8A32.Flt, false);
		}

		private void ConvertR4ToI32(Context context)
		{
			Translate(context, ARMv8A32.Fix, true);
		}

		private void ConvertR8ToI32(Context context)
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

			var v1 = AllocateVirtualRegisterI32();

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

		#endregion 32-bit Transformations

		#region 64-bit Transformations

		private void Add64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			SplitLongOperand(context.Operand1, out var op1L, out var op1H);
			SplitLongOperand(context.Operand2, out var op2L, out var op2H);

			// FUTURE: Swap so constant are on the right

			op1L = MoveConstantToRegister(context, op1L);
			op1H = MoveConstantToRegister(context, op1H);
			op2L = MoveConstantToRegisterOrImmediate(context, op2L);
			op2H = MoveConstantToRegisterOrImmediate(context, op2H);

			context.SetInstruction(ARMv8A32.Add, StatusRegister.Set, resultLow, op1L, op2L);
			context.AppendInstruction(ARMv8A32.Adc, resultHigh, op1H, op2H);
		}

		private void ArithShiftRight64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			SplitLongOperand(context.Operand1, out var op1L, out var op1H);
			SplitLongOperand(context.Operand2, out var op2L, out var op2H);

			op1L = MoveConstantToRegister(context, op1L);
			op1H = MoveConstantToRegister(context, op1H);
			op2L = MoveConstantToRegister(context, op2L);

			var v1 = AllocateVirtualRegisterI32();
			var v2 = AllocateVirtualRegisterI32();

			context.SetInstruction(ARMv8A32.Asr, resultHigh, op1H, op2L);
			context.AppendInstruction(ARMv8A32.Sub, StatusRegister.Set, v1, op2L, Constant_0);
			context.AppendInstruction(ARMv8A32.Lsr, resultLow, op1L, op2L);
			context.AppendInstruction(ARMv8A32.Rsb, v2, op2L, Constant_0);
			context.AppendInstruction(ARMv8A32.Asr, ConditionCode.Positive, resultHigh, op1H, Constant_1F);
			context.AppendInstruction(ARMv8A32.OrrRegShift, resultLow, resultLow, op1H, v2, LSL);
			context.AppendInstruction(ARMv8A32.Asr, ConditionCode.Positive, resultLow, op1H, v1);
		}

		private void Call(Context context)
		{
			if (context.Result?.IsInteger64 == true)
			{
				SplitLongOperand(context.Result, out _, out _);
			}

			foreach (var operand in context.Operands)
			{
				if (operand.IsInteger64)
				{
					SplitLongOperand(operand, out _, out _);
				}
			}
		}

		private void Load64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);

			var address = context.Operand1;
			var offset = context.Operand2;

			address = MoveConstantToRegister(context, address);
			offset = MoveConstantToRegisterOrImmediate(context, offset);

			var v1 = AllocateVirtualRegisterI32();
			var v2 = AllocateVirtualRegisterI32();

			context.SetInstruction(ARMv8A32.Add, v1, address, offset);
			context.AppendInstruction(ARMv8A32.Ldr32, resultLow, v1, ConstantZero32);
			context.AppendInstruction(ARMv8A32.Add, v2, v1, Constant_4);
			context.AppendInstruction(ARMv8A32.Ldr32, resultHigh, v2, ConstantZero32);
		}

		private void LoadParam64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			SplitLongOperand(context.Operand1, out var lowOffset, out var highOffset);

			TransformLoad(context, ARMv8A32.Ldr32, resultLow, StackFrame, lowOffset);
			TransformLoad(context.InsertAfter(), ARMv8A32.Ldr32, resultHigh, StackFrame, highOffset);
		}

		private void LoadParamSignExtend16x64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			SplitLongOperand(context.Operand1, out var lowOffset, out var highOffset);

			TransformLoad(context, ARMv8A32.LdrS16, resultLow, StackFrame, lowOffset);

			context.AppendInstruction(ARMv8A32.Asr, resultHigh, resultLow, Constant_31);
		}

		private void LoadParamSignExtend32x64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			SplitLongOperand(context.Operand1, out var lowOffset, out var highOffset);

			TransformLoad(context, ARMv8A32.Ldr32, resultLow, StackFrame, lowOffset);
			context.AppendInstruction(ARMv8A32.Asr, resultHigh, resultLow, Constant_31);
		}

		private void LoadParamSignExtend8x64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			SplitLongOperand(context.Operand1, out var lowOffset, out var highOffset);

			TransformLoad(context, ARMv8A32.LdrS8, resultLow, StackFrame, lowOffset);
			context.AppendInstruction(ARMv8A32.Asr, resultHigh, resultLow, Constant_31);
		}

		private void LoadParamZeroExtended16x64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			SplitLongOperand(context.Operand1, out var lowOffset, out _);

			TransformLoad(context, ARMv8A32.Ldr16, resultLow, StackFrame, lowOffset);
			context.AppendInstruction(ARMv8A32.Mov, resultHigh, Constant_0);
		}

		private void LoadParamZeroExtended32x64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			SplitLongOperand(context.Operand1, out var lowOffset, out _);

			TransformLoad(context, ARMv8A32.Ldr32, resultLow, StackFrame, lowOffset);
			context.AppendInstruction(ARMv8A32.Mov, resultHigh, Constant_0);
		}

		private void LoadParamZeroExtended8x64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			SplitLongOperand(context.Operand1, out var lowOffset, out _);

			TransformLoad(context, ARMv8A32.Ldr8, resultLow, StackFrame, lowOffset);
			context.AppendInstruction(ARMv8A32.Mov, resultHigh, Constant_0);
		}

		private void IfThenElse64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			SplitLongOperand(context.Operand1, out var op1L, out var op1H);
			SplitLongOperand(context.Operand2, out var op2L, out var op2H);
			SplitLongOperand(context.Operand3, out var op3L, out var op3H);

			var v1 = AllocateVirtualRegisterI32();

			op1L = MoveConstantToRegister(context, op1L);
			op1H = MoveConstantToRegisterOrImmediate(context, op1H);
			op2L = MoveConstantToRegisterOrImmediate(context, op2L);
			op2H = MoveConstantToRegisterOrImmediate(context, op2H);
			op3L = MoveConstantToRegisterOrImmediate(context, op3L);
			op3H = MoveConstantToRegisterOrImmediate(context, op3H);

			context.SetInstruction(ARMv8A32.Orr, v1, op1L, op1H);
			context.AppendInstruction(ARMv8A32.Cmp, StatusRegister.Set, null, v1, Constant_0);
			context.AppendInstruction(ARMv8A32.Mov, ConditionCode.NotEqual, resultLow, resultLow, op2L);
			context.AppendInstruction(ARMv8A32.Mov, ConditionCode.NotEqual, resultHigh, resultHigh, op2H);
			context.AppendInstruction(ARMv8A32.Mov, ConditionCode.Equal, resultLow, resultLow, op3L);
			context.AppendInstruction(ARMv8A32.Mov, ConditionCode.Equal, resultHigh, resultHigh, op3H);
		}

		private void Move64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			SplitLongOperand(context.Operand1, out var op1L, out var op1H);

			op1L = MoveConstantToRegisterOrImmediate(context, op1L);
			op1H = MoveConstantToRegisterOrImmediate(context, op1H);

			context.SetInstruction(ARMv8A32.Mov, resultLow, op1L);
			context.AppendInstruction(ARMv8A32.Mov, resultHigh, op1H);
		}

		private void MulSigned64(Context context)
		{
			ExpandMul(context);
		}

		private void MulUnsigned64(Context context)
		{
			ExpandMul(context);
		}

		private void ShiftLeft64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			SplitLongOperand(context.Operand1, out var op1L, out var op1H);

			//shiftleft64(long long, int):
			//		r0 (op1l), r1 (op1h), r2 (operand2)

			//mov	v1, op1l
			//sub	v2, operand2, #32
			//rsb	v3, operand2, #32
			//lsl	v4, op1L, op1L

			//orr	v5, v4, v4, lsl v2
			//lsl	resultLow, v1, operand2
			//orr	resultHigh, v5, v1, lsr v3

			var v1 = AllocateVirtualRegisterI32();
			var v2 = AllocateVirtualRegisterI32();
			var v3 = AllocateVirtualRegisterI32();
			var v4 = AllocateVirtualRegisterI32();
			var v5 = AllocateVirtualRegisterI32();

			var operand2 = MoveConstantToRegister(context, context.Operand2);

			op1L = MoveConstantToRegisterOrImmediate(context, op1L);
			op1H = MoveConstantToRegister(context, op1H);

			context.SetInstruction(ARMv8A32.Mov, v1, op1L);
			context.AppendInstruction(ARMv8A32.Sub, v2, operand2, Constant_32);
			context.AppendInstruction(ARMv8A32.Rsb, v3, operand2, Constant_32);
			context.AppendInstruction(ARMv8A32.Lsl, v4, op1H, operand2);

			context.AppendInstruction(ARMv8A32.OrrRegShift, v5, v4, v4, v2, LSL);
			context.AppendInstruction(ARMv8A32.Lsl, resultLow, v1, operand2);
			context.AppendInstruction(ARMv8A32.OrrRegShift, resultHigh, v5, v1, v3, LSR);
		}

		private void ShiftRight64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			SplitLongOperand(context.Operand1, out var op1L, out var op1H);
			SplitLongOperand(context.Operand2, out var op2L, out var op2H);

			//shiftright64(long long, int):
			//		r0 (op1l), r1 (op1h), r2 (operand2)

			//rsb	v1, operand2, #32
			//subs	v2, operand2, #32
			//lsr	v3, op1l, operand2

			//orr	v4, v3, op1h, lsl v1
			//orrpl	resultLow, v4, op1h, asr v2

			//asr	resultHigh, op1h, operand2

			var v1 = AllocateVirtualRegisterI32();
			var v2 = AllocateVirtualRegisterI32();
			var v3 = AllocateVirtualRegisterI32();
			var v4 = AllocateVirtualRegisterI32();

			op1L = MoveConstantToRegister(context, op1L);
			op1H = MoveConstantToRegister(context, op1H);
			op2L = MoveConstantToRegister(context, op2L);

			context.SetInstruction(ARMv8A32.Rsb, v1, op2L, Constant_32);
			context.AppendInstruction(ARMv8A32.Sub, StatusRegister.Set, v2, op2L, Constant_32);
			context.AppendInstruction(ARMv8A32.Lsr, v3, op1L, op2L);
			context.AppendInstruction(ARMv8A32.OrrRegShift, v4, v3, op1H, v1, LSL);
			context.AppendInstruction(ARMv8A32.OrrRegShift, ConditionCode.Zero, resultLow, v4, op1H, v2, ASR);
			context.AppendInstruction(ARMv8A32.Asr, resultHigh, op1H, op2L);
		}

		private void SignExtend16x64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);

			var op1 = MoveConstantToRegister(context, context.Operand1);

			context.SetInstruction(ARMv8A32.Lsl, resultLow, op1, Constant_16);
			context.AppendInstruction(ARMv8A32.Asr, resultLow, resultLow, Constant_16);
			context.AppendInstruction(ARMv8A32.Asr, resultHigh, resultLow, Constant_31);
		}

		private void SignExtend32x64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);

			var op1 = MoveConstantToRegisterOrImmediate(context, context.Operand1);

			context.SetInstruction(ARMv8A32.Mov, resultLow, op1);
			context.AppendInstruction(ARMv8A32.Asr, resultHigh, resultLow, Constant_31);
		}

		private void SignExtend8x64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);

			var op1 = MoveConstantToRegister(context, context.Operand1);

			context.SetInstruction(ARMv8A32.Lsl, resultLow, op1, Constant_24);
			context.AppendInstruction(ARMv8A32.Asr, resultLow, resultLow, Constant_24);
			context.AppendInstruction(ARMv8A32.Asr, resultHigh, resultLow, Constant_31);
		}

		private void And64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			SplitLongOperand(context.Operand1, out var op1L, out var op1H);
			SplitLongOperand(context.Operand2, out var op2L, out var op2H);

			op1L = MoveConstantToRegister(context, op1L);
			op1H = MoveConstantToRegister(context, op1H);
			op2L = MoveConstantToRegisterOrImmediate(context, op2L);
			op2H = MoveConstantToRegisterOrImmediate(context, op2H);

			context.SetInstruction(ARMv8A32.And, resultLow, op1L, op2L);
			context.AppendInstruction(ARMv8A32.And, resultHigh, op1H, op2H);
		}

		private void Not64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			SplitLongOperand(context.Operand1, out var op1L, out var op1H);

			op1L = MoveConstantToRegisterOrImmediate(context, op1L);
			op1H = MoveConstantToRegisterOrImmediate(context, op1H);

			context.SetInstruction(ARMv8A32.Mvn, resultLow, op1L);
			context.AppendInstruction(ARMv8A32.Mvn, resultHigh, op1H);
		}

		private void Or64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			SplitLongOperand(context.Operand1, out var op1L, out var op1H);
			SplitLongOperand(context.Operand2, out var op2L, out var op2H);

			op1L = MoveConstantToRegister(context, op1L);
			op1H = MoveConstantToRegister(context, op1H);
			op2L = MoveConstantToRegisterOrImmediate(context, op2L);
			op2H = MoveConstantToRegisterOrImmediate(context, op2H);

			context.SetInstruction(ARMv8A32.Orr, resultLow, op1L, op2L);
			context.AppendInstruction(ARMv8A32.Orr, resultHigh, op1H, op2H);
		}

		private void Xor64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			SplitLongOperand(context.Operand1, out var op1L, out var op1H);
			SplitLongOperand(context.Operand2, out var op2L, out var op2H);

			op1L = MoveConstantToRegister(context, op1L);
			op1H = MoveConstantToRegister(context, op1H);
			op2L = MoveConstantToRegisterOrImmediate(context, op2L);
			op2H = MoveConstantToRegisterOrImmediate(context, op2H);

			context.SetInstruction(ARMv8A32.Eor, resultLow, op1L, op2L);
			context.AppendInstruction(ARMv8A32.Eor, resultHigh, op1H, op2H);
		}

		private void GetHigh32(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out _);
			SplitLongOperand(context.Operand1, out _, out var op1H);

			op1H = MoveConstantToRegisterOrImmediate(context, op1H);

			context.SetInstruction(ARMv8A32.Mov, resultLow, op1H);
		}

		private void GetLow32(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out _);
			SplitLongOperand(context.Operand1, out var op1L, out _);

			op1L = MoveConstantToRegisterOrImmediate(context, op1L);

			context.SetInstruction(ARMv8A32.Mov, resultLow, op1L);
		}

		private void Store64(Context context)
		{
			SplitLongOperand(context.Operand1, out var baseLow, out var baseHigh);
			SplitLongOperand(context.Operand2, out var lowOffset, out var highOffset);
			SplitLongOperand(context.Operand3, out var valueLow, out var valueHigh);

			TransformStore(context, ARMv8A32.Str32, baseLow, lowOffset, valueLow);
			TransformStore(context.InsertAfter(), ARMv8A32.Str32, baseLow, highOffset, valueHigh);
		}

		private void StoreParam64(Context context)
		{
			SplitLongOperand(context.Operand1, out var lowOffset, out var highOffset);
			SplitLongOperand(context.Operand2, out var valueLow, out var valueHigh);

			TransformStore(context, ARMv8A32.Str32, StackFrame, lowOffset, valueLow);
			TransformStore(context.InsertAfter(), ARMv8A32.Str32, StackFrame, highOffset, valueHigh);
		}

		private void To64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);

			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			operand1 = MoveConstantToRegisterOrImmediate(context, operand1);
			operand2 = MoveConstantToRegisterOrImmediate(context, operand2);

			context.SetInstruction(ARMv8A32.Mov, resultLow, operand1);
			context.AppendInstruction(ARMv8A32.Mov, resultHigh, operand2);
		}

		private void Sub64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			SplitLongOperand(context.Operand1, out var op1L, out var op1H);
			SplitLongOperand(context.Operand2, out var op2L, out var op2H);

			op1L = MoveConstantToRegister(context, op1L);
			op1H = MoveConstantToRegister(context, op1H);
			op2L = MoveConstantToRegisterOrImmediate(context, op2L);
			op2H = MoveConstantToRegisterOrImmediate(context, op2H);

			context.SetInstruction(ARMv8A32.Sub, StatusRegister.Set, resultLow, op1L, op2L);
			context.AppendInstruction(ARMv8A32.Sbc, resultHigh, op1H, op2H);
		}

		private void Truncate64x32(Context context)
		{
			Debug.Assert(context.Operand1.IsInteger64);
			Debug.Assert(!context.Result.IsInteger64);

			SplitLongOperand(context.Result, out var resultLow, out _);
			SplitLongOperand(context.Operand1, out var op1L, out _);

			op1L = MoveConstantToRegisterOrImmediate(context, op1L);

			context.SetInstruction(ARMv8A32.Mov, StatusRegister.Set, resultLow, op1L);
		}

		private void ZeroExtended16x64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			SplitLongOperand(context.Operand1, out var op1L, out _);

			op1L = MoveConstantToRegisterOrImmediate(context, op1L);

			var operand1 = MoveConstantToRegister(context, CreateConstant32((uint)0xFFFF));

			context.SetInstruction(ARMv8A32.Mov, resultLow, op1L);
			context.AppendInstruction(ARMv8A32.And, resultLow, resultLow, operand1);
			context.AppendInstruction(ARMv8A32.Mov, resultHigh, Constant_0);
		}

		private void ZeroExtended32x64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			SplitLongOperand(context.Operand1, out var op1L, out _);

			op1L = MoveConstantToRegisterOrImmediate(context, op1L);

			context.SetInstruction(ARMv8A32.Mov, resultLow, op1L);
			context.AppendInstruction(ARMv8A32.Mov, resultHigh, Constant_0);
		}

		private void ZeroExtended8x64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			SplitLongOperand(context.Operand1, out var op1L, out _);

			op1L = MoveConstantToRegisterOrImmediate(context, op1L);

			context.SetInstruction(ARMv8A32.Mov, resultLow, op1L);
			context.AppendInstruction(ARMv8A32.And, resultLow, resultLow, CreateConstant32((uint)0xFF));
			context.AppendInstruction(ARMv8A32.Mov, resultHigh, Constant_0);
		}

		#endregion 64-bit Transformations

		private void ExpandMul(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			SplitLongOperand(context.Operand1, out var op1L, out var op1H);
			SplitLongOperand(context.Operand2, out var op2L, out var op2H);

			var v1 = AllocateVirtualRegisterI32();
			var v2 = AllocateVirtualRegisterI32();

			op1L = MoveConstantToRegister(context, op1L);
			op1H = MoveConstantToRegister(context, op1H);
			op2L = MoveConstantToRegister(context, op2L);
			op2H = MoveConstantToRegister(context, op2H);

			//umull		low, v1 <= op1l, op2l
			//mla		v2, <= op1l, op2h, v1
			//mla		high, <= op1h, op2l, v2

			context.SetInstruction2(ARMv8A32.UMull, v1, resultLow, op1L, op2L);
			context.AppendInstruction(ARMv8A32.Mla, v2, op1L, op2H, v1);
			context.AppendInstruction(ARMv8A32.Mla, resultHigh, op1H, op2L, v2);
		}

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

		public void SplitLongOperand(Operand operand, out Operand operandLow, out Operand operandHigh)
		{
			MethodCompiler.SplitLongOperand(operand, out operandLow, out operandHigh);
		}

		#endregion Helper Methods
	}
}
