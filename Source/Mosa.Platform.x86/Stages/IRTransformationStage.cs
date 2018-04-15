// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.MosaTypeSystem;
using System;
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
			AddVisitation(IRInstruction.AddFloatR4, AddFloatR4);
			AddVisitation(IRInstruction.AddFloatR8, AddFloatR8);
			AddVisitation(IRInstruction.AddressOf, AddressOf);
			AddVisitation(IRInstruction.AddSigned32, AddSigned32);
			AddVisitation(IRInstruction.AddUnsigned32, AddUnsigned32);
			AddVisitation(IRInstruction.ArithmeticShiftRight32, ArithmeticShiftRight32);
			AddVisitation(IRInstruction.Break, Break);
			AddVisitation(IRInstruction.CallDirect, CallDirect);
			AddVisitation(IRInstruction.CompareFloatR4, CompareFloatR4);
			AddVisitation(IRInstruction.CompareFloatR8, CompareFloatR8);
			AddVisitation(IRInstruction.CompareInteger32x32, CompareInteger32x32);
			AddVisitation(IRInstruction.CompareIntegerBranch32, CompareIntegerBranch32);
			AddVisitation(IRInstruction.LoadCompound, LoadCompound);
			AddVisitation(IRInstruction.MoveCompound, MoveCompound);
			AddVisitation(IRInstruction.StoreCompound, StoreCompound);
			AddVisitation(IRInstruction.ConvertFloatR4ToFloatR8, ConvertFloatR4ToFloatR8);
			AddVisitation(IRInstruction.ConvertFloatR8ToFloatR4, ConvertFloatR8ToFloatR4);
			AddVisitation(IRInstruction.ConvertFloatR4ToInteger32, ConvertFloatR4ToInteger32);
			AddVisitation(IRInstruction.ConvertFloatR8ToInteger32, ConvertFloatR8ToInteger32);
			AddVisitation(IRInstruction.ConvertInteger32ToFloatR4, ConvertInteger32ToFloatR4);
			AddVisitation(IRInstruction.ConvertInteger32ToFloatR8, ConvertInteger32ToFloatR8);
			AddVisitation(IRInstruction.DivFloatR4, DivFloatR4);
			AddVisitation(IRInstruction.DivFloatR8, DivFloatR8);
			AddVisitation(IRInstruction.DivSigned32, DivSigned32);
			AddVisitation(IRInstruction.DivUnsigned32, DivUnsigned32);
			AddVisitation(IRInstruction.Jmp, Jmp);
			AddVisitation(IRInstruction.LoadFloatR4, LoadFloatR4);
			AddVisitation(IRInstruction.LoadFloatR8, LoadFloatR8);
			AddVisitation(IRInstruction.LoadInteger32, LoadInteger32);
			AddVisitation(IRInstruction.LoadSignExtended8x32, LoadSignExtended8x32);
			AddVisitation(IRInstruction.LoadSignExtended16x32, LoadSignExtended16x32);
			AddVisitation(IRInstruction.LoadZeroExtended8x32, LoadZeroExtended8x32);
			AddVisitation(IRInstruction.LoadZeroExtended16x32, LoadZeroExtended16x32);
			AddVisitation(IRInstruction.LoadParameterFloatR4, LoadParameterFloatR4);
			AddVisitation(IRInstruction.LoadParameterFloatR8, LoadParameterFloatR8);
			AddVisitation(IRInstruction.LoadParameterInteger32, LoadParameterInteger32);
			AddVisitation(IRInstruction.LoadParameterSignExtended8x32, LoadParameterSignExtended8x32);
			AddVisitation(IRInstruction.LoadParameterSignExtended16x32, LoadParameterSignExtended16x32);
			AddVisitation(IRInstruction.LoadParameterZeroExtended8x32, LoadParameterZeroExtended8x32);
			AddVisitation(IRInstruction.LoadParameterZeroExtended16x32, LoadParameterZeroExtended16x32);
			AddVisitation(IRInstruction.LoadParameterCompound, LoadParameterCompound);
			AddVisitation(IRInstruction.LogicalAnd32, LogicalAnd32);
			AddVisitation(IRInstruction.LogicalNot32, LogicalNot32);
			AddVisitation(IRInstruction.LogicalOr32, LogicalOr32);
			AddVisitation(IRInstruction.LogicalXor32, LogicalXor32);
			AddVisitation(IRInstruction.MoveFloatR4, MoveFloatR4);
			AddVisitation(IRInstruction.MoveFloatR8, MoveFloatR8);
			AddVisitation(IRInstruction.MoveInteger32, MoveInteger32);
			AddVisitation(IRInstruction.SignExtended8x32, SignExtended8x32);
			AddVisitation(IRInstruction.SignExtended16x32, SignExtended16x32);
			AddVisitation(IRInstruction.ZeroExtended8x32, ZeroExtended8x32);
			AddVisitation(IRInstruction.ZeroExtended16x32, ZeroExtended16x32);
			AddVisitation(IRInstruction.MulFloatR4, MulFloatR4);
			AddVisitation(IRInstruction.MulFloatR8, MulFloatR8);
			AddVisitation(IRInstruction.MulSigned32, MulSigned32);
			AddVisitation(IRInstruction.MulUnsigned32, MulUnsigned32);
			AddVisitation(IRInstruction.Nop, Nop);
			AddVisitation(IRInstruction.RemSigned32, RemSigned32);
			AddVisitation(IRInstruction.RemUnsigned32, RemUnsigned32);
			AddVisitation(IRInstruction.ShiftLeft32, ShiftLeft32);
			AddVisitation(IRInstruction.ShiftRight32, ShiftRight32);
			AddVisitation(IRInstruction.StoreFloatR4, StoreFloatR4);
			AddVisitation(IRInstruction.StoreFloatR8, StoreFloatR8);
			AddVisitation(IRInstruction.StoreInteger8, StoreInteger8);
			AddVisitation(IRInstruction.StoreInteger16, StoreInteger16);
			AddVisitation(IRInstruction.StoreInteger32, StoreInteger32);
			AddVisitation(IRInstruction.StoreParameterFloatR4, StoreParameterFloatR4);
			AddVisitation(IRInstruction.StoreParameterFloatR8, StoreParameterFloatR8);
			AddVisitation(IRInstruction.StoreParameterInteger8, StoreParameterInteger8);
			AddVisitation(IRInstruction.StoreParameterInteger16, StoreParameterInteger16);
			AddVisitation(IRInstruction.StoreParameterInteger32, StoreParameterInteger32);
			AddVisitation(IRInstruction.StoreParameterCompound, StoreParameterCompound);
			AddVisitation(IRInstruction.SubFloatR4, SubFloatR4);
			AddVisitation(IRInstruction.SubFloatR8, SubFloatR8);
			AddVisitation(IRInstruction.SubSigned32, SubSigned32);
			AddVisitation(IRInstruction.SubUnsigned32, SubUnsigned32);
			AddVisitation(IRInstruction.Switch, Switch);
		}

		#region Visitation Methods

		private void AddFloatR4(InstructionNode node)
		{
			Debug.Assert(node.Result.IsR4);
			Debug.Assert(node.Operand1.IsR4);

			node.ReplaceInstruction(X86.Addss);
		}

		private void AddFloatR8(InstructionNode node)
		{
			Debug.Assert(node.Result.IsR8);
			Debug.Assert(node.Operand1.IsR8);

			node.ReplaceInstruction(X86.Addsd);
		}

		private void AddressOf(InstructionNode node)
		{
			Debug.Assert(node.Operand1.IsOnStack | node.Operand1.IsStaticField);

			if (node.Operand1.IsStaticField)
			{
				node.SetInstruction(X86.Mov32, node.Result, node.Operand1);
			}
			else if (node.Operand1.IsStackLocal)
			{
				node.SetInstruction(X86.Lea32, node.Result, StackFrame, node.Operand1);
			}
			else
			{
				var offset = CreateConstant(node.Operand1.Offset);

				node.SetInstruction(X86.Lea32, node.Result, StackFrame, offset);
			}
		}

		private void AddSigned32(InstructionNode node)
		{
			node.ReplaceInstruction(X86.Add32);
		}

		private void AddUnsigned32(InstructionNode node)
		{
			node.ReplaceInstruction(X86.Add32);
		}

		private void ArithmeticShiftRight32(InstructionNode node)
		{
			node.ReplaceInstruction(X86.Sar32);
		}

		private void Break(InstructionNode node)
		{
			node.SetInstruction(X86.Break);
		}

		private void CallDirect(InstructionNode node)
		{
			Debug.Assert(node.Operand1 != null);

			if (node.Operand1.IsConstant)
				node.ReplaceInstruction(X86.CallStatic);
			else if (node.Operand1.IsVirtualRegister)
				node.ReplaceInstruction(X86.CallReg);
			else throw new NotSupportedException();
		}

		private void CompareFloatR4(Context context)
		{
			FloatCompare(context, X86.Ucomiss);
		}

		private void CompareFloatR8(Context context)
		{
			FloatCompare(context, X86.Ucomisd);
		}

		private void CompareInteger32x32(Context context)
		{
			var condition = context.ConditionCode;
			var resultOperand = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			BaseInstruction setcc = GetSetcc(condition);

			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			context.SetInstruction(X86.Cmp32, null, operand1, operand2);
			context.AppendInstruction(setcc, v1);
			context.AppendInstruction(X86.Movzx8To32, resultOperand, v1);

			//Operand v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			//context.SetInstruction(X86.MovConst32, v1, ConstantZero);
			//context.AppendInstruction(X86.Cmp32, null, operand1, operand2);
			//context.AppendInstruction(setcc, condition, v1);
			//context.AppendInstruction(X86.Mov32, resultOperand, v1);
		}

		private void CompareIntegerBranch32(Context context)
		{
			var target = context.BranchTargets[0];
			var condition = context.ConditionCode;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			var branch = GetBranch(condition);

			context.SetInstruction(X86.Cmp32, null, operand1, operand2);
			context.AppendInstruction(branch, target);
		}

		private void ConvertFloatR4ToFloatR8(InstructionNode node)
		{
			node.ReplaceInstruction(X86.Cvtss2sd);
		}

		private void ConvertFloatR4ToInteger32(InstructionNode node)
		{
			Debug.Assert(node.Result.Type.IsI1 || node.Result.Type.IsI2 || node.Result.Type.IsI4);
			node.ReplaceInstruction(X86.Cvttss2si);
		}

		private void ConvertFloatR8ToFloatR4(InstructionNode node)
		{
			node.ReplaceInstruction(X86.Cvtsd2ss);
		}

		private void ConvertFloatR8ToInteger32(InstructionNode node)
		{
			Debug.Assert(node.Result.Type.IsI1 || node.Result.Type.IsI2 || node.Result.Type.IsI4);
			node.ReplaceInstruction(X86.Cvttsd2si);
		}

		private void ConvertInteger32ToFloatR4(InstructionNode node)
		{
			Debug.Assert(node.Result.IsR4);
			node.ReplaceInstruction(X86.Cvtsi2ss);
		}

		private void ConvertInteger32ToFloatR8(InstructionNode node)
		{
			Debug.Assert(node.Result.IsR8);
			node.ReplaceInstruction(X86.Cvtsi2sd);
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

			node.ReplaceInstruction(X86.Divss);
		}

		private void DivFloatR8(InstructionNode node)
		{
			Debug.Assert(node.Result.IsR8);
			Debug.Assert(node.Operand1.IsR8);

			node.ReplaceInstruction(X86.Divsd);
		}

		private void DivSigned32(Context context)
		{
			Operand operand1 = context.Operand1;
			Operand operand2 = context.Operand2;
			Operand result = context.Result;

			Operand v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			Operand v2 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);
			Operand v3 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			context.SetInstruction2(X86.Cdq, v1, v2, operand1);
			context.AppendInstruction2(X86.IDiv32, v3, result, v1, v2, operand2);
		}

		private void DivUnsigned32(Context context)
		{
			Operand operand1 = context.Operand1;
			Operand operand2 = context.Operand2;
			Operand result = context.Result;

			Operand v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);
			Operand v2 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);

			context.SetInstruction(X86.MovConst32, v1, ConstantZero);
			context.AppendInstruction2(X86.Div32, v1, v2, v1, operand1, operand2);
			context.AppendInstruction(X86.Mov32, result, v2);
		}

		private void FloatCompare(Context context, X86Instruction instruction)
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

						var newBlocks = CreateNewBlockContexts(2);
						var nextBlock = Split(context);

						context.SetInstruction(X86.MovConst32, result, CreateConstant(1));
						context.AppendInstruction(instruction, null, left, right);
						context.AppendInstruction(X86.BranchParity, newBlocks[1].Block);
						context.AppendInstruction(X86.Jmp, newBlocks[0].Block);

						newBlocks[0].AppendInstruction(X86.BranchNotEqual, newBlocks[1].Block);
						newBlocks[0].AppendInstruction(X86.Jmp, nextBlock.Block);

						newBlocks[1].AppendInstruction(X86.MovConst32, result, ConstantZero);
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

						var newBlocks = CreateNewBlockContexts(1);
						var nextBlock = Split(context);

						context.SetInstruction(X86.Mov32, result, CreateConstant(1));
						context.AppendInstruction(instruction, null, left, right);
						context.AppendInstruction(X86.BranchParity, nextBlock.Block);
						context.AppendInstruction(X86.Jmp, newBlocks[0].Block);
						newBlocks[0].AppendInstruction(X86.SetNotEqual, result);

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

						context.SetInstruction(X86.Mov32, result, ConstantZero);
						context.AppendInstruction(instruction, null, right, left);
						context.AppendInstruction(X86.SetUnsignedGreaterThan, result);
						break;
					}
				case ConditionCode.GreaterThan:
					{
						//	a>b
						//	mov	eax, 0
						//	ucomisd	xmm0, xmm1
						//	seta	al

						context.SetInstruction(X86.MovConst32, result, ConstantZero);
						context.AppendInstruction(instruction, null, left, right);
						context.AppendInstruction(X86.SetUnsignedGreaterThan, result);
						break;
					}
				case ConditionCode.LessOrEqual:
					{
						//	a<=b
						//	mov	eax, 0
						//	ucomisd	xmm1, xmm0
						//	setae	al

						context.SetInstruction(X86.Mov32, result, ConstantZero);
						context.AppendInstruction(instruction, null, right, left);
						context.AppendInstruction(X86.SetUnsignedGreaterOrEqual, result);
						break;
					}
				case ConditionCode.GreaterOrEqual:
					{
						//	a>=b
						//	mov	eax, 0
						//	ucomisd	xmm0, xmm1
						//	setae	al

						context.SetInstruction(X86.Mov32, result, ConstantZero);
						context.AppendInstruction(instruction, null, left, right);
						context.AppendInstruction(X86.SetUnsignedGreaterOrEqual, result);
						break;
					}
			}
		}

		private void Jmp(InstructionNode node)
		{
			if (node.Operand1 == null)
				node.ReplaceInstruction(X86.Jmp);
			else
				node.ReplaceInstruction(X86.JmpStatic); // FUTURE: Add IR.JumpStatic
		}

		private void LoadCompound(Context context)
		{
			CopyCompound(context, context.Result.Type, StackFrame, context.Result, context.Operand1, context.Operand2);
		}

		private void LoadFloatR4(InstructionNode node)
		{
			Debug.Assert(node.Result.IsR4);

			node.SetInstruction(X86.MovssLoad, node.Result, node.Operand1, node.Operand2);
		}

		private void LoadFloatR8(InstructionNode node)
		{
			Debug.Assert(node.Result.IsR8);

			node.SetInstruction(X86.MovsdLoad, node.Result, node.Operand1, node.Operand2);
		}

		private void LoadInteger32(InstructionNode node)
		{
			Debug.Assert(!node.Result.IsR4);
			Debug.Assert(!node.Result.IsR8);

			LoadStore.OrderLoadOperands(node, MethodCompiler);

			node.SetInstruction(X86.MovLoad32, node.Result, node.Operand1, node.Operand2);
		}

		private void LoadParameterCompound(Context context)
		{
			CopyCompound(context, context.Result.Type, StackFrame, context.Result, StackFrame, context.Operand1);
		}

		private void LoadParameterFloatR4(InstructionNode node)
		{
			Debug.Assert(node.Result.IsR4);

			node.SetInstruction(X86.MovssLoad, node.Result, StackFrame, node.Operand1);
		}

		private void LoadParameterFloatR8(InstructionNode node)
		{
			Debug.Assert(node.Result.IsR8);

			node.SetInstruction(X86.MovsdLoad, node.Result, StackFrame, node.Operand1);
		}

		private void LoadParameterInteger32(InstructionNode node)
		{
			node.SetInstruction(X86.MovLoad32, node.Result, StackFrame, node.Operand1);
		}

		private void LoadParameterSignExtended16x32(Context node)
		{
			node.SetInstruction(X86.MovsxLoad16, node.Result, StackFrame, node.Operand1);
		}

		private void LoadParameterSignExtended8x32(Context node)
		{
			node.SetInstruction(X86.MovsxLoad8, node.Result, StackFrame, node.Operand1);
		}

		private void LoadParameterZeroExtended16x32(Context node)
		{
			node.SetInstruction(X86.MovzxLoad16, node.Result, StackFrame, node.Operand1);
		}

		private void LoadParameterZeroExtended8x32(Context node)
		{
			node.SetInstruction(X86.MovzxLoad8, node.Result, StackFrame, node.Operand1);
		}

		private void LoadSignExtended8x32(InstructionNode node)
		{
			LoadStore.OrderLoadOperands(node, MethodCompiler);

			node.SetInstruction(X86.MovsxLoad8, node.Result, node.Operand1, node.Operand2);
		}

		private void LoadSignExtended16x32(InstructionNode node)
		{
			LoadStore.OrderLoadOperands(node, MethodCompiler);

			node.SetInstruction(X86.MovsxLoad16, node.Result, node.Operand1, node.Operand2);
		}

		private void LoadZeroExtended8x32(InstructionNode node)
		{
			LoadStore.OrderLoadOperands(node, MethodCompiler);

			node.SetInstruction(X86.MovzxLoad8, node.Result, node.Operand1, node.Operand2);
		}

		private void LoadZeroExtended16x32(InstructionNode node)
		{
			LoadStore.OrderLoadOperands(node, MethodCompiler);

			node.SetInstruction(X86.MovzxLoad16, node.Result, node.Operand1, node.Operand2);
		}

		private void LogicalAnd32(InstructionNode node)
		{
			node.ReplaceInstruction(X86.And32);
		}

		private void LogicalNot32(Context context)
		{
			var dest = context.Result;

			context.SetInstruction(X86.Mov32, dest, context.Operand1);
			context.AppendInstruction(X86.Not32, dest, dest);
		}

		private void LogicalOr32(InstructionNode node)
		{
			node.ReplaceInstruction(X86.Or32);
		}

		private void LogicalXor32(InstructionNode node)
		{
			node.ReplaceInstruction(X86.Xor32);
		}

		private void MoveCompound(Context context)
		{
			CopyCompound(context, context.Result.Type, StackFrame, context.Result, StackFrame, context.Operand1);
		}

		private void MoveFloatR4(InstructionNode node)
		{
			node.ReplaceInstruction(X86.Movss);
		}

		private void MoveFloatR8(InstructionNode node)
		{
			node.ReplaceInstruction(X86.Movsd);
		}

		private void MoveInteger(InstructionNode node)
		{
			node.ReplaceInstruction(X86.Mov32);
		}

		private void MoveInteger32(InstructionNode node)
		{
			node.ReplaceInstruction(X86.Mov32);
		}

		private void MulFloatR4(InstructionNode node)
		{
			Debug.Assert(node.Result.IsR4);
			Debug.Assert(node.Operand1.IsR4);

			node.ReplaceInstruction(X86.Mulss);
		}

		private void MulFloatR8(InstructionNode node)
		{
			Debug.Assert(node.Result.IsR8);
			Debug.Assert(node.Operand1.IsR8);

			node.ReplaceInstruction(X86.Mulsd);
		}

		private void MulSigned32(InstructionNode node)
		{
			Operand result = node.Result;
			Operand operand1 = node.Operand1;
			Operand operand2 = node.Operand2;

			Operand v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);
			node.SetInstruction2(X86.Mul32, v1, result, operand1, operand2);
		}

		private void MulUnsigned32(InstructionNode node)
		{
			Operand result = node.Result;
			Operand operand1 = node.Operand1;
			Operand operand2 = node.Operand2;

			Operand v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);
			node.SetInstruction2(X86.Mul32, v1, result, operand1, operand2);
		}

		private void Nop(InstructionNode node)
		{
			node.SetInstruction(X86.Nop);
		}

		private void RemSigned32(Context context)
		{
			Operand result = context.Result;
			Operand operand1 = context.Operand1;
			Operand operand2 = context.Operand2;

			Operand v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			Operand v2 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);
			Operand v3 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			context.SetInstruction2(X86.Cdq, v1, v2, operand1);
			context.AppendInstruction2(X86.IDiv32, result, v3, v1, v2, operand2);
		}

		private void RemUnsigned32(Context context)
		{
			Operand result = context.Result;
			Operand operand1 = context.Operand1;
			Operand operand2 = context.Operand2;

			Operand v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);
			Operand v2 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);

			context.SetInstruction(X86.MovConst32, v1, ConstantZero);
			context.AppendInstruction2(X86.Div32, v1, v2, v1, operand1, operand2);
			context.AppendInstruction(X86.Mov32, result, v1);
		}

		private void ShiftLeft32(InstructionNode node)
		{
			node.ReplaceInstruction(X86.Shl32);
		}

		private void ShiftRight32(InstructionNode node)
		{
			node.ReplaceInstruction(X86.Shr32);
		}

		private void SignExtended8x32(InstructionNode node)
		{
			node.ReplaceInstruction(X86.Movsx8To32);
		}

		private void SignExtended16x32(InstructionNode node)
		{
			node.ReplaceInstruction(X86.Movsx16To32);
		}

		private void StoreCompound(Context context)
		{
			CopyCompound(context, context.Operand3.Type, context.Operand1, context.Operand2, StackFrame, context.Operand3);
		}

		private void StoreFloatR4(InstructionNode node)
		{
			node.SetInstruction(X86.MovssStore, null, node.Operand1, node.Operand2, node.Operand3);
		}

		private void StoreFloatR8(InstructionNode node)
		{
			node.SetInstruction(X86.MovsdStore, null, node.Operand1, node.Operand2, node.Operand3);
		}

		private void StoreInteger16(InstructionNode node)
		{
			LoadStore.OrderStoreOperands(node, MethodCompiler);

			node.SetInstruction(X86.MovStore16, null, node.Operand1, node.Operand2, node.Operand3);
		}

		private void StoreInteger32(InstructionNode node)
		{
			LoadStore.OrderStoreOperands(node, MethodCompiler);

			node.SetInstruction(X86.MovStore32, null, node.Operand1, node.Operand2, node.Operand3);
		}

		private void StoreInteger8(InstructionNode node)
		{
			LoadStore.OrderStoreOperands(node, MethodCompiler);

			node.SetInstruction(X86.MovStore8, null, node.Operand1, node.Operand2, node.Operand3);
		}

		private void StoreParameterCompound(Context context)
		{
			CopyCompound(context, context.Operand2.Type, StackFrame, context.Operand1, StackFrame, context.Operand2);
		}

		private void StoreParameterFloatR4(InstructionNode node)
		{
			node.SetInstruction(X86.MovssStore, null, StackFrame, node.Operand1, node.Operand2);
		}

		private void StoreParameterFloatR8(InstructionNode node)
		{
			node.SetInstruction(X86.MovsdStore, null, StackFrame, node.Operand1, node.Operand2);
		}

		private void StoreParameterInteger16(InstructionNode node)
		{
			node.SetInstruction(X86.MovStore16, null, StackFrame, node.Operand1, node.Operand2);
		}

		private void StoreParameterInteger32(InstructionNode node)
		{
			node.SetInstruction(X86.MovStore32, null, StackFrame, node.Operand1, node.Operand2);
		}

		private void StoreParameterInteger8(InstructionNode node)
		{
			node.SetInstruction(X86.MovStore8, null, StackFrame, node.Operand1, node.Operand2);
		}

		private void SubFloatR4(InstructionNode node)
		{
			Debug.Assert(node.Result.IsR4);
			Debug.Assert(node.Operand1.IsR4);

			node.ReplaceInstruction(X86.Subss);
		}

		private void SubFloatR8(InstructionNode node)
		{
			Debug.Assert(node.Result.IsR8);
			Debug.Assert(node.Operand1.IsR8);

			node.ReplaceInstruction(X86.Subsd);
		}

		private void SubSigned32(InstructionNode node)
		{
			node.ReplaceInstruction(X86.Sub32);
		}

		private void SubUnsigned32(InstructionNode node)
		{
			node.ReplaceInstruction(X86.Sub32);
		}

		private void Switch(Context context)
		{
			var targets = context.BranchTargets;
			Operand operand = context.Operand1;

			context.Empty();

			for (int i = 0; i < targets.Count - 1; ++i)
			{
				context.AppendInstruction(X86.CmpConst32, null, operand, CreateConstant(i));
				context.AppendInstruction(X86.BranchEqual, targets[i]);
			}
		}

		private void ZeroExtended8x32(InstructionNode node)
		{
			node.ReplaceInstruction(X86.Movzx8To32);
		}

		private void ZeroExtended16x32(InstructionNode node)
		{
			node.ReplaceInstruction(X86.Movzx16To32);
		}

		#endregion Visitation Methods

		#region Helper Methods

		public static BaseInstruction GetBranch(ConditionCode condition)
		{
			switch (condition)
			{
				case ConditionCode.Overflow: return X86.BranchOverflow;
				case ConditionCode.NoOverflow: return X86.BranchNoOverflow;
				case ConditionCode.Carry: return X86.BranchCarry;
				case ConditionCode.UnsignedLessThan: return X86.BranchUnsignedLessThan;
				case ConditionCode.UnsignedGreaterOrEqual: return X86.BranchUnsignedGreaterOrEqual;
				case ConditionCode.NoCarry: return X86.BranchNoCarry;
				case ConditionCode.Equal: return X86.BranchEqual;
				case ConditionCode.Zero: return X86.BranchZero;
				case ConditionCode.NotEqual: return X86.BranchNotEqual;
				case ConditionCode.NotZero: return X86.BranchNotZero;
				case ConditionCode.UnsignedLessOrEqual: return X86.BranchUnsignedLessOrEqual;
				case ConditionCode.UnsignedGreaterThan: return X86.BranchUnsignedGreaterThan;
				case ConditionCode.Signed: return X86.BranchSigned;
				case ConditionCode.NotSigned: return X86.BranchNotSigned;
				case ConditionCode.LessThan: return X86.BranchLessThan;
				case ConditionCode.GreaterOrEqual: return X86.BranchGreaterOrEqual;
				case ConditionCode.LessOrEqual: return X86.BranchLessOrEqual;
				case ConditionCode.GreaterThan: return X86.BranchGreaterThan;

				default: throw new NotSupportedException();
			}
		}

		public static BaseInstruction GetSetcc(ConditionCode condition)
		{
			switch (condition)
			{
				case ConditionCode.Overflow: return X86.SetOverflow;
				case ConditionCode.NoOverflow: return X86.SetNoOverflow;
				case ConditionCode.Carry: return X86.SetCarry;
				case ConditionCode.UnsignedLessThan: return X86.SetUnsignedLessThan;
				case ConditionCode.UnsignedGreaterOrEqual: return X86.SetUnsignedGreaterOrEqual;
				case ConditionCode.NoCarry: return X86.SetNoCarry;
				case ConditionCode.Equal: return X86.SetEqual;
				case ConditionCode.Zero: return X86.SetZero;
				case ConditionCode.NotEqual: return X86.SetNotEqual;
				case ConditionCode.NotZero: return X86.SetNotZero;
				case ConditionCode.UnsignedLessOrEqual: return X86.SetUnsignedLessOrEqual;
				case ConditionCode.UnsignedGreaterThan: return X86.SetUnsignedGreaterThan;
				case ConditionCode.Signed: return X86.SetSigned;
				case ConditionCode.NotSigned: return X86.SetNotSigned;
				case ConditionCode.LessThan: return X86.SetLessThan;
				case ConditionCode.GreaterOrEqual: return X86.SetGreaterOrEqual;
				case ConditionCode.LessOrEqual: return X86.SetLessOrEqual;
				case ConditionCode.GreaterThan: return X86.SetGreaterThan;

				default: throw new NotSupportedException();
			}
		}

		#endregion Helper Methods
	}
}
