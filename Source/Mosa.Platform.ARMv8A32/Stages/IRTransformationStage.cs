// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework;
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
			AddVisitation(IRInstruction.AddR4, AddR4);
			AddVisitation(IRInstruction.AddR8, AddR8);

			//AddVisitation(IRInstruction.AddressOf, AddressOf);
			AddVisitation(IRInstruction.Add32, Add32);
			AddVisitation(IRInstruction.AddCarryOut32, AddCarryOut32);
			AddVisitation(IRInstruction.AddCarryIn32, AddCarryIn32);
			AddVisitation(IRInstruction.ArithShiftRight32, ArithShiftRight32);

			//AddVisitation(IRInstruction.CallDirect, CallDirect);
			AddVisitation(IRInstruction.CompareR4, CompareR4);
			AddVisitation(IRInstruction.CompareR8, CompareR8);
			AddVisitation(IRInstruction.Compare32x32, Compare32x32);
			AddVisitation(IRInstruction.CompareBranch32, CompareBranch32);
			AddVisitation(IRInstruction.IfThenElse32, IfThenElse32);
			AddVisitation(IRInstruction.ConvertR4To32, ConvertR4ToInt32);
			AddVisitation(IRInstruction.ConvertR8To32, ConvertR8ToInt32);
			AddVisitation(IRInstruction.Convert32ToR4, ConvertInt32ToR4);
			AddVisitation(IRInstruction.Convert32ToR8, ConvertInt32ToR8);
			AddVisitation(IRInstruction.DivR4, DivR4);
			AddVisitation(IRInstruction.DivR8, DivR8);
			AddVisitation(IRInstruction.Jmp, Jmp);
			AddVisitation(IRInstruction.LoadR4, LoadR4);
			AddVisitation(IRInstruction.LoadR8, LoadR8);
			AddVisitation(IRInstruction.Load32, LoadInt32);
			AddVisitation(IRInstruction.LoadSignExtend8x32, LoadSignExtend8x32);
			AddVisitation(IRInstruction.LoadSignExtend16x32, LoadSignExtend16x32);
			AddVisitation(IRInstruction.LoadZeroExtend8x32, LoadZeroExtend8x32);
			AddVisitation(IRInstruction.LoadZeroExtend16x32, LoadZeroExtend16x32);
			AddVisitation(IRInstruction.LoadParamR4, LoadParamR4);
			AddVisitation(IRInstruction.LoadParamR8, LoadParamR8);
			AddVisitation(IRInstruction.LoadParam32, LoadParamInt32);
			AddVisitation(IRInstruction.LoadParamSignExtend8x32, LoadParamSignExtend8x32);
			AddVisitation(IRInstruction.LoadParamSignExtend16x32, LoadParamSignExtend16x32);
			AddVisitation(IRInstruction.LoadParamZeroExtend8x32, LoadParamZeroExtend8x32);
			AddVisitation(IRInstruction.LoadParamZeroExtend16x32, LoadParamZeroExtend16x32);
			AddVisitation(IRInstruction.And32, And32);
			AddVisitation(IRInstruction.Not32, Not32);
			AddVisitation(IRInstruction.Or32, Or32);
			AddVisitation(IRInstruction.Xor32, Xor32);
			AddVisitation(IRInstruction.MoveR4, MoveR4);
			AddVisitation(IRInstruction.MoveR8, MoveR8);
			AddVisitation(IRInstruction.Move32, MoveInt32);
			AddVisitation(IRInstruction.SignExtend8x32, SignExtend8x32);
			AddVisitation(IRInstruction.SignExtend16x32, SignExtend16x32);
			AddVisitation(IRInstruction.ZeroExtend8x32, ZeroExtend8x32);
			AddVisitation(IRInstruction.ZeroExtend16x32, ZeroExtend16x32);
			AddVisitation(IRInstruction.MulR4, MulR4);
			AddVisitation(IRInstruction.MulR8, MulR8);
			AddVisitation(IRInstruction.MulSigned32, MulSigned32);
			AddVisitation(IRInstruction.MulUnsigned32, MulUnsigned32);

			//AddVisitation(IRInstruction.Nop, Nop);
			AddVisitation(IRInstruction.ShiftLeft32, ShiftLeft32);
			AddVisitation(IRInstruction.ShiftRight32, ShiftRight32);
			AddVisitation(IRInstruction.StoreR4, StoreR4);
			AddVisitation(IRInstruction.StoreR8, StoreR8);
			AddVisitation(IRInstruction.Store8, StoreInt8);
			AddVisitation(IRInstruction.Store16, StoreInt16);
			AddVisitation(IRInstruction.Store32, StoreInt32);
			AddVisitation(IRInstruction.StoreParamR4, StoreParamR4);
			AddVisitation(IRInstruction.StoreParamR8, StoreParamR8);
			AddVisitation(IRInstruction.StoreParam8, StoreParamInt8);
			AddVisitation(IRInstruction.StoreParam16, StoreParamInt16);
			AddVisitation(IRInstruction.StoreParam32, StoreParamInt32);
			AddVisitation(IRInstruction.SubR4, SubR4);
			AddVisitation(IRInstruction.SubR8, SubR8);
			AddVisitation(IRInstruction.Sub32, Sub32);
			AddVisitation(IRInstruction.SubCarryOut32, SubCarryOut32);
			AddVisitation(IRInstruction.SubCarryIn32, SubCarryIn32);

			//AddVisitation(IRInstruction.Switch, Switch);
			AddVisitation(IRInstruction.ZeroExtend16x32, ZeroExtend16x32);
			AddVisitation(IRInstruction.ZeroExtend8x32, ZeroExtend8x32);
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

		private void AddR4(Context context)
		{
			// TODO: (across all float instructions)
			// if operand1 is constant
			// if resolved & specific constant, then AdfImm
			// else if resolved & non-specific constant, then LoadConstant, adf
			// else if unresolved, throw not implemented

			context.ReplaceInstruction(ARMv8A32.Adf);
		}

		private void AddR8(Context context)
		{
			context.ReplaceInstruction(ARMv8A32.Adf);
		}

		private void AddCarryIn32(Context context)
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

		private void CompareR4(Context context)
		{
			FloatCompare(context, ARMv8A32.Cmf);
		}

		private void CompareR8(Context context)
		{
			FloatCompare(context, ARMv8A32.Cmf);
		}

		private void Compare32x32(Context context)
		{
			var condition = context.ConditionCode;
			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			// TODO: operand1 and operand2 must be registers
			// otherwise, place them into a register (or use CmpImm32, if possible)

			context.SetInstruction(ARMv8A32.Cmp, null, operand1, operand2);
			context.AppendInstruction(ARMv8A32.Mov, condition, result, CreateConstant(1));
			context.AppendInstruction(ARMv8A32.Mov, condition.GetOpposite(), result, CreateConstant(0));
		}

		private void CompareBranch32(Context context)
		{
			OptimizeBranch(context);

			var target = context.BranchTargets[0];
			var condition = context.ConditionCode;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			// TODO: operand1 and operand2 must be registers
			// otherwise, place them into a register (or use CmpImm32, if possible)

			context.SetInstruction(ARMv8A32.Cmp, null, operand1, operand2);
			context.AppendInstruction(ARMv8A32.B, condition, target);
		}

		private void IfThenElse32(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var operand3 = context.Operand3;

			// TODO: operand2 must be a register
			// if not place it into a register

			context.SetInstruction(ARMv8A32.Cmp, null, operand1, ConstantZero32);
			context.AppendInstruction(ARMv8A32.Mov, ConditionCode.NotZero, result, operand2);    // true
			context.AppendInstruction(ARMv8A32.Mov, ConditionCode.Zero, result, operand3);       // false
		}

		private void ConvertR4ToInt32(Context context)
		{
			context.ReplaceInstruction(ARMv8A32.Fix);
		}

		private void ConvertR8ToInt32(Context context)
		{
			context.ReplaceInstruction(ARMv8A32.Fix);
		}

		private void ConvertInt32ToR4(Context context)
		{
			context.ReplaceInstruction(ARMv8A32.Flt);
		}

		private void ConvertInt32ToR8(Context context)
		{
			context.ReplaceInstruction(ARMv8A32.Flt);
		}

		private void DivR4(Context context)
		{
			context.ReplaceInstruction(ARMv8A32.Dvf);
		}

		private void DivR8(Context context)
		{
			context.ReplaceInstruction(ARMv8A32.Dvf);
		}

		private void Jmp(Context context)
		{
			context.ReplaceInstruction(ARMv8A32.B);
			context.ConditionCode = ConditionCode.Always;
		}

		private void LoadR4(Context context)
		{
			Debug.Assert(context.Result.IsR4);

			// TODO: Operand1 must be a register
			// TODO: Operand2 must be a constant between 0-255, if not create new virtual register to create new base address

			context.SetInstruction(ARMv8A32.LdfUpOffset, context.Result, context.Operand1, context.Operand2);
		}

		private void LoadR8(Context context)
		{
			Debug.Assert(context.Result.IsR8);

			// TODO: Operand1 must be a register
			// TODO: Operand2 must be a constant between 0-255, if not create new virtual register to create new base address

			context.SetInstruction(ARMv8A32.LdfUpOffset, context.Result, context.Operand1, context.Operand2);
		}

		private void LoadInt32(Context context)
		{
			Debug.Assert(!context.Result.IsR4);
			Debug.Assert(!context.Result.IsR8);

			TransformLoadInstruction(context, ARMv8A32.LdrUp32, ARMv8A32.LdrUpImm32, ARMv8A32.LdrDownImm32, context.Result, context.Operand1, context.Operand2);
		}

		private void LoadParamR4(Context context)
		{
			Debug.Assert(context.Result.IsR4);
			Debug.Assert(context.Operand1.IsConstant);

			// TODO: Operand1 must be a constant between 0-255, if not create new virtual register to create new base address

			context.SetInstruction(ARMv8A32.LdfUpOffset, context.Result, StackFrame, context.Operand1);
		}

		private void LoadParamR8(Context context)
		{
			Debug.Assert(context.Result.IsR8);
			Debug.Assert(context.Operand1.IsConstant);

			// TODO: Operand1 must be a constant between 0-255, if not create new virtual register to create new base address

			context.SetInstruction(ARMv8A32.LdfUpOffset, context.Result, StackFrame, context.Operand1);
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

		private void And32(Context context)
		{
			TransformInstruction(context, ARMv8A32.And, ARMv8A32.AndImm, context.Result, StatusRegister.NotSet, context.Operand1, context.Operand2);
		}

		private void Not32(Context context)
		{
			TransformInstruction(context, ARMv8A32.Mvn, ARMv8A32.MvnImm, context.Result, StatusRegister.NotSet, context.Operand1);
		}

		private void Or32(Context context)
		{
			TransformInstruction(context, ARMv8A32.Orr, ARMv8A32.OrrImm, context.Result, StatusRegister.NotSet, context.Operand1, context.Operand2);
		}

		private void Xor32(Context context)
		{
			TransformInstruction(context, ARMv8A32.Eor, ARMv8A32.EorImm, context.Result, StatusRegister.NotSet, context.Operand1, context.Operand2);
		}

		private void MoveR4(Context context)
		{
			context.ReplaceInstruction(ARMv8A32.Mvf);
		}

		private void MoveR8(Context context)
		{
			context.ReplaceInstruction(ARMv8A32.Mvf);
		}

		private void MoveInt32(Context context)
		{
			TransformInstruction(context, ARMv8A32.Mov, ARMv8A32.MovImm, context.Result, StatusRegister.NotSet, context.Operand1);
		}

		private void MulR4(Context context)
		{
			context.ReplaceInstruction(ARMv8A32.Muf);
		}

		private void MulR8(Context context)
		{
			context.ReplaceInstruction(ARMv8A32.Muf);
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

		private void StoreR4(Context context)
		{
			//Debug.Assert(context.Operand2.IsConstant);
			Debug.Assert(context.Operand3.IsR4);

			// TODO: Operand1 must be a virtual register
			// TODO: Operand2 must be a constant between 0-255, if not create new virtual register to create new base address
			// TODO: Operand3 must be a virtual register
			// TODO: Instruction up/down depends on base and real offset

			context.SetInstruction(ARMv8A32.StfUpOffset, null, context.Operand1, context.Operand2, context.Operand3);
		}

		private void StoreR8(Context context)
		{
			//Debug.Assert(context.Operand2.IsConstant);
			Debug.Assert(context.Operand3.IsR8);

			// TODO: Operand1 must be a virtual register
			// TODO: Operand2 must be a constant between 0-255, if not create new virtual register to create new base address
			// TODO: Operand3 must be a virtual register
			// TODO: Instruction up/down depends on base and real offset

			context.SetInstruction(ARMv8A32.StfUpOffset, null, context.Operand1, context.Operand2, context.Operand3);
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

		private void StoreParamR4(Context context)
		{
			Debug.Assert(context.Operand2.IsR4);
			Debug.Assert(context.Operand1.IsConstant);

			// TODO: Operand1 must be a constant between 0-255, if not create new virtual register to create new base address
			// TODO: Operand2 must be a virtual register
			// TODO: Instruction up/down depends on base and real offset

			context.SetInstruction(ARMv8A32.StfUpOffset, null, StackFrame, context.Operand1, context.Operand2);
		}

		private void StoreParamR8(Context context)
		{
			Debug.Assert(context.Operand2.IsR8);
			Debug.Assert(context.Operand1.IsConstant);

			// TODO: Operand1 must be a constant between 0-255, if not create new virtual register to create new base address
			// TODO: Operand2 must be a virtual register
			// TODO: Instruction up/down depends on base and real offset

			context.SetInstruction(ARMv8A32.StfUpOffset, null, StackFrame, context.Operand1, context.Operand2);
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

		private void SubR4(Context context)
		{
			context.ReplaceInstruction(ARMv8A32.Suf);
		}

		private void SubR8(Context context)
		{
			context.ReplaceInstruction(ARMv8A32.Suf);
		}

		private void SubCarryIn32(Context context)
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

		#region Helper Methods

		public static void OptimizeBranch(Context context)
		{
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			if (operand2.IsConstant || operand1.IsVirtualRegister)
				return;

			// Move constant to the right
			context.Operand1 = operand2;
			context.Operand2 = operand1;
			context.ConditionCode = context.ConditionCode.GetReverse();
		}

		private void FloatCompare(Context context, BaseInstruction instruction)
		{
			// TODO
		}

		#endregion Helper Methods
	}
}
