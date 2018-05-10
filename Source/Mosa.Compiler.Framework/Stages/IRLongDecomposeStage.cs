// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.MosaTypeSystem;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// IR Long Expansion Stage
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.BaseCodeTransformationStage" />
	public sealed class IRLongDecomposeStage : BaseCodeTransformationStage
	{
		private Operand Constant4;

		protected override void PopulateVisitationDictionary()
		{
			AddVisitation(IRInstruction.Add64, Add64);
			AddVisitation(IRInstruction.CompareInt64x32, CompareInteger64x32);
			AddVisitation(IRInstruction.CompareIntBranch64, CompareIntBranch64);
			AddVisitation(IRInstruction.LoadParamInt64, LoadParamInt64);

			AddVisitation(IRInstruction.LoadParamZeroExtend8x64, LoadParamZeroExtend8x64);
			AddVisitation(IRInstruction.LoadParamZeroExtend16x64, LoadParamZeroExtend16x64);
			AddVisitation(IRInstruction.LoadParamZeroExtend32x64, LoadParamZeroExtend32x64);

			AddVisitation(IRInstruction.LoadInt64, LoadInt64);
			AddVisitation(IRInstruction.LogicalAnd64, LogicalAnd64);
			AddVisitation(IRInstruction.LogicalOr64, LogicalOr64);
			AddVisitation(IRInstruction.LogicalXor64, LogicalXor64);
			AddVisitation(IRInstruction.LogicalNot64, LogicalNot64);
			AddVisitation(IRInstruction.StoreInt64, StoreInt64);
			AddVisitation(IRInstruction.Sub64, Sub64);
			AddVisitation(IRInstruction.Truncation64x32, Truncation64x32);
			AddVisitation(IRInstruction.ZeroExtend8x64, ZeroExtend8x64);
			AddVisitation(IRInstruction.ZeroExtend16x64, ZeroExtend16x64);
			AddVisitation(IRInstruction.ZeroExtend32x64, ZeroExtend32x64);

			//AddVisitation(IRInstruction.GetHigh64, GetHigh64);
			//AddVisitation(IRInstruction.GetLow64, GetLow64);
			//AddVisitation(IRInstruction.MoveInt64, MoveInt64);

			//AddVisitation(IRInstruction.CompareInt64x64, CompareInt64x64);
			//AddVisitation(IRInstruction.IfThenElse64, IfThenElse64);
			//AddVisitation(IRInstruction.LoadParamSignExtend16x64, LoadParamSignExtend16x64);
			//AddVisitation(IRInstruction.LoadParamSignExtend32x64, LoadParamSignExtend32x64);
			//AddVisitation(IRInstruction.LoadParamSignExtend8x64, LoadParamSignExtend8x64);
			//AddVisitation(IRInstruction.MulSigned64, MulSigned64);
			//AddVisitation(IRInstruction.MulUnsigned64, MulUnsigned64);
			//AddVisitation(IRInstruction.ShiftLeft64, ShiftLeft64);
			//AddVisitation(IRInstruction.ShiftRight64, ShiftRight64);
			//AddVisitation(IRInstruction.SignExtend16x64, SignExtend16x64);
			//AddVisitation(IRInstruction.SignExtend32x64, SignExtend32x64);
			//AddVisitation(IRInstruction.SignExtend8x64, SignExtend8x64);

			//AddVisitation(IRInstruction.StoreParamInt64, StoreParamInt64);
		}

		protected override void Setup()
		{
			base.Setup();

			Constant4 = CreateConstant(4);
		}

		#region Visitation Methods

		private void StoreInt64(Context context)
		{
			var address = context.Operand1;
			var offset = context.Operand2;
			var value = context.Operand3;

			var valueLow = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var valueHigh = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var offsetLow = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var addressLow = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var offset4 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);

			context.SetInstruction(IRInstruction.GetLow64, valueLow, value);
			context.AppendInstruction(IRInstruction.GetHigh64, valueHigh, value);
			context.AppendInstruction(IRInstruction.GetLow64, offsetLow, offset);
			context.AppendInstruction(IRInstruction.GetLow64, addressLow, address);
			context.AppendInstruction(IRInstruction.StoreInt32, null, addressLow, offset, valueLow);
			context.AppendInstruction(IRInstruction.Add32, offset4, offsetLow, Constant4);
			context.AppendInstruction(IRInstruction.StoreInt32, null, addressLow, offset4, valueHigh);
		}

		private void LoadParamZeroExtend8x64(Context context)
		{
			Debug.Assert(!context.Result.IsR4);
			Debug.Assert(!context.Result.IsR8);
			Debug.Assert(context.Result.Is64BitInteger);

			var result = context.Result;
			var operand1 = context.Operand1;

			MethodCompiler.SplitLongOperand(operand1, out Operand op0Low, out Operand op0High);

			var resultLow = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var resultHigh = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			context.SetInstruction(IRInstruction.LoadParamZeroExtend8x32, resultLow, op0Low);
			context.AppendInstruction(IRInstruction.To64, result, resultLow, ConstantZero);
		}

		private void LoadParamZeroExtend16x64(Context context)
		{
			Debug.Assert(!context.Result.IsR4);
			Debug.Assert(!context.Result.IsR8);
			Debug.Assert(context.Result.Is64BitInteger);

			var result = context.Result;
			var operand1 = context.Operand1;

			MethodCompiler.SplitLongOperand(operand1, out Operand op0Low, out Operand op0High);

			var resultLow = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var resultHigh = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			context.SetInstruction(IRInstruction.LoadParamZeroExtend16x32, resultLow, op0Low);
			context.AppendInstruction(IRInstruction.To64, result, resultLow, ConstantZero);
		}

		private void LoadParamZeroExtend32x64(Context context)
		{
			Debug.Assert(!context.Result.IsR4);
			Debug.Assert(!context.Result.IsR8);
			Debug.Assert(context.Result.Is64BitInteger);

			var result = context.Result;
			var operand1 = context.Operand1;

			MethodCompiler.SplitLongOperand(operand1, out Operand op0Low, out Operand op0High);

			var resultLow = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var resultHigh = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			context.SetInstruction(IRInstruction.LoadParamInt32, resultLow, op0Low);
			context.AppendInstruction(IRInstruction.LoadParamInt32, resultHigh, op0High);
			context.AppendInstruction(IRInstruction.To64, result, resultLow, ConstantZero);
		}

		//private void GetLow64(Context context)
		//{
		//	MethodCompiler.SplitLongOperand(context.Operand1, out Operand op0L, out Operand op0H);

		//	context.SetInstruction(IRInstruction.MoveInt32, context.Result, op0L);
		//}

		//private void GetHigh64(Context context)
		//{
		//	MethodCompiler.SplitLongOperand(context.Operand1, out Operand op0L, out Operand op0H);

		//	context.SetInstruction(IRInstruction.MoveInt32, context.Result, op0H);
		//}

		//private void MoveInt64(Context context)
		//{
		//	MethodCompiler.SplitLongOperand(context.Operand1, out Operand op0L, out Operand op0H);
		//	MethodCompiler.SplitLongOperand(context.Result, out Operand resultLow, out Operand resultHigh);

		//	context.SetInstruction(IRInstruction.MoveInt32, resultLow, op0L);
		//	context.AppendInstruction(IRInstruction.MoveInt32, resultHigh, op0H);
		//}

		private void ZeroExtend8x64(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;

			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			context.SetInstruction(IRInstruction.ZeroExtend8x32, v1, operand1);
			context.AppendInstruction(IRInstruction.To64, context.Result, operand1, ConstantZero);
		}

		private void ZeroExtend16x64(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;

			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			context.SetInstruction(IRInstruction.ZeroExtend16x32, v1, operand1);
			context.AppendInstruction(IRInstruction.To64, context.Result, operand1, ConstantZero);
		}

		private void LoadInt64(Context context)
		{
			Debug.Assert(!context.Result.IsR4);
			Debug.Assert(!context.Result.IsR8);

			Debug.Assert(context.Result.Is64BitInteger);

			var result = context.Result;
			var location = context.Operand1;
			var offset = context.Operand2;

			var resultLow = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var resultHigh = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			if (offset.IsConstant && !offset.IsLong && !location.IsLong)
			{
				var offset4 = CreateConstant(offset.ConstantUnsignedLongInteger + 4u);

				context.SetInstruction(IRInstruction.LoadInt32, resultLow, location, offset);
				context.AppendInstruction(IRInstruction.LoadInt32, resultHigh, location, offset4);
				context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
				return;
			}

			if (offset.IsConstant && !offset.IsLong && location.IsLong)
			{
				var offset4 = CreateConstant(offset.ConstantUnsignedLongInteger + 4u);

				var op0Low = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				var op0High = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

				context.SetInstruction(IRInstruction.GetLow64, op0Low, location);
				context.AppendInstruction(IRInstruction.GetHigh64, op0High, location);
				context.AppendInstruction(IRInstruction.LoadInt32, resultLow, op0Low, offset);
				context.AppendInstruction(IRInstruction.LoadInt32, resultHigh, op0Low, offset4);
				context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
				return;
			}

			if (offset.IsVirtualRegister && !offset.IsLong && !location.IsLong)
			{
				var offset4 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

				context.SetInstruction(IRInstruction.LoadInt32, resultLow, location, offset);
				context.AppendInstruction(IRInstruction.Add32, offset4, offset, CreateConstant(4u));
				context.AppendInstruction(IRInstruction.LoadInt32, resultHigh, location, offset4);
				context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
				return;
			}

			if (offset.IsVirtualRegister && !offset.IsLong && location.IsLong)
			{
				var op0Low = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				var op0High = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				var offset4 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

				context.SetInstruction(IRInstruction.GetLow64, op0Low, location);
				context.AppendInstruction(IRInstruction.GetHigh64, op0High, location);
				context.AppendInstruction(IRInstruction.Add32, offset4, offset, CreateConstant(4u));
				context.AppendInstruction(IRInstruction.LoadInt32, resultLow, op0Low, offset);
				context.AppendInstruction(IRInstruction.LoadInt32, resultHigh, op0Low, offset4);
				context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
				return;
			}

			if (offset.IsConstant && offset.IsLong && !location.IsLong)
			{
				var op0Low = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				var op0High = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				var offset4 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

				context.SetInstruction(IRInstruction.GetLow64, op0Low, offset);
				context.AppendInstruction(IRInstruction.GetHigh64, op0High, offset);
				context.AppendInstruction(IRInstruction.Add32, offset4, op0Low, CreateConstant(4u));
				context.AppendInstruction(IRInstruction.LoadInt32, resultLow, location, op0Low);
				context.AppendInstruction(IRInstruction.LoadInt32, resultHigh, location, offset4);
				context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
				return;
			}
		}

		private void LoadParamInt64(Context context)
		{
			Debug.Assert(!context.Result.IsR4);
			Debug.Assert(!context.Result.IsR8);
			Debug.Assert(context.Result.Is64BitInteger);

			var result = context.Result;
			var operand1 = context.Operand1;

			MethodCompiler.SplitLongOperand(operand1, out Operand op0Low, out Operand op0High);

			var resultLow = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var resultHigh = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			context.SetInstruction(IRInstruction.LoadParamInt32, resultLow, op0Low);
			context.AppendInstruction(IRInstruction.LoadParamInt32, resultHigh, op0High);
			context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
		}

		private void LogicalAnd64(Context context)
		{
			Debug.Assert(context.Result.Is64BitInteger);

			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			var op0Low = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var op0High = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var op1Low = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var op1High = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var resultLow = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var resultHigh = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			context.SetInstruction(IRInstruction.GetLow64, op0Low, operand1);
			context.AppendInstruction(IRInstruction.GetHigh64, op0High, operand1);
			context.AppendInstruction(IRInstruction.GetLow64, op1Low, operand2);
			context.AppendInstruction(IRInstruction.GetHigh64, op1High, operand2);
			context.AppendInstruction(IRInstruction.LogicalAnd32, resultLow, op0Low, op1Low);
			context.AppendInstruction(IRInstruction.LogicalAnd32, resultHigh, op0High, op1High);
			context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
		}

		private void LogicalNot64(Context context)
		{
			Debug.Assert(context.Result.Is64BitInteger);

			var result = context.Result;
			var operand1 = context.Operand1;

			var op0Low = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var op0High = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var resultLow = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var resultHigh = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			context.SetInstruction(IRInstruction.GetLow64, op0Low, operand1);
			context.AppendInstruction(IRInstruction.GetHigh64, op0High, operand1);
			context.AppendInstruction(IRInstruction.LogicalNot32, resultLow, op0Low);
			context.AppendInstruction(IRInstruction.LogicalNot32, resultHigh, op0High);
			context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
		}

		private void LogicalOr64(Context context)
		{
			Debug.Assert(context.Result.Is64BitInteger);

			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			var op0Low = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var op0High = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var op1Low = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var op1High = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var resultLow = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var resultHigh = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			context.SetInstruction(IRInstruction.GetLow64, op0Low, operand1);
			context.AppendInstruction(IRInstruction.GetHigh64, op0High, operand1);
			context.AppendInstruction(IRInstruction.GetLow64, op1Low, operand2);
			context.AppendInstruction(IRInstruction.GetHigh64, op1High, operand2);
			context.AppendInstruction(IRInstruction.LogicalOr32, resultLow, op0Low, op1Low);
			context.AppendInstruction(IRInstruction.LogicalOr32, resultHigh, op0High, op1High);
			context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
		}

		private void LogicalXor64(Context context)
		{
			Debug.Assert(context.Result.Is64BitInteger);

			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			var op0Low = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var op0High = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var op1Low = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var op1High = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var resultLow = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var resultHigh = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			context.SetInstruction(IRInstruction.GetLow64, op0Low, operand1);
			context.AppendInstruction(IRInstruction.GetHigh64, op0High, operand1);
			context.AppendInstruction(IRInstruction.GetLow64, op1Low, operand2);
			context.AppendInstruction(IRInstruction.GetHigh64, op1High, operand2);
			context.AppendInstruction(IRInstruction.LogicalXor32, resultLow, op0Low, op1Low);
			context.AppendInstruction(IRInstruction.LogicalXor32, resultHigh, op0High, op1High);
			context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
		}

		private void Truncation64x32(Context context)
		{
			Debug.Assert(context.Operand1.Is64BitInteger);
			Debug.Assert(!context.Result.Is64BitInteger);

			context.SetInstruction(IRInstruction.GetLow64, context.Result, context.Operand1);
		}

		private void ZeroExtend32x64(Context context)
		{
			context.SetInstruction(IRInstruction.To64, context.Result, context.Operand1, ConstantZero);
		}

		private void CompareInteger64x32(Context context)
		{
			Debug.Assert(context.Operand1.Is64BitInteger);
			Debug.Assert(context.Operand2.Is64BitInteger);
			Debug.Assert(!context.Result.Is64BitInteger);

			var condition = context.ConditionCode;

			if (condition == ConditionCode.Equal || condition == ConditionCode.NotEqual)
			{
				var result = context.Result;
				var operand1 = context.Operand1;
				var operand2 = context.Operand2;

				var op0Low = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				var op0High = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				var op1Low = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				var op1High = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

				var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				var v2 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				var v3 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

				context.SetInstruction(IRInstruction.GetLow64, op0Low, operand1);
				context.AppendInstruction(IRInstruction.GetHigh64, op0High, operand1);
				context.AppendInstruction(IRInstruction.GetLow64, op1Low, operand2);
				context.AppendInstruction(IRInstruction.GetHigh64, op1High, operand2);

				context.AppendInstruction(IRInstruction.LogicalXor32, v1, op0Low, op1Low);
				context.AppendInstruction(IRInstruction.LogicalXor32, v2, op0High, op1High);
				context.AppendInstruction(IRInstruction.LogicalOr32, v3, v1, v2);
				context.AppendInstruction(IRInstruction.CompareInt32x32, condition, result, v3, ConstantZero);

				return;
			}

			CompareInteger64x32Alt(context);
		}

		private void CompareInteger64x32Alt(Context context)
		{
			Debug.Assert(context.Operand1.Is64BitInteger);
			Debug.Assert(context.Operand2.Is64BitInteger);
			Debug.Assert(!context.Result.Is64BitInteger);

			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			var branch = context.ConditionCode;
			var branchUnsigned = context.ConditionCode.GetUnsigned();

			var nextBlock = Split(context);
			var newBlocks = CreateNewBlockContexts(5, context.Label);

			UpdatePhiInstructionTargets(nextBlock.Block.NextBlocks, context.Block, nextBlock.Block);

			var op0Low = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var op0High = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var op1Low = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var op1High = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var resultLow = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			context.SetInstruction(IRInstruction.GetLow64, op0Low, operand1);
			context.AppendInstruction(IRInstruction.GetHigh64, op0High, operand1);
			context.AppendInstruction(IRInstruction.GetLow64, op1Low, operand2);
			context.AppendInstruction(IRInstruction.GetHigh64, op1High, operand2);

			// Compare high
			context.AppendInstruction(IRInstruction.CompareIntBranch32, ConditionCode.Equal, null, op0High, op1High, newBlocks[1].Block);
			context.AppendInstruction(IRInstruction.Jmp, newBlocks[0].Block);

			// Branch if check already gave results
			if (branch == ConditionCode.Equal)
			{
				newBlocks[0].AppendInstruction(IRInstruction.Jmp, newBlocks[3].Block);
			}
			else
			{
				newBlocks[0].AppendInstruction(IRInstruction.CompareIntBranch32, branch, null, op0High, op1High, newBlocks[2].Block);
				newBlocks[0].AppendInstruction(IRInstruction.Jmp, newBlocks[3].Block);
			}

			// Compare low
			newBlocks[1].AppendInstruction(IRInstruction.CompareIntBranch32, branchUnsigned, null, op0Low, op1Low, newBlocks[2].Block);
			newBlocks[1].AppendInstruction(IRInstruction.Jmp, newBlocks[3].Block);

			// Success
			newBlocks[2].AppendInstruction(IRInstruction.MoveInt32, resultLow, CreateConstant((uint)1));
			newBlocks[2].AppendInstruction(IRInstruction.Jmp, newBlocks[4].Block);

			// Failed
			newBlocks[3].AppendInstruction(IRInstruction.MoveInt32, resultLow, CreateConstant((uint)0));
			newBlocks[3].AppendInstruction(IRInstruction.Jmp, newBlocks[4].Block);

			// Exit
			newBlocks[4].AppendInstruction(IRInstruction.MoveInt32, result, resultLow);
			newBlocks[4].AppendInstruction(IRInstruction.Jmp, nextBlock.Block);
		}

		private void CompareIntBranch64(Context context)
		{
			//Debug.Assert(context.Operand1.Is64BitInteger);
			//Debug.Assert(context.Operand2.Is64BitInteger);
			Debug.Assert(context.BranchTargets.Count == 1);

			if (context.Block.NextBlocks.Count == 1)
			{
				context.SetInstruction(IRInstruction.Nop);
				return;
			}

			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			var target = context.BranchTargets[0];

			var branch = context.ConditionCode;
			var branchUnsigned = context.ConditionCode.GetUnsigned();

			var nextBlock = Split(context);
			var newBlocks = CreateNewBlockContexts(3, context.Label);

			UpdatePhiInstructionTargets(nextBlock.Block.NextBlocks, context.Block, newBlocks[2].Block);

			var op0Low = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var op0High = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var op1Low = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var op1High = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			context.SetInstruction(IRInstruction.GetLow64, op0Low, operand1);
			context.AppendInstruction(IRInstruction.GetHigh64, op0High, operand1);
			context.AppendInstruction(IRInstruction.GetLow64, op1Low, operand2);
			context.AppendInstruction(IRInstruction.GetHigh64, op1High, operand2);

			// Compare high (equal)
			context.AppendInstruction(IRInstruction.CompareIntBranch32, ConditionCode.Equal, null, op0High, op1High, newBlocks[1].Block);
			context.AppendInstruction(IRInstruction.Jmp, newBlocks[0].Block);

			// Compare high
			newBlocks[0].AppendInstruction(IRInstruction.CompareIntBranch32, branch, null, op0High, op1High, newBlocks[2].Block);
			newBlocks[0].AppendInstruction(IRInstruction.Jmp, nextBlock.Block);

			// Compare low
			newBlocks[1].AppendInstruction(IRInstruction.CompareIntBranch32, branchUnsigned, null, op0Low, op1Low, newBlocks[2].Block);
			newBlocks[1].AppendInstruction(IRInstruction.Jmp, nextBlock.Block);

			// Target
			newBlocks[2].AppendInstruction(IRInstruction.Jmp, target);
		}

		private void Add64(Context context)
		{
			Debug.Assert(context.Result.Is64BitInteger);

			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			var op0Low = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var op0High = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var op1Low = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var op1High = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var resultLow = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var resultHigh = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			var resultCarry = AllocateVirtualRegister(TypeSystem.BuiltIn.Boolean);

			context.SetInstruction(IRInstruction.GetLow64, op0Low, operand1);
			context.AppendInstruction(IRInstruction.GetHigh64, op0High, operand1);
			context.AppendInstruction(IRInstruction.GetLow64, op1Low, operand2);
			context.AppendInstruction(IRInstruction.GetHigh64, op1High, operand2);

			context.AppendInstruction2(IRInstruction.AddCarryOut32, resultLow, resultCarry, op0Low, op1Low);
			context.AppendInstruction(IRInstruction.AddWithCarry32, resultHigh, op0High, op1High, resultCarry);
			context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
		}

		private void Sub64(Context context)
		{
			Debug.Assert(context.Result.Is64BitInteger);

			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			var op0Low = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var op0High = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var op1Low = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var op1High = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var resultLow = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var resultHigh = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			var resultCarry = AllocateVirtualRegister(TypeSystem.BuiltIn.Boolean);

			context.SetInstruction(IRInstruction.GetLow64, op0Low, operand1);
			context.AppendInstruction(IRInstruction.GetHigh64, op0High, operand1);
			context.AppendInstruction(IRInstruction.GetLow64, op1Low, operand2);
			context.AppendInstruction(IRInstruction.GetHigh64, op1High, operand2);

			context.AppendInstruction2(IRInstruction.SubCarryOut32, resultLow, resultCarry, op0Low, op1Low);
			context.AppendInstruction(IRInstruction.SubWithCarry32, resultHigh, op0High, op1High, resultCarry);
			context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
		}

		#endregion Visitation Methods
	}
}
