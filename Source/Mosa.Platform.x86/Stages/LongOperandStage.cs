// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;
using System.Diagnostics;

namespace Mosa.Platform.x86.Stages
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
		private Operand Constant4;

		protected override void PopulateVisitationDictionary()
		{
			AddVisitation(IRInstruction.Add64, Add64);
			AddVisitation(IRInstruction.BitCopyFloatR8ToInt64, BitCopyFloatR8ToInt64);
			AddVisitation(IRInstruction.BitCopyInt64ToFloatR8, BitCopyInt64ToFloatR8);
			AddVisitation(IRInstruction.ArithShiftRight64, ArithShiftRight64);
			AddVisitation(IRInstruction.Call, Call);
			AddVisitation(IRInstruction.CompareInt32x64, CompareInt32x64);
			AddVisitation(IRInstruction.CompareInt64x32, CompareInt64x32);
			AddVisitation(IRInstruction.CompareInt64x64, CompareInt64x64);
			AddVisitation(IRInstruction.CompareIntBranch64, CompareIntBranch64);
			AddVisitation(IRInstruction.ConvertFloatR4ToInt64, ConvertFloatR4ToInt64);
			AddVisitation(IRInstruction.ConvertFloatR8ToInt64, ConvertFloatR8ToInteger64);
			AddVisitation(IRInstruction.ConvertInt64ToFloatR4, ConvertInt64ToFloatR4);
			AddVisitation(IRInstruction.ConvertInt64ToFloatR8, ConvertInt64ToFloatR8);
			AddVisitation(IRInstruction.IfThenElse64, IfThenElse64);
			AddVisitation(IRInstruction.LoadInt64, LoadInt64);
			AddVisitation(IRInstruction.LoadParamInt64, LoadParamInt64);
			AddVisitation(IRInstruction.LoadParamSignExtend16x64, LoadParamSignExtend16x64);
			AddVisitation(IRInstruction.LoadParamSignExtend32x64, LoadParamSignExtend32x64);
			AddVisitation(IRInstruction.LoadParamSignExtend8x64, LoadParamSignExtend8x64);
			AddVisitation(IRInstruction.LoadParamZeroExtend16x64, LoadParamZeroExtended16x64);
			AddVisitation(IRInstruction.LoadParamZeroExtend32x64, LoadParamZeroExtended32x64);
			AddVisitation(IRInstruction.LoadParamZeroExtend8x64, LoadParamZeroExtended8x64);
			AddVisitation(IRInstruction.LogicalAnd64, LogicalAnd64);
			AddVisitation(IRInstruction.LogicalNot64, LogicalNot64);
			AddVisitation(IRInstruction.LogicalOr64, LogicalOr64);
			AddVisitation(IRInstruction.LogicalXor64, LogicalXor64);
			AddVisitation(IRInstruction.MoveInt64, MoveInteger64);
			AddVisitation(IRInstruction.MulSigned64, MulSigned64);
			AddVisitation(IRInstruction.MulUnsigned64, MulUnsigned64);
			AddVisitation(IRInstruction.ShiftLeft64, ShiftLeft64);
			AddVisitation(IRInstruction.ShiftRight64, ShiftRight64);
			AddVisitation(IRInstruction.SignExtend16x64, SignExtend16x64);
			AddVisitation(IRInstruction.SignExtend32x64, SignExtend32x64);
			AddVisitation(IRInstruction.SignExtend8x64, SignExtend8x64);
			AddVisitation(IRInstruction.GetHigh64, GetHigh64);
			AddVisitation(IRInstruction.GetLow64, GetLow64);
			AddVisitation(IRInstruction.StoreInt64, StoreInt64);
			AddVisitation(IRInstruction.StoreParamInt64, StoreParamInt64);
			AddVisitation(IRInstruction.Sub64, Sub64);
			AddVisitation(IRInstruction.To64, To64);
			AddVisitation(IRInstruction.Truncation64x32, Truncation64x32);
			AddVisitation(IRInstruction.ZeroExtend16x64, ZeroExtended16x64);
			AddVisitation(IRInstruction.ZeroExtend32x64, ZeroExtended32x64);
			AddVisitation(IRInstruction.ZeroExtend8x64, ZeroExtended8x64);
		}

		protected override void Setup()
		{
			Constant4 = CreateConstant(4);
		}

		#region Visitation Methods

		private void Add64(Context context)
		{
			SplitLongOperand(context.Result, out Operand resultLow, out Operand resultHigh);
			SplitLongOperand(context.Operand1, out Operand op1L, out Operand op1H);
			SplitLongOperand(context.Operand2, out Operand op2L, out Operand op2H);

			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var v2 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);

			context.SetInstruction(X86.Mov32, v2, op1L);
			context.AppendInstruction(X86.Add32, resultLow, v2, op2L);
			context.AppendInstruction(X86.Mov32, v1, op1H);
			context.AppendInstruction(X86.Adc32, v1, v1, op2H);

			if (!resultHigh.IsConstantZero)
			{
				context.AppendInstruction(X86.Mov32, resultHigh, v1);
			}
		}

		private void ArithShiftRight64(Context context)
		{
			SplitLongOperand(context.Result, out Operand resultLow, out Operand resultHigh);
			SplitLongOperand(context.Operand1, out Operand op1L, out Operand op1H);

			var count = context.Operand2;

			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var v2 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var v3 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			var newBlocks = CreateNewBlockContexts(6, context.Label);
			var nextBlock = Split(context);

			context.SetInstruction(X86.Jmp, newBlocks[0].Block);

			newBlocks[0].AppendInstruction(X86.Mov32, v3, count);
			newBlocks[0].AppendInstruction(X86.Mov32, v2, op1H);
			newBlocks[0].AppendInstruction(X86.Mov32, v1, op1L);
			newBlocks[0].AppendInstruction(X86.Cmp32, null, v3, CreateConstant(64));
			newBlocks[0].AppendInstruction(X86.BranchUnsignedGreaterOrEqual, newBlocks[4].Block);
			newBlocks[0].AppendInstruction(X86.Jmp, newBlocks[1].Block);

			newBlocks[1].AppendInstruction(X86.Cmp32, null, v3, CreateConstant(32));
			newBlocks[1].AppendInstruction(X86.BranchUnsignedGreaterOrEqual, newBlocks[3].Block);
			newBlocks[1].AppendInstruction(X86.Jmp, newBlocks[2].Block);

			newBlocks[2].AppendInstruction(X86.Shrd32, v1, v1, v2, v3);
			newBlocks[2].AppendInstruction(X86.Sar32, v2, v2, v3);
			newBlocks[2].AppendInstruction(X86.Jmp, newBlocks[5].Block);

			newBlocks[3].AppendInstruction(X86.Mov32, v1, v2);
			newBlocks[3].AppendInstruction(X86.Sar32, v2, v2, CreateConstant(0x1F));
			newBlocks[3].AppendInstruction(X86.And32, v3, v3, CreateConstant(0x1F));
			newBlocks[3].AppendInstruction(X86.Sar32, v1, v1, v3);
			newBlocks[3].AppendInstruction(X86.Jmp, newBlocks[5].Block);

			newBlocks[4].AppendInstruction(X86.Sar32, v2, v2, CreateConstant(0x1F));
			newBlocks[4].AppendInstruction(X86.Mov32, v1, v2);
			newBlocks[4].AppendInstruction(X86.Jmp, newBlocks[5].Block);

			newBlocks[5].AppendInstruction(X86.Mov32, resultHigh, v2);
			newBlocks[5].AppendInstruction(X86.Mov32, resultLow, v1);
			newBlocks[5].AppendInstruction(X86.Jmp, nextBlock.Block);
		}

		private void BitCopyFloatR8ToInt64(Context context)
		{
			var operand1 = context.Operand1;

			SplitLongOperand(context.Result, out Operand resultLow, out Operand resultHigh);

			context.SetInstruction(X86.Movd, resultLow, operand1);
			context.AppendInstruction(X86.Pextrd32, resultHigh, operand1, CreateConstant(1));
		}

		private void BitCopyInt64ToFloatR8(Context context)
		{
			var result = context.Result;

			SplitLongOperand(context.Operand1, out Operand op1L, out Operand op1H);

			context.SetInstruction(X86.Movd, result, op1L);
			context.AppendInstruction(X86.Pextrd32, result, op1H, CreateConstant(1));
		}

		private void Call(Context context)
		{
			if (context.Result?.Is64BitInteger == true)
			{
				SplitLongOperand(context.Result, out Operand resultLow, out Operand resultHigh);
			}

			foreach (var operand in context.Operands)
			{
				if (operand.Is64BitInteger)
				{
					SplitLongOperand(operand, out Operand op0L, out Operand op0H);
				}
			}
		}

		private void CompareInt32x64(Context context)
		{
			CompareInt32x64(context);
		}

		private void CompareInt64x32(Context context)
		{
			CompareInt64x64(context);
		}

		private void CompareInt64x64(Context context)
		{
			Debug.Assert(context.Operand1 != null && context.Operand2 != null);
			Debug.Assert(context.Result.IsVirtualRegister);

			SplitLongOperand(context.Operand1, out Operand op1L, out Operand op1H);
			SplitLongOperand(context.Operand2, out Operand op2L, out Operand op2H);

			var result = context.Result;

			var branch = IRTransformationStage.GetBranch(context.ConditionCode);
			var branchUnsigned = IRTransformationStage.GetBranch(context.ConditionCode.GetUnsigned());

			var nextBlock = Split(context);
			var newBlocks = CreateNewBlockContexts(4, context.Label);

			// Compare high dwords
			context.SetInstruction(X86.Cmp32, null, op1H, op2H);
			context.AppendInstruction(X86.BranchEqual, newBlocks[1].Block);
			context.AppendInstruction(X86.Jmp, newBlocks[0].Block);

			// Branch if check already gave results
			if (branch == X86.BranchEqual)
			{
				newBlocks[0].AppendInstruction(X86.Jmp, newBlocks[3].Block);
			}
			else
			{
				// Branch if check already gave results
				newBlocks[0].AppendInstruction(branch, newBlocks[2].Block);
				newBlocks[0].AppendInstruction(X86.Jmp, newBlocks[3].Block);
			}

			// Compare low dwords
			newBlocks[1].AppendInstruction(X86.Cmp32, null, op1L, op2L);
			newBlocks[1].AppendInstruction(branchUnsigned, newBlocks[2].Block);
			newBlocks[1].AppendInstruction(X86.Jmp, newBlocks[3].Block);

			// Success
			newBlocks[2].AppendInstruction(X86.Mov32, result, CreateConstant(1));
			newBlocks[2].AppendInstruction(X86.Jmp, nextBlock.Block);

			// Failed
			newBlocks[3].AppendInstruction(X86.Mov32, result, ConstantZero32);
			newBlocks[3].AppendInstruction(X86.Jmp, nextBlock.Block);
		}

		private void CompareIntBranch64(Context context)
		{
			SplitLongOperand(context.Operand1, out Operand op1L, out Operand op1H);
			SplitLongOperand(context.Operand2, out Operand op2L, out Operand op2H);

			var target = context.BranchTargets[0];

			var branch = IRTransformationStage.GetBranch(context.ConditionCode);
			var branchUnsigned = IRTransformationStage.GetBranch(context.ConditionCode.GetUnsigned());

			var nextBlock = Split(context);
			var newBlocks = CreateNewBlockContexts(2, context.Label);

			// Compare high dwords
			context.SetInstruction(X86.Cmp32, null, op1H, op2H);
			context.AppendInstruction(X86.BranchEqual, newBlocks[1].Block);
			context.AppendInstruction(X86.Jmp, newBlocks[0].Block);

			// Branch if check already gave results
			newBlocks[0].AppendInstruction(branch, target);
			newBlocks[0].AppendInstruction(X86.Jmp, nextBlock.Block);

			// Compare low dwords
			newBlocks[1].AppendInstruction(X86.Cmp32, null, op1L, op2L);
			newBlocks[1].AppendInstruction(branchUnsigned, target);
			newBlocks[1].AppendInstruction(X86.Jmp, nextBlock.Block);
		}

		private void ConvertFloatR4ToInt64(Context context)
		{
			SplitLongOperand(context.Result, out Operand resultLow, out Operand resultHigh);

			context.SetInstruction(X86.Cvttss2si32, resultLow, context.Operand1);
			context.AppendInstruction(X86.Mov32, resultHigh, ConstantZero32);
		}

		private void ConvertFloatR8ToInteger64(Context context)
		{
			SplitLongOperand(context.Result, out Operand resultLow, out Operand resultHigh);

			context.SetInstruction(X86.Cvttsd2si32, resultLow, context.Operand1);
			context.AppendInstruction(X86.Mov32, resultHigh, ConstantZero32);
		}

		private void ConvertInt64ToFloatR4(Context context)
		{
			SplitLongOperand(context.Result, out Operand op1Low, out Operand op1High);

			context.SetInstruction(X86.Cvtsi2ss32, context.Result, op1Low);
		}

		private void ConvertInt64ToFloatR8(Context context)
		{
			SplitLongOperand(context.Result, out Operand op1Low, out Operand op1High);

			context.SetInstruction(X86.Cvtsi2sd32, context.Result, op1Low);
		}

		private void ExpandMul(Context context)
		{
			SplitLongOperand(context.Result, out Operand resultLow, out Operand resultHigh);
			SplitLongOperand(context.Operand1, out Operand op1L, out Operand op1H);
			SplitLongOperand(context.Operand2, out Operand op2L, out Operand op2H);

			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var v2 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var v3 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var v4 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			context.SetInstruction(X86.Mov32, v1, op2L);
			context.AppendInstruction2(X86.Mul32, v1, resultLow, v1, op1L);

			if (!resultHigh.IsConstantZero)
			{
				context.AppendInstruction(X86.Mov32, v2, op1L);
				context.AppendInstruction(X86.IMul32, v3, v2, op2H);
				context.AppendInstruction(X86.Add32, v4, v1, v3);
				context.AppendInstruction(X86.Mov32, v3, op2L);
				context.AppendInstruction(X86.IMul32, v3, v3, op1H);
				context.AppendInstruction(X86.Add32, resultHigh, v4, v3);
			}
		}

		private void GetHigh64(Context context)
		{
			SplitLongOperand(context.Operand1, out Operand _, out Operand op0H);

			context.SetInstruction(X86.Mov32, context.Result, op0H);
		}

		private void GetLow64(Context context)
		{
			SplitLongOperand(context.Operand1, out Operand op0L, out Operand _);

			context.SetInstruction(X86.Mov32, context.Result, op0L);
		}

		private void IfThenElse64(Context context)
		{
			SplitLongOperand(context.Result, out Operand resultLow, out Operand resultHigh);
			SplitLongOperand(context.Operand1, out Operand op1L, out Operand op1H);
			SplitLongOperand(context.Operand2, out Operand op2L, out Operand op2H);
			SplitLongOperand(context.Operand3, out Operand op3L, out Operand op3H);

			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			context.SetInstruction(X86.Or32, v1, op1L, op1H);
			context.SetInstruction(X86.Cmp32, null, v1, ConstantZero32);
			context.AppendInstruction(X86.CMovNotEqual32, resultLow, op2L);    // true
			context.AppendInstruction(X86.CMovNotEqual32, resultHigh, op2H);   // true
			context.AppendInstruction(X86.CMovEqual32, resultLow, op3L);       // false
			context.AppendInstruction(X86.CMovEqual32, resultHigh, op3H);      // false
		}

		private void LoadInt64(Context context)
		{
			SplitLongOperand(context.Result, out Operand resultLow, out Operand resultHigh);

			var address = context.Operand1;
			var offset = context.Operand2;

			context.SetInstruction(X86.MovLoad32, resultLow, address, offset);

			if (offset.IsResolvedConstant)
			{
				var offset2 = offset.IsConstantZero ? Constant4 : CreateConstant(offset.Offset + NativePointerSize);
				context.AppendInstruction(X86.MovLoad32, resultHigh, address, offset2);
				return;
			}

			SplitLongOperand(offset, out Operand op2L, out Operand op2H);

			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);

			context.AppendInstruction(X86.Add32, v1, op2L, Constant4);
			context.AppendInstruction(X86.MovLoad32, resultHigh, address, v1);
		}

		private void LoadParamInt64(Context context)
		{
			SplitLongOperand(context.Result, out Operand resultLow, out Operand resultHigh);
			SplitLongOperand(context.Operand1, out Operand lowOffset, out Operand highOffset);

			context.SetInstruction(X86.MovLoad32, resultLow, StackFrame, lowOffset);
			context.AppendInstruction(X86.MovLoad32, resultHigh, StackFrame, highOffset);
		}

		private void LoadParamSignExtend16x64(Context context)
		{
			SplitLongOperand(context.Result, out Operand resultLow, out Operand resultHigh);
			SplitLongOperand(context.Operand1, out Operand lowOffset, out Operand highOffset);

			context.SetInstruction(X86.MovsxLoad16, resultLow, StackFrame, lowOffset);
			context.AppendInstruction2(X86.Cdq32, resultHigh, resultLow, resultLow);
		}

		private void LoadParamSignExtend32x64(Context context)
		{
			SplitLongOperand(context.Result, out Operand resultLow, out Operand resultHigh);
			SplitLongOperand(context.Operand1, out Operand lowOffset, out Operand highOffset);

			context.SetInstruction(X86.MovLoad32, resultLow, StackFrame, lowOffset);
			context.AppendInstruction2(X86.Cdq32, resultHigh, resultLow, resultLow);
		}

		private void LoadParamSignExtend8x64(Context context)
		{
			SplitLongOperand(context.Result, out Operand resultLow, out Operand resultHigh);
			SplitLongOperand(context.Operand1, out Operand lowOffset, out Operand highOffset);

			context.SetInstruction(X86.MovsxLoad8, resultLow, StackFrame, lowOffset);
			context.AppendInstruction2(X86.Cdq32, resultHigh, resultLow, resultLow);
		}

		private void LoadParamZeroExtended16x64(Context context)
		{
			SplitLongOperand(context.Result, out Operand resultLow, out Operand resultHigh);
			SplitLongOperand(context.Operand1, out Operand lowOffset, out Operand highOffset);

			context.SetInstruction(X86.MovLoad16, resultLow, StackFrame, lowOffset);
			context.AppendInstruction(X86.Mov32, resultHigh, ConstantZero32);
		}

		private void LoadParamZeroExtended32x64(Context context)
		{
			SplitLongOperand(context.Result, out Operand resultLow, out Operand resultHigh);
			SplitLongOperand(context.Operand1, out Operand lowOffset, out Operand highOffset);

			context.SetInstruction(X86.MovLoad32, resultLow, StackFrame, lowOffset);
			context.AppendInstruction(X86.Mov32, resultHigh, ConstantZero32);
		}

		private void LoadParamZeroExtended8x64(Context context)
		{
			SplitLongOperand(context.Result, out Operand resultLow, out Operand resultHigh);
			SplitLongOperand(context.Operand1, out Operand lowOffset, out Operand highOffset);

			context.SetInstruction(X86.MovLoad8, resultLow, StackFrame, lowOffset);
			context.AppendInstruction(X86.Mov32, resultHigh, ConstantZero32);
		}

		private void LogicalAnd64(Context context)
		{
			SplitLongOperand(context.Result, out Operand resultLow, out Operand resultHigh);
			SplitLongOperand(context.Operand1, out Operand op1L, out Operand op1H);
			SplitLongOperand(context.Operand2, out Operand op2L, out Operand op2H);

			context.SetInstruction(X86.And32, resultHigh, op1H, op2H);
			context.AppendInstruction(X86.And32, resultLow, op1L, op2L);
		}

		private void LogicalNot64(Context context)
		{
			SplitLongOperand(context.Result, out Operand resultLow, out Operand resultHigh);
			SplitLongOperand(context.Operand1, out Operand op1L, out Operand op1H);

			context.SetInstruction(X86.Not32, resultHigh, op1H);
			context.AppendInstruction(X86.Not32, resultLow, op1L);
		}

		private void LogicalOr64(Context context)
		{
			SplitLongOperand(context.Result, out Operand resultLow, out Operand resultHigh);
			SplitLongOperand(context.Operand1, out Operand op1L, out Operand op1H);
			SplitLongOperand(context.Operand2, out Operand op2L, out Operand op2H);

			context.SetInstruction(X86.Or32, resultHigh, op1H, op2H);
			context.AppendInstruction(X86.Or32, resultLow, op1L, op2L);
		}

		private void LogicalXor64(Context context)
		{
			SplitLongOperand(context.Result, out Operand resultLow, out Operand resultHigh);
			SplitLongOperand(context.Operand1, out Operand op1L, out Operand op1H);
			SplitLongOperand(context.Operand2, out Operand op2L, out Operand op2H);

			context.SetInstruction(X86.Xor32, resultHigh, op1H, op2H);
			context.AppendInstruction(X86.Xor32, resultLow, op1L, op2L);
		}

		private void MoveInteger64(Context context)
		{
			SplitLongOperand(context.Result, out Operand resultLow, out Operand resultHigh);
			SplitLongOperand(context.Operand1, out Operand op1L, out Operand op1H);

			context.SetInstruction(X86.Mov32, resultLow, op1L);
			context.AppendInstruction(X86.Mov32, resultHigh, op1H);
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
			SplitLongOperand(context.Result, out Operand resultLow, out Operand resultHigh);
			SplitLongOperand(context.Operand1, out Operand op1L, out Operand op1H);

			var count = context.Operand2;

			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var v2 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			context.SetInstruction(X86.Shld32, resultHigh, op1H, op1L, count);
			context.AppendInstruction(X86.Mov32, resultLow, ConstantZero32);
			context.AppendInstruction(X86.Shl32, v1, op1L, count);
			context.AppendInstruction(X86.Mov32, v2, count);

			// FUTURE: Optimization - TestConst32 and conditional moves are not necessary if count is a constant
			context.AppendInstruction(X86.Test32, null, v2, CreateConstant(32));
			context.AppendInstruction(X86.CMovNotEqual32, resultHigh, v1);
			context.AppendInstruction(X86.CMovEqual32, resultLow, v1);
		}

		private void ShiftRight64(Context context)
		{
			SplitLongOperand(context.Result, out Operand resultLow, out Operand resultHigh);
			SplitLongOperand(context.Operand1, out Operand op1L, out Operand op1H);

			var count = context.Operand2;

			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var v2 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			context.SetInstruction(X86.Shrd32, resultLow, op1L, op1H, count);
			context.AppendInstruction(X86.Mov32, resultHigh, ConstantZero32);
			context.AppendInstruction(X86.Shr32, v1, op1H, count);
			context.AppendInstruction(X86.Mov32, v2, count);

			// FUTURE: Optimization - TestConst32 and conditional moves are not necessary if count is a constant
			context.AppendInstruction(X86.Test32, null, v2, CreateConstant(32));
			context.AppendInstruction(X86.CMovNotEqual32, resultLow, v1);
			context.AppendInstruction(X86.CMovEqual32, resultHigh, v1);
		}

		private void SignExtend16x64(Context context)
		{
			SplitLongOperand(context.Result, out Operand resultLow, out Operand resultHigh);

			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var v2 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);
			var v3 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);

			context.SetInstruction(X86.Movsx16To32, v1, context.Operand1);
			context.AppendInstruction2(X86.Cdq32, resultHigh, resultLow, v1);
		}

		private void SignExtend32x64(Context context)
		{
			SplitLongOperand(context.Result, out Operand resultLow, out Operand resultHigh);

			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			context.SetInstruction(X86.Mov32, v1, context.Operand1);
			context.AppendInstruction2(X86.Cdq32, resultHigh, resultLow, v1);
		}

		private void SignExtend8x64(Context context)
		{
			SplitLongOperand(context.Result, out Operand resultLow, out Operand resultHigh);

			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			context.SetInstruction(X86.Movsx8To32, v1, context.Operand1);
			context.AppendInstruction2(X86.Cdq32, resultHigh, resultLow, v1);
		}

		private void StoreInt64(Context context)
		{
			SplitLongOperand(context.Operand3, out Operand op3L, out Operand op3H);
			SplitLongOperand(context.Operand2, out Operand op2L, out Operand op2H);

			var address = context.Operand1;
			var offset = context.Operand2;

			context.SetInstruction(X86.MovStore32, null, address, offset, op3L);

			if (offset.IsResolvedConstant)
			{
				var offset2 = offset.IsConstantZero ? Constant4 : CreateConstant(offset.Offset + NativePointerSize);
				context.AppendInstruction(X86.MovStore32, null, address, offset2, op3H);
				return;
			}

			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);

			context.AppendInstruction(X86.Add32, v1, op2L, Constant4);
			context.AppendInstruction(X86.MovStore32, null, address, v1, op3H);
		}

		private void StoreParamInt64(Context context)
		{
			SplitLongOperand(context.Operand1, out Operand op0L, out Operand op0H);
			SplitLongOperand(context.Operand2, out Operand op1L, out Operand op1H);

			context.SetInstruction(X86.MovStore32, null, StackFrame, op0L, op1L);
			context.AppendInstruction(X86.MovStore32, null, StackFrame, op0H, op1H);
		}

		private void Sub64(Context context)
		{
			SplitLongOperand(context.Result, out Operand resultLow, out Operand resultHigh);
			SplitLongOperand(context.Operand1, out Operand op1L, out Operand op1H);
			SplitLongOperand(context.Operand2, out Operand op2L, out Operand op2H);

			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var v2 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);

			context.SetInstruction(X86.Mov32, v2, op1L);
			context.AppendInstruction(X86.Sub32, v2, v2, op2L);
			context.AppendInstruction(X86.Mov32, resultLow, v2);
			context.AppendInstruction(X86.Mov32, v1, op1H);
			context.AppendInstruction(X86.Sbb32, v1, v1, op2H);

			if (!resultHigh.IsConstantZero)
			{
				context.AppendInstruction(X86.Mov32, resultHigh, v1);
			}
		}

		private void To64(Context context)
		{
			SplitLongOperand(context.Result, out Operand resultLow, out Operand resultHigh);

			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			context.SetInstruction(X86.Mov32, resultLow, operand1);
			context.AppendInstruction(X86.Mov32, resultHigh, operand2);
		}

		private void Truncation64x32(Context context)
		{
			Debug.Assert(context.Operand1.Is64BitInteger);
			Debug.Assert(!context.Result.Is64BitInteger);

			SplitLongOperand(context.Operand1, out Operand op1L, out Operand op1H);

			context.SetInstruction(X86.Mov32, context.Result, op1L);
		}

		private void ZeroExtended16x64(Context context)
		{
			SplitLongOperand(context.Result, out Operand resultLow, out Operand resultHigh);
			SplitLongOperand(context.Operand1, out Operand op1L, out Operand op1H);

			context.SetInstruction(X86.Movzx16To32, resultLow, op1L);
			context.AppendInstruction(X86.Mov32, resultHigh, ConstantZero32);
		}

		private void ZeroExtended32x64(Context context)
		{
			SplitLongOperand(context.Result, out Operand resultLow, out Operand resultHigh);
			SplitLongOperand(context.Operand1, out Operand op1L, out Operand op1H);

			context.SetInstruction(X86.Mov32, resultLow, op1L);
			context.AppendInstruction(X86.Mov32, resultHigh, ConstantZero32);
		}

		private void ZeroExtended8x64(Context context)
		{
			SplitLongOperand(context.Result, out Operand resultLow, out Operand resultHigh);
			SplitLongOperand(context.Operand1, out Operand op1L, out Operand op1H);

			context.SetInstruction(X86.Movzx8To32, resultLow, op1L);
			context.AppendInstruction(X86.Mov32, resultHigh, ConstantZero32);
		}

		#endregion Visitation Methods

		#region Utility Methods

		public void SplitLongOperand(Operand operand, out Operand operandLow, out Operand operandHigh)
		{
			MethodCompiler.SplitLongOperand(operand, out operandLow, out operandHigh);
		}

		#endregion Utility Methods
	}
}
