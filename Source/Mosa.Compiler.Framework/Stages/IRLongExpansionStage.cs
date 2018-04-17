// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.MosaTypeSystem;
using System.Diagnostics;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// IR Long Expansion Stage
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.BaseCodeTransformationStage" />
	public sealed class IRLongExpansionStage : BaseCodeTransformationStage
	{
		protected override void PopulateVisitationDictionary()
		{
			AddVisitation(IRInstruction.LoadParamInt64, LoadParamInt64);
			AddVisitation(IRInstruction.LoadInt64, LoadInteger64);
			AddVisitation(IRInstruction.LogicalAnd64, LogicalAnd64);
			AddVisitation(IRInstruction.LogicalOr64, LogicalOr64);
			AddVisitation(IRInstruction.LogicalXor64, LogicalXor64);
			AddVisitation(IRInstruction.LogicalNot64, LogicalNot64);
			AddVisitation(IRInstruction.Truncation64x32, Truncation64x32);
			AddVisitation(IRInstruction.ZeroExtend32x64, ZeroExtended32x64);

			//AddVisitation(IRInstruction.CompareInt64x64, CompareInteger64x64);
			//AddVisitation(IRInstruction.CompareInt64x32, CompareInteger64x32);
			AddVisitation(IRInstruction.CompareInt64x32, CompareInteger64x32InSSA);

			//AddVisitation(IRInstruction.CompareIntBranch64, CompareIntBranch64);
		}

		#region Visitation Methods

		private void LoadInteger64(Context context)
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
				context.AppendInstruction(IRInstruction.AddUnsigned32, offset4, offset, CreateConstant(4u));
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
				context.AppendInstruction(IRInstruction.AddUnsigned32, offset4, offset, CreateConstant(4u));
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
				context.AppendInstruction(IRInstruction.AddUnsigned32, offset4, op0Low, CreateConstant(4u));
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

			// TODO: Managed 64bit pointers

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

		private void ZeroExtended32x64(Context context)
		{
			context.SetInstruction(IRInstruction.To64, context.Result, context.Operand1, ConstantZero);
		}

		private void CompareInteger64x32InSSA(Context context)
		{
			Debug.Assert(context.Operand1.Is64BitInteger);
			Debug.Assert(context.Operand2.Is64BitInteger);
			Debug.Assert(!context.Result.Is64BitInteger);

			// Note --- this breaks SSA form

			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			var branch = context.ConditionCode;
			var branchUnsigned = context.ConditionCode.GetUnsigned();

			var nextBlock = Split(context);
			var newBlocks = CreateNewBlockContexts(4);

			// CHALLENGE: To maintain PHI-form, the phi blocks list and phi operands must stay in sync
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

			var success = CreateConstant((uint)1);
			var failed = CreateConstant((uint)0);

			// Branch if check already gave results
			if (branch == ConditionCode.Equal)
			{
				newBlocks[0].AppendInstruction(IRInstruction.Jmp, newBlocks[3].Block); // failed
			}
			else
			{
				newBlocks[0].AppendInstruction(IRInstruction.CompareIntBranch32, branch, null, op0High, op1High, newBlocks[2].Block); // success
				newBlocks[0].AppendInstruction(IRInstruction.Jmp, newBlocks[3].Block); // failed
			}

			// Compare low
			newBlocks[1].AppendInstruction(IRInstruction.CompareIntBranch32, branchUnsigned, null, op0Low, op1Low, newBlocks[2].Block); // success
			newBlocks[1].AppendInstruction(IRInstruction.Jmp, newBlocks[3].Block); // failed

			// Success
			newBlocks[2].AppendInstruction(IRInstruction.Jmp, newBlocks[3].Block);

			// Exit
			newBlocks[3].AppendInstruction(IRInstruction.Phi, result, failed, failed, success);
			newBlocks[3].PhiBlocks = new List<BasicBlock>() { newBlocks[0].Block, newBlocks[1].Block, newBlocks[2].Block };

			newBlocks[3].AppendInstruction(IRInstruction.Jmp, nextBlock.Block);

		}

		private void CompareInteger64x32(Context context)
		{
			Debug.Assert(context.Operand1.Is64BitInteger);
			Debug.Assert(context.Operand2.Is64BitInteger);
			Debug.Assert(!context.Result.Is64BitInteger);

			// Note --- this breaks SSA form

			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			var branch = context.ConditionCode;
			var branchUnsigned = context.ConditionCode.GetUnsigned();

			var nextBlock = Split(context);
			var newBlocks = CreateNewBlockContexts(5);

			// CHALLENGE: To maintain PHI-form, the phi blocks list and phi operands must stay in sync
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

		#endregion Visitation Methods
	}
}
