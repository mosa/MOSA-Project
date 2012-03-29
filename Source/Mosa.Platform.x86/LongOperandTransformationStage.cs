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

using System;
using System.Diagnostics;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Operands;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;
using CIL = Mosa.Compiler.Framework.CIL;
using IR = Mosa.Compiler.Framework.IR;

namespace Mosa.Platform.x86
{
	/// <summary>
	/// Transforms 64-bit arithmetic to 32-bit operations.
	/// </summary>
	/// <remarks>
	/// This stage translates all 64-bit operations to appropriate 32-bit operations on
	/// architectures without appropriate 64-bit integral operations.
	/// </remarks>
	public sealed class LongOperandTransformationStage : BaseTransformationStage, IR.IIRVisitor, IPlatformStage, IPipelineStage
	{

		#region Utility Methods

		/// <summary>
		/// Splits the long operand into its high and low parts.
		/// </summary>
		/// <param name="operand">The operand to split.</param>
		/// <param name="operandLow">The low operand.</param>
		/// <param name="operandHigh">The high operand.</param>
		/// <exception cref="T:System.ArgumentException"><paramref name="operand"/> is not a ConstantOperand and not a MemoryOperand.</exception>
		public static void SplitLongOperand(Operand operand, out Operand operandLow, out Operand operandHigh)
		{
			if (operand.Type.Type != CilElementType.I8 && operand.Type.Type != CilElementType.U8)
			{
				operandLow = operand;
				operandHigh = new ConstantOperand(BuiltInSigType.Int32, (int)0);
				return;
			}

			Debug.Assert(operand is MemoryOperand || operand is ConstantOperand, @"Long operand not memory or constant.");

			if (operand is ConstantOperand)
				SplitFromConstantOperand(operand, out operandLow, out operandHigh);
			else
				SplitFromNonConstantOperand(operand, out operandLow, out operandHigh);
		}

		private static void SplitFromConstantOperand(Operand operand, out Operand operandLow, out Operand operandHigh)
		{
			SigType HighType = (operand.Type.Type == CilElementType.I8) ? BuiltInSigType.Int32 : BuiltInSigType.UInt32;

			ConstantOperand constantOperand = operand as ConstantOperand;

			if (HighType.Type == CilElementType.I4)
			{
				long value = (long)constantOperand.Value;
				operandLow = new ConstantOperand(BuiltInSigType.UInt32, (uint)(value & 0xFFFFFFFF));
				operandHigh = new ConstantOperand(HighType, (int)(value >> 32));
			}
			else
			{
				ulong value = (ulong)constantOperand.Value;
				operandLow = new ConstantOperand(BuiltInSigType.UInt32, (uint)(value & 0xFFFFFFFF));
				operandHigh = new ConstantOperand(HighType, (uint)(value >> 32));
			}
		}

		private static void SplitFromNonConstantOperand(Operand operand, out Operand operandLow, out Operand operandHigh)
		{
			SigType HighType = (operand.Type.Type == CilElementType.I8) ? BuiltInSigType.Int32 : BuiltInSigType.UInt32;

			// No, could be a member or a plain memory operand
			MemberOperand memberOperand = operand as MemberOperand;
			if (memberOperand != null)
			{
				// We need to keep the member reference, otherwise the linker can't fixup
				// the member address.
				operandLow = new MemberOperand(memberOperand.Member, BuiltInSigType.UInt32, memberOperand.Offset);
				operandHigh = new MemberOperand(memberOperand.Member, HighType, new IntPtr(memberOperand.Offset.ToInt64() + 4));
			}
			else
			{
				// Plain memory, we can handle it here
				MemoryOperand memoryOperand = (MemoryOperand)operand;
				operandLow = new MemoryOperand(BuiltInSigType.UInt32, memoryOperand.Base, memoryOperand.Offset);
				operandHigh = new MemoryOperand(HighType, memoryOperand.Base, new IntPtr(memoryOperand.Offset.ToInt64() + 4));
			}
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

			// This only works for memory operands (can't store I8/U8 in a register.)
			// This fails for constant operands right now, which need to be extracted into memory
			// with a literal/literal operand first - TODO
			RegisterOperand eaxH = new RegisterOperand(BuiltInSigType.Int32, GeneralPurposeRegister.EAX);
			RegisterOperand eaxL = new RegisterOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.EAX);

			Operand op1H, op1L, op2H, op2L, resH, resL;
			SplitLongOperand(context.Operand1, out op1L, out op1H);
			SplitLongOperand(context.Operand2, out op2L, out op2H);
			SplitLongOperand(context.Result, out resL, out resH);

			context.SetInstruction(Instruction.MovInstruction, eaxL, op1L);
			context.AppendInstruction(Instruction.AddInstruction, eaxL, op2L);
			context.AppendInstruction(Instruction.MovInstruction, resL, eaxL);
			context.AppendInstruction(Instruction.MovInstruction, eaxH, op1H);
			context.AppendInstruction(Instruction.AdcInstruction, eaxH, op2H);
			context.AppendInstruction(Instruction.MovInstruction, resH, eaxH);
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

			// This only works for memory operands (can't store I8/U8 in a register.)
			// This fails for constant operands right now, which need to be extracted into memory
			// with a literal/literal operand first - TODO
			RegisterOperand eaxH = new RegisterOperand(BuiltInSigType.Int32, GeneralPurposeRegister.EAX);
			RegisterOperand eaxL = new RegisterOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.EAX);

			Operand op1L, op1H, op2L, op2H, resL, resH;
			SplitLongOperand(context.Operand1, out op1L, out op1H);
			SplitLongOperand(context.Operand2, out op2L, out op2H);
			SplitLongOperand(context.Result, out resL, out resH);

			context.SetInstruction(Instruction.MovInstruction, eaxL, op1L);
			context.AppendInstruction(Instruction.SubInstruction, eaxL, op2L);
			context.AppendInstruction(Instruction.MovInstruction, resL, eaxL);
			context.AppendInstruction(Instruction.MovInstruction, eaxH, op1H);
			context.AppendInstruction(Instruction.SbbInstruction, eaxH, op2H);
			context.AppendInstruction(Instruction.MovInstruction, resH, eaxH);
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
			Debug.Assert(op0 != null && op1 != null && op2 != null, @"Operands to 64 bit multiplication are not MemoryOperands.");

			Operand op0H, op1H, op2H, op0L, op1L, op2L;
			SplitLongOperand(context.Result, out op0L, out op0H);
			SplitLongOperand(context.Operand1, out op1L, out op1H);
			SplitLongOperand(context.Operand2, out op2L, out op2H);

			RegisterOperand eax = new RegisterOperand(BuiltInSigType.Int32, GeneralPurposeRegister.EAX);
			RegisterOperand ebx = new RegisterOperand(BuiltInSigType.Int32, GeneralPurposeRegister.EBX);
			RegisterOperand ecx = new RegisterOperand(BuiltInSigType.Int32, GeneralPurposeRegister.ECX);
			RegisterOperand edx = new RegisterOperand(BuiltInSigType.Int32, GeneralPurposeRegister.EDX);

			Context nextBlock = SplitContext(context, false);
			Context[] newBlocks = CreateEmptyBlockContexts(context.Label, 4);

			context.SetInstruction(Instruction.JmpInstruction, newBlocks[0].BasicBlock);
			LinkBlocks(context, newBlocks[0]);

			newBlocks[0].SetInstruction(Instruction.MovInstruction, eax, op1H);
			newBlocks[0].AppendInstruction(Instruction.MovInstruction, ecx, op2H);
			newBlocks[0].AppendInstruction(Instruction.OrInstruction, ecx, eax);
			newBlocks[0].AppendInstruction(Instruction.MovInstruction, ecx, op2L);
			newBlocks[0].AppendInstruction(Instruction.BranchInstruction, IR.ConditionCode.NotEqual, newBlocks[2].BasicBlock);
			newBlocks[0].AppendInstruction(Instruction.JmpInstruction, newBlocks[1].BasicBlock);
			LinkBlocks(newBlocks[0], newBlocks[1], newBlocks[2]);

			newBlocks[1].AppendInstruction(Instruction.MovInstruction, eax, op1L);
			newBlocks[1].AppendInstruction(Instruction.MulInstruction, null, ecx);
			newBlocks[1].AppendInstruction(Instruction.JmpInstruction, newBlocks[3].BasicBlock);
			LinkBlocks(newBlocks[1], newBlocks[3]);

			newBlocks[2].AppendInstruction(Instruction.PushInstruction, null, ebx);
			newBlocks[2].AppendInstruction(Instruction.MulInstruction, null, ecx);
			newBlocks[2].AppendInstruction(Instruction.MovInstruction, ebx, eax);
			newBlocks[2].AppendInstruction(Instruction.MovInstruction, eax, op1L);
			newBlocks[2].AppendInstruction(Instruction.MulInstruction, null, op2H);
			newBlocks[2].AppendInstruction(Instruction.AddInstruction, ebx, eax);
			newBlocks[2].AppendInstruction(Instruction.MovInstruction, eax, op1L);
			newBlocks[2].AppendInstruction(Instruction.MulInstruction, null, ecx);
			newBlocks[2].AppendInstruction(Instruction.AddInstruction, edx, ebx);
			newBlocks[2].AppendInstruction(Instruction.PopInstruction, ebx);
			newBlocks[2].AppendInstruction(Instruction.JmpInstruction, newBlocks[3].BasicBlock);
			LinkBlocks(newBlocks[2], newBlocks[3]);

			newBlocks[3].AppendInstruction(Instruction.MovInstruction, op0L, eax);
			newBlocks[3].AppendInstruction(Instruction.MovInstruction, op0H, edx);
			newBlocks[3].AppendInstruction(Instruction.JmpInstruction, nextBlock.BasicBlock);
			LinkBlocks(newBlocks[2], nextBlock);
		}

		/// <summary>
		/// Expands the div.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ExpandDiv(Context context)
		{
			Operand op0H, op1H, op2H, op0L, op1L, op2L;
			//Operand op1 = EmitConstant(context.Operand1);
			//Operand op2 = EmitConstant(context.Operand2);
			SplitLongOperand(context.Result, out op0L, out op0H);
			SplitLongOperand(context.Operand1, out op1L, out op1H);
			SplitLongOperand(context.Operand2, out op2L, out op2H);

			Context[] newBlocks = CreateEmptyBlockContexts(context.Label, 17);
			Context nextBlock = SplitContext(context, false);

			RegisterOperand eax = new RegisterOperand(BuiltInSigType.Int32, GeneralPurposeRegister.EAX);
			RegisterOperand ebx = new RegisterOperand(BuiltInSigType.Int32, GeneralPurposeRegister.EBX);
			RegisterOperand edx = new RegisterOperand(BuiltInSigType.Int32, GeneralPurposeRegister.EDX);
			RegisterOperand ecx = new RegisterOperand(BuiltInSigType.Int32, GeneralPurposeRegister.ECX);
			RegisterOperand edi = new RegisterOperand(BuiltInSigType.Int32, GeneralPurposeRegister.EDI);
			RegisterOperand esi = new RegisterOperand(BuiltInSigType.Int32, GeneralPurposeRegister.ESI);

			RegisterOperand ueax = new RegisterOperand(BuiltInSigType.Int32, GeneralPurposeRegister.EAX);
			RegisterOperand uedx = new RegisterOperand(BuiltInSigType.Int32, GeneralPurposeRegister.EDX);
			RegisterOperand uecx = new RegisterOperand(BuiltInSigType.Int32, GeneralPurposeRegister.ECX);

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
			context.SetInstruction(Instruction.JmpInstruction, newBlocks[0].BasicBlock);
			LinkBlocks(context, newBlocks[0]);
			newBlocks[0].SetInstruction(Instruction.PushInstruction, null, edi);
			newBlocks[0].AppendInstruction(Instruction.PushInstruction, null, esi);
			newBlocks[0].AppendInstruction(Instruction.PushInstruction, null, ebx);
			newBlocks[0].AppendInstruction(Instruction.XorInstruction, edi, edi);
			newBlocks[0].AppendInstruction(Instruction.MovInstruction, eax, op1H);
			newBlocks[0].AppendInstruction(Instruction.OrInstruction, eax, eax);
			newBlocks[0].AppendInstruction(Instruction.BranchInstruction, IR.ConditionCode.GreaterOrEqual, newBlocks[2].BasicBlock);
			newBlocks[0].AppendInstruction(Instruction.JmpInstruction, newBlocks[1].BasicBlock);
			LinkBlocks(newBlocks[0], newBlocks[1], newBlocks[2]);

			newBlocks[1].SetInstruction(Instruction.IncInstruction, edi);
			newBlocks[1].AppendInstruction(Instruction.MovInstruction, uedx, op1L);
			newBlocks[1].AppendInstruction(Instruction.NegInstruction, eax);
			newBlocks[1].AppendInstruction(Instruction.NegInstruction, edx);
			newBlocks[1].AppendInstruction(Instruction.SbbInstruction, eax, new ConstantOperand(BuiltInSigType.Int32, 0));
			newBlocks[1].AppendInstruction(Instruction.MovInstruction, op1H, eax);
			newBlocks[1].AppendInstruction(Instruction.MovInstruction, op1L, uedx);
			newBlocks[1].AppendInstruction(Instruction.JmpInstruction, newBlocks[2].BasicBlock);
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
			newBlocks[2].SetInstruction(Instruction.MovInstruction, eax, op2H);
			newBlocks[2].AppendInstruction(Instruction.OrInstruction, eax, eax);
			newBlocks[2].AppendInstruction(Instruction.BranchInstruction, IR.ConditionCode.GreaterOrEqual, newBlocks[4].BasicBlock);
			newBlocks[2].AppendInstruction(Instruction.JmpInstruction, newBlocks[3].BasicBlock);
			LinkBlocks(newBlocks[2], newBlocks[3], newBlocks[4]);

			newBlocks[3].SetInstruction(Instruction.IncInstruction, edi);
			newBlocks[3].AppendInstruction(Instruction.MovInstruction, uedx, op2L);
			newBlocks[3].AppendInstruction(Instruction.NegInstruction, eax);
			newBlocks[3].AppendInstruction(Instruction.NegInstruction, edx);
			newBlocks[3].AppendInstruction(Instruction.SbbInstruction, eax, new ConstantOperand(BuiltInSigType.Int32, 0));
			newBlocks[3].AppendInstruction(Instruction.MovInstruction, op2H, eax);
			newBlocks[3].AppendInstruction(Instruction.MovInstruction, op2L, uedx);
			newBlocks[3].AppendInstruction(Instruction.JmpInstruction, newBlocks[4].BasicBlock);
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
			// mov     ecx,LOWORD(DVSR) ; load divisor
			// mov     eax,HIWORD(DVND) ; load high word of dividend
			// xor     edx,edx
			// div     ecx             ; eax <- high order bits of quotient
			// mov     ebx,eax         ; save high bits of quotient
			// mov     eax,LOWORD(DVND) ; edx:eax <- remainder:lo word of dividend
			// div     ecx             ; eax <- low order bits of quotient
			// mov     edx,ebx         ; edx:eax <- quotient
			// jmp     short L4        ; set sign, restore stack and return
			newBlocks[4].SetInstruction(Instruction.OrInstruction, eax, eax);
			newBlocks[4].AppendInstruction(Instruction.BranchInstruction, IR.ConditionCode.NotEqual, newBlocks[6].BasicBlock);
			newBlocks[4].AppendInstruction(Instruction.JmpInstruction, newBlocks[5].BasicBlock);
			LinkBlocks(newBlocks[4], newBlocks[5], newBlocks[6]);

			newBlocks[5].SetInstruction(Instruction.MovInstruction, uecx, op2L);
			newBlocks[5].AppendInstruction(Instruction.MovInstruction, eax, op1H);
			newBlocks[5].AppendInstruction(Instruction.XorInstruction, edx, edx);
			newBlocks[5].AppendInstruction(Instruction.DirectDivisionInstruction, eax, ecx);
			newBlocks[5].AppendInstruction(Instruction.MovInstruction, ebx, eax);
			newBlocks[5].AppendInstruction(Instruction.MovInstruction, ueax, op1L);
			newBlocks[5].AppendInstruction(Instruction.DirectDivisionInstruction, eax, ecx);
			newBlocks[5].AppendInstruction(Instruction.MovInstruction, edx, ebx);
			newBlocks[5].AppendInstruction(Instruction.JmpInstruction, newBlocks[14].BasicBlock);
			LinkBlocks(newBlocks[5], newBlocks[14]);

			// Here we do it the hard way.  Remember, eax contains the high word of DVSR
			//
			// L3:
			//        mov     ebx,eax         ; ebx:ecx <- divisor
			//        mov     ecx,LOWORD(DVSR)
			//        mov     edx,HIWORD(DVND) ; edx:eax <- dividend
			//        mov     eax,LOWORD(DVND)
			newBlocks[6].SetInstruction(Instruction.MovInstruction, ebx, eax);
			newBlocks[6].AppendInstruction(Instruction.MovInstruction, uecx, op2L);
			newBlocks[6].AppendInstruction(Instruction.MovInstruction, edx, op1H);
			newBlocks[6].AppendInstruction(Instruction.MovInstruction, ueax, op1L);
			newBlocks[6].AppendInstruction(Instruction.JmpInstruction, newBlocks[7].BasicBlock);
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
			//
			//

			// ;
			// ; We may be off by one, so to check, we will multiply the quotient
			// ; by the divisor and check the result against the original dividend
			// ; Note that we must also check for overflow, which can occur if the
			// ; dividend is close to 2**64 and the quotient is off by 1.
			// ;

			// mul     dword ptr HIWORD(DVSR) ; QUOT * HIWORD(DVSR)
			// mov     ecx,eax
			// mov     eax,LOWORD(DVSR)
			// mul     esi             ; QUOT * LOWORD(DVSR)
			// add     edx,ecx         ; EDX:EAX = QUOT * DVSR
			// jc      short L6        ; carry means Quotient is off by 1
			newBlocks[7].SetInstruction(Instruction.ShrInstruction, ebx, new ConstantOperand(BuiltInSigType.Byte, 1));
			newBlocks[7].AppendInstruction(Instruction.RcrInstruction, ecx, new ConstantOperand(BuiltInSigType.Byte, 1)); // RCR
			newBlocks[7].AppendInstruction(Instruction.ShrInstruction, edx, new ConstantOperand(BuiltInSigType.Byte, 1));
			newBlocks[7].AppendInstruction(Instruction.RcrInstruction, eax, new ConstantOperand(BuiltInSigType.Byte, 1));
			newBlocks[7].AppendInstruction(Instruction.OrInstruction, ebx, ebx);
			newBlocks[7].AppendInstruction(Instruction.BranchInstruction, IR.ConditionCode.NotEqual, newBlocks[7].BasicBlock);
			newBlocks[7].AppendInstruction(Instruction.JmpInstruction, newBlocks[8].BasicBlock);
			LinkBlocks(newBlocks[7], newBlocks[7], newBlocks[8]);

			newBlocks[8].SetInstruction(Instruction.DirectDivisionInstruction, eax, ecx);
			newBlocks[8].AppendInstruction(Instruction.MovInstruction, esi, eax);
			newBlocks[8].AppendInstruction(Instruction.MulInstruction, null, op2H);
			newBlocks[8].AppendInstruction(Instruction.MovInstruction, ecx, eax);
			newBlocks[8].AppendInstruction(Instruction.MovInstruction, ueax, op2L);
			newBlocks[8].AppendInstruction(Instruction.MulInstruction, null, esi);
			newBlocks[8].AppendInstruction(Instruction.AddInstruction, edx, ecx);
			newBlocks[8].AppendInstruction(Instruction.BranchInstruction, IR.ConditionCode.UnsignedLessThan, newBlocks[12].BasicBlock);
			newBlocks[8].AppendInstruction(Instruction.JmpInstruction, newBlocks[9].BasicBlock);
			LinkBlocks(newBlocks[8], newBlocks[9], newBlocks[12]);

			newBlocks[9].SetInstruction(Instruction.DirectCompareInstruction, edx, op1H);
			newBlocks[9].AppendInstruction(Instruction.BranchInstruction, IR.ConditionCode.UnsignedGreaterThan, newBlocks[12].BasicBlock);
			newBlocks[9].AppendInstruction(Instruction.JmpInstruction, newBlocks[10].BasicBlock);
			LinkBlocks(newBlocks[9], newBlocks[10], newBlocks[12]);

			newBlocks[10].SetInstruction(Instruction.BranchInstruction, IR.ConditionCode.UnsignedLessThan, newBlocks[13].BasicBlock);
			newBlocks[10].AppendInstruction(Instruction.JmpInstruction, newBlocks[11].BasicBlock);
			LinkBlocks(newBlocks[10], newBlocks[11], newBlocks[13]);

			newBlocks[11].SetInstruction(Instruction.DirectCompareInstruction, ueax, op1L);
			newBlocks[11].AppendInstruction(Instruction.BranchInstruction, IR.ConditionCode.UnsignedLessOrEqual, newBlocks[13].BasicBlock);
			newBlocks[11].AppendInstruction(Instruction.JmpInstruction, newBlocks[12].BasicBlock);
			LinkBlocks(newBlocks[11], newBlocks[12], newBlocks[13]);

			// L6:
			newBlocks[12].SetInstruction(Instruction.DecInstruction, esi);
			newBlocks[12].AppendInstruction(Instruction.JmpInstruction, newBlocks[13].BasicBlock);
			LinkBlocks(newBlocks[12], newBlocks[13]);

			// L7:
			newBlocks[13].SetInstruction(Instruction.XorInstruction, edx, edx);
			newBlocks[13].AppendInstruction(Instruction.MovInstruction, eax, esi);
			newBlocks[13].AppendInstruction(Instruction.JmpInstruction, newBlocks[14].BasicBlock);
			LinkBlocks(newBlocks[13], newBlocks[14]);

			;
			// ; Just the cleanup left to do.  edx:eax contains the quotient.  Set the sign
			// ; according to the save value, cleanup the stack, and return.
			// ;
			// L4:
			//        dec     edi             ; check to see if result is negative
			//        jnz     short L8        ; if EDI == 0, result should be negative
			//        neg     edx             ; otherwise, negate the result
			//        neg     eax
			//        sbb     edx,0
			newBlocks[14].SetInstruction(Instruction.DecInstruction, edi);
			newBlocks[14].AppendInstruction(Instruction.BranchInstruction, IR.ConditionCode.NotEqual, newBlocks[16].BasicBlock);
			newBlocks[14].AppendInstruction(Instruction.JmpInstruction, newBlocks[15].BasicBlock);
			LinkBlocks(newBlocks[14], newBlocks[15], newBlocks[16]);

			newBlocks[15].SetInstruction(Instruction.NegInstruction, edx);
			newBlocks[15].AppendInstruction(Instruction.NegInstruction, eax);
			newBlocks[15].AppendInstruction(Instruction.SbbInstruction, edx, new ConstantOperand(BuiltInSigType.Int32, 0));
			newBlocks[15].AppendInstruction(Instruction.JmpInstruction, newBlocks[16].BasicBlock);
			LinkBlocks(newBlocks[15], newBlocks[16]);

			newBlocks[16].SetInstruction(Instruction.MovInstruction, op0L, ueax);
			newBlocks[16].AppendInstruction(Instruction.MovInstruction, op0H, edx);
			newBlocks[16].AppendInstruction(Instruction.PopInstruction, ebx);
			newBlocks[16].AppendInstruction(Instruction.PopInstruction, esi);
			newBlocks[16].AppendInstruction(Instruction.PopInstruction, edi);
			newBlocks[15].AppendInstruction(Instruction.JmpInstruction, nextBlock.BasicBlock);
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
			RegisterOperand eax = new RegisterOperand(BuiltInSigType.Int32, GeneralPurposeRegister.EAX);
			RegisterOperand ebx = new RegisterOperand(BuiltInSigType.Int32, GeneralPurposeRegister.EBX);
			RegisterOperand edx = new RegisterOperand(BuiltInSigType.Int32, GeneralPurposeRegister.EDX);
			RegisterOperand ecx = new RegisterOperand(BuiltInSigType.Int32, GeneralPurposeRegister.ECX);
			RegisterOperand edi = new RegisterOperand(BuiltInSigType.Int32, GeneralPurposeRegister.EDI);
			RegisterOperand esi = new RegisterOperand(BuiltInSigType.Int32, GeneralPurposeRegister.ESI);

			Context[] newBlocks = CreateEmptyBlockContexts(context.Label, 16);
			Context nextBlock = SplitContext(context, false);

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
			context.SetInstruction(Instruction.JmpInstruction, newBlocks[0].BasicBlock);
			newBlocks[0].AppendInstruction(Instruction.PushInstruction, null, edi);
			newBlocks[0].AppendInstruction(Instruction.PushInstruction, null, esi);
			newBlocks[0].AppendInstruction(Instruction.PushInstruction, null, ebx);
			newBlocks[0].AppendInstruction(Instruction.XorInstruction, edi, edi);
			newBlocks[0].AppendInstruction(Instruction.MovInstruction, eax, op1H);
			newBlocks[0].AppendInstruction(Instruction.OrInstruction, eax, eax);
			newBlocks[0].AppendInstruction(Instruction.BranchInstruction, IR.ConditionCode.GreaterOrEqual, newBlocks[2].BasicBlock);
			newBlocks[0].AppendInstruction(Instruction.JmpInstruction, newBlocks[1].BasicBlock);
			LinkBlocks(newBlocks[0], newBlocks[2], newBlocks[1]);

			newBlocks[1].AppendInstruction(Instruction.IncInstruction, edi);
			newBlocks[1].AppendInstruction(Instruction.MovInstruction, edx, op1L);
			newBlocks[1].AppendInstruction(Instruction.NegInstruction, eax);
			newBlocks[1].AppendInstruction(Instruction.NegInstruction, edx);
			newBlocks[1].AppendInstruction(Instruction.SbbInstruction, eax, new ConstantOperand(BuiltInSigType.Int32, 0));
			newBlocks[1].AppendInstruction(Instruction.MovInstruction, op1H, eax);
			newBlocks[1].AppendInstruction(Instruction.MovInstruction, op1L, edx);
			newBlocks[1].AppendInstruction(Instruction.JmpInstruction, newBlocks[2].BasicBlock);
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
			newBlocks[2].AppendInstruction(Instruction.MovInstruction, eax, op2H);
			newBlocks[2].AppendInstruction(Instruction.OrInstruction, eax, eax);
			newBlocks[2].AppendInstruction(Instruction.BranchInstruction, IR.ConditionCode.GreaterOrEqual, newBlocks[4].BasicBlock);
			newBlocks[2].AppendInstruction(Instruction.JmpInstruction, newBlocks[3].BasicBlock);
			LinkBlocks(newBlocks[2], newBlocks[4], newBlocks[3]);

			newBlocks[3].AppendInstruction(Instruction.MovInstruction, edx, op2L);
			newBlocks[3].AppendInstruction(Instruction.NegInstruction, eax);
			newBlocks[3].AppendInstruction(Instruction.NegInstruction, edx);
			newBlocks[3].AppendInstruction(Instruction.SbbInstruction, eax, new ConstantOperand(BuiltInSigType.Int32, 0));
			newBlocks[3].AppendInstruction(Instruction.MovInstruction, op2H, eax);
			newBlocks[3].AppendInstruction(Instruction.MovInstruction, op2L, edx);
			newBlocks[3].AppendInstruction(Instruction.JmpInstruction, newBlocks[4].BasicBlock);
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
			newBlocks[4].AppendInstruction(Instruction.OrInstruction, eax, eax);
			newBlocks[4].AppendInstruction(Instruction.BranchInstruction, IR.ConditionCode.NotEqual, newBlocks[6].BasicBlock);
			newBlocks[4].AppendInstruction(Instruction.JmpInstruction, newBlocks[5].BasicBlock);
			LinkBlocks(newBlocks[4], newBlocks[6], newBlocks[5]);

			newBlocks[5].AppendInstruction(Instruction.MovInstruction, ecx, op2L);
			newBlocks[5].AppendInstruction(Instruction.MovInstruction, eax, op1H);
			newBlocks[5].AppendInstruction(Instruction.XorInstruction, edx, edx);
			newBlocks[5].AppendInstruction(Instruction.DirectDivisionInstruction, eax, ecx);
			newBlocks[5].AppendInstruction(Instruction.MovInstruction, eax, op1L);
			newBlocks[5].AppendInstruction(Instruction.DirectDivisionInstruction, eax, ecx);
			newBlocks[5].AppendInstruction(Instruction.MovInstruction, eax, edx);
			newBlocks[5].AppendInstruction(Instruction.XorInstruction, edx, edx);
			newBlocks[5].AppendInstruction(Instruction.DecInstruction, edi);
			newBlocks[5].AppendInstruction(Instruction.BranchInstruction, IR.ConditionCode.NotSigned, newBlocks[14].BasicBlock);
			newBlocks[5].AppendInstruction(Instruction.JmpInstruction, newBlocks[15].BasicBlock);
			LinkBlocks(newBlocks[5], newBlocks[14], newBlocks[15]);

			// Here we do it the hard way.  Remember, eax contains the high word of DVSR
			//
			// L3:
			//        mov     ebx,eax         ; ebx:ecx <- divisor
			//        mov     ecx,LOWORD(DVSR)
			//        mov     edx,HIWORD(DVND) ; edx:eax <- dividend
			//        mov     eax,LOWORD(DVND)
			newBlocks[6].AppendInstruction(Instruction.MovInstruction, ebx, eax);
			newBlocks[6].AppendInstruction(Instruction.MovInstruction, ecx, op2L);
			newBlocks[6].AppendInstruction(Instruction.MovInstruction, edx, op1H);
			newBlocks[6].AppendInstruction(Instruction.MovInstruction, eax, op1L);
			newBlocks[6].AppendInstruction(Instruction.JmpInstruction, newBlocks[7].BasicBlock);
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

			newBlocks[7].AppendInstruction(Instruction.ShrInstruction, ebx, new ConstantOperand(BuiltInSigType.Byte, 1));
			newBlocks[7].AppendInstruction(Instruction.RcrInstruction, ecx, new ConstantOperand(BuiltInSigType.Byte, 1)); // RCR
			newBlocks[7].AppendInstruction(Instruction.ShrInstruction, edx, new ConstantOperand(BuiltInSigType.Byte, 1));
			newBlocks[7].AppendInstruction(Instruction.RcrInstruction, eax, new ConstantOperand(BuiltInSigType.Byte, 1));
			newBlocks[7].AppendInstruction(Instruction.OrInstruction, ebx, ebx);
			newBlocks[7].AppendInstruction(Instruction.BranchInstruction, IR.ConditionCode.NotEqual, newBlocks[7].BasicBlock);
			newBlocks[7].AppendInstruction(Instruction.JmpInstruction, newBlocks[8].BasicBlock);
			LinkBlocks(newBlocks[7], newBlocks[7], newBlocks[8]);

			newBlocks[8].AppendInstruction(Instruction.DirectDivisionInstruction, eax, ecx);
			newBlocks[8].AppendInstruction(Instruction.MovInstruction, ecx, eax);
			newBlocks[8].AppendInstruction(Instruction.MulInstruction, null, op2H);
			newBlocks[8].AppendInstruction(Instruction.XchgInstruction, ecx, eax);
			newBlocks[8].AppendInstruction(Instruction.MulInstruction, null, op2L);
			newBlocks[8].AppendInstruction(Instruction.AddInstruction, edx, ecx);
			newBlocks[8].AppendInstruction(Instruction.BranchInstruction, IR.ConditionCode.UnsignedLessThan, newBlocks[12].BasicBlock);
			newBlocks[8].AppendInstruction(Instruction.JmpInstruction, newBlocks[9].BasicBlock);
			LinkBlocks(newBlocks[8], newBlocks[12], newBlocks[9]);

			newBlocks[9].AppendInstruction(Instruction.DirectCompareInstruction, edx, op1H);
			newBlocks[9].AppendInstruction(Instruction.BranchInstruction, IR.ConditionCode.UnsignedGreaterThan, newBlocks[12].BasicBlock);
			newBlocks[9].AppendInstruction(Instruction.JmpInstruction, newBlocks[10].BasicBlock);
			LinkBlocks(newBlocks[9], newBlocks[12], newBlocks[10]);

			newBlocks[10].AppendInstruction(Instruction.BranchInstruction, IR.ConditionCode.UnsignedLessThan, newBlocks[13].BasicBlock);
			newBlocks[10].AppendInstruction(Instruction.JmpInstruction, newBlocks[11].BasicBlock);
			LinkBlocks(newBlocks[10], newBlocks[13], newBlocks[11]);

			newBlocks[11].AppendInstruction(Instruction.DirectCompareInstruction, eax, op1L);
			newBlocks[11].AppendInstruction(Instruction.BranchInstruction, IR.ConditionCode.UnsignedLessOrEqual, newBlocks[13].BasicBlock);
			newBlocks[11].AppendInstruction(Instruction.JmpInstruction, newBlocks[12].BasicBlock);
			LinkBlocks(newBlocks[11], newBlocks[13], newBlocks[12]);

			// L6:
			newBlocks[12].AppendInstruction(Instruction.SubInstruction, eax, op2L);
			newBlocks[12].AppendInstruction(Instruction.SbbInstruction, edx, op2H);
			newBlocks[12].AppendInstruction(Instruction.JmpInstruction, newBlocks[13].BasicBlock);
			LinkBlocks(newBlocks[13], newBlocks[13]);

			// L7:
			//
			// Calculate remainder by subtracting the result from the original dividend.
			// Since the result is already in a register, we will do the subtract in the
			// opposite direction and negate the result if necessary.
			//
			newBlocks[13].AppendInstruction(Instruction.SubInstruction, eax, op1L);
			newBlocks[13].AppendInstruction(Instruction.SbbInstruction, edx, op1H);
			newBlocks[13].AppendInstruction(Instruction.DecInstruction, edi);
			newBlocks[13].AppendInstruction(Instruction.BranchInstruction, IR.ConditionCode.NotSigned, newBlocks[15].BasicBlock);
			newBlocks[13].AppendInstruction(Instruction.JmpInstruction, newBlocks[14].BasicBlock);
			LinkBlocks(newBlocks[13], newBlocks[14], newBlocks[15]);

			// L4:
			//        neg     edx             ; otherwise, negate the result
			//        neg     eax
			//        sbb     edx,0
			newBlocks[14].AppendInstruction(Instruction.NegInstruction, edx);
			newBlocks[14].AppendInstruction(Instruction.NegInstruction, eax);
			newBlocks[14].AppendInstruction(Instruction.SbbInstruction, edx, new ConstantOperand(BuiltInSigType.Int32, 0));
			newBlocks[14].AppendInstruction(Instruction.JmpInstruction, newBlocks[15].BasicBlock);
			LinkBlocks(newBlocks[14], newBlocks[15]);

			newBlocks[15].SetInstruction(Instruction.MovInstruction, op0L, eax);
			newBlocks[15].AppendInstruction(Instruction.MovInstruction, op0H, edx);
			newBlocks[15].AppendInstruction(Instruction.PopInstruction, ebx);
			newBlocks[15].AppendInstruction(Instruction.PopInstruction, esi);
			newBlocks[15].AppendInstruction(Instruction.PopInstruction, edi);
			newBlocks[15].AppendInstruction(Instruction.JmpInstruction, nextBlock.BasicBlock);
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
			RegisterOperand eax = new RegisterOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.EAX);
			RegisterOperand ebx = new RegisterOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.EBX);
			RegisterOperand edx = new RegisterOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.EDX);
			RegisterOperand ecx = new RegisterOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.ECX);
			RegisterOperand edi = new RegisterOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.EDI);
			RegisterOperand esi = new RegisterOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.ESI);

			Context[] newBlocks = CreateEmptyBlockContexts(context.Label, 12);
			Context nextBlock = SplitContext(context, false);
			context.SetInstruction(Instruction.JmpInstruction, newBlocks[0].BasicBlock);
			newBlocks[0].AppendInstruction(Instruction.PushInstruction, null, edi);
			newBlocks[0].AppendInstruction(Instruction.PushInstruction, null, esi);
			newBlocks[0].AppendInstruction(Instruction.PushInstruction, null, ebx);
			newBlocks[0].AppendInstruction(Instruction.MovInstruction, eax, op2H);
			newBlocks[0].AppendInstruction(Instruction.OrInstruction, eax, eax);
			newBlocks[0].AppendInstruction(Instruction.BranchInstruction, IR.ConditionCode.NotEqual, newBlocks[2].BasicBlock); // JNZ
			newBlocks[0].AppendInstruction(Instruction.JmpInstruction, newBlocks[1].BasicBlock);
			LinkBlocks(newBlocks[0], newBlocks[2], newBlocks[1]);

			newBlocks[1].AppendInstruction(Instruction.MovInstruction, ecx, op2L);
			newBlocks[1].AppendInstruction(Instruction.MovInstruction, eax, op1H);
			newBlocks[1].AppendInstruction(Instruction.XorInstruction, edx, edx);
			newBlocks[1].AppendInstruction(Instruction.DirectDivisionInstruction, eax, ecx);
			newBlocks[1].AppendInstruction(Instruction.MovInstruction, ebx, eax);
			newBlocks[1].AppendInstruction(Instruction.MovInstruction, eax, op1L);
			newBlocks[1].AppendInstruction(Instruction.DirectDivisionInstruction, eax, ecx);
			newBlocks[1].AppendInstruction(Instruction.MovInstruction, edx, ebx);
			newBlocks[1].AppendInstruction(Instruction.JmpInstruction, newBlocks[10].BasicBlock);
			LinkBlocks(newBlocks[0], newBlocks[10]);

			// L1
			newBlocks[2].AppendInstruction(Instruction.MovInstruction, ecx, eax);
			newBlocks[2].AppendInstruction(Instruction.MovInstruction, ebx, op2L);
			newBlocks[2].AppendInstruction(Instruction.MovInstruction, edx, op1H);
			newBlocks[2].AppendInstruction(Instruction.MovInstruction, eax, op1L);
			newBlocks[2].AppendInstruction(Instruction.JmpInstruction, newBlocks[3].BasicBlock);
			LinkBlocks(newBlocks[2], newBlocks[3]);

			// L3
			newBlocks[3].AppendInstruction(Instruction.ShrInstruction, ecx, new ConstantOperand(BuiltInSigType.Byte, 1));
			newBlocks[3].AppendInstruction(Instruction.RcrInstruction, ebx, new ConstantOperand(BuiltInSigType.Byte, 1)); // RCR
			newBlocks[3].AppendInstruction(Instruction.ShrInstruction, edx, new ConstantOperand(BuiltInSigType.Byte, 1));
			newBlocks[3].AppendInstruction(Instruction.RcrInstruction, eax, new ConstantOperand(BuiltInSigType.Byte, 1));
			newBlocks[3].AppendInstruction(Instruction.OrInstruction, ecx, ecx);
			newBlocks[3].AppendInstruction(Instruction.BranchInstruction, IR.ConditionCode.NotEqual, newBlocks[3].BasicBlock); // JNZ
			newBlocks[3].AppendInstruction(Instruction.JmpInstruction, newBlocks[4].BasicBlock);
			LinkBlocks(newBlocks[3], newBlocks[3], newBlocks[4]);

			newBlocks[4].AppendInstruction(Instruction.DirectDivisionInstruction, eax, ebx);
			newBlocks[4].AppendInstruction(Instruction.MovInstruction, esi, eax);
			newBlocks[4].AppendInstruction(Instruction.MulInstruction, null, op2H);
			newBlocks[4].AppendInstruction(Instruction.MovInstruction, ecx, eax);
			newBlocks[4].AppendInstruction(Instruction.MovInstruction, eax, op2L);
			newBlocks[4].AppendInstruction(Instruction.MulInstruction, null, esi);
			newBlocks[4].AppendInstruction(Instruction.AddInstruction, edx, ecx);
			newBlocks[4].AppendInstruction(Instruction.BranchInstruction, IR.ConditionCode.UnsignedLessThan, newBlocks[8].BasicBlock);
			newBlocks[4].AppendInstruction(Instruction.JmpInstruction, newBlocks[5].BasicBlock);
			LinkBlocks(newBlocks[4], newBlocks[8], newBlocks[5]);

			newBlocks[5].AppendInstruction(Instruction.DirectCompareInstruction, edx, op1H);
			newBlocks[5].AppendInstruction(Instruction.BranchInstruction, IR.ConditionCode.UnsignedGreaterThan, newBlocks[8].BasicBlock);
			newBlocks[5].AppendInstruction(Instruction.JmpInstruction, newBlocks[6].BasicBlock);
			LinkBlocks(newBlocks[5], newBlocks[8], newBlocks[6]);

			newBlocks[6].AppendInstruction(Instruction.BranchInstruction, IR.ConditionCode.UnsignedLessThan, newBlocks[9].BasicBlock);
			newBlocks[6].AppendInstruction(Instruction.JmpInstruction, newBlocks[7].BasicBlock);
			LinkBlocks(newBlocks[6], newBlocks[9], newBlocks[7]);

			newBlocks[7].AppendInstruction(Instruction.DirectCompareInstruction, eax, op1L);
			newBlocks[7].AppendInstruction(Instruction.BranchInstruction, IR.ConditionCode.UnsignedLessOrEqual, newBlocks[9].BasicBlock);
			newBlocks[7].AppendInstruction(Instruction.JmpInstruction, newBlocks[8].BasicBlock);
			LinkBlocks(newBlocks[7], newBlocks[9], newBlocks[8]);

			// L4:
			newBlocks[8].AppendInstruction(Instruction.DecInstruction, esi);
			newBlocks[8].AppendInstruction(Instruction.JmpInstruction, newBlocks[9].BasicBlock);
			LinkBlocks(newBlocks[8], newBlocks[9]);

			// L5
			newBlocks[9].AppendInstruction(Instruction.XorInstruction, edx, edx);
			newBlocks[9].AppendInstruction(Instruction.MovInstruction, eax, esi);
			newBlocks[9].AppendInstruction(Instruction.JmpInstruction, newBlocks[10].BasicBlock);
			LinkBlocks(newBlocks[9], newBlocks[10]);

			// L2
			newBlocks[10].SetInstruction(Instruction.MovInstruction, op0L, eax);
			newBlocks[10].AppendInstruction(Instruction.MovInstruction, op0H, edx);
			newBlocks[10].AppendInstruction(Instruction.PopInstruction, ebx);
			newBlocks[10].AppendInstruction(Instruction.PopInstruction, esi);
			newBlocks[10].AppendInstruction(Instruction.PopInstruction, edi);
			newBlocks[10].AppendInstruction(Instruction.JmpInstruction, nextBlock.BasicBlock);
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
			RegisterOperand eax = new RegisterOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.EAX);
			RegisterOperand ebx = new RegisterOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.EBX);
			RegisterOperand edx = new RegisterOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.EDX);
			RegisterOperand ecx = new RegisterOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.ECX);
			RegisterOperand edi = new RegisterOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.EDI);
			RegisterOperand esi = new RegisterOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.ESI);

			Context[] newBlocks = CreateEmptyBlockContexts(context.Label, 11);
			Context nextBlock = SplitContext(context, false);

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
			context.SetInstruction(Instruction.JmpInstruction, newBlocks[0].BasicBlock);
			newBlocks[0].AppendInstruction(Instruction.PushInstruction, null, edi);
			newBlocks[0].AppendInstruction(Instruction.PushInstruction, null, esi);
			newBlocks[0].AppendInstruction(Instruction.PushInstruction, null, ebx);
			newBlocks[0].AppendInstruction(Instruction.MovInstruction, eax, op2H);
			newBlocks[0].AppendInstruction(Instruction.OrInstruction, eax, eax);
			newBlocks[0].AppendInstruction(Instruction.BranchInstruction, IR.ConditionCode.NotEqual, newBlocks[2].BasicBlock);
			newBlocks[0].AppendInstruction(Instruction.JmpInstruction, newBlocks[1].BasicBlock);
			LinkBlocks(newBlocks[0], newBlocks[2], newBlocks[1]);

			newBlocks[1].AppendInstruction(Instruction.MovInstruction, ecx, op2L);
			newBlocks[1].AppendInstruction(Instruction.MovInstruction, eax, op1H);
			newBlocks[1].AppendInstruction(Instruction.XorInstruction, edx, edx);
			newBlocks[1].AppendInstruction(Instruction.DirectDivisionInstruction, eax, ecx);
			newBlocks[1].AppendInstruction(Instruction.MovInstruction, eax, op1L);
			newBlocks[1].AppendInstruction(Instruction.DirectDivisionInstruction, eax, ecx);
			newBlocks[1].AppendInstruction(Instruction.MovInstruction, eax, edx);
			newBlocks[1].AppendInstruction(Instruction.XorInstruction, edx, edx);
			newBlocks[1].AppendInstruction(Instruction.JmpInstruction, newBlocks[10].BasicBlock);
			LinkBlocks(newBlocks[1], newBlocks[10]);

			// L1:
			newBlocks[2].AppendInstruction(Instruction.MovInstruction, ecx, eax);
			newBlocks[2].AppendInstruction(Instruction.MovInstruction, ebx, op2L);
			newBlocks[2].AppendInstruction(Instruction.MovInstruction, edx, op1H);
			newBlocks[2].AppendInstruction(Instruction.MovInstruction, eax, op1L);
			newBlocks[2].AppendInstruction(Instruction.JmpInstruction, newBlocks[3].BasicBlock);
			LinkBlocks(newBlocks[2], newBlocks[3]);

			// L3:
			newBlocks[3].AppendInstruction(Instruction.ShrInstruction, ecx, new ConstantOperand(BuiltInSigType.Byte, 1));
			newBlocks[3].AppendInstruction(Instruction.RcrInstruction, ebx, new ConstantOperand(BuiltInSigType.Byte, 1)); // RCR
			newBlocks[3].AppendInstruction(Instruction.ShrInstruction, edx, new ConstantOperand(BuiltInSigType.Byte, 1));
			newBlocks[3].AppendInstruction(Instruction.RcrInstruction, eax, new ConstantOperand(BuiltInSigType.Byte, 1));
			newBlocks[3].AppendInstruction(Instruction.OrInstruction, ecx, ecx);
			newBlocks[3].AppendInstruction(Instruction.BranchInstruction, IR.ConditionCode.NotEqual, newBlocks[3].BasicBlock);
			newBlocks[3].AppendInstruction(Instruction.JmpInstruction, newBlocks[4].BasicBlock);
			LinkBlocks(newBlocks[3], newBlocks[3], newBlocks[4]);

			newBlocks[4].AppendInstruction(Instruction.DirectDivisionInstruction, eax, ebx);
			newBlocks[4].AppendInstruction(Instruction.MovInstruction, ecx, eax);
			newBlocks[4].AppendInstruction(Instruction.MulInstruction, null, op2H);
			newBlocks[4].AppendInstruction(Instruction.XchgInstruction, ecx, eax);
			newBlocks[4].AppendInstruction(Instruction.MulInstruction, null, op2L);
			newBlocks[4].AppendInstruction(Instruction.AddInstruction, edx, ecx);
			newBlocks[4].AppendInstruction(Instruction.BranchInstruction, IR.ConditionCode.UnsignedLessThan, newBlocks[8].BasicBlock);
			newBlocks[4].AppendInstruction(Instruction.JmpInstruction, newBlocks[5].BasicBlock);
			LinkBlocks(newBlocks[4], newBlocks[8], newBlocks[5]);

			newBlocks[5].AppendInstruction(Instruction.DirectCompareInstruction, edx, op1H);
			newBlocks[5].AppendInstruction(Instruction.BranchInstruction, IR.ConditionCode.UnsignedGreaterThan, newBlocks[8].BasicBlock);
			newBlocks[5].AppendInstruction(Instruction.JmpInstruction, newBlocks[6].BasicBlock);
			LinkBlocks(newBlocks[5], newBlocks[8], newBlocks[6]);

			newBlocks[6].AppendInstruction(Instruction.BranchInstruction, IR.ConditionCode.UnsignedLessThan, newBlocks[9].BasicBlock);
			newBlocks[6].AppendInstruction(Instruction.JmpInstruction, newBlocks[7].BasicBlock);
			LinkBlocks(newBlocks[6], newBlocks[6], newBlocks[7]);

			newBlocks[7].AppendInstruction(Instruction.DirectCompareInstruction, eax, op1L);
			newBlocks[7].AppendInstruction(Instruction.BranchInstruction, IR.ConditionCode.UnsignedLessOrEqual, newBlocks[9].BasicBlock);
			newBlocks[7].AppendInstruction(Instruction.JmpInstruction, newBlocks[3].BasicBlock);
			LinkBlocks(newBlocks[7], newBlocks[9], newBlocks[3]);

			// L4:
			newBlocks[8].AppendInstruction(Instruction.SubInstruction, eax, op2L);
			newBlocks[8].AppendInstruction(Instruction.SbbInstruction, edx, op2H);
			newBlocks[8].AppendInstruction(Instruction.JmpInstruction, newBlocks[9].BasicBlock);
			LinkBlocks(newBlocks[8], newBlocks[9]);

			// L5:
			newBlocks[9].AppendInstruction(Instruction.SubInstruction, eax, op1L);
			newBlocks[9].AppendInstruction(Instruction.SbbInstruction, edx, op1H);
			newBlocks[9].AppendInstruction(Instruction.NegInstruction, edx);
			newBlocks[9].AppendInstruction(Instruction.NegInstruction, eax);
			newBlocks[9].AppendInstruction(Instruction.SbbInstruction, edx, new ConstantOperand(BuiltInSigType.UInt32, (int)0));
			newBlocks[9].AppendInstruction(Instruction.JmpInstruction, newBlocks[10].BasicBlock);
			LinkBlocks(newBlocks[9], newBlocks[10]);

			newBlocks[10].SetInstruction(Instruction.MovInstruction, op0L, eax);
			newBlocks[10].AppendInstruction(Instruction.MovInstruction, op0H, edx);
			newBlocks[10].AppendInstruction(Instruction.PopInstruction, ebx);
			newBlocks[10].AppendInstruction(Instruction.PopInstruction, esi);
			newBlocks[10].AppendInstruction(Instruction.PopInstruction, edi);
			newBlocks[10].AppendInstruction(Instruction.JmpInstruction, nextBlock.BasicBlock);
			LinkBlocks(newBlocks[10], nextBlock);
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
			RegisterOperand eax = new RegisterOperand(BuiltInSigType.Int32, GeneralPurposeRegister.EAX);
			RegisterOperand edx = new RegisterOperand(BuiltInSigType.Int32, GeneralPurposeRegister.EDX);
			RegisterOperand ecx = new RegisterOperand(BuiltInSigType.Int32, GeneralPurposeRegister.ECX);

			Context[] newBlocks = CreateEmptyBlockContexts(context.Label, 6);
			Context nextBlock = SplitContext(context, true);

			// Handle shifts of 64 bits or more (if shifting 64 bits or more, the result
			// depends only on the high order bit of edx).
			context.SetInstruction(Instruction.JmpInstruction, newBlocks[0].BasicBlock);
			newBlocks[0].AppendInstruction(Instruction.PushInstruction, null, ecx);
			newBlocks[0].AppendInstruction(IR.Instruction.LogicalAndInstruction, count, count, new ConstantOperand(BuiltInSigType.Int32, 0x3F));
			newBlocks[0].AppendInstruction(Instruction.MovInstruction, ecx, count);
			newBlocks[0].AppendInstruction(Instruction.MovInstruction, edx, op1H);
			newBlocks[0].AppendInstruction(Instruction.MovInstruction, eax, op1L);
			newBlocks[0].AppendInstruction(Instruction.DirectCompareInstruction, ecx, new ConstantOperand(BuiltInSigType.Int32, 64));
			newBlocks[0].AppendInstruction(Instruction.BranchInstruction, IR.ConditionCode.UnsignedGreaterOrEqual, newBlocks[4].BasicBlock);
			newBlocks[0].AppendInstruction(Instruction.JmpInstruction, newBlocks[1].BasicBlock);
			LinkBlocks(newBlocks[0], newBlocks[4], newBlocks[1]);

			newBlocks[1].AppendInstruction(Instruction.DirectCompareInstruction, ecx, new ConstantOperand(BuiltInSigType.Byte, 32));
			newBlocks[1].AppendInstruction(Instruction.BranchInstruction, IR.ConditionCode.UnsignedGreaterOrEqual, newBlocks[3].BasicBlock);
			newBlocks[1].AppendInstruction(Instruction.JmpInstruction, newBlocks[2].BasicBlock);
			LinkBlocks(newBlocks[1], newBlocks[3], newBlocks[2]);

			newBlocks[2].AppendInstruction(Instruction.ShrdInstruction, edx, eax, ecx);
			newBlocks[2].AppendInstruction(Instruction.SarInstruction, edx, ecx);
			newBlocks[2].AppendInstruction(Instruction.JmpInstruction, newBlocks[5].BasicBlock);
			LinkBlocks(newBlocks[2], newBlocks[5]);

			// Handle shifts of between 32 and 63 bits
			// MORE32:
			newBlocks[3].AppendInstruction(Instruction.MovInstruction, eax, edx);
			newBlocks[3].AppendInstruction(Instruction.SarInstruction, edx, new ConstantOperand(BuiltInSigType.Byte, (sbyte)0x1F));
			newBlocks[3].AppendInstruction(Instruction.AndInstruction, ecx, new ConstantOperand(BuiltInSigType.Int32, 0x1F));
			newBlocks[3].AppendInstruction(Instruction.SarInstruction, eax, ecx);
			newBlocks[3].AppendInstruction(Instruction.JmpInstruction, newBlocks[5].BasicBlock);
			LinkBlocks(newBlocks[3], nextBlock);

			// Return double precision 0 or -1, depending on the sign of edx
			// RETSIGN:
			newBlocks[4].AppendInstruction(Instruction.SarInstruction, edx, new ConstantOperand(BuiltInSigType.Byte, (sbyte)0x1F));
			newBlocks[4].AppendInstruction(Instruction.MovInstruction, eax, edx);
			newBlocks[4].AppendInstruction(Instruction.JmpInstruction, newBlocks[5].BasicBlock);
			LinkBlocks(newBlocks[4], newBlocks[5]);

			// done:
			// ; remaining code from current basic block
			newBlocks[5].SetInstruction(Instruction.MovInstruction, op0H, edx);
			newBlocks[5].AppendInstruction(Instruction.MovInstruction, op0L, eax);
			newBlocks[5].AppendInstruction(Instruction.PopInstruction, ecx);
			newBlocks[5].AppendInstruction(Instruction.JmpInstruction, nextBlock.BasicBlock);
			LinkBlocks(newBlocks[5], nextBlock);
		}

		/// <summary>
		/// Expands the shift left instruction for 64-bits.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ExpandShiftLeft(Context context)
		{
			Operand count = context.Operand2;  //  FIXME PG

			Operand op0H, op1H, op0L, op1L;
			SplitLongOperand(context.Result, out op0L, out op0H);
			SplitLongOperand(context.Operand1, out op1L, out op1H);
			RegisterOperand eax = new RegisterOperand(BuiltInSigType.Int32, GeneralPurposeRegister.EAX);
			RegisterOperand edx = new RegisterOperand(BuiltInSigType.Int32, GeneralPurposeRegister.EDX);
			RegisterOperand ecx = new RegisterOperand(BuiltInSigType.Int32, GeneralPurposeRegister.ECX);

			RegisterOperand cl = new RegisterOperand(BuiltInSigType.Byte, GeneralPurposeRegister.ECX);

			Context nextBlock = SplitContext(context, true);
			Context[] newBlocks = CreateEmptyBlockContexts(context.Label, 6);

			// Handle shifts of 64 bits or more (if shifting 64 bits or more, the result
			// depends only on the high order bit of edx).
			context.SetInstruction(Instruction.JmpInstruction, newBlocks[0].BasicBlock);
			newBlocks[0].AppendInstruction(Instruction.PushInstruction, null, ecx);
			newBlocks[0].AppendInstruction(IR.Instruction.LogicalAndInstruction, count, count, new ConstantOperand(BuiltInSigType.Int32, 0x3F));
			newBlocks[0].AppendInstruction(Instruction.MovInstruction, ecx, count);
			newBlocks[0].AppendInstruction(Instruction.MovInstruction, edx, op1H);
			newBlocks[0].AppendInstruction(Instruction.MovInstruction, eax, op1L);
			newBlocks[0].AppendInstruction(Instruction.DirectCompareInstruction, ecx, new ConstantOperand(BuiltInSigType.Int32, 64));
			newBlocks[0].AppendInstruction(Instruction.BranchInstruction, IR.ConditionCode.UnsignedGreaterOrEqual, newBlocks[4].BasicBlock);
			newBlocks[0].AppendInstruction(Instruction.JmpInstruction, newBlocks[1].BasicBlock);
			LinkBlocks(newBlocks[0], newBlocks[4], newBlocks[1]);

			newBlocks[1].AppendInstruction(Instruction.DirectCompareInstruction, ecx, new ConstantOperand(BuiltInSigType.Int32, 32));
			newBlocks[1].AppendInstruction(Instruction.BranchInstruction, IR.ConditionCode.UnsignedGreaterOrEqual, newBlocks[3].BasicBlock);
			newBlocks[1].AppendInstruction(Instruction.JmpInstruction, newBlocks[2].BasicBlock);
			LinkBlocks(newBlocks[1], newBlocks[3], newBlocks[2]);

			newBlocks[2].AppendInstruction(Instruction.ShldInstruction, eax, edx, cl);
			newBlocks[2].AppendInstruction(Instruction.ShlInstruction, eax, cl);
			newBlocks[2].AppendInstruction(Instruction.JmpInstruction, newBlocks[5].BasicBlock);
			LinkBlocks(newBlocks[2], newBlocks[5]);

			// Handle shifts of between 32 and 63 bits
			// MORE32:
			newBlocks[3].AppendInstruction(Instruction.MovInstruction, edx, eax);
			newBlocks[3].AppendInstruction(Instruction.XorInstruction, eax, eax);
			newBlocks[3].AppendInstruction(Instruction.AndInstruction, ecx, new ConstantOperand(BuiltInSigType.Int32, 0x1F));
			newBlocks[3].AppendInstruction(Instruction.ShlInstruction, edx, ecx);
			newBlocks[3].AppendInstruction(Instruction.JmpInstruction, newBlocks[5].BasicBlock);
			LinkBlocks(newBlocks[3], newBlocks[5]);

			// Return double precision 0 or -1, depending on the sign of edx
			// RETZERO:
			newBlocks[4].AppendInstruction(Instruction.XorInstruction, eax, eax);
			newBlocks[4].AppendInstruction(Instruction.XorInstruction, edx, edx);
			newBlocks[4].AppendInstruction(Instruction.JmpInstruction, newBlocks[5].BasicBlock);
			LinkBlocks(newBlocks[4], newBlocks[5]);

			// done:
			// ; remaining code from current basic block
			newBlocks[5].AppendInstruction(Instruction.MovInstruction, op0H, edx);
			newBlocks[5].AppendInstruction(Instruction.MovInstruction, op0L, eax);
			newBlocks[5].AppendInstruction(Instruction.PopInstruction, ecx);
			newBlocks[5].AppendInstruction(Instruction.JmpInstruction, nextBlock.BasicBlock);
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
			SplitLongOperand(context.Operand1, out op0L, out op0H);
			SplitLongOperand(context.Operand2, out op1L, out op1H);
			RegisterOperand eax = new RegisterOperand(BuiltInSigType.Int32, GeneralPurposeRegister.EAX);
			RegisterOperand edx = new RegisterOperand(BuiltInSigType.Int32, GeneralPurposeRegister.EDX);
			RegisterOperand ecx = new RegisterOperand(BuiltInSigType.Byte, GeneralPurposeRegister.ECX);

			Context[] newBlocks = CreateEmptyBlockContexts(context.Label, 6);
			Context nextBlock = SplitContext(context, true);

			// Handle shifts of 64 bits or more (if shifting 64 bits or more, the result
			// depends only on the high order bit of edx).
			context.SetInstruction(Instruction.JmpInstruction, newBlocks[0].BasicBlock);
			newBlocks[0].AppendInstruction(Instruction.PushInstruction, null, ecx);
			newBlocks[0].AppendInstruction(IR.Instruction.LogicalAndInstruction, count, count, new ConstantOperand(BuiltInSigType.Int32, 0x3F));
			newBlocks[0].AppendInstruction(Instruction.MovInstruction, ecx, count);
			newBlocks[0].AppendInstruction(Instruction.MovInstruction, edx, op1H);
			newBlocks[0].AppendInstruction(Instruction.MovInstruction, eax, op1L);
			newBlocks[0].AppendInstruction(Instruction.CmpInstruction, ecx, new ConstantOperand(BuiltInSigType.Int32, 64));
			newBlocks[0].AppendInstruction(Instruction.BranchInstruction, IR.ConditionCode.UnsignedGreaterOrEqual, newBlocks[4].BasicBlock);
			newBlocks[0].AppendInstruction(Instruction.JmpInstruction, newBlocks[1].BasicBlock);
			LinkBlocks(newBlocks[0], newBlocks[4], newBlocks[1]);

			newBlocks[1].AppendInstruction(Instruction.CmpInstruction, ecx, new ConstantOperand(BuiltInSigType.Int32, 32));
			newBlocks[1].AppendInstruction(Instruction.BranchInstruction, IR.ConditionCode.UnsignedGreaterOrEqual, newBlocks[3].BasicBlock);
			newBlocks[1].AppendInstruction(Instruction.JmpInstruction, newBlocks[2].BasicBlock);
			LinkBlocks(newBlocks[3], newBlocks[2], newBlocks[1]);

			newBlocks[2].AppendInstruction(Instruction.ShrdInstruction, eax, edx, ecx);
			newBlocks[2].AppendInstruction(Instruction.SarInstruction, edx, ecx);
			newBlocks[2].AppendInstruction(Instruction.JmpInstruction, newBlocks[5].BasicBlock);
			LinkBlocks(newBlocks[2], newBlocks[5]);

			// Handle shifts of between 32 and 63 bits
			// MORE32:
			newBlocks[3].AppendInstruction(Instruction.MovInstruction, eax, edx);
			newBlocks[3].AppendInstruction(Instruction.PushInstruction, null, ecx);
			newBlocks[3].AppendInstruction(Instruction.MovInstruction, ecx, new ConstantOperand(BuiltInSigType.SByte, (sbyte)0x1F));
			newBlocks[3].AppendInstruction(Instruction.SarInstruction, edx, ecx);
			newBlocks[3].AppendInstruction(Instruction.PopInstruction, ecx);
			newBlocks[3].AppendInstruction(Instruction.AndInstruction, ecx, new ConstantOperand(BuiltInSigType.Int32, 0x1F));
			newBlocks[3].AppendInstruction(Instruction.PushInstruction, null, ecx);
			newBlocks[3].AppendInstruction(Instruction.MovInstruction, ecx, new ConstantOperand(BuiltInSigType.SByte, (sbyte)0x1F));
			newBlocks[3].AppendInstruction(Instruction.SarInstruction, eax, ecx);
			newBlocks[3].AppendInstruction(Instruction.PopInstruction, ecx);
			newBlocks[3].AppendInstruction(Instruction.JmpInstruction, newBlocks[5].BasicBlock);
			LinkBlocks(newBlocks[3], newBlocks[5]);

			// Return double precision 0 or -1, depending on the sign of edx
			// RETSIGN:
			newBlocks[4].AppendInstruction(Instruction.SarInstruction, edx, new ConstantOperand(BuiltInSigType.SByte, (sbyte)0x1F));
			newBlocks[4].AppendInstruction(Instruction.MovInstruction, eax, edx);
			newBlocks[4].AppendInstruction(Instruction.JmpInstruction, newBlocks[5].BasicBlock);
			LinkBlocks(newBlocks[4], newBlocks[5]);

			// done:
			// ; remaining code from current basic block
			newBlocks[5].SetInstruction(Instruction.MovInstruction, op0H, edx);
			newBlocks[5].AppendInstruction(Instruction.MovInstruction, op0L, eax);
			newBlocks[5].AppendInstruction(Instruction.PopInstruction, ecx);
			newBlocks[5].AppendInstruction(Instruction.JmpInstruction, nextBlock.BasicBlock);
			LinkBlocks(newBlocks[5], nextBlock);
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

			context.SetInstruction(IR.Instruction.LogicalNotInstruction, op0H, op1H);
			context.AppendInstruction(IR.Instruction.LogicalNotInstruction, op0L, op1L);
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
				context.AppendInstruction(Instruction.MovInstruction, op0L, op1L);
				context.AppendInstruction(Instruction.AndInstruction, op0L, op2L);
			}
			else
			{
				context.SetInstruction(Instruction.MovInstruction, op0H, op1H);
				context.AppendInstruction(Instruction.MovInstruction, op0L, op1L);
				context.AppendInstruction(Instruction.AndInstruction, op0H, op2H);
				context.AppendInstruction(Instruction.AndInstruction, op0L, op2L);
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

			context.SetInstruction(Instruction.MovInstruction, op0H, op1H);
			context.AppendInstruction(Instruction.MovInstruction, op0L, op1L);
			context.AppendInstruction(Instruction.OrInstruction, op0H, op2H);
			context.AppendInstruction(Instruction.OrInstruction, op0L, op2L);
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

			context.SetInstruction(Instruction.MovInstruction, op0H, op1H);
			context.AppendInstruction(Instruction.MovInstruction, op0L, op1L);
			context.AppendInstruction(Instruction.XorInstruction, op0H, op2H);
			context.AppendInstruction(Instruction.XorInstruction, op0L, op2L);
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

				context.SetInstruction(Instruction.MovInstruction, op0L, op1L);
				context.AppendInstruction(Instruction.MovInstruction, op0H, op1H);
			}
			else
			{
				SplitLongOperand(context.Operand1, out op1L, out op1H);
				context.SetInstruction(Instruction.MovInstruction, context.Result, op1L);
			}
		}

		/// <summary>
		/// Expands the unsigned move instruction for 64-bits.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ExpandUnsignedMove(Context context)
		{
			MemoryOperand op0 = context.Result as MemoryOperand;
			Operand op1 = context.Operand1;
			Debug.Assert(op0 != null, @"I8 not in a memory operand!");

			Operand op0L, op0H, op1L, op1H;
			SplitLongOperand(op0, out op0L, out op0H);
			SplitLongOperand(op1, out op1L, out op1H);
			RegisterOperand eax = new RegisterOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.EAX);
			RegisterOperand edx = new RegisterOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.EDX);

			switch (op1.Type.Type)
			{
				case CilElementType.Boolean:
					context.SetInstruction(IR.Instruction.ZeroExtendedMoveInstruction, op0L, op1L);
					context.AppendInstruction(IR.Instruction.LogicalXorInstruction, op0H, op0H, op0H);
					break;

				case CilElementType.U1:
					context.SetInstruction(IR.Instruction.ZeroExtendedMoveInstruction, eax, op1L);
					context.AppendInstruction(Instruction.CdqInstruction);
					context.AppendInstruction(Instruction.MovInstruction, op0L, eax);
					context.AppendInstruction(IR.Instruction.LogicalXorInstruction, op0H, op0H, op0H);
					break;

				case CilElementType.U2: goto case CilElementType.U1;

				case CilElementType.I4:
					context.SetInstruction(IR.Instruction.ZeroExtendedMoveInstruction, eax, op1L);
					context.AppendInstruction(Instruction.XorInstruction, edx, edx);
					context.AppendInstruction(Instruction.MovInstruction, op0L, eax);
					context.AppendInstruction(Instruction.MovInstruction, op0H, edx);
					break;
				case CilElementType.U4:
					context.SetInstruction(IR.Instruction.ZeroExtendedMoveInstruction, eax, op1L);
					context.AppendInstruction(Instruction.CdqInstruction);
					context.AppendInstruction(Instruction.MovInstruction, op0L, eax);
					context.AppendInstruction(Instruction.MovInstruction, op0H, edx);
					break;

				case CilElementType.U8:
					context.SetInstruction(IR.Instruction.ZeroExtendedMoveInstruction, op0L, op1L);
					context.SetInstruction(IR.Instruction.ZeroExtendedMoveInstruction, op0H, op1H);
					break;

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
			RegisterOperand eax = new RegisterOperand(BuiltInSigType.Int32, GeneralPurposeRegister.EAX);
			RegisterOperand edx = new RegisterOperand(BuiltInSigType.Int32, GeneralPurposeRegister.EDX);

			switch (op1.Type.Type)
			{
				case CilElementType.Boolean:
					context.SetInstruction(IR.Instruction.ZeroExtendedMoveInstruction, op0L, op1);
					context.AppendInstruction(IR.Instruction.LogicalXorInstruction, op0H, op0H, op0H);
					break;

				case CilElementType.I1:
					context.SetInstruction(IR.Instruction.SignExtendedMoveInstruction, eax, op1);
					context.AppendInstruction(Instruction.CdqInstruction);
					context.AppendInstruction(Instruction.MovInstruction, op0L, eax);
					context.AppendInstruction(Instruction.MovInstruction, op0H, edx);
					break;

				case CilElementType.I2: goto case CilElementType.I1;

				case CilElementType.I4:
					context.SetInstruction(Instruction.MovInstruction, eax, op1);
					context.AppendInstruction(Instruction.CdqInstruction);
					context.AppendInstruction(Instruction.MovInstruction, op0L, eax);
					context.AppendInstruction(Instruction.MovInstruction, op0H, edx);
					break;

				case CilElementType.I8:
					context.SetInstruction(Instruction.MovInstruction, op0, op1);
					break;

				case CilElementType.U1:
					context.SetInstruction(IR.Instruction.ZeroExtendedMoveInstruction, eax, op1);
					context.AppendInstruction(Instruction.CdqInstruction);
					context.AppendInstruction(Instruction.MovInstruction, op0L, eax);
					context.AppendInstruction(IR.Instruction.LogicalXorInstruction, op0H, op0H, op0H);
					break;

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
			Operand op0 = context.Result;
			Operand op1 = context.Operand1;
			Operand offsetOperand = context.Operand2;
			Debug.Assert(op0 != null && op1 != null, @"Operands to I8 LoadInstruction are not MemoryOperand.");

			Operand op0L, op0H;
			SplitLongOperand(op0, out op0L, out op0H);

			RegisterOperand eax = new RegisterOperand(BuiltInSigType.Int32, GeneralPurposeRegister.EAX);
			RegisterOperand edx = new RegisterOperand(BuiltInSigType.Int32, GeneralPurposeRegister.EDX);

			context.SetInstruction(Instruction.MovInstruction, eax, op1);
			context.AppendInstruction(Instruction.AddInstruction, eax, offsetOperand);
			context.AppendInstruction(Instruction.MovInstruction, edx, new MemoryOperand(op0L.Type, GeneralPurposeRegister.EAX, IntPtr.Zero));
			context.AppendInstruction(Instruction.MovInstruction, op0L, edx);
			context.AppendInstruction(Instruction.MovInstruction, edx, new MemoryOperand(op0H.Type, GeneralPurposeRegister.EAX, new IntPtr(4)));
			context.AppendInstruction(Instruction.MovInstruction, op0H, edx);
		}

		/// <summary>
		/// Expands the store instruction for 64-bits.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ExpandStore(Context context)
		{
			MemoryOperand op0 = context.Result as MemoryOperand;
			Operand offsetOperand = context.Operand1;
			MemoryOperand op2 = context.Operand2 as MemoryOperand;
			Debug.Assert(op0 != null && op2 != null, @"Operands to I8 LoadInstruction are not MemoryOperand.");

			Operand op1L, op1H;
			SplitLongOperand(op2, out op1L, out op1H);
			RegisterOperand eax = new RegisterOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.EAX);
			RegisterOperand edx = new RegisterOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.EDX);

			context.SetInstruction(Instruction.MovInstruction, edx, op0);

			// Fortunately in 32-bit mode, we can't have 64-bit offsets, so this plain add should suffice.
			context.AppendInstruction(Instruction.AddInstruction, edx, offsetOperand);

			context.AppendInstruction(Instruction.MovInstruction, eax, op1L);
			context.AppendInstruction(Instruction.MovInstruction, new MemoryOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.EDX, IntPtr.Zero), eax);
			context.AppendInstruction(Instruction.MovInstruction, eax, op1H);
			context.AppendInstruction(Instruction.MovInstruction, new MemoryOperand(BuiltInSigType.Int32, GeneralPurposeRegister.EDX, new IntPtr(4)), eax);
		}

		/// <summary>
		/// Expands the pop instruction for 64-bits.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ExpandPop(Context context)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Expands the push instruction for 64-bits.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ExpandPush(Context context)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Expands the unary branch instruction for 64-bits.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ExpandUnaryBranch(Context context)
		{
			Debug.Assert(context.Branch.Targets.Length == 2);

			BasicBlock targetBlock = FindBlock(context.Branch.Targets[0]);

			Operand op1H, op1L, op2H, op2L;
			Operand zero = new ConstantOperand(BuiltInSigType.Int32, (int)0);
			SplitLongOperand(context.Operand1, out op1L, out op1H);
			SplitLongOperand(zero, out op2L, out op2H);
			IR.ConditionCode code;

			switch (((context.Instruction) as CIL.ICILInstruction).OpCode)
			{
				// Signed
				case CIL.OpCode.Brtrue: code = IR.ConditionCode.NotEqual; break;
				case CIL.OpCode.Brfalse: code = IR.ConditionCode.Equal; break;
				case CIL.OpCode.Beq_s: code = IR.ConditionCode.Equal; break;
				case CIL.OpCode.Bge_s: code = IR.ConditionCode.GreaterOrEqual; break;
				case CIL.OpCode.Bgt_s: code = IR.ConditionCode.GreaterThan; break;
				case CIL.OpCode.Ble_s: code = IR.ConditionCode.LessOrEqual; break;
				case CIL.OpCode.Blt_s: code = IR.ConditionCode.LessThan; break;

				// Unsigned
				case CIL.OpCode.Bne_un_s: code = IR.ConditionCode.NotEqual; break;
				case CIL.OpCode.Bge_un_s: code = IR.ConditionCode.UnsignedGreaterOrEqual; break;
				case CIL.OpCode.Bgt_un_s: code = IR.ConditionCode.UnsignedGreaterThan; break;
				case CIL.OpCode.Ble_un_s: code = IR.ConditionCode.UnsignedLessOrEqual; break;
				case CIL.OpCode.Blt_un_s: code = IR.ConditionCode.UnsignedLessThan; break;

				// Long form signed
				case CIL.OpCode.Beq: goto case CIL.OpCode.Beq_s;
				case CIL.OpCode.Bge: goto case CIL.OpCode.Bge_s;
				case CIL.OpCode.Bgt: goto case CIL.OpCode.Bgt_s;
				case CIL.OpCode.Ble: goto case CIL.OpCode.Ble_s;
				case CIL.OpCode.Blt: goto case CIL.OpCode.Blt_s;

				// Long form unsigned
				case CIL.OpCode.Bne_un: goto case CIL.OpCode.Bne_un_s;
				case CIL.OpCode.Bge_un: goto case CIL.OpCode.Bge_un_s;
				case CIL.OpCode.Bgt_un: goto case CIL.OpCode.Bgt_un_s;
				case CIL.OpCode.Ble_un: goto case CIL.OpCode.Ble_un_s;
				case CIL.OpCode.Blt_un: goto case CIL.OpCode.Blt_un_s;
				default: throw new NotImplementedException();
			}

			Context[] newBlocks = CreateEmptyBlockContexts(context.Label, 3);
			Context nextBlock = SplitContext(context, false);

			context.SetInstruction(Instruction.JmpInstruction, newBlocks[0].BasicBlock);
			LinkBlocks(context, newBlocks[0]);
			// Compare high dwords
			newBlocks[0].AppendInstruction(Instruction.DirectCompareInstruction, op1H, op2H);
			// Branch if check already gave results
			newBlocks[0].AppendInstruction(Instruction.BranchInstruction, IR.ConditionCode.Equal, newBlocks[2].BasicBlock);
			newBlocks[0].AppendInstruction(Instruction.JmpInstruction, newBlocks[1].BasicBlock);
			LinkBlocks(newBlocks[0], newBlocks[1], newBlocks[2]);

			newBlocks[1].AppendInstruction(Instruction.BranchInstruction, code, targetBlock);
			newBlocks[1].AppendInstruction(Instruction.JmpInstruction);
			newBlocks[1].SetBranch(nextBlock.BasicBlock);
			LinkBlocks(newBlocks[1], targetBlock);
			LinkBlocks(newBlocks[1], nextBlock);

			// Compare low dwords
			newBlocks[2].SetInstruction(Instruction.DirectCompareInstruction, op1L, op2L);
			// Set the unsigned result...
			newBlocks[2].AppendInstruction(Instruction.BranchInstruction, code, targetBlock);
			newBlocks[2].AppendInstruction(Instruction.JmpInstruction);
			newBlocks[2].SetBranch(nextBlock.BasicBlock);
			LinkBlocks(newBlocks[2], targetBlock);
			LinkBlocks(newBlocks[2], nextBlock);
		}

		/// <summary>
		/// Expands the binary branch instruction for 64-bits.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ExpandBinaryBranch(Context context)
		{
			Debug.Assert(context.Branch.Targets.Length == 1);

			BasicBlock target = FindBlock(context.Branch.Targets[0]);

			Operand op1L, op1H, op2L, op2H;
			SplitLongOperand(context.Operand1, out op1L, out op1H);
			SplitLongOperand(context.Operand2, out op2L, out op2H);

			Context[] newBlocks = CreateEmptyBlockContexts(context.Label, 2);
			IR.ConditionCode conditionCode = context.ConditionCode;
			Context nextBlock = SplitContext(context, false);

			// Compare high dwords
			context.SetInstruction(Instruction.CmpInstruction, op1H, op2H);
			context.AppendInstruction(Instruction.BranchInstruction, IR.ConditionCode.Equal, newBlocks[1].BasicBlock);
			context.AppendInstruction(Instruction.JmpInstruction, newBlocks[0].BasicBlock);
			LinkBlocks(context, newBlocks[0], newBlocks[1]);

			// Branch if check already gave results
			newBlocks[0].SetInstruction(Instruction.BranchInstruction, conditionCode, target);
			newBlocks[0].AppendInstruction(Instruction.JmpInstruction, nextBlock.BasicBlock);
			LinkBlocks(newBlocks[0], target);
			LinkBlocks(newBlocks[0], nextBlock);

			// Compare low dwords
			newBlocks[1].SetInstruction(Instruction.CmpInstruction, op1L, op2L);
			// Set the unsigned result...
			newBlocks[1].AppendInstruction(Instruction.BranchInstruction, GetUnsignedConditionCode(conditionCode), target);
			newBlocks[1].AppendInstruction(Instruction.JmpInstruction, nextBlock.BasicBlock);
			LinkBlocks(newBlocks[1], target);
			LinkBlocks(newBlocks[1], nextBlock);
		}

		/// <summary>
		/// Gets the high condition.
		/// </summary>
		/// <param name="code">The code.</param>
		/// <returns></returns>
		private static IR.ConditionCode GetHighCondition(IR.ConditionCode code)
		{
			switch (code)
			{
				case IR.ConditionCode.Equal: return IR.ConditionCode.NotEqual;
				case IR.ConditionCode.GreaterOrEqual: return IR.ConditionCode.LessThan;
				case IR.ConditionCode.GreaterThan: return IR.ConditionCode.LessThan;
				case IR.ConditionCode.LessOrEqual: return IR.ConditionCode.GreaterThan;
				case IR.ConditionCode.LessThan: return IR.ConditionCode.GreaterThan;
				case IR.ConditionCode.NotEqual: return IR.ConditionCode.Equal;
				default: return code;
			}
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
			Debug.Assert(op0 is MemoryOperand || op0 is RegisterOperand, @"IntegerCompareInstruction result not memory and not register!");

			Operand op1L, op1H, op2L, op2H;
			SplitLongOperand(op1, out op1L, out op1H);
			SplitLongOperand(op2, out op2L, out op2H);

			Context[] newBlocks = CreateEmptyBlockContexts(context.Label, 4);
			IR.ConditionCode conditionCode = context.ConditionCode;
			Context nextBlock = SplitContext(context, false);

			// Compare high dwords
			context.SetInstruction(Instruction.CmpInstruction, op1H, op2H);
			context.AppendInstruction(Instruction.BranchInstruction, IR.ConditionCode.Equal, newBlocks[1].BasicBlock);
			context.AppendInstruction(Instruction.JmpInstruction, newBlocks[0].BasicBlock);
			LinkBlocks(context, newBlocks[0], newBlocks[1]);

			// Branch if check already gave results
			newBlocks[0].SetInstruction(Instruction.BranchInstruction, conditionCode, newBlocks[2].BasicBlock);
			newBlocks[0].AppendInstruction(Instruction.JmpInstruction, newBlocks[3].BasicBlock);
			LinkBlocks(newBlocks[0], newBlocks[2], newBlocks[3]);

			// Compare low dwords
			newBlocks[1].SetInstruction(Instruction.CmpInstruction, op1L, op2L);
			// Set the unsigned result...
			newBlocks[1].AppendInstruction(Instruction.BranchInstruction, GetUnsignedConditionCode(conditionCode), newBlocks[2].BasicBlock);
			newBlocks[1].AppendInstruction(Instruction.JmpInstruction, newBlocks[3].BasicBlock);
			LinkBlocks(newBlocks[1], newBlocks[2], newBlocks[3]);

			// Success
			newBlocks[2].SetInstruction(Instruction.MovsxInstruction, op0, new ConstantOperand(BuiltInSigType.Int32, 1));
			newBlocks[2].AppendInstruction(Instruction.JmpInstruction, nextBlock.BasicBlock);
			LinkBlocks(newBlocks[2], nextBlock);

			// Failed
			newBlocks[3].SetInstruction(Instruction.MovsxInstruction, op0, new ConstantOperand(BuiltInSigType.Int32, 0));
			newBlocks[3].AppendInstruction(Instruction.JmpInstruction, nextBlock.BasicBlock);
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
		/// Determines whether the specified op is double.
		/// </summary>
		/// <param name="op">The op.</param>
		/// <returns>
		/// 	<c>true</c> if the specified op is double; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsDouble(Operand op)
		{
			return op.Type.Matches(BuiltInSigType.Double);
		}

		/// <summary>
		/// Determines whether [is double or int64] [the specified op].
		/// </summary>
		/// <param name="op">The op.</param>
		/// <returns>
		/// 	<c>true</c> if [is double or int64] [the specified op]; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsDoubleOrInt64(Operand op)
		{
			return IsInt64(op) || IsDouble(op);
		}

		#endregion // Utility Methods

		#region IIRVisitor

		/// <summary>
		/// Arithmetics the shift right instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.ArithmeticShiftRightInstruction(Context context)
		{
			if (IsInt64(context.Operand1))
			{
				ExpandArithmeticShiftRight(context);
			}
		}

		/// <summary>
		/// Integers the compare instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.IntegerCompareBranchInstruction(Context context)
		{
			if (IsInt64(context.Operand1) || IsInt64(context.Operand2))
			{
				ExpandBinaryBranch(context);
			}
		}

		/// <summary>
		/// Integers the compare instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.IntegerCompareInstruction(Context context)
		{
			if (IsInt64(context.Operand1))
			{
				ExpandComparison(context);
			}
		}

		/// <summary>
		/// Loads the instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.LoadInstruction(Context context)
		{
			if (IsInt64(context.Operand1) || IsInt64(context.Result))
			{
				ExpandLoad(context);
			}
		}

		/// <summary>
		/// Logicals the and instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.LogicalAndInstruction(Context context)
		{
			if (IsInt64(context.Operand1))
			{
				ExpandAnd(context);
			}
		}

		/// <summary>
		/// Logicals the or instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.LogicalOrInstruction(Context context)
		{
			if (IsInt64(context.Operand1))
			{
				ExpandOr(context);
			}
		}

		/// <summary>
		/// Logicals the xor instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.LogicalXorInstruction(Context context)
		{
			if (IsInt64(context.Operand1))
			{
				ExpandXor(context);
			}
		}

		/// <summary>
		/// Logicals the not instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.LogicalNotInstruction(Context context)
		{
			if (IsInt64(context.Operand1))
			{
				ExpandNot(context);
			}
		}

		/// <summary>
		/// Moves the instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.MoveInstruction(Context context)
		{
			// FIXME: Why aren't we doing an SSE2 move for int64?
			if (IsInt64(context.Operand1))
			{
				ExpandMove(context);
			}
		}

		/// <summary>
		/// Shifts the left instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.ShiftLeftInstruction(Context context)
		{
			if (IsInt64(context.Operand1))
			{
				ExpandShiftLeft(context);
			}
		}

		/// <summary>
		/// Shifts the right instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.ShiftRightInstruction(Context context)
		{
			if (IsInt64(context.Operand1))
			{
				ExpandShiftRight(context);
			}
		}

		/// <summary>
		/// Signs the extended move instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.SignExtendedMoveInstruction(Context context)
		{
			if (IsInt64(context.Operand1) || IsInt64(context.Result))
			{
				ExpandSignedMove(context);
			}
		}

		/// <summary>
		/// Stores the instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.StoreInstruction(Context context)
		{
			if (IsInt64(context.Operand2))
			{
				ExpandStore(context);
			}
		}

		/// <summary>
		/// Visitation function for DivSInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.DivSInstruction(Context context)
		{
			if (IsInt64(context.Operand1))
			{
				ExpandDiv(context);
			}
		}

		void IR.IIRVisitor.DivUInstruction(Context context)
		{
			if (IsInt64(context.Operand1))
			{
				ExpandUDiv(context);
			}
		}

		/// <summary>
		/// Visitation function for MulSInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.MulSInstruction(Context context)
		{
			if (IsInt64(context.Operand1))
			{
				ExpandMul(context);
			}
		}

		/// <summary>
		/// Visitation function for MulFInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.MulFInstruction(Context context)
		{
		}

		/// <summary>
		/// Visitation function for MulUInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.MulUInstruction(Context context)
		{
			if (IsInt64(context.Operand1))
			{
				ExpandMul(context);
			}
		}

		/// <summary>
		/// Visitation function for SubFInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.SubFInstruction(Context context)
		{
		}

		/// <summary>
		/// Visitation function for SubSInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.SubSInstruction(Context context)
		{
			if (IsInt64(context.Operand1))
			{
				ExpandSub(context);
			}
			else
			{
				if (context.Operand2 is ConstantOperand && context.Operand1.Type.Type == CilElementType.Char)
				{
					RegisterOperand ecx = new RegisterOperand(context.Operand1.Type, GeneralPurposeRegister.ECX);
					context.InsertBefore().SetInstruction(Instruction.MovInstruction, ecx, context.Operand2);
					context.Operand2 = ecx;
				}
			}
		}

		/// <summary>
		/// Visitation function for SubUInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.SubUInstruction(Context context)
		{
			if (IsInt64(context.Operand1))
			{
				ExpandSub(context);
			}
		}

		/// <summary>
		/// Visitation function for RemFInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.RemFInstruction(Context context)
		{
		}

		/// <summary>
		/// Visitation function for RemSInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.RemSInstruction(Context context)
		{
			if (IsInt64(context.Operand1))
			{
				ExpandRem(context);
			}
		}

		/// <summary>
		/// Visitation function for RemUInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.RemUInstruction(Context context)
		{
			if (IsInt64(context.Operand1))
			{
				ExpandURem(context);
			}
		}

		/// <summary>
		/// Visitation function for SwitchInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.SwitchInstruction(Context context)
		{
		}

		/// <summary>
		/// Zeroes the extended move instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.ZeroExtendedMoveInstruction(Context context)
		{
			if (IsInt64(context.Result))
			{
				ExpandUnsignedMove(context);
			}
		}

		/// <summary>
		/// Visitation function for AddSInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.AddSInstruction(Context context)
		{
			if (IsInt64(context.Operand1))
			{
				ExpandAdd(context);
			}
		}

		/// <summary>
		/// Visitation function for AddUInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.AddUInstruction(Context context)
		{
			if (IsInt64(context.Operand1))
			{
				ExpandAdd(context);
			}
		}

		/// <summary>
		/// Visitation function for AddFInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.AddFInstruction(Context context)
		{
		}

		/// <summary>
		/// Visitation function for DivFInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.DivFInstruction(Context context)
		{
		}

		#endregion // IIRVisitor

		#region IIRVisitor - Unused

		/// <summary>
		/// Visitation function for BreakInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.BreakInstruction(Context context) { }

		/// <summary>
		/// Visitation function for AddressOfInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.AddressOfInstruction(Context context) { }

		/// <summary>
		/// Visitation function for CallInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.CallInstruction(Context context) { }

		/// <summary>
		/// Visitation function for EpilogueInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.EpilogueInstruction(Context context) { }

		/// <summary>
		/// Visitation function for FloatingPointCompareInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.FloatingPointCompareInstruction(Context context) { }

		/// <summary>
		/// Visitation function for FloatingPointToIntegerConversionInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.FloatingPointToIntegerConversionInstruction(Context context) { }

		/// <summary>
		/// Visitation function for IntegerToFloatingPointConversionInstruction instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.IntegerToFloatingPointConversionInstruction(Context context) { }

		/// <summary>
		/// Visitation function for JmpInstruction"/&gt; instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.JmpInstruction(Context context) { }

		/// <summary>
		/// Visitation function for PhiInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.PhiInstruction(Context context) { }

		/// <summary>
		/// Visitation function for PrologueInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.PrologueInstruction(Context context) { }

		/// <summary>
		/// Visitation function for ReturnInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.ReturnInstruction(Context context) { }

		/// <summary>
		/// Visitation function for NopInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.NopInstruction(Context context) { }

		/// <summary>
		/// Visitation function for ThrowInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.ThrowInstruction(Context context) { }

		/// <summary>
		/// Visitation function for ExceptionPrologueInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.ExceptionPrologueInstruction(Context context) { }

		#endregion // IIRVisitor - Unused

	}
}
