// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.MosaTypeSystem;
using System.Diagnostics;

namespace Mosa.Platform.x64.Stages
{
	/// <summary>
	/// Transforms IR instructions into their appropriate X64.
	/// </summary>
	/// <remarks>
	/// This transformation stage transforms IR instructions into their equivalent X86 sequences.
	/// </remarks>
	/// <seealso cref="Mosa.Platform.x64.BaseTransformationStage" />
	public sealed class IRTransformationStage : BaseTransformationStage
	{
		protected override void PopulateVisitationDictionary()
		{
			AddVisitation(IRInstruction.AddR4, AddR4);
			AddVisitation(IRInstruction.AddR8, AddR8);
			AddVisitation(IRInstruction.AddressOf, AddressOf);
			AddVisitation(IRInstruction.Add32, Add32);
			AddVisitation(IRInstruction.AddCarryOut32, AddCarryOut32);
			AddVisitation(IRInstruction.AddCarryIn32, AddCarryIn32);

			//AddVisitation(IRInstruction.BitCopyR4To32, BitCopyR4To64);
			//AddVisitation(IRInstruction.BitCopy32ToR4, BitCopy64ToR4);
			AddVisitation(IRInstruction.ArithShiftRight32, ArithShiftRight32);
			AddVisitation(IRInstruction.CallDirect, CallDirect);
			AddVisitation(IRInstruction.CompareR4, CompareR4);
			AddVisitation(IRInstruction.CompareR8, CompareR8);
			AddVisitation(IRInstruction.Compare32x32, Compare32x32);
			AddVisitation(IRInstruction.Branch32, Branch32);
			AddVisitation(IRInstruction.IfThenElse32, IfThenElse32);
			AddVisitation(IRInstruction.ConvertR4ToR8, ConvertR4ToR8);
			AddVisitation(IRInstruction.ConvertR8ToR4, ConvertR8ToR4);
			AddVisitation(IRInstruction.ConvertR4ToI32, ConvertR4ToI32);
			AddVisitation(IRInstruction.ConvertR8ToI32, ConvertR8ToI32);
			AddVisitation(IRInstruction.ConvertI32ToR4, ConvertI32ToR4);
			AddVisitation(IRInstruction.ConvertI32ToR8, ConvertI32ToR8);

			//AddVisitation(IRInstruction.ConvertR4ToU32, ConvertR4ToU32);
			//AddVisitation(IRInstruction.ConvertR8ToU32, ConvertR8ToU32);
			//AddVisitation(IRInstruction.ConvertU32ToR4, ConvertU32ToR4);
			//AddVisitation(IRInstruction.ConvertU32ToR8, ConvertU32ToR8);

			AddVisitation(IRInstruction.DivR4, DivR4);
			AddVisitation(IRInstruction.DivR8, DivR8);
			AddVisitation(IRInstruction.DivSigned32, DivSigned32);
			AddVisitation(IRInstruction.DivUnsigned32, DivUnsigned32);
			AddVisitation(IRInstruction.Jmp, Jmp);
			AddVisitation(IRInstruction.LoadR4, LoadR4);
			AddVisitation(IRInstruction.LoadR8, LoadR8);
			AddVisitation(IRInstruction.Load32, Load32);
			AddVisitation(IRInstruction.LoadSignExtend8x32, LoadSignExtend8x32);
			AddVisitation(IRInstruction.LoadSignExtend16x32, LoadSignExtend16x32);
			AddVisitation(IRInstruction.LoadZeroExtend8x32, LoadZeroExtend8x32);
			AddVisitation(IRInstruction.LoadZeroExtend16x32, LoadZeroExtend16x32);
			AddVisitation(IRInstruction.LoadParamR4, LoadParamR4);
			AddVisitation(IRInstruction.LoadParamR8, LoadParamR8);
			AddVisitation(IRInstruction.LoadParam32, LoadParam32);
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
			AddVisitation(IRInstruction.Move32, Move32);
			AddVisitation(IRInstruction.SignExtend8x32, SignExtend8x32);
			AddVisitation(IRInstruction.SignExtend16x32, SignExtend16x32);
			AddVisitation(IRInstruction.ZeroExtend8x32, ZeroExtend8x32);
			AddVisitation(IRInstruction.ZeroExtend16x32, ZeroExtend16x32);
			AddVisitation(IRInstruction.MulR4, MulR4);
			AddVisitation(IRInstruction.MulR8, MulR8);
			AddVisitation(IRInstruction.MulSigned32, MulSigned32);
			AddVisitation(IRInstruction.MulUnsigned32, MulUnsigned32);
			AddVisitation(IRInstruction.Nop, Nop);
			AddVisitation(IRInstruction.RemSigned32, RemSigned32);
			AddVisitation(IRInstruction.RemUnsigned32, RemUnsigned32);
			AddVisitation(IRInstruction.ShiftLeft32, ShiftLeft32);
			AddVisitation(IRInstruction.ShiftRight32, ShiftRight32);
			AddVisitation(IRInstruction.StoreR4, StoreR4);
			AddVisitation(IRInstruction.StoreR8, StoreR8);
			AddVisitation(IRInstruction.Store8, StoreInt8);
			AddVisitation(IRInstruction.Store16, StoreInt16);
			AddVisitation(IRInstruction.Store32, Store32);
			AddVisitation(IRInstruction.StoreParamR4, StoreParamR4);
			AddVisitation(IRInstruction.StoreParamR8, StoreParamR8);
			AddVisitation(IRInstruction.StoreParam8, StoreParamInt8);
			AddVisitation(IRInstruction.StoreParam16, StoreParamInt16);
			AddVisitation(IRInstruction.StoreParam32, StoreParam32);
			AddVisitation(IRInstruction.SubR4, SubR4);
			AddVisitation(IRInstruction.SubR8, SubR8);
			AddVisitation(IRInstruction.Sub32, Sub32);
			AddVisitation(IRInstruction.SubCarryOut32, SubCarryOut32);
			AddVisitation(IRInstruction.SubCarryIn32, SubCarryIn32);
			AddVisitation(IRInstruction.Switch, Switch);

			// 64-bit Transforms

			AddVisitation(IRInstruction.Add64, Add64);
			AddVisitation(IRInstruction.ArithShiftRight64, ArithShiftRight64);
			AddVisitation(IRInstruction.Compare32x64, Compare32x64);
			AddVisitation(IRInstruction.Compare64x32, Compare64x32);
			AddVisitation(IRInstruction.Compare64x64, Compare64x64);
			AddVisitation(IRInstruction.Branch64, Branch64);
			AddVisitation(IRInstruction.ConvertR4ToI64, ConvertR4ToI64);
			AddVisitation(IRInstruction.ConvertR8ToI64, ConvertR8ToI64);
			AddVisitation(IRInstruction.ConvertI64ToR4, ConvertI64ToR4);
			AddVisitation(IRInstruction.ConvertI64ToR8, ConvertI64ToR8);

			//AddVisitation(IRInstruction.ConvertR4ToI64, ConvertR4ToI64);
			//AddVisitation(IRInstruction.ConvertR8ToI64, ConvertR8ToI64);
			//AddVisitation(IRInstruction.ConvertU64ToR4, ConvertU64ToR4);
			//AddVisitation(IRInstruction.ConvertU64ToR8, ConvertU64ToR8);

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
			AddVisitation(IRInstruction.Store64, Store64);
			AddVisitation(IRInstruction.StoreParam64, StoreParam64);
			AddVisitation(IRInstruction.Sub64, Sub64);
			AddVisitation(IRInstruction.Truncate64x32, Truncate64x32);
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

		#region 32-bit Transformations

		private void Add32(Context context)
		{
			context.ReplaceInstruction(X64.Add32);
		}

		private void AddCarryOut32(Context context)
		{
			var result = context.Result;
			var result2 = context.Result2;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.Boolean);

			context.SetInstruction(X64.Add32, result, operand1, operand2);
			context.AppendInstruction(X64.Setcc, ConditionCode.Carry, v1);
			context.AppendInstruction(X64.Movzx8To32, result2, v1);
		}

		private void AddR4(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			operand1 = MoveConstantToFloatRegister(context, operand1);
			operand2 = MoveConstantToFloatRegister(context, operand2);

			context.SetInstruction(X64.Addss, result, operand1, operand2);
		}

		private void AddR8(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			operand1 = MoveConstantToFloatRegister(context, operand1);
			operand2 = MoveConstantToFloatRegister(context, operand2);

			context.SetInstruction(X64.Addsd, result, operand1, operand2);
		}

		private void AddressOf(Context context)
		{
			Debug.Assert(context.Operand1.IsOnStack || context.Operand1.IsStaticField);

			if (context.Operand1.IsStaticField)
			{
				context.SetInstruction(X64.Mov64, context.Result, context.Operand1);
			}
			else if (context.Operand1.IsStackLocal)
			{
				context.SetInstruction(X64.Lea64, context.Result, StackFrame, context.Operand1);
			}
			else
			{
				var offset = CreateConstant32(context.Operand1.Offset);

				context.SetInstruction(X64.Lea64, context.Result, StackFrame, offset);
			}
		}

		private void AddCarryIn32(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var operand3 = context.Operand3;

			var v1 = AllocateVirtualRegisterI32();

			context.SetInstruction(X64.Bt32, v1, operand3, ConstantZero64);
			context.AppendInstruction(X64.Adc32, result, operand1, operand2);
		}

		private void BitCopyR8To64(Context context)
		{
			context.ReplaceInstruction(X64.Movdsdi64);
		}

		private void BitCopy64ToR8(Context context)
		{
			context.ReplaceInstruction(X64.Movdi64sd);
		}

		private void ArithShiftRight32(Context context)
		{
			context.ReplaceInstruction(X64.Sar32);
		}

		private void Break(Context context)
		{
			context.SetInstruction(X64.Break);
		}

		private void CallDirect(Context context)
		{
			context.ReplaceInstruction(X64.Call);
		}

		private void CompareR4(Context context)
		{
			FloatCompare(context, X64.Ucomiss);
		}

		private void CompareR8(Context context)
		{
			FloatCompare(context, X64.Ucomisd);
		}

		private void Compare32x32(Context context)
		{
			var condition = context.ConditionCode;
			var resultOperand = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			var v1 = AllocateVirtualRegisterI32();
			context.SetInstruction(X64.Cmp32, null, operand1, operand2);
			context.AppendInstruction(X64.Setcc, condition, v1);
			context.AppendInstruction(X64.Movzx8To32, resultOperand, v1);
		}

		private void Branch32(Context context)
		{
			MoveConstantRight(context);

			var target = context.BranchTargets[0];
			var condition = context.ConditionCode;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			context.SetInstruction(X64.Cmp32, null, operand1, operand2);
			context.AppendInstruction(X64.Branch, condition, target);
		}

		private void ConvertR4ToR8(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;

			operand1 = MoveConstantToFloatRegister(context, operand1);

			context.SetInstruction(X64.Cvtss2sd, result, operand1);
		}

		private void ConvertR4ToI32(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;

			operand1 = MoveConstantToFloatRegister(context, operand1);

			context.SetInstruction(X64.Cvttss2si32, result, operand1);
		}

		private void ConvertR8ToR4(Context context)
		{
			Debug.Assert(context.Result.IsInteger && !context.Result.IsFloatingPoint);

			var result = context.Result;
			var operand1 = context.Operand1;

			operand1 = MoveConstantToFloatRegister(context, operand1);

			context.SetInstruction(X64.Cvtsd2ss, result, operand1); // FIXME!
		}

		private void ConvertR8ToI32(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;

			operand1 = MoveConstantToFloatRegister(context, operand1);

			context.SetInstruction(X64.Cvttsd2si32, result, operand1);
		}

		private void ConvertI32ToR4(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;

			operand1 = MoveConstantToFloatRegister(context, operand1);

			context.SetInstruction(X64.Cvtsd2ss, result, operand1);
		}

		private void ConvertI32ToR8(Context context)
		{
			Debug.Assert(context.Result.IsR8);
			context.ReplaceInstruction(X64.Cvtsi2sd32);
		}

		private void DivR4(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			operand1 = MoveConstantToFloatRegister(context, operand1);
			operand2 = MoveConstantToFloatRegister(context, operand2);

			context.SetInstruction(X64.Divss, result, operand1, operand2);
		}

		private void DivR8(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			operand1 = MoveConstantToFloatRegister(context, operand1);
			operand2 = MoveConstantToFloatRegister(context, operand2);

			context.SetInstruction(X64.Divsd, result, operand1, operand2);
		}

		private void DivSigned32(Context context)
		{
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var result = context.Result;

			var v1 = AllocateVirtualRegisterI32();
			var v2 = AllocateVirtualRegisterI32();
			var v3 = AllocateVirtualRegisterI32();

			context.SetInstruction2(X64.Cdq32, v1, v2, operand1);
			context.AppendInstruction2(X64.IDiv32, v3, result, v1, v2, operand2);
		}

		private void DivUnsigned32(Context context)
		{
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var result = context.Result;

			var v1 = AllocateVirtualRegisterI32();
			var v2 = AllocateVirtualRegisterI32();

			context.SetInstruction(X64.Mov32, v1, ConstantZero32);
			context.AppendInstruction2(X64.Div32, v1, v2, v1, operand1, operand2);
			context.AppendInstruction(X64.Mov32, result, v2);
		}

		private void IfThenElse32(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var operand3 = context.Operand3;

			context.SetInstruction(X64.Cmp32, null, operand1, ConstantZero32);
			context.AppendInstruction(X64.CMov32, ConditionCode.NotEqual, result, result, operand2);    // true
			context.AppendInstruction(X64.CMov32, ConditionCode.Equal, result, result, operand3);       // false
		}

		private void Jmp(Context context)
		{
			context.ReplaceInstruction(X64.Jmp);
		}

		private void LoadR4(Context context)
		{
			Debug.Assert(context.Result.IsR4);

			context.SetInstruction(X64.MovssLoad, context.Result, context.Operand1, context.Operand2);
		}

		private void LoadR8(Context context)
		{
			Debug.Assert(context.Result.IsR8);

			context.SetInstruction(X64.MovsdLoad, context.Result, context.Operand1, context.Operand2);
		}

		private void Load32(Context context)
		{
			Debug.Assert(!context.Result.IsR4);
			Debug.Assert(!context.Result.IsR8);

			LoadStore.OrderOperands(context, MethodCompiler);

			context.SetInstruction(X64.MovLoad32, context.Result, context.Operand1, context.Operand2);
		}

		private void LoadParamR4(Context context)
		{
			Debug.Assert(context.Result.IsR4);

			context.SetInstruction(X64.MovssLoad, context.Result, StackFrame, context.Operand1);
		}

		private void LoadParamR8(Context context)
		{
			Debug.Assert(context.Result.IsR8);

			context.SetInstruction(X64.MovsdLoad, context.Result, StackFrame, context.Operand1);
		}

		private void LoadParam32(Context context)
		{
			context.SetInstruction(X64.MovLoad32, context.Result, StackFrame, context.Operand1);
		}

		private void LoadParamSignExtend16x32(Context context)
		{
			context.SetInstruction(X64.MovsxLoad16, context.Result, StackFrame, context.Operand1);
		}

		private void LoadParamSignExtend8x32(Context context)
		{
			context.SetInstruction(X64.MovsxLoad8, context.Result, StackFrame, context.Operand1);
		}

		private void LoadParamZeroExtend16x32(Context context)
		{
			context.SetInstruction(X64.MovzxLoad16, context.Result, StackFrame, context.Operand1);
		}

		private void LoadParamZeroExtend8x32(Context context)
		{
			context.SetInstruction(X64.MovzxLoad8, context.Result, StackFrame, context.Operand1);
		}

		private void LoadSignExtend16x32(Context context)
		{
			LoadStore.OrderOperands(context, MethodCompiler);

			context.SetInstruction(X64.MovsxLoad16, context.Result, context.Operand1, context.Operand2);
		}

		private void LoadSignExtend8x32(Context context)
		{
			LoadStore.OrderOperands(context, MethodCompiler);

			context.SetInstruction(X64.MovsxLoad8, context.Result, context.Operand1, context.Operand2);
		}

		private void LoadZeroExtend16x32(Context context)
		{
			LoadStore.OrderOperands(context, MethodCompiler);

			context.SetInstruction(X64.MovzxLoad16, context.Result, context.Operand1, context.Operand2);
		}

		private void LoadZeroExtend8x32(Context context)
		{
			LoadStore.OrderOperands(context, MethodCompiler);

			context.SetInstruction(X64.MovzxLoad8, context.Result, context.Operand1, context.Operand2);
		}

		private void And32(Context context)
		{
			context.ReplaceInstruction(X64.And32);
		}

		private void Not32(Context context)
		{
			context.SetInstruction(X64.Not32, context.Result, context.Operand1);
		}

		private void Or32(Context context)
		{
			context.ReplaceInstruction(X64.Or32);
		}

		private void Xor32(Context context)
		{
			context.ReplaceInstruction(X64.Xor32);
		}

		private void MoveR4(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;

			operand1 = MoveConstantToFloatRegister(context, operand1);

			context.SetInstruction(X64.Movss, result, operand1);
		}

		private void MoveR8(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;

			operand1 = MoveConstantToFloatRegister(context, operand1);

			context.SetInstruction(X64.Movsd, result, operand1);
		}

		private void Move32(Context context)
		{
			context.ReplaceInstruction(X64.Mov32);
		}

		private void MulR4(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			operand1 = MoveConstantToFloatRegister(context, operand1);
			operand2 = MoveConstantToFloatRegister(context, operand2);

			context.SetInstruction(X64.Mulss, result, operand1, operand2);
		}

		private void MulR8(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			operand1 = MoveConstantToFloatRegister(context, operand1);
			operand2 = MoveConstantToFloatRegister(context, operand2);

			context.SetInstruction(X64.Mulsd, result, operand1, operand2);
		}

		private void MulSigned32(Context context)
		{
			var v1 = AllocateVirtualRegisterI32();
			context.SetInstruction2(X64.Mul32, v1, context.Result, context.Operand1, context.Operand2);
		}

		private void MulUnsigned32(Context context)
		{
			var v1 = AllocateVirtualRegisterI32();
			context.SetInstruction2(X64.Mul32, v1, context.Result, context.Operand1, context.Operand2);
		}

		private void Nop(Context context)
		{
			context.SetInstruction(X64.Nop);
		}

		private void RemSigned32(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			var v1 = AllocateVirtualRegisterI32();
			var v2 = AllocateVirtualRegisterI32();
			var v3 = AllocateVirtualRegisterI32();

			context.SetInstruction2(X64.Cdq32, v1, v2, operand1);
			context.AppendInstruction2(X64.IDiv32, result, v3, v1, v2, operand2);
		}

		private void RemUnsigned32(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			var v1 = AllocateVirtualRegisterI32();
			var v2 = AllocateVirtualRegisterI32();

			context.SetInstruction(X64.Mov32, v1, ConstantZero32);
			context.AppendInstruction2(X64.Div32, result, v2, v1, operand1, operand2);
		}

		private void ShiftLeft32(Context context)
		{
			context.ReplaceInstruction(X64.Shl32);
		}

		private void ShiftRight32(Context context)
		{
			context.ReplaceInstruction(X64.Shr32);
		}

		private void SignExtend16x32(Context context)
		{
			context.ReplaceInstruction(X64.Movsx16To32);
		}

		private void SignExtend8x32(Context context)
		{
			context.ReplaceInstruction(X64.Movsx8To32);
		}

		private void StoreR4(Context context)
		{
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var operand3 = context.Operand3;

			operand3 = MoveConstantToFloatRegister(context, operand3);

			context.SetInstruction(X64.MovssStore, null, operand1, operand2, operand3);
		}

		private void StoreR8(Context context)
		{
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var operand3 = context.Operand3;

			operand3 = MoveConstantToFloatRegister(context, operand3);

			context.SetInstruction(X64.MovsdStore, null, operand1, operand2, operand3);
		}

		private void StoreInt16(Context context)
		{
			LoadStore.OrderOperands(context, MethodCompiler);

			context.SetInstruction(X64.MovStore16, null, context.Operand1, context.Operand2, context.Operand3);
		}

		private void Store32(Context context)
		{
			LoadStore.OrderOperands(context, MethodCompiler);

			context.SetInstruction(X64.MovStore32, null, context.Operand1, context.Operand2, context.Operand3);
		}

		private void StoreInt8(Context context)
		{
			LoadStore.OrderOperands(context, MethodCompiler);

			context.SetInstruction(X64.MovStore8, null, context.Operand1, context.Operand2, context.Operand3);
		}

		private void StoreParamR4(Context context)
		{
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			operand2 = MoveConstantToFloatRegister(context, operand2);

			context.SetInstruction(X64.MovssStore, null, StackFrame, operand1, operand2);
		}

		private void StoreParamR8(Context context)
		{
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			operand2 = MoveConstantToFloatRegister(context, operand2);

			context.SetInstruction(X64.MovsdStore, null, StackFrame, operand1, operand2);
		}

		private void StoreParamInt16(Context context)
		{
			context.SetInstruction(X64.MovStore16, null, StackFrame, context.Operand1, context.Operand2);
		}

		private void StoreParam32(Context context)
		{
			context.SetInstruction(X64.MovStore32, null, StackFrame, context.Operand1, context.Operand2);
		}

		private void StoreParamInt8(Context context)
		{
			context.SetInstruction(X64.MovStore8, null, StackFrame, context.Operand1, context.Operand2);
		}

		private void Sub32(Context context)
		{
			context.ReplaceInstruction(X64.Sub32);
		}

		private void SubCarryOut32(Context context)
		{
			var result = context.Result;
			var result2 = context.Result2;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.Boolean);

			context.SetInstruction(X64.Sub32, result, operand1, operand2);
			context.AppendInstruction(X64.Setcc, ConditionCode.Carry, v1);
			context.AppendInstruction(X64.Movzx8To32, result2, v1);
		}

		private void SubR4(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			operand1 = MoveConstantToFloatRegister(context, operand1);
			operand2 = MoveConstantToFloatRegister(context, operand2);

			context.SetInstruction(X64.Subss, result, operand1, operand2);
		}

		private void SubR8(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			operand1 = MoveConstantToFloatRegister(context, operand1);
			operand2 = MoveConstantToFloatRegister(context, operand2);

			context.SetInstruction(X64.Subsd, result, operand1, operand2);
		}

		private void SubCarryIn32(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var operand3 = context.Operand3;

			var v1 = AllocateVirtualRegisterI32();

			context.SetInstruction(X64.Bt32, v1, operand3, ConstantZero64);
			context.AppendInstruction(X64.Sbb32, result, operand1, operand2);
		}

		private void Switch(Context context)
		{
			var targets = context.BranchTargets;
			var operand = context.Operand1;

			context.Empty();

			for (int i = 0; i < targets.Count - 1; ++i)
			{
				context.AppendInstruction(X64.Cmp32, null, operand, CreateConstant32(i));
				context.AppendInstruction(X64.Branch, ConditionCode.Equal, targets[i]);
			}
		}

		private void ZeroExtend16x32(Context context)
		{
			context.ReplaceInstruction(X64.Movzx16To32);
		}

		private void ZeroExtend8x32(Context context)
		{
			context.ReplaceInstruction(X64.Movzx8To32);
		}

		#endregion 32-bit Transformations

		#region 64-bit Transformations

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

			var v1 = AllocateVirtualRegisterI32();
			context.SetInstruction(X64.Cmp64, null, operand1, operand2);
			context.AppendInstruction(X64.Setcc, condition, v1);
			context.AppendInstruction(X64.Movzx8To64, resultOperand, v1);
		}

		private void Branch64(Context context)
		{
			MoveConstantRight(context);

			var target = context.BranchTargets[0];
			var condition = context.ConditionCode;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			context.SetInstruction(X64.Cmp64, null, operand1, operand2);
			context.AppendInstruction(X64.Branch, condition, target);
		}

		private void ConvertR4ToI64(Context context)
		{
			context.ReplaceInstruction(X64.Cvtss2sd);
		}

		private void ConvertR8ToI64(Context context)
		{
			Debug.Assert(context.Result.IsInteger && !context.Result.IsFloatingPoint);
			context.ReplaceInstruction(X64.Cvttss2si64);
		}

		private void ConvertI64ToR4(Context context)
		{
			context.SetInstruction(X64.Cvtsi2ss64, context.Result, context.Operand1);
		}

		private void ConvertI64ToR8(Context context)
		{
			context.SetInstruction(X64.Cvtsi2sd64, context.Result, context.Operand1);
		}

		private void DivSigned64(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			var v1 = AllocateVirtualRegisterI32();
			var v2 = AllocateVirtualRegisterI32();
			var v3 = AllocateVirtualRegisterI32();

			context.SetInstruction2(X64.Cdq64, v1, v2, operand1);
			context.AppendInstruction2(X64.IDiv64, result, v3, v1, v2, operand2);
		}

		private void DivUnsigned64(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			var v1 = AllocateVirtualRegisterI64();
			var v2 = AllocateVirtualRegisterI64();

			context.SetInstruction(X64.Mov64, v1, ConstantZero64);
			context.AppendInstruction2(X64.Div64, result, v2, v1, operand1, operand2);
		}

		private void IfThenElse64(Context context)
		{
			var result = context.Operand1;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			context.SetInstruction(X64.Cmp64, null, operand1, ConstantZero64);
			context.AppendInstruction(X64.CMov64, ConditionCode.NotEqual, result, result, operand1);    // true
			context.AppendInstruction(X64.CMov64, ConditionCode.Equal, result, result, operand2);       // false
		}

		private void Load64(Context context)
		{
			Debug.Assert(!context.Result.IsR4);
			Debug.Assert(!context.Result.IsR8);

			LoadStore.OrderOperands(context, MethodCompiler);

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
			LoadStore.OrderOperands(context, MethodCompiler);

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

		private void And64(Context context)
		{
			context.ReplaceInstruction(X64.And64);
		}

		private void Not64(Context context)
		{
			context.SetInstruction(X64.Mov64, context.Result, context.Operand1);
		}

		private void Or64(Context context)
		{
			context.ReplaceInstruction(X64.Or64);
		}

		private void Xor64(Context context)
		{
			context.ReplaceInstruction(X64.Xor64);
		}

		private void Move64(Context context)
		{
			context.ReplaceInstruction(X64.Mov64);
		}

		private void MulSigned64(Context context)
		{
			var v1 = AllocateVirtualRegisterI32();
			context.SetInstruction2(X64.Mul64, v1, context.Result, context.Operand1, context.Operand2);
		}

		private void MulUnsigned64(Context context)
		{
			var v1 = AllocateVirtualRegisterI32();
			context.SetInstruction2(X64.Mul64, v1, context.Result, context.Operand1, context.Operand2);
		}

		private void RemSigned64(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			var v1 = AllocateVirtualRegisterI32();
			var v2 = AllocateVirtualRegisterI32();
			var v3 = AllocateVirtualRegisterI32();

			context.SetInstruction2(X64.Cdq64, v1, v2, operand1);
			context.AppendInstruction2(X64.IDiv64, result, v3, v1, v2, operand2);
		}

		private void RemUnsigned64(Context context)
		{
			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			var v1 = AllocateVirtualRegisterI32();
			var v2 = AllocateVirtualRegisterI32();

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
			LoadStore.OrderOperands(context, MethodCompiler);

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

		private void Truncate64x32(Context context)
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

		private void FloatCompare(Context context, X64Instruction instruction)
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
				context.AppendInstruction(X64.Setcc, ConditionCode.NoParity, result);
				context.AppendInstruction(X64.Mov32, v1, ConstantZero32);
				context.AppendInstruction(X64.CMov32, ConditionCode.NotEqual, result, result, v1);
				context.AppendInstruction(X64.Movzx8To32, result, result);
				return;
			}
			else if (condition == ConditionCode.NotEqual)
			{
				context.SetInstruction(instruction, null, operand1, operand2);
				context.AppendInstruction(X64.Setcc, ConditionCode.Parity, result);
				context.AppendInstruction(X64.Mov32, v1, CreateConstant32(1));
				context.AppendInstruction(X64.CMov32, ConditionCode.NotEqual, result, result, v1);
				context.AppendInstruction(X64.Movzx8To32, result, result);
				return;
			}
			else if (condition == ConditionCode.Greater || condition == ConditionCode.UnsignedGreater)
			{
				context.SetInstruction(instruction, null, operand1, operand2);
				context.AppendInstruction(X64.Setcc, ConditionCode.UnsignedGreater, v1);
				context.AppendInstruction(X64.Movzx8To32, result, v1);
				return;
			}
			else if (condition == ConditionCode.Less || condition == ConditionCode.UnsignedLess)
			{
				context.SetInstruction(instruction, null, operand2, operand1);
				context.AppendInstruction(X64.Setcc, ConditionCode.UnsignedGreater, v1);
				context.AppendInstruction(X64.Movzx8To32, result, v1);
				return;
			}
			else if (condition == ConditionCode.GreaterOrEqual || condition == ConditionCode.UnsignedGreaterOrEqual)
			{
				context.SetInstruction(instruction, null, operand2, operand1);
				context.AppendInstruction(X64.Setcc, ConditionCode.NoCarry, v1);
				context.AppendInstruction(X64.Movzx8To32, result, v1);
				return;
			}
			else if (condition == ConditionCode.LessOrEqual || condition == ConditionCode.UnsignedLessOrEqual)
			{
				context.SetInstruction(instruction, null, operand2, operand1);
				context.AppendInstruction(X64.Setcc, ConditionCode.NoCarry, v1);
				context.AppendInstruction(X64.Movzx8To32, result, v1);
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

			var instruction = operand.IsR4 ? (BaseInstruction)X64.MovssLoad : X64.MovsdLoad;

			context.InsertBefore().SetInstruction(instruction, v1, label, ConstantZero);

			return v1;
		}

		#endregion Helper Methods
	}
}
