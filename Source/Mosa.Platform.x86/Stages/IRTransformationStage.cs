// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.MosaTypeSystem;
using System.Diagnostics;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	/// Transforms IR instructions into their appropriate X86.
	/// </summary>
	/// <remarks>
	/// This transformation stage transforms IR instructions into their equivalent X86 sequences.
	/// </remarks>
	public sealed class IRTransformationStage : BaseTransformationStage
	{
		private const int LargeAlignment = 16;

		protected override void PopulateVisitationDictionary()
		{
			visitationDictionary[IRInstruction.AddFloatR4] = AddFloatR4;
			visitationDictionary[IRInstruction.AddFloatR8] = AddFloatR8;
			visitationDictionary[IRInstruction.AddressOf] = AddressOf;
			visitationDictionary[IRInstruction.AddSigned] = AddSigned;
			visitationDictionary[IRInstruction.AddUnsigned] = AddUnsigned;
			visitationDictionary[IRInstruction.ArithmeticShiftRight] = ArithmeticShiftRight;
			visitationDictionary[IRInstruction.Break] = Break;
			visitationDictionary[IRInstruction.Call] = Call;
			visitationDictionary[IRInstruction.CompareFloatR4] = CompareFloatR4;
			visitationDictionary[IRInstruction.CompareFloatR8] = CompareFloatR8;
			visitationDictionary[IRInstruction.CompareInteger] = CompareInteger;
			visitationDictionary[IRInstruction.CompareIntegerBranch] = CompareIntegerBranch;
			visitationDictionary[IRInstruction.LoadCompound] = LoadCompound;
			visitationDictionary[IRInstruction.MoveCompound] = MoveCompound;
			visitationDictionary[IRInstruction.StoreCompound] = StoreCompound;
			visitationDictionary[IRInstruction.ConversionFloatR4ToFloatR8] = ConversionFloatR4ToFloatR8;
			visitationDictionary[IRInstruction.ConversionFloatR4ToInteger] = ConversionFloatR4ToInteger;
			visitationDictionary[IRInstruction.ConversionFloatR8ToFloatR4] = ConversionFloatR8ToFloatR4;
			visitationDictionary[IRInstruction.ConversionFloatR8ToInteger] = ConversionFloatR8ToInteger;
			visitationDictionary[IRInstruction.ConversionIntegerToFloatR4] = ConversionIntegerToFloatR4;
			visitationDictionary[IRInstruction.ConversionIntegerToFloatR8] = ConversionIntegerToFloatR8;
			visitationDictionary[IRInstruction.DivFloatR4] = DivFloatR4;
			visitationDictionary[IRInstruction.DivFloatR8] = DivFloatR8;
			visitationDictionary[IRInstruction.DivSigned] = DivSigned;
			visitationDictionary[IRInstruction.DivUnsigned] = DivUnsigned;
			visitationDictionary[IRInstruction.InternalCall] = InternalCall;
			visitationDictionary[IRInstruction.InternalReturn] = InternalReturn;
			visitationDictionary[IRInstruction.Jmp] = Jmp;
			visitationDictionary[IRInstruction.LoadFloatR4] = LoadFloatR4;
			visitationDictionary[IRInstruction.LoadFloatR8] = LoadFloatR8;
			visitationDictionary[IRInstruction.LoadInteger] = LoadInteger;
			visitationDictionary[IRInstruction.LoadSignExtended] = LoadSignExtended;
			visitationDictionary[IRInstruction.LoadZeroExtended] = LoadZeroExtended;
			visitationDictionary[IRInstruction.LoadParameterFloatR4] = LoadParameterFloatR4;
			visitationDictionary[IRInstruction.LoadParameterFloatR8] = LoadParameterFloatR8;
			visitationDictionary[IRInstruction.LoadParameterInteger] = LoadParameterInteger;
			visitationDictionary[IRInstruction.LoadParameterSignExtended] = LoadParameterSignExtended;
			visitationDictionary[IRInstruction.LoadParameterZeroExtended] = LoadParameterZeroExtended;
			visitationDictionary[IRInstruction.LogicalAnd] = LogicalAnd;
			visitationDictionary[IRInstruction.LogicalNot] = LogicalNot;
			visitationDictionary[IRInstruction.LogicalOr] = LogicalOr;
			visitationDictionary[IRInstruction.LogicalXor] = LogicalXor;
			visitationDictionary[IRInstruction.MoveFloatR4] = MoveFloatR4;
			visitationDictionary[IRInstruction.MoveFloatR8] = MoveFloatR8;
			visitationDictionary[IRInstruction.MoveInteger] = MoveInteger;
			visitationDictionary[IRInstruction.MoveSignExtended] = SignExtendedMove;
			visitationDictionary[IRInstruction.MoveZeroExtended] = ZeroExtendedMove;
			visitationDictionary[IRInstruction.MulFloatR4] = MulFloatR4;
			visitationDictionary[IRInstruction.MulFloatR8] = MulFloatR8;
			visitationDictionary[IRInstruction.MulSigned] = MulSigned;
			visitationDictionary[IRInstruction.MulUnsigned] = MulUnsigned;
			visitationDictionary[IRInstruction.Nop] = Nop;
			visitationDictionary[IRInstruction.RemFloatR4] = RemFloatR4;
			visitationDictionary[IRInstruction.RemFloatR8] = RemFloatR8;
			visitationDictionary[IRInstruction.RemSigned] = RemSigned;
			visitationDictionary[IRInstruction.RemUnsigned] = RemUnsigned;
			visitationDictionary[IRInstruction.Return] = Return;
			visitationDictionary[IRInstruction.ShiftLeft] = ShiftLeft;
			visitationDictionary[IRInstruction.ShiftRight] = ShiftRight;
			visitationDictionary[IRInstruction.StoreFloatR4] = StoreFloatR4;
			visitationDictionary[IRInstruction.StoreFloatR8] = StoreFloatR8;
			visitationDictionary[IRInstruction.StoreInteger] = StoreInt;
			visitationDictionary[IRInstruction.SubFloatR4] = SubFloatR4;
			visitationDictionary[IRInstruction.SubFloatR8] = SubFloatR8;
			visitationDictionary[IRInstruction.SubSigned] = SubSigned;
			visitationDictionary[IRInstruction.SubUnsigned] = SubUnsigned;
			visitationDictionary[IRInstruction.Switch] = Switch;
		}

		#region Visitation Methods

		/// <summary>
		/// Visitation function for AddFloat.
		/// </summary>
		/// <param name="context">The context.</param>
		private void AddFloatR4(Context context)
		{
			Debug.Assert(context.Result.IsR4);
			Debug.Assert(context.Operand1.IsR4);

			context.ReplaceInstructionOnly(X86.Addss);
			context.Size = InstructionSize.Size32;
		}

		/// <summary>
		/// Visitation function for AddFloat.
		/// </summary>
		/// <param name="context">The context.</param>
		private void AddFloatR8(Context context)
		{
			Debug.Assert(context.Result.IsR8);
			Debug.Assert(context.Operand1.IsR8);

			context.ReplaceInstructionOnly(X86.Addsd);
			context.Size = InstructionSize.Size32;
		}

		/// <summary>
		/// Addresses the of instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void AddressOf(Context context)
		{
			Debug.Assert(context.Operand1.IsOnStack | context.Operand1.IsStaticField);

			if (context.Operand1.IsStaticField)
			{
				context.SetInstruction(X86.Mov, NativeInstructionSize, context.Result, context.Operand1);
				return;
			}

			if (context.Operand1.IsStackLocal)
			{
				context.SetInstruction(X86.Lea, NativeInstructionSize, context.Result, StackFrame, context.Operand1);
			}
			else
			{
				var offset = Operand.CreateConstant(TypeSystem, context.Operand1.Offset);

				context.SetInstruction(X86.Lea, NativeInstructionSize, context.Result, StackFrame, offset);
			}
		}

		/// <summary>
		/// Visitation function for AddSigned.
		/// </summary>
		/// <param name="context">The context.</param>
		private void AddSigned(Context context)
		{
			context.ReplaceInstructionOnly(X86.Add);
		}

		/// <summary>
		/// Visitation function for AddUnsigned.
		/// </summary>
		/// <param name="context">The context.</param>
		private void AddUnsigned(Context context)
		{
			context.ReplaceInstructionOnly(X86.Add);
		}

		/// <summary>
		/// Arithmetic the shift right instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ArithmeticShiftRight(Context context)
		{
			context.ReplaceInstructionOnly(X86.Sar);
		}

		/// <summary>
		/// Visitation function for BreakInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Break(Context context)
		{
			context.SetInstruction(X86.Break);
		}

		/// <summary>
		/// Visitation function for Call.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Call(Context context)
		{
			if (context.OperandCount == 0 && context.BranchTargets != null)
			{
				// inter-method call; usually for exception processing
				context.ReplaceInstructionOnly(X86.Call);
			}
			else
			{
				CallingConvention.MakeCall(MethodCompiler, context);
			}
		}

		/// <summary>
		/// Floating point compare instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void CompareFloatR4(Context context)
		{
			FloatCompare(context, X86.Ucomiss, InstructionSize.Size32);
		}

		/// <summary>
		/// Floating point compare instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void CompareFloatR8(Context context)
		{
			FloatCompare(context, X86.Ucomisd, InstructionSize.Size64);
		}

		/// <summary>
		/// Visitation function for IntegerCompareInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void CompareInteger(Context context)
		{
			var condition = context.ConditionCode;
			var resultOperand = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			Operand v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			context.SetInstruction(X86.Mov, v1, ConstantZero);

			context.AppendInstruction(X86.Cmp, null, operand1, operand2);

			if (resultOperand.IsUnsigned || resultOperand.IsChar)
			{
				context.AppendInstruction(X86.Setcc, condition.GetUnsigned(), v1);
			}
			else
			{
				context.AppendInstruction(X86.Setcc, condition, v1);
			}

			context.AppendInstruction(X86.Mov, resultOperand, v1);
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

			context.SetInstruction(X86.Cmp, null, operand1, operand2);
			context.AppendInstruction(X86.Branch, condition, target);
		}

		private void LoadCompound(Context context)
		{
			var type = context.Result.Type;
			int typeSize = TypeLayout.GetTypeSize(type);
			int alignedTypeSize = typeSize - (typeSize % NativeAlignment);
			int largeAlignedTypeSize = typeSize - (typeSize % LargeAlignment);
			Debug.Assert(typeSize > 0);

			var dest = context.Result;
			var src = context.Operand1;
			var srcOffset = context.Operand2;

			var srcReg = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var dstReg = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			context.SetInstruction(IRInstruction.UnstableObjectTracking);

			context.AppendInstruction(X86.Lea, srcReg, src, srcOffset);
			context.AppendInstruction(X86.Lea, dstReg, StackFrame, dest);

			var tmp = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var tmpLarge = AllocateVirtualRegister(TypeSystem.BuiltIn.R8);

			for (int i = 0; i < largeAlignedTypeSize; i += LargeAlignment)
			{
				// Large Aligned moves 128bits at a time
				var index = Operand.CreateConstant(TypeSystem.BuiltIn.I4, i);

				//var offset2 = Operand.CreateConstant(TypeSystem.BuiltIn.I4, i);
				context.AppendInstruction(X86.MovupsLoad, tmpLarge, srcReg, index);
				context.AppendInstruction(X86.MovupsStore, null, dstReg, index, tmpLarge);
			}
			for (int i = largeAlignedTypeSize; i < alignedTypeSize; i += NativeAlignment)
			{
				var index = Operand.CreateConstant(TypeSystem.BuiltIn.I4, i);
				var offset2 = Operand.CreateConstant(TypeSystem.BuiltIn.I4, i);
				context.AppendInstruction(X86.MovLoad, InstructionSize.Size32, tmp, srcReg, offset2);
				context.AppendInstruction(X86.MovStore, InstructionSize.Size32, null, dstReg, index, tmp);
			}
			for (int i = alignedTypeSize; i < typeSize; i++)
			{
				var index = Operand.CreateConstant(TypeSystem.BuiltIn.I4, i);
				var offset2 = Operand.CreateConstant(TypeSystem.BuiltIn.I4, i);
				context.AppendInstruction(X86.MovzxLoad, InstructionSize.Size8, tmp, srcReg, offset2);
				context.AppendInstruction(X86.MovStore, InstructionSize.Size8, null, dstReg, index, tmp);
			}

			context.AppendInstruction(IRInstruction.StableObjectTracking);
		}

		private void MoveCompound(Context context)
		{
			var src = context.Operand1;
			var dest = context.Result;
			var type = context.Result.Type;
			int typeSize = TypeLayout.GetTypeSize(type);
			int alignedTypeSize = typeSize - (typeSize % NativeAlignment);
			int largeAlignedTypeSize = typeSize - (typeSize % LargeAlignment);

			Debug.Assert(typeSize > 0);
			Debug.Assert(dest.IsOnStack);
			Debug.Assert(src.IsOnStack);

			var srcReg = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var dstReg = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			context.SetInstruction(IRInstruction.UnstableObjectTracking);

			context.AppendInstruction(X86.Lea, srcReg, StackFrame, src);
			context.AppendInstruction(X86.Lea, dstReg, StackFrame, dest);

			var tmp = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var tmpLarge = AllocateVirtualRegister(TypeSystem.BuiltIn.R8);

			for (int i = 0; i < largeAlignedTypeSize; i += LargeAlignment)
			{
				// Large Aligned moves 128bits at a time
				var index = Operand.CreateConstant(TypeSystem.BuiltIn.I4, i);
				context.AppendInstruction(X86.MovupsLoad, tmpLarge, srcReg, index);
				context.AppendInstruction(X86.MovupsStore, null, dstReg, index, tmpLarge);
			}
			for (int i = largeAlignedTypeSize; i < alignedTypeSize; i += NativeAlignment)
			{
				var index = Operand.CreateConstant(TypeSystem.BuiltIn.I4, i);
				context.AppendInstruction(X86.MovLoad, InstructionSize.Size32, tmp, srcReg, index);
				context.AppendInstruction(X86.MovStore, InstructionSize.Size32, null, dstReg, index, tmp);
			}
			for (int i = alignedTypeSize; i < typeSize; i++)
			{
				var index = Operand.CreateConstant(TypeSystem.BuiltIn.I4, i);
				context.AppendInstruction(X86.MovzxLoad, InstructionSize.Size8, tmp, srcReg, index);
				context.AppendInstruction(X86.MovStore, InstructionSize.Size8, null, dstReg, index, tmp);
			}

			context.AppendInstruction(IRInstruction.StableObjectTracking);
		}

		private void StoreCompound(Context context)
		{
			var type = context.Operand3.Type;
			int typeSize = TypeLayout.GetTypeSize(type);
			int alignedTypeSize = typeSize - (typeSize % NativeAlignment);
			int largeAlignedTypeSize = typeSize - (typeSize % LargeAlignment);

			Debug.Assert(typeSize > 0);

			var src = context.Operand3;
			var dest = context.Operand1;
			var destOffset = context.Operand2;

			var srcReg = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var dstReg = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			Debug.Assert(src.IsStackLocal);

			context.SetInstruction(IRInstruction.UnstableObjectTracking);

			context.AppendInstruction(X86.Lea, srcReg, StackFrame, src);
			context.AppendInstruction(X86.Lea, dstReg, dest, destOffset);

			var tmp = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var tmpLarge = AllocateVirtualRegister(TypeSystem.BuiltIn.R8);

			for (int i = 0; i < largeAlignedTypeSize; i += LargeAlignment)
			{
				// Large Aligned moves 128bits at a time
				var index = Operand.CreateConstant(TypeSystem.BuiltIn.I4, i);
				var indexOffset = Operand.CreateConstant(TypeSystem.BuiltIn.I4, i);
				context.AppendInstruction(X86.MovupsLoad, InstructionSize.Size128, tmpLarge, srcReg, index);
				context.AppendInstruction(X86.MovupsStore, InstructionSize.Size128, null, dstReg, indexOffset, tmpLarge);
			}
			for (int i = largeAlignedTypeSize; i < alignedTypeSize; i += NativeAlignment)
			{
				var index = Operand.CreateConstant(TypeSystem.BuiltIn.I4, i);
				var indexOffset = Operand.CreateConstant(TypeSystem.BuiltIn.I4, i);
				context.AppendInstruction(X86.MovLoad, InstructionSize.Size32, tmp, srcReg, index);
				context.AppendInstruction(X86.MovStore, InstructionSize.Size32, null, dstReg, indexOffset, tmp);
			}
			for (int i = alignedTypeSize; i < typeSize; i++)
			{
				var index = Operand.CreateConstant(TypeSystem.BuiltIn.I4, i);
				var indexOffset = Operand.CreateConstant(TypeSystem.BuiltIn.I4, i);
				context.AppendInstruction(X86.MovzxLoad, InstructionSize.Size8, tmp, srcReg, index);
				context.AppendInstruction(X86.MovStore, InstructionSize.Size8, null, dstReg, indexOffset, tmp);
			}

			context.AppendInstruction(IRInstruction.StableObjectTracking);
		}

		/// <summary>
		/// Visitation function for MoveInteger instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ConversionFloatR4ToFloatR8(Context context)
		{
			context.ReplaceInstructionOnly(X86.Cvtss2sd);
		}

		/// <summary>
		/// Visitation function for FloatingPointToIntegerConversionInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ConversionFloatR4ToInteger(Context context)
		{
			Debug.Assert(context.Result.Type.IsI1 || context.Result.Type.IsI2 || context.Result.Type.IsI4);

			context.ReplaceInstructionOnly(X86.Cvttss2si);
		}

		/// <summary>
		/// Visitation function for MoveInteger instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ConversionFloatR8ToFloatR4(Context context)
		{
			context.ReplaceInstructionOnly(X86.Cvtsd2ss);
		}

		/// <summary>
		/// Visitation function for FloatingPointToIntegerConversionInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ConversionFloatR8ToInteger(Context context)
		{
			Debug.Assert(context.Result.Type.IsI1 || context.Result.Type.IsI2 || context.Result.Type.IsI4);

			context.ReplaceInstructionOnly(X86.Cvttsd2si);
		}

		/// <summary>
		/// Visitation function for IntegerToFloatR4Conversion.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ConversionIntegerToFloatR4(Context context)
		{
			Debug.Assert(context.Result.IsR4);
			context.ReplaceInstructionOnly(X86.Cvtsi2ss);
		}

		/// <summary>
		/// Visitation function for IntegerToFloatR8Conversion.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ConversionIntegerToFloatR8(Context context)
		{
			Debug.Assert(context.Result.IsR8);
			context.ReplaceInstructionOnly(X86.Cvtsi2sd);
		}

		/// <summary>
		/// Visitation function for DivFloat.
		/// </summary>
		/// <param name="context">The context.</param>
		private void DivFloatR4(Context context)
		{
			Debug.Assert(context.Result.IsR4);
			Debug.Assert(context.Operand1.IsR4);

			context.ReplaceInstructionOnly(X86.Divss);
			context.Size = InstructionSize.Size32;
		}

		/// <summary>
		/// Visitation function for DivFloat.
		/// </summary>
		/// <param name="context">The context.</param>
		private void DivFloatR8(Context context)
		{
			Debug.Assert(context.Result.IsR8);
			Debug.Assert(context.Operand1.IsR8);

			context.ReplaceInstructionOnly(X86.Divsd);
			context.Size = InstructionSize.Size32;
		}

		/// <summary>
		/// Visitation function for DivSigned.
		/// </summary>
		/// <param name="context">The context.</param>
		private void DivSigned(Context context)
		{
			Operand operand1 = context.Operand1;
			Operand operand2 = context.Operand2;
			Operand result = context.Result;

			Operand v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			Operand v2 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);
			Operand v3 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			context.SetInstruction2(X86.Cdq, v1, v2, operand1);
			context.AppendInstruction2(X86.IDiv, v3, result, v1, v2, operand2);
		}

		/// <summary>
		/// Visitation function for DivUnsigned.
		/// </summary>
		/// <param name="context">The context.</param>
		private void DivUnsigned(Context context)
		{
			Operand operand1 = context.Operand1;
			Operand operand2 = context.Operand2;
			Operand result = context.Result;

			Operand v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);
			Operand v2 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);

			context.SetInstruction(X86.Mov, v1, ConstantZero);
			context.AppendInstruction2(X86.Div, v1, v2, v1, operand1, operand2);
			context.AppendInstruction(X86.Mov, result, v2);
		}

		/// <summary>
		/// Floating point compare instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void FloatCompare(Context context, X86Instruction instruction, InstructionSize size)
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
						Context nextBlock = Split(context);

						context.SetInstruction(X86.Mov, result, Operand.CreateConstant(TypeSystem, 1));
						context.AppendInstruction(instruction, size, null, left, right);
						context.AppendInstruction(X86.Branch, ConditionCode.Parity, newBlocks[1].Block);
						context.AppendInstruction(X86.Jmp, newBlocks[0].Block);

						newBlocks[0].AppendInstruction(X86.Branch, ConditionCode.NotEqual, newBlocks[1].Block);
						newBlocks[0].AppendInstruction(X86.Jmp, nextBlock.Block);

						newBlocks[1].AppendInstruction(X86.Mov, result, ConstantZero);
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
						Context nextBlock = Split(context);

						context.SetInstruction(X86.Mov, result, Operand.CreateConstant(TypeSystem, 1));
						context.AppendInstruction(instruction, size, null, left, right);
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

						context.SetInstruction(X86.Mov, result, ConstantZero);
						context.AppendInstruction(instruction, size, null, right, left);
						context.AppendInstruction(X86.Setcc, ConditionCode.UnsignedGreaterThan, result);
						break;
					}
				case ConditionCode.GreaterThan:
					{
						//	a>b
						//	mov	eax, 0
						//	ucomisd	xmm0, xmm1
						//	seta	al

						context.SetInstruction(X86.Mov, result, ConstantZero);
						context.AppendInstruction(instruction, size, null, left, right);
						context.AppendInstruction(X86.Setcc, ConditionCode.UnsignedGreaterThan, result);
						break;
					}
				case ConditionCode.LessOrEqual:
					{
						//	a<=b
						//	mov	eax, 0
						//	ucomisd	xmm1, xmm0
						//	setae	al

						context.SetInstruction(X86.Mov, result, ConstantZero);
						context.AppendInstruction(instruction, size, null, right, left);
						context.AppendInstruction(X86.Setcc, ConditionCode.UnsignedGreaterOrEqual, result);
						break;
					}
				case ConditionCode.GreaterOrEqual:
					{
						//	a>=b
						//	mov	eax, 0
						//	ucomisd	xmm0, xmm1
						//	setae	al

						context.SetInstruction(X86.Mov, result, ConstantZero);
						context.AppendInstruction(instruction, size, null, left, right);
						context.AppendInstruction(X86.Setcc, ConditionCode.UnsignedGreaterOrEqual, result);
						break;
					}
			}
		}

		/// <summary>
		/// Visitation function for InternalCall.
		/// </summary>
		/// <param name="context">The context.</param>
		private void InternalCall(Context context)
		{
			context.ReplaceInstructionOnly(X86.Call);
		}

		/// <summary>
		/// Visitation function for InternalReturn.
		/// </summary>
		/// <param name="context">The context.</param>
		private void InternalReturn(Context context)
		{
			Debug.Assert(context.BranchTargets == null);

			// To return from an internal method call (usually from within a finally or exception clause)
			context.SetInstruction(X86.Ret);
		}

		/// <summary>
		/// Visitation function for JmpInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Jmp(Context context)
		{
			context.ReplaceInstructionOnly(X86.Jmp);
		}

		private void LoadParameterFloatR4(Context context)
		{
			Debug.Assert(context.Result.IsR4);

			context.SetInstruction(X86.MovssLoad, context.Size, context.Result, StackFrame, context.Operand1);
		}

		private void LoadParameterFloatR8(Context context)
		{
			Debug.Assert(context.Result.IsR8);

			context.SetInstruction(X86.MovsdLoad, context.Size, context.Result, StackFrame, context.Operand1);
		}

		private void LoadParameterInteger(Context context)
		{
			Debug.Assert(!context.Result.IsR4);
			Debug.Assert(!context.Result.IsR8);

			context.SetInstruction(X86.MovLoad, context.Size, context.Result, StackFrame, context.Operand1);
		}

		private void LoadParameterSignExtended(Context context)
		{
			Debug.Assert(context.Size == InstructionSize.Size8 || context.Size == InstructionSize.Size16);
			context.SetInstruction(X86.MovsxLoad, context.Size, context.Result, StackFrame, context.Operand1);
		}

		private void LoadParameterZeroExtended(Context context)
		{
			Debug.Assert(context.Size == InstructionSize.Size8 || context.Size == InstructionSize.Size16);
			context.SetInstruction(X86.MovzxLoad, context.Size, context.Result, StackFrame, context.Operand1);
		}

		private void LoadFloatR4(Context context)
		{
			Debug.Assert(context.Result.IsR4);

			context.SetInstruction(X86.MovssLoad, context.Size, context.Result, context.Operand1, context.Operand2);
		}

		private void LoadFloatR8(Context context)
		{
			Debug.Assert(context.Result.IsR8);

			context.SetInstruction(X86.MovsdLoad, context.Size, context.Result, context.Operand1, context.Operand2);
		}

		private void LoadInteger(Context context)
		{
			Debug.Assert(!context.Result.IsR4);
			Debug.Assert(!context.Result.IsR8);

			context.SetInstruction(X86.MovLoad, context.Size, context.Result, context.Operand1, context.Operand2);
		}

		/// <summary>
		/// Visitation function for Load Sign Extended.
		/// </summary>
		/// <param name="context">The context.</param>
		private void LoadSignExtended(Context context)
		{
			Debug.Assert(context.Size == InstructionSize.Size8 || context.Size == InstructionSize.Size16);
			context.SetInstruction(X86.MovsxLoad, context.Size, context.Result, context.Operand1, context.Operand2);
		}

		/// <summary>
		/// Visitation function for Load Zero Extended.
		/// </summary>
		/// <param name="context">The context.</param>
		private void LoadZeroExtended(Context context)
		{
			Debug.Assert(context.Size == InstructionSize.Size8 || context.Size == InstructionSize.Size16);
			context.SetInstruction(X86.MovzxLoad, context.Size, context.Result, context.Operand1, context.Operand2);
		}

		/// <summary>
		/// Visitation function for LogicalAndInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void LogicalAnd(Context context)
		{
			context.ReplaceInstructionOnly(X86.And);
		}

		/// <summary>
		/// Visitation function for LogicalNotInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void LogicalNot(Context context)
		{
			Operand dest = context.Result;

			context.SetInstruction(X86.Mov, dest, context.Operand1);

			if (dest.IsByte)
				context.AppendInstruction(X86.Xor, dest, dest, Operand.CreateConstant(TypeSystem, 0xFF));
			else if (dest.IsU2)
				context.AppendInstruction(X86.Xor, dest, dest, Operand.CreateConstant(TypeSystem, 0xFFFF));
			else
				context.AppendInstruction(X86.Not, dest, dest);
		}

		/// <summary>
		/// Visitation function for LogicalOrInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void LogicalOr(Context context)
		{
			context.ReplaceInstructionOnly(X86.Or);
		}

		/// <summary>
		/// Visitation function for LogicalXorInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void LogicalXor(Context context)
		{
			context.ReplaceInstructionOnly(X86.Xor);
		}

		/// <summary>
		/// Visitation function for MoveFloatR4 instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void MoveFloatR4(Context context)
		{
			context.ReplaceInstructionOnly(X86.Movss);
		}

		/// <summary>
		/// Visitation function for MoveFloatR8 instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void MoveFloatR8(Context context)
		{
			context.ReplaceInstructionOnly(X86.Movsd);
		}

		/// <summary>
		/// Visitation function for MoveInteger instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void MoveInteger(Context context)
		{
			context.ReplaceInstructionOnly(X86.Mov);
		}

		/// <summary>
		/// Visitation function for MulFloat.
		/// </summary>
		/// <param name="context">The context.</param>
		private void MulFloatR4(Context context)
		{
			Debug.Assert(context.Result.IsR4);
			Debug.Assert(context.Operand1.IsR4);

			context.ReplaceInstructionOnly(X86.Mulss);
			context.Size = InstructionSize.Size32;
		}

		/// <summary>
		/// Visitation function for MulFloat.
		/// </summary>
		/// <param name="context">The context.</param>
		private void MulFloatR8(Context context)
		{
			Debug.Assert(context.Result.IsR8);
			Debug.Assert(context.Operand1.IsR8);

			context.ReplaceInstructionOnly(X86.Mulsd);
			context.Size = InstructionSize.Size32;
		}

		/// <summary>
		/// Visitation function for MulSigned.
		/// </summary>
		/// <param name="context">The context.</param>
		private void MulSigned(Context context)
		{
			Operand result = context.Result;
			Operand operand1 = context.Operand1;
			Operand operand2 = context.Operand2;

			Operand v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);
			context.SetInstruction2(X86.Mul, v1, result, operand1, operand2);
		}

		/// <summary>
		/// Visitation function for MulUnsigned.
		/// </summary>
		/// <param name="context">The context.</param>
		private void MulUnsigned(Context context)
		{
			Operand result = context.Result;
			Operand operand1 = context.Operand1;
			Operand operand2 = context.Operand2;

			Operand v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);
			context.SetInstruction2(X86.Mul, v1, result, operand1, operand2);
		}

		/// <summary>
		/// Visitation function for NopInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Nop(Context context)
		{
			context.SetInstruction(X86.Nop);
		}

		/// <summary>
		/// Visitation function for RemFloat.
		/// </summary>
		/// <param name="context">The context.</param>
		private void RemFloat(Context context, string method)
		{
			var result = context.Result;
			var dividend = context.Operand1;
			var divisor = context.Operand2;

			var type = TypeSystem.GetTypeByName("Mosa.Runtime.x86", "Division");

			Debug.Assert(type != null, "Cannot find type: Mosa.Runtime.x86.Division type");

			var mosaMethod = type.FindMethodByName(method);

			Debug.Assert(method != null, "Cannot find method: " + method);

			context.ReplaceInstructionOnly(IRInstruction.Call);
			context.SetOperand(0, Operand.CreateSymbolFromMethod(TypeSystem, mosaMethod));
			context.Result = result;
			context.Operand2 = dividend;
			context.Operand3 = divisor;
			context.OperandCount = 3;
			context.ResultCount = 1;
			context.InvokeMethod = mosaMethod;

			// Since we are already in IR Transformation Stage we gotta call this now
			CallingConvention.MakeCall(MethodCompiler, context);
		}

		/// <summary>
		/// Visitation function for RemFloat.
		/// </summary>
		/// <param name="context">The context.</param>
		private void RemFloatR4(Context context)
		{
			Debug.Assert(context.Result.IsR4);
			Debug.Assert(context.Operand1.IsR4);

			RemFloat(context, "RemR4");
		}

		/// <summary>
		/// Visitation function for RemFloat.
		/// </summary>
		/// <param name="context">The context.</param>
		private void RemFloatR8(Context context)
		{
			Debug.Assert(context.Result.IsR8);
			Debug.Assert(context.Operand1.IsR8);

			RemFloat(context, "RemR8");
		}

		/// <summary>
		/// Visitation function for RemSigned.
		/// </summary>
		/// <param name="context">The context.</param>
		private void RemSigned(Context context)
		{
			Operand result = context.Result;
			Operand operand1 = context.Operand1;
			Operand operand2 = context.Operand2;

			Operand v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			Operand v2 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);
			Operand v3 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			context.SetInstruction2(X86.Cdq, v1, v2, operand1);
			context.AppendInstruction2(X86.IDiv, result, v3, v1, v2, operand2);
		}

		/// <summary>
		/// Visitation function for RemUnsigned.
		/// </summary>
		/// <param name="context">The context.</param>
		private void RemUnsigned(Context context)
		{
			Operand result = context.Result;
			Operand operand1 = context.Operand1;
			Operand operand2 = context.Operand2;

			Operand v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);
			Operand v2 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);

			context.SetInstruction(X86.Mov, v1, ConstantZero);
			context.AppendInstruction2(X86.Div, v1, v2, v1, operand1, operand2);
			context.AppendInstruction(X86.Mov, result, v1);
		}

		/// <summary>
		/// Visitation function for Return.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Return(Context context)
		{
			//Debug.Assert(context.BranchTargets != null);

			if (context.Operand1 != null)
			{
				var returnOperand = context.Operand1;

				context.Empty();

				CallingConvention.SetReturnValue(MethodCompiler, context, returnOperand);

				context.AppendInstruction(X86.Jmp, BasicBlocks.EpilogueBlock);
			}
			else
			{
				context.SetInstruction(X86.Jmp, BasicBlocks.EpilogueBlock);
			}
		}

		/// <summary>
		/// Visitation function for ShiftLeftInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ShiftLeft(Context context)
		{
			context.ReplaceInstructionOnly(X86.Shl);
		}

		/// <summary>
		/// Visitation function for ShiftRightInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ShiftRight(Context context)
		{
			context.ReplaceInstructionOnly(X86.Shr);
		}

		/// <summary>
		/// Visitation function for SignExtendedMoveInstruction instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		private void SignExtendedMove(Context context)
		{
			context.ReplaceInstructionOnly(X86.Movsx);
		}

		private void StoreFloatR4(Context context)
		{
			context.SetInstruction(X86.MovssStore, context.Size, null, context.Operand1, context.Operand2, context.Operand3);
		}

		private void StoreFloatR8(Context context)
		{
			context.SetInstruction(X86.MovsdStore, context.Size, null, context.Operand1, context.Operand2, context.Operand3);
		}

		private void StoreInt(Context context)
		{
			context.SetInstruction(X86.MovStore, context.Size, null, context.Operand1, context.Operand2, context.Operand3);
		}

		/// <summary>
		/// Visitation function for SubFloat.
		/// </summary>
		/// <param name="context">The context.</param>
		private void SubFloatR4(Context context)
		{
			Debug.Assert(context.Result.IsR4);
			Debug.Assert(context.Operand1.IsR4);

			context.ReplaceInstructionOnly(X86.Subss);
			context.Size = InstructionSize.Size32;
		}

		/// <summary>
		/// Visitation function for SubFloat.
		/// </summary>
		/// <param name="context">The context.</param>
		private void SubFloatR8(Context context)
		{
			Debug.Assert(context.Result.IsR8);
			Debug.Assert(context.Operand1.IsR8);

			context.ReplaceInstructionOnly(X86.Subsd);
			context.Size = InstructionSize.Size64;
		}

		/// <summary>
		/// Visitation function for SubSigned.
		/// </summary>
		/// <param name="context">The context.</param>
		private void SubSigned(Context context)
		{
			context.ReplaceInstructionOnly(X86.Sub);
		}

		/// <summary>
		/// Visitation function for SubUnsigned.
		/// </summary>
		/// <param name="context">The context.</param>
		private void SubUnsigned(Context context)
		{
			context.ReplaceInstructionOnly(X86.Sub);
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
				context.AppendInstruction(X86.Cmp, null, operand, Operand.CreateConstant(TypeSystem, i));
				context.AppendInstruction(X86.Branch, ConditionCode.Equal, targets[i]);
			}
		}

		/// <summary>
		/// Visitation function for ZeroExtendedMoveInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ZeroExtendedMove(Context context)
		{
			Debug.Assert(context.Size != InstructionSize.None);

			context.ReplaceInstructionOnly(X86.Movzx);
		}

		#endregion Visitation Methods
	}
}
