// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.MosaTypeSystem;
using Mosa.Platform.Intel;
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
			AddVisitation(IRInstruction.Branch32, Branch32);
			AddVisitation(IRInstruction.BranchObject, BranchObject);
			AddVisitation(IRInstruction.CompareObject, CompareObject);
			AddVisitation(IRInstruction.CompareR4, CompareR4);
			AddVisitation(IRInstruction.CompareR8, CompareR8);
			AddVisitation(IRInstruction.Compare32x32, Compare32x32);
			AddVisitation(IRInstruction.ConvertR4ToR8, ConvertR4ToR8);
			AddVisitation(IRInstruction.ConvertR4ToI32, ConvertR4ToI32);
			AddVisitation(IRInstruction.ConvertR8ToR4, ConvertR8ToR4);
			AddVisitation(IRInstruction.ConvertR8ToI32, ConvertR8ToI32);
			AddVisitation(IRInstruction.ConvertI32ToR4, ConvertI32ToR4);
			AddVisitation(IRInstruction.ConvertI32ToR8, ConvertI32ToR8);

			//AddVisitation(IRInstruction.ConvertR4ToI32, ConvertR4ToI32);	// TODO
			//AddVisitation(IRInstruction.ConvertR8ToI32, ConvertR8ToI32);	// TODO
			//AddVisitation(IRInstruction.ConvertU32ToR4, ConvertU32ToR4);	// TODO
			//AddVisitation(IRInstruction.ConvertU32ToR8, ConvertU32ToR8);	// TODO

			AddVisitation(IRInstruction.DivR4, DivR4);
			AddVisitation(IRInstruction.DivR8, DivR8);
			AddVisitation(IRInstruction.DivSigned32, DivSigned32);
			AddVisitation(IRInstruction.DivUnsigned32, DivUnsigned32);
			AddVisitation(IRInstruction.IfThenElse32, IfThenElse32);
			AddVisitation(IRInstruction.Jmp, Jmp);
			AddVisitation(IRInstruction.LoadR4, LoadR4);
			AddVisitation(IRInstruction.LoadR8, LoadR8);
			AddVisitation(IRInstruction.LoadObject, LoadObject);
			AddVisitation(IRInstruction.Load32, Load32);
			AddVisitation(IRInstruction.LoadParamObject, LoadParamObject);
			AddVisitation(IRInstruction.LoadParam32, LoadParam32);
			AddVisitation(IRInstruction.LoadParamR4, LoadParamR4);
			AddVisitation(IRInstruction.LoadParamR8, LoadParamR8);
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
			AddVisitation(IRInstruction.MoveObject, MoveObject);
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
			AddVisitation(IRInstruction.StoreObject, StoreObject);
			AddVisitation(IRInstruction.Store8, StoreInt8);
			AddVisitation(IRInstruction.StoreParamR4, StoreParamR4);
			AddVisitation(IRInstruction.StoreParamR8, StoreParamR8);
			AddVisitation(IRInstruction.StoreParam16, StoreParamInt16);
			AddVisitation(IRInstruction.StoreParam32, StoreParam32);
			AddVisitation(IRInstruction.StoreParamObject, StoreParamObject);
			AddVisitation(IRInstruction.StoreParam8, StoreParamInt8);
			AddVisitation(IRInstruction.Sub32, Sub32);
			AddVisitation(IRInstruction.SubCarryOut32, SubCarryOut32);
			AddVisitation(IRInstruction.SubR4, SubR4);
			AddVisitation(IRInstruction.SubR8, SubR8);
			AddVisitation(IRInstruction.SubCarryIn32, SubCarryIn32);
			AddVisitation(IRInstruction.Switch, Switch);
			AddVisitation(IRInstruction.ZeroExtend16x32, ZeroExtend16x32);
			AddVisitation(IRInstruction.ZeroExtend8x32, ZeroExtend8x32);

			// 64-bit Transforms

			AddVisitation(IRInstruction.Add64, Add64);
			AddVisitation(IRInstruction.BitCopyR8To64, BitCopyFloatR8To64);
			AddVisitation(IRInstruction.BitCopy64ToR8, BitCopy64ToFloatR8);
			AddVisitation(IRInstruction.ArithShiftRight64, ArithShiftRight64);
			AddVisitation(IRInstruction.Call, Call);
			AddVisitation(IRInstruction.Compare32x64, Compare32x64);
			AddVisitation(IRInstruction.Compare64x32, Compare64x32);
			AddVisitation(IRInstruction.Compare64x64, Compare64x64);
			AddVisitation(IRInstruction.Branch64, Branch64);
			AddVisitation(IRInstruction.ConvertR4ToI64, ConvertFloatR4To64);
			AddVisitation(IRInstruction.ConvertR8ToI64, ConvertFloatR8ToInteger64);
			AddVisitation(IRInstruction.ConvertI64ToR4, Convert64ToFloatR4);
			AddVisitation(IRInstruction.ConvertI64ToR8, Convert64ToFloatR8);

			//AddVisitation(IRInstruction.ConvertR4ToI64, ConvertR4ToI64);
			//AddVisitation(IRInstruction.ConvertR8ToI64, ConvertR8ToI64);
			//AddVisitation(IRInstruction.ConvertU64ToR4, ConvertU64ToR4);
			//AddVisitation(IRInstruction.ConvertU64ToR8, ConvertU64ToR8);

			AddVisitation(IRInstruction.IfThenElse64, IfThenElse64);
			AddVisitation(IRInstruction.Load64, Load64);
			AddVisitation(IRInstruction.LoadParam64, LoadParam64);
			AddVisitation(IRInstruction.LoadParamSignExtend16x64, LoadParamSignExtend16x64);
			AddVisitation(IRInstruction.LoadParamSignExtend32x64, LoadParamSignExtend32x64);
			AddVisitation(IRInstruction.LoadParamSignExtend8x64, LoadParamSignExtend8x64);
			AddVisitation(IRInstruction.LoadParamZeroExtend16x64, LoadParamZeroExtended16x64);
			AddVisitation(IRInstruction.LoadParamZeroExtend32x64, LoadParamZeroExtended32x64);
			AddVisitation(IRInstruction.LoadParamZeroExtend8x64, LoadParamZeroExtended8x64);
			AddVisitation(IRInstruction.And64, And64);
			AddVisitation(IRInstruction.Not64, Not64);
			AddVisitation(IRInstruction.Or64, Or64);
			AddVisitation(IRInstruction.Xor64, Xor64);
			AddVisitation(IRInstruction.Move64, Move64);
			AddVisitation(IRInstruction.MulSigned64, MulSigned64);
			AddVisitation(IRInstruction.MulUnsigned64, MulUnsigned64);
			AddVisitation(IRInstruction.ShiftLeft64, ShiftLeft64);
			AddVisitation(IRInstruction.ShiftRight64, ShiftRight64);
			AddVisitation(IRInstruction.SignExtend16x64, SignExtend16x64);
			AddVisitation(IRInstruction.SignExtend32x64, SignExtend32x64);
			AddVisitation(IRInstruction.SignExtend8x64, SignExtend8x64);
			AddVisitation(IRInstruction.GetHigh32, GetHigh32);
			AddVisitation(IRInstruction.GetLow32, GetLow32);
			AddVisitation(IRInstruction.Store64, Store64);
			AddVisitation(IRInstruction.StoreParam64, StoreParam64);
			AddVisitation(IRInstruction.Sub64, Sub64);
			AddVisitation(IRInstruction.To64, To64);
			AddVisitation(IRInstruction.Truncate64x32, Truncate64x32);
			AddVisitation(IRInstruction.ZeroExtend16x64, ZeroExtended16x64);
			AddVisitation(IRInstruction.ZeroExtend32x64, ZeroExtended32x64);
			AddVisitation(IRInstruction.ZeroExtend8x64, ZeroExtended8x64);
		}

		#region 32-bit Transformations

		private void Add32(Context context)
		{
			context.ReplaceInstruction(X86.Add32);
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
			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			operand1 = MoveConstantToFloatRegister(context, operand1);
			operand2 = MoveConstantToFloatRegister(context, operand2);

			context.SetInstruction(X86.Addss, result, operand1, operand2);
		}

		private void AddR8(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			operand1 = MoveConstantToFloatRegister(context, operand1);
			operand2 = MoveConstantToFloatRegister(context, operand2);

			context.SetInstruction(X86.Addsd, result, operand1, operand2);
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
				var offset = CreateConstant32(context.Operand1.Offset);

				context.SetInstruction(X86.Lea32, context.Result, StackFrame, offset);
			}
		}

		private void And32(Context context)
		{
			context.ReplaceInstruction(X86.And32);
		}

		private void ArithShiftRight32(Context context)
		{
			context.ReplaceInstruction(X86.Sar32);
		}

		private void BitCopy32ToR4(Context context)
		{
			context.ReplaceInstruction(X86.Movdi32ss);
		}

		private void BitCopyR4To32(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;

			operand1 = MoveConstantToFloatRegister(context, operand1);

			context.SetInstruction(X86.Movdssi32, result, operand1);
		}

		private void Branch32(Context context)
		{
			MoveConstantRight(context);

			var target = context.BranchTargets[0];
			var condition = context.ConditionCode;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			context.SetInstruction(X86.Cmp32, null, operand1, operand2);
			context.AppendInstruction(X86.Branch, condition, target);
		}

		private void BranchObject(Context context)
		{
			MoveConstantRight(context);

			var target = context.BranchTargets[0];
			var condition = context.ConditionCode;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			context.SetInstruction(X86.Cmp32, null, operand1, operand2);
			context.AppendInstruction(X86.Branch, condition, target);
		}

		private void CallDirect(Context context)
		{
			context.ReplaceInstruction(X86.Call);
		}

		private void Compare32x32(Context context)
		{
			var condition = context.ConditionCode;
			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			var v1 = AllocateVirtualRegisterI32();

			context.SetInstruction(X86.Cmp32, null, operand1, operand2);
			context.AppendInstruction(X86.Setcc, condition, v1);
			context.AppendInstruction(X86.Movzx8To32, result, v1);
		}

		private void CompareObject(Context context)
		{
			var condition = context.ConditionCode;
			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			var v1 = AllocateVirtualRegisterI32();

			context.SetInstruction(X86.Cmp32, null, operand1, operand2);
			context.AppendInstruction(X86.Setcc, condition, v1);
			context.AppendInstruction(X86.Movzx8To32, result, v1);
		}

		private void CompareR4(Context context)
		{
			FloatCompare(context, X86.Ucomiss);
		}

		private void CompareR8(Context context)
		{
			FloatCompare(context, X86.Ucomisd);
		}

		private void ConvertI32ToR4(Context context)
		{
			Debug.Assert(context.Result.IsR4);

			context.ReplaceInstruction(X86.Cvtsi2ss32);
		}

		private void ConvertI32ToR8(Context context)
		{
			Debug.Assert(context.Result.IsR8);

			context.ReplaceInstruction(X86.Cvtsi2sd32);
		}

		private void ConvertR4ToI32(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;

			operand1 = MoveConstantToFloatRegister(context, operand1);

			context.SetInstruction(X86.Cvttss2si32, result, operand1);
		}

		private void ConvertR4ToR8(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;

			operand1 = MoveConstantToFloatRegister(context, operand1);

			context.SetInstruction(X86.Cvtss2sd, result, operand1);
		}

		private void ConvertR8ToI32(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;

			operand1 = MoveConstantToFloatRegister(context, operand1);

			context.SetInstruction(X86.Cvttsd2si32, result, operand1);
		}

		private void ConvertR8ToR4(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;

			operand1 = MoveConstantToFloatRegister(context, operand1);

			context.SetInstruction(X86.Cvtsd2ss, result, operand1);
		}

		private void DivR4(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			operand1 = MoveConstantToFloatRegister(context, operand1);
			operand2 = MoveConstantToFloatRegister(context, operand2);

			context.SetInstruction(X86.Divss, result, operand1, operand2);
		}

		private void DivR8(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			operand1 = MoveConstantToFloatRegister(context, operand1);
			operand2 = MoveConstantToFloatRegister(context, operand2);

			context.SetInstruction(X86.Divsd, result, operand1, operand2);
		}

		private void DivSigned32(Context context)
		{
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var result = context.Result;

			var v1 = AllocateVirtualRegisterI32();
			var v2 = AllocateVirtualRegisterI32();
			var v3 = AllocateVirtualRegisterI32();

			context.SetInstruction2(X86.Cdq32, v1, v2, operand1);
			context.AppendInstruction2(X86.IDiv32, v3, result, v1, v2, operand2);
		}

		private void DivUnsigned32(Context context)
		{
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var result = context.Result;

			var v1 = AllocateVirtualRegisterI32();
			var v2 = AllocateVirtualRegisterI32();

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

		private void Load32(Context context)
		{
			Debug.Assert(!context.Result.IsR4);
			Debug.Assert(!context.Result.IsR8);

			LoadStore.OrderOperands(context, MethodCompiler);

			context.SetInstruction(X86.MovLoad32, context.Result, context.Operand1, context.Operand2);
		}

		private void LoadObject(Context context)
		{
			Debug.Assert(!context.Result.IsR4);
			Debug.Assert(!context.Result.IsR8);

			LoadStore.OrderOperands(context, MethodCompiler);

			context.SetInstruction(X86.MovLoad32, context.Result, context.Operand1, context.Operand2);
		}

		private void LoadParam32(Context context)
		{
			context.SetInstruction(X86.MovLoad32, context.Result, StackFrame, context.Operand1);
		}

		private void LoadParamObject(Context context)
		{
			context.SetInstruction(X86.MovLoad32, context.Result, StackFrame, context.Operand1);
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

		private void LoadSignExtend16x32(Context context)
		{
			LoadStore.OrderOperands(context, MethodCompiler);

			context.SetInstruction(X86.MovsxLoad16, context.Result, context.Operand1, context.Operand2);
		}

		private void LoadSignExtend8x32(Context context)
		{
			LoadStore.OrderOperands(context, MethodCompiler);

			context.SetInstruction(X86.MovsxLoad8, context.Result, context.Operand1, context.Operand2);
		}

		private void LoadZeroExtend16x32(Context context)
		{
			LoadStore.OrderOperands(context, MethodCompiler);

			context.SetInstruction(X86.MovzxLoad16, context.Result, context.Operand1, context.Operand2);
		}

		private void LoadZeroExtend8x32(Context context)
		{
			LoadStore.OrderOperands(context, MethodCompiler);

			context.SetInstruction(X86.MovzxLoad8, context.Result, context.Operand1, context.Operand2);
		}

		private void Move32(Context context)
		{
			context.ReplaceInstruction(X86.Mov32);
		}

		private void MoveObject(Context context)
		{
			context.ReplaceInstruction(X86.Mov32);
		}

		private void MoveR4(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;

			operand1 = MoveConstantToFloatRegister(context, operand1);

			context.SetInstruction(X86.Movss, result, operand1);
		}

		private void MoveR8(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;

			operand1 = MoveConstantToFloatRegister(context, operand1);

			context.SetInstruction(X86.Movsd, result, operand1);
		}

		private void MulR4(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			operand1 = MoveConstantToFloatRegister(context, operand1);
			operand2 = MoveConstantToFloatRegister(context, operand2);

			context.SetInstruction(X86.Mulss, result, operand1, operand2);
		}

		private void MulR8(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			operand1 = MoveConstantToFloatRegister(context, operand1);
			operand2 = MoveConstantToFloatRegister(context, operand2);

			context.SetInstruction(X86.Mulsd, result, operand1, operand2);
		}

		private void MulSigned32(Context context)
		{
			var v1 = AllocateVirtualRegisterI32();
			context.SetInstruction2(X86.Mul32, v1, context.Result, context.Operand1, context.Operand2);
		}

		private void MulUnsigned32(Context context)
		{
			var v1 = AllocateVirtualRegisterI32();
			context.SetInstruction2(X86.Mul32, v1, context.Result, context.Operand1, context.Operand2);
		}

		private void Nop(Context context)
		{
			context.Empty();

			//context.SetInstruction(X86.Nop);
		}

		private void Not32(Context context)
		{
			context.SetInstruction(X86.Not32, context.Result, context.Operand1);
		}

		private void Or32(Context context)
		{
			context.ReplaceInstruction(X86.Or32);
		}

		private void RemSigned32(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			var v1 = AllocateVirtualRegisterI32();
			var v2 = AllocateVirtualRegisterI32();
			var v3 = AllocateVirtualRegisterI32();

			context.SetInstruction2(X86.Cdq32, v1, v2, operand1);
			context.AppendInstruction2(X86.IDiv32, result, v3, v1, v2, operand2);
		}

		private void RemUnsigned32(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			var v1 = AllocateVirtualRegisterI32();
			var v2 = AllocateVirtualRegisterI32();

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

		private void Store32(Context context)
		{
			LoadStore.OrderOperands(context, MethodCompiler);

			context.SetInstruction(X86.MovStore32, null, context.Operand1, context.Operand2, context.Operand3);
		}

		private void StoreInt16(Context context)
		{
			LoadStore.OrderOperands(context, MethodCompiler);

			context.SetInstruction(X86.MovStore16, null, context.Operand1, context.Operand2, context.Operand3);
		}

		private void StoreInt8(Context context)
		{
			LoadStore.OrderOperands(context, MethodCompiler);

			context.SetInstruction(X86.MovStore8, null, context.Operand1, context.Operand2, context.Operand3);
		}

		private void StoreObject(Context context)
		{
			LoadStore.OrderOperands(context, MethodCompiler);

			context.SetInstruction(X86.MovStore32, null, context.Operand1, context.Operand2, context.Operand3);
		}

		private void StoreParam32(Context context)
		{
			context.SetInstruction(X86.MovStore32, null, StackFrame, context.Operand1, context.Operand2);
		}

		private void StoreParamInt16(Context context)
		{
			context.SetInstruction(X86.MovStore16, null, StackFrame, context.Operand1, context.Operand2);
		}

		private void StoreParamInt8(Context context)
		{
			context.SetInstruction(X86.MovStore8, null, StackFrame, context.Operand1, context.Operand2);
		}

		private void StoreParamObject(Context context)
		{
			context.SetInstruction(X86.MovStore32, null, StackFrame, context.Operand1, context.Operand2);
		}

		private void StoreParamR4(Context context)
		{
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			operand2 = MoveConstantToFloatRegister(context, operand2);

			context.SetInstruction(X86.MovssStore, null, StackFrame, operand1, operand2);
		}

		private void StoreParamR8(Context context)
		{
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			operand2 = MoveConstantToFloatRegister(context, operand2);

			context.SetInstruction(X86.MovsdStore, null, StackFrame, operand1, operand2);
		}

		private void StoreR4(Context context)
		{
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var operand3 = context.Operand3;

			operand3 = MoveConstantToFloatRegister(context, operand3);

			context.SetInstruction(X86.MovssStore, null, operand1, operand2, operand3);
		}

		private void StoreR8(Context context)
		{
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var operand3 = context.Operand3;

			operand3 = MoveConstantToFloatRegister(context, operand3);

			context.SetInstruction(X86.MovsdStore, null, operand1, operand2, operand3);
		}

		private void Sub32(Context context)
		{
			context.ReplaceInstruction(X86.Sub32);
		}

		private void SubCarryIn32(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var operand3 = context.Operand3;

			var v1 = AllocateVirtualRegisterI32();

			context.SetInstruction(X86.Bt32, v1, operand3, ConstantZero32);
			context.AppendInstruction(X86.Sbb32, result, operand1, operand2);
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
			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			operand1 = MoveConstantToFloatRegister(context, operand1);
			operand2 = MoveConstantToFloatRegister(context, operand2);

			context.SetInstruction(X86.Subss, result, operand1, operand2);
		}

		private void SubR8(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			operand1 = MoveConstantToFloatRegister(context, operand1);
			operand2 = MoveConstantToFloatRegister(context, operand2);

			context.SetInstruction(X86.Subsd, result, operand1, operand2);
		}

		private void Switch(Context context)
		{
			var targets = context.BranchTargets;
			var operand = context.Operand1;

			context.Empty();

			for (int i = 0; i < targets.Count - 1; ++i)
			{
				context.AppendInstruction(X86.Cmp32, null, operand, CreateConstant32(i));
				context.AppendInstruction(X86.Branch, ConditionCode.Equal, targets[i]);
			}
		}

		private void Xor32(Context context)
		{
			context.ReplaceInstruction(X86.Xor32);
		}

		private void ZeroExtend16x32(Context context)
		{
			context.ReplaceInstruction(X86.Movzx16To32);
		}

		private void ZeroExtend8x32(Context context)
		{
			context.ReplaceInstruction(X86.Movzx8To32);
		}

		#endregion 32-bit Transformations

		#region 64-bit Transformations

		private void Add64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			SplitLongOperand(context.Operand1, out var op1L, out var op1H);
			SplitLongOperand(context.Operand2, out var op2L, out var op2H);

			context.SetInstruction(X86.Add32, resultLow, op1L, op2L);
			context.AppendInstruction(X86.Adc32, resultHigh, op1H, op2H);
		}

		private void ArithShiftRight64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			SplitLongOperand(context.Operand1, out var op1L, out var op1H);

			var count = context.Operand2;

			var v1 = AllocateVirtualRegisterI32();

			context.SetInstruction(X86.Sar32, v1, op1H, count);
			context.AppendInstruction(X86.Shrd32, resultLow, op1L, op1H, count);
			context.AppendInstruction(X86.Sar32, resultHigh, resultHigh, Constant_1F);

			/// Optimized when shift value is a constant
			if (count.IsResolvedConstant)
			{
				if (count.ConstantUnsigned32 == 32)
				{
					context.AppendInstruction(X86.Mov32, resultHigh, v1);
				}
				else
				{
					context.AppendInstruction(X86.Mov32, resultLow, v1);
				}

				return;
			}

			var v2 = AllocateVirtualRegisterI32();

			context.AppendInstruction(X86.Mov32, v2, count);
			context.AppendInstruction(X86.Test32, null, v2, Constant_32);
			context.AppendInstruction(X86.CMov32, ConditionCode.NotEqual, resultLow, resultLow, v1);
			context.AppendInstruction(X86.CMov32, ConditionCode.Equal, resultHigh, resultHigh, v1);
		}

		private void BitCopyFloatR8To64(Context context)
		{
			var operand1 = context.Operand1;

			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);

			context.SetInstruction(X86.Movdssi32, resultLow, operand1); // FIXME
			context.AppendInstruction(X86.Pextrd32, resultHigh, operand1, Constant_1);
		}

		private void BitCopy64ToFloatR8(Context context)
		{
			var result = context.Result;

			SplitLongOperand(context.Operand1, out var op1L, out var op1H);

			context.SetInstruction(X86.Movdssi32, result, op1L);    // FIXME
			context.AppendInstruction(X86.Pextrd32, result, op1H, Constant_1);
		}

		private void Call(Context context)
		{
			if (context.Result?.IsInteger64 == true)
			{
				SplitLongOperand(context.Result, out _, out _);
			}

			foreach (var operand in context.Operands)
			{
				if (operand.IsInteger64)
				{
					SplitLongOperand(operand, out var op0L, out var op0H);
				}
			}
		}

		private void Compare32x64(Context context)
		{
			// FIXME!
			//Compare32x64(context);
		}

		private void Compare64x32(Context context)
		{
			Compare64x64(context);
		}

		private void Compare64x64(Context context)
		{
			Debug.Assert(context.Operand1 != null && context.Operand2 != null);
			Debug.Assert(context.Result.IsVirtualRegister);

			SplitLongOperand(context.Operand1, out var op1L, out var op1H);
			SplitLongOperand(context.Operand2, out var op2L, out var op2H);

			var result = context.Result;
			var condition = context.ConditionCode;

			var nextBlock = Split(context);
			var newBlocks = CreateNewBlockContexts(4, context.Label);

			// Compare high dwords
			context.SetInstruction(X86.Cmp32, null, op1H, op2H);
			context.AppendInstruction(X86.Branch, ConditionCode.Equal, newBlocks[1].Block);
			context.AppendInstruction(X86.Jmp, newBlocks[0].Block);

			// Branch if check already gave results
			if (condition == ConditionCode.Equal)
			{
				newBlocks[0].AppendInstruction(X86.Jmp, newBlocks[3].Block);
			}
			else
			{
				// Branch if check already gave results
				newBlocks[0].AppendInstruction(X86.Branch, condition, newBlocks[2].Block);
				newBlocks[0].AppendInstruction(X86.Jmp, newBlocks[3].Block);
			}

			// Compare low dwords
			newBlocks[1].AppendInstruction(X86.Cmp32, null, op1L, op2L);
			newBlocks[1].AppendInstruction(X86.Branch, condition.GetUnsigned(), newBlocks[2].Block);
			newBlocks[1].AppendInstruction(X86.Jmp, newBlocks[3].Block);

			// Success
			newBlocks[2].AppendInstruction(X86.Mov32, result, Constant_1);
			newBlocks[2].AppendInstruction(X86.Jmp, nextBlock.Block);

			// Failed
			newBlocks[3].AppendInstruction(X86.Mov32, result, ConstantZero32);
			newBlocks[3].AppendInstruction(X86.Jmp, nextBlock.Block);
		}

		private void Branch64(Context context)
		{
			SplitLongOperand(context.Operand1, out var op1L, out var op1H);
			SplitLongOperand(context.Operand2, out var op2L, out var op2H);

			var target = context.BranchTargets[0];
			var condition = context.ConditionCode;

			var nextBlock = Split(context);
			var newBlocks = CreateNewBlockContexts(2, context.Label);

			// Compare high dwords
			context.SetInstruction(X86.Cmp32, null, op1H, op2H);
			context.AppendInstruction(X86.Branch, ConditionCode.Equal, newBlocks[1].Block);
			context.AppendInstruction(X86.Jmp, newBlocks[0].Block);

			// Branch if check already gave results
			newBlocks[0].AppendInstruction(X86.Branch, condition, target);
			newBlocks[0].AppendInstruction(X86.Jmp, nextBlock.Block);

			// Compare low dwords
			newBlocks[1].AppendInstruction(X86.Cmp32, null, op1L, op2L);
			newBlocks[1].AppendInstruction(X86.Branch, condition.GetUnsigned(), target);
			newBlocks[1].AppendInstruction(X86.Jmp, nextBlock.Block);
		}

		private void ConvertFloatR4To64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);

			context.SetInstruction(X86.Cvttss2si32, resultLow, context.Operand1);
			context.AppendInstruction(X86.Mov32, resultHigh, ConstantZero32);
		}

		private void ConvertFloatR8ToInteger64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);

			context.SetInstruction(X86.Cvttsd2si32, resultLow, context.Operand1);
			context.AppendInstruction(X86.Mov32, resultHigh, ConstantZero32);
		}

		private void Convert64ToFloatR4(Context context)
		{
			SplitLongOperand(context.Result, out var op1Low, out var op1High);

			context.SetInstruction(X86.Cvtsi2ss32, context.Result, op1Low);
		}

		private void Convert64ToFloatR8(Context context)
		{
			SplitLongOperand(context.Result, out var op1Low, out var op1High);

			context.SetInstruction(X86.Cvtsi2sd32, context.Result, op1Low);
		}

		private void ExpandMul(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			SplitLongOperand(context.Operand1, out var op1L, out var op1H);
			SplitLongOperand(context.Operand2, out var op2L, out var op2H);

			var v1 = AllocateVirtualRegisterI32();
			var v2 = AllocateVirtualRegisterI32();
			var v3 = AllocateVirtualRegisterI32();
			var v4 = AllocateVirtualRegisterI32();

			context.SetInstruction(X86.Mov32, v1, op2L);
			context.AppendInstruction2(X86.Mul32, v1, resultLow, op2L, op1L);

			if (!resultHigh.IsConstantZero)
			{
				context.AppendInstruction(X86.Mov32, v2, op1L);
				context.AppendInstruction(X86.IMul32, v3, v2, op2H);
				context.AppendInstruction(X86.Add32, v4, v1, v3);
				context.AppendInstruction(X86.Mov32, v3, op2L);
				context.AppendInstruction(X86.IMul32, v3, v3, op1H);
				context.AppendInstruction(X86.Add32, resultHigh, v4, v3);
			}
		}

		private void GetHigh32(Context context)
		{
			SplitLongOperand(context.Operand1, out var _, out var op0H);

			context.SetInstruction(X86.Mov32, context.Result, op0H);
		}

		private void GetLow32(Context context)
		{
			SplitLongOperand(context.Operand1, out var op0L, out var _);

			context.SetInstruction(X86.Mov32, context.Result, op0L);
		}

		private void IfThenElse64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			SplitLongOperand(context.Operand1, out var op1L, out var op1H);
			SplitLongOperand(context.Operand2, out var op2L, out var op2H);
			SplitLongOperand(context.Operand3, out var op3L, out var op3H);

			var v1 = AllocateVirtualRegisterI32();

			context.SetInstruction(X86.Or32, v1, op1L, op1H);
			context.AppendInstruction(X86.Cmp32, null, v1, ConstantZero32);
			context.AppendInstruction(X86.CMov32, ConditionCode.NotEqual, resultLow, resultLow, op2L);     // true
			context.AppendInstruction(X86.CMov32, ConditionCode.NotEqual, resultHigh, resultHigh, op2H);   // true
			context.AppendInstruction(X86.CMov32, ConditionCode.Equal, resultLow, resultLow, op3L);        // false
			context.AppendInstruction(X86.CMov32, ConditionCode.Equal, resultHigh, resultHigh, op3H);      // false
		}

		private void Load64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);

			var address = context.Operand1;
			var offset = context.Operand2;

			context.SetInstruction(X86.MovLoad32, resultLow, address, offset);

			if (offset.IsResolvedConstant)
			{
				var offset2 = offset.IsConstantZero ? Constant_4 : CreateConstant32(offset.Offset + NativePointerSize);
				context.AppendInstruction(X86.MovLoad32, resultHigh, address, offset2);
				return;
			}

			SplitLongOperand(offset, out var op2L, out _);

			var v1 = AllocateVirtualRegisterI32();

			context.AppendInstruction(X86.Add32, v1, op2L, Constant_4);
			context.AppendInstruction(X86.MovLoad32, resultHigh, address, v1);
		}

		private void LoadParam64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			SplitLongOperand(context.Operand1, out var lowOffset, out var highOffset);

			context.SetInstruction(X86.MovLoad32, resultLow, StackFrame, lowOffset);
			context.AppendInstruction(X86.MovLoad32, resultHigh, StackFrame, highOffset);
		}

		private void LoadParamSignExtend16x64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			SplitLongOperand(context.Operand1, out var lowOffset, out _);

			context.SetInstruction(X86.MovsxLoad16, resultLow, StackFrame, lowOffset);
			context.AppendInstruction2(X86.Cdq32, resultHigh, resultLow, resultLow);
		}

		private void LoadParamSignExtend32x64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			SplitLongOperand(context.Operand1, out var lowOffset, out _);

			context.SetInstruction(X86.MovLoad32, resultLow, StackFrame, lowOffset);
			context.AppendInstruction2(X86.Cdq32, resultHigh, resultLow, resultLow);
		}

		private void LoadParamSignExtend8x64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			SplitLongOperand(context.Operand1, out var lowOffset, out var highOffset);

			context.SetInstruction(X86.MovsxLoad8, resultLow, StackFrame, lowOffset);
			context.AppendInstruction2(X86.Cdq32, resultHigh, resultLow, resultLow);
		}

		private void LoadParamZeroExtended16x64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			SplitLongOperand(context.Operand1, out var lowOffset, out _);

			context.SetInstruction(X86.MovLoad16, resultLow, StackFrame, lowOffset);
			context.AppendInstruction(X86.Mov32, resultHigh, ConstantZero32);
		}

		private void LoadParamZeroExtended32x64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			SplitLongOperand(context.Operand1, out var lowOffset, out _);

			context.SetInstruction(X86.MovLoad32, resultLow, StackFrame, lowOffset);
			context.AppendInstruction(X86.Mov32, resultHigh, ConstantZero32);
		}

		private void LoadParamZeroExtended8x64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			SplitLongOperand(context.Operand1, out var lowOffset, out _);

			context.SetInstruction(X86.MovLoad8, resultLow, StackFrame, lowOffset);
			context.AppendInstruction(X86.Mov32, resultHigh, ConstantZero32);
		}

		private void And64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			SplitLongOperand(context.Operand1, out var op1L, out var op1H);
			SplitLongOperand(context.Operand2, out var op2L, out var op2H);

			context.SetInstruction(X86.And32, resultHigh, op1H, op2H);
			context.AppendInstruction(X86.And32, resultLow, op1L, op2L);
		}

		private void Not64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			SplitLongOperand(context.Operand1, out var op1L, out var op1H);

			context.SetInstruction(X86.Not32, resultHigh, op1H);
			context.AppendInstruction(X86.Not32, resultLow, op1L);
		}

		private void Or64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			SplitLongOperand(context.Operand1, out var op1L, out var op1H);
			SplitLongOperand(context.Operand2, out var op2L, out var op2H);

			context.SetInstruction(X86.Or32, resultHigh, op1H, op2H);
			context.AppendInstruction(X86.Or32, resultLow, op1L, op2L);
		}

		private void Xor64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			SplitLongOperand(context.Operand1, out var op1L, out var op1H);
			SplitLongOperand(context.Operand2, out var op2L, out var op2H);

			context.SetInstruction(X86.Xor32, resultHigh, op1H, op2H);
			context.AppendInstruction(X86.Xor32, resultLow, op1L, op2L);
		}

		private void Move64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			SplitLongOperand(context.Operand1, out var op1L, out var op1H);

			context.SetInstruction(X86.Mov32, resultLow, op1L);
			context.AppendInstruction(X86.Mov32, resultHigh, op1H);
		}

		private void MulSigned64(Context context)
		{
			ExpandMul(context);
		}

		private void MulUnsigned64(Context context)
		{
			ExpandMul(context);
		}

		private void ShiftLeft64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			SplitLongOperand(context.Operand1, out var op1L, out var op1H);

			var count = context.Operand2;

			var v1 = AllocateVirtualRegisterI32();
			var v2 = AllocateVirtualRegisterI32();

			context.SetInstruction(X86.Shld32, resultHigh, op1H, op1L, count);
			context.AppendInstruction(X86.Shl32, v1, op1L, count);

			if (count.IsConstant)
			{
				// FUTURE: Optimization - Test32 and conditional moves are not necessary if the count is a resolved constant

				context.AppendInstruction(X86.Mov32, v2, count);
				context.AppendInstruction(X86.Test32, null, v2, Constant_32);
				context.AppendInstruction(X86.CMov32, ConditionCode.NotEqual, resultHigh, resultHigh, v1);
				context.AppendInstruction(X86.Mov32, resultLow, ConstantZero32);
				context.AppendInstruction(X86.CMov32, ConditionCode.Equal, resultLow, resultLow, v1);
			}
			else
			{
				context.AppendInstruction(X86.Test32, null, count, Constant_32);
				context.AppendInstruction(X86.CMov32, ConditionCode.NotEqual, resultHigh, resultHigh, v1);
				context.AppendInstruction(X86.Mov32, resultLow, ConstantZero32);
				context.AppendInstruction(X86.CMov32, ConditionCode.Equal, resultLow, resultLow, v1);
			}
		}

		private void ShiftRight64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			SplitLongOperand(context.Operand1, out var op1L, out var op1H);

			var count = context.Operand2;

			/// Optimized shift when shift value is a constant and 32 or more, or zero
			if (count.IsResolvedConstant)
			{
				var shift = count.ConstantUnsigned32 & 0b111111;

				if (shift == 0)
				{
					// shift is exactly 0 bits (nop)
					context.SetInstruction(X86.Mov32, resultLow, op1L);
					context.AppendInstruction(X86.Mov32, resultHigh, op1H);
					return;
				}
				else if (shift == 32)
				{
					// shift is exactly 32 bits
					context.SetInstruction(X86.Mov32, resultLow, op1H);
					context.AppendInstruction(X86.Mov32, resultHigh, ConstantZero32);
					return;
				}
				else if (shift > 32)
				{
					// shift is greater than 32 bits
					var newshift = CreateConstant32(shift - 32);
					context.SetInstruction(X86.Shr32, resultHigh, op1H, newshift);
					context.AppendInstruction(X86.Mov32, resultHigh, ConstantZero32);
					return;
				}
			}

			var newBlocks = CreateNewBlockContexts(1, context.Label);
			var nextBlock = Split(context);

			var ECX = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.ECX);

			context.SetInstruction(X86.Mov32, ECX, count);
			context.AppendInstruction(X86.Shrd32, resultLow, op1L, op1H, ECX);
			context.AppendInstruction(X86.Shr32, resultHigh, op1H, ECX);

			context.AppendInstruction(X86.Test32, null, ECX, Constant_32);
			context.AppendInstruction(X86.Branch, ConditionCode.Zero, nextBlock.Block);
			context.AppendInstruction(X86.Jmp, newBlocks[0].Block);

			newBlocks[0].AppendInstruction(X86.Mov32, resultLow, resultHigh);
			newBlocks[0].AppendInstruction(X86.Mov32, resultHigh, ConstantZero32);
			newBlocks[0].AppendInstruction(X86.Jmp, nextBlock.Block);
		}

		private void SignExtend16x64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);

			var v1 = AllocateVirtualRegisterI32();

			context.SetInstruction(X86.Movsx16To32, v1, context.Operand1);
			context.AppendInstruction2(X86.Cdq32, resultHigh, resultLow, v1);
		}

		private void SignExtend32x64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);

			var v1 = AllocateVirtualRegisterI32();

			context.SetInstruction(X86.Mov32, v1, context.Operand1);
			context.AppendInstruction2(X86.Cdq32, resultHigh, resultLow, v1);
		}

		private void SignExtend8x64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);

			var v1 = AllocateVirtualRegisterI32();

			context.SetInstruction(X86.Movsx8To32, v1, context.Operand1);
			context.AppendInstruction2(X86.Cdq32, resultHigh, resultLow, v1);
		}

		private void Store64(Context context)
		{
			SplitLongOperand(context.Operand2, out var op2L, out _);
			SplitLongOperand(context.Operand3, out var op3L, out var op3H);

			var address = context.Operand1;
			var offset = context.Operand2;

			context.SetInstruction(X86.MovStore32, null, address, op2L, op3L);

			if (offset.IsResolvedConstant)
			{
				context.AppendInstruction(X86.MovStore32, null, address, CreateConstant32(offset.Offset + NativePointerSize), op3H);
			}
			else
			{
				var offset4 = AllocateVirtualRegisterI32();

				context.AppendInstruction(X86.Add32, offset4, op2L, Constant_4);
				context.AppendInstruction(X86.MovStore32, null, address, offset4, op3H);
			}
		}

		private void StoreParam64(Context context)
		{
			SplitLongOperand(context.Operand1, out var op0L, out var op0H);
			SplitLongOperand(context.Operand2, out var op1L, out var op1H);

			context.SetInstruction(X86.MovStore32, null, StackFrame, op0L, op1L);
			context.AppendInstruction(X86.MovStore32, null, StackFrame, op0H, op1H);
		}

		private void Sub64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			SplitLongOperand(context.Operand1, out var op1L, out var op1H);
			SplitLongOperand(context.Operand2, out var op2L, out var op2H);

			context.SetInstruction(X86.Sub32, resultLow, op1L, op2L);
			context.AppendInstruction(X86.Sbb32, resultHigh, op1H, op2H);
		}

		private void To64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);

			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			context.SetInstruction(X86.Mov32, resultLow, operand1);
			context.AppendInstruction(X86.Mov32, resultHigh, operand2);
		}

		private void Truncate64x32(Context context)
		{
			Debug.Assert(context.Operand1.IsInteger64);
			Debug.Assert(!context.Result.IsInteger64);
			SplitLongOperand(context.Operand1, out var op1L, out _);

			context.SetInstruction(X86.Mov32, context.Result, op1L);
		}

		private void ZeroExtended16x64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			SplitLongOperand(context.Operand1, out var op1L, out _);

			context.SetInstruction(X86.Movzx16To32, resultLow, op1L);
			context.AppendInstruction(X86.Mov32, resultHigh, ConstantZero32);
		}

		private void ZeroExtended32x64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			SplitLongOperand(context.Operand1, out var op1L, out _);

			context.SetInstruction(X86.Mov32, resultLow, op1L);
			context.AppendInstruction(X86.Mov32, resultHigh, ConstantZero32);
		}

		private void ZeroExtended8x64(Context context)
		{
			SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			SplitLongOperand(context.Operand1, out var op1L, out _);

			context.SetInstruction(X86.Movzx8To32, resultLow, op1L);
			context.AppendInstruction(X86.Mov32, resultHigh, ConstantZero32);
		}

		#endregion 64-bit Transformations

		#region Helper Methods

		public static void MoveConstantRight(Context context)
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
			var condition = context.ConditionCode;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			operand1 = MoveConstantToFloatRegister(context, operand1);
			operand2 = MoveConstantToFloatRegister(context, operand2);

			var v1 = AllocateVirtualRegisterI32();

			if (condition == ConditionCode.Equal)
			{
				context.SetInstruction(instruction, null, operand1, operand2);
				context.AppendInstruction(X86.Setcc, ConditionCode.NoParity, result);
				context.AppendInstruction(X86.Mov32, v1, ConstantZero32);
				context.AppendInstruction(X86.CMov32, ConditionCode.NotEqual, result, result, v1);
				context.AppendInstruction(X86.Movzx8To32, result, result);
				return;
			}
			else if (condition == ConditionCode.NotEqual)
			{
				context.SetInstruction(instruction, null, operand1, operand2);
				context.AppendInstruction(X86.Setcc, ConditionCode.Parity, result);
				context.AppendInstruction(X86.Mov32, v1, Constant_1);
				context.AppendInstruction(X86.CMov32, ConditionCode.NotEqual, result, result, v1);
				context.AppendInstruction(X86.Movzx8To32, result, result);
				return;
			}
			else if (condition == ConditionCode.Greater || condition == ConditionCode.UnsignedGreater)
			{
				context.SetInstruction(instruction, null, operand1, operand2);
				context.AppendInstruction(X86.Setcc, ConditionCode.UnsignedGreater, v1);
				context.AppendInstruction(X86.Movzx8To32, result, v1);
				return;
			}
			else if (condition == ConditionCode.Less || condition == ConditionCode.UnsignedLess)
			{
				context.SetInstruction(instruction, null, operand2, operand1);
				context.AppendInstruction(X86.Setcc, ConditionCode.UnsignedGreater, v1);
				context.AppendInstruction(X86.Movzx8To32, result, v1);
				return;
			}
			else if (condition == ConditionCode.GreaterOrEqual || condition == ConditionCode.UnsignedGreaterOrEqual)
			{
				context.SetInstruction(instruction, null, operand2, operand1);
				context.AppendInstruction(X86.Setcc, ConditionCode.NoCarry, v1);
				context.AppendInstruction(X86.Movzx8To32, result, v1);
				return;
			}
			else if (condition == ConditionCode.LessOrEqual || condition == ConditionCode.UnsignedLessOrEqual)
			{
				context.SetInstruction(instruction, null, operand2, operand1);
				context.AppendInstruction(X86.Setcc, ConditionCode.NoCarry, v1);
				context.AppendInstruction(X86.Movzx8To32, result, v1);
				return;
			}
		}

		private Operand MoveConstantToFloatRegister(Context context, Operand operand)
		{
			if (!operand.IsConstant)
				return operand;

			var v1 = operand.IsR4 ? AllocateVirtualRegisterR4() : AllocateVirtualRegisterR8();

			var symbol = operand.IsR4 ? Linker.GetConstantSymbol(operand.ConstantFloat) : Linker.GetConstantSymbol(operand.ConstantDouble);

			var label = Operand.CreateLabel(v1.Type, symbol.Name);

			var instruction = operand.IsR4 ? (BaseInstruction)X86.MovssLoad : X86.MovsdLoad;

			context.InsertBefore().SetInstruction(instruction, v1, label, ConstantZero);

			return v1;
		}

		#endregion Helper Methods

		#region Utility Methods

		public void SplitLongOperand(Operand operand, out Operand operandLow, out Operand operandHigh)
		{
			MethodCompiler.SplitLongOperand(operand, out operandLow, out operandHigh);
		}

		#endregion Utility Methods
	}
}
