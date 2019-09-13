// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;
using System.Diagnostics;

namespace Mosa.Platform.x64.Stages
{
	/// <summary>
	/// 64bit IR Transformation Stage
	/// </summary>
	/// <remarks>
	/// This stage translates all 64-bit operations to appropriate 32-bit operations on
	/// architectures without appropriate 64-bit integral operations.
	/// </remarks>
	/// <seealso cref="Mosa.Platform.x64.BaseTransformationStage" />
	public sealed class LongOperandStage : BaseTransformationStage
	{
		private Operand Constant4;

		protected override void PopulateVisitationDictionary()
		{
			AddVisitation(IRInstruction.Add64, Add64);
			AddVisitation(IRInstruction.ArithShiftRight64, ArithShiftRight64);
			AddVisitation(IRInstruction.Compare32x64, Compare32x64);
			AddVisitation(IRInstruction.Compare64x32, Compare64x32);
			AddVisitation(IRInstruction.Compare64x64, Compare64x64);
			AddVisitation(IRInstruction.CompareIntBranch64, CompareIntBranch64);
			AddVisitation(IRInstruction.ConvertR4To64, ConvertFloatR4To64);
			AddVisitation(IRInstruction.ConvertR8To64, ConvertFloatR8ToInteger64);
			AddVisitation(IRInstruction.Convert64ToR4, Convert64ToFloatR4);
			AddVisitation(IRInstruction.Convert64ToR8, Convert64ToFloatR8);
			AddVisitation(IRInstruction.IfThenElse64, IfThenElse64);
			AddVisitation(IRInstruction.Load64, Load64);
			AddVisitation(IRInstruction.LoadSignExtend32x64, LoadSignExtend32x64);
			AddVisitation(IRInstruction.LoadParam64, LoadParam64);
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
			AddVisitation(IRInstruction.Move64, Move64);
			AddVisitation(IRInstruction.MulSigned64, MulSigned64);
			AddVisitation(IRInstruction.MulUnsigned64, MulUnsigned64);
			AddVisitation(IRInstruction.ShiftLeft64, ShiftLeft64);
			AddVisitation(IRInstruction.ShiftRight64, ShiftRight64);
			AddVisitation(IRInstruction.SignExtend16x64, SignExtend16x64);
			AddVisitation(IRInstruction.SignExtend32x64, SignExtend32x64);
			AddVisitation(IRInstruction.SignExtend8x64, SignExtend8x64);
			AddVisitation(IRInstruction.Store64, Store64);
			AddVisitation(IRInstruction.StoreParam64, StoreParam64);
			AddVisitation(IRInstruction.Sub64, Sub64);
			AddVisitation(IRInstruction.Truncation64x32, Truncation64x32);
			AddVisitation(IRInstruction.ZeroExtend16x64, ZeroExtended16x64);
			AddVisitation(IRInstruction.ZeroExtend32x64, ZeroExtended32x64);
			AddVisitation(IRInstruction.ZeroExtend8x64, ZeroExtended8x64);

			AddVisitation(IRInstruction.DivSigned64, DivSigned64);
			AddVisitation(IRInstruction.DivUnsigned64, DivUnsigned64);
			AddVisitation(IRInstruction.LoadSignExtend16x64, LoadSignExtend16x64);
			AddVisitation(IRInstruction.LoadSignExtend8x64, LoadSignExtend8x64);
			AddVisitation(IRInstruction.LoadZeroExtend16x64, LoadZeroExtend16x64);
			AddVisitation(IRInstruction.LoadZeroExtend32x64, LoadZeroExtend32x64);
			AddVisitation(IRInstruction.LoadZeroExtend8x64, LoadZeroExtend8x64);
			AddVisitation(IRInstruction.RemSigned64, RemSigned64);
			AddVisitation(IRInstruction.RemUnsigned64, RemUnsigned64);
		}

		protected override void Setup()
		{
			Constant4 = CreateConstant(4);
		}

		#region Visitation Methods

		private void Add64(Context context)
		{
			context.ReplaceInstruction(X64.Add64);
		}

		private void ArithShiftRight64(Context context)
		{
			context.ReplaceInstruction(X64.Sar64);
		}

		private void Compare32x64(Context context)
		{
			Compare64x64(context);
		}

		private void Compare64x32(Context context)
		{
			Compare64x64(context);
		}

		private void Compare64x64(Context context)
		{
			var condition = context.ConditionCode;
			var resultOperand = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			context.SetInstruction(X64.Cmp64, null, operand1, operand2);
			context.AppendInstruction(X64.Setcc, condition, v1);
			context.AppendInstruction(X64.Movzx8To64, resultOperand, v1);
		}

		private void CompareIntBranch64(Context context)
		{
			OptimizeBranch(context);

			var target = context.BranchTargets[0];
			var condition = context.ConditionCode;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			context.SetInstruction(X64.Cmp64, null, operand1, operand2);
			context.AppendInstruction(X64.Branch, condition, target);
		}

		private void ConvertFloatR4To64(Context context)
		{
			context.ReplaceInstruction(X64.Cvtss2sd);
		}

		private void ConvertFloatR8ToInteger64(Context context)
		{
			Debug.Assert(context.Result.IsI1 || context.Result.IsI2 || context.Result.IsI4);
			context.ReplaceInstruction(X64.Cvttss2si64);
		}

		private void Convert64ToFloatR4(Context context)
		{
			context.SetInstruction(X64.Cvtsi2ss64, context.Result, context.Operand1);
		}

		private void Convert64ToFloatR8(Context context)
		{
			context.SetInstruction(X64.Cvtsi2sd64, context.Result, context.Operand1);
		}

		private void DivSigned64(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var v2 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);
			var v3 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			context.SetInstruction2(X64.Cdq64, v1, v2, operand1);
			context.AppendInstruction2(X64.IDiv64, result, v3, v1, v2, operand2);
		}

		private void DivUnsigned64(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.U8);
			var v2 = AllocateVirtualRegister(TypeSystem.BuiltIn.U8);

			context.SetInstruction(X64.Mov64, v1, ConstantZero64);
			context.AppendInstruction2(X64.Div64, result, v2, v1, operand1, operand2);
		}

		private void IfThenElse64(Context context)
		{
			var result = context.Operand1;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			context.SetInstruction(X64.Cmp64, null, operand1, ConstantZero64);
			context.AppendInstruction(X64.CMov64, ConditionCode.NotEqual, result, operand1);    // true
			context.AppendInstruction(X64.CMov64, ConditionCode.Equal, result, operand2);       // false
		}

		private void Load64(Context context)
		{
			Debug.Assert(!context.Result.IsR4);
			Debug.Assert(!context.Result.IsR8);

			LoadStore.OrderLoadOperands(context, MethodCompiler);

			context.SetInstruction(X64.MovLoad64, context.Result, context.Operand1, context.Operand2);
		}

		private void LoadParam64(Context context)
		{
			context.SetInstruction(X64.MovLoad64, context.Result, StackFrame, context.Operand1);
		}

		private void LoadParamSignExtend16x64(Context context)
		{
			context.SetInstruction(X64.MovsxLoad16, context.Result, StackFrame, context.Operand1);
		}

		private void LoadParamSignExtend32x64(Context context)
		{
			context.SetInstruction(X64.MovsxLoad32, context.Result, StackFrame, context.Operand1);
		}

		private void LoadParamSignExtend8x64(Context context)
		{
			context.SetInstruction(X64.MovsxLoad8, context.Result, StackFrame, context.Operand1);
		}

		private void LoadParamZeroExtended16x64(Context context)
		{
			context.SetInstruction(X64.MovzxLoad16, context.Result, StackFrame, context.Operand1);
		}

		private void LoadParamZeroExtended32x64(Context context)
		{
			context.SetInstruction(X64.MovzxLoad32, context.Result, StackFrame, context.Operand1);
		}

		private void LoadParamZeroExtended8x64(Context context)
		{
			context.SetInstruction(X64.MovzxLoad8, context.Result, StackFrame, context.Operand1);
		}

		private void LoadSignExtend16x64(Context context)
		{
			context.ReplaceInstruction(X64.MovsxLoad16);
		}

		private void LoadSignExtend32x64(Context context)
		{
			LoadStore.OrderLoadOperands(context, MethodCompiler);

			context.SetInstruction(X64.MovzxLoad32, context.Result, context.Operand1, context.Operand2);
		}

		private void LoadSignExtend8x64(Context context)
		{
			context.ReplaceInstruction(X64.MovsxLoad8);
		}

		private void LoadZeroExtend16x64(Context context)
		{
			context.ReplaceInstruction(X64.MovzxLoad16);
		}

		private void LoadZeroExtend32x64(Context context)
		{
			context.ReplaceInstruction(X64.MovzxLoad32);
		}

		private void LoadZeroExtend8x64(Context context)
		{
			context.ReplaceInstruction(X64.MovzxLoad8);
		}

		private void LogicalAnd64(Context context)
		{
			context.ReplaceInstruction(X64.And64);
		}

		private void LogicalNot64(Context context)
		{
			context.SetInstruction(X64.Mov64, context.Result, context.Operand1);
		}

		private void LogicalOr64(Context context)
		{
			context.ReplaceInstruction(X64.Or64);
		}

		private void LogicalXor64(Context context)
		{
			context.ReplaceInstruction(X64.Xor64);
		}

		private void Move64(Context context)
		{
			context.ReplaceInstruction(X64.Mov64);
		}

		private void MulSigned64(Context context)
		{
			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);
			context.SetInstruction2(X64.Mul64, v1, context.Result, context.Operand1, context.Operand2);
		}

		private void MulUnsigned64(Context context)
		{
			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);
			context.SetInstruction2(X64.Mul64, v1, context.Result, context.Operand1, context.Operand2);
		}

		private void RemSigned64(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var v2 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);
			var v3 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			context.SetInstruction2(X64.Cdq64, v1, v2, operand1);
			context.AppendInstruction2(X64.IDiv64, result, v3, v1, v2, operand2);
		}

		private void RemUnsigned64(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);
			var v2 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);

			context.SetInstruction(X64.Mov64, v1, ConstantZero64);
			context.AppendInstruction2(X64.Div64, result, v2, v1, operand1, operand2);
		}

		private void ShiftLeft64(Context context)
		{
			context.ReplaceInstruction(X64.Shl64);
		}

		private void ShiftRight64(Context context)
		{
			context.ReplaceInstruction(X64.Shr64);
		}

		private void SignExtend16x64(Context context)
		{
			context.ReplaceInstruction(X64.Movsx16To64);
		}

		private void SignExtend32x64(Context context)
		{
			context.ReplaceInstruction(X64.Movsx32To64);
		}

		private void SignExtend8x64(Context context)
		{
			context.ReplaceInstruction(X64.Movsx8To64);
		}

		private void Store64(Context context)
		{
			LoadStore.OrderStoreOperands(context, MethodCompiler);

			context.SetInstruction(X64.MovStore64, null, context.Operand1, context.Operand2, context.Operand3);
		}

		private void StoreParam64(Context context)
		{
			context.SetInstruction(X64.MovStore64, null, StackFrame, context.Operand1, context.Operand2);
		}

		private void Sub64(Context context)
		{
			context.ReplaceInstruction(X64.Sub64);
		}

		private void Truncation64x32(Context context)
		{
			context.ReplaceInstruction(X64.Movzx32To64);
		}

		private void ZeroExtended16x64(Context context)
		{
			context.ReplaceInstruction(X64.Movzx16To64);
		}

		private void ZeroExtended32x64(Context context)
		{
			context.ReplaceInstruction(X64.Movzx32To64);
		}

		private void ZeroExtended8x64(Context context)
		{
			context.ReplaceInstruction(X64.Movzx8To64);
		}

		#endregion Visitation Methods

		#region Utility Methods

		public static void OptimizeBranch(Context context)
		{
			// Note: same method as in x64
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			if (operand2.IsConstant || operand1.IsVirtualRegister)
				return;

			// Move constant to the right
			context.Operand1 = operand2;
			context.Operand2 = operand1;
			context.ConditionCode = context.ConditionCode.GetReverse();
		}

		#endregion Utility Methods
	}
}
