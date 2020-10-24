// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using System.Diagnostics;

namespace Mosa.Platform.ARMv8A32.Stages
{
	/// <summary>
	/// Transforms 64-bit arithmetic to 32-bit operations.
	/// </summary>
	/// <seealso cref="Mosa.Platform.x86.BaseTransformationStage" />
	/// <remarks>
	/// This stage translates all 64-bit operations to appropriate 32-bit operations on
	/// architectures without appropriate 64-bit integral operations.
	/// </remarks>
	public sealed class LongOperandStage : BaseTransformationStage
	{
		protected override void PopulateVisitationDictionary()
		{
			AddVisitation(IRInstruction.Add64, Add64);

			//AddVisitation(IRInstruction.BitCopyFloatR8To64, BitCopyFloatR8To64);
			//AddVisitation(IRInstruction.BitCopy64ToFloatR8, BitCopy64ToFloatR8);
			AddVisitation(IRInstruction.ArithShiftRight64, ArithShiftRight64);
			AddVisitation(IRInstruction.Call, Call);

			//AddVisitation(IRInstruction.Compare32x64, Compare32x64);
			//AddVisitation(IRInstruction.Compare64x32, Compare64x32);
			//AddVisitation(IRInstruction.Compare64x64, Compare64x64);
			//AddVisitation(IRInstruction.Branch64, Branch64);
			//AddVisitation(IRInstruction.ConvertFloatR4To64, ConvertFloatR4To64);
			//AddVisitation(IRInstruction.ConvertFloatR8To64, ConvertFloatR8ToInteger64);
			//AddVisitation(IRInstruction.Convert64ToFloatR4, Convert64ToFloatR4);
			//AddVisitation(IRInstruction.Convert64ToFloatR8, Convert64ToFloatR8);

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

		#region Visitation Methods

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

			var v1 = AllocateVirtualRegister32();
			var v2 = AllocateVirtualRegister32();

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

			var v1 = AllocateVirtualRegister32();
			var v2 = AllocateVirtualRegister32();

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

			var v1 = AllocateVirtualRegister32();

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

			var v1 = AllocateVirtualRegister32();
			var v2 = AllocateVirtualRegister32();
			var v3 = AllocateVirtualRegister32();
			var v4 = AllocateVirtualRegister32();
			var v5 = AllocateVirtualRegister32();

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

			var v1 = AllocateVirtualRegister32();
			var v2 = AllocateVirtualRegister32();
			var v3 = AllocateVirtualRegister32();
			var v4 = AllocateVirtualRegister32();

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

		#endregion Visitation Methods

		private void ExpandMul(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			SplitLongOperand(context.Operand1, out var op1L, out var op1H);
			SplitLongOperand(context.Operand2, out var op2L, out var op2H);

			var v1 = AllocateVirtualRegister32();
			var v2 = AllocateVirtualRegister32();

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

		#region Utility Methods

		public void SplitLongOperand(Operand operand, out Operand operandLow, out Operand operandHigh)
		{
			MethodCompiler.SplitLongOperand(operand, out operandLow, out operandHigh);
		}

		#endregion Utility Methods
	}
}
