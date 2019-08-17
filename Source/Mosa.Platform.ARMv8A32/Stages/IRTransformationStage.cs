// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;
using System.Diagnostics;

namespace Mosa.Platform.ARMv8A32.Stages
{
	/// <summary>
	/// Transforms IR instructions into their appropriate ARMv8A32.
	/// </summary>
	/// <remarks>
	/// This transformation stage transforms IR instructions into their equivalent ARMv8A32 sequences.
	/// </remarks>
	public sealed class IRTransformationStage : BaseTransformationStage
	{
		protected override void PopulateVisitationDictionary()
		{
			//AddVisitation(IRInstruction.AddFloatR4, AddFloatR4);
			//AddVisitation(IRInstruction.AddFloatR8, AddFloatR8);
			//AddVisitation(IRInstruction.AddressOf, AddressOf);
			AddVisitation(IRInstruction.Add32, Add32);
			AddVisitation(IRInstruction.AddCarryOut32, AddCarryOut32);
			AddVisitation(IRInstruction.AddWithCarry32, AddWithCarry32);
			AddVisitation(IRInstruction.ArithShiftRight32, ArithShiftRight32);

			//AddVisitation(IRInstruction.BitCopyFloatR4ToInt32, BitCopyFloatR4ToInt32);
			//AddVisitation(IRInstruction.BitCopyInt32ToFloatR4, BitCopyInt32ToFloatR4);
			//AddVisitation(IRInstruction.Break, Break);
			//AddVisitation(IRInstruction.CallDirect, CallDirect);
			//AddVisitation(IRInstruction.CompareFloatR4, CompareFloatR4);
			//AddVisitation(IRInstruction.CompareFloatR8, CompareFloatR8);
			//AddVisitation(IRInstruction.CompareInt32x32, CompareInt32x32);
			//AddVisitation(IRInstruction.CompareIntBranch32, CompareIntBranch32);
			//AddVisitation(IRInstruction.IfThenElse32, IfThenElse32);
			//AddVisitation(IRInstruction.ConvertFloatR4ToFloatR8, ConvertFloatR4ToFloatR8);
			//AddVisitation(IRInstruction.ConvertFloatR8ToFloatR4, ConvertFloatR8ToFloatR4);
			//AddVisitation(IRInstruction.ConvertFloatR4ToInt32, ConvertFloatR4ToInt32);
			//AddVisitation(IRInstruction.ConvertFloatR8ToInt32, ConvertFloatR8ToInt32);
			//AddVisitation(IRInstruction.ConvertInt32ToFloatR4, ConvertInt32ToFloatR4);
			//AddVisitation(IRInstruction.ConvertInt32ToFloatR8, ConvertInt32ToFloatR8);
			//AddVisitation(IRInstruction.DivFloatR4, DivFloatR4);
			//AddVisitation(IRInstruction.DivFloatR8, DivFloatR8);
			//AddVisitation(IRInstruction.DivSigned32, DivSigned32);
			//AddVisitation(IRInstruction.DivUnsigned32, DivUnsigned32);
			AddVisitation(IRInstruction.Jmp, Jmp);

			//AddVisitation(IRInstruction.LoadFloatR4, LoadFloatR4);
			//AddVisitation(IRInstruction.LoadFloatR8, LoadFloatR8);
			AddVisitation(IRInstruction.LoadInt32, LoadInt32);
			AddVisitation(IRInstruction.LoadSignExtend8x32, LoadSignExtend8x32);
			AddVisitation(IRInstruction.LoadSignExtend16x32, LoadSignExtend16x32);
			AddVisitation(IRInstruction.LoadZeroExtend8x32, LoadZeroExtend8x32);
			AddVisitation(IRInstruction.LoadZeroExtend16x32, LoadZeroExtend16x32);

			//AddVisitation(IRInstruction.LoadParamFloatR4, LoadParamFloatR4);
			//AddVisitation(IRInstruction.LoadParamFloatR8, LoadParamFloatR8);
			AddVisitation(IRInstruction.LoadParamInt32, LoadParamInt32);
			AddVisitation(IRInstruction.LoadParamSignExtend8x32, LoadParamSignExtend8x32);
			AddVisitation(IRInstruction.LoadParamSignExtend16x32, LoadParamSignExtend16x32);
			AddVisitation(IRInstruction.LoadParamZeroExtend8x32, LoadParamZeroExtend8x32);
			AddVisitation(IRInstruction.LoadParamZeroExtend16x32, LoadParamZeroExtend16x32);

			AddVisitation(IRInstruction.LogicalAnd32, LogicalAnd32);
			AddVisitation(IRInstruction.LogicalNot32, LogicalNot32);
			AddVisitation(IRInstruction.LogicalOr32, LogicalOr32);
			AddVisitation(IRInstruction.LogicalXor32, LogicalXor32);

			//AddVisitation(IRInstruction.MoveFloatR4, MoveFloatR4);
			//AddVisitation(IRInstruction.MoveFloatR8, MoveFloatR8);
			AddVisitation(IRInstruction.MoveInt32, MoveInt32);
			AddVisitation(IRInstruction.SignExtend8x32, SignExtend8x32);
			AddVisitation(IRInstruction.SignExtend16x32, SignExtend16x32);
			AddVisitation(IRInstruction.ZeroExtend8x32, ZeroExtend8x32);
			AddVisitation(IRInstruction.ZeroExtend16x32, ZeroExtend16x32);

			//AddVisitation(IRInstruction.MulFloatR4, MulFloatR4);
			//AddVisitation(IRInstruction.MulFloatR8, MulFloatR8);
			AddVisitation(IRInstruction.MulSigned32, MulSigned32);
			AddVisitation(IRInstruction.MulUnsigned32, MulUnsigned32);

			//AddVisitation(IRInstruction.Nop, Nop);
			//AddVisitation(IRInstruction.RemSigned32, RemSigned32);
			//AddVisitation(IRInstruction.RemUnsigned32, RemUnsigned32);
			AddVisitation(IRInstruction.ShiftLeft32, ShiftLeft32);
			AddVisitation(IRInstruction.ShiftRight32, ShiftRight32);

			//AddVisitation(IRInstruction.StoreFloatR4, StoreFloatR4);
			//AddVisitation(IRInstruction.StoreFloatR8, StoreFloatR8);
			AddVisitation(IRInstruction.StoreInt8, StoreInt8);
			AddVisitation(IRInstruction.StoreInt16, StoreInt16);
			AddVisitation(IRInstruction.StoreInt32, StoreInt32);

			//AddVisitation(IRInstruction.StoreParamFloatR4, StoreParamFloatR4);
			//AddVisitation(IRInstruction.StoreParamFloatR8, StoreParamFloatR8);
			AddVisitation(IRInstruction.StoreParamInt8, StoreParamInt8);
			AddVisitation(IRInstruction.StoreParamInt16, StoreParamInt16);
			AddVisitation(IRInstruction.StoreParamInt32, StoreParamInt32);

			//AddVisitation(IRInstruction.SubFloatR4, SubFloatR4);
			//AddVisitation(IRInstruction.SubFloatR8, SubFloatR8);
			AddVisitation(IRInstruction.Sub32, Sub32);
			AddVisitation(IRInstruction.SubCarryOut32, SubCarryOut32);
			AddVisitation(IRInstruction.SubWithCarry32, SubWithCarry32);

			//AddVisitation(IRInstruction.Switch, Switch);
		}

		#region Visitation Methods

		private void Add32(Context context)
		{
			TransformInstruction(context, ARMv8A32.Add, ARMv8A32.AddImm, context.Result, StatusRegister.NotSet, context.Operand1, context.Operand2);
		}

		private void AddCarryOut32(Context context)
		{
			var result2 = context.Result2;

			TransformInstruction(context, ARMv8A32.Add, ARMv8A32.AddImm, context.Result, StatusRegister.Update, context.Operand1, context.Operand2);

			context.AppendInstruction(ARMv8A32.MovImm, ConditionCode.Carry, result2, CreateConstant(1));
			context.AppendInstruction(ARMv8A32.MovImm, ConditionCode.NoCarry, result2, CreateConstant(0));
		}

		private void AddWithCarry32(Context context)
		{
			var result = context.Result;
			var operand3 = context.Operand3;

			TransformInstruction(context, ARMv8A32.Add, ARMv8A32.AddImm, context.Result, StatusRegister.NotSet, context.Operand1, context.Operand2);

			// FIXME: Operand3 may need fixup
			if (operand3.IsVirtualRegister)
			{
				context.AppendInstruction(ARMv8A32.Add, result, result, operand3);
			}
			else if (operand3.IsResolvedConstant)
			{
				context.AppendInstruction(ARMv8A32.AddImm, result, result, operand3);
			}
			else
			{
				throw new CompilerException("Error at {context} in {Method}");
			}
		}

		private void ArithShiftRight32(Context context)
		{
			TransformInstruction(context, ARMv8A32.Asr, ARMv8A32.AsrImm, context.Result, StatusRegister.NotSet, context.Operand1, context.Operand2);
		}

		private void Jmp(Context context)
		{
			context.ReplaceInstruction(ARMv8A32.B);
			context.ConditionCode = ConditionCode.Always;
		}

		private void LoadInt32(Context context)
		{
			Debug.Assert(!context.Result.IsR4);
			Debug.Assert(!context.Result.IsR8);

			TransformLoadInstruction(context, ARMv8A32.LdrUp32, ARMv8A32.LdrUpImm32, ARMv8A32.LdrDownImm32, context.Result, context.Operand1, context.Operand2);
		}

		private void LoadParamInt32(Context context)
		{
			Debug.Assert(!context.Result.IsR4);
			Debug.Assert(!context.Result.IsR8);

			TransformLoadInstruction(context, ARMv8A32.LdrUp32, ARMv8A32.LdrUpImm32, ARMv8A32.LdrDownImm32, context.Result, StackFrame, context.Operand1);
		}

		private void LoadParamSignExtend16x32(Context context)
		{
			Debug.Assert(!context.Result.IsR4);
			Debug.Assert(!context.Result.IsR8);

			TransformLoadInstruction(context, ARMv8A32.LdrUpS16, ARMv8A32.LdrUpImmS16, ARMv8A32.LdrDownImmS16, context.Result, StackFrame, context.Operand1);
		}

		private void LoadParamSignExtend8x32(Context context)
		{
			Debug.Assert(!context.Result.IsR4);
			Debug.Assert(!context.Result.IsR8);

			TransformLoadInstruction(context, ARMv8A32.LdrUpS8, ARMv8A32.LdrUpImmS8, ARMv8A32.LdrDownImmS8, context.Result, StackFrame, context.Operand1);
		}

		private void LoadParamZeroExtend16x32(Context context)
		{
			Debug.Assert(!context.Result.IsR4);
			Debug.Assert(!context.Result.IsR8);

			TransformLoadInstruction(context, ARMv8A32.LdrUp16, ARMv8A32.LdrUpImm16, ARMv8A32.LdrDownImm16, context.Result, StackFrame, context.Operand1);
		}

		private void LoadParamZeroExtend8x32(Context context)
		{
			Debug.Assert(!context.Result.IsR4);
			Debug.Assert(!context.Result.IsR8);

			TransformLoadInstruction(context, ARMv8A32.LdrUp8, ARMv8A32.LdrUpImm8, ARMv8A32.LdrDownImm8, context.Result, StackFrame, context.Operand1);
		}

		private void LoadSignExtend16x32(Context context)
		{
			Debug.Assert(!context.Result.IsR4);
			Debug.Assert(!context.Result.IsR8);

			TransformLoadInstruction(context, ARMv8A32.LdrUpS16, ARMv8A32.LdrUpImmS16, ARMv8A32.LdrDownImmS16, context.Result, StackFrame, context.Operand1);
		}

		private void LoadSignExtend8x32(Context context)
		{
			Debug.Assert(!context.Result.IsR4);
			Debug.Assert(!context.Result.IsR8);

			TransformLoadInstruction(context, ARMv8A32.LdrUpS8, ARMv8A32.LdrUpImmS8, ARMv8A32.LdrDownImmS8, context.Result, StackFrame, context.Operand1);
		}

		private void LoadZeroExtend16x32(Context context)
		{
			Debug.Assert(!context.Result.IsR4);
			Debug.Assert(!context.Result.IsR8);

			TransformLoadInstruction(context, ARMv8A32.LdrUp16, ARMv8A32.LdrUpImm16, ARMv8A32.LdrDownImm16, context.Result, StackFrame, context.Operand1);
		}

		private void LoadZeroExtend8x32(Context context)
		{
			Debug.Assert(!context.Result.IsR4);
			Debug.Assert(!context.Result.IsR8);

			TransformLoadInstruction(context, ARMv8A32.LdrUp8, ARMv8A32.LdrUpImm8, ARMv8A32.LdrDownImm8, context.Result, StackFrame, context.Operand1);
		}

		private void LogicalAnd32(Context context)
		{
			TransformInstruction(context, ARMv8A32.And, ARMv8A32.AndImm, context.Result, StatusRegister.NotSet, context.Operand1, context.Operand2);
		}

		private void LogicalNot32(Context context)
		{
			TransformInstruction(context, ARMv8A32.Mvn, ARMv8A32.MvnImm, context.Result, StatusRegister.NotSet, context.Operand1);
		}

		private void LogicalOr32(Context context)
		{
			TransformInstruction(context, ARMv8A32.Orr, ARMv8A32.OrrImm, context.Result, StatusRegister.NotSet, context.Operand1, context.Operand2);
		}

		private void LogicalXor32(Context context)
		{
			TransformInstruction(context, ARMv8A32.Eor, ARMv8A32.EorImm, context.Result, StatusRegister.NotSet, context.Operand1, context.Operand2);
		}

		private void MoveInt32(Context context)
		{
			TransformInstruction(context, ARMv8A32.Mov, ARMv8A32.MovImm, context.Result, StatusRegister.NotSet, context.Operand1);
		}

		private void MulSigned32(Context context)
		{
			context.SetInstruction(ARMv8A32.Mul, context.Result, context.Operand1, context.Operand2);
		}

		private void MulUnsigned32(Context context)
		{
			context.SetInstruction(ARMv8A32.Mul, context.Result, context.Operand1, context.Operand2);
		}

		private void ShiftLeft32(Context context)
		{
			TransformInstruction(context, ARMv8A32.Lsl, ARMv8A32.LslImm, context.Result, StatusRegister.NotSet, context.Operand1, context.Operand2);
		}

		private void ShiftRight32(Context context)
		{
			TransformInstruction(context, ARMv8A32.Lsr, ARMv8A32.LsrImm, context.Result, StatusRegister.NotSet, context.Operand1, context.Operand2);
		}

		private void SignExtend16x32(Context context)
		{
			TransformExtend(context, ARMv8A32.Sxth, context.Result, context.Operand1);
		}

		private void SignExtend8x32(Context context)
		{
			TransformExtend(context, ARMv8A32.Sxtb, context.Result, context.Operand1);
		}

		private void StoreInt16(Context context)
		{
			TransformStoreInstruction(context, ARMv8A32.StrUp16, ARMv8A32.StrUpImm16, ARMv8A32.StrDownImm16, context.Operand1, context.Operand2, context.Operand3);
		}

		private void StoreInt32(Context context)
		{
			TransformStoreInstruction(context, ARMv8A32.StrUp32, ARMv8A32.StrUpImm32, ARMv8A32.StrDownImm32, context.Operand1, context.Operand2, context.Operand3);
		}

		private void StoreInt8(Context context)
		{
			TransformStoreInstruction(context, ARMv8A32.StrUp8, ARMv8A32.StrUpImm8, ARMv8A32.StrDownImm8, context.Operand1, context.Operand2, context.Operand3);
		}

		private void StoreParamInt16(Context context)
		{
			TransformStoreInstruction(context, ARMv8A32.StrUp16, ARMv8A32.StrUpImm16, ARMv8A32.StrDownImm16, StackFrame, context.Operand1, context.Operand2);
		}

		private void StoreParamInt32(Context context)
		{
			TransformStoreInstruction(context, ARMv8A32.StrUp32, ARMv8A32.StrUpImm32, ARMv8A32.StrDownImm32, StackFrame, context.Operand1, context.Operand2);
		}

		private void StoreParamInt8(Context context)
		{
			TransformStoreInstruction(context, ARMv8A32.StrUp8, ARMv8A32.StrUpImm8, ARMv8A32.StrDownImm8, StackFrame, context.Operand1, context.Operand2);
		}

		private void Sub32(Context context)
		{
			TransformInstruction(context, ARMv8A32.Sub, ARMv8A32.SubImm, context.Result, StatusRegister.NotSet, context.Operand1, context.Operand2);
		}

		private void SubCarryOut32(Context context)
		{
			var result2 = context.Result2;

			TransformInstruction(context, ARMv8A32.Sub, ARMv8A32.SubImm, context.Result, StatusRegister.Update, context.Operand1, context.Operand2);

			context.AppendInstruction(ARMv8A32.MovImm, ConditionCode.Carry, result2, CreateConstant(1));
			context.AppendInstruction(ARMv8A32.MovImm, ConditionCode.NoCarry, result2, CreateConstant(0));
		}

		private void SubWithCarry32(Context context)
		{
			var result = context.Result;
			var operand3 = context.Operand3;

			TransformInstruction(context, ARMv8A32.Sub, ARMv8A32.SubImm, context.Result, StatusRegister.NotSet, context.Operand1, context.Operand2);

			// FIXME: Operand3 may need fixup
			if (operand3.IsVirtualRegister)
			{
				context.AppendInstruction(ARMv8A32.Sub, result, result, operand3);
			}
			else if (operand3.IsResolvedConstant)
			{
				context.AppendInstruction(ARMv8A32.SubImm, result, result, operand3);
			}
			else
			{
				throw new CompilerException("Error at {context} in {Method}");
			}
		}

		private void ZeroExtend16x32(Context context)
		{
			TransformExtend(context, ARMv8A32.Uxth, context.Result, context.Operand1);
		}

		private void ZeroExtend8x32(Context context)
		{
			TransformExtend(context, ARMv8A32.Uxtb, context.Result, context.Operand1);
		}

		#endregion Visitation Methods
	}
}
