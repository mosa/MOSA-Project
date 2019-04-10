// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;
using System;
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
			AddVisitation(IRInstruction.LoadSignExtend32x64, LoadSignExtend32x64);
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
			AddVisitation(IRInstruction.MoveInt64, MoveInt64);
			AddVisitation(IRInstruction.MulSigned64, MulSigned64);
			AddVisitation(IRInstruction.MulUnsigned64, MulUnsigned64);
			AddVisitation(IRInstruction.ShiftLeft64, ShiftLeft64);
			AddVisitation(IRInstruction.ShiftRight64, ShiftRight64);
			AddVisitation(IRInstruction.SignExtend16x64, SignExtend16x64);
			AddVisitation(IRInstruction.SignExtend32x64, SignExtend32x64);
			AddVisitation(IRInstruction.SignExtend8x64, SignExtend8x64);
			AddVisitation(IRInstruction.StoreInt64, StoreInt64);
			AddVisitation(IRInstruction.StoreParamInt64, StoreParamInt64);
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

		private void Add64(InstructionNode node)
		{
			node.ReplaceInstruction(X64.Add64);
		}

		private void ArithShiftRight64(InstructionNode node)
		{
			node.ReplaceInstruction(X64.Sar64);
		}

		private void CompareInt32x64(Context context)
		{
			CompareInt64x64(context);
		}

		private void CompareInt64x32(Context context)
		{
			CompareInt64x64(context);
		}

		private void CompareInt64x64(Context context)
		{
			var condition = context.ConditionCode;
			var resultOperand = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			var setcc = IRTransformationStage.GetSetcc(condition);

			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			context.SetInstruction(X64.Cmp64, null, operand1, operand2);
			context.AppendInstruction(setcc, v1);
			context.AppendInstruction(X64.Movzx8To64, resultOperand, v1);
		}

		private void CompareIntBranch64(Context context)
		{
			OptimizeBranch(context);

			var target = context.BranchTargets[0];
			var condition = context.ConditionCode;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			var branch = GetBranch(condition);

			context.SetInstruction(X64.Cmp64, null, operand1, operand2);
			context.AppendInstruction(branch, target);
		}

		private void ConvertFloatR4ToInt64(InstructionNode node)
		{
			node.ReplaceInstruction(X64.Cvtss2sd);
		}

		private void ConvertFloatR8ToInteger64(InstructionNode node)
		{
			Debug.Assert(node.Result.Type.IsI1 || node.Result.Type.IsI2 || node.Result.Type.IsI4);
			node.ReplaceInstruction(X64.Cvttss2si64);
		}

		private void ConvertInt64ToFloatR4(InstructionNode node)
		{
			node.SetInstruction(X64.Cvtsi2ss64, node.Result, node.Operand1);
		}

		private void ConvertInt64ToFloatR8(InstructionNode node)
		{
			node.SetInstruction(X64.Cvtsi2sd64, node.Result, node.Operand1);
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
			context.AppendInstruction(X64.CMovNotEqual64, result, operand1);    // true
			context.AppendInstruction(X64.CMovEqual64, result, operand2);       // false
		}

		private void LoadInt64(InstructionNode node)
		{
			Debug.Assert(!node.Result.IsR4);
			Debug.Assert(!node.Result.IsR8);

			LoadStore.OrderLoadOperands(node, MethodCompiler);

			node.SetInstruction(X64.MovLoad64, node.Result, node.Operand1, node.Operand2);
		}

		private void LoadParamInt64(InstructionNode node)
		{
			node.SetInstruction(X64.MovLoad64, node.Result, StackFrame, node.Operand1);
		}

		private void LoadParamSignExtend16x64(InstructionNode node)
		{
			node.SetInstruction(X64.MovsxLoad16, node.Result, StackFrame, node.Operand1);
		}

		private void LoadParamSignExtend32x64(InstructionNode node)
		{
			node.SetInstruction(X64.MovsxLoad32, node.Result, StackFrame, node.Operand1);
		}

		private void LoadParamSignExtend8x64(InstructionNode node)
		{
			node.SetInstruction(X64.MovsxLoad8, node.Result, StackFrame, node.Operand1);
		}

		private void LoadParamZeroExtended16x64(InstructionNode node)
		{
			node.SetInstruction(X64.MovzxLoad16, node.Result, StackFrame, node.Operand1);
		}

		private void LoadParamZeroExtended32x64(InstructionNode node)
		{
			node.SetInstruction(X64.MovzxLoad32, node.Result, StackFrame, node.Operand1);
		}

		private void LoadParamZeroExtended8x64(InstructionNode node)
		{
			node.SetInstruction(X64.MovzxLoad8, node.Result, StackFrame, node.Operand1);
		}

		private void LoadSignExtend16x64(InstructionNode node)
		{
			node.ReplaceInstruction(X64.MovsxLoad16);
		}

		private void LoadSignExtend32x64(InstructionNode node)
		{
			LoadStore.OrderLoadOperands(node, MethodCompiler);

			node.SetInstruction(X64.MovzxLoad32, node.Result, node.Operand1, node.Operand2);
		}

		private void LoadSignExtend8x64(InstructionNode node)
		{
			node.ReplaceInstruction(X64.MovsxLoad8);
		}

		private void LoadZeroExtend16x64(InstructionNode node)
		{
			node.ReplaceInstruction(X64.MovzxLoad16);
		}

		private void LoadZeroExtend32x64(InstructionNode node)
		{
			node.ReplaceInstruction(X64.MovzxLoad32);
		}

		private void LoadZeroExtend8x64(InstructionNode node)
		{
			node.ReplaceInstruction(X64.MovzxLoad8);
		}

		private void LogicalAnd64(InstructionNode node)
		{
			node.ReplaceInstruction(X64.And64);
		}

		private void LogicalNot64(InstructionNode node)
		{
			node.SetInstruction(X64.Mov64, node.Result, node.Operand1);
		}

		private void LogicalOr64(InstructionNode node)
		{
			node.ReplaceInstruction(X64.Or64);
		}

		private void LogicalXor64(InstructionNode node)
		{
			node.ReplaceInstruction(X64.Xor64);
		}

		private void MoveInt64(InstructionNode node)
		{
			node.ReplaceInstruction(X64.Mov64);
		}

		private void MulSigned64(InstructionNode node)
		{
			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);
			node.SetInstruction2(X64.Mul64, v1, node.Result, node.Operand1, node.Operand2);
		}

		private void MulUnsigned64(InstructionNode node)
		{
			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);
			node.SetInstruction2(X64.Mul64, v1, node.Result, node.Operand1, node.Operand2);
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

		private void ShiftLeft64(InstructionNode node)
		{
			node.ReplaceInstruction(X64.Shl64);
		}

		private void ShiftRight64(InstructionNode node)
		{
			node.ReplaceInstruction(X64.Shr64);
		}

		private void SignExtend16x64(InstructionNode node)
		{
			node.ReplaceInstruction(X64.Movsx16To64);
		}

		private void SignExtend32x64(InstructionNode node)
		{
			node.ReplaceInstruction(X64.Movsx32To64);
		}

		private void SignExtend8x64(InstructionNode node)
		{
			node.ReplaceInstruction(X64.Movsx8To64);
		}

		private void StoreInt64(InstructionNode node)
		{
			LoadStore.OrderStoreOperands(node, MethodCompiler);

			node.SetInstruction(X64.MovStore64, null, node.Operand1, node.Operand2, node.Operand3);
		}

		private void StoreParamInt64(InstructionNode node)
		{
			node.SetInstruction(X64.MovStore64, null, StackFrame, node.Operand1, node.Operand2);
		}

		private void Sub64(InstructionNode node)
		{
			node.ReplaceInstruction(X64.Sub64);
		}

		private void Truncation64x32(InstructionNode node)
		{
			node.ReplaceInstruction(X64.Movzx32To64);
		}

		private void ZeroExtended16x64(InstructionNode node)
		{
			node.ReplaceInstruction(X64.Movzx16To64);
		}

		private void ZeroExtended32x64(InstructionNode node)
		{
			node.ReplaceInstruction(X64.Movzx32To64);
		}

		private void ZeroExtended8x64(InstructionNode node)
		{
			node.ReplaceInstruction(X64.Movzx8To64);
		}

		#endregion Visitation Methods

		#region Utility Methods

		public static BaseInstruction GetBranch(ConditionCode condition)
		{
			switch (condition)
			{
				case ConditionCode.Overflow: return X64.BranchOverflow;
				case ConditionCode.NoOverflow: return X64.BranchNoOverflow;
				case ConditionCode.Carry: return X64.BranchCarry;
				case ConditionCode.UnsignedLessThan: return X64.BranchUnsignedLessThan;
				case ConditionCode.UnsignedGreaterOrEqual: return X64.BranchUnsignedGreaterOrEqual;
				case ConditionCode.NoCarry: return X64.BranchNoCarry;
				case ConditionCode.Equal: return X64.BranchEqual;
				case ConditionCode.Zero: return X64.BranchZero;
				case ConditionCode.NotEqual: return X64.BranchNotEqual;
				case ConditionCode.NotZero: return X64.BranchNotZero;
				case ConditionCode.UnsignedLessOrEqual: return X64.BranchUnsignedLessOrEqual;
				case ConditionCode.UnsignedGreaterThan: return X64.BranchUnsignedGreaterThan;
				case ConditionCode.Signed: return X64.BranchSigned;
				case ConditionCode.NotSigned: return X64.BranchNotSigned;
				case ConditionCode.LessThan: return X64.BranchLessThan;
				case ConditionCode.GreaterOrEqual: return X64.BranchGreaterOrEqual;
				case ConditionCode.LessOrEqual: return X64.BranchLessOrEqual;
				case ConditionCode.GreaterThan: return X64.BranchGreaterThan;

				default: throw new NotSupportedException();
			}
		}

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
