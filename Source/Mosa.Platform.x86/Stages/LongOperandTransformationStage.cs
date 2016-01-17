// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;
using System;
using System.Diagnostics;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	/// Transforms 64-bit arithmetic to 32-bit operations.
	/// </summary>
	/// <remarks>
	/// This stage translates all 64-bit operations to appropriate 32-bit operations on
	/// architectures without appropriate 64-bit integral operations.
	/// </remarks>
	public sealed class LongOperandTransformationStage : BaseTransformationStage, IIRVisitor
	{
		#region Utility Methods

		public static void SplitLongOperand(BaseMethodCompiler methodCompiler, Operand operand, out Operand operandLow, out Operand operandHigh, Operand ConstantZero)
		{
			if (operand.Is64BitInteger)
			{
				methodCompiler.VirtualRegisters.SplitLongOperand(methodCompiler.TypeSystem, operand, 4, 0);
				operandLow = operand.Low;
				operandHigh = operand.High;
				return;
			}
			else
			{
				operandLow = operand;
				operandHigh = ConstantZero;
				return;
			}

			//throw new InvalidProgramException("@can not split" + operand.ToString());
		}

		private void SplitLongOperand(Operand operand, out Operand operandLow, out Operand operandHigh)
		{
			SplitLongOperand(MethodCompiler, operand, out operandLow, out operandHigh, ConstantZero);
		}

		private void ReplaceWithDivisionCall(Context context, string methodName)
		{
			var type = TypeSystem.GetTypeByName("Mosa.Platform.Internal.x86", "Division");

			Debug.Assert(type != null, "Cannot find type: Mosa.Platform.Internal.x86.Division type");

			var method = type.FindMethodByName(methodName);

			Debug.Assert(method != null, "Cannot find method: " + methodName);

			context.ReplaceInstructionOnly(IRInstruction.Call);
			context.SetOperand(0, Operand.CreateSymbolFromMethod(TypeSystem, method));
			context.OperandCount = 1;
			context.InvokeMethod = method;
		}

		/// <summary>
		/// Expands the add instruction for 64-bit operands.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ExpandAdd(Context context)
		{
			Operand op0H, op1H, op2H, op0L, op1L, op2L;
			SplitLongOperand(context.Result, out op0L, out op0H);
			SplitLongOperand(context.Operand1, out op1L, out op1H);
			SplitLongOperand(context.Operand2, out op2L, out op2H);

			Operand v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			Operand v2 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);

			context.SetInstruction(X86.Mov, v2, op1L);
			context.AppendInstruction(X86.Add, v2, v2, op2L);
			context.AppendInstruction(X86.Mov, op0L, v2);
			context.AppendInstruction(X86.Mov, v1, op1H);
			context.AppendInstruction(X86.Adc, v1, v1, op2H);
			if (!op0H.IsConstantZero)
				context.AppendInstruction(X86.Mov, op0H, v1);
		}

		/// <summary>
		/// Expands the sub instruction for 64-bit operands.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ExpandSub(Context context)
		{
			Operand op0H, op1H, op2H, op0L, op1L, op2L;
			SplitLongOperand(context.Result, out op0L, out op0H);
			SplitLongOperand(context.Operand1, out op1L, out op1H);
			SplitLongOperand(context.Operand2, out op2L, out op2H);

			Operand v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			Operand v2 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);

			context.SetInstruction(X86.Mov, v2, op1L);
			context.AppendInstruction(X86.Sub, v2, v2, op2L);
			context.AppendInstruction(X86.Mov, op0L, v2);
			context.AppendInstruction(X86.Mov, v1, op1H);
			context.AppendInstruction(X86.Sbb, v1, v1, op2H);
			if (!op0H.IsConstantZero)
				context.AppendInstruction(X86.Mov, op0H, v1);
		}

		/// <summary>
		/// Expands the mul instruction for 64-bit operands.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ExpandMul(Context context)
		{
			Operand op0H, op1H, op2H, op0L, op1L, op2L;
			SplitLongOperand(context.Result, out op0L, out op0H);
			SplitLongOperand(context.Operand1, out op1L, out op1H);
			SplitLongOperand(context.Operand2, out op2L, out op2H);

			Operand eax = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			Operand edx = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			Operand ebx = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			Operand v20 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			Operand v12 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			// unoptimized
			context.SetInstruction(X86.Mov, eax, op2L);
			context.AppendInstruction(X86.Mov, v20, eax);
			context.AppendInstruction(X86.Mov, eax, v20);
			context.AppendInstruction2(X86.Mul, edx, eax, eax, op1L);
			context.AppendInstruction(X86.Mov, op0L, eax);

			if (!op0H.IsConstantZero)
			{
				context.AppendInstruction(X86.Mov, v12, edx);
				context.AppendInstruction(X86.Mov, eax, op1L);
				context.AppendInstruction(X86.Mov, ebx, eax);
				context.AppendInstruction(X86.IMul, ebx, ebx, op2H);
				context.AppendInstruction(X86.Mov, eax, v12);
				context.AppendInstruction(X86.Add, eax, eax, ebx);
				context.AppendInstruction(X86.Mov, ebx, op2L);
				context.AppendInstruction(X86.IMul, ebx, ebx, op1H);
				context.AppendInstruction(X86.Add, eax, eax, ebx);
				context.AppendInstruction(X86.Mov, v12, eax);
				context.AppendInstruction(X86.Mov, op0H, v12);
			}
		}

		/// <summary>
		/// Expands the div.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ExpandDiv(Context context)
		{
			Operand op0H, op1H, op2H, op0L, op1L, op2L;
			var result = context.Result;
			var op1 = context.Operand1;
			var op2 = context.Operand2;

			SplitLongOperand(result, out op0L, out op0H);
			SplitLongOperand(op1, out op1L, out op1H);
			SplitLongOperand(op2, out op2L, out op2H);

			ReplaceWithDivisionCall(context, "sdiv64");
			context.Result = result;
			context.Operand2 = op1;
			context.Operand3 = op2;
			context.OperandCount = 3;
			context.ResultCount = 1;
		}

		/// <summary>
		/// Expands the rem.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ExpandRem(Context context)
		{
			Operand op0H, op1H, op2H, op0L, op1L, op2L;
			var result = context.Result;
			var op1 = context.Operand1;
			var op2 = context.Operand2;

			SplitLongOperand(result, out op0L, out op0H);
			SplitLongOperand(op1, out op1L, out op1H);
			SplitLongOperand(op2, out op2L, out op2H);

			ReplaceWithDivisionCall(context, "smod64");
			context.Result = result;
			context.Operand2 = op1;
			context.Operand3 = op2;
			context.OperandCount = 3;
			context.ResultCount = 1;
		}

		/// <summary>
		/// Expands the udiv instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ExpandUDiv(Context context)
		{
			Operand op0H, op1H, op2H, op0L, op1L, op2L;
			var result = context.Result;
			var op1 = context.Operand1;
			var op2 = context.Operand2;

			SplitLongOperand(result, out op0L, out op0H);
			SplitLongOperand(op1, out op1L, out op1H);
			SplitLongOperand(op2, out op2L, out op2H);

			ReplaceWithDivisionCall(context, "udiv64");
			context.Result = result;
			context.Operand2 = op1;
			context.Operand3 = op2;
			context.OperandCount = 3;
			context.ResultCount = 1;
		}

		/// <summary>
		/// Expands the urem instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ExpandURem(Context context)
		{
			Operand op0H, op1H, op2H, op0L, op1L, op2L;
			var result = context.Result;
			var op1 = context.Operand1;
			var op2 = context.Operand2;

			SplitLongOperand(result, out op0L, out op0H);
			SplitLongOperand(op1, out op1L, out op1H);
			SplitLongOperand(op2, out op2L, out op2H);

			ReplaceWithDivisionCall(context, "umod64");
			context.Result = result;
			context.Operand2 = op1;
			context.Operand3 = op2;
			context.OperandCount = 3;
			context.ResultCount = 1;
		}

		/// <summary>
		/// Expands the arithmetic shift right instruction for 64-bits.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ExpandArithmeticShiftRight(Context context)
		{
			Operand count = context.Operand2;

			Operand op0H, op1H, op0L, op1L;
			SplitLongOperand(context.Result, out op0L, out op0H);
			SplitLongOperand(context.Operand1, out op1L, out op1H);

			Operand eax = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			Operand edx = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			Operand ecx = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			Context[] newBlocks = CreateNewBlockContexts(6);
			Context nextBlock = Split(context);

			context.SetInstruction(X86.Jmp, newBlocks[0].Block);

			newBlocks[0].AppendInstruction(X86.Mov, ecx, count);
			newBlocks[0].AppendInstruction(X86.Mov, edx, op1H);
			newBlocks[0].AppendInstruction(X86.Mov, eax, op1L);
			newBlocks[0].AppendInstruction(X86.Cmp, null, ecx, Operand.CreateConstant(TypeSystem, 64));
			newBlocks[0].AppendInstruction(X86.Branch, ConditionCode.UnsignedGreaterOrEqual, newBlocks[4].Block);
			newBlocks[0].AppendInstruction(X86.Jmp, newBlocks[1].Block);

			newBlocks[1].AppendInstruction(X86.Cmp, null, ecx, Operand.CreateConstant(TypeSystem, 32));
			newBlocks[1].AppendInstruction(X86.Branch, ConditionCode.UnsignedGreaterOrEqual, newBlocks[3].Block);
			newBlocks[1].AppendInstruction(X86.Jmp, newBlocks[2].Block);

			newBlocks[2].AppendInstruction(X86.Shrd, eax, eax, edx, ecx);
			newBlocks[2].AppendInstruction(X86.Sar, edx, edx, ecx);
			newBlocks[2].AppendInstruction(X86.Jmp, newBlocks[5].Block);

			newBlocks[3].AppendInstruction(X86.Mov, eax, edx);
			newBlocks[3].AppendInstruction(X86.Sar, edx, edx, Operand.CreateConstant(TypeSystem, 0x1F));
			newBlocks[3].AppendInstruction(X86.And, ecx, ecx, Operand.CreateConstant(TypeSystem, 0x1F));
			newBlocks[3].AppendInstruction(X86.Sar, eax, eax, ecx);
			newBlocks[3].AppendInstruction(X86.Jmp, newBlocks[5].Block);

			newBlocks[4].AppendInstruction(X86.Sar, edx, edx, Operand.CreateConstant(TypeSystem, 0x1F));
			newBlocks[4].AppendInstruction(X86.Mov, eax, edx);
			newBlocks[4].AppendInstruction(X86.Jmp, newBlocks[5].Block);

			newBlocks[5].AppendInstruction(X86.Mov, op0H, edx);
			newBlocks[5].AppendInstruction(X86.Mov, op0L, eax);
			newBlocks[5].AppendInstruction(X86.Jmp, nextBlock.Block);
		}

		/// <summary>
		/// Expands the shift left instruction for 64-bits.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ExpandShiftLeft(Context context)
		{
			Operand count = context.Operand2;

			Operand op0H, op1H, op0L, op1L;
			SplitLongOperand(context.Result, out op0L, out op0H);
			SplitLongOperand(context.Operand1, out op1L, out op1H);

			Operand eax = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			Operand edx = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			Operand ecx = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			Context nextBlock = Split(context);
			Context[] newBlocks = CreateNewBlockContexts(6);

			context.SetInstruction(X86.Jmp, newBlocks[0].Block);

			newBlocks[0].AppendInstruction(X86.Mov, ecx, count);
			newBlocks[0].AppendInstruction(X86.Mov, edx, op1H);
			newBlocks[0].AppendInstruction(X86.Mov, eax, op1L);
			newBlocks[0].AppendInstruction(X86.Cmp, null, ecx, Operand.CreateConstant(TypeSystem, 64));
			newBlocks[0].AppendInstruction(X86.Branch, ConditionCode.UnsignedGreaterOrEqual, newBlocks[4].Block);
			newBlocks[0].AppendInstruction(X86.Jmp, newBlocks[1].Block);

			newBlocks[1].AppendInstruction(X86.Cmp, null, ecx, Operand.CreateConstant(TypeSystem, 32));
			newBlocks[1].AppendInstruction(X86.Branch, ConditionCode.UnsignedGreaterOrEqual, newBlocks[3].Block);
			newBlocks[1].AppendInstruction(X86.Jmp, newBlocks[2].Block);

			newBlocks[2].AppendInstruction(X86.Shld, edx, edx, eax, ecx);
			newBlocks[2].AppendInstruction(X86.Shl, eax, eax, ecx);
			newBlocks[2].AppendInstruction(X86.Jmp, newBlocks[5].Block);

			newBlocks[3].AppendInstruction(X86.Mov, edx, eax);
			newBlocks[3].AppendInstruction(X86.Mov, eax, ConstantZero);
			newBlocks[3].AppendInstruction(X86.And, ecx, ecx, Operand.CreateConstant(TypeSystem, 0x1F));
			newBlocks[3].AppendInstruction(X86.Shl, edx, edx, ecx);
			newBlocks[3].AppendInstruction(X86.Jmp, newBlocks[5].Block);

			newBlocks[4].AppendInstruction(X86.Mov, eax, ConstantZero);
			newBlocks[4].AppendInstruction(X86.Mov, edx, ConstantZero);
			newBlocks[4].AppendInstruction(X86.Jmp, newBlocks[5].Block);

			newBlocks[5].AppendInstruction(X86.Mov, op0H, edx);
			newBlocks[5].AppendInstruction(X86.Mov, op0L, eax);
			newBlocks[5].AppendInstruction(X86.Jmp, nextBlock.Block);
		}

		/// <summary>
		/// Expands the shift right instruction for 64-bits.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ExpandShiftRight(Context context)
		{
			Operand count = context.Operand2;

			Operand op0H, op1H, op0L, op1L;
			SplitLongOperand(context.Result, out op0L, out op0H);
			SplitLongOperand(context.Operand1, out op1L, out op1H);

			Operand ecx = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			Operand v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			Context nextBlock = Split(context);
			Context[] newBlocks = CreateNewBlockContexts(4);

			context.SetInstruction(X86.Mov, ecx, count);
			context.AppendInstruction(X86.Cmp, null, ecx, Operand.CreateConstant(TypeSystem, 64));
			context.AppendInstruction(X86.Branch, ConditionCode.UnsignedGreaterOrEqual, newBlocks[3].Block);
			context.AppendInstruction(X86.Jmp, newBlocks[0].Block);

			newBlocks[0].AppendInstruction(X86.Cmp, null, ecx, Operand.CreateConstant(TypeSystem, 32));
			newBlocks[0].AppendInstruction(X86.Branch, ConditionCode.UnsignedGreaterOrEqual, newBlocks[2].Block);
			newBlocks[0].AppendInstruction(X86.Jmp, newBlocks[1].Block);

			newBlocks[1].AppendInstruction(X86.Mov, v1, op1H);
			newBlocks[1].AppendInstruction(X86.Mov, op0L, op1L);
			newBlocks[1].AppendInstruction(X86.Shrd, op0L, op0L, v1, ecx);
			newBlocks[1].AppendInstruction(X86.Shr, v1, v1, ecx);
			if (!op0H.IsConstantZero)
				newBlocks[1].AppendInstruction(X86.Mov, op0H, v1);
			newBlocks[1].AppendInstruction(X86.Jmp, nextBlock.Block);

			newBlocks[2].AppendInstruction(X86.Mov, op0L, op1H);
			if (!op0H.IsConstantZero)
				newBlocks[2].AppendInstruction(X86.Mov, op0H, ConstantZero);
			newBlocks[2].AppendInstruction(X86.And, ecx, ecx, Operand.CreateConstant(TypeSystem, 0x1F));
			newBlocks[2].AppendInstruction(X86.Sar, op0L, op0L, ecx);
			newBlocks[2].AppendInstruction(X86.Jmp, nextBlock.Block);

			newBlocks[3].AppendInstruction(X86.Mov, op0L, op0H);
			if (!op0H.IsConstantZero)
				newBlocks[3].AppendInstruction(X86.Mov, op0H, ConstantZero);
			newBlocks[3].AppendInstruction(X86.Jmp, nextBlock.Block);
		}

		/// <summary>
		/// Expands the neg instruction for 64-bits.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ExpandNeg(Context context)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Expands the not instruction for 64-bits.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ExpandNot(Context context)
		{
			Operand op0H, op1H, op0L, op1L;
			SplitLongOperand(context.Result, out op0L, out op0H);
			SplitLongOperand(context.Operand1, out op1L, out op1H);

			Operand eax = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);

			context.SetInstruction(X86.Mov, eax, op1H);
			context.AppendInstruction(X86.Not, eax, eax);
			context.AppendInstruction(X86.Mov, op0H, eax);

			context.AppendInstruction(X86.Mov, eax, op1L);
			context.AppendInstruction(X86.Not, eax, eax);
			context.AppendInstruction(X86.Mov, op0L, eax);
		}

		/// <summary>
		/// Expands the and instruction for 64-bits.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ExpandAnd(Context context)
		{
			Operand op0H, op1H, op2H, op0L, op1L, op2L;
			SplitLongOperand(context.Result, out op0L, out op0H);
			SplitLongOperand(context.Operand1, out op1L, out op1H);
			SplitLongOperand(context.Operand2, out op2L, out op2H);

			if (!context.Result.Is64BitInteger)
			{
				context.SetInstruction(X86.Mov, op0L, op1L);
				context.AppendInstruction(X86.And, op0L, op0L, op2L);
			}
			else
			{
				context.SetInstruction(X86.Mov, op0H, op1H);
				context.AppendInstruction(X86.Mov, op0L, op1L);
				context.AppendInstruction(X86.And, op0H, op0H, op2H);
				context.AppendInstruction(X86.And, op0L, op0L, op2L);
			}
		}

		/// <summary>
		/// Expands the or instruction for 64-bits.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ExpandOr(Context context)
		{
			Operand op0H, op1H, op2H, op0L, op1L, op2L;
			SplitLongOperand(context.Result, out op0L, out op0H);
			SplitLongOperand(context.Operand1, out op1L, out op1H);
			SplitLongOperand(context.Operand2, out op2L, out op2H);

			context.SetInstruction(X86.Mov, op0H, op1H);
			context.AppendInstruction(X86.Mov, op0L, op1L);
			context.AppendInstruction(X86.Or, op0H, op0H, op2H);
			context.AppendInstruction(X86.Or, op0L, op0L, op2L);
		}

		/// <summary>
		/// Expands the neg instruction for 64-bits.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ExpandXor(Context context)
		{
			Operand op0H, op1H, op2H, op0L, op1L, op2L;
			SplitLongOperand(context.Result, out op0L, out op0H);
			SplitLongOperand(context.Operand1, out op1L, out op1H);
			SplitLongOperand(context.Operand2, out op2L, out op2H);

			context.SetInstruction(X86.Mov, op0H, op1H);
			context.AppendInstruction(X86.Mov, op0L, op1L);
			context.AppendInstruction(X86.Xor, op0H, op0H, op2H);
			context.AppendInstruction(X86.Xor, op0L, op0L, op2L);
		}

		/// <summary>
		/// Expands the move instruction for 64-bits.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ExpandMove(Context context)
		{
			Operand op0L, op0H, op1L, op1H;

			if (context.Result.Is64BitInteger)
			{
				SplitLongOperand(context.Result, out op0L, out op0H);
				SplitLongOperand(context.Operand1, out op1L, out op1H);

				context.SetInstruction(X86.Mov, op0L, op1L);
				context.AppendInstruction(X86.Mov, op0H, op1H);
			}
			else
			{
				SplitLongOperand(context.Operand1, out op1L, out op1H);
				context.SetInstruction(X86.Mov, context.Result, op1L);
			}
		}

		/// <summary>
		/// Expands the unsigned move instruction for 64-bits.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ExpandUnsignedMove(Context context)
		{
			Operand op0 = context.Result;
			Operand op1 = context.Operand1;

			Operand op0L, op0H, op1L, op1H;
			SplitLongOperand(op0, out op0L, out op0H);
			SplitLongOperand(op1, out op1L, out op1H);

			if (op1.IsInt)
			{
				context.SetInstruction(X86.Mov, op0L, op1L);
				context.AppendInstruction(X86.Mov, op0H, ConstantZero);
			}
			else if (op1.IsBoolean || op1.IsChar || op1.IsU1 || op1.IsU2)
			{
				context.SetInstruction(X86.Movzx, op0L, op1L);
				context.AppendInstruction(X86.Mov, op0H, ConstantZero);
			}
			else if (op1.IsU8)
			{
				context.SetInstruction(X86.Mov, op0L, op1L);
				context.AppendInstruction(X86.Mov, op0H, op1H);
			}
			else
			{
				throw new NotSupportedException();
			}
		}

		/// <summary>
		/// Expands the signed move instruction for 64-bits.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ExpandSignedMove(Context context)
		{
			Operand op0 = context.Result;
			Operand op1 = context.Operand1;
			Debug.Assert(op0 != null, @"I8 not in a memory operand!");

			Operand op0L, op0H;
			SplitLongOperand(op0, out op0L, out op0H);

			if (op1.IsBoolean)
			{
				context.SetInstruction(X86.Movzx, op0L, op1);
				context.AppendInstruction(X86.Mov, op0H, ConstantZero);
			}
			else if (op1.IsI1 || op1.IsI2)
			{
				Operand v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				Operand v2 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);
				Operand v3 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);

				context.SetInstruction(X86.Movsx, v1, op1);
				context.AppendInstruction2(X86.Cdq, v3, v2, v1);
				context.AppendInstruction(X86.Mov, op0L, v2);
				context.AppendInstruction(X86.Mov, op0H, v3);
			}
			else if (op1.IsI4 || op1.IsU4 || op1.IsPointer || op1.IsI || op1.IsU)
			{
				Operand v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				Operand v2 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);
				Operand v3 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);

				context.SetInstruction(X86.Mov, v1, op1);
				context.AppendInstruction2(X86.Cdq, v3, v2, v1);
				context.AppendInstruction(X86.Mov, op0L, v2);
				context.AppendInstruction(X86.Mov, op0H, v3);
			}
			else if (op1.IsI8)
			{
				context.SetInstruction(X86.Mov, op0, op1);
			}
			else if (op1.IsU1 || op1.IsU2)
			{
				Operand v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);
				Operand v2 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);
				Operand v3 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);

				context.SetInstruction(X86.Movzx, v1, op1);
				context.AppendInstruction2(X86.Cdq, v3, v2, v1);
				context.AppendInstruction(X86.Mov, op0L, v2);
				context.AppendInstruction(X86.Mov, op0H, ConstantZero);
			}
			else
			{
				throw new InvalidCompilerException();
			}
		}

		/// <summary>
		/// Expands the load instruction for 64-bits.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ExpandLoad(Context context)
		{
			Operand address = context.Operand1;
			Operand offset = context.Operand2;

			Operand op0L, op0H;
			SplitLongOperand(context.Result, out op0L, out op0H);

			Operand v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);
			Operand v2 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);
			Operand v3 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);

			if (offset.IsConstant && offset.IsConstantZero)
			{
				if (address.IsField)
				{
					Debug.Assert(address.Field.IsStatic);

					context.SetInstruction(X86.Lea, v1, address);
				}
				else
				{
					context.SetInstruction(X86.Mov, v1, address);
				}
			}
			else
			{
				context.SetInstruction(X86.Add, v1, address, offset);
			}
			context.AppendInstruction(X86.Mov, v2, Operand.CreateMemoryAddress(TypeSystem.BuiltIn.U4, v1, 0));
			context.AppendInstruction(X86.Mov, op0L, v2);
			context.AppendInstruction(X86.Mov, v3, Operand.CreateMemoryAddress(TypeSystem.BuiltIn.U4, v1, 4));
			context.AppendInstruction(X86.Mov, op0H, v3);
		}

		/// <summary>
		/// Expands the store instruction for 64-bits.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ExpandStore(Context context)
		{
			Operand address = context.Operand1;
			Operand offset = context.Operand2;

			Operand op0L, op0H;
			SplitLongOperand(context.Operand3, out op0L, out op0H);

			Operand v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);

			// Fortunately in 32-bit mode, we can't have 64-bit offsets so a 32-bit add willl work.
			if (offset.IsConstant && offset.IsConstantZero)
			{
				if (address.IsField)
				{
					Debug.Assert(address.Field.IsStatic);

					context.SetInstruction(X86.Lea, v1, address);
				}
				else
				{
					context.SetInstruction(X86.Mov, v1, address);
				}
			}
			else
			{
				context.SetInstruction(X86.Add, v1, address, offset);
			}

			context.AppendInstruction(X86.Mov, Operand.CreateMemoryAddress(TypeSystem.BuiltIn.U4, v1, 0), op0L);
			context.AppendInstruction(X86.Mov, Operand.CreateMemoryAddress(TypeSystem.BuiltIn.U4, v1, 4), op0H);
		}

		/// <summary>
		/// Expands the binary branch instruction for 64-bits.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ExpandBinaryBranch(Context context)
		{
			//Debug.Assert(context.BranchTargets.Length == 1);

			Operand op1L, op1H, op2L, op2H;
			SplitLongOperand(context.Operand1, out op1L, out op1H);
			SplitLongOperand(context.Operand2, out op2L, out op2H);

			BasicBlock target = context.BranchTargets[0];
			ConditionCode conditionCode = context.ConditionCode;

			Context nextBlock = Split(context);
			Context[] newBlocks = CreateNewBlockContexts(2);

			// FIXME: If the conditional branch and unconditional branch are the same, this could cause a problem
			target.PreviousBlocks.Remove(context.Block);

			// The block is being split on the condition, so the new next block has one too many next blocks!
			nextBlock.Block.NextBlocks.Remove(target);

			// Compare high dwords
			context.SetInstruction(X86.Cmp, null, op1H, op2H);
			context.AppendInstruction(X86.Branch, ConditionCode.Equal, newBlocks[1].Block);
			context.AppendInstruction(X86.Jmp, newBlocks[0].Block);

			// Branch if check already gave results
			newBlocks[0].AppendInstruction(X86.Branch, conditionCode, target);
			newBlocks[0].AppendInstruction(X86.Jmp, nextBlock.Block);

			// Compare low dwords
			newBlocks[1].AppendInstruction(X86.Cmp, null, op1L, op2L);
			newBlocks[1].AppendInstruction(X86.Branch, conditionCode.GetUnsigned(), target);
			newBlocks[1].AppendInstruction(X86.Jmp, nextBlock.Block);
		}

		/// <summary>
		/// Expands the binary comparison instruction for 64-bits.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ExpandComparison(Context context)
		{
			Operand op0 = context.Result;
			Operand op1 = context.Operand1;
			Operand op2 = context.Operand2;

			Debug.Assert(op1 != null && op2 != null, @"IntegerCompareInstruction operand not memory!");
			Debug.Assert(op0.IsMemoryAddress || op0.IsRegister, @"IntegerCompareInstruction result not memory and not register!");

			Operand op1L, op1H, op2L, op2H;
			SplitLongOperand(op1, out op1L, out op1H);
			SplitLongOperand(op2, out op2L, out op2H);

			ConditionCode conditionCode = context.ConditionCode;

			Context nextBlock = Split(context);
			Context[] newBlocks = CreateNewBlockContexts(4);

			// Compare high dwords
			context.SetInstruction(X86.Cmp, null, op1H, op2H);
			context.AppendInstruction(X86.Branch, ConditionCode.Equal, newBlocks[1].Block);
			context.AppendInstruction(X86.Jmp, newBlocks[0].Block);

			// Branch if check already gave results
			newBlocks[0].AppendInstruction(X86.Branch, conditionCode, newBlocks[2].Block);
			newBlocks[0].AppendInstruction(X86.Jmp, newBlocks[3].Block);

			// Compare low dwords
			newBlocks[1].AppendInstruction(X86.Cmp, null, op1L, op2L);
			newBlocks[1].AppendInstruction(X86.Branch, conditionCode.GetUnsigned(), newBlocks[2].Block);
			newBlocks[1].AppendInstruction(X86.Jmp, newBlocks[3].Block);

			// Success
			newBlocks[2].AppendInstruction(X86.Mov, op0, Operand.CreateConstant(TypeSystem, 1));
			newBlocks[2].AppendInstruction(X86.Jmp, nextBlock.Block);

			// Failed
			newBlocks[3].AppendInstruction(X86.Mov, op0, ConstantZero);
			newBlocks[3].AppendInstruction(X86.Jmp, nextBlock.Block);
		}

		/// <summary>
		/// Ares the any64 bit.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns></returns>
		public static bool AreAny64Bit(Context context)
		{
			if (context.Result.Is64BitInteger)
				return true;

			foreach (var operand in context.Operands)
				if (operand.Is64BitInteger)
					return true;

			return false;
		}

		#endregion Utility Methods

		#region IIRVisitor

		/// <summary>
		/// Visitation function for ArithmeticShiftRightInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.ArithmeticShiftRight(Context context)
		{
			if (context.Operand1.Is64BitInteger)
			{
				ExpandArithmeticShiftRight(context);
			}
		}

		/// <summary>
		/// Visitation function for IntegerCompareInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.IntegerCompareBranch(Context context)
		{
			if (context.Operand1.Is64BitInteger || context.Operand2.Is64BitInteger)
			{
				ExpandBinaryBranch(context);
			}
		}

		/// <summary>
		/// Visitation function for IntegerCompare.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.IntegerCompare(Context context)
		{
			if (context.Operand1.Is64BitInteger)
			{
				ExpandComparison(context);
			}
		}

		/// <summary>
		/// Visitation function for Load.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.Load(Context context)
		{
			if (context.Operand1.Is64BitInteger || context.Result.Is64BitInteger)
			{
				ExpandLoad(context);
			}
		}

		void IIRVisitor.CompoundLoad(Context context)
		{
		}

		/// <summary>
		/// Visitation function for Load Zero Extended.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.LoadZeroExtended(Context context)
		{
			if (AreAny64Bit(context))
			{
				throw new NotImplementCompilerException("64bit LoadZeroExtended not implemented!");
			}
		}

		/// <summary>
		/// Visitation function for Load Sign Extended.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.LoadSignExtended(Context context)
		{
			if (AreAny64Bit(context))
			{
				throw new NotImplementCompilerException("64bit LoadSignExtended not implemented!");
			}
		}

		/// <summary>
		/// Visitation function for LogicalAndInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.LogicalAnd(Context context)
		{
			if (AreAny64Bit(context))
			{
				ExpandAnd(context);
			}
		}

		/// <summary>
		/// Visitation function for LogicalOrInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.LogicalOr(Context context)
		{
			if (AreAny64Bit(context))
			{
				ExpandOr(context);
			}
		}

		/// <summary>
		/// Visitation function for LogicalXorInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.LogicalXor(Context context)
		{
			if (AreAny64Bit(context))
			{
				ExpandXor(context);
			}
		}

		/// <summary>
		/// Visitation function for LogicalNotInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.LogicalNot(Context context)
		{
			if (AreAny64Bit(context))
			{
				ExpandNot(context);
			}
		}

		/// <summary>
		/// Visitation function for MoveInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.Move(Context context)
		{
			if (AreAny64Bit(context))
			{
				ExpandMove(context);
			}
		}

		void IIRVisitor.CompoundMove(Context context)
		{
		}

		/// <summary>
		/// Visitation function for ShiftLeftInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.ShiftLeft(Context context)
		{
			if (AreAny64Bit(context))
			{
				ExpandShiftLeft(context);
			}
		}

		/// <summary>
		/// Visitation function for ShiftRightInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.ShiftRight(Context context)
		{
			if (AreAny64Bit(context))
			{
				ExpandShiftRight(context);
			}
		}

		/// <summary>
		/// Visitation function for SignExtendedMoveInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.SignExtendedMove(Context context)
		{
			if (AreAny64Bit(context))
			{
				ExpandSignedMove(context);
			}
		}

		/// <summary>
		/// Visitation function for StoreInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.Store(Context context)
		{
			//if ((context.MosaType ?? context.Operand3.Type).IsUI8)
			if (context.MosaType.IsUI8)
			{
				ExpandStore(context);
			}
		}

		void IIRVisitor.CompoundStore(Context context)
		{
		}

		/// <summary>
		/// Visitation function for DivSInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.DivSigned(Context context)
		{
			if (AreAny64Bit(context))
			{
				ExpandDiv(context);
			}
		}

		/// <summary>
		/// Visitation function for DivUInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.DivUnsigned(Context context)
		{
			if (AreAny64Bit(context))
			{
				ExpandUDiv(context);
			}
		}

		/// <summary>
		/// Visitation function for MulSInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.MulSigned(Context context)
		{
			if (AreAny64Bit(context))
			{
				ExpandMul(context);
			}
		}

		/// <summary>
		/// Visitation function for MulUInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.MulUnsigned(Context context)
		{
			if (AreAny64Bit(context))
			{
				ExpandMul(context);
			}
		}

		/// <summary>
		/// Visitation function for SubSInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.SubSigned(Context context)
		{
			if (AreAny64Bit(context))
			{
				ExpandSub(context);
			}
		}

		/// <summary>
		/// Visitation function for SubUInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.SubUnsigned(Context context)
		{
			if (AreAny64Bit(context))
			{
				ExpandSub(context);
			}
		}

		/// <summary>
		/// Visitation function for RemSigned.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.RemSigned(Context context)
		{
			if (AreAny64Bit(context))
			{
				ExpandRem(context);
			}
		}

		/// <summary>
		/// Visitation function for RemUInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.RemUnsigned(Context context)
		{
			if (AreAny64Bit(context))
			{
				ExpandURem(context);
			}
		}

		/// <summary>
		/// Zeroes the extended move instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.ZeroExtendedMove(Context context)
		{
			if (AreAny64Bit(context))
			{
				ExpandUnsignedMove(context);
			}
		}

		/// <summary>
		/// Visitation function for AddSigned.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.AddSigned(Context context)
		{
			if (AreAny64Bit(context))
			{
				ExpandAdd(context);
			}
		}

		/// <summary>
		/// Visitation function for AddUnsigned.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.AddUnsigned(Context context)
		{
			if (AreAny64Bit(context))
			{
				ExpandAdd(context);
			}
		}

		/// <summary>
		/// Visitation function for CallInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.Call(Context context)
		{
			Operand op0L, op0H;

			if (context.Result != null && context.Result.Is64BitInteger)
			{
				SplitLongOperand(context.Result, out op0L, out op0H);
			}

			foreach (var operand in context.Operands)
			{
				if (operand.Is64BitInteger)
				{
					SplitLongOperand(operand, out op0L, out op0H);
				}
			}
		}

		/// <summary>
		/// Visitation function for ReturnInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.Return(Context context)
		{
			Operand op0L, op0H;

			if (context.Result != null && context.Result.Is64BitInteger)
			{
				SplitLongOperand(context.Result, out op0L, out op0H);
			}

			foreach (var operand in context.Operands)
			{
				if (operand.Is64BitInteger)
				{
					SplitLongOperand(operand, out op0L, out op0H);
				}
			}
		}

		#endregion IIRVisitor

		#region IIRVisitor - Unused

		/// <summary>
		/// Visitation function for InternalCall.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.InternalCall(Context context)
		{
		}

		/// <summary>
		/// Visitation function for InternalReturn.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.InternalReturn(Context context)
		{
		}

		/// <summary>
		/// Visitation function for RemFloatInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.RemFloat(Context context)
		{
		}

		/// <summary>
		/// Visitation function for SubFloatInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.SubFloat(Context context)
		{
		}

		/// <summary>
		/// Visitation function for SwitchInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.Switch(Context context)
		{
		}

		/// <summary>
		/// Visitation function for AddFloat.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.AddFloat(Context context)
		{
		}

		/// <summary>
		/// Visitation function for DivFloat.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.DivFloat(Context context)
		{
		}

		/// <summary>
		/// Visitation function for BreakInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.Break(Context context)
		{
		}

		/// <summary>
		/// Visitation function for AddressOfInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.AddressOf(Context context)
		{
		}

		/// <summary>
		/// Visitation function for intrinsic the method call.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.IntrinsicMethodCall(Context context)
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
		/// Visitation function for FloatingPointCompareInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.FloatCompare(Context context)
		{
		}

		/// <summary>
		/// Visitation function for FloatingPointToIntegerConversionInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.FloatToIntegerConversion(Context context)
		{
		}

		/// <summary>
		/// Visitation function for IntegerToFloatingPointConversionInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.IntegerToFloatConversion(Context context)
		{
		}

		/// <summary>
		/// Visitation function for JmpInstruction instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.Jmp(Context context)
		{
		}

		/// <summary>
		/// Visitation function for PhiInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.Phi(Context context)
		{
		}

		/// <summary>
		/// Visitation function for PrologueInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.Prologue(Context context)
		{
		}

		/// <summary>
		/// Visitation function for NopInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.Nop(Context context)
		{
		}

		/// <summary>
		/// Visitation function for MulFloatInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.MulFloat(Context context)
		{
		}

		#endregion IIRVisitor - Unused
	}
}
