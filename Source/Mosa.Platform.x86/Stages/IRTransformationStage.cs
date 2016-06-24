// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
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
	/// <remarks>
	/// This transformation stage transforms IR instructions into their equivalent X86 sequences.
	/// </remarks>
	public sealed class IRTransformationStage : BaseTransformationStage
	{
		private const int LargeAlignment = 16;

		protected override void PopulateVisitationDictionary()
		{
			visitationDictionary[IRInstruction.AddSigned] = AddSigned;
			visitationDictionary[IRInstruction.AddUnsigned] = AddUnsigned;
			visitationDictionary[IRInstruction.AddFloat] = AddFloat;
			visitationDictionary[IRInstruction.DivFloat] = DivFloat;
			visitationDictionary[IRInstruction.AddressOf] = AddressOf;
			visitationDictionary[IRInstruction.FloatCompare] = FloatCompare;
			visitationDictionary[IRInstruction.IntegerCompareBranch] = IntegerCompareBranch;
			visitationDictionary[IRInstruction.IntegerCompare] = IntegerCompare;
			visitationDictionary[IRInstruction.Jmp] = Jmp;
			visitationDictionary[IRInstruction.Load] = Load;
			visitationDictionary[IRInstruction.Load2] = Load2;
			visitationDictionary[IRInstruction.CompoundLoad] = CompoundLoad;
			visitationDictionary[IRInstruction.LoadSignExtended] = LoadSignExtended;
			visitationDictionary[IRInstruction.LoadZeroExtended] = LoadZeroExtended;
			visitationDictionary[IRInstruction.LogicalAnd] = LogicalAnd;
			visitationDictionary[IRInstruction.LogicalOr] = LogicalOr;
			visitationDictionary[IRInstruction.LogicalXor] = LogicalXor;
			visitationDictionary[IRInstruction.LogicalNot] = LogicalNot;
			visitationDictionary[IRInstruction.Move] = Move;
			visitationDictionary[IRInstruction.CompoundMove] = CompoundMove;
			visitationDictionary[IRInstruction.Return] = Return;
			visitationDictionary[IRInstruction.InternalCall] = InternalCall;
			visitationDictionary[IRInstruction.InternalReturn] = InternalReturn;
			visitationDictionary[IRInstruction.ArithmeticShiftRight] = ArithmeticShiftRight;
			visitationDictionary[IRInstruction.ShiftLeft] = ShiftLeft;
			visitationDictionary[IRInstruction.ShiftRight] = ShiftRight;
			visitationDictionary[IRInstruction.Store] = Store;
			visitationDictionary[IRInstruction.CompoundStore] = CompoundStore;
			visitationDictionary[IRInstruction.MulFloat] = MulFloat;
			visitationDictionary[IRInstruction.SubFloat] = SubFloat;
			visitationDictionary[IRInstruction.SubSigned] = SubSigned;
			visitationDictionary[IRInstruction.SubUnsigned] = SubUnsigned;
			visitationDictionary[IRInstruction.MulSigned] = MulSigned;
			visitationDictionary[IRInstruction.MulUnsigned] = MulUnsigned;
			visitationDictionary[IRInstruction.DivSigned] = DivSigned;
			visitationDictionary[IRInstruction.DivUnsigned] = DivUnsigned;
			visitationDictionary[IRInstruction.RemSigned] = RemSigned;
			visitationDictionary[IRInstruction.RemUnsigned] = RemUnsigned;
			visitationDictionary[IRInstruction.RemFloat] = RemFloat;
			visitationDictionary[IRInstruction.Switch] = Switch;
			visitationDictionary[IRInstruction.Break] = Break;
			visitationDictionary[IRInstruction.Nop] = Nop;
			visitationDictionary[IRInstruction.SignExtendedMove] = SignExtendedMove;
			visitationDictionary[IRInstruction.Call] = Call;
			visitationDictionary[IRInstruction.ZeroExtendedMove] = ZeroExtendedMove;
			visitationDictionary[IRInstruction.FloatToIntegerConversion] = FloatToIntegerConversion;
			visitationDictionary[IRInstruction.IntegerToFloatConversion] = IntegerToFloatConversion;
		}

		#region Visitation Methods

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
		/// Visitation function for AddFloat.
		/// </summary>
		/// <param name="context">The context.</param>
		private void AddFloat(Context context)
		{
			if (context.Result.IsR4)
			{
				context.ReplaceInstructionOnly(X86.Addss);
				context.Size = InstructionSize.Size32;
			}
			else
			{
				context.ReplaceInstructionOnly(X86.Addsd);
				context.Size = InstructionSize.Size64;
			}
		}

		/// <summary>
		/// Visitation function for DivFloat.
		/// </summary>
		/// <param name="context">The context.</param>
		private void DivFloat(Context context)
		{
			if (context.Result.IsR4)
			{
				context.ReplaceInstructionOnly(X86.Divss);
				context.Size = InstructionSize.Size32;
			}
			else
			{
				context.ReplaceInstructionOnly(X86.Divsd);
				context.Size = InstructionSize.Size64;
			}
		}

		/// <summary>
		/// Addresses the of instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void AddressOf(Context context)
		{
			Operand result = context.Result;
			Operand register = AllocateVirtualRegister(result.Type);

			context.Result = register;
			context.ReplaceInstructionOnly(X86.Lea);

			context.AppendInstruction(X86.Mov, NativeInstructionSize, result, register);
		}

		/// <summary>
		/// Floating point compare instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void FloatCompare(Context context)
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

			// TODO - Move the following to its own pre-IR decomposition stage for this instruction
			// Swap, if necessary, the operands to place register operand first than memory operand
			// otherwise the memory operand will have to be loaded into a register
			if ((condition == ConditionCode.Equal || condition == ConditionCode.NotEqual) && left.IsMemoryAddress && !right.IsMemoryAddress)
			{
				// swap order of operands to move
				var t = left;
				left = right;
				right = t;
			}

			Context before = context.InsertBefore();

			// Compare using the smallest precision
			if (left.IsR4 && right.IsR8)
			{
				Operand rop = AllocateVirtualRegister(TypeSystem.BuiltIn.R4);
				before.SetInstruction(X86.Cvtsd2ss, rop, right);
				right = rop;
			}
			if (left.IsR8 && right.IsR4)
			{
				Operand rop = AllocateVirtualRegister(TypeSystem.BuiltIn.R4);
				before.SetInstruction(X86.Cvtsd2ss, rop, left);
				left = rop;
			}

			X86Instruction instruction = null;
			InstructionSize size = InstructionSize.None;
			if (left.IsR4)
			{
				instruction = X86.Ucomiss;
				size = InstructionSize.Size32;
			}
			else
			{
				instruction = X86.Ucomisd;
				size = InstructionSize.Size64;
			}

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
						newBlocks[0].AppendInstruction(X86.Movzx, result, result);
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
		/// Visitation function for IntegerCompareBranchInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void IntegerCompareBranch(Context context)
		{
			var target = context.BranchTargets[0];
			var condition = context.ConditionCode;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			context.SetInstruction(X86.Cmp, null, operand1, operand2);
			context.AppendInstruction(X86.Branch, condition, target);
		}

		/// <summary>
		/// Visitation function for IntegerCompareInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void IntegerCompare(Context context)
		{
			var condition = context.ConditionCode;
			var resultOperand = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			Operand v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			context.SetInstruction(X86.Mov, v1, ConstantZero);

			context.AppendInstruction(X86.Cmp, null, operand1, operand2);

			if (resultOperand.IsUnsigned || resultOperand.IsChar)
				context.AppendInstruction(X86.Setcc, condition.GetUnsigned(), v1);
			else
				context.AppendInstruction(X86.Setcc, condition, v1);

			context.AppendInstruction(X86.Mov, resultOperand, v1);
		}

		/// <summary>
		/// Visitation function for JmpInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Jmp(Context context)
		{
			context.ReplaceInstructionOnly(X86.Jmp);
		}

		/// <summary>
		/// Visitation function for LoadInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Load(Context context)
		{
			Operand result = context.Result;
			Operand baseOperand = context.Operand1;
			Operand offsetOperand = context.Operand2;
			var type = context.MosaType;
			var size = context.Size;

			if (offsetOperand.IsConstant)
			{
				if (baseOperand.IsField)
				{
					Debug.Assert(offsetOperand.IsConstantZero);
					Debug.Assert(baseOperand.Field.IsStatic);

					var mov = GetMove(result, baseOperand);

					if (result.IsR8 && type.IsR4) // size == InstructionSize.Size32)
					{
						mov = X86.Cvtss2sd;
					}

					context.SetInstruction(GetMove(result, baseOperand), size, result, baseOperand);
				}
				else
				{
					Operand mem = Operand.CreateMemoryAddress(baseOperand.Type, baseOperand, offsetOperand.ConstantSignedLongInteger);

					var mov = GetMove(result, mem);

					if (result.IsR8 && type.IsR4) // size == InstructionSize.Size32)
					{
						mov = X86.Cvtss2sd;
					}

					context.SetInstruction(mov, size, result, mem);
				}
			}
			else
			{
				Operand v1 = AllocateVirtualRegister(baseOperand.Type);
				Operand mem = Operand.CreateMemoryAddress(v1.Type, v1, 0);

				context.SetInstruction(X86.Mov, v1, baseOperand);
				context.AppendInstruction(X86.Add, v1, v1, offsetOperand);

				var mov = GetMove(result, mem);

				if (result.IsR8 && type.IsR4) // size == InstructionSize.Size32)
				{
					mov = X86.Cvtss2sd;
				}

				context.AppendInstruction(mov, size, result, mem);
			}
		}

		private void Load2(Context context)
		{
			Operand result = context.Result;
			Operand baseOperand = context.Operand1;
			Operand offsetOperand = context.Operand2;
			var type = context.MosaType;
			var size = context.Size;

			context.SetInstruction(X86.MovLoad, size, result, baseOperand, offsetOperand);
		}

		private void CompoundLoad(Context context)
		{
			var type = context.Result.Type;
			int typeSize = TypeLayout.GetTypeSize(type);
			int alignedTypeSize = typeSize - (typeSize % NativeAlignment);
			int largeAlignedTypeSize = typeSize - (typeSize % LargeAlignment);
			Debug.Assert(typeSize > 0, context.Operand2.Name);

			int offset = 0;
			if (context.Operand2.IsConstant)
			{
				offset = (int)context.Operand2.ConstantSignedLongInteger;
			}

			var offsetop = context.Operand2;
			var src = context.Operand1;
			var dest = context.Result;
			Debug.Assert(dest.IsMemoryAddress, dest.Name);

			var srcReg = MethodCompiler.CreateVirtualRegister(dest.Type.TypeSystem.BuiltIn.I4);
			var dstReg = MethodCompiler.CreateVirtualRegister(dest.Type.TypeSystem.BuiltIn.I4);
			var tmp = MethodCompiler.CreateVirtualRegister(dest.Type.TypeSystem.BuiltIn.I4);
			var tmpLarge = Operand.CreateCPURegister(MethodCompiler.TypeSystem.BuiltIn.Void, SSE2Register.XMM1);

			context.SetInstruction(X86.Nop);
			context.AppendInstruction(X86.Mov, srcReg, src);
			context.AppendInstruction(X86.Lea, dstReg, dest);

			if (!offsetop.IsConstant)
			{
				context.AppendInstruction(X86.Add, srcReg, srcReg, offsetop);
			}

			for (int i = 0; i < largeAlignedTypeSize; i += LargeAlignment)
			{
				// Large Aligned moves allow 128bits to be copied at a time
				var memSrc = Operand.CreateMemoryAddress(MethodCompiler.TypeSystem.BuiltIn.Void, srcReg, i + offset);
				var memDest = Operand.CreateMemoryAddress(MethodCompiler.TypeSystem.BuiltIn.Void, dstReg, i);
				context.AppendInstruction(X86.MovUPS, InstructionSize.Size128, tmpLarge, memSrc);
				context.AppendInstruction(X86.MovUPS, InstructionSize.Size128, memDest, tmpLarge);
			}
			for (int i = largeAlignedTypeSize; i < alignedTypeSize; i += NativeAlignment)
			{
				context.AppendInstruction(X86.Mov, InstructionSize.Size32, tmp, Operand.CreateMemoryAddress(TypeSystem.BuiltIn.I4, srcReg, i + offset));
				context.AppendInstruction(X86.Mov, InstructionSize.Size32, Operand.CreateMemoryAddress(TypeSystem.BuiltIn.I4, dstReg, i), tmp);
			}
			for (int i = alignedTypeSize; i < typeSize; i++)
			{
				context.AppendInstruction(X86.Mov, InstructionSize.Size8, tmp, Operand.CreateMemoryAddress(TypeSystem.BuiltIn.I4, srcReg, i + offset));
				context.AppendInstruction(X86.Mov, InstructionSize.Size8, Operand.CreateMemoryAddress(TypeSystem.BuiltIn.I4, dstReg, i), tmp);
			}
		}

		/// <summary>
		/// Visitation function for Load Sign Extended.
		/// </summary>
		/// <param name="context">The context.</param>
		private void LoadSignExtended(Context context)
		{
			var destination = context.Result;
			var source = context.Operand1;
			var type = context.MosaType;
			var offset = context.Operand2;
			var size = context.Size;

			var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			context.SetInstruction(X86.Mov, v1, source);

			if (offset.IsConstant)
			{
				if (source.IsField)
				{
					Debug.Assert(offset.IsConstantZero);
					Debug.Assert(source.Field.IsStatic);

					context.SetInstruction(X86.Movsx, size, destination, source);
				}
				else
				{
					context.AppendInstruction(X86.Movsx, size, destination, Operand.CreateMemoryAddress(type, v1, offset.ConstantSignedLongInteger));
				}
			}
			else
			{
				context.AppendInstruction(X86.Add, v1, v1, offset);
				context.AppendInstruction(X86.Movsx, size, destination, Operand.CreateMemoryAddress(type, v1, 0));
			}
		}

		/// <summary>
		/// Visitation function for Load Zero Extended.
		/// </summary>
		/// <param name="context">The context.</param>
		private void LoadZeroExtended(Context context)
		{
			var destination = context.Result;
			var source = context.Operand1;
			var offset = context.Operand2;
			var type = context.MosaType;
			var size = context.Size;

			Debug.Assert(offset != null);

			Operand v1 = AllocateVirtualRegister(source.Type);

			context.SetInstruction(X86.Mov, v1, source);

			if (offset.IsConstant)
			{
				if (source.IsField)
				{
					Debug.Assert(offset.IsConstantZero);
					Debug.Assert(source.Field.IsStatic);

					context.SetInstruction(X86.Movzx, size, destination, source);
				}
				else
				{
					context.AppendInstruction(X86.Movzx, size, destination, Operand.CreateMemoryAddress(type, v1, offset.ConstantSignedLongInteger));
				}
			}
			else
			{
				context.AppendInstruction(X86.Add, v1, v1, offset);
				context.AppendInstruction(X86.Movzx, size, destination, Operand.CreateMemoryAddress(type, v1, 0));
			}
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
		/// Visitation function for LogicalNotInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void LogicalNot(Context context)
		{
			Operand dest = context.Result;

			context.SetInstruction(X86.Mov, context.Result, context.Operand1);
			if (dest.IsByte)
				context.AppendInstruction(X86.Xor, dest, dest, Operand.CreateConstant(TypeSystem, 0xFF));
			else if (dest.IsU2)
				context.AppendInstruction(X86.Xor, dest, dest, Operand.CreateConstant(TypeSystem, 0xFFFF));
			else
				context.AppendInstruction(X86.Not, dest, dest);
		}

		/// <summary>
		/// Visitation function for MoveInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Move(Context context)
		{
			Operand result = context.Result;
			Operand operand = context.Operand1;

			X86Instruction instruction = X86.Mov;
			InstructionSize size = InstructionSize.None;

			if (result.IsR)
			{
				//Debug.Assert(operand.IsFloatingPoint, @"Move can't convert to floating point type.");

				if (result.Type == operand.Type)
				{
					if (result.IsR4)
					{
						instruction = X86.Movss;
						size = InstructionSize.Size32;
					}
					else if (result.IsR8)
					{
						instruction = X86.Movsd;
						size = InstructionSize.Size64;
					}
				}
				else if (result.IsR8)
				{
					instruction = X86.Cvtss2sd;
				}
				else if (result.IsR4)
				{
					instruction = X86.Cvtsd2ss;
				}
			}

			context.ReplaceInstructionOnly(instruction);
			context.Size = size;
		}

		private void CompoundMove(Context context)
		{
			var type = context.Result.Type;
			int typeSize = TypeLayout.GetTypeSize(type);
			int alignedTypeSize = typeSize - (typeSize % NativeAlignment);
			int largeAlignedTypeSize = typeSize - (typeSize % LargeAlignment);
			Debug.Assert(typeSize > 0, MethodCompiler.Method.FullName);

			var src = context.Operand1;
			var dest = context.Result;
			Debug.Assert((src.IsMemoryAddress || src.IsSymbol) && dest.IsMemoryAddress, context.ToString());

			var srcReg = MethodCompiler.CreateVirtualRegister(dest.Type.TypeSystem.BuiltIn.I4);
			var dstReg = MethodCompiler.CreateVirtualRegister(dest.Type.TypeSystem.BuiltIn.I4);
			var tmp = MethodCompiler.CreateVirtualRegister(dest.Type.TypeSystem.BuiltIn.I4);
			var tmpLarge = Operand.CreateCPURegister(MethodCompiler.TypeSystem.BuiltIn.Void, SSE2Register.XMM1);

			context.SetInstruction(X86.Nop);
			if (src.IsSymbol)
			{
				context.AppendInstruction(X86.Mov, srcReg, src);
			}
			else
			{
				context.AppendInstruction(X86.Lea, srcReg, src);
			}
			context.AppendInstruction(X86.Lea, dstReg, dest);

			for (int i = 0; i < largeAlignedTypeSize; i += LargeAlignment)
			{
				// Large Aligned moves allow 128bits to be copied at a time
				var memSrc = Operand.CreateMemoryAddress(MethodCompiler.TypeSystem.BuiltIn.Void, srcReg, i);
				var memDest = Operand.CreateMemoryAddress(MethodCompiler.TypeSystem.BuiltIn.Void, dstReg, i);
				context.AppendInstruction(X86.MovUPS, InstructionSize.Size128, tmpLarge, memSrc);
				context.AppendInstruction(X86.MovUPS, InstructionSize.Size128, memDest, tmpLarge);
			}
			for (int i = largeAlignedTypeSize; i < alignedTypeSize; i += NativeAlignment)
			{
				context.AppendInstruction(X86.Mov, InstructionSize.Size32, tmp, Operand.CreateMemoryAddress(TypeSystem.BuiltIn.I4, srcReg, i));
				context.AppendInstruction(X86.Mov, InstructionSize.Size32, Operand.CreateMemoryAddress(TypeSystem.BuiltIn.I4, dstReg, i), tmp);
			}
			for (int i = alignedTypeSize; i < typeSize; i++)
			{
				context.AppendInstruction(X86.Mov, InstructionSize.Size8, tmp, Operand.CreateMemoryAddress(TypeSystem.BuiltIn.I4, srcReg, i));
				context.AppendInstruction(X86.Mov, InstructionSize.Size8, Operand.CreateMemoryAddress(TypeSystem.BuiltIn.I4, dstReg, i), tmp);
			}
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

				CallingConvention.SetReturnValue(MethodCompiler, TypeLayout, context, returnOperand);

				context.AppendInstruction(X86.Jmp, BasicBlocks.EpilogueBlock);
			}
			else
			{
				context.SetInstruction(X86.Jmp, BasicBlocks.EpilogueBlock);
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
		/// Arithmetic the shift right instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ArithmeticShiftRight(Context context)
		{
			context.ReplaceInstructionOnly(X86.Sar);
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
		/// Visitation function for StoreInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Store(Context context)
		{
			Operand baseOperand = context.Operand1;
			Operand offsetOperand = context.Operand2;
			Operand value = context.Operand3;

			MosaType storeType = context.MosaType;
			var type = baseOperand.Type;
			var size = context.Size;

			if (value.IsR8 && type.IsR4) //&& size == InstructionSize.Size32)
			{
				Operand xmm1 = AllocateVirtualRegister(TypeSystem.BuiltIn.R4);
				context.InsertBefore().AppendInstruction(X86.Cvtsd2ss, size, xmm1, value);
				value = xmm1;
			}
			else if (value.IsMemoryAddress)
			{
				Operand v2 = AllocateVirtualRegister(value.Type);
				context.InsertBefore().AppendInstruction(X86.Mov, size, v2, value);
				value = v2;
			}

			if (offsetOperand.IsConstant)
			{
				if (baseOperand.IsField)
				{
					Debug.Assert(offsetOperand.IsConstantZero);
					Debug.Assert(baseOperand.Field.IsStatic);

					context.SetInstruction(GetMove(baseOperand, value), size, baseOperand, value);
				}
				else
				{
					var mem = Operand.CreateMemoryAddress(storeType, baseOperand, offsetOperand.ConstantSignedLongInteger);
					context.SetInstruction(GetMove(mem, value), size, mem, value);
				}
			}
			else
			{
				if (type.IsUnmanagedPointer)
					type = storeType.ToUnmanagedPointer();
				else if (type.IsManagedPointer)
					type = storeType.ToManagedPointer();

				Operand v1 = AllocateVirtualRegister(type);
				Operand mem = Operand.CreateMemoryAddress(storeType, v1, 0);

				context.SetInstruction(X86.Mov, v1, baseOperand);
				context.AppendInstruction(X86.Add, v1, v1, offsetOperand);
				context.AppendInstruction(GetMove(mem, value), size, mem, value);
			}
		}

		private void CompoundStore(Context context)
		{
			var type = context.Operand3.Type;
			int typeSize = TypeLayout.GetTypeSize(type);
			int alignedTypeSize = typeSize - (typeSize % NativeAlignment);
			int largeAlignedTypeSize = typeSize - (typeSize % LargeAlignment);
			Debug.Assert(typeSize > 0, MethodCompiler.Method.FullName);

			int offset = 0;
			if (context.Operand2.IsConstant)
			{
				offset = (int)context.Operand2.ConstantSignedLongInteger;
			}

			var offsetop = context.Operand2;
			var src = context.Operand3;
			var dest = context.Operand1;
			Debug.Assert(src.IsMemoryAddress);

			var srcReg = MethodCompiler.CreateVirtualRegister(dest.Type.TypeSystem.BuiltIn.I4);
			var dstReg = MethodCompiler.CreateVirtualRegister(dest.Type.TypeSystem.BuiltIn.I4);
			var tmp = MethodCompiler.CreateVirtualRegister(dest.Type.TypeSystem.BuiltIn.I4);
			var tmpLarge = Operand.CreateCPURegister(MethodCompiler.TypeSystem.BuiltIn.Void, SSE2Register.XMM1);

			context.SetInstruction(X86.Nop);
			context.AppendInstruction(X86.Lea, srcReg, src);
			context.AppendInstruction(X86.Mov, dstReg, dest);

			if (!offsetop.IsConstant)
			{
				context.AppendInstruction(X86.Add, dstReg, dstReg, offsetop);
			}

			for (int i = 0; i < largeAlignedTypeSize; i += LargeAlignment)
			{
				// Large Aligned moves allow 128bits to be copied at a time
				var memSrc = Operand.CreateMemoryAddress(MethodCompiler.TypeSystem.BuiltIn.Void, srcReg, i);
				var memDest = Operand.CreateMemoryAddress(MethodCompiler.TypeSystem.BuiltIn.Void, dstReg, i + offset);
				context.AppendInstruction(X86.MovUPS, InstructionSize.Size128, tmpLarge, memSrc);
				context.AppendInstruction(X86.MovUPS, InstructionSize.Size128, memDest, tmpLarge);
			}
			for (int i = largeAlignedTypeSize; i < alignedTypeSize; i += NativeAlignment)
			{
				context.AppendInstruction(X86.Mov, InstructionSize.Size32, tmp, Operand.CreateMemoryAddress(TypeSystem.BuiltIn.I4, srcReg, i));
				context.AppendInstruction(X86.Mov, InstructionSize.Size32, Operand.CreateMemoryAddress(TypeSystem.BuiltIn.I4, dstReg, i + offset), tmp);
			}
			for (int i = alignedTypeSize; i < typeSize; i++)
			{
				context.AppendInstruction(X86.Mov, InstructionSize.Size8, tmp, Operand.CreateMemoryAddress(TypeSystem.BuiltIn.I4, srcReg, i));
				context.AppendInstruction(X86.Mov, InstructionSize.Size8, Operand.CreateMemoryAddress(TypeSystem.BuiltIn.I4, dstReg, i + offset), tmp);
			}
		}

		/// <summary>
		/// Visitation function for MulFloat.
		/// </summary>
		/// <param name="context">The context.</param>
		private void MulFloat(Context context)
		{
			if (context.Result.IsR4)
			{
				context.ReplaceInstructionOnly(X86.Mulss);
				context.Size = InstructionSize.Size32;
			}
			else
			{
				context.ReplaceInstructionOnly(X86.Mulsd);
				context.Size = InstructionSize.Size64;
			}
		}

		/// <summary>
		/// Visitation function for SubFloat.
		/// </summary>
		/// <param name="context">The context.</param>
		private void SubFloat(Context context)
		{
			if (context.Result.IsR4)
			{
				context.ReplaceInstructionOnly(X86.Subss);
				context.Size = InstructionSize.Size32;
			}
			else
			{
				context.ReplaceInstructionOnly(X86.Subsd);
				context.Size = InstructionSize.Size64;
			}
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
		/// Visitation function for RemFloat.
		/// </summary>
		/// <param name="context">The context.</param>
		private void RemFloat(Context context)
		{
			var result = context.Result;
			var dividend = context.Operand1;
			var divisor = context.Operand2;
			var method = (result.IsR8) ? "RemR8" : "RemR4";

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
			CallingConvention.MakeCall(MethodCompiler, TypeLayout, context);
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
		/// Visitation function for BreakInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Break(Context context)
		{
			context.SetInstruction(X86.Break);
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
		/// Visitation function for SignExtendedMoveInstruction instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		private void SignExtendedMove(Context context)
		{
			context.ReplaceInstructionOnly(X86.Movsx);
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
				CallingConvention.MakeCall(MethodCompiler, TypeLayout, context);
			}
		}

		/// <summary>
		/// Visitation function for ZeroExtendedMoveInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ZeroExtendedMove(Context context)
		{
			context.ReplaceInstructionOnly(X86.Movzx);
		}

		/// <summary>
		/// Visitation function for FloatingPointToIntegerConversionInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void FloatToIntegerConversion(Context context)
		{
			Operand source = context.Operand1;
			Operand destination = context.Result;

			if (destination.Type.IsI1 || destination.Type.IsI2 || destination.Type.IsI4)
			{
				if (source.IsR8)
					context.ReplaceInstructionOnly(X86.Cvttsd2si);
				else
					context.ReplaceInstructionOnly(X86.Cvttss2si);
			}
			else
			{
				throw new NotImplementCompilerException();
			}
		}

		/// <summary>
		/// Visitation function for IntegerToFloatingPointConversion.
		/// </summary>
		/// <param name="context">The context.</param>
		private void IntegerToFloatConversion(Context context)
		{
			if (context.Result.IsR4)
				context.ReplaceInstructionOnly(X86.Cvtsi2ss);
			else if (context.Result.IsR8)
				context.ReplaceInstructionOnly(X86.Cvtsi2sd);
			else
				throw new NotSupportedException();
		}

		#endregion Visitation Methods
	}
}
