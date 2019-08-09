// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;
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
		private Operand Constant4;

		protected override void PopulateVisitationDictionary()
		{
			AddVisitation(IRInstruction.Add64, Add64);

			//AddVisitation(IRInstruction.BitCopyFloatR8ToInt64, BitCopyFloatR8ToInt64);
			//AddVisitation(IRInstruction.BitCopyInt64ToFloatR8, BitCopyInt64ToFloatR8);
			//AddVisitation(IRInstruction.ArithShiftRight64, ArithShiftRight64);
			//AddVisitation(IRInstruction.Call, Call);
			//AddVisitation(IRInstruction.CompareInt32x64, CompareInt32x64);
			//AddVisitation(IRInstruction.CompareInt64x32, CompareInt64x32);
			//AddVisitation(IRInstruction.CompareInt64x64, CompareInt64x64);
			//AddVisitation(IRInstruction.CompareIntBranch64, CompareIntBranch64);
			//AddVisitation(IRInstruction.ConvertFloatR4ToInt64, ConvertFloatR4ToInt64);
			//AddVisitation(IRInstruction.ConvertFloatR8ToInt64, ConvertFloatR8ToInteger64);
			//AddVisitation(IRInstruction.ConvertInt64ToFloatR4, ConvertInt64ToFloatR4);
			//AddVisitation(IRInstruction.ConvertInt64ToFloatR8, ConvertInt64ToFloatR8);
			//AddVisitation(IRInstruction.IfThenElse64, IfThenElse64);
			//AddVisitation(IRInstruction.LoadInt64, LoadInt64);
			//AddVisitation(IRInstruction.LoadParamInt64, LoadParamInt64);
			//AddVisitation(IRInstruction.LoadParamSignExtend16x64, LoadParamSignExtend16x64);
			//AddVisitation(IRInstruction.LoadParamSignExtend32x64, LoadParamSignExtend32x64);
			//AddVisitation(IRInstruction.LoadParamSignExtend8x64, LoadParamSignExtend8x64);
			//AddVisitation(IRInstruction.LoadParamZeroExtend16x64, LoadParamZeroExtended16x64);
			//AddVisitation(IRInstruction.LoadParamZeroExtend32x64, LoadParamZeroExtended32x64);
			//AddVisitation(IRInstruction.LoadParamZeroExtend8x64, LoadParamZeroExtended8x64);
			//AddVisitation(IRInstruction.LogicalAnd64, LogicalAnd64);
			//AddVisitation(IRInstruction.LogicalNot64, LogicalNot64);
			//AddVisitation(IRInstruction.LogicalOr64, LogicalOr64);
			//AddVisitation(IRInstruction.LogicalXor64, LogicalXor64);
			//AddVisitation(IRInstruction.MoveInt64, MoveInteger64);
			//AddVisitation(IRInstruction.MulSigned64, MulSigned64);
			//AddVisitation(IRInstruction.MulUnsigned64, MulUnsigned64);
			//AddVisitation(IRInstruction.ShiftLeft64, ShiftLeft64);
			//AddVisitation(IRInstruction.ShiftRight64, ShiftRight64);
			//AddVisitation(IRInstruction.SignExtend16x64, SignExtend16x64);
			//AddVisitation(IRInstruction.SignExtend32x64, SignExtend32x64);
			//AddVisitation(IRInstruction.SignExtend8x64, SignExtend8x64);
			//AddVisitation(IRInstruction.GetHigh64, GetHigh64);
			//AddVisitation(IRInstruction.GetLow64, GetLow64);
			//AddVisitation(IRInstruction.StoreInt64, StoreInt64);
			//AddVisitation(IRInstruction.StoreParamInt64, StoreParamInt64);
			//AddVisitation(IRInstruction.Sub64, Sub64);
			//AddVisitation(IRInstruction.To64, To64);
			//AddVisitation(IRInstruction.Truncation64x32, Truncation64x32);
			//AddVisitation(IRInstruction.ZeroExtend16x64, ZeroExtended16x64);
			//AddVisitation(IRInstruction.ZeroExtend32x64, ZeroExtended32x64);
			//AddVisitation(IRInstruction.ZeroExtend8x64, ZeroExtended8x64);
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

			// TODO

			context.SetInstruction(ARMv8A32.Add, StatusRegister.Update, resultLow, op1L, op2L);
			context.AppendInstruction(ARMv8A32.Adc, resultHigh, op1H, op2H);
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
