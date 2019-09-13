// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;

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

			//AddVisitation(IRInstruction.BitCopyFloatR8To64, BitCopyFloatR8To64);
			//AddVisitation(IRInstruction.BitCopy64ToFloatR8, BitCopy64ToFloatR8);
			//AddVisitation(IRInstruction.ArithShiftRight64, ArithShiftRight64);
			//AddVisitation(IRInstruction.Call, Call);
			//AddVisitation(IRInstruction.CompareInt32x64, CompareInt32x64);
			//AddVisitation(IRInstruction.Compare64x32, Compare64x32);
			//AddVisitation(IRInstruction.Compare64x64, Compare64x64);
			//AddVisitation(IRInstruction.CompareIntBranch64, CompareIntBranch64);
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
			//AddVisitation(IRInstruction.LogicalAnd64, LogicalAnd64);
			//AddVisitation(IRInstruction.LogicalNot64, LogicalNot64);
			//AddVisitation(IRInstruction.LogicalOr64, LogicalOr64);
			//AddVisitation(IRInstruction.LogicalXor64, LogicalXor64);
			//AddVisitation(IRInstruction.Move64, MoveInteger64);
			//AddVisitation(IRInstruction.MulSigned64, MulSigned64);
			//AddVisitation(IRInstruction.MulUnsigned64, MulUnsigned64);
			//AddVisitation(IRInstruction.ShiftLeft64, ShiftLeft64);
			//AddVisitation(IRInstruction.ShiftRight64, ShiftRight64);
			//AddVisitation(IRInstruction.SignExtend16x64, SignExtend16x64);
			//AddVisitation(IRInstruction.SignExtend32x64, SignExtend32x64);
			//AddVisitation(IRInstruction.SignExtend8x64, SignExtend8x64);
			//AddVisitation(IRInstruction.GetHigh64, GetHigh64);
			//AddVisitation(IRInstruction.GetLow64, GetLow64);
			//AddVisitation(IRInstruction.Store64, Store64);
			//AddVisitation(IRInstruction.StoreParam64, StoreParam64);
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

			TransformInstruction(context, ARMv8A32.Add, ARMv8A32.AddImm, resultLow, StatusRegister.Update, op1L, op2L);
			TransformInstruction(context.InsertAfter(), ARMv8A32.Adc, ARMv8A32.AdcImm, resultLow, StatusRegister.NotSet, op1H, op2H);
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
