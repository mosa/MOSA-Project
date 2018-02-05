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
			AddVisitation(IRInstruction.CompareIntegerBranch, CompareIntegerBranch);
			AddVisitation(IRInstruction.LoadCompound, LoadCompound);
			AddVisitation(IRInstruction.MoveCompound, MoveCompound);
			AddVisitation(IRInstruction.StoreCompound, StoreCompound);
			AddVisitation(IRInstruction.ConversionFloatR4ToFloatR8, ConversionFloatR4ToFloatR8);
			AddVisitation(IRInstruction.ConversionFloatR4ToInteger, ConversionFloatR4ToInteger);
			AddVisitation(IRInstruction.ConversionFloatR8ToFloatR4, ConversionFloatR8ToFloatR4);
			AddVisitation(IRInstruction.ConversionFloatR8ToInteger, ConversionFloatR8ToInteger);
			AddVisitation(IRInstruction.ConversionIntegerToFloatR4, ConversionIntegerToFloatR4);
			AddVisitation(IRInstruction.ConversionIntegerToFloatR8, ConversionIntegerToFloatR8);
			AddVisitation(IRInstruction.DivFloatR4, DivFloatR4);
			AddVisitation(IRInstruction.DivFloatR8, DivFloatR8);
			AddVisitation(IRInstruction.DivSigned32, DivSigned32);
			AddVisitation(IRInstruction.DivUnsigned32, DivUnsigned32);
			AddVisitation(IRInstruction.Jmp, Jmp);
			AddVisitation(IRInstruction.LoadFloatR4, LoadFloatR4);
			AddVisitation(IRInstruction.LoadFloatR8, LoadFloatR8);
			AddVisitation(IRInstruction.LoadInteger, LoadInteger);
			AddVisitation(IRInstruction.LoadSignExtended, LoadSignExtended);
			AddVisitation(IRInstruction.LoadZeroExtended, LoadZeroExtended);
			AddVisitation(IRInstruction.LoadParameterFloatR4, LoadParameterFloatR4);
			AddVisitation(IRInstruction.LoadParameterFloatR8, LoadParameterFloatR8);
			AddVisitation(IRInstruction.LoadParameterInteger, LoadParameterInteger);
			AddVisitation(IRInstruction.LoadParameterSignExtended, LoadParameterSignExtended);
			AddVisitation(IRInstruction.LoadParameterZeroExtended, LoadParameterZeroExtended);
			AddVisitation(IRInstruction.LoadParameterCompound, LoadParameterCompound);
			AddVisitation(IRInstruction.LogicalAnd32, LogicalAnd32);
			AddVisitation(IRInstruction.LogicalNot32, LogicalNot32);
			AddVisitation(IRInstruction.LogicalOr32, LogicalOr32);
			AddVisitation(IRInstruction.LogicalXor32, LogicalXor32);
			AddVisitation(IRInstruction.MoveFloatR4, MoveFloatR4);
			AddVisitation(IRInstruction.MoveFloatR8, MoveFloatR8);
			AddVisitation(IRInstruction.MoveInteger, MoveInteger);
			AddVisitation(IRInstruction.MoveSignExtended, SignExtendedMove);
			AddVisitation(IRInstruction.MoveZeroExtended, ZeroExtendedMove);
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

		/// <summary>
		/// Visitation function for AddFloat.
		/// </summary>
		/// <param name="node">The node.</param>
		private void AddFloatR4(InstructionNode node)
		{
			Debug.Assert(node.Result.IsR4);
			Debug.Assert(node.Operand1.IsR4);

			node.ReplaceInstruction(X86.Addss);
		}

		/// <summary>
		/// Visitation function for AddFloat.
		/// </summary>
		/// <param name="node">The node.</param>
		private void AddFloatR8(InstructionNode node)
		{
			Debug.Assert(node.Result.IsR8);
			Debug.Assert(node.Operand1.IsR8);

			node.ReplaceInstruction(X86.Addsd);
		}

		/// <summary>
		/// Addresses the of instruction.
		/// </summary>
		/// <param name="node">The node.</param>
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

		/// <summary>
		/// Arithmetic the shift right instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void ArithmeticShiftRight32(InstructionNode node)
		{
			node.ReplaceInstruction(X86.Sar32);
		}

		/// <summary>
		/// Visitation function for BreakInstruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Break(InstructionNode node)
		{
			node.SetInstruction(X86.Break);
		}

		/// <summary>
		/// Visitation function for direct call instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void CallDirect(InstructionNode node)
		{
			Debug.Assert(node.Operand1 != null);

			if (node.Operand1.IsConstant)
				node.ReplaceInstruction(X86.CallStatic);
			else if (node.Operand1.IsVirtualRegister)
				node.ReplaceInstruction(X86.CallReg);
			else throw new NotSupportedException();
		}

		/// <summary>
		/// Floating point compare instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void CompareFloatR4(Context context)
		{
			FloatCompare(context, X86.Ucomiss);
		}

		/// <summary>
		/// Floating point compare instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void CompareFloatR8(Context context)
		{
			FloatCompare(context, X86.Ucomisd);
		}

		/// <summary>
		/// Visitation function for IntegerCompareInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
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
		}

		/// <summary>
		/// Visitation function for IntegerCompareBranchInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void CompareIntegerBranch(Context context)
		{
			var target = context.BranchTargets[0];
			var condition = context.ConditionCode;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			var branch = GetBranch(condition);

			context.SetInstruction(X86.Cmp32, null, operand1, operand2);
			context.AppendInstruction(branch, target);
		}

		private void LoadCompound(Context context)
		{
			CopyCompound(context, context.Result.Type, StackFrame, context.Result, context.Operand1, context.Operand2);
		}

		private void LoadParameterCompound(Context context)
		{
			CopyCompound(context, context.Result.Type, StackFrame, context.Result, StackFrame, context.Operand1);
		}

		private void MoveCompound(Context context)
		{
			CopyCompound(context, context.Result.Type, StackFrame, context.Result, StackFrame, context.Operand1);
		}

		private void StoreCompound(Context context)
		{
			CopyCompound(context, context.Operand3.Type, context.Operand1, context.Operand2, StackFrame, context.Operand3);
		}

		private void StoreParameterCompound(Context context)
		{
			CopyCompound(context, context.Operand2.Type, StackFrame, context.Operand1, StackFrame, context.Operand2);
		}

		private void CopyCompound(Context context, MosaType type, Operand destinationBase, Operand destination, Operand sourceBase, Operand source)
		{
			context.Empty();
			Architecture.InsertCompoundCopy(MethodCompiler, context, destinationBase, destination, sourceBase, source, TypeLayout.GetTypeSize(type));
		}

		/// <summary>
		/// Visitation function for MoveInteger instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void ConversionFloatR4ToFloatR8(InstructionNode node)
		{
			node.ReplaceInstruction(X86.Cvtss2sd);
		}

		/// <summary>
		/// Visitation function for ConversionFloatR4ToInteger.
		/// </summary>
		/// <param name="node">The node.</param>
		private void ConversionFloatR4ToInteger(InstructionNode node)
		{
			Debug.Assert(node.Result.Type.IsI1 || node.Result.Type.IsI2 || node.Result.Type.IsI4);
			node.ReplaceInstruction(X86.Cvttss2si);
		}

		/// <summary>
		/// Visitation function for ConversionFloatR8ToFloatR4 instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void ConversionFloatR8ToFloatR4(InstructionNode node)
		{
			node.ReplaceInstruction(X86.Cvtsd2ss);
		}

		/// <summary>
		/// Visitation function for ConversionFloatR8ToInteger.
		/// </summary>
		/// <param name="node">The node.</param>
		private void ConversionFloatR8ToInteger(InstructionNode node)
		{
			Debug.Assert(node.Result.Type.IsI1 || node.Result.Type.IsI2 || node.Result.Type.IsI4);
			node.ReplaceInstruction(X86.Cvttsd2si);
		}

		/// <summary>
		/// Visitation function for ConversionIntegerToFloatR4.
		/// </summary>
		/// <param name="node">The node.</param>
		private void ConversionIntegerToFloatR4(InstructionNode node)
		{
			Debug.Assert(node.Result.IsR4);
			node.ReplaceInstruction(X86.Cvtsi2ss);
		}

		/// <summary>
		/// Visitation function for ConversionIntegerToFloatR8.
		/// </summary>
		/// <param name="node">The node.</param>
		private void ConversionIntegerToFloatR8(InstructionNode node)
		{
			Debug.Assert(node.Result.IsR8);
			node.ReplaceInstruction(X86.Cvtsi2sd);
		}

		/// <summary>
		/// Visitation function for DivFloatR4.
		/// </summary>
		/// <param name="node">The node.</param>
		private void DivFloatR4(InstructionNode node)
		{
			Debug.Assert(node.Result.IsR4);
			Debug.Assert(node.Operand1.IsR4);

			node.ReplaceInstruction(X86.Divss);
		}

		/// <summary>
		/// Visitation function for DivFloatR8.
		/// </summary>
		/// <param name="node">The node.</param>
		private void DivFloatR8(InstructionNode node)
		{
			Debug.Assert(node.Result.IsR8);
			Debug.Assert(node.Operand1.IsR8);

			node.ReplaceInstruction(X86.Divsd);
		}

		/// <summary>
		/// Visitation function for DivSigned.
		/// </summary>
		/// <param name="context">The context.</param>
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

		/// <summary>
		/// Visitation function for DivUnsigned.
		/// </summary>
		/// <param name="context">The context.</param>
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

		/// <summary>
		/// Floating point compare instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="instruction">The instruction.</param>
		/// <param name="size">The size.</param>
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

		/// <summary>
		/// Visitation function for JmpInstruction instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Jmp(InstructionNode node)
		{
			if (node.Operand1 == null)
				node.ReplaceInstruction(X86.Jmp);
			else
				node.ReplaceInstruction(X86.JmpStatic); // FUTURE: Add IR.JumpStatic
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

		public static BaseInstruction GetMovLoad(InstructionSize size)
		{
			switch (size)
			{
				case InstructionSize.Size32: return X86.MovLoad32;
				case InstructionSize.None: return X86.MovLoad32;
				case InstructionSize.Native: return X86.MovLoad32;
				case InstructionSize.Size16: return X86.MovLoad16;
				case InstructionSize.Size8: return X86.MovLoad8;
				default: throw new NotSupportedException();
			}
		}

		public static BaseInstruction GetMovStore(InstructionSize size)
		{
			switch (size)
			{
				case InstructionSize.Size32: return X86.MovStore32;
				case InstructionSize.None: return X86.MovStore32;
				case InstructionSize.Native: return X86.MovStore32;
				case InstructionSize.Size16: return X86.MovStore16;
				case InstructionSize.Size8: return X86.MovStore8;
				default: throw new NotSupportedException();
			}
		}

		private void LoadParameterInteger(InstructionNode node)
		{
			Debug.Assert(!node.Result.IsR4);
			Debug.Assert(!node.Result.IsR8);

			var movLoad = GetMovLoad(node.Size);

			node.SetInstruction(movLoad, node.Result, StackFrame, node.Operand1);
		}

		private void LoadParameterSignExtended(InstructionNode node)
		{
			Debug.Assert(node.Size == InstructionSize.Size8 || node.Size == InstructionSize.Size16);

			if (node.Size == InstructionSize.Size8)
			{
				node.SetInstruction(X86.MovsxLoad8, node.Result, StackFrame, node.Operand1);
			}
			else if (node.Size == InstructionSize.Size16)
			{
				node.SetInstruction(X86.MovsxLoad16, node.Result, StackFrame, node.Operand1);
			}
		}

		private void LoadParameterZeroExtended(InstructionNode node)
		{
			Debug.Assert(node.Size == InstructionSize.Size8 || node.Size == InstructionSize.Size16);

			if (node.Size == InstructionSize.Size8)
			{
				node.SetInstruction(X86.MovzxLoad8, node.Result, StackFrame, node.Operand1);
			}
			else if (node.Size == InstructionSize.Size16)
			{
				node.SetInstruction(X86.MovzxLoad16, node.Result, StackFrame, node.Operand1);
			}
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

		private void LoadInteger(InstructionNode node)
		{
			Debug.Assert(!node.Result.IsR4);
			Debug.Assert(!node.Result.IsR8);

			LoadStore.OrderLoadOperands(node, MethodCompiler);

			var movLoad = GetMovLoad(node.Size);

			node.SetInstruction(movLoad, node.Result, node.Operand1, node.Operand2);
		}

		/// <summary>
		/// Visitation function for LoadSignExtended instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void LoadSignExtended(InstructionNode node)
		{
			Debug.Assert(node.Size == InstructionSize.Size8 || node.Size == InstructionSize.Size16);

			LoadStore.OrderLoadOperands(node, MethodCompiler);

			if (node.Size == InstructionSize.Size8)
			{
				node.SetInstruction(X86.MovsxLoad8, node.Result, node.Operand1, node.Operand2);
			}
			else if (node.Size == InstructionSize.Size16)
			{
				node.SetInstruction(X86.MovsxLoad16, node.Result, node.Operand1, node.Operand2);
			}
		}

		/// <summary>
		/// Visitation function for LoadZeroExtended instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void LoadZeroExtended(InstructionNode node)
		{
			Debug.Assert(node.Size == InstructionSize.Size8 || node.Size == InstructionSize.Size16);

			LoadStore.OrderLoadOperands(node, MethodCompiler);

			if (node.Size == InstructionSize.Size8)
			{
				node.SetInstruction(X86.MovzxLoad8, node.Result, node.Operand1, node.Operand2);
			}
			else if (node.Size == InstructionSize.Size16)
			{
				node.SetInstruction(X86.MovzxLoad16, node.Result, node.Operand1, node.Operand2);
			}
		}

		/// <summary>
		/// Visitation function for LogicalAnd instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void LogicalAnd32(InstructionNode node)
		{
			node.ReplaceInstruction(X86.And32);
		}

		/// <summary>
		/// Visitation function for LogicalNot instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void LogicalNot32(Context context)
		{
			var dest = context.Result;

			context.SetInstruction(X86.Mov32, dest, context.Operand1);
			context.AppendInstruction(X86.Not32, dest, dest);
		}

		/// <summary>
		/// Visitation function for LogicalOr instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void LogicalOr32(InstructionNode node)
		{
			node.ReplaceInstruction(X86.Or32);
		}

		/// <summary>
		/// Visitation function for LogicalXor instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void LogicalXor32(InstructionNode node)
		{
			node.ReplaceInstruction(X86.Xor32);
		}

		/// <summary>
		/// Visitation function for MoveFloatR4 instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void MoveFloatR4(InstructionNode node)
		{
			node.ReplaceInstruction(X86.Movss);
		}

		/// <summary>
		/// Visitation function for MoveFloatR8 instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void MoveFloatR8(InstructionNode node)
		{
			node.ReplaceInstruction(X86.Movsd);
		}

		/// <summary>
		/// Visitation function for MoveInteger instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void MoveInteger(InstructionNode node)
		{
			node.ReplaceInstruction(X86.Mov32);
		}

		/// <summary>
		/// Visitation function for MulFloat.
		/// </summary>
		/// <param name="node">The node.</param>
		private void MulFloatR4(InstructionNode node)
		{
			Debug.Assert(node.Result.IsR4);
			Debug.Assert(node.Operand1.IsR4);

			node.ReplaceInstruction(X86.Mulss);
		}

		/// <summary>
		/// Visitation function for MulFloat.
		/// </summary>
		/// <param name="node">The node.</param>
		private void MulFloatR8(InstructionNode node)
		{
			Debug.Assert(node.Result.IsR8);
			Debug.Assert(node.Operand1.IsR8);

			node.ReplaceInstruction(X86.Mulsd);
		}

		/// <summary>
		/// Visitation function for MulSigned.
		/// </summary>
		/// <param name="node">The node.</param>
		private void MulSigned32(InstructionNode node)
		{
			Operand result = node.Result;
			Operand operand1 = node.Operand1;
			Operand operand2 = node.Operand2;

			Operand v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);
			node.SetInstruction2(X86.Mul32, v1, result, operand1, operand2);
		}

		/// <summary>
		/// Visitation function for MulUnsigned.
		/// </summary>
		/// <param name="node">The node.</param>
		private void MulUnsigned32(InstructionNode node)
		{
			Operand result = node.Result;
			Operand operand1 = node.Operand1;
			Operand operand2 = node.Operand2;

			Operand v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);
			node.SetInstruction2(X86.Mul32, v1, result, operand1, operand2);
		}

		/// <summary>
		/// Visitation function for NopInstruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Nop(InstructionNode node)
		{
			node.SetInstruction(X86.Nop);
		}

		/// <summary>
		/// Visitation function for RemSigned.
		/// </summary>
		/// <param name="context">The context.</param>
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

		/// <summary>
		/// Visitation function for RemUnsigned.
		/// </summary>
		/// <param name="context">The context.</param>
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

		/// <summary>
		/// Visitation function for ShiftLeftInstruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void ShiftLeft32(InstructionNode node)
		{
			node.ReplaceInstruction(X86.Shl32);
		}

		/// <summary>
		/// Visitation function for ShiftRightInstruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void ShiftRight32(InstructionNode node)
		{
			node.ReplaceInstruction(X86.Shr32);
		}

		/// <summary>
		/// Visitation function for SignExtendedMove instructions.
		/// </summary>
		/// <param name="node">The node.</param>
		private void SignExtendedMove(InstructionNode node)
		{
			Debug.Assert(node.Size != InstructionSize.Size32);

			X86Instruction Movsx = (node.Size == InstructionSize.Size8) ? X86.Movsx8To32 : (X86Instruction)X86.Movsx16To32;

			node.ReplaceInstruction(Movsx);
		}

		private void StoreFloatR4(InstructionNode node)
		{
			node.SetInstruction(X86.MovssStore, null, node.Operand1, node.Operand2, node.Operand3);
		}

		private void StoreFloatR8(InstructionNode node)
		{
			node.SetInstruction(X86.MovsdStore, null, node.Operand1, node.Operand2, node.Operand3);
		}

		private void StoreInteger8(InstructionNode node)
		{
			LoadStore.OrderStoreOperands(node, MethodCompiler);

			node.SetInstruction(X86.MovStore8, null, node.Operand1, node.Operand2, node.Operand3);
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

		private void StoreParameterFloatR4(InstructionNode node)
		{
			node.SetInstruction(X86.MovssStore, null, StackFrame, node.Operand1, node.Operand2);
		}

		private void StoreParameterFloatR8(InstructionNode node)
		{
			node.SetInstruction(X86.MovsdStore, null, StackFrame, node.Operand1, node.Operand2);
		}

		private void StoreParameterInteger8(InstructionNode node)
		{
			node.SetInstruction(X86.MovStore8, null, StackFrame, node.Operand1, node.Operand2);
		}

		private void StoreParameterInteger16(InstructionNode node)
		{
			node.SetInstruction(X86.MovStore16, null, StackFrame, node.Operand1, node.Operand2);
		}

		private void StoreParameterInteger32(InstructionNode node)
		{
			node.SetInstruction(X86.MovStore32, null, StackFrame, node.Operand1, node.Operand2);
		}

		/// <summary>
		/// Visitation function for SubFloat.
		/// </summary>
		/// <param name="node">The node.</param>
		private void SubFloatR4(InstructionNode node)
		{
			Debug.Assert(node.Result.IsR4);
			Debug.Assert(node.Operand1.IsR4);

			node.ReplaceInstruction(X86.Subss);
		}

		/// <summary>
		/// Visitation function for SubFloat.
		/// </summary>
		/// <param name="node">The node.</param>
		private void SubFloatR8(InstructionNode node)
		{
			Debug.Assert(node.Result.IsR8);
			Debug.Assert(node.Operand1.IsR8);

			node.ReplaceInstruction(X86.Subsd);
		}

		/// <summary>
		/// Visitation function for SubSigned.
		/// </summary>
		/// <param name="node">The node.</param>
		private void SubSigned32(InstructionNode node)
		{
			node.ReplaceInstruction(X86.Sub32);
		}

		/// <summary>
		/// Visitation function for SubUnsigned.
		/// </summary>
		/// <param name="node">The node.</param>
		private void SubUnsigned32(InstructionNode node)
		{
			node.ReplaceInstruction(X86.Sub32);
		}

		/// <summary>
		/// Visitation function for SwitchInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
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

		/// <summary>
		/// Visitation function for ZeroExtendedMoveInstruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void ZeroExtendedMove(InstructionNode node)
		{
			Debug.Assert(node.Size != InstructionSize.None);
			Debug.Assert(node.Size != InstructionSize.Size32);

			X86Instruction Movzx = (node.Size == InstructionSize.Size8) ? X86.Movzx8To32 : (X86Instruction)X86.Movzx16To32;

			node.ReplaceInstruction(Movzx);
		}

		#endregion Visitation Methods

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
				case ConditionCode.Parity: return X86.SetParity;
				case ConditionCode.NoParity: return X86.SetNoParity;
				case ConditionCode.LessThan: return X86.SetLessThan;
				case ConditionCode.GreaterOrEqual: return X86.SetGreaterOrEqual;
				case ConditionCode.LessOrEqual: return X86.SetLessOrEqual;
				case ConditionCode.GreaterThan: return X86.SetGreaterThan;

				default: throw new NotSupportedException();
			}
		}

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
				case ConditionCode.Parity: return X86.BranchParity;
				case ConditionCode.NoParity: return X86.BranchNoParity;
				case ConditionCode.LessThan: return X86.BranchLessThan;
				case ConditionCode.GreaterOrEqual: return X86.BranchGreaterOrEqual;
				case ConditionCode.LessOrEqual: return X86.BranchLessOrEqual;
				case ConditionCode.GreaterThan: return X86.BranchGreaterThan;

				default: throw new NotSupportedException();
			}
		}
	}
}
