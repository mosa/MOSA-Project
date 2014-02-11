/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Scott Balmos <sbalmos@fastmail.fm>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

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
	public sealed class IRTransformationStage : BaseTransformationStage, IIRVisitor, IMethodCompilerStage
	{
		#region IMethodCompilerStage

		/// <summary>
		/// Setup stage specific processing on the compiler context.
		/// </summary>
		/// <param name="methodCompiler">The compiler context to perform processing in.</param>
		void IMethodCompilerStage.Setup(BaseMethodCompiler methodCompiler)
		{
			base.Setup(methodCompiler);
		}

		#endregion IMethodCompilerStage

		#region IIRVisitor

		/// <summary>
		/// Visitation function for AddSigned.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.AddSigned(Context context)
		{
			ReplaceInstructionAndAnyFloatingPointConstant(context, X86.Add);
		}

		/// <summary>
		/// Visitation function for AddUnsigned.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.AddUnsigned(Context context)
		{
			ReplaceInstructionAndAnyFloatingPointConstant(context, X86.Add);
		}

		/// <summary>
		/// Visitation function for AddFloat.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.AddFloat(Context context)
		{
			if (context.Result.IsSingle)
				ReplaceInstructionAndAnyFloatingPointConstant(context, X86.Addss);
			else
				ReplaceInstructionAndAnyFloatingPointConstant(context, X86.Addsd);
		}

		/// <summary>
		/// Visitation function for DivFloat.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.DivFloat(Context context)
		{
			if (context.Result.IsSingle)
				ReplaceInstructionAndAnyFloatingPointConstant(context, X86.Divss);
			else
				ReplaceInstructionAndAnyFloatingPointConstant(context, X86.Divsd);
		}

		/// <summary>
		/// Addresses the of instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.AddressOf(Context context)
		{
			Operand result = context.Result;

			Operand register = AllocateVirtualRegister(result.Type);

			context.Result = register;
			context.ReplaceInstructionOnly(X86.Lea);

			context.AppendInstruction(X86.Mov, result, register);
		}

		/// <summary>
		/// Floating point compare instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.FloatCompare(Context context)
		{
			EmitFloatingPointConstants(context);

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
			if (left.IsSingle && right.IsDouble)
			{
				Operand rop = AllocateVirtualRegister(typeSystem.BuiltIn.Single);
				before.SetInstruction(X86.Cvtsd2ss, rop, right);
				right = rop;
			}
			if (left.IsDouble && right.IsSingle)
			{
				Operand rop = AllocateVirtualRegister(typeSystem.BuiltIn.Single);
				before.SetInstruction(X86.Cvtsd2ss, rop, left);
				left = rop;
			}

			X86Instruction instruction = null;
			if (left.IsSingle)
			{
				instruction = X86.Ucomiss;
			}
			else
			{
				instruction = X86.Ucomisd;
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

						Context[] newBlocks = CreateNewBlocksWithContexts(2);
						Context nextBlock = Split(context);

						context.SetInstruction(X86.Mov, result, Operand.CreateConstantSignedInt(typeSystem, 1));
						context.AppendInstruction(instruction, null, left, right);
						context.AppendInstruction(X86.Branch, ConditionCode.Parity, newBlocks[1].BasicBlock);
						context.AppendInstruction(X86.Jmp, newBlocks[0].BasicBlock);
						LinkBlocks(context, newBlocks[0], newBlocks[1]);

						newBlocks[0].AppendInstruction(X86.Branch, ConditionCode.NotEqual, newBlocks[1].BasicBlock);
						newBlocks[0].AppendInstruction(X86.Jmp, nextBlock.BasicBlock);
						LinkBlocks(newBlocks[0], newBlocks[1], nextBlock);

						newBlocks[1].AppendInstruction(X86.Mov, result, Operand.CreateConstantSignedInt(typeSystem, 0));
						newBlocks[1].AppendInstruction(X86.Jmp, nextBlock.BasicBlock);
						LinkBlocks(newBlocks[1], nextBlock);

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

						Context[] newBlocks = CreateNewBlocksWithContexts(1);
						Context nextBlock = Split(context);

						context.SetInstruction(X86.Mov, result, Operand.CreateConstantSignedInt(typeSystem, 1));
						context.AppendInstruction(instruction, null, left, right);
						context.AppendInstruction(X86.Branch, ConditionCode.Parity, nextBlock.BasicBlock);
						context.AppendInstruction(X86.Jmp, newBlocks[0].BasicBlock);
						LinkBlocks(context, nextBlock, newBlocks[0]);

						newBlocks[0].AppendInstruction(X86.Setcc, ConditionCode.NotEqual, result);
						newBlocks[0].AppendInstruction(X86.Movzx, result, result);
						newBlocks[0].AppendInstruction(X86.Jmp, newBlocks[0].BasicBlock);
						LinkBlocks(newBlocks[0], newBlocks[0]);

						break;
					}

				case ConditionCode.LessThan:
					{
						//	a>b and a<b
						//	mov	eax, 0
						//	ucomisd	xmm1, xmm0
						//	seta	al

						context.SetInstruction(X86.Mov, result, Operand.CreateConstantSignedInt(typeSystem, 0));
						context.AppendInstruction(instruction, null, right, left);
						context.AppendInstruction(X86.Setcc, ConditionCode.UnsignedGreaterThan, result);
						break;
					}
				case ConditionCode.GreaterThan:
					{
						//	a>b and a<b
						//	mov	eax, 0
						//	ucomisd	xmm0, xmm1
						//	seta	al

						context.SetInstruction(X86.Mov, result, Operand.CreateConstantSignedInt(typeSystem, 0));
						context.AppendInstruction(instruction, null, left, right);
						context.AppendInstruction(X86.Setcc, ConditionCode.UnsignedGreaterThan, result);
						break;
					}
				case ConditionCode.LessOrEqual:
					{
						//	a<=b and a>=b
						//	mov	eax, 0
						//	ucomisd	xmm1, xmm0
						//	setae	al

						context.SetInstruction(X86.Mov, result, Operand.CreateConstantSignedInt(typeSystem, 0));
						context.AppendInstruction(instruction, null, left, right);
						context.AppendInstruction(X86.Setcc, ConditionCode.NoCarry, result);
						break;
					}
				case ConditionCode.GreaterOrEqual:
					{
						//	a<=b and a>=b
						//	mov	eax, 0
						//	ucomisd	xmm0, xmm1
						//	setae	al

						context.SetInstruction(X86.Mov, result, Operand.CreateConstantSignedInt(typeSystem, 0));
						context.AppendInstruction(instruction, null, right, left);
						context.AppendInstruction(X86.Setcc, ConditionCode.NoCarry, result);
						break;
					}
			}
		}

		/// <summary>
		/// Visitation function for IntegerCompareBranchInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.IntegerCompareBranch(Context context)
		{
			EmitFloatingPointConstants(context);

			int target = context.BranchTargets[0];
			var condition = context.ConditionCode;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			context.SetInstruction(X86.Cmp, null, operand1, operand2);
			context.AppendInstruction(X86.Branch, condition);
			context.SetBranch(target);
		}

		/// <summary>
		/// Visitation function for IntegerCompareInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.IntegerCompare(Context context)
		{
			EmitFloatingPointConstants(context);

			var condition = context.ConditionCode;
			var resultOperand = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			Operand v1 = AllocateVirtualRegister(typeSystem.BuiltIn.Byte);

			context.SetInstruction(X86.Mov, v1, Operand.CreateConstantSignedInt(typeSystem, 0));

			context.AppendInstruction(X86.Cmp, null, operand1, operand2);

			if (resultOperand.IsUnsigned || resultOperand.IsChar)
				context.AppendInstruction(X86.Setcc, condition.GetUnsigned(), v1);
			else
				context.AppendInstruction(X86.Setcc, condition, v1);

			context.AppendInstruction(X86.Movzx, resultOperand, v1);
		}

		/// <summary>
		/// Visitation function for JmpInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.Jmp(Context context)
		{
			context.ReplaceInstructionOnly(X86.Jmp);
		}

		/// <summary>
		/// Visitation function for LoadInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.Load(Context context)
		{
			Operand result = context.Result;
			Operand baseOperand = context.Operand1;
			Operand offsetOperand = context.Operand2;
			var type = context.MosaType;

			if (offsetOperand.IsConstant)
			{
				Operand mem = Operand.CreateMemoryAddress(baseOperand.Type, baseOperand, offsetOperand.ConstantSignedInteger);

				var mov = GetMove(result, mem);

				if (result.IsDouble && type.IsSingle)
				{
					mov = X86.Cvtss2sd;
				}

				context.SetInstruction(mov, result, mem);
			}
			else
			{
				Operand v1 = AllocateVirtualRegister(baseOperand.Type);
				Operand mem = Operand.CreateMemoryAddress(v1.Type, v1, 0);

				context.SetInstruction(X86.Mov, v1, baseOperand);
				context.AppendInstruction(X86.Add, v1, v1, offsetOperand);

				var mov = GetMove(result, mem);

				if (result.IsDouble && type.IsSingle)
				{
					mov = X86.Cvtss2sd;
				}

				context.AppendInstruction(mov, result, mem);
			}
		}

		/// <summary>
		/// Visitation function for Load Sign Extended.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.LoadSignExtended(Context context)
		{
			var destination = context.Result;
			var source = context.Operand1;
			var type = context.MosaType;
			var offset = context.Operand2;

			var v1 = AllocateVirtualRegister(typeSystem.BuiltIn.Int32);
			long offsetPtr = 0;

			context.SetInstruction(X86.Mov, v1, source);

			if (offset.IsConstant)
			{
				offsetPtr = offset.ConstantSignedInteger;
			}
			else
			{
				context.AppendInstruction(X86.Add, v1, v1, offset);
			}

			context.AppendInstruction(X86.Movsx, destination, Operand.CreateMemoryAddress(type, v1, offsetPtr));
		}

		/// <summary>
		/// Visitation function for Load Zero Extended.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.LoadZeroExtended(Context context)
		{
			var destination = context.Result;
			var source = context.Operand1;
			var offset = context.Operand2;
			var type = context.MosaType;

			Debug.Assert(offset != null);

			Operand v1 = AllocateVirtualRegister(source.Type);
			long offsetPtr = 0;

			context.SetInstruction(X86.Mov, v1, source);

			if (offset.IsConstant)
			{
				offsetPtr = offset.ConstantSignedInteger;
			}
			else
			{
				context.AppendInstruction(X86.Add, v1, v1, offset);
			}
			context.AppendInstruction(X86.Movzx, destination, Operand.CreateMemoryAddress(type, v1, offsetPtr));
		}

		/// <summary>
		/// Visitation function for LogicalAndInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.LogicalAnd(Context context)
		{
			context.ReplaceInstructionOnly(X86.And);
		}

		/// <summary>
		/// Visitation function for LogicalOrInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.LogicalOr(Context context)
		{
			context.ReplaceInstructionOnly(X86.Or);
		}

		/// <summary>
		/// Visitation function for LogicalXorInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.LogicalXor(Context context)
		{
			context.ReplaceInstructionOnly(X86.Xor);
		}

		/// <summary>
		/// Visitation function for LogicalNotInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.LogicalNot(Context context)
		{
			Operand dest = context.Result;

			context.SetInstruction(X86.Mov, context.Result, context.Operand1);
			if (dest.IsByte)
				context.AppendInstruction(X86.Xor, dest, dest, Operand.CreateConstantUnsignedInt(typeSystem, 0xFF));
			else if (dest.IsUnsignedShort)
				context.AppendInstruction(X86.Xor, dest, dest, Operand.CreateConstantUnsignedInt(typeSystem, 0xFFFF));
			else
				context.AppendInstruction(X86.Not, dest, dest);
		}

		/// <summary>
		/// Visitation function for MoveInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.Move(Context context)
		{
			Operand result = context.Result;
			Operand operand = context.Operand1;

			X86Instruction instruction = X86.Mov;

			if (result.IsFloatingPoint)
			{
				//Debug.Assert(operand.IsFloatingPoint, @"Move can't convert to floating point type.");

				if (result.Type == operand.Type)
				{
					if (result.IsSingle)
					{
						instruction = X86.Movss;
					}
					else if (result.IsDouble)
					{
						instruction = X86.Movsd;
					}
				}
				else if (result.IsDouble)
				{
					instruction = X86.Cvtss2sd;
				}
				else if (result.IsSingle)
				{
					instruction = X86.Cvtsd2ss;
				}
			}

			context.ReplaceInstructionOnly(instruction);
		}

		/// <summary>
		/// Visitation function for Return.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.Return(Context context)
		{
			Debug.Assert(context.BranchTargets != null);

			if (context.Operand1 != null)
			{
				if (context.Operand1.IsFloatingPoint)
				{
					// HACK - to support test suit on windows
					Operand stack = methodCompiler.StackLayout.AddStackLocal(context.Operand1.Type);
					Context before = context.InsertBefore();
					architecture.InsertMoveInstruction(before, stack, context.Operand1);
					before.AppendInstruction(X86.Fld, null, stack);
				}

				var returnOperand = context.Operand1;

				context.Remove();

				callingConvention.SetReturnValue(typeSystem, context, returnOperand);

				context.AppendInstruction(X86.Jmp);
				context.SetBranch(Int32.MaxValue);
			}
			else
			{
				context.SetInstruction(X86.Jmp);
				context.SetBranch(Int32.MaxValue);
			}
		}

		/// <summary>
		/// Visitation function for InternalReturn.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.InternalReturn(Context context)
		{
			Debug.Assert(context.BranchTargets == null);

			// To return from an internal method call (usually from within a finally or exception clause)
			context.SetInstruction(X86.Ret);
		}

		/// <summary>
		/// Arithmetic the shift right instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.ArithmeticShiftRight(Context context)
		{
			ReplaceInstructionAndAnyFloatingPointConstant(context, X86.Sar);
		}

		/// <summary>
		/// Visitation function for ShiftLeftInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.ShiftLeft(Context context)
		{
			ReplaceInstructionAndAnyFloatingPointConstant(context, X86.Shl);
		}

		/// <summary>
		/// Visitation function for ShiftRightInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.ShiftRight(Context context)
		{
			ReplaceInstructionAndAnyFloatingPointConstant(context, X86.Shr);
		}

		/// <summary>
		/// Visitation function for StoreInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.Store(Context context)
		{
			Operand baseOperand = context.Operand1;
			Operand offsetOperand = context.Operand2;
			Operand value = context.Operand3;

			if (value.IsDouble && baseOperand.Type.ElementType.IsSingle)
			{
				Operand xmm1 = AllocateVirtualRegister(typeSystem.BuiltIn.Single);
				context.InsertBefore().AppendInstruction(X86.Cvtsd2ss, xmm1, value);
				value = xmm1;
			}
			else
			{
				Operand v2 = AllocateVirtualRegister(value.Type);
				context.InsertBefore().AppendInstruction(X86.Mov, v2, value); // FIXME
				value = v2;
			}

			if (offsetOperand.IsConstant)
			{
				Operand mem = Operand.CreateMemoryAddress(baseOperand.Type, baseOperand, offsetOperand.ConstantSignedInteger);

				context.SetInstruction(GetMove(mem, value), mem, value);
			}
			else
			{
				Operand v1 = AllocateVirtualRegister(baseOperand.Type);
				Operand mem = Operand.CreateMemoryAddress(baseOperand.Type, v1, 0);

				context.SetInstruction(X86.Mov, v1, baseOperand);
				context.AppendInstruction(X86.Add, v1, v1, offsetOperand);
				context.AppendInstruction(GetMove(mem, value), mem, value);
			}
		}

		/// <summary>
		/// Visitation function for MulFloat.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.MulFloat(Context context)
		{
			if (context.Result.IsSingle)
				ReplaceInstructionAndAnyFloatingPointConstant(context, X86.Mulss);
			else
				ReplaceInstructionAndAnyFloatingPointConstant(context, X86.Mulsd);
		}

		/// <summary>
		/// Visitation function for SubFloat.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.SubFloat(Context context)
		{
			if (context.Result.IsSingle)
				ReplaceInstructionAndAnyFloatingPointConstant(context, X86.Subss);
			else
				ReplaceInstructionAndAnyFloatingPointConstant(context, X86.Subsd);
		}

		/// <summary>
		/// Visitation function for SubSigned.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.SubSigned(Context context)
		{
			//EmitResultConstants(context);
			EmitFloatingPointConstants(context);
			context.ReplaceInstructionOnly(X86.Sub);
		}

		/// <summary>
		/// Visitation function for SubUnsigned.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.SubUnsigned(Context context)
		{
			//EmitResultConstants(context);
			EmitFloatingPointConstants(context);
			context.ReplaceInstructionOnly(X86.Sub);
		}

		/// <summary>
		/// Visitation function for MulSigned.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.MulSigned(Context context)
		{
			EmitFloatingPointConstants(context);

			Operand result = context.Result;
			Operand operand1 = context.Operand1;
			Operand operand2 = context.Operand2;

			Operand v1 = AllocateVirtualRegister(typeSystem.BuiltIn.UInt32);
			context.SetInstruction2(X86.Mul, v1, result, operand1, operand2);
		}

		/// <summary>
		/// Visitation function for MulUnsigned.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.MulUnsigned(Context context)
		{
			EmitFloatingPointConstants(context);

			Operand result = context.Result;
			Operand operand1 = context.Operand1;
			Operand operand2 = context.Operand2;

			Operand v1 = AllocateVirtualRegister(typeSystem.BuiltIn.UInt32);
			context.SetInstruction2(X86.Mul, v1, result, operand1, operand2);
		}

		/// <summary>
		/// Visitation function for DivSigned.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.DivSigned(Context context)
		{
			EmitFloatingPointConstants(context);

			Operand operand1 = context.Operand1;
			Operand operand2 = context.Operand2;
			Operand result = context.Result;

			Operand v1 = AllocateVirtualRegister(typeSystem.BuiltIn.Int32);
			Operand v2 = AllocateVirtualRegister(typeSystem.BuiltIn.UInt32);
			Operand v3 = AllocateVirtualRegister(typeSystem.BuiltIn.Int32);

			context.SetInstruction2(X86.Cdq, v1, v2, operand1);
			context.AppendInstruction2(X86.IDiv, v3, result, v1, v2, operand2);
		}

		/// <summary>
		/// Visitation function for DivUnsigned.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.DivUnsigned(Context context)
		{
			EmitFloatingPointConstants(context);

			Operand operand1 = context.Operand1;
			Operand operand2 = context.Operand2;
			Operand result = context.Result;

			Operand v1 = AllocateVirtualRegister(typeSystem.BuiltIn.UInt32);
			Operand v2 = AllocateVirtualRegister(typeSystem.BuiltIn.UInt32);

			context.SetInstruction(X86.Mov, v1, Operand.CreateConstantUnsignedInt(typeSystem, (uint)0x0));
			context.AppendInstruction2(X86.Div, v1, v2, v1, operand1, operand2);
			context.AppendInstruction(X86.Mov, result, v1);
		}

		/// <summary>
		/// Visitation function for RemSigned.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.RemSigned(Context context)
		{
			EmitFloatingPointConstants(context);

			Operand result = context.Result;
			Operand operand1 = context.Operand1;
			Operand operand2 = context.Operand2;

			Operand v1 = AllocateVirtualRegister(typeSystem.BuiltIn.Int32);
			Operand v2 = AllocateVirtualRegister(typeSystem.BuiltIn.UInt32);
			Operand v3 = AllocateVirtualRegister(typeSystem.BuiltIn.Int32);

			// FIXME
			context.SetInstruction2(X86.Cdq, v1, v2, operand1);
			context.AppendInstruction2(X86.IDiv, result, v3, v1, v2, operand2);
		}

		/// <summary>
		/// Visitation function for RemUnsigned.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.RemUnsigned(Context context)
		{
			EmitFloatingPointConstants(context);

			Operand result = context.Result;
			Operand operand1 = context.Operand1;
			Operand operand2 = context.Operand2;

			Operand v1 = AllocateVirtualRegister(typeSystem.BuiltIn.UInt32);
			Operand v2 = AllocateVirtualRegister(typeSystem.BuiltIn.UInt32);

			context.SetInstruction(X86.Mov, v1, Operand.CreateConstantUnsignedInt(typeSystem, (uint)0x0));
			context.AppendInstruction2(X86.Div, v1, v2, v1, operand1, operand2);
			context.AppendInstruction(X86.Mov, result, v2);
		}

		/// <summary>
		/// Visitation function for RemFloat.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.RemFloat(Context context)
		{
			EmitFloatingPointConstants(context);

			Operand result = context.Result;
			Operand operand1 = context.Operand1;
			Operand operand2 = context.Operand2;

			Operand xmm1 = AllocateVirtualRegister(typeSystem.BuiltIn.Double);
			Operand xmm2 = AllocateVirtualRegister(typeSystem.BuiltIn.Double);
			Operand xmm3 = AllocateVirtualRegister(typeSystem.BuiltIn.Double);

			context.SetInstruction(X86.Divsd, xmm1, operand1, operand2);
			context.AppendInstruction(X86.Roundsd, xmm2, xmm1, Operand.CreateConstant(typeSystem.BuiltIn.Byte, 0x3));
			context.AppendInstruction(X86.Mulsd, xmm3, operand2, xmm2);
			context.AppendInstruction(X86.Subsd, result, operand1, xmm3);
		}

		/// <summary>
		/// Visitation function for SwitchInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.Switch(Context context)
		{
			int[] targets = context.BranchTargets;
			Operand operand = context.Operand1;

			context.Remove();

			for (int i = 0; i < targets.Length - 1; ++i)
			{
				context.AppendInstruction(X86.Cmp, null, operand, Operand.CreateConstantSignedInt(typeSystem, i));
				context.AppendInstruction(X86.Branch, ConditionCode.Equal);
				context.SetBranch(targets[i]);
			}
		}

		/// <summary>
		/// Visitation function for BreakInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.Break(Context context)
		{
			context.SetInstruction(X86.Break);
		}

		/// <summary>
		/// Visitation function for NopInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.Nop(Context context)
		{
			context.SetInstruction(X86.Nop);
		}

		/// <summary>
		/// Visitation function for SignExtendedMoveInstruction instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.SignExtendedMove(Context context)
		{
			context.ReplaceInstructionOnly(X86.Movsx);
		}

		/// <summary>
		/// Visitation function for Call.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.Call(Context context)
		{
			if (context.OperandCount == 0 && context.BranchTargets != null)
			{
				// inter-method call; usually for exception processing
				context.ReplaceInstructionOnly(X86.Call);
			}
			else
			{
				callingConvention.MakeCall(typeSystem, context);
			}
		}

		/// <summary>
		/// Visitation function for ZeroExtendedMoveInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.ZeroExtendedMove(Context context)
		{
			context.ReplaceInstructionOnly(X86.Movzx);
		}

		/// <summary>
		/// Visitation function for FloatingPointToIntegerConversionInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.FloatToIntegerConversion(Context context)
		{
			Operand source = context.Operand1;
			Operand destination = context.Result;

			if (destination.Type.IsSignedByte || destination.Type.IsSignedShort || destination.Type.IsSignedInt)
			{
				if (source.IsDouble)
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
		/// Visitation function for ThrowInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.Throw(Context context)
		{
			var type = typeSystem.GetTypeByName("Mosa.Internal", "ExceptionEngine");
			var runtimeMethod = TypeSystem.GetMethodByName(type, "ThrowException");
			Operand throwMethod = Operand.CreateSymbolFromMethod(typeSystem, runtimeMethod);

			// Push exception object onto stack
			context.SetInstruction(X86.Push, null, context.Operand1);

			// Save entire CPU context onto stack
			context.AppendInstruction(X86.Pushad);

			// Call exception handling
			context.AppendInstruction(X86.Call, null, throwMethod);
		}

		/// <summary>
		/// Visitation function for ExceptionPrologueInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.ExceptionPrologue(Context context)
		{
			// Exception Handler will pass the exception object in the register - EDX was choosen
			context.SetInstruction(X86.Mov, context.Result, Operand.CreateCPURegister(typeSystem.BuiltIn.Object, GeneralPurposeRegister.EDX));

			// Alternative method is to pop it off the stack instead, going passing via register for now
			//context.SetInstruction(CPUx86.Instruction.PopInstruction, context.Result);
		}

		/// <summary>
		/// Visitation function for IntegerToFloatingPointConversion.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.IntegerToFloatConversion(Context context)
		{
			if (context.Result.IsSingle)
				context.ReplaceInstructionOnly(X86.Cvtsi2ss);
			else if (context.Result.IsDouble)
				context.ReplaceInstructionOnly(X86.Cvtsi2sd);
			else
				throw new NotSupportedException();
		}

		#endregion IIRVisitor

		#region IIRVisitor - Unused

		/// <summary>
		/// Visitation function for PrologueInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.Prologue(Context context)
		{
		}

		/// <summary>
		/// Visitation function for EpilogueInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.Epilogue(Context context)
		{
		}

		/// <summary>
		/// Visitation function for PhiInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.Phi(Context context)
		{
		}

		/// <summary>
		/// Visitation function for intrinsic the method call.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.IntrinsicMethodCall(Context context)
		{
		}

		#endregion IIRVisitor - Unused

		#region Internals

		/// <summary>
		/// Removes the any floating point constant and replace instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="instruction">The instruction.</param>
		private void ReplaceInstructionAndAnyFloatingPointConstant(Context context, BaseInstruction instruction)
		{
			EmitFloatingPointConstants(context);
			context.ReplaceInstructionOnly(instruction);
		}

		#endregion Internals
	}
}