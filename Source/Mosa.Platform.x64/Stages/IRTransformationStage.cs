// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.MosaTypeSystem;
using System;
using System.Diagnostics;

namespace Mosa.Platform.x64.Stages
{
	/// <summary>
	/// Transforms IR instructions into their appropriate X64.
	/// </summary>
	/// <seealso cref="Mosa.Platform.x64.BaseTransformationStage" />
	/// <remarks>
	/// This transformation stage transforms IR instructions into their equivalent X86 sequences.
	/// </remarks>
	public sealed class IRTransformationStage : BaseTransformationStage
	{
		protected override void PopulateVisitationDictionary()
		{
			AddVisitation(IRInstruction.AddFloatR4, AddFloatR4);
			AddVisitation(IRInstruction.AddFloatR8, AddFloatR8);
			AddVisitation(IRInstruction.AddressOf, AddressOf);
			AddVisitation(IRInstruction.Add64, Add64);
			AddVisitation(IRInstruction.AddCarryOut64, AddCarryOut64);
			AddVisitation(IRInstruction.AddWithCarry64, AddWithCarry64);
			AddVisitation(IRInstruction.ArithShiftRight64, ArithShiftRight64);
			AddVisitation(IRInstruction.Break, Break);
			AddVisitation(IRInstruction.CallDirect, CallDirect);
			AddVisitation(IRInstruction.CompareFloatR4, CompareFloatR4);
			AddVisitation(IRInstruction.CompareFloatR8, CompareFloatR8);
			AddVisitation(IRInstruction.CompareInt64x64, CompareInt64x64);
			AddVisitation(IRInstruction.CompareIntBranch64, CompareIntBranch64);
			AddVisitation(IRInstruction.IfThenElse64, IfThenElse64);
			AddVisitation(IRInstruction.LoadCompound, LoadCompound);
			AddVisitation(IRInstruction.MoveCompound, MoveCompound);
			AddVisitation(IRInstruction.StoreCompound, StoreCompound);
			AddVisitation(IRInstruction.ConvertFloatR4ToFloatR8, ConvertFloatR4ToFloatR8);
			AddVisitation(IRInstruction.ConvertFloatR8ToFloatR4, ConvertFloatR8ToFloatR4);
			AddVisitation(IRInstruction.ConvertFloatR4ToInt64, ConvertFloatR4ToInt64);
			AddVisitation(IRInstruction.ConvertFloatR8ToInt64, ConvertFloatR8ToInt64);
			AddVisitation(IRInstruction.ConvertInt64ToFloatR4, ConvertInt64ToFloatR4);
			AddVisitation(IRInstruction.ConvertInt64ToFloatR8, ConvertInt64ToFloatR8);
			AddVisitation(IRInstruction.DivFloatR4, DivFloatR4);
			AddVisitation(IRInstruction.DivFloatR8, DivFloatR8);
			AddVisitation(IRInstruction.DivSigned64, DivSigned64);
			AddVisitation(IRInstruction.DivUnsigned64, DivUnsigned64);
			AddVisitation(IRInstruction.Jmp, Jmp);
			AddVisitation(IRInstruction.LoadFloatR4, LoadFloatR4);
			AddVisitation(IRInstruction.LoadFloatR8, LoadFloatR8);
			AddVisitation(IRInstruction.LoadInt64, LoadInt64);
			AddVisitation(IRInstruction.LoadSignExtend8x64, LoadSignExtend8x64);
			AddVisitation(IRInstruction.LoadSignExtend16x64, LoadSignExtend16x64);
			AddVisitation(IRInstruction.LoadZeroExtend8x64, LoadZeroExtend8x64);
			AddVisitation(IRInstruction.LoadZeroExtend16x64, LoadZeroExtend16x64);
			AddVisitation(IRInstruction.LoadParamFloatR4, LoadParamFloatR4);
			AddVisitation(IRInstruction.LoadParamFloatR8, LoadParamFloatR8);
			AddVisitation(IRInstruction.LoadParamInt64, LoadParamInt64);
			AddVisitation(IRInstruction.LoadParamSignExtend8x64, LoadParamSignExtend8x64);
			AddVisitation(IRInstruction.LoadParamSignExtend16x64, LoadParamSignExtend16x64);
			AddVisitation(IRInstruction.LoadParamZeroExtend8x64, LoadParamZeroExtend8x64);
			AddVisitation(IRInstruction.LoadParamZeroExtend16x64, LoadParamZeroExtend16x64);
			AddVisitation(IRInstruction.LoadParamCompound, LoadParamCompound);
			AddVisitation(IRInstruction.LogicalAnd64, LogicalAnd64);
			AddVisitation(IRInstruction.LogicalNot64, LogicalNot64);
			AddVisitation(IRInstruction.LogicalOr64, LogicalOr64);
			AddVisitation(IRInstruction.LogicalXor64, LogicalXor64);
			AddVisitation(IRInstruction.MoveFloatR4, MoveFloatR4);
			AddVisitation(IRInstruction.MoveFloatR8, MoveFloatR8);
			AddVisitation(IRInstruction.MoveInt64, MoveInt64);
			AddVisitation(IRInstruction.SignExtend8x64, SignExtend8x64);
			AddVisitation(IRInstruction.SignExtend16x64, SignExtend16x64);

			//AddVisitation(IRInstruction.SignExtend32x64, SignExtend32x64);
			AddVisitation(IRInstruction.ZeroExtend8x64, ZeroExtend8x64);
			AddVisitation(IRInstruction.ZeroExtend16x64, ZeroExtend16x64);

			//AddVisitation(IRInstruction.ZeroExtend32x64, ZeroExtend32x64);
			AddVisitation(IRInstruction.MulFloatR4, MulFloatR4);
			AddVisitation(IRInstruction.MulFloatR8, MulFloatR8);
			AddVisitation(IRInstruction.MulSigned64, MulSigned64);
			AddVisitation(IRInstruction.MulUnsigned64, MulUnsigned64);
			AddVisitation(IRInstruction.Nop, Nop);
			AddVisitation(IRInstruction.RemSigned64, RemSigned64);
			AddVisitation(IRInstruction.RemUnsigned64, RemUnsigned64);
			AddVisitation(IRInstruction.ShiftLeft64, ShiftLeft64);
			AddVisitation(IRInstruction.ShiftRight64, ShiftRight64);
			AddVisitation(IRInstruction.StoreFloatR4, StoreFloatR4);
			AddVisitation(IRInstruction.StoreFloatR8, StoreFloatR8);
			AddVisitation(IRInstruction.StoreInt8, StoreInt8);
			AddVisitation(IRInstruction.StoreInt16, StoreInt16);
			AddVisitation(IRInstruction.StoreInt32, StoreInt32);
			AddVisitation(IRInstruction.StoreParamFloatR4, StoreParamFloatR4);
			AddVisitation(IRInstruction.StoreParamFloatR8, StoreParamFloatR8);
			AddVisitation(IRInstruction.StoreParamInt8, StoreParamInt8);
			AddVisitation(IRInstruction.StoreParamInt16, StoreParamInt16);
			AddVisitation(IRInstruction.StoreParamInt32, StoreParamInt32);
			AddVisitation(IRInstruction.StoreParamCompound, StoreParamCompound);
			AddVisitation(IRInstruction.SubFloatR4, SubFloatR4);
			AddVisitation(IRInstruction.SubFloatR8, SubFloatR8);
			AddVisitation(IRInstruction.Sub64, Sub64);
			AddVisitation(IRInstruction.SubCarryOut64, SubCarryOut64);
			AddVisitation(IRInstruction.SubWithCarry64, SubWithCarry64);
			AddVisitation(IRInstruction.Switch, Switch);
		}

		#region Visitation Methods

		private void AddFloatR4(InstructionNode node)
		{
			Debug.Assert(node.Result.IsR4);
			Debug.Assert(node.Operand1.IsR4);

			node.ReplaceInstruction(X64.Addss);
		}

		private void AddFloatR8(InstructionNode node)
		{
			Debug.Assert(node.Result.IsR8);
			Debug.Assert(node.Operand1.IsR8);

			node.ReplaceInstruction(X64.Addsd);
		}

		private void AddressOf(InstructionNode node)
		{
			Debug.Assert(node.Operand1.IsOnStack | node.Operand1.IsStaticField);

			if (node.Operand1.IsStaticField)
			{
				node.SetInstruction(X64.Mov64, node.Result, node.Operand1);
			}
			else if (node.Operand1.IsStackLocal)
			{
				node.SetInstruction(X64.Lea64, node.Result, StackFrame, node.Operand1);
			}
			else
			{
				var offset = CreateConstant(node.Operand1.Offset);

				node.SetInstruction(X64.Lea64, node.Result, StackFrame, offset);
			}
		}

		private void Add64(InstructionNode node)
		{
			node.ReplaceInstruction(X64.Add64);
		}

		private void AddCarryOut64(Context context)
		{
			var result = context.Result;
			var result2 = context.Result2;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.Boolean);

			context.SetInstruction(X64.Add64, result, operand1, operand2);
			context.AppendInstruction(X64.SetByteIfCarry, v1);
			context.AppendInstruction(X64.Movzx8To64, result2, v1);
		}

		private void AddWithCarry64(Context context)
		{
			var result = context.Result;
			var result2 = context.Result2;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var operand3 = context.Operand3;

			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			context.SetInstruction(X64.Bt64, v1, operand3, CreateConstant((byte)0));
			context.AppendInstruction(X64.Adc64, result, operand1, operand2);
		}

		private void SubCarryOut64(Context context)
		{
			var result = context.Result;
			var result2 = context.Result2;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.Boolean);

			context.SetInstruction(X64.Sub64, result, operand1, operand2);
			context.AppendInstruction(X64.SetByteIfCarry, v1);
			context.AppendInstruction(X64.Movzx8To64, result2, v1);
		}

		private void SubWithCarry64(Context context)
		{
			var result = context.Result;
			var result2 = context.Result2;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var operand3 = context.Operand3;

			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			context.SetInstruction(X64.Bt64, v1, operand3, CreateConstant((byte)0));
			context.AppendInstruction(X64.Sbb64, result, operand1, operand2);
		}

		private void ArithShiftRight64(InstructionNode node)
		{
			node.ReplaceInstruction(X64.Sar64);
		}

		private void Break(InstructionNode node)
		{
			node.SetInstruction(X64.Break);
		}

		private void CallDirect(InstructionNode node)
		{
			Debug.Assert(node.Operand1 != null);

			if (node.Operand1.IsConstant)
				node.ReplaceInstruction(X64.CallStatic);
			else if (node.Operand1.IsVirtualRegister)
				node.ReplaceInstruction(X64.CallReg);
			else throw new NotSupportedException();
		}

		private void CompareFloatR4(Context context)
		{
			FloatCompare(context, X64.Ucomiss);
		}

		private void CompareFloatR8(Context context)
		{
			FloatCompare(context, X64.Ucomisd);
		}

		private void CompareInt64x64(Context context)
		{
			var condition = context.ConditionCode;
			var resultOperand = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			var setcc = GetSetcc(condition);

			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			context.SetInstruction(X64.Cmp64, null, operand1, operand2);
			context.AppendInstruction(setcc, v1);
			context.AppendInstruction(X64.Movzx8To64, resultOperand, v1);
		}

		private void IfThenElse64(Context context)
		{
			var result = context.Operand1;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var operand3 = context.Operand3;

			context.SetInstruction(X64.Cmp64, null, operand1, ConstantZero);
			context.AppendInstruction(X64.CMovNotEqual64, result, operand1);    // true
			context.AppendInstruction(X64.CMovEqual64, result, operand2);       // false
		}

		private void CompareIntBranch64(Context context)
		{
			var target = context.BranchTargets[0];
			var condition = context.ConditionCode;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			var branch = GetBranch(condition);

			context.SetInstruction(X64.Cmp64, null, operand1, operand2);
			context.AppendInstruction(branch, target);
		}

		private void ConvertFloatR4ToFloatR8(InstructionNode node)
		{
			node.ReplaceInstruction(X64.Cvtss2sd);
		}

		private void ConvertFloatR4ToInt64(InstructionNode node)
		{
			Debug.Assert(node.Result.Type.IsI1 || node.Result.Type.IsI2 || node.Result.Type.IsI4);
			node.ReplaceInstruction(X64.Cvttss2si);
		}

		private void ConvertFloatR8ToFloatR4(InstructionNode node)
		{
			node.ReplaceInstruction(X64.Cvtsd2ss);
		}

		private void ConvertFloatR8ToInt64(InstructionNode node)
		{
			Debug.Assert(node.Result.Type.IsI1 || node.Result.Type.IsI2 || node.Result.Type.IsI4);
			node.ReplaceInstruction(X64.Cvttsd2si);
		}

		private void ConvertInt64ToFloatR4(InstructionNode node)
		{
			Debug.Assert(node.Result.IsR4);
			node.ReplaceInstruction(X64.Cvtsi2ss);
		}

		private void ConvertInt64ToFloatR8(InstructionNode node)
		{
			Debug.Assert(node.Result.IsR8);
			node.ReplaceInstruction(X64.Cvtsi2sd);
		}

		private void CopyCompound(Context context, MosaType type, Operand destinationBase, Operand destination, Operand sourceBase, Operand source)
		{
			context.Empty();
			Architecture.InsertCompoundCopy(MethodCompiler, context, destinationBase, destination, sourceBase, source, TypeLayout.GetTypeSize(type));
		}

		private void DivFloatR4(InstructionNode node)
		{
			Debug.Assert(node.Result.IsR4);
			Debug.Assert(node.Operand1.IsR4);

			node.ReplaceInstruction(X64.Divss);
		}

		private void DivFloatR8(InstructionNode node)
		{
			Debug.Assert(node.Result.IsR8);
			Debug.Assert(node.Operand1.IsR8);

			node.ReplaceInstruction(X64.Divsd);
		}

		private void DivSigned64(Context context)
		{
			Operand operand1 = context.Operand1;
			Operand operand2 = context.Operand2;
			Operand result = context.Result;

			Operand v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			Operand v2 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);
			Operand v3 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			context.SetInstruction2(X64.Cdq, v1, v2, operand1);
			context.AppendInstruction2(X64.IDiv64, v3, result, v1, v2, operand2);
		}

		private void DivUnsigned64(Context context)
		{
			Operand operand1 = context.Operand1;
			Operand operand2 = context.Operand2;
			Operand result = context.Result;

			Operand v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);
			Operand v2 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);

			context.SetInstruction(X64.MovConst64, v1, ConstantZero);
			context.AppendInstruction2(X64.Div64, v1, v2, v1, operand1, operand2);
			context.AppendInstruction(X64.Mov64, result, v2);
		}

		private void FloatCompare(Context context, X64Instruction instruction)
		{
			Operand result = context.Result;
			Operand left = context.Operand1;
			Operand right = context.Operand2;
			ConditionCode condition = context.ConditionCode;

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

						context.SetInstruction(X64.MovConst64, result, CreateConstant(1));
						context.AppendInstruction(instruction, null, left, right);
						context.AppendInstruction(X64.BranchParity, newBlocks[1].Block);
						context.AppendInstruction(X64.Jmp, newBlocks[0].Block);

						newBlocks[0].AppendInstruction(X64.BranchNotEqual, newBlocks[1].Block);
						newBlocks[0].AppendInstruction(X64.Jmp, nextBlock.Block);

						newBlocks[1].AppendInstruction(X64.MovConst64, result, ConstantZero);
						newBlocks[1].AppendInstruction(X64.Jmp, nextBlock.Block);
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

						context.SetInstruction(X64.Mov64, result, CreateConstant(1));
						context.AppendInstruction(instruction, null, left, right);
						context.AppendInstruction(X64.BranchParity, nextBlock.Block);
						context.AppendInstruction(X64.Jmp, newBlocks[0].Block);
						newBlocks[0].AppendInstruction(X64.SetByteIfNotEqual, result);

						//newBlocks[0].AppendInstruction(X64.Movzx, InstructionSize.Size8, result, result);
						newBlocks[0].AppendInstruction(X64.Jmp, nextBlock.Block);
						break;
					}
				case ConditionCode.LessThan:
					{
						//	a<b
						//	mov	eax, 0
						//	ucomisd	xmm1, xmm0
						//	seta	al

						context.SetInstruction(X64.Mov64, result, ConstantZero);
						context.AppendInstruction(instruction, null, right, left);
						context.AppendInstruction(X64.SetByteIfUnsignedGreaterThan, result);
						break;
					}
				case ConditionCode.GreaterThan:
					{
						//	a>b
						//	mov	eax, 0
						//	ucomisd	xmm0, xmm1
						//	seta	al

						context.SetInstruction(X64.MovConst64, result, ConstantZero);
						context.AppendInstruction(instruction, null, left, right);
						context.AppendInstruction(X64.SetByteIfUnsignedGreaterThan, result);
						break;
					}
				case ConditionCode.LessOrEqual:
					{
						//	a<=b
						//	mov	eax, 0
						//	ucomisd	xmm1, xmm0
						//	setae	al

						context.SetInstruction(X64.Mov64, result, ConstantZero);
						context.AppendInstruction(instruction, null, right, left);
						context.AppendInstruction(X64.SetByteIfUnsignedGreaterOrEqual, result);
						break;
					}
				case ConditionCode.GreaterOrEqual:
					{
						//	a>=b
						//	mov	eax, 0
						//	ucomisd	xmm0, xmm1
						//	setae	al

						context.SetInstruction(X64.Mov64, result, ConstantZero);
						context.AppendInstruction(instruction, null, left, right);
						context.AppendInstruction(X64.SetByteIfUnsignedGreaterOrEqual, result);
						break;
					}
			}
		}

		private void Jmp(InstructionNode node)
		{
			if (node.Operand1 == null)
				node.ReplaceInstruction(X64.Jmp);
			else
				node.ReplaceInstruction(X64.JmpStatic); // FUTURE: Add IR.JumpStatic
		}

		private void LoadCompound(Context context)
		{
			CopyCompound(context, context.Result.Type, StackFrame, context.Result, context.Operand1, context.Operand2);
		}

		private void LoadFloatR4(InstructionNode node)
		{
			Debug.Assert(node.Result.IsR4);

			node.SetInstruction(X64.MovssLoad, node.Result, node.Operand1, node.Operand2);
		}

		private void LoadFloatR8(InstructionNode node)
		{
			Debug.Assert(node.Result.IsR8);

			node.SetInstruction(X64.MovsdLoad, node.Result, node.Operand1, node.Operand2);
		}

		private void LoadInt64(InstructionNode node)
		{
			Debug.Assert(!node.Result.IsR4);
			Debug.Assert(!node.Result.IsR8);

			LoadStore.OrderLoadOperands(node, MethodCompiler);

			node.SetInstruction(X64.MovLoad64, node.Result, node.Operand1, node.Operand2);
		}

		private void LoadParamCompound(Context context)
		{
			CopyCompound(context, context.Result.Type, StackFrame, context.Result, StackFrame, context.Operand1);
		}

		private void LoadParamFloatR4(InstructionNode node)
		{
			Debug.Assert(node.Result.IsR4);

			node.SetInstruction(X64.MovssLoad, node.Result, StackFrame, node.Operand1);
		}

		private void LoadParamFloatR8(InstructionNode node)
		{
			Debug.Assert(node.Result.IsR8);

			node.SetInstruction(X64.MovsdLoad, node.Result, StackFrame, node.Operand1);
		}

		private void LoadParamInt64(InstructionNode node)
		{
			node.SetInstruction(X64.MovLoad64, node.Result, StackFrame, node.Operand1);
		}

		private void LoadParamSignExtend16x64(Context node)
		{
			node.SetInstruction(X64.MovsxLoad16, node.Result, StackFrame, node.Operand1);
		}

		private void LoadParamSignExtend8x64(Context node)
		{
			node.SetInstruction(X64.MovsxLoad8, node.Result, StackFrame, node.Operand1);
		}

		private void LoadParamZeroExtend16x64(Context node)
		{
			node.SetInstruction(X64.MovzxLoad16, node.Result, StackFrame, node.Operand1);
		}

		private void LoadParamZeroExtend8x64(Context node)
		{
			node.SetInstruction(X64.MovzxLoad8, node.Result, StackFrame, node.Operand1);
		}

		private void LoadSignExtend8x64(InstructionNode node)
		{
			LoadStore.OrderLoadOperands(node, MethodCompiler);

			node.SetInstruction(X64.MovsxLoad8, node.Result, node.Operand1, node.Operand2);
		}

		private void LoadSignExtend16x64(InstructionNode node)
		{
			LoadStore.OrderLoadOperands(node, MethodCompiler);

			node.SetInstruction(X64.MovsxLoad16, node.Result, node.Operand1, node.Operand2);
		}

		private void LoadZeroExtend8x64(InstructionNode node)
		{
			LoadStore.OrderLoadOperands(node, MethodCompiler);

			node.SetInstruction(X64.MovzxLoad8, node.Result, node.Operand1, node.Operand2);
		}

		private void LoadZeroExtend16x64(InstructionNode node)
		{
			LoadStore.OrderLoadOperands(node, MethodCompiler);

			node.SetInstruction(X64.MovzxLoad16, node.Result, node.Operand1, node.Operand2);
		}

		private void LogicalAnd64(InstructionNode node)
		{
			node.ReplaceInstruction(X64.And64);
		}

		private void LogicalNot64(Context context)
		{
			var dest = context.Result;

			context.SetInstruction(X64.Mov64, dest, context.Operand1);
			context.AppendInstruction(X64.Not64, dest, dest);
		}

		private void LogicalOr64(InstructionNode node)
		{
			node.ReplaceInstruction(X64.Or64);
		}

		private void LogicalXor64(InstructionNode node)
		{
			node.ReplaceInstruction(X64.Xor64);
		}

		private void MoveCompound(Context context)
		{
			CopyCompound(context, context.Result.Type, StackFrame, context.Result, StackFrame, context.Operand1);
		}

		private void MoveFloatR4(InstructionNode node)
		{
			node.ReplaceInstruction(X64.Movss);
		}

		private void MoveFloatR8(InstructionNode node)
		{
			node.ReplaceInstruction(X64.Movsd);
		}

		private void MoveInt64(InstructionNode node)
		{
			node.ReplaceInstruction(X64.Mov64);
		}

		private void MulFloatR4(InstructionNode node)
		{
			Debug.Assert(node.Result.IsR4);
			Debug.Assert(node.Operand1.IsR4);

			node.ReplaceInstruction(X64.Mulss);
		}

		private void MulFloatR8(InstructionNode node)
		{
			Debug.Assert(node.Result.IsR8);
			Debug.Assert(node.Operand1.IsR8);

			node.ReplaceInstruction(X64.Mulsd);
		}

		private void MulSigned64(InstructionNode node)
		{
			Operand result = node.Result;
			Operand operand1 = node.Operand1;
			Operand operand2 = node.Operand2;

			Operand v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);
			node.SetInstruction2(X64.Mul64, v1, result, operand1, operand2);
		}

		private void MulUnsigned64(InstructionNode node)
		{
			Operand result = node.Result;
			Operand operand1 = node.Operand1;
			Operand operand2 = node.Operand2;

			Operand v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);
			node.SetInstruction2(X64.Mul64, v1, result, operand1, operand2);
		}

		private void Nop(InstructionNode node)
		{
			node.SetInstruction(X64.Nop);
		}

		private void RemSigned64(Context context)
		{
			Operand result = context.Result;
			Operand operand1 = context.Operand1;
			Operand operand2 = context.Operand2;

			Operand v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			Operand v2 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);
			Operand v3 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			context.SetInstruction2(X64.Cdq, v1, v2, operand1);
			context.AppendInstruction2(X64.IDiv64, result, v3, v1, v2, operand2);
		}

		private void RemUnsigned64(Context context)
		{
			Operand result = context.Result;
			Operand operand1 = context.Operand1;
			Operand operand2 = context.Operand2;

			Operand v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);
			Operand v2 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);

			context.SetInstruction(X64.MovConst64, v1, ConstantZero);
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

		private void SignExtend8x64(InstructionNode node)
		{
			node.ReplaceInstruction(X64.Movsx8To64);
		}

		private void SignExtend16x64(InstructionNode node)
		{
			node.ReplaceInstruction(X64.Movsx16To64);
		}

		//private void SignExtend32x64(InstructionNode node)
		//{
		//	node.ReplaceInstruction(X64.Movsx32To64);
		//}

		private void StoreCompound(Context context)
		{
			CopyCompound(context, context.Operand3.Type, context.Operand1, context.Operand2, StackFrame, context.Operand3);
		}

		private void StoreFloatR4(InstructionNode node)
		{
			node.SetInstruction(X64.MovssStore, null, node.Operand1, node.Operand2, node.Operand3);
		}

		private void StoreFloatR8(InstructionNode node)
		{
			node.SetInstruction(X64.MovsdStore, null, node.Operand1, node.Operand2, node.Operand3);
		}

		private void StoreInt16(InstructionNode node)
		{
			LoadStore.OrderStoreOperands(node, MethodCompiler);

			node.SetInstruction(X64.MovStore16, null, node.Operand1, node.Operand2, node.Operand3);
		}

		private void StoreInt32(InstructionNode node)
		{
			LoadStore.OrderStoreOperands(node, MethodCompiler);

			node.SetInstruction(X64.MovStore32, null, node.Operand1, node.Operand2, node.Operand3);
		}

		private void StoreInt8(InstructionNode node)
		{
			LoadStore.OrderStoreOperands(node, MethodCompiler);

			node.SetInstruction(X64.MovStore8, null, node.Operand1, node.Operand2, node.Operand3);
		}

		private void StoreParamCompound(Context context)
		{
			CopyCompound(context, context.Operand2.Type, StackFrame, context.Operand1, StackFrame, context.Operand2);
		}

		private void StoreParamFloatR4(InstructionNode node)
		{
			node.SetInstruction(X64.MovssStore, null, StackFrame, node.Operand1, node.Operand2);
		}

		private void StoreParamFloatR8(InstructionNode node)
		{
			node.SetInstruction(X64.MovsdStore, null, StackFrame, node.Operand1, node.Operand2);
		}

		private void StoreParamInt16(InstructionNode node)
		{
			node.SetInstruction(X64.MovStore16, null, StackFrame, node.Operand1, node.Operand2);
		}

		private void StoreParamInt32(InstructionNode node)
		{
			node.SetInstruction(X64.MovStore32, null, StackFrame, node.Operand1, node.Operand2);
		}

		private void StoreParamInt8(InstructionNode node)
		{
			node.SetInstruction(X64.MovStore8, null, StackFrame, node.Operand1, node.Operand2);
		}

		private void SubFloatR4(InstructionNode node)
		{
			Debug.Assert(node.Result.IsR4);
			Debug.Assert(node.Operand1.IsR4);

			node.ReplaceInstruction(X64.Subss);
		}

		private void SubFloatR8(InstructionNode node)
		{
			Debug.Assert(node.Result.IsR8);
			Debug.Assert(node.Operand1.IsR8);

			node.ReplaceInstruction(X64.Subsd);
		}

		private void Sub64(InstructionNode node)
		{
			node.ReplaceInstruction(X64.Sub64);
		}

		private void Switch(Context context)
		{
			var targets = context.BranchTargets;
			Operand operand = context.Operand1;

			context.Empty();

			for (int i = 0; i < targets.Count - 1; ++i)
			{
				context.AppendInstruction(X64.CmpConst64, null, operand, CreateConstant(i));
				context.AppendInstruction(X64.BranchEqual, targets[i]);
			}
		}

		private void ZeroExtend8x64(InstructionNode node)
		{
			node.ReplaceInstruction(X64.Movzx8To64);
		}

		private void ZeroExtend16x64(InstructionNode node)
		{
			node.ReplaceInstruction(X64.Movzx16To64);
		}

		//private void ZeroExtend32x64(InstructionNode node)
		//{
		//	node.ReplaceInstruction(X64.Movzx32To64);
		//}

		#endregion Visitation Methods

		#region Helper Methods

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

		public static BaseInstruction GetSetcc(ConditionCode condition)
		{
			switch (condition)
			{
				case ConditionCode.Overflow: return X64.SetByteIfOverflow;
				case ConditionCode.NoOverflow: return X64.SetByteIfNoOverflow;
				case ConditionCode.Carry: return X64.SetByteIfCarry;
				case ConditionCode.UnsignedLessThan: return X64.SetByteIfUnsignedLessThan;
				case ConditionCode.UnsignedGreaterOrEqual: return X64.SetByteIfUnsignedGreaterOrEqual;
				case ConditionCode.NoCarry: return X64.SetByteIfNoCarry;
				case ConditionCode.Equal: return X64.SetByteIfEqual;
				case ConditionCode.Zero: return X64.SetByteIfZero;
				case ConditionCode.NotEqual: return X64.SetByteIfNotEqual;
				case ConditionCode.NotZero: return X64.SetByteIfNotZero;
				case ConditionCode.UnsignedLessOrEqual: return X64.SetByteIfUnsignedLessOrEqual;
				case ConditionCode.UnsignedGreaterThan: return X64.SetByteIfUnsignedGreaterThan;
				case ConditionCode.Signed: return X64.SetByteIfSigned;
				case ConditionCode.NotSigned: return X64.SetByteIfNotSigned;
				case ConditionCode.LessThan: return X64.SetByteIfLessThan;
				case ConditionCode.GreaterOrEqual: return X64.SetByteIfGreaterOrEqual;
				case ConditionCode.LessOrEqual: return X64.SetByteIfLessOrEqual;
				case ConditionCode.GreaterThan: return X64.SetByteIfGreaterThan;

				default: throw new NotSupportedException();
			}
		}

		public static BaseInstruction GetCMovcc64(ConditionCode condition)
		{
			switch (condition)
			{
				case ConditionCode.Overflow: return X64.CMovOverflow64;
				case ConditionCode.NoOverflow: return X64.CMovNoOverflow64;
				case ConditionCode.Carry: return X64.CMovCarry64;
				case ConditionCode.UnsignedLessThan: return X64.CMovUnsignedLessThan64;
				case ConditionCode.UnsignedGreaterOrEqual: return X64.CMovUnsignedGreaterOrEqual64;
				case ConditionCode.NoCarry: return X64.CMovNoCarry64;
				case ConditionCode.Equal: return X64.CMovEqual64;
				case ConditionCode.Zero: return X64.CMovZero64;
				case ConditionCode.NotEqual: return X64.CMovNotEqual64;
				case ConditionCode.NotZero: return X64.CMovNotZero64;
				case ConditionCode.UnsignedLessOrEqual: return X64.CMovUnsignedLessOrEqual64;
				case ConditionCode.UnsignedGreaterThan: return X64.CMovUnsignedGreaterThan64;
				case ConditionCode.Signed: return X64.CMovSigned64;
				case ConditionCode.NotSigned: return X64.CMovNotSigned64;
				case ConditionCode.LessThan: return X64.CMovLessThan64;
				case ConditionCode.GreaterOrEqual: return X64.CMovGreaterOrEqual64;
				case ConditionCode.LessOrEqual: return X64.CMovLessOrEqual64;
				case ConditionCode.GreaterThan: return X64.CMovGreaterThan64;

				default: throw new NotSupportedException();
			}
		}

		#endregion Helper Methods
	}
}
