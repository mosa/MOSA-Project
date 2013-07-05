/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 *  Scott Balmos <sbalmos@fastmail.fm>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;
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
		private Operand operandConstantZero = Operand.CreateConstant((int)0);

		#region Utility Methods

		private void SplitLongOperand(Operand operand, out Operand operandLow, out Operand operandHigh)
		{
			if (operand.StackType == StackTypeCode.Int64)
			{
				methodCompiler.VirtualRegisters.SplitLongOperand(operand, 4, 0);
				operandLow = operand.Low;
				operandHigh = operand.High;
				return;
			}
			else if (operand.StackType == StackTypeCode.Int32 || operand.StackType == StackTypeCode.Ptr)
			{
				operandLow = operand;
				operandHigh = operandConstantZero;
				return;
			}

			throw new InvalidProgramException("@can not split" + operand.ToString());
		}

		/// <summary>
		/// Expands the add instruction for 64-bit operands.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ExpandAdd(Context context)
		{
			/* This function transforms the ADD into the following sequence of x86 instructions:
			 *
			 * mov eax, [op1]       ; Move lower 32-bits of the first operand into eax
			 * add eax, [op2]       ; Add lower 32-bits of second operand to eax
			 * mov [result], eax    ; Save the result into the lower 32-bits of the result operand
			 * mov eax, [op1+4]     ; Move upper 32-bits of the first operand into eax
			 * adc eax, [op2+4]     ; Add upper 32-bits of the second operand to eax
			 * mov [result+4], eax  ; Save the result into the upper 32-bits of the result operand
			 *
			 */

			Operand op0H, op1H, op2H, op0L, op1L, op2L;
			SplitLongOperand(context.Result, out op0L, out op0H);
			SplitLongOperand(context.Operand1, out op1L, out op1H);
			SplitLongOperand(context.Operand2, out op2L, out op2H);

			Operand eaxH = AllocateVirtualRegister(BuiltInSigType.Int32);
			Operand eaxL = AllocateVirtualRegister(BuiltInSigType.UInt32);

			context.SetInstruction(X86.Mov, eaxL, op1L);
			context.AppendInstruction(X86.Add, eaxL, eaxL, op2L);
			context.AppendInstruction(X86.Mov, op0L, eaxL);
			context.AppendInstruction(X86.Mov, eaxH, op1H);
			context.AppendInstruction(X86.Adc, eaxH, eaxH, op2H);
			context.AppendInstruction(X86.Mov, op0H, eaxH);
		}

		/// <summary>
		/// Expands the sub instruction for 64-bit operands.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ExpandSub(Context context)
		{
			/* This function transforms the SUB into the following sequence of x86 instructions:
			 *
			 * mov eax, [op1]       ; Move lower 32-bits of the first operand into eax
			 * sub eax, [op2]       ; Sub lower 32-bits of second operand to eax
			 * mov [result], eax    ; Save the result into the lower 32-bits of the result operand
			 * mov eax, [op1+4]     ; Move upper 32-bits of the first operand into eax
			 * sbb eax, [op2+4]     ; Sub with borrow upper 32-bits of the second operand to eax
			 * mov [result+4], eax  ; Save the result into the upper 32-bits of the result operand
			 *
			 */

			Operand op0H, op1H, op2H, op0L, op1L, op2L;
			SplitLongOperand(context.Result, out op0L, out op0H);
			SplitLongOperand(context.Operand1, out op1L, out op1H);
			SplitLongOperand(context.Operand2, out op2L, out op2H);

			Operand eaxH = AllocateVirtualRegister(BuiltInSigType.Int32);
			Operand eaxL = AllocateVirtualRegister(BuiltInSigType.UInt32);

			context.SetInstruction(X86.Mov, eaxL, op1L);
			context.AppendInstruction(X86.Sub, eaxL, eaxL, op2L);
			context.AppendInstruction(X86.Mov, op0L, eaxL);
			context.AppendInstruction(X86.Mov, eaxH, op1H);
			context.AppendInstruction(X86.Sbb, eaxH, eaxH, op2H);
			context.AppendInstruction(X86.Mov, op0H, eaxH);
		}

		/// <summary>
		/// Expands the mul instruction for 64-bit operands.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ExpandMul(Context context)
		{
			Operand op0 = context.Result;
			Operand op1 = context.Operand1;
			Operand op2 = context.Operand2;

			Operand op0H, op1H, op2H, op0L, op1L, op2L;
			SplitLongOperand(context.Result, out op0L, out op0H);
			SplitLongOperand(context.Operand1, out op1L, out op1H);
			SplitLongOperand(context.Operand2, out op2L, out op2H);

			//Operand eax = AllocateVirtualRegister(BuiltInSigType.Int32);
			//Operand edx = AllocateVirtualRegister(BuiltInSigType.Int32);
			//Operand ebx = AllocateVirtualRegister(BuiltInSigType.Int32);
			Operand eax = Operand.CreateCPURegister(BuiltInSigType.Int32, GeneralPurposeRegister.EAX);
			Operand edx = Operand.CreateCPURegister(BuiltInSigType.Int32, GeneralPurposeRegister.EDX);
			Operand ebx = Operand.CreateCPURegister(BuiltInSigType.Int32, GeneralPurposeRegister.EBX);
			Operand v16 = AllocateVirtualRegister(BuiltInSigType.Int32);
			Operand v20 = AllocateVirtualRegister(BuiltInSigType.Int32);
			Operand v12 = AllocateVirtualRegister(BuiltInSigType.Int32);

			// unoptimized 
			context.SetInstruction(X86.Mov, eax, op2L);					// mov     eax, [ebp+16]	// 2l
			context.AppendInstruction(X86.Mov, v20, eax);				// mov     [ebp-20], eax
			context.AppendInstruction(X86.Mov, eax, v20);				// mov     eax, [ebp-20]
			context.AppendInstruction2(X86.Mul, edx, eax, eax, op1L);	// mul     [ebp+8]			// 1l
			context.AppendInstruction(X86.Mov, v16, eax);				// mov     [ebp-16], eax
			context.AppendInstruction(X86.Mov, v12, edx);				// mov     [ebp-12], edx
			context.AppendInstruction(X86.Mov, eax, op1L);				// mov     eax, [ebp+8]		// 1l
			context.AppendInstruction(X86.Mov, ebx, eax);				// mov     ebx, eax
			context.AppendInstruction(X86.IMul, ebx, ebx, op2H);		// imul    ebx, [ebp+20]	// 2h
			context.AppendInstruction(X86.Mov, eax, v12);				// mov     eax, [ebp-12]
			context.AppendInstruction(X86.Add, eax, eax, ebx);			// add     eax, ebx
			context.AppendInstruction(X86.Mov, ebx, op2L);				// mov     ebx, [ebp+16]	// 2l
			context.AppendInstruction(X86.IMul, ebx, ebx, op1H);		// imul    ebx, [ebp+12]	// 1h
			context.AppendInstruction(X86.Add, eax, eax, ebx);			// add     eax, ebx
			context.AppendInstruction(X86.Mov, v12, eax);				// mov     [ebp-12], eax
			context.AppendInstruction(X86.Mov, op0L, v16);				// mov     eax, [ebp-16]
			context.AppendInstruction(X86.Mov, op0H, v12);				// mov     edx, [ebp-12]
		}

		/// <summary>
		/// Expands the div.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ExpandDiv(Context context)
		{
			Operand op0H, op1H, op2H, op0L, op1L, op2L;

			SplitLongOperand(context.Result, out op0L, out op0H);
			SplitLongOperand(context.Operand1, out op1L, out op1H);
			SplitLongOperand(context.Operand2, out op2L, out op2H);

			Context[] newBlocks = CreateNewBlocksWithContexts(17);
			Context nextBlock = Split(context, false);

			Operand eax = AllocateVirtualRegister(BuiltInSigType.Int32);
			Operand edx = AllocateVirtualRegister(BuiltInSigType.Int32);
			Operand ebx = AllocateVirtualRegister(BuiltInSigType.Int32);
			Operand ecx = AllocateVirtualRegister(BuiltInSigType.Int32);
			Operand edi = AllocateVirtualRegister(BuiltInSigType.Int32);
			Operand esi = AllocateVirtualRegister(BuiltInSigType.Int32);

			Operand v8 = AllocateVirtualRegister(BuiltInSigType.Int32);

			Operand ConstantByte1 = Operand.CreateConstant(BuiltInSigType.Byte, 1);

			// ; Determine sign of the result (edi = 0 if result is positive, non-zero
			// ; otherwise) and make operands positive.
			// xor     edi,edi         ; result sign assumed positive
			// mov     eax,HIWORD(DVND) ; hi word of a
			// or      eax,eax         ; test to see if signed
			// jge     short L1        ; skip rest if a is already positive
			// inc     edi             ; complement result sign flag
			// mov     edx,LOWORD(DVND) ; lo word of a
			// neg     eax             ; make a positive
			// neg     edx
			// sbb     eax,0
			// mov     HIWORD(DVND),eax ; save positive value
			// mov     LOWORD(DVND),edx
			context.SetInstruction(X86.Jmp, newBlocks[0].BasicBlock);
			LinkBlocks(context, newBlocks[0]);

			newBlocks[0].AppendInstruction(X86.Mov, edi, Operand.CreateConstant((uint)0));
			newBlocks[0].AppendInstruction(X86.Mov, eax, op1H);
			newBlocks[0].AppendInstruction(X86.Or, eax, eax, eax);
			newBlocks[0].AppendInstruction(X86.Branch, ConditionCode.GreaterOrEqual, newBlocks[2].BasicBlock);
			newBlocks[0].AppendInstruction(X86.Jmp, newBlocks[1].BasicBlock);
			LinkBlocks(newBlocks[0], newBlocks[1], newBlocks[2]);

			newBlocks[1].AppendInstruction(X86.Inc, edi, edi);
			newBlocks[1].AppendInstruction(X86.Mov, edx, op1L);
			newBlocks[1].AppendInstruction(X86.Neg, eax, eax);
			newBlocks[1].AppendInstruction(X86.Neg, edx, edx);
			newBlocks[1].AppendInstruction(X86.Sbb, eax, eax, Operand.CreateConstant((int)0));
			newBlocks[1].AppendInstruction(X86.Mov, op1H, eax);
			newBlocks[1].AppendInstruction(X86.Mov, op1L, edx);
			newBlocks[1].AppendInstruction(X86.Jmp, newBlocks[2].BasicBlock);
			LinkBlocks(newBlocks[1], newBlocks[2]);

			// L1:
			//
			// mov     eax,HIWORD(DVSR) ; hi word of b
			// or      eax,eax         ; test to see if signed
			// jge     short L2        ; skip rest if b is already positive
			// inc     edi             ; complement the result sign flag
			// mov     edx,LOWORD(DVSR) ; lo word of a
			// neg     eax             ; make b positive
			// neg     edx
			// sbb     eax,0
			// mov     HIWORD(DVSR),eax ; save positive value
			// mov     LOWORD(DVSR),edx
			newBlocks[2].AppendInstruction(X86.Mov, eax, op2H);
			newBlocks[2].AppendInstruction(X86.Or, eax, eax, eax);
			newBlocks[2].AppendInstruction(X86.Branch, ConditionCode.GreaterOrEqual, newBlocks[4].BasicBlock);
			newBlocks[2].AppendInstruction(X86.Jmp, newBlocks[3].BasicBlock);
			LinkBlocks(newBlocks[2], newBlocks[3], newBlocks[4]);

			newBlocks[3].AppendInstruction(X86.Inc, edi, edi);
			newBlocks[3].AppendInstruction(X86.Mov, edx, op2L); // ??
			newBlocks[3].AppendInstruction(X86.Neg, eax, eax);
			newBlocks[3].AppendInstruction(X86.Neg, edx, edx);
			newBlocks[3].AppendInstruction(X86.Sbb, eax, eax, Operand.CreateConstant((int)0));
			newBlocks[3].AppendInstruction(X86.Mov, op2H, eax);
			newBlocks[3].AppendInstruction(X86.Mov, op2L, edx);
			newBlocks[3].AppendInstruction(X86.Jmp, newBlocks[4].BasicBlock);
			LinkBlocks(newBlocks[3], newBlocks[4]);

			// L2:
			//
			// ;
			// ; Now do the divide.  First look to see if the divisor is less than 4194304K.
			// ; If so, then we can use a simple algorithm with word divides, otherwise
			// ; things get a little more complex.
			// ;
			// ; NOTE - eax currently contains the high order word of DVSR
			// ;
			//
			// or      eax,eax         ; check to see if divisor < 4194304K
			// jnz     short L3        ; nope, gotta do this the hard way

			newBlocks[4].AppendInstruction(X86.Or, eax, eax, eax);
			newBlocks[4].AppendInstruction(X86.Branch, ConditionCode.NoZero, newBlocks[6].BasicBlock);
			newBlocks[4].AppendInstruction(X86.Jmp, newBlocks[5].BasicBlock);
			LinkBlocks(newBlocks[4], newBlocks[5], newBlocks[6]);

			// mov     ecx,LOWORD(DVSR) ; load divisor
			// mov     eax,HIWORD(DVND) ; load high word of dividend
			// xor     edx,edx
			// div     ecx             ; eax <- high order bits of quotient
			// mov     ebx,eax         ; save high bits of quotient
			// mov     eax,LOWORD(DVND) ; edx:eax <- remainder:lo word of dividend
			// div     ecx             ; eax <- low order bits of quotient
			// mov     edx,ebx         ; edx:eax <- quotient
			// jmp     short L4        ; set sign, restore stack and return

			newBlocks[5].AppendInstruction(X86.Mov, ecx, op2L);
			newBlocks[5].AppendInstruction(X86.Mov, eax, op1H);
			newBlocks[5].AppendInstruction(X86.Mov, edx, Operand.CreateConstant(0));
			newBlocks[5].AppendInstruction2(X86.Div, edx, eax, edx, eax, ecx);
			newBlocks[5].AppendInstruction(X86.Mov, ebx, eax);
			newBlocks[5].AppendInstruction(X86.Mov, eax, op1H);
			newBlocks[5].AppendInstruction2(X86.Div, edx, eax, edx, eax, ecx);
			newBlocks[5].AppendInstruction(X86.Mov, edx, ebx);
			newBlocks[5].AppendInstruction(X86.Jmp, newBlocks[14].BasicBlock);
			LinkBlocks(newBlocks[5], newBlocks[14]);

			// Here we do it the hard way.  Remember, eax contains the high word of DVSR
			//
			// L3:
			//        mov     ebx,eax         ; ebx:ecx <- divisor
			//        mov     ecx,LOWORD(DVSR)
			//        mov     edx,HIWORD(DVND) ; edx:eax <- dividend
			//        mov     eax,LOWORD(DVND)
			newBlocks[6].AppendInstruction(X86.Mov, ebx, eax);
			newBlocks[6].AppendInstruction(X86.Mov, ecx, op2L);
			newBlocks[6].AppendInstruction(X86.Mov, edx, op1H);
			newBlocks[6].AppendInstruction(X86.Mov, eax, op1L);
			newBlocks[6].AppendInstruction(X86.Jmp, newBlocks[7].BasicBlock);
			LinkBlocks(newBlocks[6], newBlocks[7]);

			// L5:
			//
			// shr     ebx,1           ; shift divisor right one bit
			// rcr     ecx,1
			// shr     edx,1           ; shift dividend right one bit
			// rcr     eax,1
			// or      ebx,ebx
			// jnz     short L5        ; loop until divisor < 4194304K
			// div     ecx             ; now divide, ignore remainder
			// mov     esi,eax         ; save quotient

			// ; We may be off by one, so to check, we will multiply the quotient
			// ; by the divisor and check the result against the original dividend
			// ; Note that we must also check for overflow, which can occur if the
			// ; dividend is close to 2**64 and the quotient is off by 1.

			// mul     dword ptr HIWORD(DVSR) ; QUOT * HIWORD(DVSR)
			// mov     ecx,eax
			// mov     eax,LOWORD(DVSR)
			// mul     esi             ; QUOT * LOWORD(DVSR)
			// add     edx,ecx         ; EDX:EAX = QUOT * DVSR
			// jc      short L6        ; carry means Quotient is off by 1
			newBlocks[7].AppendInstruction(X86.Shr, ebx, ebx, ConstantByte1);
			newBlocks[7].AppendInstruction(X86.Rcr, ecx, ecx, ConstantByte1);
			newBlocks[7].AppendInstruction(X86.Shr, edx, edx, ConstantByte1);
			newBlocks[7].AppendInstruction(X86.Rcr, eax, eax, ConstantByte1);
			newBlocks[7].AppendInstruction(X86.Or, ebx, ebx, ebx);
			newBlocks[7].AppendInstruction(X86.Branch, ConditionCode.NotEqual, newBlocks[7].BasicBlock);
			newBlocks[7].AppendInstruction(X86.Jmp, newBlocks[8].BasicBlock);
			LinkBlocks(newBlocks[7], newBlocks[7], newBlocks[8]);

			newBlocks[8].AppendInstruction2(X86.Div, eax, edx, eax, edx, ecx);
			newBlocks[8].AppendInstruction(X86.Mov, esi, eax);

			newBlocks[8].AppendInstruction2(X86.Mul, edx, eax, eax, op2H);
			newBlocks[8].AppendInstruction(X86.Mov, ecx, eax);
			newBlocks[8].AppendInstruction(X86.Mov, eax, op2L);

			newBlocks[8].AppendInstruction2(X86.Mul, edx, eax, eax, esi);
			newBlocks[8].AppendInstruction(X86.Mov, ecx, eax);
			newBlocks[8].AppendInstruction(X86.Add, edx, edx, ecx);
			newBlocks[8].AppendInstruction(X86.Branch, ConditionCode.UnsignedLessThan, newBlocks[12].BasicBlock);
			newBlocks[8].AppendInstruction(X86.Jmp, newBlocks[9].BasicBlock);
			LinkBlocks(newBlocks[8], newBlocks[9], newBlocks[12]);

			newBlocks[9].AppendInstruction(X86.Cmp, null, edx, op1H);
			newBlocks[9].AppendInstruction(X86.Branch, ConditionCode.UnsignedGreaterThan, newBlocks[12].BasicBlock);
			newBlocks[9].AppendInstruction(X86.Jmp, newBlocks[10].BasicBlock);
			LinkBlocks(newBlocks[9], newBlocks[10], newBlocks[12]);

			newBlocks[10].AppendInstruction(X86.Branch, ConditionCode.UnsignedLessThan, newBlocks[13].BasicBlock);
			newBlocks[10].AppendInstruction(X86.Jmp, newBlocks[11].BasicBlock);
			LinkBlocks(newBlocks[10], newBlocks[11], newBlocks[13]);

			newBlocks[11].AppendInstruction(X86.Cmp, null, eax, op1L);
			newBlocks[11].AppendInstruction(X86.Branch, ConditionCode.UnsignedLessOrEqual, newBlocks[13].BasicBlock);
			newBlocks[11].AppendInstruction(X86.Jmp, newBlocks[12].BasicBlock);
			LinkBlocks(newBlocks[11], newBlocks[12], newBlocks[13]);

			// L6:
			newBlocks[12].AppendInstruction(X86.Dec, esi, esi);
			newBlocks[12].AppendInstruction(X86.Jmp, newBlocks[13].BasicBlock);
			LinkBlocks(newBlocks[12], newBlocks[13]);

			// L7:
			newBlocks[13].AppendInstruction(X86.Mov, edx, Operand.CreateConstant((uint)0));
			newBlocks[13].AppendInstruction(X86.Mov, eax, esi);
			newBlocks[13].AppendInstruction(X86.Jmp, newBlocks[14].BasicBlock);
			LinkBlocks(newBlocks[13], newBlocks[14]);

			// ; Just the cleanup left to do.  edx:eax contains the quotient.  Set the sign
			// ; according to the save value, cleanup the stack, and return.
			// ;
			// L4:
			//        dec     edi             ; check to see if result is negative
			//        jnz     short L8        ; if EDI == 0, result should be negative
			//        neg     edx             ; otherwise, negate the result
			//        neg     eax
			//        sbb     edx,0
			newBlocks[14].AppendInstruction(X86.Dec, edi, edi);
			newBlocks[14].AppendInstruction(X86.Branch, ConditionCode.NotEqual, newBlocks[16].BasicBlock);
			newBlocks[14].AppendInstruction(X86.Jmp, newBlocks[15].BasicBlock);
			LinkBlocks(newBlocks[14], newBlocks[15], newBlocks[16]);

			newBlocks[15].AppendInstruction(X86.Neg, edx, edx);
			newBlocks[15].AppendInstruction(X86.Neg, eax, eax);
			newBlocks[15].AppendInstruction(X86.Sbb, edx, edx, Operand.CreateConstant((int)0));
			newBlocks[15].AppendInstruction(X86.Jmp, newBlocks[16].BasicBlock);
			LinkBlocks(newBlocks[15], newBlocks[16]);

			newBlocks[16].AppendInstruction(X86.Mov, op0L, eax);
			newBlocks[16].AppendInstruction(X86.Mov, op0H, edx);
			newBlocks[16].AppendInstruction(X86.Jmp, nextBlock.BasicBlock);
			LinkBlocks(newBlocks[16], nextBlock);
		}

		/// <summary>
		/// Expands the rem.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ExpandRem(Context context)
		{
			Operand op0H, op1H, op2H, op0L, op1L, op2L;
			SplitLongOperand(context.Result, out op0L, out op0H);
			SplitLongOperand(context.Operand1, out op1L, out op1H);
			SplitLongOperand(context.Operand2, out op2L, out op2H);

			Operand eax = AllocateVirtualRegister(BuiltInSigType.Int32);
			Operand edx = AllocateVirtualRegister(BuiltInSigType.Int32);
			Operand ebx = AllocateVirtualRegister(BuiltInSigType.Int32);
			Operand ecx = AllocateVirtualRegister(BuiltInSigType.Int32);
			Operand edi = AllocateVirtualRegister(BuiltInSigType.Int32);
			Operand esi = AllocateVirtualRegister(BuiltInSigType.Int32);

			Operand v8 = AllocateVirtualRegister(BuiltInSigType.Int32);

			Context[] newBlocks = CreateNewBlocksWithContexts(16);
			Context nextBlock = Split(context, false);

			// Determine sign of the result (edi = 0 if result is positive, non-zero
			// otherwise) and make operands positive.
			//    xor     edi,edi         ; result sign assumed positive
			//mov     eax,HIWORD(DVND) ; hi word of a
			//or      eax,eax         ; test to see if signed
			//jge     short L1        ; skip rest if a is already positive
			//inc     edi             ; complement result sign flag bit
			//mov     edx,LOWORD(DVND) ; lo word of a
			//neg     eax             ; make a positive
			//neg     edx
			//sbb     eax,0
			//mov     HIWORD(DVND),eax ; save positive value
			//mov     LOWORD(DVND),edx
			context.SetInstruction(X86.Jmp, newBlocks[0].BasicBlock);
			LinkBlocks(context, newBlocks[0]);

			newBlocks[0].AppendInstruction(X86.Mov, edi, Operand.CreateConstant((uint)0));
			newBlocks[0].AppendInstruction(X86.Mov, eax, op1H);
			newBlocks[0].AppendInstruction(X86.Or, eax, eax, eax);
			newBlocks[0].AppendInstruction(X86.Branch, ConditionCode.GreaterOrEqual, newBlocks[2].BasicBlock);
			newBlocks[0].AppendInstruction(X86.Jmp, newBlocks[1].BasicBlock);
			LinkBlocks(newBlocks[0], newBlocks[2], newBlocks[1]);

			newBlocks[1].AppendInstruction(X86.Inc, edi, edi);
			newBlocks[1].AppendInstruction(X86.Mov, edx, op1L);
			newBlocks[1].AppendInstruction(X86.Neg, eax, eax);
			newBlocks[1].AppendInstruction(X86.Neg, edx, edx);
			newBlocks[1].AppendInstruction(X86.Sbb, eax, eax, Operand.CreateConstant((int)0));
			newBlocks[1].AppendInstruction(X86.Mov, op1H, eax);
			newBlocks[1].AppendInstruction(X86.Mov, op1L, edx);
			newBlocks[1].AppendInstruction(X86.Jmp, newBlocks[2].BasicBlock);
			LinkBlocks(newBlocks[1], newBlocks[2]);

			// L1:
			//
			// mov     eax,HIWORD(DVSR) ; hi word of b
			// or      eax,eax         ; test to see if signed
			// jge     short L2        ; skip rest if b is already positive
			// mov     edx,LOWORD(DVSR) ; lo word of b
			// neg     eax             ; make b positive
			// neg     edx
			// sbb     eax,0
			// mov     HIWORD(DVSR),eax ; save positive value
			// mov     LOWORD(DVSR),edx
			newBlocks[2].AppendInstruction(X86.Mov, eax, op2H);
			newBlocks[2].AppendInstruction(X86.Or, eax, eax, eax);
			newBlocks[2].AppendInstruction(X86.Branch, ConditionCode.GreaterOrEqual, newBlocks[4].BasicBlock);
			newBlocks[2].AppendInstruction(X86.Jmp, newBlocks[3].BasicBlock);
			LinkBlocks(newBlocks[2], newBlocks[4], newBlocks[3]);

			newBlocks[3].AppendInstruction(X86.Mov, edx, op2L);
			newBlocks[3].AppendInstruction(X86.Neg, eax, eax);
			newBlocks[3].AppendInstruction(X86.Neg, edx, edx);
			newBlocks[3].AppendInstruction(X86.Sbb, eax, eax, Operand.CreateConstant((int)0));
			newBlocks[3].AppendInstruction(X86.Mov, op2H, eax);
			newBlocks[3].AppendInstruction(X86.Mov, op2L, edx);
			newBlocks[3].AppendInstruction(X86.Jmp, newBlocks[4].BasicBlock);
			LinkBlocks(newBlocks[3], newBlocks[4]);

			// L2:
			//
			//
			// Now do the divide.  First look to see if the divisor is less than 4194304K.
			// If so, then we can use a simple algorithm with word divides, otherwise
			// things get a little more complex.
			//
			// NOTE - eax currently contains the high order word of DVSR
			//
			//
			// or      eax,eax         ; check to see if divisor < 4194304K
			// jnz     short L3        ; nope, gotta do this the hard way
			// mov     ecx,LOWORD(DVSR) ; load divisor
			// mov     eax,HIWORD(DVND) ; load high word of dividend
			// xor     edx,edx
			// div     ecx             ; eax <- high order bits of quotient
			// mov     eax,LOWORD(DVND) ; edx:eax <- remainder:lo word of dividend
			// div     ecx             ; eax <- low order bits of quotient
			// mov     edx,ebx         ; edx:eax <- quotient
			// jmp     short L4        ; set sign, restore stack and return
			newBlocks[4].AppendInstruction(X86.Or, eax, eax, eax);
			newBlocks[4].AppendInstruction(X86.Branch, ConditionCode.NotEqual, newBlocks[6].BasicBlock);
			newBlocks[4].AppendInstruction(X86.Jmp, newBlocks[5].BasicBlock);
			LinkBlocks(newBlocks[4], newBlocks[6], newBlocks[5]);

			newBlocks[5].AppendInstruction(X86.Mov, v8, Operand.CreateConstant(0));
			newBlocks[5].AppendInstruction2(X86.Div, eax, edx, op1H, v8, op2L);
			newBlocks[5].AppendInstruction2(X86.Div, eax, edx, op1L, edx, op2L);
			newBlocks[5].AppendInstruction(X86.Mov, eax, edx);
			newBlocks[5].AppendInstruction(X86.Mov, edx, Operand.CreateConstant((uint)0));
			newBlocks[5].AppendInstruction(X86.Dec, edi, edi);
			newBlocks[5].AppendInstruction(X86.Branch, ConditionCode.NotSigned, newBlocks[14].BasicBlock);
			newBlocks[5].AppendInstruction(X86.Jmp, newBlocks[15].BasicBlock);
			LinkBlocks(newBlocks[5], newBlocks[14], newBlocks[15]);

			// Here we do it the hard way.  Remember, eax contains the high word of DVSR
			//
			// L3:
			//        mov     ebx,eax         ; ebx:ecx <- divisor
			//        mov     ecx,LOWORD(DVSR)
			//        mov     edx,HIWORD(DVND) ; edx:eax <- dividend
			//        mov     eax,LOWORD(DVND)
			newBlocks[6].AppendInstruction(X86.Mov, ebx, eax);
			newBlocks[6].AppendInstruction(X86.Mov, ecx, op2L);
			newBlocks[6].AppendInstruction(X86.Mov, edx, op1H);
			newBlocks[6].AppendInstruction(X86.Mov, eax, op1L);
			newBlocks[6].AppendInstruction(X86.Jmp, newBlocks[7].BasicBlock);
			LinkBlocks(newBlocks[6], newBlocks[7]);

			// L5:
			//
			//  shr     ebx,1           ; shift divisor right one bit
			//  rcr     ecx,1
			//  shr     edx,1           ; shift dividend right one bit
			//  rcr     eax,1
			//  or      ebx,ebx
			//  jnz     short L5        ; loop until divisor < 4194304K
			//  div     ecx             ; now divide, ignore remainder

			//
			// We may be off by one, so to check, we will multiply the quotient
			// by the divisor and check the result against the orignal dividend
			// Note that we must also check for overflow, which can occur if the
			// dividend is close to 2**64 and the quotient is off by 1.
			//

			//  mov     ecx,eax         ; save a copy of quotient in ECX
			//  mul     dword ptr HIWORD(DVSR)
			//  xchg    ecx,eax         ; save product, get quotient in EAX
			//  mul     dword ptr LOWORD(DVSR)
			//  add     edx,ecx         ; EDX:EAX = QUOT * DVSR
			//  jc      short L6        ; carry means Quotient is off by 1

			//
			// do long compare here between original dividend and the result of the
			// multiply in edx:eax.  If original is larger or equal, we are ok, otherwise
			// subtract the original divisor from the result.
			//

			//  cmp     edx,HIWORD(DVND) ; compare hi words of result and original
			//  ja      short L6        ; if result > original, do subtract
			//  jb      short L7        ; if result < original, we are ok
			//  cmp     eax,LOWORD(DVND) ; hi words are equal, compare lo words
			//  jbe     short L7        ; if less or equal we are ok, else subtract

			newBlocks[7].AppendInstruction(X86.Shr, ebx, ebx, Operand.CreateConstant(BuiltInSigType.Byte, 1));
			newBlocks[7].AppendInstruction(X86.Rcr, ecx, ecx, Operand.CreateConstant(BuiltInSigType.Byte, 1)); // RCR
			newBlocks[7].AppendInstruction(X86.Shr, edx, edx, Operand.CreateConstant(BuiltInSigType.Byte, 1));
			newBlocks[7].AppendInstruction(X86.Rcr, eax, eax, Operand.CreateConstant(BuiltInSigType.Byte, 1));
			newBlocks[7].AppendInstruction(X86.Or, ebx, ebx, ebx);
			newBlocks[7].AppendInstruction(X86.Branch, ConditionCode.NotEqual, newBlocks[7].BasicBlock);
			newBlocks[7].AppendInstruction(X86.Jmp, newBlocks[8].BasicBlock);
			LinkBlocks(newBlocks[7], newBlocks[7], newBlocks[8]);

			newBlocks[8].AppendInstruction2(X86.Div, eax, edx, eax, edx, ecx);
			newBlocks[8].AppendInstruction(X86.Mov, ecx, eax);

			//newBlocks[8].AppendInstruction(X86.Mul, eax, op2H);
			newBlocks[8].AppendInstruction2(X86.Mul, edx, eax, eax, op2H);
			newBlocks[8].AppendInstruction(X86.Xchg, ecx, eax);

			//newBlocks[8].AppendInstruction(X86.Mul, eax, op2L);
			newBlocks[8].AppendInstruction2(X86.Mul, edx, eax, eax, op2L);
			newBlocks[8].AppendInstruction(X86.Add, edx, edx, ecx);
			newBlocks[8].AppendInstruction(X86.Branch, ConditionCode.UnsignedLessThan, newBlocks[12].BasicBlock);
			newBlocks[8].AppendInstruction(X86.Jmp, newBlocks[9].BasicBlock);
			LinkBlocks(newBlocks[8], newBlocks[12], newBlocks[9]);

			newBlocks[9].AppendInstruction(X86.Cmp, null, edx, op1H);
			newBlocks[9].AppendInstruction(X86.Branch, ConditionCode.UnsignedGreaterThan, newBlocks[12].BasicBlock);
			newBlocks[9].AppendInstruction(X86.Jmp, newBlocks[10].BasicBlock);
			LinkBlocks(newBlocks[9], newBlocks[12], newBlocks[10]);

			newBlocks[10].AppendInstruction(X86.Branch, ConditionCode.UnsignedLessThan, newBlocks[13].BasicBlock);
			newBlocks[10].AppendInstruction(X86.Jmp, newBlocks[11].BasicBlock);
			LinkBlocks(newBlocks[10], newBlocks[13], newBlocks[11]);

			newBlocks[11].AppendInstruction(X86.Cmp, null, eax, op1L);
			newBlocks[11].AppendInstruction(X86.Branch, ConditionCode.UnsignedLessOrEqual, newBlocks[13].BasicBlock);
			newBlocks[11].AppendInstruction(X86.Jmp, newBlocks[12].BasicBlock);
			LinkBlocks(newBlocks[11], newBlocks[13], newBlocks[12]);

			// L6:
			newBlocks[12].AppendInstruction(X86.Sub, eax, eax, op2L);
			newBlocks[12].AppendInstruction(X86.Sbb, edx, edx, op2H);
			newBlocks[12].AppendInstruction(X86.Jmp, newBlocks[13].BasicBlock);
			LinkBlocks(newBlocks[13], newBlocks[13]);

			// L7:
			//
			// Calculate remainder by subtracting the result from the original dividend.
			// Since the result is already in a register, we will do the subtract in the
			// opposite direction and negate the result if necessary.
			//
			newBlocks[13].AppendInstruction(X86.Sub, eax, eax, op1L);
			newBlocks[13].AppendInstruction(X86.Sbb, edx, edx, op1H);
			newBlocks[13].AppendInstruction(X86.Dec, edi, edi);
			newBlocks[13].AppendInstruction(X86.Branch, ConditionCode.NotSigned, newBlocks[15].BasicBlock);
			newBlocks[13].AppendInstruction(X86.Jmp, newBlocks[14].BasicBlock);
			LinkBlocks(newBlocks[13], newBlocks[14], newBlocks[15]);

			// L4:
			//        neg     edx             ; otherwise, negate the result
			//        neg     eax
			//        sbb     edx,0
			newBlocks[14].AppendInstruction(X86.Neg, edx, edx);
			newBlocks[14].AppendInstruction(X86.Neg, eax, eax);
			newBlocks[14].AppendInstruction(X86.Sbb, edx, edx, Operand.CreateConstant((int)0));
			newBlocks[14].AppendInstruction(X86.Jmp, newBlocks[15].BasicBlock);
			LinkBlocks(newBlocks[14], newBlocks[15]);

			newBlocks[15].AppendInstruction(X86.Mov, op0L, eax);
			newBlocks[15].AppendInstruction(X86.Mov, op0H, edx);
			newBlocks[15].AppendInstruction(X86.Jmp, nextBlock.BasicBlock);
			LinkBlocks(newBlocks[15], nextBlock);
		}

		/// <summary>
		/// Expands the udiv instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ExpandUDiv(Context context)
		{
			Operand op0H, op1H, op2H, op0L, op1L, op2L;
			SplitLongOperand(context.Result, out op0L, out op0H);
			SplitLongOperand(context.Operand1, out op1L, out op1H);
			SplitLongOperand(context.Operand2, out op2L, out op2H);

			//Operand eax = AllocateVirtualRegister(BuiltInSigType.UInt32);
			//Operand edx = AllocateVirtualRegister(BuiltInSigType.UInt32);
			//Operand ebx = AllocateVirtualRegister(BuiltInSigType.UInt32);
			//Operand ecx = AllocateVirtualRegister(BuiltInSigType.UInt32);
			//Operand edi = AllocateVirtualRegister(BuiltInSigType.UInt32);
			//Operand esi = AllocateVirtualRegister(BuiltInSigType.UInt32);

			Operand eax = Operand.CreateCPURegister(BuiltInSigType.UInt32, GeneralPurposeRegister.EAX);
			Operand ebx = Operand.CreateCPURegister(BuiltInSigType.UInt32, GeneralPurposeRegister.EBX);
			Operand edx = Operand.CreateCPURegister(BuiltInSigType.UInt32, GeneralPurposeRegister.EDX);
			Operand ecx = Operand.CreateCPURegister(BuiltInSigType.UInt32, GeneralPurposeRegister.ECX);
			Operand edi = Operand.CreateCPURegister(BuiltInSigType.UInt32, GeneralPurposeRegister.EDI);
			Operand esi = Operand.CreateCPURegister(BuiltInSigType.UInt32, GeneralPurposeRegister.ESI);

			Context[] newBlocks = CreateNewBlocksWithContexts(12);
			Context nextBlock = Split(context, false);

			context.SetInstruction(X86.Jmp, newBlocks[0].BasicBlock);
			LinkBlocks(context, newBlocks[0]);

			newBlocks[0].AppendInstruction(X86.Mov, eax, op2H);
			newBlocks[0].AppendInstruction(X86.Or, eax, eax, eax);
			newBlocks[0].AppendInstruction(X86.Branch, ConditionCode.NotEqual, newBlocks[2].BasicBlock); // JNZ
			newBlocks[0].AppendInstruction(X86.Jmp, newBlocks[1].BasicBlock);
			LinkBlocks(newBlocks[0], newBlocks[2], newBlocks[1]);

			newBlocks[1].AppendInstruction(X86.Mov, ecx, op2L);
			newBlocks[1].AppendInstruction(X86.Mov, eax, op1H);
			newBlocks[1].AppendInstruction(X86.Mov, edx, Operand.CreateConstant((uint)0));
			newBlocks[1].AppendInstruction2(X86.Div, edx, eax, edx, eax, ecx);
			newBlocks[1].AppendInstruction(X86.Mov, ebx, eax);
			newBlocks[1].AppendInstruction(X86.Mov, eax, op1L);
			newBlocks[1].AppendInstruction2(X86.Div, edx, eax, edx, eax, ecx);
			newBlocks[1].AppendInstruction(X86.Mov, edx, ebx);
			newBlocks[1].AppendInstruction(X86.Jmp, newBlocks[10].BasicBlock);
			LinkBlocks(newBlocks[1], newBlocks[10]);

			// L1
			newBlocks[2].AppendInstruction(X86.Mov, ecx, eax);
			newBlocks[2].AppendInstruction(X86.Mov, ebx, op2L);
			newBlocks[2].AppendInstruction(X86.Mov, edx, op1H);
			newBlocks[2].AppendInstruction(X86.Mov, eax, op1L);
			newBlocks[2].AppendInstruction(X86.Jmp, newBlocks[3].BasicBlock);
			LinkBlocks(newBlocks[2], newBlocks[3]);

			// L3
			newBlocks[3].AppendInstruction(X86.Shr, ecx, ecx, Operand.CreateConstant(BuiltInSigType.Byte, 1));
			newBlocks[3].AppendInstruction(X86.Rcr, ebx, ebx, Operand.CreateConstant(BuiltInSigType.Byte, 1)); // RCR
			newBlocks[3].AppendInstruction(X86.Shr, edx, edx, Operand.CreateConstant(BuiltInSigType.Byte, 1));
			newBlocks[3].AppendInstruction(X86.Rcr, eax, eax, Operand.CreateConstant(BuiltInSigType.Byte, 1));
			newBlocks[3].AppendInstruction(X86.Or, ecx, ecx, ecx);
			newBlocks[3].AppendInstruction(X86.Branch, ConditionCode.NotEqual, newBlocks[3].BasicBlock); // JNZ
			newBlocks[3].AppendInstruction(X86.Jmp, newBlocks[4].BasicBlock);
			LinkBlocks(newBlocks[3], newBlocks[3], newBlocks[4]);

			newBlocks[4].AppendInstruction2(X86.Div, edx, eax, edx, eax, ebx);
			newBlocks[4].AppendInstruction(X86.Mov, esi, eax);
			newBlocks[4].AppendInstruction2(X86.Mul, edx, eax, eax, op2H);
			newBlocks[4].AppendInstruction(X86.Mov, ecx, eax);
			newBlocks[4].AppendInstruction(X86.Mov, eax, op2L);
			newBlocks[4].AppendInstruction2(X86.Mul, edx, eax, eax, esi);
			newBlocks[4].AppendInstruction(X86.Add, edx, edx, ecx);
			newBlocks[4].AppendInstruction(X86.Branch, ConditionCode.UnsignedLessThan, newBlocks[8].BasicBlock);
			newBlocks[4].AppendInstruction(X86.Jmp, newBlocks[5].BasicBlock);
			LinkBlocks(newBlocks[4], newBlocks[8], newBlocks[5]);

			newBlocks[5].AppendInstruction(X86.Cmp, null, edx, op1H);
			newBlocks[5].AppendInstruction(X86.Branch, ConditionCode.UnsignedGreaterThan, newBlocks[8].BasicBlock);
			newBlocks[5].AppendInstruction(X86.Jmp, newBlocks[6].BasicBlock);
			LinkBlocks(newBlocks[5], newBlocks[8], newBlocks[6]);

			newBlocks[6].AppendInstruction(X86.Branch, ConditionCode.UnsignedLessThan, newBlocks[9].BasicBlock);
			newBlocks[6].AppendInstruction(X86.Jmp, newBlocks[7].BasicBlock);
			LinkBlocks(newBlocks[6], newBlocks[9], newBlocks[7]);

			newBlocks[7].AppendInstruction(X86.Cmp, null, eax, op1L);
			newBlocks[7].AppendInstruction(X86.Branch, ConditionCode.UnsignedLessOrEqual, newBlocks[9].BasicBlock);
			newBlocks[7].AppendInstruction(X86.Jmp, newBlocks[8].BasicBlock);
			LinkBlocks(newBlocks[7], newBlocks[9], newBlocks[8]);

			// L4:
			newBlocks[8].AppendInstruction(X86.Dec, esi, esi);
			newBlocks[8].AppendInstruction(X86.Jmp, newBlocks[9].BasicBlock);
			LinkBlocks(newBlocks[8], newBlocks[9]);

			// L5
			newBlocks[9].AppendInstruction(X86.Mov, edx, Operand.CreateConstant((uint)0));
			newBlocks[9].AppendInstruction(X86.Mov, eax, esi);
			newBlocks[9].AppendInstruction(X86.Jmp, newBlocks[10].BasicBlock);
			LinkBlocks(newBlocks[9], newBlocks[10]);

			// L2
			newBlocks[10].AppendInstruction(X86.Mov, op0L, eax);
			newBlocks[10].AppendInstruction(X86.Mov, op0H, edx);
			newBlocks[10].AppendInstruction(X86.Jmp, nextBlock.BasicBlock);
			LinkBlocks(newBlocks[10], nextBlock);
		}

		/// <summary>
		/// Expands the urem instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ExpandURem(Context context)
		{
			Operand op0H, op1H, op2H, op0L, op1L, op2L;
			SplitLongOperand(context.Result, out op0L, out op0H);
			SplitLongOperand(context.Operand1, out op1L, out op1H);
			SplitLongOperand(context.Operand2, out op2L, out op2H);

			Operand eax = AllocateVirtualRegister(BuiltInSigType.UInt32);
			Operand edx = AllocateVirtualRegister(BuiltInSigType.UInt32);
			Operand ebx = AllocateVirtualRegister(BuiltInSigType.UInt32);
			Operand ecx = AllocateVirtualRegister(BuiltInSigType.UInt32);
			Operand edi = AllocateVirtualRegister(BuiltInSigType.UInt32);
			Operand esi = AllocateVirtualRegister(BuiltInSigType.UInt32);

			Context[] newBlocks = CreateNewBlocksWithContexts(11);
			Context nextBlock = Split(context, false);

			// Determine sign of the result (edi = 0 if result is positive, non-zero
			// otherwise) and make operands positive.
			//    xor     edi,edi         ; result sign assumed positive
			//mov     eax,HIWORD(DVND) ; hi word of a
			//or      eax,eax         ; test to see if signed
			//jge     short L1        ; skip rest if a is already positive
			//inc     edi             ; complement result sign flag bit
			//mov     edx,LOWORD(DVND) ; lo word of a
			//neg     eax             ; make a positive
			//neg     edx
			//sbb     eax,0
			//mov     HIWORD(DVND),eax ; save positive value
			//mov     LOWORD(DVND),edx
			context.SetInstruction(X86.Jmp, newBlocks[0].BasicBlock);
			LinkBlocks(context, newBlocks[0]);

			newBlocks[0].AppendInstruction(X86.Mov, eax, op2H);
			newBlocks[0].AppendInstruction(X86.Or, eax, eax, eax);
			newBlocks[0].AppendInstruction(X86.Branch, ConditionCode.NotEqual, newBlocks[2].BasicBlock);
			newBlocks[0].AppendInstruction(X86.Jmp, newBlocks[1].BasicBlock);
			LinkBlocks(newBlocks[0], newBlocks[2], newBlocks[1]);

			newBlocks[1].AppendInstruction(X86.Mov, ecx, op2L);
			newBlocks[1].AppendInstruction(X86.Mov, eax, op1H);
			newBlocks[1].AppendInstruction(X86.Mov, edx, Operand.CreateConstant((uint)0));
			newBlocks[1].AppendInstruction2(X86.Div, eax, edx, eax, edx, ecx);
			newBlocks[1].AppendInstruction(X86.Mov, eax, op1L);
			newBlocks[1].AppendInstruction2(X86.Div, eax, edx, eax, edx, ecx);
			newBlocks[1].AppendInstruction(X86.Mov, eax, edx);
			newBlocks[1].AppendInstruction(X86.Mov, edx, Operand.CreateConstant((uint)0));
			newBlocks[1].AppendInstruction(X86.Jmp, newBlocks[10].BasicBlock);
			LinkBlocks(newBlocks[1], newBlocks[10]);

			// L1:
			newBlocks[2].AppendInstruction(X86.Mov, ecx, eax);
			newBlocks[2].AppendInstruction(X86.Mov, ebx, op2L);
			newBlocks[2].AppendInstruction(X86.Mov, edx, op1H);
			newBlocks[2].AppendInstruction(X86.Mov, eax, op1L);
			newBlocks[2].AppendInstruction(X86.Jmp, newBlocks[3].BasicBlock);
			LinkBlocks(newBlocks[2], newBlocks[3]);

			// L3:
			newBlocks[3].AppendInstruction(X86.Shr, ecx, ecx, Operand.CreateConstant(BuiltInSigType.Byte, 1));
			newBlocks[3].AppendInstruction(X86.Rcr, ebx, ebx, Operand.CreateConstant(BuiltInSigType.Byte, 1)); // RCR
			newBlocks[3].AppendInstruction(X86.Shr, edx, edx, Operand.CreateConstant(BuiltInSigType.Byte, 1));
			newBlocks[3].AppendInstruction(X86.Rcr, eax, eax, Operand.CreateConstant(BuiltInSigType.Byte, 1));
			newBlocks[3].AppendInstruction(X86.Or, ecx, ecx, ecx);
			newBlocks[3].AppendInstruction(X86.Branch, ConditionCode.NotEqual, newBlocks[3].BasicBlock);
			newBlocks[3].AppendInstruction(X86.Jmp, newBlocks[4].BasicBlock);
			LinkBlocks(newBlocks[3], newBlocks[3], newBlocks[4]);

			newBlocks[4].AppendInstruction2(X86.Div, eax, edx, eax, edx, ebx);
			newBlocks[4].AppendInstruction(X86.Mov, ecx, eax);

			newBlocks[4].AppendInstruction2(X86.Mul, edx, eax, eax, op2H);
			newBlocks[4].AppendInstruction(X86.Xchg, ecx, eax);

			newBlocks[4].AppendInstruction2(X86.Mul, edx, eax, eax, op2L);
			newBlocks[4].AppendInstruction(X86.Add, edx, edx, ecx);
			newBlocks[4].AppendInstruction(X86.Branch, ConditionCode.UnsignedLessThan, newBlocks[8].BasicBlock);
			newBlocks[4].AppendInstruction(X86.Jmp, newBlocks[5].BasicBlock);
			LinkBlocks(newBlocks[4], newBlocks[8], newBlocks[5]);

			newBlocks[5].AppendInstruction(X86.Cmp, null, edx, op1H);
			newBlocks[5].AppendInstruction(X86.Branch, ConditionCode.UnsignedGreaterThan, newBlocks[8].BasicBlock);
			newBlocks[5].AppendInstruction(X86.Jmp, newBlocks[6].BasicBlock);
			LinkBlocks(newBlocks[5], newBlocks[8], newBlocks[6]);

			newBlocks[6].AppendInstruction(X86.Branch, ConditionCode.UnsignedLessThan, newBlocks[9].BasicBlock);
			newBlocks[6].AppendInstruction(X86.Jmp, newBlocks[7].BasicBlock);
			LinkBlocks(newBlocks[6], newBlocks[6], newBlocks[7]);

			newBlocks[7].AppendInstruction(X86.Cmp, null, eax, op1L);
			newBlocks[7].AppendInstruction(X86.Branch, ConditionCode.UnsignedLessOrEqual, newBlocks[9].BasicBlock);
			newBlocks[7].AppendInstruction(X86.Jmp, newBlocks[3].BasicBlock);
			LinkBlocks(newBlocks[7], newBlocks[9], newBlocks[3]);

			// L4:
			newBlocks[8].AppendInstruction(X86.Sub, eax, eax, op2L);
			newBlocks[8].AppendInstruction(X86.Sbb, edx, edx, op2H);
			newBlocks[8].AppendInstruction(X86.Jmp, newBlocks[9].BasicBlock);
			LinkBlocks(newBlocks[8], newBlocks[9]);

			// L5:
			newBlocks[9].AppendInstruction(X86.Sub, eax, eax, op1L);
			newBlocks[9].AppendInstruction(X86.Sbb, edx, edx, op1H);
			newBlocks[9].AppendInstruction(X86.Neg, edx, edx);
			newBlocks[9].AppendInstruction(X86.Neg, eax, eax);
			newBlocks[9].AppendInstruction(X86.Sbb, edx, edx, Operand.CreateConstant((uint)0));
			newBlocks[9].AppendInstruction(X86.Jmp, newBlocks[10].BasicBlock);
			LinkBlocks(newBlocks[9], newBlocks[10]);

			newBlocks[10].AppendInstruction(X86.Mov, op0L, eax);
			newBlocks[10].AppendInstruction(X86.Mov, op0H, edx);
			newBlocks[10].AppendInstruction(X86.Jmp, nextBlock.BasicBlock);
			LinkBlocks(newBlocks[10], nextBlock);
		}

		/// <summary>
		/// Expands the arithmetic shift right instruction for 64-bits.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ExpandArithmeticShiftRight(Context context)
		{
			Operand count = context.Operand2;

			Debug.Assert(!count.IsConstant);

			Operand op0H, op1H, op0L, op1L;
			SplitLongOperand(context.Result, out op0L, out op0H);
			SplitLongOperand(context.Operand1, out op1L, out op1H);

			Operand eax = AllocateVirtualRegister(BuiltInSigType.Int32);
			Operand edx = AllocateVirtualRegister(BuiltInSigType.Int32);
			Operand ecx = AllocateVirtualRegister(BuiltInSigType.Int32);

			Context[] newBlocks = CreateNewBlocksWithContexts(6);
			Context nextBlock = Split(context, false);

			// Handle shifts of 64 bits or more (if shifting 64 bits or more, the result
			// depends only on the high order bit of edx).
			context.SetInstruction(X86.Jmp, newBlocks[0].BasicBlock);
			LinkBlocks(context, newBlocks[0]);

			//newBlocks[0].AppendInstruction(X86.Push, null, ecx);
			newBlocks[0].AppendInstruction(X86.Mov, ecx, count);
			newBlocks[0].AppendInstruction(X86.Mov, edx, op1H);
			newBlocks[0].AppendInstruction(X86.Mov, eax, op1L);
			newBlocks[0].AppendInstruction(X86.Cmp, null, ecx, Operand.CreateConstant((int)64));
			newBlocks[0].AppendInstruction(X86.Branch, ConditionCode.UnsignedGreaterOrEqual, newBlocks[4].BasicBlock);
			newBlocks[0].AppendInstruction(X86.Jmp, newBlocks[1].BasicBlock);
			LinkBlocks(newBlocks[0], newBlocks[4], newBlocks[1]);

			newBlocks[1].AppendInstruction(X86.Cmp, null, ecx, Operand.CreateConstant(BuiltInSigType.Int32, 32));
			newBlocks[1].AppendInstruction(X86.Branch, ConditionCode.UnsignedGreaterOrEqual, newBlocks[3].BasicBlock);
			newBlocks[1].AppendInstruction(X86.Jmp, newBlocks[2].BasicBlock);
			LinkBlocks(newBlocks[1], newBlocks[3], newBlocks[2]);

			newBlocks[2].AppendInstruction(X86.Shrd, eax, eax, edx, ecx);
			newBlocks[2].AppendInstruction(X86.Sar, edx, edx, ecx);
			newBlocks[2].AppendInstruction(X86.Jmp, newBlocks[5].BasicBlock);
			LinkBlocks(newBlocks[2], newBlocks[5]);

			// Handle shifts of between 32 and 63 bits
			// MORE32:
			newBlocks[3].AppendInstruction(X86.Mov, eax, edx);
			newBlocks[3].AppendInstruction(X86.Sar, edx, edx, Operand.CreateConstant(BuiltInSigType.Int32, (int)0x1F));
			newBlocks[3].AppendInstruction(X86.And, ecx, ecx, Operand.CreateConstant((int)0x1F));
			newBlocks[3].AppendInstruction(X86.Sar, eax, eax, ecx);
			newBlocks[3].AppendInstruction(X86.Jmp, newBlocks[5].BasicBlock);
			LinkBlocks(newBlocks[3], nextBlock);

			// Return double precision 0 or -1, depending on the sign of edx
			// RETSIGN:
			newBlocks[4].AppendInstruction(X86.Sar, edx, edx, Operand.CreateConstant(BuiltInSigType.Int32, (int)0x1F));
			newBlocks[4].AppendInstruction(X86.Mov, eax, edx);
			newBlocks[4].AppendInstruction(X86.Jmp, newBlocks[5].BasicBlock);
			LinkBlocks(newBlocks[4], newBlocks[5]);

			// done:
			// ; remaining code from current basic block
			newBlocks[5].AppendInstruction(X86.Mov, op0H, edx);
			newBlocks[5].AppendInstruction(X86.Mov, op0L, eax);
			newBlocks[5].AppendInstruction(X86.Jmp, nextBlock.BasicBlock);
			LinkBlocks(newBlocks[5], nextBlock);
		}

		/// <summary>
		/// Expands the shift left instruction for 64-bits.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ExpandShiftLeft(Context context)
		{
			Operand count = context.Operand2;

			Debug.Assert(!count.IsConstant);

			Operand op0H, op1H, op0L, op1L;
			SplitLongOperand(context.Result, out op0L, out op0H);
			SplitLongOperand(context.Operand1, out op1L, out op1H);

			Operand eax = AllocateVirtualRegister(BuiltInSigType.Int32);
			Operand edx = AllocateVirtualRegister(BuiltInSigType.Int32);
			Operand ecx = AllocateVirtualRegister(BuiltInSigType.Int32);

			Context nextBlock = Split(context, false);
			Context[] newBlocks = CreateNewBlocksWithContexts(6);

			// Handle shifts of 64 bits or more (if shifting 64 bits or more, the result
			// depends only on the high order bit of edx).
			context.SetInstruction(X86.Jmp, newBlocks[0].BasicBlock);
			LinkBlocks(context, newBlocks[0]);

			newBlocks[0].AppendInstruction(X86.Mov, ecx, count);
			newBlocks[0].AppendInstruction(X86.Mov, edx, op1H);
			newBlocks[0].AppendInstruction(X86.Mov, eax, op1L);
			newBlocks[0].AppendInstruction(X86.Cmp, null, ecx, Operand.CreateConstant((int)64));
			newBlocks[0].AppendInstruction(X86.Branch, ConditionCode.UnsignedGreaterOrEqual, newBlocks[4].BasicBlock);
			newBlocks[0].AppendInstruction(X86.Jmp, newBlocks[1].BasicBlock);
			LinkBlocks(newBlocks[0], newBlocks[4], newBlocks[1]);

			newBlocks[1].AppendInstruction(X86.Cmp, null, ecx, Operand.CreateConstant((int)32));
			newBlocks[1].AppendInstruction(X86.Branch, ConditionCode.UnsignedGreaterOrEqual, newBlocks[3].BasicBlock);
			newBlocks[1].AppendInstruction(X86.Jmp, newBlocks[2].BasicBlock);
			LinkBlocks(newBlocks[1], newBlocks[3], newBlocks[2]);

			newBlocks[2].AppendInstruction(X86.Shld, edx, edx, eax, ecx);
			newBlocks[2].AppendInstruction(X86.Shl, eax, eax, ecx);
			newBlocks[2].AppendInstruction(X86.Jmp, newBlocks[5].BasicBlock);
			LinkBlocks(newBlocks[2], newBlocks[5]);

			// Handle shifts of between 32 and 63 bits
			// MORE32:
			newBlocks[3].AppendInstruction(X86.Mov, edx, eax);
			newBlocks[3].AppendInstruction(X86.Mov, eax, Operand.CreateConstant((uint)0));
			newBlocks[3].AppendInstruction(X86.And, ecx, ecx, Operand.CreateConstant(BuiltInSigType.Int32, 0x1F));
			newBlocks[3].AppendInstruction(X86.Shl, edx, edx, ecx);
			newBlocks[3].AppendInstruction(X86.Jmp, newBlocks[5].BasicBlock);
			LinkBlocks(newBlocks[3], newBlocks[5]);

			// Return double precision 0 or -1, depending on the sign of edx
			// RETZERO:
			newBlocks[4].AppendInstruction(X86.Mov, eax, Operand.CreateConstant((uint)0));
			newBlocks[4].AppendInstruction(X86.Mov, edx, Operand.CreateConstant((uint)0));
			newBlocks[4].AppendInstruction(X86.Jmp, newBlocks[5].BasicBlock);
			LinkBlocks(newBlocks[4], newBlocks[5]);

			// done:
			// ; remaining code from current basic block
			newBlocks[5].AppendInstruction(X86.Mov, op0H, edx);
			newBlocks[5].AppendInstruction(X86.Mov, op0L, eax);
			newBlocks[5].AppendInstruction(X86.Jmp, nextBlock.BasicBlock);
			LinkBlocks(newBlocks[5], nextBlock);
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

			Operand eax = AllocateVirtualRegister(BuiltInSigType.Int32);
			Operand edx = AllocateVirtualRegister(BuiltInSigType.Int32);
			Operand ecx = AllocateVirtualRegister(BuiltInSigType.Int32);

			Context nextBlock = Split(context, false);
			Context[] newBlocks = CreateNewBlocksWithContexts(4);

			// Handle shifts of 64 bits or more (if shifting 64 bits or more, the result
			// depends only on the high order bit of edx).
			context.SetInstruction(X86.Mov, ecx, count);
			context.AppendInstruction(X86.Cmp, null, ecx, Operand.CreateConstant((int)64));
			context.AppendInstruction(X86.Branch, ConditionCode.UnsignedGreaterOrEqual, newBlocks[3].BasicBlock);
			context.AppendInstruction(X86.Jmp, newBlocks[0].BasicBlock);
			LinkBlocks(context, newBlocks[3], newBlocks[0]);

			newBlocks[0].AppendInstruction(X86.Cmp, null, ecx, Operand.CreateConstant((int)32));
			newBlocks[0].AppendInstruction(X86.Branch, ConditionCode.UnsignedGreaterOrEqual, newBlocks[2].BasicBlock);
			newBlocks[0].AppendInstruction(X86.Jmp, newBlocks[1].BasicBlock);
			LinkBlocks(newBlocks[0], newBlocks[2], newBlocks[1]);

			newBlocks[1].AppendInstruction(X86.Mov, op0H, op1H);
			newBlocks[1].AppendInstruction(X86.Mov, op0L, op1L);
			newBlocks[1].AppendInstruction(X86.Shrd, op0L, op0L, op0H, ecx);
			newBlocks[1].AppendInstruction(X86.Shr, op0H, op0H, ecx);
			newBlocks[1].AppendInstruction(X86.Jmp, nextBlock.BasicBlock);
			LinkBlocks(newBlocks[1], nextBlock.BasicBlock);

			// Handle shifts of between 32 and 63 bits
			// MORE32:
			newBlocks[2].AppendInstruction(X86.Mov, op0L, op1H);
			newBlocks[2].AppendInstruction(X86.Mov, op0H, Operand.CreateConstant((int)0x0));
			newBlocks[2].AppendInstruction(X86.And, ecx, ecx, Operand.CreateConstant((int)0x1F));
			newBlocks[2].AppendInstruction(X86.Sar, op0L, op0L, ecx);
			newBlocks[2].AppendInstruction(X86.Jmp, nextBlock.BasicBlock);
			LinkBlocks(newBlocks[2], nextBlock.BasicBlock);

			// Return double precision 0 or -1, depending on the sign of edx
			// RETSIGN:
			newBlocks[3].AppendInstruction(X86.Mov, op0H, Operand.CreateConstant((int)0x0));
			newBlocks[3].AppendInstruction(X86.Mov, op0L, op0H);
			newBlocks[3].AppendInstruction(X86.Jmp, nextBlock.BasicBlock);
			LinkBlocks(newBlocks[3], nextBlock.BasicBlock);
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

			Operand eax = AllocateVirtualRegister(BuiltInSigType.UInt32);

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

			if (context.Result.StackType != StackTypeCode.Int64)
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

			if (context.Result.StackType == StackTypeCode.Int64)
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

			switch (op1.Type.Type)
			{
				case CilElementType.Boolean:
					{
						context.SetInstruction(X86.Movzx, op0L, op1L);
						context.AppendInstruction(X86.Mov, op0H, Operand.CreateConstant((uint)0));
						break;
					}
				case CilElementType.Char: goto case CilElementType.U2;
				case CilElementType.U1:
					{
						Operand v1 = AllocateVirtualRegister(BuiltInSigType.UInt32);
						Operand v2 = AllocateVirtualRegister(BuiltInSigType.UInt32);
						Operand v3 = AllocateVirtualRegister(BuiltInSigType.UInt32);

						context.SetInstruction(X86.Movzx, v1, op1L);
						context.AppendInstruction2(X86.Cdq, v3, v2, v1);
						context.AppendInstruction(X86.Mov, op0L, v2);
						context.AppendInstruction(X86.Mov, op0H, Operand.CreateConstant((uint)0));
						break;
					}
				case CilElementType.U2: goto case CilElementType.U1;
				case CilElementType.I4:
					{
						Operand v1 = AllocateVirtualRegister(BuiltInSigType.Int32);
						Operand v2 = AllocateVirtualRegister(BuiltInSigType.UInt32);

						context.SetInstruction(X86.Movzx, v1, op1L);
						context.AppendInstruction(X86.Mov, v2, Operand.CreateConstant((uint)0));
						context.AppendInstruction(X86.Mov, op0L, v1);
						context.AppendInstruction(X86.Mov, op0H, v2);
						break;
					}
				case CilElementType.U4:
					{
						Operand v1 = AllocateVirtualRegister(BuiltInSigType.Int32);
						Operand v2 = AllocateVirtualRegister(BuiltInSigType.UInt32);
						Operand v3 = AllocateVirtualRegister(BuiltInSigType.UInt32);

						context.SetInstruction(X86.Movzx, v1, op1L);
						context.AppendInstruction2(X86.Cdq, v3, v2, v1);
						context.AppendInstruction(X86.Mov, op0L, v2);
						context.AppendInstruction(X86.Mov, op0H, v3);
						break;
					}
				case CilElementType.U8:
					{
						context.SetInstruction(X86.Movzx, op0L, op1L);
						context.SetInstruction(X86.Movzx, op0H, op1H);
						break;
					}

				case CilElementType.R4:
					throw new NotSupportedException();

				case CilElementType.R8:
					throw new NotSupportedException();

				default:
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

			switch (op1.Type.Type)
			{
				case CilElementType.Boolean:
					{
						context.SetInstruction(X86.Movzx, op0L, op1);
						context.AppendInstruction(X86.Mov, op0H, Operand.CreateConstant((uint)0));
						break;
					}
				case CilElementType.I1:
					{
						Operand v1 = AllocateVirtualRegister(BuiltInSigType.Int32);
						Operand v2 = AllocateVirtualRegister(BuiltInSigType.UInt32);
						Operand v3 = AllocateVirtualRegister(BuiltInSigType.UInt32);

						context.SetInstruction(X86.Movsx, v1, op1);
						context.AppendInstruction2(X86.Cdq, v3, v2, v1);
						context.AppendInstruction(X86.Mov, op0L, v2);
						context.AppendInstruction(X86.Mov, op0H, v3);
						break;
					}
				case CilElementType.I2: goto case CilElementType.I1;

				case CilElementType.I4:
					{
						Operand v1 = AllocateVirtualRegister(BuiltInSigType.Int32);
						Operand v2 = AllocateVirtualRegister(BuiltInSigType.UInt32);
						Operand v3 = AllocateVirtualRegister(BuiltInSigType.UInt32);

						context.SetInstruction(X86.Mov, v1, op1);
						context.AppendInstruction2(X86.Cdq, v3, v2, v1);
						context.AppendInstruction(X86.Mov, op0L, v2);
						context.AppendInstruction(X86.Mov, op0H, v3);
						break;
					}
				case CilElementType.I8:
					{
						context.SetInstruction(X86.Mov, op0, op1);
						break;
					}
				case CilElementType.U1:
					{
						Operand v1 = AllocateVirtualRegister(BuiltInSigType.UInt32);
						Operand v2 = AllocateVirtualRegister(BuiltInSigType.UInt32);
						Operand v3 = AllocateVirtualRegister(BuiltInSigType.UInt32);

						context.SetInstruction(X86.Movzx, v1, op1);
						context.AppendInstruction2(X86.Cdq, v3, v2, v1);
						context.AppendInstruction(X86.Mov, op0L, v2);
						context.AppendInstruction(X86.Mov, op0H, Operand.CreateConstant((uint)0));
						break;
					}
				case CilElementType.U2: goto case CilElementType.U1;

				case CilElementType.U4:
					throw new NotSupportedException();

				case CilElementType.U8:
					throw new NotSupportedException();

				case CilElementType.R4:
					throw new NotSupportedException();

				case CilElementType.R8:
					throw new NotSupportedException();

				default:
					throw new NotSupportedException();
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

			Operand v1 = AllocateVirtualRegister(BuiltInSigType.UInt32);
			Operand v2 = AllocateVirtualRegister(BuiltInSigType.UInt32);
			Operand v3 = AllocateVirtualRegister(BuiltInSigType.UInt32);

			if (offset.IsConstant && offset.ValueAsLongInteger == 0)
			{
				context.SetInstruction(X86.Mov, v1, address);
			}
			else
			{
				context.SetInstruction(X86.Add, v1, address, offset);
			}
			context.AppendInstruction(X86.Mov, v2, Operand.CreateMemoryAddress(BuiltInSigType.UInt32, v1, 0));
			context.AppendInstruction(X86.Mov, op0L, v2);
			context.AppendInstruction(X86.Mov, v3, Operand.CreateMemoryAddress(BuiltInSigType.UInt32, v1, 4));
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

			Operand v1 = AllocateVirtualRegister(BuiltInSigType.UInt32);
			Operand v2 = AllocateVirtualRegister(BuiltInSigType.UInt32);
			Operand v3 = AllocateVirtualRegister(BuiltInSigType.UInt32);

			// Fortunately in 32-bit mode, we can't have 64-bit offsets, so this plain add should suffice.
			if (offset.IsConstant && offset.ValueAsLongInteger == 0)
			{
				context.SetInstruction(X86.Mov, v1, address);
			}
			else
			{
				context.SetInstruction(X86.Add, v1, address, offset);
			}

			context.AppendInstruction(X86.Mov, v1, op0L);
			context.AppendInstruction(X86.Mov, Operand.CreateMemoryAddress(BuiltInSigType.UInt32, v2, 0), v1);
			context.AppendInstruction(X86.Mov, v3, op0H);
			context.AppendInstruction(X86.Mov, Operand.CreateMemoryAddress(BuiltInSigType.UInt32, v3, 4), v1);
		}

		/// <summary>
		/// Expands the binary branch instruction for 64-bits.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ExpandBinaryBranch(Context context)
		{
			Debug.Assert(context.BranchTargets.Length == 1);

			BasicBlock target = basicBlocks.GetByLabel(context.BranchTargets[0]);

			Operand op1L, op1H, op2L, op2H;
			SplitLongOperand(context.Operand1, out op1L, out op1H);
			SplitLongOperand(context.Operand2, out op2L, out op2H);

			Context[] newBlocks = CreateNewBlocksWithContexts(2);
			ConditionCode conditionCode = context.ConditionCode;
			Context nextBlock = Split(context, false);

			// Compare high dwords
			context.SetInstruction(X86.Cmp, null, op1H, op2H);
			context.AppendInstruction(X86.Branch, ConditionCode.Equal, newBlocks[1].BasicBlock);
			context.AppendInstruction(X86.Jmp, newBlocks[0].BasicBlock);
			LinkBlocks(context, newBlocks[0], newBlocks[1]);

			// Branch if check already gave results
			newBlocks[0].AppendInstruction(X86.Branch, conditionCode, target);
			newBlocks[0].AppendInstruction(X86.Jmp, nextBlock.BasicBlock);
			LinkBlocks(newBlocks[0], target, nextBlock.BasicBlock);

			// Compare low dwords
			newBlocks[1].AppendInstruction(X86.Cmp, null, op1L, op2L);
			newBlocks[1].AppendInstruction(X86.Branch, GetUnsignedConditionCode(conditionCode), target);
			newBlocks[1].AppendInstruction(X86.Jmp, nextBlock.BasicBlock);
			LinkBlocks(newBlocks[1], target, nextBlock.BasicBlock);
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

			Context[] newBlocks = CreateNewBlocksWithContexts(4);
			ConditionCode conditionCode = context.ConditionCode;
			Context nextBlock = Split(context, false);

			// Compare high dwords
			context.SetInstruction(X86.Cmp, null, op1H, op2H);
			context.AppendInstruction(X86.Branch, ConditionCode.Equal, newBlocks[1].BasicBlock);
			context.AppendInstruction(X86.Jmp, newBlocks[0].BasicBlock);
			LinkBlocks(context, newBlocks[0], newBlocks[1]);

			// Branch if check already gave results
			newBlocks[0].AppendInstruction(X86.Branch, conditionCode, newBlocks[2].BasicBlock);
			newBlocks[0].AppendInstruction(X86.Jmp, newBlocks[3].BasicBlock);
			LinkBlocks(newBlocks[0], newBlocks[2], newBlocks[3]);

			// Compare low dwords
			newBlocks[1].AppendInstruction(X86.Cmp, null, op1L, op2L);
			newBlocks[1].AppendInstruction(X86.Branch, GetUnsignedConditionCode(conditionCode), newBlocks[2].BasicBlock);
			newBlocks[1].AppendInstruction(X86.Jmp, newBlocks[3].BasicBlock);
			LinkBlocks(newBlocks[1], newBlocks[2], newBlocks[3]);

			// Success
			newBlocks[2].AppendInstruction(X86.Mov, op0, Operand.CreateConstant((int)1));
			newBlocks[2].AppendInstruction(X86.Jmp, nextBlock.BasicBlock);
			LinkBlocks(newBlocks[2], nextBlock);

			// Failed
			newBlocks[3].AppendInstruction(X86.Mov, op0, Operand.CreateConstant((int)0));
			newBlocks[3].AppendInstruction(X86.Jmp, nextBlock.BasicBlock);
			LinkBlocks(newBlocks[3], nextBlock);
		}

		/// <summary>
		/// Determines whether the specified op is int64.
		/// </summary>
		/// <param name="op">The op.</param>
		/// <returns>
		/// 	<c>true</c> if the specified op is int64; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsInt64(Operand op)
		{
			return op.StackType == StackTypeCode.Int64;
		}

		/// <summary>
		/// Ares the any64 bit.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns></returns>
		public static bool AreAny64Bit(Context context)
		{
			if (IsInt64(context.Result))
				return true;

			foreach (var operand in context.Operands)
				if (IsInt64(operand))
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
			if (IsInt64(context.Operand1))
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
			if (IsInt64(context.Operand1) || IsInt64(context.Operand2))
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
			if (IsInt64(context.Operand1))
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
			if (IsInt64(context.Operand1) || IsInt64(context.Result))
			{
				ExpandLoad(context);
			}
		}

		/// <summary>
		/// Visitation function for Load Zero Extended.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.LoadZeroExtended(Context context)
		{
			// TODO
		}

		/// <summary>
		/// Visitation function for Load Sign Extended.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.LoadSignExtended(Context context)
		{
			// TODO
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
			if (IsInt64(context.Operand3))
			{
				ExpandStore(context);
			}
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
		/// Visitation function for RemSInstruction.
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
		/// Visitation function for AddSInstruction.
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
		/// Visitation function for AddUInstruction.
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

			if (context.Result != null && IsInt64(context.Result))
			{
				SplitLongOperand(context.Result, out op0L, out op0H);
			}

			foreach (var operand in context.Operands)
			{
				if (IsInt64(operand))
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

			if (context.Result != null && IsInt64(context.Result))
			{
				SplitLongOperand(context.Result, out op0L, out op0H);
			}

			foreach (var operand in context.Operands)
			{
				if (IsInt64(operand))
				{
					SplitLongOperand(operand, out op0L, out op0H);
				}
			}
		}

		#endregion IIRVisitor

		#region IIRVisitor - Unused

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
		/// Visitation function for AddFInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.AddFloat(Context context)
		{
		}

		/// <summary>
		/// Visitation function for DivFInstruction.
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

		/// <summary>
		/// Visitation function for ThrowInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.Throw(Context context)
		{
		}

		/// <summary>
		/// Visitation function for ExceptionPrologueInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.ExceptionPrologue(Context context)
		{
		}

		#endregion IIRVisitor - Unused
	}
}