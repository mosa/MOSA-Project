// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.MosaTypeSystem;
using System.Diagnostics;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	/// Transforms IR instructions into their appropriate X86.
	/// </summary>
	/// <seealso cref="Mosa.Platform.x86.BaseTransformationStage" />
	/// <remarks>
	/// This transformation stage transforms IR instructions into their equivalent X86 sequences.
	/// </remarks>
	public sealed class IRTransformationStage : BaseTransformationStage
	{
		protected override void PopulateVisitationDictionary()
		{
			AddVisitation(IRInstruction.Add32, Add32);
			AddVisitation(IRInstruction.AddCarryOut32, AddCarryOut32);
			AddVisitation(IRInstruction.AddR4, AddR4);
			AddVisitation(IRInstruction.AddR8, AddR8);
			AddVisitation(IRInstruction.AddressOf, AddressOf);
			AddVisitation(IRInstruction.AddCarryIn32, AddCarryIn32);
			AddVisitation(IRInstruction.ArithShiftRight32, ArithShiftRight32);
			AddVisitation(IRInstruction.BitCopyR4To32, BitCopyR4To32);
			AddVisitation(IRInstruction.BitCopy32ToR4, BitCopy32ToR4);
			AddVisitation(IRInstruction.CallDirect, CallDirect);
			AddVisitation(IRInstruction.CompareR4, CompareR4);
			AddVisitation(IRInstruction.CompareR8, CompareR8);
			AddVisitation(IRInstruction.Compare32x32, Compare32x32);
			AddVisitation(IRInstruction.CompareBranch32, CompareBranch32);
			AddVisitation(IRInstruction.ConvertR4ToR8, ConvertR4ToR8);
			AddVisitation(IRInstruction.ConvertR4To32, ConvertR4To32);
			AddVisitation(IRInstruction.ConvertR8ToR4, ConvertR8ToR4);
			AddVisitation(IRInstruction.ConvertR8To32, ConvertR8To32);
			AddVisitation(IRInstruction.Convert32ToR4, Convert32ToR4);
			AddVisitation(IRInstruction.Convert32ToR8, Convert32ToR8);
			AddVisitation(IRInstruction.DivR4, DivR4);
			AddVisitation(IRInstruction.DivR8, DivR8);
			AddVisitation(IRInstruction.DivSigned32, DivSigned32);
			AddVisitation(IRInstruction.DivUnsigned32, DivUnsigned32);
			AddVisitation(IRInstruction.IfThenElse32, IfThenElse32);
			AddVisitation(IRInstruction.Jmp, Jmp);
			AddVisitation(IRInstruction.LoadR4, LoadR4);
			AddVisitation(IRInstruction.LoadR8, LoadR8);
			AddVisitation(IRInstruction.Load32, Load32);
			AddVisitation(IRInstruction.LoadParamR4, LoadParamR4);
			AddVisitation(IRInstruction.LoadParamR8, LoadParamR8);
			AddVisitation(IRInstruction.LoadParam32, LoadParam32);
			AddVisitation(IRInstruction.LoadParamSignExtend16x32, LoadParamSignExtend16x32);
			AddVisitation(IRInstruction.LoadParamSignExtend8x32, LoadParamSignExtend8x32);
			AddVisitation(IRInstruction.LoadParamZeroExtend16x32, LoadParamZeroExtend16x32);
			AddVisitation(IRInstruction.LoadParamZeroExtend8x32, LoadParamZeroExtend8x32);
			AddVisitation(IRInstruction.LoadSignExtend16x32, LoadSignExtend16x32);
			AddVisitation(IRInstruction.LoadSignExtend8x32, LoadSignExtend8x32);
			AddVisitation(IRInstruction.LoadZeroExtend16x32, LoadZeroExtend16x32);
			AddVisitation(IRInstruction.LoadZeroExtend8x32, LoadZeroExtend8x32);
			AddVisitation(IRInstruction.And32, And32);
			AddVisitation(IRInstruction.Not32, Not32);
			AddVisitation(IRInstruction.Or32, Or32);
			AddVisitation(IRInstruction.Xor32, Xor32);
			AddVisitation(IRInstruction.MoveR4, MoveR4);
			AddVisitation(IRInstruction.MoveR8, MoveR8);
			AddVisitation(IRInstruction.Move32, Move32);
			AddVisitation(IRInstruction.MulR4, MulR4);
			AddVisitation(IRInstruction.MulR8, MulR8);
			AddVisitation(IRInstruction.MulSigned32, MulSigned32);
			AddVisitation(IRInstruction.MulUnsigned32, MulUnsigned32);
			AddVisitation(IRInstruction.Nop, Nop);
			AddVisitation(IRInstruction.RemSigned32, RemSigned32);
			AddVisitation(IRInstruction.RemUnsigned32, RemUnsigned32);
			AddVisitation(IRInstruction.ShiftLeft32, ShiftLeft32);
			AddVisitation(IRInstruction.ShiftRight32, ShiftRight32);
			AddVisitation(IRInstruction.SignExtend16x32, SignExtend16x32);
			AddVisitation(IRInstruction.SignExtend8x32, SignExtend8x32);
			AddVisitation(IRInstruction.StoreR4, StoreR4);
			AddVisitation(IRInstruction.StoreR8, StoreR8);
			AddVisitation(IRInstruction.Store16, StoreInt16);
			AddVisitation(IRInstruction.Store32, Store32);
			AddVisitation(IRInstruction.Store8, StoreInt8);
			AddVisitation(IRInstruction.StoreParamR4, StoreParamR4);
			AddVisitation(IRInstruction.StoreParamR8, StoreParamR8);
			AddVisitation(IRInstruction.StoreParam16, StoreParamInt16);
			AddVisitation(IRInstruction.StoreParam32, StoreParam32);
			AddVisitation(IRInstruction.StoreParam8, StoreParamInt8);
			AddVisitation(IRInstruction.Sub32, Sub32);
			AddVisitation(IRInstruction.SubCarryOut32, SubCarryOut32);
			AddVisitation(IRInstruction.SubR4, SubR4);
			AddVisitation(IRInstruction.SubR8, SubR8);
			AddVisitation(IRInstruction.SubCarryIn32, SubCarryIn32);
			AddVisitation(IRInstruction.Switch, Switch);
			AddVisitation(IRInstruction.ZeroExtend16x32, ZeroExtend16x32);
			AddVisitation(IRInstruction.ZeroExtend8x32, ZeroExtend8x32);
		}

		#region Visitation Methods

		private void Add32(Context context)
		{
			context.ReplaceInstruction(X86.Add32);
		}

		private void AddCarryOut32(Context context)
		{
			var result = context.Result;
			var result2 = context.Result2;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.Boolean);

			context.SetInstruction(X86.Add32, result, operand1, operand2);
			context.AppendInstruction(X86.Setcc, ConditionCode.Carry, v1);
			context.AppendInstruction(X86.Movzx8To32, result2, v1);
		}

		private void AddR4(Context context)
		{
			Debug.Assert(context.Result.IsR4);
			Debug.Assert(context.Operand1.IsR4);

			context.ReplaceInstruction(X86.Addss);
		}

		private void AddR8(Context context)
		{
			Debug.Assert(context.Result.IsR8);
			Debug.Assert(context.Operand1.IsR8);

			context.ReplaceInstruction(X86.Addsd);
		}

		private void AddressOf(Context context)
		{
			Debug.Assert(context.Operand1.IsOnStack || context.Operand1.IsStaticField);

			if (context.Operand1.IsStaticField)
			{
				context.SetInstruction(X86.Mov32, context.Result, context.Operand1);
			}
			else if (context.Operand1.IsStackLocal)
			{
				context.SetInstruction(X86.Lea32, context.Result, StackFrame, context.Operand1);
			}
			else
			{
				var offset = CreateConstant(context.Operand1.Offset);

				context.SetInstruction(X86.Lea32, context.Result, StackFrame, offset);
			}
		}

		private void AddCarryIn32(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var operand3 = context.Operand3;

			context.SetInstruction(X86.Add32, result, operand1, operand2);
			context.AppendInstruction(X86.Add32, result, result, operand3);
		}

		private void ArithShiftRight32(Context context)
		{
			context.ReplaceInstruction(X86.Sar32);
		}

		private void BitCopyR4To32(Context context)
		{
			context.ReplaceInstruction(X86.Movdssi32);
		}

		private void BitCopy32ToR4(Context context)
		{
			context.ReplaceInstruction(X86.Movdi32ss);
		}

		private void CallDirect(Context context)
		{
			context.ReplaceInstruction(X86.Call);
		}

		private void CompareR4(Context context)
		{
			FloatCompare(context, X86.Ucomiss);
		}

		private void CompareR8(Context context)
		{
			FloatCompare(context, X86.Ucomisd);
		}

		private void Compare32x32(Context context)
		{
			var condition = context.ConditionCode;
			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			context.SetInstruction(X86.Cmp32, null, operand1, operand2);
			context.AppendInstruction(X86.Setcc, condition, v1);
			context.AppendInstruction(X86.Movzx8To32, result, v1);
		}

		private void CompareBranch32(Context context)
		{
			OptimizeBranch(context);

			var target = context.BranchTargets[0];
			var condition = context.ConditionCode;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			context.SetInstruction(X86.Cmp32, null, operand1, operand2);
			context.AppendInstruction(X86.Branch, condition, target);
		}

		private void ConvertR4ToR8(Context context)
		{
			context.ReplaceInstruction(X86.Cvtss2sd);
		}

		private void ConvertR4To32(Context context)
		{
			Debug.Assert(context.Result.IsI1 || context.Result.IsI2 || context.Result.IsI4);
			context.ReplaceInstruction(X86.Cvttss2si32);
		}

		private void ConvertR8ToR4(Context context)
		{
			context.ReplaceInstruction(X86.Cvtsd2ss);
		}

		private void ConvertR8To32(Context context)
		{
			Debug.Assert(context.Result.IsI1 || context.Result.IsI2 || context.Result.IsI4);
			context.ReplaceInstruction(X86.Cvttsd2si32);
		}

		private void Convert32ToR4(Context context)
		{
			Debug.Assert(context.Result.IsR4);
			context.ReplaceInstruction(X86.Cvtsi2ss32);
		}

		private void Convert32ToR8(Context context)
		{
			Debug.Assert(context.Result.IsR8);
			context.ReplaceInstruction(X86.Cvtsi2sd32);
		}

		private void DivR4(Context context)
		{
			Debug.Assert(context.Result.IsR4);
			Debug.Assert(context.Operand1.IsR4);

			context.ReplaceInstruction(X86.Divss);
		}

		private void DivR8(Context context)
		{
			Debug.Assert(context.Result.IsR8);
			Debug.Assert(context.Operand1.IsR8);

			context.ReplaceInstruction(X86.Divsd);
		}

		private void DivSigned32(Context context)
		{
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var result = context.Result;

			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var v2 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);
			var v3 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			context.SetInstruction2(X86.Cdq32, v1, v2, operand1);
			context.AppendInstruction2(X86.IDiv32, v3, result, v1, v2, operand2);
		}

		private void DivUnsigned32(Context context)
		{
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var result = context.Result;

			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);
			var v2 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);

			context.SetInstruction(X86.Mov32, v1, ConstantZero32);
			context.AppendInstruction2(X86.Div32, v1, v2, v1, operand1, operand2);
			context.AppendInstruction(X86.Mov32, result, v2);
		}

		private void IfThenElse32(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var operand3 = context.Operand3;

			context.SetInstruction(X86.Cmp32, null, operand1, ConstantZero32);
			context.AppendInstruction(X86.CMov32, ConditionCode.NotEqual, result, result, operand2);    // true
			context.AppendInstruction(X86.CMov32, ConditionCode.Equal, result, result, operand3);       // false
		}

		private void Jmp(Context context)
		{
			context.ReplaceInstruction(X86.Jmp);
		}

		private void LoadR4(Context context)
		{
			Debug.Assert(context.Result.IsR4);

			context.SetInstruction(X86.MovssLoad, context.Result, context.Operand1, context.Operand2);
		}

		private void LoadR8(Context context)
		{
			Debug.Assert(context.Result.IsR8);

			context.SetInstruction(X86.MovsdLoad, context.Result, context.Operand1, context.Operand2);
		}

		private void Load32(Context context)
		{
			Debug.Assert(!context.Result.IsR4);
			Debug.Assert(!context.Result.IsR8);

			LoadStore.OrderLoadOperands(context, MethodCompiler);

			context.SetInstruction(X86.MovLoad32, context.Result, context.Operand1, context.Operand2);
		}

		private void LoadParamR4(Context context)
		{
			Debug.Assert(context.Result.IsR4);

			context.SetInstruction(X86.MovssLoad, context.Result, StackFrame, context.Operand1);
		}

		private void LoadParamR8(Context context)
		{
			Debug.Assert(context.Result.IsR8);

			context.SetInstruction(X86.MovsdLoad, context.Result, StackFrame, context.Operand1);
		}

		private void LoadParam32(Context context)
		{
			context.SetInstruction(X86.MovLoad32, context.Result, StackFrame, context.Operand1);
		}

		private void LoadParamSignExtend16x32(Context context)
		{
			context.SetInstruction(X86.MovsxLoad16, context.Result, StackFrame, context.Operand1);
		}

		private void LoadParamSignExtend8x32(Context context)
		{
			context.SetInstruction(X86.MovsxLoad8, context.Result, StackFrame, context.Operand1);
		}

		private void LoadParamZeroExtend16x32(Context context)
		{
			context.SetInstruction(X86.MovzxLoad16, context.Result, StackFrame, context.Operand1);
		}

		private void LoadParamZeroExtend8x32(Context context)
		{
			context.SetInstruction(X86.MovzxLoad8, context.Result, StackFrame, context.Operand1);
		}

		private void LoadSignExtend16x32(Context context)
		{
			LoadStore.OrderLoadOperands(context, MethodCompiler);

			context.SetInstruction(X86.MovsxLoad16, context.Result, context.Operand1, context.Operand2);
		}

		private void LoadSignExtend8x32(Context context)
		{
			LoadStore.OrderLoadOperands(context, MethodCompiler);

			context.SetInstruction(X86.MovsxLoad8, context.Result, context.Operand1, context.Operand2);
		}

		private void LoadZeroExtend16x32(Context context)
		{
			LoadStore.OrderLoadOperands(context, MethodCompiler);

			context.SetInstruction(X86.MovzxLoad16, context.Result, context.Operand1, context.Operand2);
		}

		private void LoadZeroExtend8x32(Context context)
		{
			LoadStore.OrderLoadOperands(context, MethodCompiler);

			context.SetInstruction(X86.MovzxLoad8, context.Result, context.Operand1, context.Operand2);
		}

		private void And32(Context context)
		{
			context.ReplaceInstruction(X86.And32);
		}

		private void Not32(Context context)
		{
			context.SetInstruction(X86.Not32, context.Result, context.Operand1);
		}

		private void Or32(Context context)
		{
			context.ReplaceInstruction(X86.Or32);
		}

		private void Xor32(Context context)
		{
			context.ReplaceInstruction(X86.Xor32);
		}

		private void MoveR4(Context context)
		{
			context.ReplaceInstruction(X86.Movss);
		}

		private void MoveR8(Context context)
		{
			context.ReplaceInstruction(X86.Movsd);
		}

		private void Move32(Context context)
		{
			context.ReplaceInstruction(X86.Mov32);
		}

		private void MulR4(Context context)
		{
			Debug.Assert(context.Result.IsR4);
			Debug.Assert(context.Operand1.IsR4);

			context.ReplaceInstruction(X86.Mulss);
		}

		private void MulR8(Context context)
		{
			Debug.Assert(context.Result.IsR8);
			Debug.Assert(context.Operand1.IsR8);

			context.ReplaceInstruction(X86.Mulsd);
		}

		private void MulSigned32(Context context)
		{
			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);
			context.SetInstruction2(X86.Mul32, v1, context.Result, context.Operand1, context.Operand2);
		}

		private void MulUnsigned32(Context context)
		{
			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);
			context.SetInstruction2(X86.Mul32, v1, context.Result, context.Operand1, context.Operand2);
		}

		private void Nop(Context context)
		{
			context.Empty();

			//context.SetInstruction(X86.Nop);
		}

		private void RemSigned32(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var v2 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);
			var v3 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			context.SetInstruction2(X86.Cdq32, v1, v2, operand1);
			context.AppendInstruction2(X86.IDiv32, result, v3, v1, v2, operand2);
		}

		private void RemUnsigned32(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);
			var v2 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);

			context.SetInstruction(X86.Mov32, v1, ConstantZero32);
			context.AppendInstruction2(X86.Div32, result, v2, v1, operand1, operand2);
		}

		private void ShiftLeft32(Context context)
		{
			context.ReplaceInstruction(X86.Shl32);
		}

		private void ShiftRight32(Context context)
		{
			context.ReplaceInstruction(X86.Shr32);
		}

		private void SignExtend16x32(Context context)
		{
			context.ReplaceInstruction(X86.Movsx16To32);
		}

		private void SignExtend8x32(Context context)
		{
			context.ReplaceInstruction(X86.Movsx8To32);
		}

		private void StoreR4(Context context)
		{
			context.SetInstruction(X86.MovssStore, null, context.Operand1, context.Operand2, context.Operand3);
		}

		private void StoreR8(Context context)
		{
			context.SetInstruction(X86.MovsdStore, null, context.Operand1, context.Operand2, context.Operand3);
		}

		private void StoreInt16(Context context)
		{
			LoadStore.OrderStoreOperands(context, MethodCompiler);

			context.SetInstruction(X86.MovStore16, null, context.Operand1, context.Operand2, context.Operand3);
		}

		private void Store32(Context context)
		{
			LoadStore.OrderStoreOperands(context, MethodCompiler);

			context.SetInstruction(X86.MovStore32, null, context.Operand1, context.Operand2, context.Operand3);
		}

		private void StoreInt8(Context context)
		{
			LoadStore.OrderStoreOperands(context, MethodCompiler);

			context.SetInstruction(X86.MovStore8, null, context.Operand1, context.Operand2, context.Operand3);
		}

		private void StoreParamR4(Context context)
		{
			context.SetInstruction(X86.MovssStore, null, StackFrame, context.Operand1, context.Operand2);
		}

		private void StoreParamR8(Context context)
		{
			context.SetInstruction(X86.MovsdStore, null, StackFrame, context.Operand1, context.Operand2);
		}

		private void StoreParamInt16(Context context)
		{
			context.SetInstruction(X86.MovStore16, null, StackFrame, context.Operand1, context.Operand2);
		}

		private void StoreParam32(Context context)
		{
			context.SetInstruction(X86.MovStore32, null, StackFrame, context.Operand1, context.Operand2);
		}

		private void StoreParamInt8(Context context)
		{
			context.SetInstruction(X86.MovStore8, null, StackFrame, context.Operand1, context.Operand2);
		}

		private void Sub32(Context context)
		{
			context.ReplaceInstruction(X86.Sub32);
		}

		private void SubCarryOut32(Context context)
		{
			var result = context.Result;
			var result2 = context.Result2;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.Boolean);

			context.SetInstruction(X86.Sub32, result, operand1, operand2);
			context.AppendInstruction(X86.Setcc, ConditionCode.Carry, v1);
			context.AppendInstruction(X86.Movzx8To32, result2, v1);
		}

		private void SubR4(Context context)
		{
			Debug.Assert(context.Result.IsR4);
			Debug.Assert(context.Operand1.IsR4);

			context.ReplaceInstruction(X86.Subss);
		}

		private void SubR8(Context context)
		{
			Debug.Assert(context.Result.IsR8);
			Debug.Assert(context.Operand1.IsR8);

			context.ReplaceInstruction(X86.Subsd);
		}

		private void SubCarryIn32(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var operand3 = context.Operand3;

			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			context.SetInstruction(X86.Bt32, v1, operand3, CreateConstant((byte)0));
			context.AppendInstruction(X86.Sbb32, result, operand1, operand2);
		}

		private void Switch(Context context)
		{
			var targets = context.BranchTargets;
			var operand = context.Operand1;

			context.Empty();

			for (int i = 0; i < targets.Count - 1; ++i)
			{
				context.AppendInstruction(X86.Cmp32, null, operand, CreateConstant(i));
				context.AppendInstruction(X86.Branch, ConditionCode.Equal, targets[i]);
			}
		}

		private void ZeroExtend16x32(Context context)
		{
			context.ReplaceInstruction(X86.Movzx16To32);
		}

		private void ZeroExtend8x32(Context context)
		{
			context.ReplaceInstruction(X86.Movzx8To32);
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

		private void FloatCompare(Context context, X86Instruction instruction)
		{
			var result = context.Result;
			var left = context.Operand1;
			var right = context.Operand2;
			var condition = context.ConditionCode;

			// normalize condition
			switch (condition)
			{
				case ConditionCode.Equal: break;
				case ConditionCode.NotEqual: break;
				case ConditionCode.UnsignedGreaterOrEqual: condition = ConditionCode.GreaterOrEqual; break;
				case ConditionCode.UnsignedGreaterThan: condition = ConditionCode.GreaterThan; break;
				case ConditionCode.UnsignedLessOrEqual: condition = ConditionCode.LessOrEqual; break;
				case ConditionCode.UnsignedLessThan: condition = ConditionCode.LessThan; break;
			}

			Debug.Assert(!(left.IsR4 && right.IsR8));
			Debug.Assert(!(left.IsR8 && right.IsR4));

			switch (condition)
			{
				case ConditionCode.Equal:
					{
						//  a==b
						//	mov	eax, 1
						//	ucomisd	xmm0, xmm1
						//	jp	L3
						//	jne	L3
						//	ret
						//L3:
						//	mov	eax, 0

						var newBlocks = CreateNewBlockContexts(2, context.Label);
						var nextBlock = Split(context);

						context.SetInstruction(X86.Mov32, result, CreateConstant(1));
						context.AppendInstruction(instruction, null, left, right);
						context.AppendInstruction(X86.Branch, ConditionCode.Parity, newBlocks[1].Block);
						context.AppendInstruction(X86.Jmp, newBlocks[0].Block);

						newBlocks[0].AppendInstruction(X86.Branch, ConditionCode.NotEqual, newBlocks[1].Block);
						newBlocks[0].AppendInstruction(X86.Jmp, nextBlock.Block);

						newBlocks[1].AppendInstruction(X86.Mov32, result, ConstantZero32);
						newBlocks[1].AppendInstruction(X86.Jmp, nextBlock.Block);
						break;
					}
				case ConditionCode.NotEqual:
					{
						//  a!=b
						//	mov	eax, 1
						//	ucomisd	xmm0, xmm1
						//	jp	L5
						//	setne	al
						//	movzx	eax, al
						//L5:

						var newBlocks = CreateNewBlockContexts(1, context.Label);
						var nextBlock = Split(context);

						context.SetInstruction(X86.Mov32, result, CreateConstant(1));
						context.AppendInstruction(instruction, null, left, right);
						context.AppendInstruction(X86.Branch, ConditionCode.Parity, nextBlock.Block);
						context.AppendInstruction(X86.Jmp, newBlocks[0].Block);
						newBlocks[0].AppendInstruction(X86.Setcc, ConditionCode.NotEqual, result);

						//newBlocks[0].AppendInstruction(X86.Movzx, InstructionSize.Size8, result, result);
						newBlocks[0].AppendInstruction(X86.Jmp, nextBlock.Block);
						break;
					}
				case ConditionCode.LessThan:
					{
						//	a<b
						//	mov	eax, 0
						//	ucomisd	xmm1, xmm0
						//	seta	al

						context.SetInstruction(X86.Mov32, result, ConstantZero32);
						context.AppendInstruction(instruction, null, right, left);
						context.AppendInstruction(X86.Setcc, ConditionCode.UnsignedGreaterThan, result);
						break;
					}
				case ConditionCode.GreaterThan:
					{
						//	a>b
						//	mov	eax, 0
						//	ucomisd	xmm0, xmm1
						//	seta	al

						context.SetInstruction(X86.Mov32, result, ConstantZero32);
						context.AppendInstruction(instruction, null, left, right);
						context.AppendInstruction(X86.Setcc, ConditionCode.UnsignedGreaterThan, result);
						break;
					}
				case ConditionCode.LessOrEqual:
					{
						//	a<=b
						//	mov	eax, 0
						//	ucomisd	xmm1, xmm0
						//	setae	al

						context.SetInstruction(X86.Mov32, result, ConstantZero32);
						context.AppendInstruction(instruction, null, right, left);
						context.AppendInstruction(X86.Setcc, ConditionCode.UnsignedGreaterOrEqual, result);
						break;
					}
				case ConditionCode.GreaterOrEqual:
					{
						//	a>=b
						//	mov	eax, 0
						//	ucomisd	xmm0, xmm1
						//	setae	al

						context.SetInstruction(X86.Mov32, result, ConstantZero32);
						context.AppendInstruction(instruction, null, left, right);
						context.AppendInstruction(X86.Setcc, ConditionCode.UnsignedGreaterOrEqual, result);
						break;
					}
			}
		}

		//private void CopyCompound(Context context, MosaType type, Operand destinationBase, Operand destination, Operand sourceBase, Operand source)
		//{
		//	int size = TypeLayout.GetTypeSize(type);
		//	const int LargeAlignment = 16;
		//	int alignedSize = size - (size % NativeAlignment);
		//	int largeAlignedTypeSize = size - (size % LargeAlignment);

		//	Debug.Assert(size > 0);

		//	var srcReg = AllocateVirtualRegister(destinationBase.Type.TypeSystem.BuiltIn.I4);
		//	var dstReg = AllocateVirtualRegister(destinationBase.Type.TypeSystem.BuiltIn.I4);

		//	context.SetInstruction(IRInstruction.UnstableObjectTracking);

		//	context.AppendInstruction(X86.Lea32, srcReg, sourceBase, source);
		//	context.AppendInstruction(X86.Lea32, dstReg, destinationBase, destination);

		//	var tmp = AllocateVirtualRegister(destinationBase.Type.TypeSystem.BuiltIn.I4);
		//	var tmpLarge = AllocateVirtualRegister(destinationBase.Type.TypeSystem.BuiltIn.R8);

		//	for (int i = 0; i < largeAlignedTypeSize; i += LargeAlignment)
		//	{
		//		// Large aligned moves allow 128bits to be copied at a time
		//		var index = CreateConstant(i);
		//		context.AppendInstruction(X86.MovupsLoad, tmpLarge, srcReg, index);
		//		context.AppendInstruction(X86.MovupsStore, null, dstReg, index, tmpLarge);
		//	}
		//	for (int i = largeAlignedTypeSize; i < alignedSize; i += 4)
		//	{
		//		var index = CreateConstant(i);
		//		context.AppendInstruction(X86.MovLoad32, tmp, srcReg, index);
		//		context.AppendInstruction(X86.MovStore32, null, dstReg, index, tmp);
		//	}
		//	for (int i = alignedSize; i < size; i++)
		//	{
		//		var index = CreateConstant(i);
		//		context.AppendInstruction(X86.MovLoad8, tmp, srcReg, index);
		//		context.AppendInstruction(X86.MovStore8, null, dstReg, index, tmp);
		//	}

		//	context.AppendInstruction(IRInstruction.StableObjectTracking);
		//}

		#endregion Helper Methods
	}
}
