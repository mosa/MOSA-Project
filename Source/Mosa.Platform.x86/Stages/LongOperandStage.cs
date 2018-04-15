// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;
using System.Diagnostics;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	/// Transforms 64-bit Arith to 32-bit operations.
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
			AddVisitation(IRInstruction.AddSigned64, AddUnsigned64);
			AddVisitation(IRInstruction.AddUnsigned64, AddUnsigned64);
			AddVisitation(IRInstruction.ArithShiftRight64, ArithShiftRight64);
			AddVisitation(IRInstruction.Call, Call);
			AddVisitation(IRInstruction.CompareInt64x32, CompareInteger64x32);
			AddVisitation(IRInstruction.CompareInt64x64, CompareInteger64x64);
			AddVisitation(IRInstruction.CompareIntBranch64, CompareIntegerBranch64);
			AddVisitation(IRInstruction.ConvertFloatR4ToInt64, ConvertFloatR4ToInt64);
			AddVisitation(IRInstruction.ConvertFloatR8ToInt64, ConvertFloatR8ToInteger64);
			AddVisitation(IRInstruction.ConvertInt64ToFloatR4, ConvertInteger64ToFloatR4);
			AddVisitation(IRInstruction.ConvertInt64ToFloatR8, ConvertInteger64ToFloatR8);
			AddVisitation(IRInstruction.LoadInt64, LoadInteger64);
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
			AddVisitation(IRInstruction.StoreInt64, StoreInteger64);
			AddVisitation(IRInstruction.StoreParamInt64, StoreParamInteger64);
			AddVisitation(IRInstruction.SubSigned64, SubUnsigned64);
			AddVisitation(IRInstruction.SubUnsigned64, SubUnsigned64);
			AddVisitation(IRInstruction.To64, To64);
			AddVisitation(IRInstruction.Truncation64x32, Truncation64x32);
			AddVisitation(IRInstruction.ZeroExtend16x64, ZeroExtended16x64);
			AddVisitation(IRInstruction.ZeroExtend32x64, ZeroExtended32x64);
			AddVisitation(IRInstruction.ZeroExtend8x64, ZeroExtended8x64);
		}

		protected override void Setup()
		{
			base.Setup();

			Constant4 = CreateConstant(4);
		}

		#region Visitation Methods

		private void AddUnsigned64(Context context)
		{
			SplitLongOperand(context.Result, out Operand op0L, out Operand op0H);
			SplitLongOperand(context.Operand1, out Operand op1L, out Operand op1H);
			SplitLongOperand(context.Operand2, out Operand op2L, out Operand op2H);

			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var v2 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);

			context.SetInstruction(X86.Mov32, v2, op1L);
			context.AppendInstruction(X86.Add32, v2, v2, op2L);
			context.AppendInstruction(X86.Mov32, op0L, v2);
			context.AppendInstruction(X86.Mov32, v1, op1H);
			context.AppendInstruction(X86.Adc32, v1, v1, op2H);

			if (!op0H.IsConstantZero)
			{
				context.AppendInstruction(X86.Mov32, op0H, v1);
			}
		}

		private void ArithShiftRight64(Context context)
		{
			var count = context.Operand2;

			SplitLongOperand(context.Result, out Operand op0L, out Operand op0H);
			SplitLongOperand(context.Operand1, out Operand op1L, out Operand op1H);

			var eax = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var edx = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var ecx = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			var newBlocks = CreateNewBlockContexts(6);
			var nextBlock = Split(context);

			context.SetInstruction(X86.Jmp, newBlocks[0].Block);

			newBlocks[0].AppendInstruction(X86.Mov32, ecx, count);
			newBlocks[0].AppendInstruction(X86.Mov32, edx, op1H);
			newBlocks[0].AppendInstruction(X86.Mov32, eax, op1L);
			newBlocks[0].AppendInstruction(X86.CmpConst32, null, ecx, CreateConstant(64));
			newBlocks[0].AppendInstruction(X86.BranchUnsignedGreaterOrEqual, newBlocks[4].Block);
			newBlocks[0].AppendInstruction(X86.Jmp, newBlocks[1].Block);

			newBlocks[1].AppendInstruction(X86.CmpConst32, null, ecx, CreateConstant(32));
			newBlocks[1].AppendInstruction(X86.BranchUnsignedGreaterOrEqual, newBlocks[3].Block);
			newBlocks[1].AppendInstruction(X86.Jmp, newBlocks[2].Block);

			newBlocks[2].AppendInstruction(X86.Shrd32, eax, eax, edx, ecx);
			newBlocks[2].AppendInstruction(X86.Sar32, edx, edx, ecx);
			newBlocks[2].AppendInstruction(X86.Jmp, newBlocks[5].Block);

			newBlocks[3].AppendInstruction(X86.Mov32, eax, edx);
			newBlocks[3].AppendInstruction(X86.SarConst32, edx, edx, CreateConstant(0x1F));
			newBlocks[3].AppendInstruction(X86.AndConst32, ecx, ecx, CreateConstant(0x1F));
			newBlocks[3].AppendInstruction(X86.Sar32, eax, eax, ecx);
			newBlocks[3].AppendInstruction(X86.Jmp, newBlocks[5].Block);

			newBlocks[4].AppendInstruction(X86.SarConst32, edx, edx, CreateConstant(0x1F));
			newBlocks[4].AppendInstruction(X86.Mov32, eax, edx);
			newBlocks[4].AppendInstruction(X86.Jmp, newBlocks[5].Block);

			newBlocks[5].AppendInstruction(X86.Mov32, op0H, edx);
			newBlocks[5].AppendInstruction(X86.Mov32, op0L, eax);
			newBlocks[5].AppendInstruction(X86.Jmp, nextBlock.Block);
		}

		private void Call(Context context)
		{
			if (context.Result?.Is64BitInteger == true)
			{
				SplitLongOperand(context.Result, out Operand op0L, out Operand op0H);
			}

			foreach (var operand in context.Operands)
			{
				if (operand.Is64BitInteger)
				{
					SplitLongOperand(operand, out Operand op0L, out Operand op0H);
				}
			}
		}

		private void CompareInteger64x32(Context context)
		{
			CompareInteger64x64(context);
		}

		private void CompareInteger64x64(Context context)
		{
			var op0 = context.Result;
			var op1 = context.Operand1;
			var op2 = context.Operand2;

			Debug.Assert(op1 != null && op2 != null);
			Debug.Assert(op0.IsVirtualRegister);

			SplitLongOperand(op1, out Operand op1L, out Operand op1H);
			SplitLongOperand(op2, out Operand op2L, out Operand op2H);

			var branch = IRTransformationStage.GetBranch(context.ConditionCode);
			var branchUnsigned = IRTransformationStage.GetBranch(context.ConditionCode.GetUnsigned());

			var nextBlock = Split(context);
			var newBlocks = CreateNewBlockContexts(4);

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
			newBlocks[2].AppendInstruction(X86.MovConst32, op0, CreateConstant(1));
			newBlocks[2].AppendInstruction(X86.Jmp, nextBlock.Block);

			// Failed
			newBlocks[3].AppendInstruction(X86.MovConst32, op0, ConstantZero);
			newBlocks[3].AppendInstruction(X86.Jmp, nextBlock.Block);
		}

		private void ConvertFloatR4ToInt64(Context context)
		{
			SplitLongOperand(context.Result, out Operand resultLow, out Operand resultHigh);

			context.SetInstruction(X86.Cvttss2si, resultLow, context.Operand1);
			context.AppendInstruction(X86.MovConst32, resultHigh, ConstantZero);
		}

		private void ConvertFloatR8ToInteger64(Context context)
		{
			SplitLongOperand(context.Result, out Operand resultLow, out Operand resultHigh);

			context.SetInstruction(X86.Cvttsd2si, resultLow, context.Operand1);
			context.AppendInstruction(X86.MovConst32, resultHigh, ConstantZero);
		}

		private void ConvertInteger64ToFloatR4(Context context)
		{
			SplitLongOperand(context.Result, out Operand op1Low, out Operand op1High);

			context.SetInstruction(X86.Cvtsi2ss, context.Result, op1Low);
		}

		private void ConvertInteger64ToFloatR8(Context context)
		{
			SplitLongOperand(context.Result, out Operand op1Low, out Operand op1High);

			context.SetInstruction(X86.Cvtsi2sd, context.Result, op1Low);
		}

		private void CompareIntegerBranch64(Context context)
		{
			SplitLongOperand(context.Operand1, out Operand op1L, out Operand op1H);
			SplitLongOperand(context.Operand2, out Operand op2L, out Operand op2H);

			var target = context.BranchTargets[0];

			var branch = IRTransformationStage.GetBranch(context.ConditionCode);
			var branchUnsigned = IRTransformationStage.GetBranch(context.ConditionCode.GetUnsigned());

			var nextBlock = Split(context);
			var newBlocks = CreateNewBlockContexts(2);

			// FIXME: If the conditional branch and unconditional branch are the same, this could cause a problem
			target.PreviousBlocks.Remove(context.Block);

			// The block is being split on the condition, so the new next block has one too many next blocks!
			nextBlock.Block.NextBlocks.Remove(target);

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

		private void ExpandMul(Context context)
		{
			SplitLongOperand(context.Result, out Operand op0L, out Operand op0H);
			SplitLongOperand(context.Operand1, out Operand op1L, out Operand op1H);
			SplitLongOperand(context.Operand2, out Operand op2L, out Operand op2H);

			var eax = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var edx = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var ebx = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			var v20 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var v12 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			// unoptimized
			context.SetInstruction(X86.Mov32, eax, op2L);
			context.AppendInstruction(X86.Mov32, v20, eax);
			context.AppendInstruction(X86.Mov32, eax, v20);
			context.AppendInstruction2(X86.Mul32, edx, eax, eax, op1L);
			context.AppendInstruction(X86.Mov32, op0L, eax);

			if (!op0H.IsConstantZero)
			{
				context.AppendInstruction(X86.Mov32, v12, edx);
				context.AppendInstruction(X86.Mov32, eax, op1L);
				context.AppendInstruction(X86.Mov32, ebx, eax);
				context.AppendInstruction(X86.IMul, ebx, ebx, op2H);
				context.AppendInstruction(X86.Mov32, eax, v12);
				context.AppendInstruction(X86.Add32, eax, eax, ebx);
				context.AppendInstruction(X86.Mov32, ebx, op2L);
				context.AppendInstruction(X86.IMul, ebx, ebx, op1H);
				context.AppendInstruction(X86.Add32, eax, eax, ebx);
				context.AppendInstruction(X86.Mov32, v12, eax);
				context.AppendInstruction(X86.Mov32, op0H, v12);
			}
		}

		private void LoadInteger64(Context context)
		{
			var address = context.Operand1;
			var offset = context.Operand2;

			SplitLongOperand(context.Result, out Operand op0L, out Operand op0H);

			context.SetInstruction(X86.MovLoad32, op0L, address, offset);

			if (offset.IsResolvedConstant)
			{
				var offset2 = offset.IsConstantZero ? Constant4 : CreateConstant(offset.Offset + NativePointerSize);
				context.AppendInstruction(X86.MovLoad32, op0H, address, offset2);
				return;
			}

			SplitLongOperand(offset, out Operand op2L, out Operand op2H);

			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);

			context.AppendInstruction(X86.AddConst32, v1, op2L, Constant4);
			context.AppendInstruction(X86.MovLoad32, op0H, address, v1);
		}

		private void LoadParamInt64(Context context)
		{
			SplitLongOperand(context.Result, out Operand lowResult, out Operand hiResult);
			SplitLongOperand(context.Operand1, out Operand lowOffset, out Operand highOffset);

			context.SetInstruction(X86.MovLoad32, lowResult, StackFrame, lowOffset);
			context.AppendInstruction(X86.MovLoad32, hiResult, StackFrame, highOffset);
		}

		private void LoadParamSignExtend16x64(Context context)
		{
			SplitLongOperand(context.Result, out Operand lowResult, out Operand hiResult);
			SplitLongOperand(context.Operand1, out Operand lowOffset, out Operand highOffset);

			context.SetInstruction(X86.MovsxLoad16, lowResult, StackFrame, lowOffset);
			context.AppendInstruction2(X86.Cdq, hiResult, lowResult, lowResult);
		}

		private void LoadParamSignExtend32x64(Context context)
		{
			SplitLongOperand(context.Result, out Operand lowResult, out Operand hiResult);
			SplitLongOperand(context.Operand1, out Operand lowOffset, out Operand highOffset);

			context.SetInstruction(X86.MovLoad32, lowResult, StackFrame, lowOffset);
			context.AppendInstruction2(X86.Cdq, hiResult, lowResult, lowResult);
		}

		private void LoadParamSignExtend8x64(Context context)
		{
			SplitLongOperand(context.Result, out Operand lowResult, out Operand hiResult);
			SplitLongOperand(context.Operand1, out Operand lowOffset, out Operand highOffset);

			context.SetInstruction(X86.MovsxLoad8, lowResult, StackFrame, lowOffset);
			context.AppendInstruction2(X86.Cdq, hiResult, lowResult, lowResult);
		}

		private void LoadParamZeroExtended16x64(Context context)
		{
			SplitLongOperand(context.Result, out Operand lowResult, out Operand hiResult);
			SplitLongOperand(context.Operand1, out Operand lowOffset, out Operand highOffset);

			context.SetInstruction(X86.MovLoad16, lowResult, StackFrame, lowOffset);
			context.AppendInstruction(X86.MovConst32, hiResult, ConstantZero);
		}

		private void LoadParamZeroExtended32x64(Context context)
		{
			SplitLongOperand(context.Result, out Operand lowResult, out Operand hiResult);
			SplitLongOperand(context.Operand1, out Operand lowOffset, out Operand highOffset);

			context.SetInstruction(X86.MovLoad32, lowResult, StackFrame, lowOffset);
			context.AppendInstruction(X86.MovConst32, hiResult, ConstantZero);
		}

		private void LoadParamZeroExtended8x64(Context context)
		{
			SplitLongOperand(context.Result, out Operand lowResult, out Operand hiResult);
			SplitLongOperand(context.Operand1, out Operand lowOffset, out Operand highOffset);

			context.SetInstruction(X86.MovLoad8, lowResult, StackFrame, lowOffset);
			context.AppendInstruction(X86.MovConst32, hiResult, ConstantZero);
		}

		private void LogicalAnd64(Context context)
		{
			SplitLongOperand(context.Result, out Operand op0L, out Operand op0H);
			SplitLongOperand(context.Operand1, out Operand op1L, out Operand op1H);
			SplitLongOperand(context.Operand2, out Operand op2L, out Operand op2H);

			if (!context.Result.Is64BitInteger)
			{
				context.SetInstruction(X86.Mov32, op0L, op1L);
				context.AppendInstruction(X86.And32, op0L, op0L, op2L);
			}
			else
			{
				context.SetInstruction(X86.Mov32, op0H, op1H);
				context.AppendInstruction(X86.Mov32, op0L, op1L);
				context.AppendInstruction(X86.And32, op0H, op0H, op2H);
				context.AppendInstruction(X86.And32, op0L, op0L, op2L);
			}
		}

		private void LogicalNot64(Context context)
		{
			SplitLongOperand(context.Result, out Operand op0L, out Operand op0H);
			SplitLongOperand(context.Operand1, out Operand op1L, out Operand op1H);

			var eax = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);

			context.SetInstruction(X86.Mov32, eax, op1H);
			context.AppendInstruction(X86.Not32, eax, eax);
			context.AppendInstruction(X86.Mov32, op0H, eax);

			context.AppendInstruction(X86.Mov32, eax, op1L);
			context.AppendInstruction(X86.Not32, eax, eax);
			context.AppendInstruction(X86.Mov32, op0L, eax);
		}

		private void LogicalOr64(Context context)
		{
			SplitLongOperand(context.Result, out Operand op0L, out Operand op0H);
			SplitLongOperand(context.Operand1, out Operand op1L, out Operand op1H);
			SplitLongOperand(context.Operand2, out Operand op2L, out Operand op2H);

			context.SetInstruction(X86.Mov32, op0H, op1H);
			context.AppendInstruction(X86.Mov32, op0L, op1L);
			context.AppendInstruction(X86.Or32, op0H, op0H, op2H);
			context.AppendInstruction(X86.Or32, op0L, op0L, op2L);
		}

		private void LogicalXor64(Context context)
		{
			SplitLongOperand(context.Result, out Operand op0L, out Operand op0H);
			SplitLongOperand(context.Operand1, out Operand op1L, out Operand op1H);
			SplitLongOperand(context.Operand2, out Operand op2L, out Operand op2H);

			context.SetInstruction(X86.Mov32, op0H, op1H);
			context.AppendInstruction(X86.Mov32, op0L, op1L);
			context.AppendInstruction(X86.Xor32, op0H, op0H, op2H);
			context.AppendInstruction(X86.Xor32, op0L, op0L, op2L);
		}

		private void MoveInteger64(Context context)
		{
			SplitLongOperand(context.Operand1, out Operand op1L, out Operand op1H);
			SplitLongOperand(context.Result, out Operand op0L, out Operand op0H);

			context.SetInstruction(X86.Mov32, op0L, op1L);
			context.AppendInstruction(X86.Mov32, op0H, op1H);
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
			var count = context.Operand2;

			SplitLongOperand(context.Result, out Operand op0L, out Operand op0H);
			SplitLongOperand(context.Operand1, out Operand op1L, out Operand op1H);

			var eax = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var edx = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var ecx = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			var nextBlock = Split(context);
			var newBlocks = CreateNewBlockContexts(6);

			context.SetInstruction(X86.Jmp, newBlocks[0].Block);

			newBlocks[0].AppendInstruction(X86.Mov32, ecx, count);
			newBlocks[0].AppendInstruction(X86.Mov32, edx, op1H);
			newBlocks[0].AppendInstruction(X86.Mov32, eax, op1L);
			newBlocks[0].AppendInstruction(X86.CmpConst32, null, ecx, CreateConstant(64));
			newBlocks[0].AppendInstruction(X86.BranchUnsignedGreaterOrEqual, newBlocks[4].Block);
			newBlocks[0].AppendInstruction(X86.Jmp, newBlocks[1].Block);

			newBlocks[1].AppendInstruction(X86.CmpConst32, null, ecx, CreateConstant(32));
			newBlocks[1].AppendInstruction(X86.BranchUnsignedGreaterOrEqual, newBlocks[3].Block);
			newBlocks[1].AppendInstruction(X86.Jmp, newBlocks[2].Block);

			newBlocks[2].AppendInstruction(X86.Shld32, edx, edx, eax, ecx);
			newBlocks[2].AppendInstruction(X86.Shl32, eax, eax, ecx);
			newBlocks[2].AppendInstruction(X86.Jmp, newBlocks[5].Block);

			newBlocks[3].AppendInstruction(X86.Mov32, edx, eax);
			newBlocks[3].AppendInstruction(X86.MovConst32, eax, ConstantZero);
			newBlocks[3].AppendInstruction(X86.AndConst32, ecx, ecx, CreateConstant(0x1F));
			newBlocks[3].AppendInstruction(X86.Shl32, edx, edx, ecx);
			newBlocks[3].AppendInstruction(X86.Jmp, newBlocks[5].Block);

			newBlocks[4].AppendInstruction(X86.MovConst32, eax, ConstantZero);
			newBlocks[4].AppendInstruction(X86.MovConst32, edx, ConstantZero);
			newBlocks[4].AppendInstruction(X86.Jmp, newBlocks[5].Block);

			newBlocks[5].AppendInstruction(X86.Mov32, op0H, edx);
			newBlocks[5].AppendInstruction(X86.Mov32, op0L, eax);
			newBlocks[5].AppendInstruction(X86.Jmp, nextBlock.Block);
		}

		private void ShiftRight64(Context context)
		{
			var count = context.Operand2;

			SplitLongOperand(context.Result, out Operand op0L, out Operand op0H);
			SplitLongOperand(context.Operand1, out Operand op1L, out Operand op1H);

			var ecx = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			var nextBlock = Split(context);
			var newBlocks = CreateNewBlockContexts(4);

			context.SetInstruction(X86.Mov32, ecx, count);
			context.AppendInstruction(X86.CmpConst32, null, ecx, CreateConstant(64));
			context.AppendInstruction(X86.BranchUnsignedGreaterOrEqual, newBlocks[3].Block);
			context.AppendInstruction(X86.Jmp, newBlocks[0].Block);

			newBlocks[0].AppendInstruction(X86.CmpConst32, null, ecx, CreateConstant(32));
			newBlocks[0].AppendInstruction(X86.BranchUnsignedGreaterOrEqual, newBlocks[2].Block);
			newBlocks[0].AppendInstruction(X86.Jmp, newBlocks[1].Block);

			newBlocks[1].AppendInstruction(X86.Mov32, v1, op1H);
			newBlocks[1].AppendInstruction(X86.Mov32, op0L, op1L);
			newBlocks[1].AppendInstruction(X86.Shrd32, op0L, op0L, v1, ecx);
			newBlocks[1].AppendInstruction(X86.Shr32, v1, v1, ecx);
			if (!op0H.IsConstantZero)
				newBlocks[1].AppendInstruction(X86.Mov32, op0H, v1);
			newBlocks[1].AppendInstruction(X86.Jmp, nextBlock.Block);

			newBlocks[2].AppendInstruction(X86.Mov32, op0L, op1H);
			if (!op0H.IsConstantZero)
				newBlocks[2].AppendInstruction(X86.MovConst32, op0H, ConstantZero);
			newBlocks[2].AppendInstruction(X86.AndConst32, ecx, ecx, CreateConstant(0x1F));
			newBlocks[2].AppendInstruction(X86.Sar32, op0L, op0L, ecx);
			newBlocks[2].AppendInstruction(X86.Jmp, nextBlock.Block);

			newBlocks[3].AppendInstruction(X86.Mov32, op0L, op0H);
			if (!op0H.IsConstantZero)
				newBlocks[3].AppendInstruction(X86.MovConst32, op0H, ConstantZero);
			newBlocks[3].AppendInstruction(X86.Jmp, nextBlock.Block);
		}

		private void SignExtend16x64(Context context)
		{
			Operand op0 = context.Result;
			Operand op1 = context.Operand1;

			SplitLongOperand(op0, out Operand op0L, out Operand op0H);

			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var v2 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);
			var v3 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);

			context.SetInstruction(X86.Movsx16To32, v1, op1);
			context.AppendInstruction2(X86.Cdq, v3, v2, v1);
			context.AppendInstruction(X86.Mov32, op0L, v2);
			context.AppendInstruction(X86.Mov32, op0H, v3);
		}

		private void SignExtend32x64(Context context)
		{
			Operand op0 = context.Result;
			Operand op1 = context.Operand1;

			SplitLongOperand(op0, out Operand op0L, out Operand op0H);

			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var v2 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);
			var v3 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);

			context.SetInstruction(X86.Mov32, v1, op1);
			context.AppendInstruction2(X86.Cdq, v3, v2, v1);
			context.AppendInstruction(X86.Mov32, op0L, v2);
			context.AppendInstruction(X86.Mov32, op0H, v3);
		}

		private void SignExtend8x64(Context context)
		{
			Operand op0 = context.Result;
			Operand op1 = context.Operand1;

			SplitLongOperand(op0, out Operand op0L, out Operand op0H);

			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var v2 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);
			var v3 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);

			context.SetInstruction(X86.Movsx8To32, v1, op1);
			context.AppendInstruction2(X86.Cdq, v3, v2, v1);
			context.AppendInstruction(X86.Mov32, op0L, v2);
			context.AppendInstruction(X86.Mov32, op0H, v3);
		}

		private void __Split64(Context context)
		{
			var operand1 = context.Operand1;
			var result = context.Result;
			var result2 = context.Result2;

			SplitLongOperand(operand1, out Operand op0L, out Operand op0H);

			context.SetInstruction(X86.Mov32, result, op0L);
			context.AppendInstruction(X86.Mov32, result2, op0H);
		}

		private void GetLow64(Context context)
		{
			SplitLongOperand(context.Operand1, out Operand op0L, out Operand op0H);

			context.SetInstruction(X86.Mov32, context.Result, op0L);
		}

		private void GetHigh64(Context context)
		{
			SplitLongOperand(context.Operand1, out Operand op0L, out Operand op0H);

			context.SetInstruction(X86.Mov32, context.Result, op0H);
		}

		private void StoreInteger64(Context context)
		{
			var address = context.Operand1;
			var offset = context.Operand2;

			SplitLongOperand(context.Operand3, out Operand op3L, out Operand op3H);

			context.SetInstruction(X86.MovStore32, null, address, offset, op3L);

			if (offset.IsResolvedConstant)
			{
				var offset2 = offset.IsConstantZero ? Constant4 : CreateConstant(offset.Offset + NativePointerSize);
				context.AppendInstruction(X86.MovStore32, null, address, offset2, op3H);
				return;
			}

			SplitLongOperand(offset, out Operand op2L, out Operand op2H);

			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);

			context.AppendInstruction(X86.AddConst32, v1, op2L, Constant4);
			context.AppendInstruction(X86.MovStore32, null, address, v1, op3H);
		}

		private void StoreParamInteger64(Context context)
		{
			SplitLongOperand(context.Operand1, out Operand op0L, out Operand op0H);
			SplitLongOperand(context.Operand2, out Operand op1L, out Operand op1H);

			context.SetInstruction(X86.MovStore32, null, StackFrame, op0L, op1L);
			context.AppendInstruction(X86.MovStore32, null, StackFrame, op0H, op1H);
		}

		private void SubUnsigned64(Context context)
		{
			SplitLongOperand(context.Result, out Operand op0L, out Operand op0H);
			SplitLongOperand(context.Operand1, out Operand op1L, out Operand op1H);
			SplitLongOperand(context.Operand2, out Operand op2L, out Operand op2H);

			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var v2 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);

			context.SetInstruction(X86.Mov32, v2, op1L);
			context.AppendInstruction(X86.Sub32, v2, v2, op2L);
			context.AppendInstruction(X86.Mov32, op0L, v2);
			context.AppendInstruction(X86.Mov32, v1, op1H);
			context.AppendInstruction(X86.Sbb32, v1, v1, op2H);

			if (!op0H.IsConstantZero)
			{
				context.AppendInstruction(X86.Mov32, op0H, v1);
			}
		}

		private void To64(Context context)
		{
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var result = context.Result;

			SplitLongOperand(result, out Operand op0L, out Operand op0H);

			context.SetInstruction(X86.Mov32, op0L, operand1);
			context.AppendInstruction(X86.Mov32, op0H, operand2);
		}

		private void Truncation64x32(Context context)
		{
			Debug.Assert(context.Operand1.Is64BitInteger);
			Debug.Assert(!context.Result.Is64BitInteger);

			var result = context.Result;
			var operand1 = context.Operand1;

			SplitLongOperand(operand1, out Operand op1L, out Operand op1H);

			context.SetInstruction(X86.Mov32, result, op1L);
		}

		private void ZeroExtended16x64(Context context)
		{
			var op0 = context.Result;
			var op1 = context.Operand1;

			SplitLongOperand(op0, out Operand op0L, out Operand op0H);
			SplitLongOperand(op1, out Operand op1L, out Operand op1H);

			context.SetInstruction(X86.Movzx16To32, op0L, op1L);
			context.AppendInstruction(X86.MovConst32, op0H, ConstantZero);
		}

		private void ZeroExtended32x64(Context context)
		{
			var op0 = context.Result;
			var op1 = context.Operand1;

			SplitLongOperand(op0, out Operand op0L, out Operand op0H);
			SplitLongOperand(op1, out Operand op1L, out Operand op1H);

			context.SetInstruction(X86.Mov32, op0L, op1L);
			context.AppendInstruction(X86.MovConst32, op0H, ConstantZero);
		}

		private void ZeroExtended8x64(Context context)
		{
			var op0 = context.Result;
			var op1 = context.Operand1;

			SplitLongOperand(op0, out Operand op0L, out Operand op0H);
			SplitLongOperand(op1, out Operand op1L, out Operand op1H);

			context.SetInstruction(X86.Movzx8To32, op0L, op1L);
			context.AppendInstruction(X86.MovConst32, op0H, ConstantZero);
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
