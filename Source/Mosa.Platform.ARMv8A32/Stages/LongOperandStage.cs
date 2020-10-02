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
		private Operand Constant1;
		private Operand Constant1F;
		private Operand Constant4;
		private Operand Constant32;
		private Operand Constant64;

		private Operand LSL;
		private Operand LSR;
		private Operand ASR;
		private Operand ROR;

		protected override void PopulateVisitationDictionary()
		{
			AddVisitation(IRInstruction.Add64, Add64);

			//AddVisitation(IRInstruction.BitCopyFloatR8To64, BitCopyFloatR8To64);
			//AddVisitation(IRInstruction.BitCopy64ToFloatR8, BitCopy64ToFloatR8);
			//AddVisitation(IRInstruction.ArithShiftRight64, ArithShiftRight64);
			//AddVisitation(IRInstruction.Call, Call);
			//AddVisitation(IRInstruction.Compare32x64, Compare32x64);
			//AddVisitation(IRInstruction.Compare64x32, Compare64x32);
			//AddVisitation(IRInstruction.Compare64x64, Compare64x64);
			//AddVisitation(IRInstruction.CompareBranch64, CompareBranch64);
			//AddVisitation(IRInstruction.ConvertFloatR4To64, ConvertFloatR4To64);
			//AddVisitation(IRInstruction.ConvertFloatR8To64, ConvertFloatR8ToInteger64);
			//AddVisitation(IRInstruction.Convert64ToFloatR4, Convert64ToFloatR4);
			//AddVisitation(IRInstruction.Convert64ToFloatR8, Convert64ToFloatR8);
			//AddVisitation(IRInstruction.IfThenElse64, IfThenElse64);
			//AddVisitation(IRInstruction.Load64, Load64);
			//AddVisitation(IRInstruction.LoadParam64, LoadParam64);
			//AddVisitation(IRInstruction.LoadParamSignExtend16x64, LoadParamSignExtend16x64);
			//AddVisitation(IRInstruction.LoadParamSignExtend32x64, LoadParamSignExtend32x64);
			//AddVisitation(IRInstruction.LoadParamSignExtend8x64, LoadParamSignExtend8x64);
			//AddVisitation(IRInstruction.LoadParamZeroExtend16x64, LoadParamZeroExtended16x64);
			//AddVisitation(IRInstruction.LoadParamZeroExtend32x64, LoadParamZeroExtended32x64);
			//AddVisitation(IRInstruction.LoadParamZeroExtend8x64, LoadParamZeroExtended8x64);
			AddVisitation(IRInstruction.And64, And64);
			AddVisitation(IRInstruction.Not64, Not64);
			AddVisitation(IRInstruction.Or64, Or64);
			AddVisitation(IRInstruction.Xor64, Xor64);
			AddVisitation(IRInstruction.Move64, Move64);
			AddVisitation(IRInstruction.MulSigned64, MulSigned64);
			AddVisitation(IRInstruction.MulUnsigned64, MulUnsigned64);

			AddVisitation(IRInstruction.ShiftLeft64, ShiftLeft64);
			AddVisitation(IRInstruction.ShiftRight64, ShiftRight64);

			//AddVisitation(IRInstruction.SignExtend16x64, SignExtend16x64);
			//AddVisitation(IRInstruction.SignExtend32x64, SignExtend32x64);
			//AddVisitation(IRInstruction.SignExtend8x64, SignExtend8x64);
			AddVisitation(IRInstruction.GetHigh64, GetHigh64);
			AddVisitation(IRInstruction.GetLow64, GetLow64);

			//AddVisitation(IRInstruction.Store64, Store64);
			//AddVisitation(IRInstruction.StoreParam64, StoreParam64);
			AddVisitation(IRInstruction.Sub64, Sub64);
			AddVisitation(IRInstruction.To64, To64);

			AddVisitation(IRInstruction.Truncate64x32, Truncate64x32);
			AddVisitation(IRInstruction.ZeroExtend16x64, ZeroExtended16x64);
			AddVisitation(IRInstruction.ZeroExtend32x64, ZeroExtended32x64);
			AddVisitation(IRInstruction.ZeroExtend8x64, ZeroExtended8x64);
		}

		protected override void Setup()
		{
			Constant1 = CreateConstant(1);
			Constant1F = CreateConstant(0x1F);
			Constant4 = CreateConstant(4);
			Constant32 = CreateConstant(32);
			Constant64 = CreateConstant(64);

			LSL = CreateConstant(0b00);
			LSR = CreateConstant(0b01);
			ASR = CreateConstant(0b10);
			ROR = CreateConstant(0b11);
		}

		#region Visitation Methods

		private void Add64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			SplitLongOperand(context.Operand1, out var op1L, out var op1H);
			SplitLongOperand(context.Operand2, out var op2L, out var op2H);

			TransformInstruction(context, ARMv8A32.Add, ARMv8A32.AddImm, resultLow, StatusRegister.Update, op1L, op2L);
			TransformInstruction(context.InsertAfter(), ARMv8A32.Adc, ARMv8A32.AdcImm, resultHigh, StatusRegister.NotSet, op1H, op2H);
		}

		private void Move64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			SplitLongOperand(context.Operand1, out var op1L, out var op1H);

			TransformInstruction(context, ARMv8A32.Mov, ARMv8A32.MovImm, resultLow, StatusRegister.Update, op1L);
			TransformInstruction(context.InsertAfter(), ARMv8A32.Mov, ARMv8A32.MovImm, resultHigh, StatusRegister.NotSet, op1H);
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

			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var v2 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var v3 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var v4 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var v5 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			var op1l = MoveConstantToRegister(context, op1L);
			var op1h = MoveConstantToRegister(context, op1H);
			var operand2 = context.Operand2;

			context.SetInstruction(ARMv8A32.Mov, v1, op1l);
			context.AppendInstruction(ARMv8A32.SubImm, v2, operand2, Constant32);
			context.AppendInstruction(ARMv8A32.RsbImm, v3, operand2, Constant32);
			context.AppendInstruction(ARMv8A32.Lsl, v4, op1h, operand2);

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

			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var v2 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var v3 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var v4 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			var op1l = MoveConstantToRegister(context, op1L);
			var op1h = MoveConstantToRegister(context, op1H);
			var operand2 = context.Operand2;

			context.SetInstruction(ARMv8A32.Rsb, v1, operand2, Constant32);
			context.AppendInstruction(ARMv8A32.Sub, StatusRegister.Update, v2, operand2, Constant32);
			context.AppendInstruction(ARMv8A32.Lsr, v3, op1l, operand2);
			context.AppendInstruction(ARMv8A32.Orr, v4, v3, op1h, v1, LSL);
			context.AppendInstruction(ARMv8A32.Orr, ConditionCode.Zero, resultLow, v4, op1h, v2, ASR);
			context.AppendInstruction(ARMv8A32.Asr, resultHigh, op1h, operand2);
		}

		private void And64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			SplitLongOperand(context.Operand1, out var op1L, out var op1H);
			SplitLongOperand(context.Operand2, out var op2L, out var op2H);

			TransformInstruction(context, ARMv8A32.And, ARMv8A32.AndImm, resultLow, StatusRegister.NotSet, op1L, op2L);
			TransformInstruction(context.InsertAfter(), ARMv8A32.And, ARMv8A32.AndImm, resultHigh, StatusRegister.NotSet, op1H, op2H);
		}

		private void Not64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			SplitLongOperand(context.Operand1, out var op1L, out var op1H);

			TransformInstruction(context, ARMv8A32.Mvn, ARMv8A32.MvnImm, resultLow, StatusRegister.NotSet, op1L);
			TransformInstruction(context.InsertAfter(), ARMv8A32.Mvn, ARMv8A32.MvnImm, resultHigh, StatusRegister.NotSet, op1H);
		}

		private void Or64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			SplitLongOperand(context.Operand1, out var op1L, out var op1H);
			SplitLongOperand(context.Operand2, out var op2L, out var op2H);

			TransformInstruction(context, ARMv8A32.Orr, ARMv8A32.OrrImm, resultLow, StatusRegister.NotSet, op1L, op2L);
			TransformInstruction(context.InsertAfter(), ARMv8A32.Orr, ARMv8A32.OrrImm, resultHigh, StatusRegister.NotSet, op1H, op2H);
		}

		private void Xor64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			SplitLongOperand(context.Operand1, out var op1L, out var op1H);
			SplitLongOperand(context.Operand2, out var op2L, out var op2H);

			TransformInstruction(context, ARMv8A32.Eor, ARMv8A32.EorImm, resultLow, StatusRegister.NotSet, op1L, op2L);
			TransformInstruction(context.InsertAfter(), ARMv8A32.Eor, ARMv8A32.EorImm, resultHigh, StatusRegister.NotSet, op1H, op2H);
		}

		private void GetHigh64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out _);
			SplitLongOperand(context.Operand1, out _, out var op1H);

			TransformInstruction(context, ARMv8A32.Mov, ARMv8A32.MovImm, resultLow, StatusRegister.Update, op1H);
		}

		private void GetLow64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			SplitLongOperand(context.Operand1, out var op1L, out var op1H);

			TransformInstruction(context, ARMv8A32.Mov, ARMv8A32.MovImm, resultLow, StatusRegister.Update, op1L);
		}

		private void To64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);

			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			TransformInstruction(context, ARMv8A32.Mov, ARMv8A32.MovImm, resultLow, StatusRegister.NotSet, operand1);
			TransformInstruction(context.InsertAfter(), ARMv8A32.Mov, ARMv8A32.MovImm, resultHigh, StatusRegister.NotSet, operand2);
		}

		private void Sub64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			SplitLongOperand(context.Operand1, out var op1L, out var op1H);
			SplitLongOperand(context.Operand2, out var op2L, out var op2H);

			TransformInstruction(context, ARMv8A32.Sub, ARMv8A32.SubImm, resultLow, StatusRegister.Update, op1L, op2L);
			TransformInstruction(context.InsertAfter(), ARMv8A32.Sbc, ARMv8A32.SbcImm, resultHigh, StatusRegister.NotSet, op1H, op2H);
		}

		private void Truncate64x32(Context context)
		{
			Debug.Assert(context.Operand1.Is64BitInteger);
			Debug.Assert(!context.Result.Is64BitInteger);

			SplitLongOperand(context.Result, out var resultLow, out _);
			SplitLongOperand(context.Operand1, out var op1L, out _);

			TransformInstruction(context, ARMv8A32.Mov, ARMv8A32.MovImm, resultLow, StatusRegister.NotSet, op1L);
		}

		private void ZeroExtended16x64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			SplitLongOperand(context.Operand1, out var op1L, out _);

			TransformInstruction(context, ARMv8A32.Mov, ARMv8A32.MovImm, resultLow, StatusRegister.NotSet, op1L);

			var operand1 = MoveConstantToRegister(context, CreateConstant((uint)0xFFFF));
			context.AppendInstruction(ARMv8A32.And, resultLow, resultLow, operand1);

			context.AppendInstruction(ARMv8A32.MovImm, resultHigh, ConstantZero32);
		}

		private void ZeroExtended32x64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			SplitLongOperand(context.Operand1, out var op1L, out _);

			TransformInstruction(context, ARMv8A32.Mov, ARMv8A32.MovImm, resultLow, StatusRegister.NotSet, op1L);
			context.AppendInstruction(ARMv8A32.MovImm, resultHigh, ConstantZero32);
		}

		private void ZeroExtended8x64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			SplitLongOperand(context.Operand1, out var op1L, out _);

			TransformInstruction(context, ARMv8A32.Mov, ARMv8A32.MovImm, resultLow, StatusRegister.NotSet, op1L);

			var operand1 = MoveConstantToRegister(context, CreateConstant((uint)0xFF));
			context.AppendInstruction(ARMv8A32.And, resultLow, resultLow, operand1);

			context.AppendInstruction(ARMv8A32.MovImm, resultHigh, ConstantZero32);
		}

		#endregion Visitation Methods

		private void ExpandMul(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			SplitLongOperand(context.Operand1, out var op1L, out var op1H);
			SplitLongOperand(context.Operand2, out var op2L, out var op2H);

			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var v2 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			var op1l = MoveConstantToRegister(context, op1L);
			var op1h = MoveConstantToRegister(context, op1H);
			var op2l = MoveConstantToRegister(context, op2L);
			var op2h = MoveConstantToRegister(context, op2H);

			//umull		low, v1 <= op1l, op2l
			//mla		v2, <= op1l, op2h, v1
			//mla		high, <= op1h, op2l, v2

			context.SetInstruction2(ARMv8A32.UMull, v1, resultLow, op1l, op2l);
			context.AppendInstruction(ARMv8A32.Mla, v2, op1l, op2h, v1);
			context.AppendInstruction(ARMv8A32.Mla, resultHigh, op1h, op2l, v2);
		}

		#region Utility Methods

		public void SplitLongOperand(Operand operand, out Operand operandLow, out Operand operandHigh)
		{
			MethodCompiler.SplitLongOperand(operand, out operandLow, out operandHigh);
		}

		#endregion Utility Methods
	}
}
