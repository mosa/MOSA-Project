// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// IR Long Decomposition Stage
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.BaseCodeTransformationStage" />
	public sealed class LongExpansionStage : BaseCodeTransformationStage
	{
		private Operand Constant4;

		private bool SSA;

		protected override void Initialize()
		{
			base.Initialize();

			// cache for performance
			SSA = CompilerSettings.SSA;
		}

		protected override void PopulateVisitationDictionary()
		{
			AddVisitation(IRInstruction.Add64, Add64);
			AddVisitation(IRInstruction.Compare64x32, Compare64x32);
			AddVisitation(IRInstruction.CompareBranch64, CompareBranch64);
			AddVisitation(IRInstruction.Load64, Load64); // exposes bug without this
			AddVisitation(IRInstruction.LoadParam64, LoadParam64);
			AddVisitation(IRInstruction.LoadParamSignExtend16x64, LoadParamSignExtend16x64);
			AddVisitation(IRInstruction.LoadParamSignExtend32x64, LoadParamSignExtend32x64);
			AddVisitation(IRInstruction.LoadParamSignExtend8x64, LoadParamSignExtend8x64);
			AddVisitation(IRInstruction.LoadParamZeroExtend8x64, LoadParamZeroExtend8x64);
			AddVisitation(IRInstruction.LoadParamZeroExtend16x64, LoadParamZeroExtend16x64);
			AddVisitation(IRInstruction.LoadParamZeroExtend32x64, LoadParamZeroExtend32x64);
			AddVisitation(IRInstruction.And64, And64);
			AddVisitation(IRInstruction.Or64, Or64);
			AddVisitation(IRInstruction.Xor64, Xor64);
			AddVisitation(IRInstruction.Not64, Not64);
			AddVisitation(IRInstruction.SignExtend16x64, SignExtend16x64);
			AddVisitation(IRInstruction.SignExtend32x64, SignExtend32x64);
			AddVisitation(IRInstruction.SignExtend8x64, SignExtend8x64);
			AddVisitation(IRInstruction.Store64, Store64);
			AddVisitation(IRInstruction.StoreParam64, StoreParam64);
			AddVisitation(IRInstruction.Sub64, Sub64);
			AddVisitation(IRInstruction.Truncate64x32, Truncate64x32);
			AddVisitation(IRInstruction.ZeroExtend8x64, ZeroExtend8x64);
			AddVisitation(IRInstruction.ZeroExtend16x64, ZeroExtend16x64);
			AddVisitation(IRInstruction.ZeroExtend32x64, ZeroExtend32x64);

			//AddVisitation(IRInstruction.RemSigned64, RemSigned64);
			//AddVisitation(IRInstruction.RemUnsigned64, RemUnsigned64);
			//AddVisitation(IRInstruction.ArithShiftRight64, ArithShiftRight64);
			//AddVisitation(IRInstruction.Compare64x64, Compare64x64);
			//AddVisitation(IRInstruction.IfThenElse64, IfThenElse64);
			//AddVisitation(IRInstruction.MulSigned64, MulSigned64);
			//AddVisitation(IRInstruction.MulUnsigned64, MulUnsigned64);
			//AddVisitation(IRInstruction.ShiftLeft64, ShiftLeft64);
			//AddVisitation(IRInstruction.ShiftRight64, ShiftRight64);
		}

		protected override void Setup()
		{
			Constant4 = CreateConstant(4);
		}

		#region Helpers

		private void SetGetLow64(Context context, Operand operand1, Operand operand2)
		{
			if (operand2.IsResolvedConstant)
			{
				context.SetInstruction(IRInstruction.Move32, operand1, CreateConstant(operand2.ConstantUnsigned32));
			}
			else
			{
				context.SetInstruction(IRInstruction.GetLow64, operand1, operand2);
			}
		}

		private void SetGetHigh64(Context context, Operand operand1, Operand operand2)
		{
			if (operand2.IsResolvedConstant)
			{
				context.SetInstruction(IRInstruction.Move32, operand1, CreateConstant(operand2.ConstantUnsigned64 >> 32));
			}
			else
			{
				context.SetInstruction(IRInstruction.GetHigh64, operand1, operand2);
			}
		}

		private void AppendGetLow64(Context context, Operand operand1, Operand operand2)
		{
			if (operand2.IsResolvedConstant)
			{
				context.AppendInstruction(IRInstruction.Move32, operand1, CreateConstant(operand2.ConstantUnsigned32));
			}
			else
			{
				context.AppendInstruction(IRInstruction.GetLow64, operand1, operand2);
			}
		}

		private void AppendGetHigh64(Context context, Operand operand1, Operand operand2)
		{
			if (operand2.IsResolvedConstant)
			{
				context.AppendInstruction(IRInstruction.Move32, operand1, CreateConstant(operand2.ConstantUnsigned64 >> 32));
			}
			else
			{
				context.AppendInstruction(IRInstruction.GetHigh64, operand1, operand2);
			}
		}

		#endregion Helpers

		#region Visitation Methods

		private void StoreParam64(Context context)
		{
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			var op0Low = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var op0High = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			MethodCompiler.SplitLongOperand(operand1, out Operand op1Low, out Operand op1High);
			MethodCompiler.SplitLongOperand(operand2, out Operand _, out Operand _);

			SetGetLow64(context, op0Low, operand2);
			AppendGetHigh64(context, op0High, operand2);
			context.AppendInstruction(IRInstruction.StoreParam32, null, op1Low, op0Low);
			context.AppendInstruction(IRInstruction.StoreParam32, null, op1High, op0High);
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

			SetGetLow64(context, op0Low, operand1);
			AppendGetHigh64(context, op0High, operand1);
			AppendGetLow64(context, op1Low, operand2);
			AppendGetHigh64(context, op1High, operand2);

			context.AppendInstruction2(IRInstruction.AddCarryOut32, resultLow, resultCarry, op0Low, op1Low);
			context.AppendInstruction(IRInstruction.AddCarryIn32, resultHigh, op0High, op1High, resultCarry);
			context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
		}

		private void Add64v2(Context context)
		{
			Debug.Assert(context.Result.Is64BitInteger);

			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			var resultLow = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var resultHigh = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var resultCarry = AllocateVirtualRegister(TypeSystem.BuiltIn.Boolean);

			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var v2 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var v3 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var v4 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			MethodCompiler.SplitLongOperand(operand1, out Operand op1Low, out Operand op1High);
			MethodCompiler.SplitLongOperand(operand2, out Operand op2Low, out Operand op2High);

			context.SetInstruction(IRInstruction.GetLow64, v1, op1Low);
			context.AppendInstruction(IRInstruction.GetLow64, v2, op2Low);
			context.AppendInstruction2(IRInstruction.AddCarryOut32, resultLow, resultCarry, v1, v2);
			context.AppendInstruction(IRInstruction.GetHigh64, v3, op1High);
			context.AppendInstruction(IRInstruction.GetHigh64, v4, op2High);
			context.AppendInstruction(IRInstruction.AddCarryIn32, resultHigh, v3, v4, resultCarry);
			context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
		}

		private void CompareBranch64(Context context)
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

			SetGetLow64(context, op0Low, operand1);
			AppendGetHigh64(context, op0High, operand1);
			AppendGetLow64(context, op1Low, operand2);
			AppendGetHigh64(context, op1High, operand2);

			// Compare high (equal)
			context.AppendInstruction(IRInstruction.CompareBranch32, ConditionCode.Equal, null, op0High, op1High, newBlocks[1].Block);
			context.AppendInstruction(IRInstruction.Jmp, newBlocks[0].Block);

			// Compare high
			newBlocks[0].AppendInstruction(IRInstruction.CompareBranch32, branch, null, op0High, op1High, newBlocks[2].Block);
			newBlocks[0].AppendInstruction(IRInstruction.Jmp, nextBlock.Block);

			// Compare low
			newBlocks[1].AppendInstruction(IRInstruction.CompareBranch32, branchUnsigned, null, op0Low, op1Low, newBlocks[2].Block);
			newBlocks[1].AppendInstruction(IRInstruction.Jmp, nextBlock.Block);

			// Target
			newBlocks[2].AppendInstruction(IRInstruction.Jmp, target);
		}

		private void Compare64x32(Context context)
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

				SetGetLow64(context, op0Low, operand1);
				AppendGetHigh64(context, op0High, operand1);
				AppendGetLow64(context, op1Low, operand2);
				AppendGetHigh64(context, op1High, operand2);

				context.AppendInstruction(IRInstruction.Xor32, v1, op0Low, op1Low);
				context.AppendInstruction(IRInstruction.Xor32, v2, op0High, op1High);
				context.AppendInstruction(IRInstruction.Or32, v3, v1, v2);
				context.AppendInstruction(IRInstruction.Compare32x32, condition, result, v3, ConstantZero);

				return;
			}

			if (SSA)
			{
				Compareeger64x32SSA(context);
			}
			else
			{
				Compareeger64x32NonSSA(context);
			}
		}

		private void Compareeger64x32SSA(Context context)
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

			//var resultLow = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			SetGetLow64(context, op0Low, operand1);
			AppendGetHigh64(context, op0High, operand1);
			AppendGetLow64(context, op1Low, operand2);
			AppendGetHigh64(context, op1High, operand2);

			// Compare high
			context.AppendInstruction(IRInstruction.CompareBranch32, ConditionCode.Equal, null, op0High, op1High, newBlocks[1].Block);
			context.AppendInstruction(IRInstruction.Jmp, newBlocks[0].Block);

			// Branch if check already gave results
			if (branch == ConditionCode.Equal)
			{
				newBlocks[0].AppendInstruction(IRInstruction.Jmp, newBlocks[3].Block);
			}
			else
			{
				newBlocks[0].AppendInstruction(IRInstruction.CompareBranch32, branch, null, op0High, op1High, newBlocks[2].Block);
				newBlocks[0].AppendInstruction(IRInstruction.Jmp, newBlocks[3].Block);
			}

			// Compare low
			newBlocks[1].AppendInstruction(IRInstruction.CompareBranch32, branchUnsigned, null, op0Low, op1Low, newBlocks[2].Block);
			newBlocks[1].AppendInstruction(IRInstruction.Jmp, newBlocks[3].Block);

			// Success
			//newBlocks[2].AppendInstruction(IRInstruction.MoveInt32, resultLow, CreateConstant((uint)1));
			newBlocks[2].AppendInstruction(IRInstruction.Jmp, newBlocks[4].Block);

			// Failed
			//newBlocks[3].AppendInstruction(IRInstruction.MoveInt32, resultLow, CreateConstant((uint)0));
			newBlocks[3].AppendInstruction(IRInstruction.Jmp, newBlocks[4].Block);

			// Exit
			//newBlocks[4].AppendInstruction(IRInstruction.MoveInt32, result, resultLow);
			newBlocks[4].AppendInstruction(IRInstruction.Phi32, result, CreateConstant((uint)1), CreateConstant((uint)0));
			newBlocks[4].PhiBlocks = new List<BasicBlock>(2) { newBlocks[2].Block, newBlocks[3].Block };
			newBlocks[4].AppendInstruction(IRInstruction.Jmp, nextBlock.Block);
		}

		private void Compareeger64x32NonSSA(Context context)
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

			SetGetLow64(context, op0Low, operand1);
			AppendGetHigh64(context, op0High, operand1);
			AppendGetLow64(context, op1Low, operand2);
			AppendGetHigh64(context, op1High, operand2);

			// Compare high
			context.AppendInstruction(IRInstruction.CompareBranch32, ConditionCode.Equal, null, op0High, op1High, newBlocks[1].Block);
			context.AppendInstruction(IRInstruction.Jmp, newBlocks[0].Block);

			// Branch if check already gave results
			if (branch == ConditionCode.Equal)
			{
				newBlocks[0].AppendInstruction(IRInstruction.Jmp, newBlocks[3].Block);
			}
			else
			{
				newBlocks[0].AppendInstruction(IRInstruction.CompareBranch32, branch, null, op0High, op1High, newBlocks[2].Block);
				newBlocks[0].AppendInstruction(IRInstruction.Jmp, newBlocks[3].Block);
			}

			// Compare low
			newBlocks[1].AppendInstruction(IRInstruction.CompareBranch32, branchUnsigned, null, op0Low, op1Low, newBlocks[2].Block);
			newBlocks[1].AppendInstruction(IRInstruction.Jmp, newBlocks[3].Block);

			// Success
			newBlocks[2].AppendInstruction(IRInstruction.Move32, resultLow, CreateConstant((uint)1));
			newBlocks[2].AppendInstruction(IRInstruction.Jmp, newBlocks[4].Block);

			// Failed
			newBlocks[3].AppendInstruction(IRInstruction.Move32, resultLow, CreateConstant((uint)0));
			newBlocks[3].AppendInstruction(IRInstruction.Jmp, newBlocks[4].Block);

			// Exit
			newBlocks[4].AppendInstruction(IRInstruction.Move32, result, resultLow);
			newBlocks[4].AppendInstruction(IRInstruction.Jmp, nextBlock.Block);
		}

		private void Load64(Context context)
		{
			Debug.Assert(context.Result.Is64BitInteger);

			var result = context.Result;
			var address = context.Operand1;
			var offset = context.Operand2;

			var resultLow = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var resultHigh = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var offsetLow = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var addressLow = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var offset4 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);

			SetGetLow64(context, addressLow, address);
			AppendGetLow64(context, offsetLow, offset);
			context.AppendInstruction(IRInstruction.Load32, resultLow, addressLow, offset);
			context.AppendInstruction(IRInstruction.Add32, offset4, offsetLow, Constant4);
			context.AppendInstruction(IRInstruction.Load32, resultHigh, addressLow, offset4);
			context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
		}

		private void LoadParam64(Context context)
		{
			Debug.Assert(!context.Result.IsR4);
			Debug.Assert(!context.Result.IsR8);
			Debug.Assert(context.Result.Is64BitInteger);

			var result = context.Result;
			var operand1 = context.Operand1;

			var resultLow = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var resultHigh = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			MethodCompiler.SplitLongOperand(operand1, out Operand op1Low, out Operand op1High);

			context.SetInstruction(IRInstruction.LoadParam32, resultLow, op1Low);
			context.AppendInstruction(IRInstruction.LoadParam32, resultHigh, op1High);
			context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
		}

		private void LoadParamSignExtend16x64(Context context)
		{
			Debug.Assert(!context.Result.IsR4);
			Debug.Assert(!context.Result.IsR8);
			Debug.Assert(context.Result.Is64BitInteger);

			var result = context.Result;
			var operand1 = context.Operand1;

			MethodCompiler.SplitLongOperand(operand1, out Operand op0Low, out _);

			var resultLow = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var resultHigh = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			context.SetInstruction(IRInstruction.LoadParamSignExtend16x32, resultLow, op0Low);
			context.AppendInstruction(IRInstruction.ArithShiftRight32, resultHigh, resultLow, CreateConstant((byte)31));
			context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
		}

		private void LoadParamSignExtend32x64(Context context)
		{
			Debug.Assert(!context.Result.IsR4);
			Debug.Assert(!context.Result.IsR8);
			Debug.Assert(context.Result.Is64BitInteger);

			var result = context.Result;
			var operand1 = context.Operand1;

			MethodCompiler.SplitLongOperand(operand1, out Operand op0Low, out Operand _);

			var resultLow = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var resultHigh = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			context.SetInstruction(IRInstruction.LoadParam32, resultLow, op0Low);
			context.AppendInstruction(IRInstruction.ArithShiftRight32, resultHigh, resultLow, CreateConstant((byte)31));
			context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
		}

		private void LoadParamSignExtend8x64(Context context)
		{
			Debug.Assert(!context.Result.IsR4);
			Debug.Assert(!context.Result.IsR8);
			Debug.Assert(context.Result.Is64BitInteger);

			var result = context.Result;
			var operand1 = context.Operand1;

			MethodCompiler.SplitLongOperand(operand1, out Operand op0Low, out Operand _);

			var resultLow = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var resultHigh = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			context.SetInstruction(IRInstruction.LoadParamSignExtend8x32, resultLow, op0Low);
			context.AppendInstruction(IRInstruction.ArithShiftRight32, resultHigh, resultLow, CreateConstant((byte)31));
			context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
		}

		private void LoadParamZeroExtend16x64(Context context)
		{
			Debug.Assert(!context.Result.IsR4);
			Debug.Assert(!context.Result.IsR8);
			Debug.Assert(context.Result.Is64BitInteger);

			var result = context.Result;
			var operand1 = context.Operand1;

			MethodCompiler.SplitLongOperand(operand1, out Operand op0Low, out Operand _);

			var resultLow = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

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

			MethodCompiler.SplitLongOperand(operand1, out Operand op0Low, out Operand _);

			var resultLow = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			context.SetInstruction(IRInstruction.LoadParam32, resultLow, op0Low);
			context.AppendInstruction(IRInstruction.To64, result, resultLow, ConstantZero);
		}

		private void LoadParamZeroExtend8x64(Context context)
		{
			Debug.Assert(!context.Result.IsR4);
			Debug.Assert(!context.Result.IsR8);
			Debug.Assert(context.Result.Is64BitInteger);

			var result = context.Result;
			var operand1 = context.Operand1;

			MethodCompiler.SplitLongOperand(operand1, out Operand op0Low, out Operand _);

			var resultLow = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			context.SetInstruction(IRInstruction.LoadParamZeroExtend8x32, resultLow, op0Low);
			context.AppendInstruction(IRInstruction.To64, result, resultLow, ConstantZero);
		}

		private void And64(Context context)
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

			SetGetLow64(context, op0Low, operand1);
			AppendGetHigh64(context, op0High, operand1);
			AppendGetLow64(context, op1Low, operand2);
			AppendGetHigh64(context, op1High, operand2);
			context.AppendInstruction(IRInstruction.And32, resultLow, op0Low, op1Low);
			context.AppendInstruction(IRInstruction.And32, resultHigh, op0High, op1High);
			context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
		}

		private void Not64(Context context)
		{
			Debug.Assert(context.Result.Is64BitInteger);

			var result = context.Result;
			var operand1 = context.Operand1;

			var op0Low = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var op0High = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var resultLow = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var resultHigh = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			SetGetLow64(context, op0Low, operand1);
			AppendGetHigh64(context, op0High, operand1);
			context.AppendInstruction(IRInstruction.Not32, resultLow, op0Low);
			context.AppendInstruction(IRInstruction.Not32, resultHigh, op0High);
			context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
		}

		private void Or64(Context context)
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

			SetGetLow64(context, op0Low, operand1);
			AppendGetHigh64(context, op0High, operand1);
			AppendGetLow64(context, op1Low, operand2);
			AppendGetHigh64(context, op1High, operand2);
			context.AppendInstruction(IRInstruction.Or32, resultLow, op0Low, op1Low);
			context.AppendInstruction(IRInstruction.Or32, resultHigh, op0High, op1High);
			context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
		}

		private void Xor64(Context context)
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

			SetGetLow64(context, op0Low, operand1);
			AppendGetHigh64(context, op0High, operand1);
			AppendGetLow64(context, op1Low, operand2);
			AppendGetHigh64(context, op1High, operand2);
			context.AppendInstruction(IRInstruction.Xor32, resultLow, op0Low, op1Low);
			context.AppendInstruction(IRInstruction.Xor32, resultHigh, op0High, op1High);
			context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
		}

		private void SignExtend16x64(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;

			var resultLow = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var resultHigh = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			context.SetInstruction(IRInstruction.Move32, v1, operand1);
			context.AppendInstruction(IRInstruction.SignExtend16x32, resultLow, v1);
			context.AppendInstruction(IRInstruction.ArithShiftRight32, resultHigh, resultLow, CreateConstant((byte)31));
			context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
		}

		private void SignExtend32x64(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;

			var resultLow = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var resultHigh = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			context.SetInstruction(IRInstruction.Move32, resultLow, operand1);
			context.AppendInstruction(IRInstruction.ArithShiftRight32, resultHigh, resultLow, CreateConstant((byte)31));
			context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
		}

		private void SignExtend8x64(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;

			var resultLow = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var resultHigh = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			context.SetInstruction(IRInstruction.Move32, v1, operand1);
			context.AppendInstruction(IRInstruction.SignExtend8x32, resultLow, v1);
			context.AppendInstruction(IRInstruction.ArithShiftRight32, resultHigh, resultLow, CreateConstant((byte)31));
			context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
		}

		private void Store64(Context context)
		{
			var address = context.Operand1;
			var offset = context.Operand2;
			var value = context.Operand3;

			var valueLow = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var valueHigh = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var offsetLow = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var addressLow = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var offset4 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);

			SetGetLow64(context, valueLow, value);
			AppendGetHigh64(context, valueHigh, value);
			AppendGetLow64(context, addressLow, address);
			AppendGetLow64(context, offsetLow, offset);
			context.AppendInstruction(IRInstruction.Store32, null, addressLow, offset, valueLow);
			context.AppendInstruction(IRInstruction.Add32, offset4, offsetLow, Constant4);
			context.AppendInstruction(IRInstruction.Store32, null, addressLow, offset4, valueHigh);
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

			SetGetLow64(context, op0Low, operand1);
			AppendGetHigh64(context, op0High, operand1);
			AppendGetLow64(context, op1Low, operand2);
			AppendGetHigh64(context, op1High, operand2);

			context.AppendInstruction2(IRInstruction.SubCarryOut32, resultLow, resultCarry, op0Low, op1Low);
			context.AppendInstruction(IRInstruction.SubCarryIn32, resultHigh, op0High, op1High, resultCarry);
			context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
		}

		private void Truncate64x32(Context context)
		{
			Debug.Assert(context.Operand1.Is64BitInteger);
			Debug.Assert(!context.Result.Is64BitInteger);

			SetGetLow64(context, context.Result, context.Operand1);
		}

		private void ZeroExtend16x64(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;

			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			context.SetInstruction(IRInstruction.ZeroExtend16x32, v1, operand1);
			context.AppendInstruction(IRInstruction.To64, result, operand1, ConstantZero);
		}

		private void ZeroExtend32x64(Context context)
		{
			context.SetInstruction(IRInstruction.To64, context.Result, context.Operand1, ConstantZero);
		}

		private void ZeroExtend8x64(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;

			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			context.SetInstruction(IRInstruction.ZeroExtend8x32, v1, operand1);
			context.AppendInstruction(IRInstruction.To64, result, operand1, ConstantZero);
		}

		#endregion Visitation Methods
	}
}
