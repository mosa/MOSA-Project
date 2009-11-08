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

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Metadata;
using CIL = Mosa.Runtime.CompilerFramework.CIL;
using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Platforms.x86
{
	/// <summary>
	/// Transforms 64-bit arithmetic to 32-bit operations.
	/// </summary>
	/// <remarks>
	/// This stage translates all 64-bit operations to appropriate 32-bit operations on
	/// architectures without appropriate 64-bit integral operations.
	/// </remarks>
	public sealed class LongOperandTransformationStage : CodeTransformationStage, CIL.ICILVisitor, IR.IIRVisitor, IPlatformTransformationStage, IPipelineStage
	{
		#region IMethodCompilerStage Members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		string IPipelineStage.Name { get { return @"X86.LongOperandTransformationStage"; } }

		private static PipelineStageOrder[] _pipelineOrder = new PipelineStageOrder[] {
				new PipelineStageOrder(PipelineStageOrder.Location.After, typeof(StackLayoutStage)),
				new PipelineStageOrder(PipelineStageOrder.Location.Before, typeof(AddressModeConversionStage))
			};

		/// <summary>
		/// Gets the pipeline stage order.
		/// </summary>
		/// <value>The pipeline stage order.</value>
		PipelineStageOrder[] IPipelineStage.PipelineStageOrder { get { return _pipelineOrder; } }

		#endregion // IMethodCompilerStage Members

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
			if (operand.Type.Type != CilElementType.I8 && operand.Type.Type != CilElementType.U8) {
				operandLow = operand;
				operandHigh = new ConstantOperand(new SigType(CilElementType.I4), (int)0);
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
			SigType HighType = (operand.Type.Type == CilElementType.I8) ? new SigType(CilElementType.I4) : new SigType(CilElementType.U4);
			SigType U4 = new SigType(CilElementType.U4);

			ConstantOperand constantOperand = operand as ConstantOperand;

			if (HighType.Type == CilElementType.I4) {
				long value = (long)constantOperand.Value;
				operandLow = new ConstantOperand(U4, (uint)(value & 0xFFFFFFFF));
				operandHigh = new ConstantOperand(HighType, (int)(value >> 32));
			}
			else {
				ulong value = (ulong)constantOperand.Value;
				operandLow = new ConstantOperand(U4, (uint)(value & 0xFFFFFFFF));
				operandHigh = new ConstantOperand(HighType, (uint)(value >> 32));
			}
		}

		private static void SplitFromNonConstantOperand(Operand operand, out Operand operandLow, out Operand operandHigh)
		{
			SigType HighType = (operand.Type.Type == CilElementType.I8) ? new SigType(CilElementType.I4) : new SigType(CilElementType.U4);
			SigType U4 = new SigType(CilElementType.U4);

			// No, could be a member or a plain memory operand
			MemberOperand memberOperand = operand as MemberOperand;
			if (memberOperand != null) {
				// We need to keep the member reference, otherwise the linker can't fixup
				// the member address.
				operandLow = new MemberOperand(memberOperand.Member, U4, memberOperand.Offset);
				operandHigh = new MemberOperand(memberOperand.Member, HighType, new IntPtr(memberOperand.Offset.ToInt64() + 4));
			}
			else {
				// Plain memory, we can handle it here
				MemoryOperand memoryOperand = (MemoryOperand)operand;
				operandLow = new MemoryOperand(U4, memoryOperand.Base, memoryOperand.Offset);
				operandHigh = new MemoryOperand(HighType, memoryOperand.Base, new IntPtr(memoryOperand.Offset.ToInt64() + 4));
			}
		}

		/// <summary>
		/// Expands the add instruction for 64-bit operands.
		/// </summary>
		/// <param name="ctx">The context.</param>
		private void ExpandAdd(Context ctx)
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
			RegisterOperand eaxH = new RegisterOperand(new SigType(CilElementType.I4), GeneralPurposeRegister.EAX);
			RegisterOperand eaxL = new RegisterOperand(new SigType(CilElementType.U4), GeneralPurposeRegister.EAX);

			Operand op1H, op1L, op2H, op2L, resH, resL;
			SplitLongOperand(ctx.Operand1, out op1L, out op1H);
			SplitLongOperand(ctx.Operand2, out op2L, out op2H);
			SplitLongOperand(ctx.Result, out resL, out resH);

			ctx.SetInstruction(CPUx86.Instruction.MovInstruction, eaxL, op1L);
			ctx.AppendInstruction(CPUx86.Instruction.AddInstruction, eaxL, op2L);
			ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, resL, eaxL);
			ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, eaxH, op1H);
			ctx.AppendInstruction(CPUx86.Instruction.AdcInstruction, eaxH, op2H);
			ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, resH, eaxH);
		}

		/// <summary>
		/// Expands the sub instruction for 64-bit operands.
		/// </summary>
		/// <param name="ctx">The context.</param>
		private void ExpandSub(Context ctx)
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
			RegisterOperand eaxH = new RegisterOperand(new SigType(CilElementType.I4), GeneralPurposeRegister.EAX);
			RegisterOperand eaxL = new RegisterOperand(new SigType(CilElementType.U4), GeneralPurposeRegister.EAX);

			Operand op1L, op1H, op2L, op2H, resL, resH;
			SplitLongOperand(ctx.Operand1, out op1L, out op1H);
			SplitLongOperand(ctx.Operand2, out op2L, out op2H);
			SplitLongOperand(ctx.Result, out resL, out resH);

			ctx.SetInstruction(CPUx86.Instruction.MovInstruction, eaxL, op1L);
			ctx.AppendInstruction(CPUx86.Instruction.SubInstruction, eaxL, op2L);
			ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, resL, eaxL);
			ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, eaxH, op1H);
			ctx.AppendInstruction(CPUx86.Instruction.SbbInstruction, eaxH, op2H);
			ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, resH, eaxH);
		}

		/// <summary>
		/// Expands the mul instruction for 64-bit operands.
		/// </summary>
		/// <param name="ctx">The context.</param>
		private void ExpandMul(Context ctx)
		{
			Context nextBlock = SplitContext(ctx);
			Context[] newBlocks = CreateEmptyBlockContexts(ctx.Label, 4);

			MemoryOperand op0 = ctx.Result as MemoryOperand;
			MemoryOperand op1 = ctx.Operand1 as MemoryOperand;
			MemoryOperand op2 = ctx.Operand2 as MemoryOperand;
			Debug.Assert(op0 != null && op1 != null && op2 != null, @"Operands to 64 bit multiplication are not MemoryOperands.");

			SigType I4 = new SigType(CilElementType.I4);
			Operand op0H, op1H, op2H, op0L, op1L, op2L;
			SplitLongOperand(ctx.Result, out op0L, out op0H);
			SplitLongOperand(ctx.Operand1, out op1L, out op1H);
			SplitLongOperand(ctx.Operand2, out op2L, out op2H);

			RegisterOperand eax = new RegisterOperand(I4, GeneralPurposeRegister.EAX);
			RegisterOperand ebx = new RegisterOperand(I4, GeneralPurposeRegister.EBX);
			RegisterOperand ecx = new RegisterOperand(I4, GeneralPurposeRegister.ECX);
			RegisterOperand edx = new RegisterOperand(I4, GeneralPurposeRegister.EDX);

			newBlocks[0].AppendInstruction(CPUx86.Instruction.MovInstruction, eax, op1H);
			newBlocks[0].AppendInstruction(CPUx86.Instruction.MovInstruction, ecx, op2H);
			newBlocks[0].AppendInstruction(CPUx86.Instruction.OrInstruction, ecx, eax);
			newBlocks[0].AppendInstruction(CPUx86.Instruction.MovInstruction, ecx, op2L);
			newBlocks[0].AppendInstruction(IR.Instruction.BranchInstruction, IR.ConditionCode.NotEqual, newBlocks[2].BasicBlock);
			newBlocks[0].AppendInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[1].BasicBlock);

			newBlocks[1].AppendInstruction(CPUx86.Instruction.MovInstruction, eax, op1L);
			newBlocks[1].AppendInstruction(CPUx86.Instruction.MulInstruction, null, ecx);
			newBlocks[1].AppendInstruction(CPUx86.Instruction.JmpInstruction, nextBlock.BasicBlock);

			newBlocks[2].AppendInstruction(CPUx86.Instruction.PushInstruction, null, ebx);
			newBlocks[2].AppendInstruction(CPUx86.Instruction.MulInstruction, null, ecx);
			newBlocks[2].AppendInstruction(CPUx86.Instruction.MovInstruction, ebx, eax);
			newBlocks[2].AppendInstruction(CPUx86.Instruction.MovInstruction, eax, op1L);
			newBlocks[2].AppendInstruction(CPUx86.Instruction.MulInstruction, null, op2H);
			newBlocks[2].AppendInstruction(CPUx86.Instruction.AddInstruction, ebx, eax);
			newBlocks[2].AppendInstruction(CPUx86.Instruction.MovInstruction, eax, op1L);
			newBlocks[2].AppendInstruction(CPUx86.Instruction.MulInstruction, null, ecx);
			newBlocks[2].AppendInstruction(CPUx86.Instruction.AddInstruction, edx, ebx);
			newBlocks[2].AppendInstruction(CPUx86.Instruction.PopInstruction, ebx);
			newBlocks[2].AppendInstruction(CPUx86.Instruction.JmpInstruction, nextBlock.BasicBlock);

			nextBlock.AppendInstruction(CPUx86.Instruction.MovInstruction, op0L, eax);
			nextBlock.AppendInstruction(CPUx86.Instruction.MovInstruction, op0H, edx);

			// Link the created Blocks together
			LinkBlocks(newBlocks, ctx, nextBlock);
		}

		/// <summary>
		/// Expands the div.
		/// </summary>
		/// <param name="ctx">The context.</param>
		private void ExpandDiv(Context ctx)
		{
			Context[] newBlocks = CreateEmptyBlockContexts(ctx.Label, 16);
			Context nextBlock = SplitContext(ctx);

			SigType I4 = new SigType(CilElementType.I4);
			SigType U4 = new SigType(CilElementType.U4);
			SigType U1 = new SigType(CilElementType.U1);

			Operand op0H, op1H, op2H, op0L, op1L, op2L;
			SplitLongOperand(ctx.Result, out op0L, out op0H);
			SplitLongOperand(ctx.Operand1, out op1L, out op1H);
			SplitLongOperand(ctx.Operand2, out op2L, out op2H);
			RegisterOperand eax = new RegisterOperand(I4, GeneralPurposeRegister.EAX);
			RegisterOperand ebx = new RegisterOperand(I4, GeneralPurposeRegister.EBX);
			RegisterOperand edx = new RegisterOperand(I4, GeneralPurposeRegister.EDX);
			RegisterOperand ecx = new RegisterOperand(I4, GeneralPurposeRegister.ECX);
			RegisterOperand edi = new RegisterOperand(I4, GeneralPurposeRegister.EDI);
			RegisterOperand esi = new RegisterOperand(I4, GeneralPurposeRegister.ESI);

			RegisterOperand ueax = new RegisterOperand(U4, GeneralPurposeRegister.EAX);
			RegisterOperand uebx = new RegisterOperand(U4, GeneralPurposeRegister.EBX);
			RegisterOperand uedx = new RegisterOperand(U4, GeneralPurposeRegister.EDX);
			RegisterOperand uecx = new RegisterOperand(U4, GeneralPurposeRegister.ECX);
			RegisterOperand uedi = new RegisterOperand(U4, GeneralPurposeRegister.EDI);
			RegisterOperand uesi = new RegisterOperand(U4, GeneralPurposeRegister.ESI);

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
			newBlocks[0].AppendInstruction(CPUx86.Instruction.PushInstruction, null, edi);
			newBlocks[0].AppendInstruction(CPUx86.Instruction.PushInstruction, null, esi);
			newBlocks[0].AppendInstruction(CPUx86.Instruction.PushInstruction, null, ebx);
			newBlocks[0].AppendInstruction(CPUx86.Instruction.XorInstruction, edi, edi);
			newBlocks[0].AppendInstruction(CPUx86.Instruction.MovInstruction, eax, op1H);
			newBlocks[0].AppendInstruction(CPUx86.Instruction.OrInstruction, eax, eax);
			newBlocks[0].AppendInstruction(IR.Instruction.BranchInstruction, IR.ConditionCode.GreaterOrEqual, newBlocks[2].BasicBlock);
			newBlocks[0].AppendInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[1].BasicBlock);

			newBlocks[1].AppendInstruction(CPUx86.Instruction.IncInstruction, edi);
			newBlocks[1].AppendInstruction(CPUx86.Instruction.MovInstruction, uedx, op1L);
			newBlocks[1].AppendInstruction(CPUx86.Instruction.NegInstruction, eax);
			newBlocks[1].AppendInstruction(CPUx86.Instruction.NegInstruction, edx);
			newBlocks[1].AppendInstruction(CPUx86.Instruction.SbbInstruction, eax, new ConstantOperand(I4, 0));
			newBlocks[1].AppendInstruction(CPUx86.Instruction.MovInstruction, op1H, eax);
			newBlocks[1].AppendInstruction(CPUx86.Instruction.MovInstruction, op1L, uedx);
			newBlocks[1].AppendInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[2].BasicBlock);

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
			newBlocks[2].AppendInstruction(CPUx86.Instruction.MovInstruction, eax, op2H);
			newBlocks[2].AppendInstruction(CPUx86.Instruction.OrInstruction, eax, eax);
			newBlocks[2].AppendInstruction(IR.Instruction.BranchInstruction, IR.ConditionCode.GreaterOrEqual, newBlocks[4].BasicBlock);
			newBlocks[2].AppendInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[3].BasicBlock);

			newBlocks[3].AppendInstruction(CPUx86.Instruction.IncInstruction, edi);
			newBlocks[3].AppendInstruction(CPUx86.Instruction.MovInstruction, uedx, op2L);
			newBlocks[3].AppendInstruction(CPUx86.Instruction.NegInstruction, eax);
			newBlocks[3].AppendInstruction(CPUx86.Instruction.NegInstruction, edx);
			newBlocks[3].AppendInstruction(CPUx86.Instruction.SbbInstruction, eax, new ConstantOperand(I4, 0));
			newBlocks[3].AppendInstruction(CPUx86.Instruction.MovInstruction, op2H, eax);
			newBlocks[3].AppendInstruction(CPUx86.Instruction.MovInstruction, op2L, uedx);
			newBlocks[3].AppendInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[4].BasicBlock);

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
			newBlocks[4].AppendInstruction(CPUx86.Instruction.OrInstruction, eax, eax);
			newBlocks[4].AppendInstruction(IR.Instruction.BranchInstruction, IR.ConditionCode.NotEqual, newBlocks[6].BasicBlock);
			newBlocks[4].AppendInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[5].BasicBlock);

			newBlocks[5].AppendInstruction(CPUx86.Instruction.MovInstruction, uecx, op2L);
			newBlocks[5].AppendInstruction(CPUx86.Instruction.MovInstruction, eax, op1H);
			newBlocks[5].AppendInstruction(CPUx86.Instruction.XorInstruction, edx, edx);
			newBlocks[5].AppendInstruction(CPUx86.Instruction.DivInstruction, null, ecx);
			newBlocks[5].AppendInstruction(CPUx86.Instruction.MovInstruction, ebx, eax);
			newBlocks[5].AppendInstruction(CPUx86.Instruction.MovInstruction, ueax, op1L);
			newBlocks[5].AppendInstruction(CPUx86.Instruction.DivInstruction, null, ecx);
			newBlocks[5].AppendInstruction(CPUx86.Instruction.MovInstruction, edx, ebx);
			newBlocks[5].AppendInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[14].BasicBlock);

			// Here we do it the hard way.  Remember, eax contains the high word of DVSR
			//
			// L3:
			//        mov     ebx,eax         ; ebx:ecx <- divisor
			//        mov     ecx,LOWORD(DVSR)
			//        mov     edx,HIWORD(DVND) ; edx:eax <- dividend
			//        mov     eax,LOWORD(DVND)
			newBlocks[6].AppendInstruction(CPUx86.Instruction.MovInstruction, ebx, eax);
			newBlocks[6].AppendInstruction(CPUx86.Instruction.MovInstruction, uecx, op2L);
			newBlocks[6].AppendInstruction(CPUx86.Instruction.MovInstruction, edx, op1H);
			newBlocks[6].AppendInstruction(CPUx86.Instruction.MovInstruction, ueax, op1L);
			newBlocks[6].AppendInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[7].BasicBlock);

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
			// ; by the divisor and check the result against the orignal dividend
			// ; Note that we must also check for overflow, which can occur if the
			// ; dividend is close to 2**64 and the quotient is off by 1.
			// ;

			// mul     dword ptr HIWORD(DVSR) ; QUOT * HIWORD(DVSR)
			// mov     ecx,eax
			// mov     eax,LOWORD(DVSR)
			// mul     esi             ; QUOT * LOWORD(DVSR)
			// add     edx,ecx         ; EDX:EAX = QUOT * DVSR
			// jc      short L6        ; carry means Quotient is off by 1
			newBlocks[7].AppendInstruction(CPUx86.Instruction.ShrInstruction, ebx, new ConstantOperand(U1, 1));
			newBlocks[7].AppendInstruction(CPUx86.Instruction.RcrInstruction, ecx, new ConstantOperand(U1, 1)); // RCR
			newBlocks[7].AppendInstruction(CPUx86.Instruction.ShrInstruction, edx, new ConstantOperand(U1, 1));
			newBlocks[7].AppendInstruction(CPUx86.Instruction.RcrInstruction, eax, new ConstantOperand(U1, 1));
			newBlocks[7].AppendInstruction(CPUx86.Instruction.OrInstruction, ebx, ebx);
			newBlocks[7].AppendInstruction(IR.Instruction.BranchInstruction, IR.ConditionCode.NotEqual, newBlocks[7].BasicBlock);
			newBlocks[7].AppendInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[8].BasicBlock);

			newBlocks[8].AppendInstruction(CPUx86.Instruction.DivInstruction, null, ecx);
			newBlocks[8].AppendInstruction(CPUx86.Instruction.MovInstruction, esi, eax);
			newBlocks[8].AppendInstruction(CPUx86.Instruction.MulInstruction, null, op2H);
			newBlocks[8].AppendInstruction(CPUx86.Instruction.MovInstruction, ecx, eax);
			newBlocks[8].AppendInstruction(CPUx86.Instruction.MovInstruction, ueax, op2L);
			newBlocks[8].AppendInstruction(CPUx86.Instruction.MulInstruction, null, esi);
			newBlocks[8].AppendInstruction(CPUx86.Instruction.AddInstruction, edx, ecx);
			newBlocks[8].AppendInstruction(IR.Instruction.BranchInstruction, IR.ConditionCode.UnsignedLessThan, newBlocks[12].BasicBlock);
			newBlocks[8].AppendInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[9].BasicBlock);

			newBlocks[9].AppendInstruction(CPUx86.Instruction.CmpInstruction, edx, op1H);
			newBlocks[9].AppendInstruction(IR.Instruction.BranchInstruction, IR.ConditionCode.UnsignedGreaterThan, newBlocks[12].BasicBlock);
			newBlocks[9].AppendInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[10].BasicBlock);

			newBlocks[10].AppendInstruction(IR.Instruction.BranchInstruction, IR.ConditionCode.UnsignedLessThan, newBlocks[13].BasicBlock);
			newBlocks[10].AppendInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[11].BasicBlock);

			newBlocks[11].AppendInstruction(CPUx86.Instruction.CmpInstruction, ueax, op1L);
			newBlocks[11].AppendInstruction(IR.Instruction.BranchInstruction, IR.ConditionCode.UnsignedLessOrEqual, newBlocks[13].BasicBlock);
			newBlocks[11].AppendInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[12].BasicBlock);

			// L6:
			newBlocks[12].AppendInstruction(CPUx86.Instruction.DecInstruction, esi);
			newBlocks[12].AppendInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[13].BasicBlock);

			// L7:
			newBlocks[13].AppendInstruction(CPUx86.Instruction.XorInstruction, edx, edx);
			newBlocks[13].AppendInstruction(CPUx86.Instruction.MovInstruction, eax, esi);
			newBlocks[13].AppendInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[14].BasicBlock);

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
			newBlocks[14].AppendInstruction(CPUx86.Instruction.DecInstruction, edi);
			newBlocks[14].AppendInstruction(IR.Instruction.BranchInstruction, IR.ConditionCode.NotEqual, nextBlock.BasicBlock);
			newBlocks[14].AppendInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[15].BasicBlock);

			newBlocks[15].AppendInstruction(CPUx86.Instruction.NegInstruction, edx);
			newBlocks[15].AppendInstruction(CPUx86.Instruction.NegInstruction, eax);
			newBlocks[15].AppendInstruction(CPUx86.Instruction.SbbInstruction, edx, new ConstantOperand(I4, 0));
			newBlocks[15].AppendInstruction(CPUx86.Instruction.JmpInstruction, nextBlock.BasicBlock);

			nextBlock.SetInstruction(CPUx86.Instruction.MovInstruction, op0L, ueax);
			nextBlock.AppendInstruction(CPUx86.Instruction.MovInstruction, op0H, edx);
			nextBlock.AppendInstruction(CPUx86.Instruction.PopInstruction, ebx);
			nextBlock.AppendInstruction(CPUx86.Instruction.PopInstruction, esi);
			nextBlock.AppendInstruction(CPUx86.Instruction.PopInstruction, edi);

			// Link the created blocks together
			LinkBlocks(newBlocks, ctx, nextBlock);
		}

		/// <summary>
		/// Expands the rem.
		/// </summary>
		/// <param name="ctx">The context.</param>
		private void ExpandRem(Context ctx)
		{
			Context[] newBlocks = CreateEmptyBlockContexts(ctx.Label, 15);
			Context nextBlock = SplitContext(ctx);

			SigType I4 = new SigType(CilElementType.I4);
			SigType U1 = new SigType(CilElementType.U1);

			Operand op0H, op1H, op2H, op0L, op1L, op2L;
			SplitLongOperand(ctx.Result, out op0L, out op0H);
			SplitLongOperand(ctx.Operand1, out op1L, out op1H);
			SplitLongOperand(ctx.Operand2, out op2L, out op2H);
			RegisterOperand eax = new RegisterOperand(I4, GeneralPurposeRegister.EAX);
			RegisterOperand ebx = new RegisterOperand(I4, GeneralPurposeRegister.EBX);
			RegisterOperand edx = new RegisterOperand(I4, GeneralPurposeRegister.EDX);
			RegisterOperand ecx = new RegisterOperand(I4, GeneralPurposeRegister.ECX);
			RegisterOperand edi = new RegisterOperand(I4, GeneralPurposeRegister.EDI);
			RegisterOperand esi = new RegisterOperand(I4, GeneralPurposeRegister.ESI);

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
			newBlocks[0].AppendInstruction(CPUx86.Instruction.PushInstruction, null, edi);
			newBlocks[0].AppendInstruction(CPUx86.Instruction.PushInstruction, null, esi);
			newBlocks[0].AppendInstruction(CPUx86.Instruction.PushInstruction, null, ebx);
			newBlocks[0].AppendInstruction(CPUx86.Instruction.XorInstruction, edi, edi);
			newBlocks[0].AppendInstruction(CPUx86.Instruction.MovInstruction, eax, op1H);
			newBlocks[0].AppendInstruction(CPUx86.Instruction.OrInstruction, eax, eax);
			newBlocks[0].AppendInstruction(IR.Instruction.BranchInstruction, IR.ConditionCode.GreaterOrEqual, newBlocks[2].BasicBlock);
			newBlocks[0].AppendInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[1].BasicBlock);

			newBlocks[1].AppendInstruction(CPUx86.Instruction.IncInstruction, edi);
			newBlocks[1].AppendInstruction(CPUx86.Instruction.MovInstruction, edx, op1L);
			newBlocks[1].AppendInstruction(CPUx86.Instruction.NegInstruction, eax);
			newBlocks[1].AppendInstruction(CPUx86.Instruction.NegInstruction, edx);
			newBlocks[1].AppendInstruction(CPUx86.Instruction.SbbInstruction, eax, new ConstantOperand(I4, 0));
			newBlocks[1].AppendInstruction(CPUx86.Instruction.MovInstruction, op1H, eax);
			newBlocks[1].AppendInstruction(CPUx86.Instruction.MovInstruction, op1L, edx);
			newBlocks[1].AppendInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[2].BasicBlock);

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
			newBlocks[2].AppendInstruction(CPUx86.Instruction.MovInstruction, eax, op2H);
			newBlocks[2].AppendInstruction(CPUx86.Instruction.OrInstruction, eax, eax);
			newBlocks[2].AppendInstruction(IR.Instruction.BranchInstruction, IR.ConditionCode.GreaterOrEqual, newBlocks[4].BasicBlock);
			newBlocks[2].AppendInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[3].BasicBlock);

			newBlocks[3].AppendInstruction(CPUx86.Instruction.MovInstruction, edx, op2L);
			newBlocks[3].AppendInstruction(CPUx86.Instruction.NegInstruction, eax);
			newBlocks[3].AppendInstruction(CPUx86.Instruction.NegInstruction, edx);
			newBlocks[3].AppendInstruction(CPUx86.Instruction.SbbInstruction, eax, new ConstantOperand(I4, 0));
			newBlocks[3].AppendInstruction(CPUx86.Instruction.MovInstruction, op2H, eax);
			newBlocks[3].AppendInstruction(CPUx86.Instruction.MovInstruction, op2L, edx);
			newBlocks[3].AppendInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[4].BasicBlock);

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
			newBlocks[4].AppendInstruction(CPUx86.Instruction.OrInstruction, eax, eax);
			newBlocks[4].AppendInstruction(IR.Instruction.BranchInstruction, IR.ConditionCode.NotEqual, newBlocks[6].BasicBlock);
			newBlocks[4].AppendInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[5].BasicBlock);

			newBlocks[5].AppendInstruction(CPUx86.Instruction.MovInstruction, ecx, op2L);
			newBlocks[5].AppendInstruction(CPUx86.Instruction.MovInstruction, eax, op1H);
			newBlocks[5].AppendInstruction(CPUx86.Instruction.XorInstruction, edx, edx);
			newBlocks[5].AppendInstruction(CPUx86.Instruction.DivInstruction, null, ecx);
			newBlocks[5].AppendInstruction(CPUx86.Instruction.MovInstruction, eax, op1L);
			newBlocks[5].AppendInstruction(CPUx86.Instruction.DivInstruction, null, ecx);
			newBlocks[5].AppendInstruction(CPUx86.Instruction.MovInstruction, eax, edx);
			newBlocks[5].AppendInstruction(CPUx86.Instruction.XorInstruction, edx, edx);
			newBlocks[5].AppendInstruction(CPUx86.Instruction.DecInstruction, edi);
			newBlocks[5].AppendInstruction(CPUx86.Instruction.JnsInstruction, newBlocks[14].BasicBlock);
			newBlocks[5].AppendInstruction(CPUx86.Instruction.JmpInstruction, nextBlock.BasicBlock);

			// Here we do it the hard way.  Remember, eax contains the high word of DVSR
			//
			// L3:
			//        mov     ebx,eax         ; ebx:ecx <- divisor
			//        mov     ecx,LOWORD(DVSR)
			//        mov     edx,HIWORD(DVND) ; edx:eax <- dividend
			//        mov     eax,LOWORD(DVND)
			newBlocks[6].AppendInstruction(CPUx86.Instruction.MovInstruction, ebx, eax);
			newBlocks[6].AppendInstruction(CPUx86.Instruction.MovInstruction, ecx, op2L);
			newBlocks[6].AppendInstruction(CPUx86.Instruction.MovInstruction, edx, op1H);
			newBlocks[6].AppendInstruction(CPUx86.Instruction.MovInstruction, eax, op1L);
			newBlocks[6].AppendInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[7].BasicBlock);

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

			newBlocks[7].AppendInstruction(CPUx86.Instruction.ShrInstruction, ebx, new ConstantOperand(U1, 1));
			newBlocks[7].AppendInstruction(CPUx86.Instruction.RcrInstruction, ecx, new ConstantOperand(U1, 1)); // RCR
			newBlocks[7].AppendInstruction(CPUx86.Instruction.ShrInstruction, edx, new ConstantOperand(U1, 1));
			newBlocks[7].AppendInstruction(CPUx86.Instruction.RcrInstruction, eax, new ConstantOperand(U1, 1));
			newBlocks[7].AppendInstruction(CPUx86.Instruction.OrInstruction, ebx, ebx);
			newBlocks[7].AppendInstruction(IR.Instruction.BranchInstruction, IR.ConditionCode.NotEqual, newBlocks[7].BasicBlock);
			newBlocks[7].AppendInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[8].BasicBlock);

			newBlocks[8].AppendInstruction(CPUx86.Instruction.DivInstruction, null, ecx);
			newBlocks[8].AppendInstruction(CPUx86.Instruction.MovInstruction, ecx, eax);
			newBlocks[8].AppendInstruction(CPUx86.Instruction.MulInstruction, null, op2H);
			newBlocks[8].AppendInstruction(CPUx86.Instruction.XchgInstruction, ecx, eax);
			newBlocks[8].AppendInstruction(CPUx86.Instruction.MulInstruction, null, op2L);
			newBlocks[8].AppendInstruction(CPUx86.Instruction.AddInstruction, edx, ecx);
			newBlocks[8].AppendInstruction(IR.Instruction.BranchInstruction, IR.ConditionCode.UnsignedLessThan, newBlocks[12].BasicBlock);
			newBlocks[8].AppendInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[9].BasicBlock);

			newBlocks[9].AppendInstruction(CPUx86.Instruction.CmpInstruction, edx, op1H);
			newBlocks[9].AppendInstruction(IR.Instruction.BranchInstruction, IR.ConditionCode.UnsignedGreaterThan, newBlocks[12].BasicBlock);
			newBlocks[9].AppendInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[10].BasicBlock);

			newBlocks[10].AppendInstruction(IR.Instruction.BranchInstruction, IR.ConditionCode.UnsignedLessThan, newBlocks[13].BasicBlock);
			newBlocks[10].AppendInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[11].BasicBlock);

			newBlocks[11].AppendInstruction(CPUx86.Instruction.CmpInstruction, eax, op1L);
			newBlocks[11].AppendInstruction(IR.Instruction.BranchInstruction, IR.ConditionCode.UnsignedLessOrEqual, newBlocks[13].BasicBlock);
			newBlocks[11].AppendInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[12].BasicBlock);

			// L6:
			newBlocks[12].AppendInstruction(CPUx86.Instruction.SubInstruction, eax, op2L);
			newBlocks[12].AppendInstruction(CPUx86.Instruction.SbbInstruction, edx, op2H);
			newBlocks[12].AppendInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[13].BasicBlock);

			// L7:
			//
			// Calculate remainder by subtracting the result from the original dividend.
			// Since the result is already in a register, we will do the subtract in the
			// opposite direction and negate the result if necessary.
			//
			newBlocks[13].AppendInstruction(CPUx86.Instruction.SubInstruction, eax, op1L);
			newBlocks[13].AppendInstruction(CPUx86.Instruction.SbbInstruction, edx, op1H);
			newBlocks[13].AppendInstruction(CPUx86.Instruction.DecInstruction, edi);
			newBlocks[13].AppendInstruction(CPUx86.Instruction.JnsInstruction, nextBlock.BasicBlock);
			newBlocks[13].AppendInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[14].BasicBlock);

			// L4:
			//        neg     edx             ; otherwise, negate the result
			//        neg     eax
			//        sbb     edx,0
			newBlocks[14].AppendInstruction(CPUx86.Instruction.NegInstruction, edx);
			newBlocks[14].AppendInstruction(CPUx86.Instruction.NegInstruction, eax);
			newBlocks[14].AppendInstruction(CPUx86.Instruction.SbbInstruction, edx, new ConstantOperand(I4, 0));
			newBlocks[14].AppendInstruction(CPUx86.Instruction.JmpInstruction, nextBlock.BasicBlock);

			ctx.SetInstruction(CPUx86.Instruction.MovInstruction, op0L, eax);
			ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, op0H, edx);
			ctx.AppendInstruction(CPUx86.Instruction.PopInstruction, ebx);
			ctx.AppendInstruction(CPUx86.Instruction.PopInstruction, esi);
			ctx.AppendInstruction(CPUx86.Instruction.PopInstruction, edi);

			// Link the created Blocks together
			LinkBlocks(newBlocks, ctx, nextBlock);
		}

		/// <summary>
		/// Expands the udiv instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		private void ExpandUDiv(Context ctx)
		{
			SigType U4 = new SigType(CilElementType.U4);
			SigType U1 = new SigType(CilElementType.U1);

			Operand op0H, op1H, op2H, op0L, op1L, op2L;
			SplitLongOperand(ctx.Result, out op0L, out op0H);
			SplitLongOperand(ctx.Operand1, out op1L, out op1H);
			SplitLongOperand(ctx.Operand2, out op2L, out op2H);
			RegisterOperand eax = new RegisterOperand(U4, GeneralPurposeRegister.EAX);
			RegisterOperand ebx = new RegisterOperand(U4, GeneralPurposeRegister.EBX);
			RegisterOperand edx = new RegisterOperand(U4, GeneralPurposeRegister.EDX);
			RegisterOperand ecx = new RegisterOperand(U4, GeneralPurposeRegister.ECX);
			RegisterOperand edi = new RegisterOperand(U4, GeneralPurposeRegister.EDI);
			RegisterOperand esi = new RegisterOperand(U4, GeneralPurposeRegister.ESI);

			Context[] newBlocks = CreateEmptyBlockContexts(ctx.Label, 10);
			Context nextBlock = SplitContext(ctx);

			newBlocks[0].AppendInstruction(CPUx86.Instruction.PushInstruction, null, edi);
			newBlocks[0].AppendInstruction(CPUx86.Instruction.PushInstruction, null, esi);
			newBlocks[0].AppendInstruction(CPUx86.Instruction.PushInstruction, null, ebx);
			newBlocks[0].AppendInstruction(CPUx86.Instruction.MovInstruction, eax, op2H);
			newBlocks[0].AppendInstruction(CPUx86.Instruction.OrInstruction, eax, eax);
			newBlocks[0].AppendInstruction(IR.Instruction.BranchInstruction, IR.ConditionCode.NotEqual, newBlocks[2].BasicBlock); // JNZ
			newBlocks[0].AppendInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[1].BasicBlock);

			newBlocks[1].AppendInstruction(CPUx86.Instruction.MovInstruction, ecx, op2L);
			newBlocks[1].AppendInstruction(CPUx86.Instruction.MovInstruction, eax, op1H);
			newBlocks[1].AppendInstruction(CPUx86.Instruction.XorInstruction, edx, edx);
			newBlocks[1].AppendInstruction(CPUx86.Instruction.DivInstruction, null, ecx);
			newBlocks[1].AppendInstruction(CPUx86.Instruction.MovInstruction, ebx, eax);
			newBlocks[1].AppendInstruction(CPUx86.Instruction.MovInstruction, eax, op1L);
			newBlocks[1].AppendInstruction(CPUx86.Instruction.DivInstruction, null, ecx);
			newBlocks[1].AppendInstruction(CPUx86.Instruction.MovInstruction, edx, ebx);
			newBlocks[1].AppendInstruction(CPUx86.Instruction.JmpInstruction, nextBlock.BasicBlock);

			// L1
			newBlocks[2].AppendInstruction(CPUx86.Instruction.MovInstruction, ecx, eax);
			newBlocks[2].AppendInstruction(CPUx86.Instruction.MovInstruction, ebx, op2L);
			newBlocks[2].AppendInstruction(CPUx86.Instruction.MovInstruction, edx, op1H);
			newBlocks[2].AppendInstruction(CPUx86.Instruction.MovInstruction, eax, op1L);
			newBlocks[2].AppendInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[3].BasicBlock);

			// L3
			newBlocks[3].AppendInstruction(CPUx86.Instruction.ShrInstruction, ecx, new ConstantOperand(U1, 1));
			newBlocks[3].AppendInstruction(CPUx86.Instruction.RcrInstruction, ebx, new ConstantOperand(U1, 1)); // RCR
			newBlocks[3].AppendInstruction(CPUx86.Instruction.ShrInstruction, edx, new ConstantOperand(U1, 1));
			newBlocks[3].AppendInstruction(CPUx86.Instruction.RcrInstruction, eax, new ConstantOperand(U1, 1));
			newBlocks[3].AppendInstruction(CPUx86.Instruction.OrInstruction, ecx, ecx);
			newBlocks[3].AppendInstruction(IR.Instruction.BranchInstruction, IR.ConditionCode.NotEqual, newBlocks[3].BasicBlock); // JNZ
			newBlocks[3].AppendInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[4].BasicBlock);

			newBlocks[4].AppendInstruction(CPUx86.Instruction.DivInstruction, null, ebx);
			newBlocks[4].AppendInstruction(CPUx86.Instruction.MovInstruction, esi, eax);
			newBlocks[4].AppendInstruction(CPUx86.Instruction.MulInstruction, null, op2H);
			newBlocks[4].AppendInstruction(CPUx86.Instruction.MovInstruction, ecx, eax);
			newBlocks[4].AppendInstruction(CPUx86.Instruction.MovInstruction, eax, op2L);
			newBlocks[4].AppendInstruction(CPUx86.Instruction.MulInstruction, null, esi);
			newBlocks[4].AppendInstruction(CPUx86.Instruction.AddInstruction, edx, ecx);
			newBlocks[4].AppendInstruction(IR.Instruction.BranchInstruction, IR.ConditionCode.UnsignedLessThan, newBlocks[8].BasicBlock);
			newBlocks[4].AppendInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[5].BasicBlock);

			newBlocks[5].AppendInstruction(CPUx86.Instruction.CmpInstruction, edx, op1H);
			newBlocks[5].AppendInstruction(IR.Instruction.BranchInstruction, IR.ConditionCode.UnsignedGreaterThan, newBlocks[8].BasicBlock);
			newBlocks[5].AppendInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[6].BasicBlock);

			newBlocks[6].AppendInstruction(IR.Instruction.BranchInstruction, IR.ConditionCode.UnsignedLessThan, newBlocks[9].BasicBlock);
			newBlocks[6].AppendInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[7].BasicBlock);

			newBlocks[7].AppendInstruction(CPUx86.Instruction.CmpInstruction, eax, op1L);
			newBlocks[7].AppendInstruction(IR.Instruction.BranchInstruction, IR.ConditionCode.UnsignedLessOrEqual, newBlocks[9].BasicBlock);
			newBlocks[7].AppendInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[8].BasicBlock);

			// L4:
			newBlocks[8].AppendInstruction(CPUx86.Instruction.DecInstruction, esi);
			newBlocks[8].AppendInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[9].BasicBlock);

			// L5
			newBlocks[9].AppendInstruction(CPUx86.Instruction.XorInstruction, edx, edx);
			newBlocks[9].AppendInstruction(CPUx86.Instruction.MovInstruction, eax, esi);
			newBlocks[9].AppendInstruction(CPUx86.Instruction.JmpInstruction, nextBlock.BasicBlock);

			// L2
			ctx.SetInstruction(CPUx86.Instruction.MovInstruction, op0L, eax);
			ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, op0H, edx);
			ctx.AppendInstruction(CPUx86.Instruction.PopInstruction, ebx);
			ctx.AppendInstruction(CPUx86.Instruction.PopInstruction, esi);
			ctx.AppendInstruction(CPUx86.Instruction.PopInstruction, edi);

			// Link the created Blocks together
			LinkBlocks(newBlocks, ctx, nextBlock);
		}

		/// <summary>
		/// Expands the urem instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		private void ExpandURem(Context ctx)
		{
			SigType U4 = new SigType(CilElementType.U4);
			SigType U1 = new SigType(CilElementType.U1);

			Operand op0H, op1H, op2H, op0L, op1L, op2L;
			SplitLongOperand(ctx.Result, out op0L, out op0H);
			SplitLongOperand(ctx.Operand1, out op1L, out op1H);
			SplitLongOperand(ctx.Operand2, out op2L, out op2H);
			RegisterOperand eax = new RegisterOperand(U4, GeneralPurposeRegister.EAX);
			RegisterOperand ebx = new RegisterOperand(U4, GeneralPurposeRegister.EBX);
			RegisterOperand edx = new RegisterOperand(U4, GeneralPurposeRegister.EDX);
			RegisterOperand ecx = new RegisterOperand(U4, GeneralPurposeRegister.ECX);
			RegisterOperand edi = new RegisterOperand(U4, GeneralPurposeRegister.EDI);
			RegisterOperand esi = new RegisterOperand(U4, GeneralPurposeRegister.ESI);

			Context[] newBlocks = CreateEmptyBlockContexts(ctx.Label, 10);
			Context nextBlock = SplitContext(ctx);

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
			newBlocks[0].AppendInstruction(CPUx86.Instruction.PushInstruction, null, edi);
			newBlocks[0].AppendInstruction(CPUx86.Instruction.PushInstruction, null, esi);
			newBlocks[0].AppendInstruction(CPUx86.Instruction.PushInstruction, null, ebx);
			newBlocks[0].AppendInstruction(CPUx86.Instruction.MovInstruction, eax, op2H);
			newBlocks[0].AppendInstruction(CPUx86.Instruction.OrInstruction, eax, eax);
			newBlocks[0].AppendInstruction(IR.Instruction.BranchInstruction, IR.ConditionCode.NotEqual, newBlocks[2].BasicBlock);
			newBlocks[0].AppendInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[1].BasicBlock);

			newBlocks[1].AppendInstruction(CPUx86.Instruction.MovInstruction, ecx, op2L);
			newBlocks[1].AppendInstruction(CPUx86.Instruction.MovInstruction, eax, op1H);
			newBlocks[1].AppendInstruction(CPUx86.Instruction.XorInstruction, edx, edx);
			newBlocks[1].AppendInstruction(CPUx86.Instruction.DivInstruction, ecx);
			newBlocks[1].AppendInstruction(CPUx86.Instruction.MovInstruction, eax, op1L);
			newBlocks[1].AppendInstruction(CPUx86.Instruction.DivInstruction, ecx);
			newBlocks[1].AppendInstruction(CPUx86.Instruction.MovInstruction, eax, edx);
			newBlocks[1].AppendInstruction(CPUx86.Instruction.XorInstruction, edx, edx);
			newBlocks[1].AppendInstruction(CPUx86.Instruction.JmpInstruction, nextBlock.BasicBlock);

			// L1:
			newBlocks[2].AppendInstruction(CPUx86.Instruction.MovInstruction, ecx, eax);
			newBlocks[2].AppendInstruction(CPUx86.Instruction.MovInstruction, ebx, op2L);
			newBlocks[2].AppendInstruction(CPUx86.Instruction.MovInstruction, edx, op1H);
			newBlocks[2].AppendInstruction(CPUx86.Instruction.MovInstruction, eax, op1L);
			newBlocks[2].AppendInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[3].BasicBlock);

			// L3:
			newBlocks[3].AppendInstruction(CPUx86.Instruction.ShrInstruction, ecx, new ConstantOperand(U1, 1));
			newBlocks[3].AppendInstruction(CPUx86.Instruction.RcrInstruction, ebx, new ConstantOperand(U1, 1)); // RCR
			newBlocks[3].AppendInstruction(CPUx86.Instruction.ShrInstruction, edx, new ConstantOperand(U1, 1));
			newBlocks[3].AppendInstruction(CPUx86.Instruction.RcrInstruction, eax, new ConstantOperand(U1, 1));
			newBlocks[3].AppendInstruction(CPUx86.Instruction.OrInstruction, ecx, ecx);
			newBlocks[3].AppendInstruction(IR.Instruction.BranchInstruction, IR.ConditionCode.NotEqual, newBlocks[3].BasicBlock);
			newBlocks[3].AppendInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[4].BasicBlock);

			newBlocks[4].AppendInstruction(CPUx86.Instruction.DivInstruction, null, ebx);
			newBlocks[4].AppendInstruction(CPUx86.Instruction.MovInstruction, ecx, eax);
			newBlocks[4].AppendInstruction(CPUx86.Instruction.MulInstruction, null, op2H);
			newBlocks[4].AppendInstruction(CPUx86.Instruction.XchgInstruction, ecx, eax);
			newBlocks[4].AppendInstruction(CPUx86.Instruction.MulInstruction, null, op2L);
			newBlocks[4].AppendInstruction(CPUx86.Instruction.AddInstruction, edx, ecx);
			newBlocks[4].AppendInstruction(IR.Instruction.BranchInstruction, IR.ConditionCode.UnsignedLessThan, newBlocks[8].BasicBlock);
			newBlocks[4].AppendInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[5].BasicBlock);

			newBlocks[5].AppendInstruction(CPUx86.Instruction.CmpInstruction, edx, op1H);
			newBlocks[5].AppendInstruction(IR.Instruction.BranchInstruction, IR.ConditionCode.UnsignedGreaterThan, newBlocks[8].BasicBlock);
			newBlocks[5].AppendInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[6].BasicBlock);

			newBlocks[6].AppendInstruction(IR.Instruction.BranchInstruction, IR.ConditionCode.UnsignedLessThan, newBlocks[9].BasicBlock);
			newBlocks[6].AppendInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[7].BasicBlock);

			newBlocks[7].AppendInstruction(CPUx86.Instruction.CmpInstruction, eax, op1L);
			newBlocks[7].AppendInstruction(IR.Instruction.BranchInstruction, IR.ConditionCode.UnsignedLessOrEqual, newBlocks[9].BasicBlock);
			newBlocks[7].AppendInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[3].BasicBlock);

			// L4:
			newBlocks[8].AppendInstruction(CPUx86.Instruction.SubInstruction, eax, op2L);
			newBlocks[8].AppendInstruction(CPUx86.Instruction.SbbInstruction, edx, op2H);
			newBlocks[8].AppendInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[9].BasicBlock);

			// L5:
			newBlocks[9].AppendInstruction(CPUx86.Instruction.SubInstruction, eax, op1L);
			newBlocks[9].AppendInstruction(CPUx86.Instruction.SbbInstruction, edx, op1H);
			newBlocks[9].AppendInstruction(CPUx86.Instruction.NegInstruction, edx);
			newBlocks[9].AppendInstruction(CPUx86.Instruction.NegInstruction, eax);
			newBlocks[9].AppendInstruction(CPUx86.Instruction.SbbInstruction, edx, new ConstantOperand(U4, (int)0));
			newBlocks[9].AppendInstruction(CPUx86.Instruction.JmpInstruction, nextBlock.BasicBlock);

			ctx.SetInstruction(CPUx86.Instruction.MovInstruction, op0L, eax);
			ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, op0H, edx);
			ctx.AppendInstruction(CPUx86.Instruction.PopInstruction, ebx);
			ctx.AppendInstruction(CPUx86.Instruction.PopInstruction, esi);
			ctx.AppendInstruction(CPUx86.Instruction.PopInstruction, edi);

			// Link the created Blocks together
			LinkBlocks(newBlocks, ctx, nextBlock);
		}

		/// <summary>
		/// Expands the arithmetic shift right instruction for 64-bits.
		/// </summary>
		/// <param name="ctx">The context.</param>
		private void ExpandArithmeticShiftRight(Context ctx)
		{
			Context[] newBlocks = CreateEmptyBlockContexts(ctx.Label, 5);
			Context nextBlock = SplitContext(ctx);

			SigType I4 = new SigType(CilElementType.I4);
			SigType U1 = new SigType(CilElementType.U1);
			Operand count = ctx.Operand2;

			Operand op0H, op1H, op0L, op1L;
			SplitLongOperand(ctx.Operand1, out op0L, out op0H);
			SplitLongOperand(ctx.Operand2, out op1L, out op1H);
			RegisterOperand eax = new RegisterOperand(I4, GeneralPurposeRegister.EAX);
			RegisterOperand edx = new RegisterOperand(I4, GeneralPurposeRegister.EDX);
			RegisterOperand ecx = new RegisterOperand(I4, GeneralPurposeRegister.ECX);

			RegisterOperand cl = new RegisterOperand(new SigType(CilElementType.U1), GeneralPurposeRegister.ECX);

			// Handle shifts of 64 bits or more (if shifting 64 bits or more, the result
			// depends only on the high order bit of edx).
			newBlocks[0].AppendInstruction(CPUx86.Instruction.PushInstruction, null, ecx);
			newBlocks[0].AppendInstruction(IR.Instruction.LogicalAndInstruction, count, count, new ConstantOperand(I4, 0x3F));
			newBlocks[0].AppendInstruction(CPUx86.Instruction.MovInstruction, ecx, count);
			newBlocks[0].AppendInstruction(CPUx86.Instruction.MovInstruction, edx, op1H);
			newBlocks[0].AppendInstruction(CPUx86.Instruction.MovInstruction, eax, op1L);
			newBlocks[0].AppendInstruction(CPUx86.Instruction.CmpInstruction, ecx, new ConstantOperand(I4, 64));
			newBlocks[0].AppendInstruction(IR.Instruction.BranchInstruction, IR.ConditionCode.UnsignedGreaterOrEqual, newBlocks[4].BasicBlock);
			newBlocks[0].AppendInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[1].BasicBlock);

			newBlocks[1].AppendInstruction(CPUx86.Instruction.CmpInstruction, ecx, new ConstantOperand(U1, 32));
			newBlocks[1].AppendInstruction(IR.Instruction.BranchInstruction, IR.ConditionCode.UnsignedGreaterOrEqual, newBlocks[3].BasicBlock);
			newBlocks[1].AppendInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[2].BasicBlock);

			newBlocks[2].AppendInstruction(CPUx86.Instruction.ShrdInstruction, eax, edx, ecx);
			newBlocks[2].AppendInstruction(CPUx86.Instruction.SarInstruction, edx, ecx);
			newBlocks[2].AppendInstruction(CPUx86.Instruction.JmpInstruction, nextBlock.BasicBlock);

			// Handle shifts of between 32 and 63 bits
			// MORE32:
			newBlocks[3].AppendInstruction(CPUx86.Instruction.MovInstruction, eax, edx);
			newBlocks[3].AppendInstruction(CPUx86.Instruction.SarInstruction, edx, new ConstantOperand(U1, (sbyte)0x1F));
			newBlocks[3].AppendInstruction(CPUx86.Instruction.AndInstruction, ecx, new ConstantOperand(I4, 0x1F));
			newBlocks[3].AppendInstruction(CPUx86.Instruction.SarInstruction, eax, ecx);
			newBlocks[3].AppendInstruction(CPUx86.Instruction.JmpInstruction, nextBlock.BasicBlock);

			// Return double precision 0 or -1, depending on the sign of edx
			// RETSIGN:
			newBlocks[4].AppendInstruction(CPUx86.Instruction.SarInstruction, edx, new ConstantOperand(U1, (sbyte)0x1F));
			newBlocks[4].AppendInstruction(CPUx86.Instruction.MovInstruction, eax, edx);
			newBlocks[4].AppendInstruction(CPUx86.Instruction.JmpInstruction, nextBlock.BasicBlock);

			// done:
			// ; remaining code from current basic block
			ctx.SetInstruction(CPUx86.Instruction.MovInstruction, op0H, edx);
			ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, op0L, eax);
			ctx.AppendInstruction(CPUx86.Instruction.PopInstruction, ecx);

			// Link the created Blocks together
			LinkBlocks(newBlocks, ctx, nextBlock);
		}

		/// <summary>
		/// Expands the shift left instruction for 64-bits.
		/// </summary>
		/// <param name="ctx">The context.</param>
		private void ExpandShiftLeft(Context ctx)
		{
			Context nextBlock = SplitContext(ctx);
			Context[] newBlocks = CreateEmptyBlockContexts(ctx.Label, 5);

			SigType I4 = new SigType(CilElementType.I4);
			Operand count = ctx.Operand2;  //  FIXME PG

			Operand op0H, op1H, op0L, op1L;
			SplitLongOperand(ctx.Operand1, out op0L, out op0H);
			SplitLongOperand(ctx.Operand2, out op1L, out op1H);
			RegisterOperand eax = new RegisterOperand(I4, GeneralPurposeRegister.EAX);
			RegisterOperand edx = new RegisterOperand(I4, GeneralPurposeRegister.EDX);
			RegisterOperand ecx = new RegisterOperand(I4, GeneralPurposeRegister.ECX);

			RegisterOperand cl = new RegisterOperand(new SigType(CilElementType.U1), GeneralPurposeRegister.ECX);

			// Handle shifts of 64 bits or more (if shifting 64 bits or more, the result
			// depends only on the high order bit of edx).
			newBlocks[0].AppendInstruction(CPUx86.Instruction.PushInstruction, null, ecx);
			newBlocks[0].AppendInstruction(IR.Instruction.LogicalAndInstruction, count, count, new ConstantOperand(I4, 0x3F));
			newBlocks[0].AppendInstruction(CPUx86.Instruction.MovInstruction, ecx, count);
			newBlocks[0].AppendInstruction(CPUx86.Instruction.MovInstruction, edx, op1H);
			newBlocks[0].AppendInstruction(CPUx86.Instruction.MovInstruction, eax, op1L);
			newBlocks[0].AppendInstruction(CPUx86.Instruction.CmpInstruction, ecx, new ConstantOperand(I4, 64));
			newBlocks[0].AppendInstruction(IR.Instruction.BranchInstruction, IR.ConditionCode.UnsignedGreaterOrEqual, newBlocks[4].BasicBlock);
			newBlocks[0].AppendInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[1].BasicBlock);

			newBlocks[1].AppendInstruction(CPUx86.Instruction.CmpInstruction, ecx, new ConstantOperand(I4, 32));
			newBlocks[1].AppendInstruction(IR.Instruction.BranchInstruction, IR.ConditionCode.UnsignedGreaterOrEqual, newBlocks[3].BasicBlock);
			newBlocks[1].AppendInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[2].BasicBlock);

			newBlocks[2].AppendInstruction(CPUx86.Instruction.ShldInstruction, edx, eax, cl);
			newBlocks[2].AppendInstruction(CPUx86.Instruction.ShlInstruction, eax, cl);
			newBlocks[2].AppendInstruction(CPUx86.Instruction.JmpInstruction, nextBlock.BasicBlock);

			// Handle shifts of between 32 and 63 bits
			// MORE32:
			newBlocks[3].AppendInstruction(CPUx86.Instruction.MovInstruction, edx, eax);
			newBlocks[3].AppendInstruction(CPUx86.Instruction.XorInstruction, eax, eax);
			newBlocks[3].AppendInstruction(CPUx86.Instruction.AndInstruction, ecx, new ConstantOperand(I4, 0x1F));
			newBlocks[3].AppendInstruction(CPUx86.Instruction.ShlInstruction, edx, ecx);
			newBlocks[3].AppendInstruction(CPUx86.Instruction.JmpInstruction, nextBlock.BasicBlock);

			// Return double precision 0 or -1, depending on the sign of edx
			// RETZERO:
			newBlocks[4].AppendInstruction(CPUx86.Instruction.XorInstruction, eax, eax);
			newBlocks[4].AppendInstruction(CPUx86.Instruction.XorInstruction, edx, edx);
			newBlocks[4].AppendInstruction(CPUx86.Instruction.JmpInstruction, nextBlock.BasicBlock);

			// done:
			// ; remaining code from current basic block
			ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, op0H, edx);
			ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, op0L, eax);
			ctx.AppendInstruction(CPUx86.Instruction.PopInstruction, ecx);

			// Link the created Blocks together
			LinkBlocks(newBlocks, ctx, nextBlock);
		}

		/// <summary>
		/// Expands the shift right instruction for 64-bits.
		/// </summary>
		/// <param name="ctx">The context.</param>
		private void ExpandShiftRight(Context ctx)
		{
			Context[] newBlocks = CreateEmptyBlockContexts(ctx.Label, 5);
			Context nextBlock = SplitContext(ctx);

			SigType I4 = new SigType(CilElementType.I4);
			SigType I1 = new SigType(CilElementType.I1);
			SigType U1 = new SigType(CilElementType.U1);
			Operand count = ctx.Operand2;

			Operand op0H, op1H, op0L, op1L;
			SplitLongOperand(ctx.Operand1, out op0L, out op0H);
			SplitLongOperand(ctx.Operand2, out op1L, out op1H);
			RegisterOperand eax = new RegisterOperand(I4, GeneralPurposeRegister.EAX);
			RegisterOperand edx = new RegisterOperand(I4, GeneralPurposeRegister.EDX);
			RegisterOperand ecx = new RegisterOperand(U1, GeneralPurposeRegister.ECX);

			RegisterOperand cl = new RegisterOperand(new SigType(CilElementType.U1), GeneralPurposeRegister.ECX);

			// Handle shifts of 64 bits or more (if shifting 64 bits or more, the result
			// depends only on the high order bit of edx).
			newBlocks[0].AppendInstruction(CPUx86.Instruction.PushInstruction, null, ecx);
			newBlocks[0].AppendInstruction(IR.Instruction.LogicalAndInstruction, count, count, new ConstantOperand(I4, 0x3F));
			newBlocks[0].AppendInstruction(CPUx86.Instruction.MovInstruction, ecx, count);
			newBlocks[0].AppendInstruction(CPUx86.Instruction.MovInstruction, edx, op1H);
			newBlocks[0].AppendInstruction(CPUx86.Instruction.MovInstruction, eax, op1L);
			newBlocks[0].AppendInstruction(CPUx86.Instruction.PushInstruction, null, ecx);
			newBlocks[0].AppendInstruction(CPUx86.Instruction.PushInstruction, null, ecx);
			newBlocks[0].AppendInstruction(CPUx86.Instruction.PushInstruction, null, ecx);
			newBlocks[0].AppendInstruction(CPUx86.Instruction.PushInstruction, null, ecx);
			newBlocks[0].AppendInstruction(CPUx86.Instruction.PushInstruction, null, ecx);
			newBlocks[0].AppendInstruction(CPUx86.Instruction.PushInstruction, null, ecx);
			newBlocks[0].AppendInstruction(CPUx86.Instruction.PushInstruction, null, ecx);
			newBlocks[0].AppendInstruction(CPUx86.Instruction.PushInstruction, null, ecx);
			newBlocks[0].AppendInstruction(CPUx86.Instruction.PushInstruction, null, ecx);
			newBlocks[0].AppendInstruction(CPUx86.Instruction.CmpInstruction, ecx, new ConstantOperand(I4, 64));
			newBlocks[0].AppendInstruction(IR.Instruction.BranchInstruction, IR.ConditionCode.UnsignedGreaterOrEqual, newBlocks[4].BasicBlock);
			newBlocks[0].AppendInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[1].BasicBlock);

			newBlocks[1].AppendInstruction(CPUx86.Instruction.CmpInstruction, ecx, new ConstantOperand(I4, 32));
			newBlocks[1].AppendInstruction(IR.Instruction.BranchInstruction, IR.ConditionCode.UnsignedGreaterOrEqual, newBlocks[3].BasicBlock);
			newBlocks[1].AppendInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[2].BasicBlock);

			newBlocks[2].AppendInstruction(CPUx86.Instruction.ShrdInstruction, eax, edx, ecx);
			newBlocks[2].AppendInstruction(CPUx86.Instruction.SarInstruction, edx, ecx);
			newBlocks[2].AppendInstruction(CPUx86.Instruction.JmpInstruction, nextBlock.BasicBlock);

			// Handle shifts of between 32 and 63 bits
			// MORE32:
			newBlocks[3].AppendInstruction(CPUx86.Instruction.MovInstruction, eax, edx);
			newBlocks[3].AppendInstruction(CPUx86.Instruction.PushInstruction, null, ecx);
			newBlocks[3].AppendInstruction(CPUx86.Instruction.MovInstruction, ecx, new ConstantOperand(I1, (sbyte)0x1F));
			newBlocks[3].AppendInstruction(CPUx86.Instruction.SarInstruction, edx, ecx);
			newBlocks[3].AppendInstruction(CPUx86.Instruction.PopInstruction, ecx);
			newBlocks[3].AppendInstruction(CPUx86.Instruction.AndInstruction, ecx, new ConstantOperand(I4, 0x1F));
			newBlocks[3].AppendInstruction(CPUx86.Instruction.PushInstruction, null, ecx);
			newBlocks[3].AppendInstruction(CPUx86.Instruction.MovInstruction, ecx, new ConstantOperand(I1, (sbyte)0x1F));
			newBlocks[3].AppendInstruction(CPUx86.Instruction.SarInstruction, eax, ecx);
			newBlocks[3].AppendInstruction(CPUx86.Instruction.PopInstruction, ecx);
			newBlocks[3].AppendInstruction(CPUx86.Instruction.JmpInstruction, nextBlock.BasicBlock);

			// Return double precision 0 or -1, depending on the sign of edx
			// RETSIGN:
			newBlocks[4].AppendInstruction(CPUx86.Instruction.SarInstruction, edx, new ConstantOperand(I1, (sbyte)0x1F));
			newBlocks[4].AppendInstruction(CPUx86.Instruction.MovInstruction, eax, edx);
			newBlocks[4].AppendInstruction(CPUx86.Instruction.JmpInstruction, nextBlock.BasicBlock);

			// done:
			// ; remaining code from current basic block
			ctx.SetInstruction(CPUx86.Instruction.MovInstruction, op0H, edx);
			ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, op0L, eax);
			ctx.AppendInstruction(CPUx86.Instruction.PopInstruction, ecx);

			// Link the created Blocks together
			LinkBlocks(newBlocks, ctx, nextBlock);
		}

		/// <summary>
		/// Expands the neg instruction for 64-bits.
		/// </summary>
		/// <param name="ctx">The context.</param>
		private void ExpandNeg(Context ctx)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Expands the not instruction for 64-bits.
		/// </summary>
		/// <param name="ctx">The context.</param>
		private void ExpandNot(Context ctx)
		{
			Operand op0H, op1H, op0L, op1L;
			SplitLongOperand(ctx.Operand1, out op0L, out op0H);
			SplitLongOperand(ctx.Operand2, out op1L, out op1H);

			ctx.SetInstruction(IR.Instruction.LogicalNotInstruction, op0H, op1H);
			ctx.AppendInstruction(IR.Instruction.LogicalNotInstruction, op0L, op1L);
		}

		/// <summary>
		/// Expands the and instruction for 64-bits.
		/// </summary>
		/// <param name="ctx">The context.</param>
		private void ExpandAnd(Context ctx)
		{
			Operand op0H, op1H, op2H, op0L, op1L, op2L;
			SplitLongOperand(ctx.Result, out op0L, out op0H);
			SplitLongOperand(ctx.Operand1, out op1L, out op1H);
			SplitLongOperand(ctx.Operand2, out op2L, out op2H);

			ctx.SetInstruction(IR.Instruction.LogicalAndInstruction, op0H, op1H, op2H);
			ctx.AppendInstruction(IR.Instruction.LogicalAndInstruction, op0L, op1L, op2L);
		}

		/// <summary>
		/// Expands the or instruction for 64-bits.
		/// </summary>
		/// <param name="ctx">The context.</param>
		private void ExpandOr(Context ctx)
		{
			Operand op0H, op1H, op2H, op0L, op1L, op2L;
			SplitLongOperand(ctx.Result, out op0L, out op0H);
			SplitLongOperand(ctx.Operand1, out op1L, out op1H);
			SplitLongOperand(ctx.Operand2, out op2L, out op2H);

			ctx.SetInstruction(IR.Instruction.LogicalAndInstruction, op0H, op1H, op2H);
			ctx.AppendInstruction(IR.Instruction.LogicalAndInstruction, op0L, op1L, op2L);
		}

		/// <summary>
		/// Expands the neg instruction for 64-bits.
		/// </summary>
		/// <param name="ctx">The context.</param>
		private void ExpandXor(Context ctx)
		{
			Operand op0H, op1H, op2H, op0L, op1L, op2L;
			SplitLongOperand(ctx.Result, out op0L, out op0H);
			SplitLongOperand(ctx.Operand1, out op1L, out op1H);
			SplitLongOperand(ctx.Operand2, out op2L, out op2H);

			ctx.SetInstruction(IR.Instruction.LogicalXorInstruction, op0H, op1H, op2H);
			ctx.AppendInstruction(IR.Instruction.LogicalXorInstruction, op0L, op1L, op2L);
		}

		/// <summary>
		/// Expands the move instruction for 64-bits.
		/// </summary>
		/// <param name="ctx">The context.</param>
		private void ExpandMove(Context ctx)
		{
			Operand op0L, op0H, op1L, op1H;

			if (ctx.Result.StackType == StackTypeCode.Int64) {
				SplitLongOperand(ctx.Result, out op0L, out op0H);
				SplitLongOperand(ctx.Operand1, out op1L, out op1H);

				ctx.SetInstruction(CPUx86.Instruction.MovInstruction, op0L, op1L);
				ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, op0H, op1H);
			}
			else {
				SplitLongOperand(ctx.Operand1, out op1L, out op1H);
				ctx.SetInstruction(CPUx86.Instruction.MovInstruction, ctx.Result, op1L);
			}
		}

		/// <summary>
		/// Expands the unsigned move instruction for 64-bits.
		/// </summary>
		/// <param name="ctx">The context.</param>
		private void ExpandUnsignedMove(Context ctx)
		{
			MemoryOperand op0 = ctx.Result as MemoryOperand;
			Operand op1 = ctx.Operand1;
			Debug.Assert(op0 != null, @"I8 not in a memory operand!");

			SigType U4 = new SigType(CilElementType.U4);
            Operand op0L, op0H, op1L, op1H;
            SplitLongOperand(op0, out op0L, out op0H);
            SplitLongOperand(op1, out op1L, out op1H);
			RegisterOperand eax = new RegisterOperand(U4, GeneralPurposeRegister.EAX);
			RegisterOperand edx = new RegisterOperand(U4, GeneralPurposeRegister.EDX);

			switch (op1.Type.Type) {
				case CilElementType.Boolean:
					ctx.SetInstruction(IR.Instruction.ZeroExtendedMoveInstruction, op0L, op1L);
					ctx.AppendInstruction(IR.Instruction.LogicalXorInstruction, op0H, op0H, op0H);
					break;

				case CilElementType.U1:
					ctx.SetInstruction(IR.Instruction.ZeroExtendedMoveInstruction, eax, op1L);
					ctx.AppendInstruction(CPUx86.Instruction.CdqInstruction);
					ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, op0L, eax);
					ctx.AppendInstruction(IR.Instruction.LogicalXorInstruction, op0H, op0H, op0H);
					break;

				case CilElementType.U2: goto case CilElementType.U1;

				case CilElementType.I4:
					ctx.SetInstruction(IR.Instruction.ZeroExtendedMoveInstruction, eax, op1L);
					ctx.AppendInstruction(CPUx86.Instruction.XorInstruction, edx, edx);
					ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, op0L, eax);
					ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, op0H, edx);
					break;
				case CilElementType.U4:
					ctx.SetInstruction(IR.Instruction.ZeroExtendedMoveInstruction, eax, op1L);
					ctx.AppendInstruction(CPUx86.Instruction.XorInstruction, edx, edx);
					ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, op0L, eax);
					ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, op0H, edx);
					break;

				case CilElementType.U8:
					ctx.SetInstruction(IR.Instruction.ZeroExtendedMoveInstruction, op0L, op1L);
                    ctx.SetInstruction(IR.Instruction.ZeroExtendedMoveInstruction, op0H, op1H);
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
		/// <param name="ctx">The context.</param>
		private void ExpandSignedMove(Context ctx)
		{
			MemoryOperand op0 = ctx.Operand1 as MemoryOperand;
			Operand op1 = ctx.Operand2;
			Debug.Assert(op0 != null, @"I8 not in a memory operand!");
			SigType I4 = new SigType(CilElementType.I4);
			MemoryOperand op0L = new MemoryOperand(I4, op0.Base, op0.Offset);
			MemoryOperand op0H = new MemoryOperand(I4, op0.Base, new IntPtr(op0.Offset.ToInt64() + 4));
			RegisterOperand eax = new RegisterOperand(I4, GeneralPurposeRegister.EAX);
			RegisterOperand edx = new RegisterOperand(I4, GeneralPurposeRegister.EDX);

			switch (op1.Type.Type) {
				case CilElementType.Boolean:
					ctx.SetInstruction(IR.Instruction.ZeroExtendedMoveInstruction, op0L, op1);
					ctx.AppendInstruction(IR.Instruction.LogicalXorInstruction, op0H, op0H, op0H);
					break;

				case CilElementType.I1:
					ctx.SetInstruction(IR.Instruction.SignExtendedMoveInstruction, eax, op1);
					ctx.AppendInstruction(CPUx86.Instruction.CdqInstruction);
					ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, op0L, eax);
					ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, op0H, edx);
					break;

				case CilElementType.I2: goto case CilElementType.I1;

				case CilElementType.I4:
					ctx.SetInstruction(CPUx86.Instruction.MovInstruction, eax, op1);
					ctx.AppendInstruction(CPUx86.Instruction.CdqInstruction);
					ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, op0L, eax);
					ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, op0H, edx);
					break;

				case CilElementType.I8:
					ctx.SetInstruction(CPUx86.Instruction.MovInstruction, op0, op1);
					break;

				case CilElementType.U1:
					ctx.SetInstruction(IR.Instruction.ZeroExtendedMoveInstruction, eax, op1);
					ctx.AppendInstruction(CPUx86.Instruction.CdqInstruction);
					ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, op0L, eax);
					ctx.AppendInstruction(IR.Instruction.LogicalXorInstruction, op0H, op0H, op0H);
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
		/// <param name="ctx">The context.</param>
		private void ExpandLoad(Context ctx)
		{
			MemoryOperand op0 = ctx.Operand1 as MemoryOperand;
			MemoryOperand op1 = ctx.Operand2 as MemoryOperand;
			Debug.Assert(op0 != null && op1 != null, @"Operands to I8 LoadInstruction are not MemoryOperand.");

			SigType I4 = new SigType(CilElementType.I4);
			Operand op0L, op0H;
			SplitLongOperand(op0, out op0L, out op0H);
			RegisterOperand eax = new RegisterOperand(I4, GeneralPurposeRegister.EAX);
			RegisterOperand edx = new RegisterOperand(I4, GeneralPurposeRegister.EDX);

			ctx.SetInstruction(CPUx86.Instruction.MovInstruction, eax, op1);
			ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, edx, new MemoryOperand(ctx.Result.Type, GeneralPurposeRegister.EAX, IntPtr.Zero));
			ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, op0L, edx);
			ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, edx, new MemoryOperand(ctx.Result.Type, GeneralPurposeRegister.EAX, new IntPtr(4)));
			ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, op0H, edx);
		}

		/// <summary>
		/// Expands the store instruction for 64-bits.
		/// </summary>
		/// <param name="ctx">The context.</param>
		private void ExpandStore(Context ctx)
		{
			MemoryOperand op0 = ctx.Operand1 as MemoryOperand;
			MemoryOperand op1 = ctx.Operand2 as MemoryOperand;
			Debug.Assert(op0 != null && op1 != null, @"Operands to I8 LoadInstruction are not MemoryOperand.");

			SigType I4 = new SigType(CilElementType.I4);
			SigType U4 = new SigType(CilElementType.U4);
			Operand op1L, op1H;
			SplitLongOperand(op1, out op1L, out op1H);
			RegisterOperand eax = new RegisterOperand(I4, GeneralPurposeRegister.EAX);
			RegisterOperand edx = new RegisterOperand(I4, GeneralPurposeRegister.EDX);

			ctx.SetInstruction(CPUx86.Instruction.MovInstruction, edx, op0);
			ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, eax, op1L);
			ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, new MemoryOperand(U4, GeneralPurposeRegister.EDX, IntPtr.Zero), eax);
			ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, eax, op1H);
			ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, new MemoryOperand(I4, GeneralPurposeRegister.EDX, new IntPtr(4)), eax);
		}

		/// <summary>
		/// Expands the pop instruction for 64-bits.
		/// </summary>
		/// <param name="ctx">The context.</param>
		private void ExpandPop(Context ctx)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Expands the push instruction for 64-bits.
		/// </summary>
		/// <param name="ctx">The context.</param>
		private void ExpandPush(Context ctx)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Expands the unary branch instruction for 64-bits.
		/// </summary>
		/// <param name="ctx">The context.</param>
		private void ExpandUnaryBranch(Context ctx)
		{
			Debug.Assert(ctx.Branch.Targets.Length == 2);

			int[] targets = ctx.Branch.Targets;

			Context[] newBlocks = CreateEmptyBlockContexts(ctx.Label, 2);
			Context nextBlock = SplitContext(ctx);

			Operand op1H, op1L, op2H, op2L;
			SplitLongOperand(ctx.Operand1, out op1L, out op1H);
			SplitLongOperand(ctx.Operand2, out op2L, out op2H);
			IR.ConditionCode code;

			switch (((ctx.Instruction) as CIL.ICILInstruction).OpCode) {
				// Signed
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

				default:
					throw new NotImplementedException();
			}

			IR.ConditionCode conditionHigh = GetHighCondition(code);

			// Compare high dwords
			newBlocks[0].AppendInstruction(CPUx86.Instruction.CmpInstruction, op1H, op2H);
			// Branch if check already gave results
			newBlocks[0].AppendInstruction(IR.Instruction.BranchInstruction, IR.ConditionCode.Equal, nextBlock.BasicBlock);
			newBlocks[0].AppendInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[1].BasicBlock);

			newBlocks[1].AppendInstruction(IR.Instruction.BranchInstruction, code);
			newBlocks[1].Branch.Targets[0] = targets[0];
			newBlocks[1].AppendInstruction(CPUx86.Instruction.JmpInstruction);
			newBlocks[1].Branch.Targets[0] = targets[1];

			LinkBlocks(newBlocks, ctx, nextBlock);

			// Compare low dwords
			ctx.SetInstruction(CPUx86.Instruction.CmpInstruction, op1L, op2L);
			// Set the unsigned result...
			ctx.AppendInstruction(IR.Instruction.BranchInstruction, code);
			ctx.SetBranch(targets[0]);
			ctx.AppendInstruction(CPUx86.Instruction.JmpInstruction);
			ctx.SetBranch(targets[1]);
		}

		/// <summary>
		/// Expands the binary branch instruction for 64-bits.
		/// </summary>
		/// <param name="ctx">The context.</param>
		private void ExpandBinaryBranch(Context ctx)
		{
			Debug.Assert(ctx.Branch.Targets.Length == 2);

			int[] targets = ctx.Branch.Targets;

			Context[] newBlocks = CreateEmptyBlockContexts(ctx.Label, 2);
			Context nextBlock = SplitContext(ctx);

			Operand op1H, op1L, op2H, op2L;
			SplitLongOperand(ctx.Operand1, out op1L, out op1H);
			SplitLongOperand(ctx.Operand2, out op2L, out op2H);
			IR.ConditionCode code;

			switch (((ctx.Instruction) as CIL.ICILInstruction).OpCode) {
				// Signed
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

				default:
					throw new NotImplementedException();
			}

			IR.ConditionCode conditionHigh = GetHighCondition(code);

			// Compare high dwords
			newBlocks[0].AppendInstruction(CPUx86.Instruction.CmpInstruction, op1H, op2H);
			newBlocks[0].AppendInstruction(IR.Instruction.BranchInstruction, IR.ConditionCode.Equal, nextBlock.BasicBlock);
			newBlocks[0].AppendInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[1].BasicBlock);

			// Branch if check already gave results
			newBlocks[1].AppendInstruction(IR.Instruction.BranchInstruction, code);
			newBlocks[1].SetBranch(targets[0]);
			newBlocks[1].AppendInstruction(CPUx86.Instruction.JmpInstruction);
			newBlocks[1].SetBranch(targets[1]);

			LinkBlocks(newBlocks, ctx, nextBlock);

			// Compare low dwords
			ctx.SetInstruction(CPUx86.Instruction.CmpInstruction, op1L, op2L);
			// Set the unsigned result...
			ctx.AppendInstruction(IR.Instruction.BranchInstruction, code);
			ctx.SetBranch(targets[0]);
			ctx.AppendInstruction(CPUx86.Instruction.JmpInstruction);
			ctx.SetBranch(targets[1]);
		}

		/// <summary>
		/// Gets the high condition.
		/// </summary>
		/// <param name="code">The code.</param>
		/// <returns></returns>
		private static IR.ConditionCode GetHighCondition(IR.ConditionCode code)
		{
			switch (code) {
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
		/// <param name="ctx">The context.</param>
		private void ExpandComparison(Context ctx)
		{
			Operand op0 = ctx.Result;
			Operand op1 = ctx.Operand1;
			Operand op2 = ctx.Operand2;

			Debug.Assert(op1 != null && op2 != null, @"IntegerCompareInstruction operand not memory!");
			Debug.Assert(op0 is MemoryOperand || op0 is RegisterOperand, @"IntegerCompareInstruction result not memory and not register!");

			SigType I4 = new SigType(CilElementType.I4), U4 = new SigType(CilElementType.U4);

			Operand op1L, op1H, op2L, op2H;
			SplitLongOperand(op1, out op1L, out op1H);
			SplitLongOperand(op2, out op2L, out op2H);

			Context[] newBlocks = CreateEmptyBlockContexts(ctx.Label, 4);
			IR.ConditionCode conditionCode = ctx.ConditionCode;
			Context nextBlock = SplitContext(ctx);
			LinkBlocks(newBlocks, ctx, nextBlock);

			// Compare high dwords
			ctx.SetInstruction(CPUx86.Instruction.CmpInstruction, op1H, op2H);
			ctx.AppendInstruction(IR.Instruction.BranchInstruction, IR.ConditionCode.Equal, newBlocks[1].BasicBlock);
			ctx.AppendInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[0].BasicBlock);

			// Branch if check already gave results
			newBlocks[0].SetInstruction(IR.Instruction.BranchInstruction, conditionCode, newBlocks[2].BasicBlock);
			newBlocks[0].AppendInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[3].BasicBlock);

			// Compare low dwords
			newBlocks[1].SetInstruction(CPUx86.Instruction.CmpInstruction, op1L, op2L);
			// Set the unsigned result...
			newBlocks[1].AppendInstruction(IR.Instruction.BranchInstruction, GetUnsignedConditionCode(conditionCode), newBlocks[2].BasicBlock);
			newBlocks[1].AppendInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[3].BasicBlock);

			// Success
			newBlocks[2].SetInstruction(CPUx86.Instruction.MovsxInstruction, op0, new ConstantOperand(I4, 1));
			newBlocks[2].AppendInstruction(CPUx86.Instruction.JmpInstruction, nextBlock.BasicBlock);

			// Failed
            newBlocks[3].SetInstruction(CPUx86.Instruction.MovsxInstruction, op0, new ConstantOperand(I4, 0));
			newBlocks[3].AppendInstruction(CPUx86.Instruction.JmpInstruction, nextBlock.BasicBlock);
		}

		#endregion // Utility Methods

		#region IIRVisitor

		/// <summary>
		/// Arithmetics the shift right instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.ArithmeticShiftRightInstruction(Context ctx)
		{
			if (ctx.Operand1.StackType == StackTypeCode.Int64)
				ExpandArithmeticShiftRight(ctx);
		}

		/// <summary>
		/// Integers the compare instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.IntegerCompareInstruction(Context ctx)
		{
			if (ctx.Operand1.StackType == StackTypeCode.Int64)
				ExpandComparison(ctx);
		}

		/// <summary>
		/// Loads the instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.LoadInstruction(Context ctx)
		{
			if (ctx.Operand1.StackType == StackTypeCode.Int64)
				ExpandLoad(ctx);
		}

		/// <summary>
		/// Logicals the and instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.LogicalAndInstruction(Context ctx)
		{
			if (ctx.Operand1.StackType == StackTypeCode.Int64)
				ExpandAnd(ctx);
		}

		/// <summary>
		/// Logicals the or instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.LogicalOrInstruction(Context ctx)
		{
			if (ctx.Operand1.StackType == StackTypeCode.Int64)
				ExpandOr(ctx);
		}

		/// <summary>
		/// Logicals the xor instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.LogicalXorInstruction(Context ctx)
		{
			if (ctx.Operand1.StackType == StackTypeCode.Int64)
				ExpandXor(ctx);
		}

		/// <summary>
		/// Logicals the not instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.LogicalNotInstruction(Context ctx)
		{
			if (ctx.Operand1.StackType == StackTypeCode.Int64)
				ExpandNot(ctx);
		}

		/// <summary>
		/// Moves the instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.MoveInstruction(Context ctx)
		{
			if (ctx.Operand1.StackType == StackTypeCode.Int64)
				ExpandMove(ctx);
		}

		/// <summary>
		/// Pops the instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.PopInstruction(Context ctx)
		{
			if (ctx.Operand1.StackType == StackTypeCode.Int64)
				ExpandPop(ctx);
		}

		/// <summary>
		/// Pushes the instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.PushInstruction(Context ctx)
		{
			if (ctx.Operand1.StackType == StackTypeCode.Int64)
				ExpandPush(ctx);
		}

		/// <summary>
		/// Shifts the left instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.ShiftLeftInstruction(Context ctx)
		{
			if (ctx.Operand1.StackType == StackTypeCode.Int64)
				ExpandShiftLeft(ctx);
		}

		/// <summary>
		/// Shifts the right instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.ShiftRightInstruction(Context ctx)
		{
			if (ctx.Operand1.StackType == StackTypeCode.Int64)
				ExpandShiftRight(ctx);
		}

		/// <summary>
		/// Signs the extended move instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.SignExtendedMoveInstruction(Context ctx)
		{
			if (ctx.Operand1.StackType == StackTypeCode.Int64)
				ExpandSignedMove(ctx);
		}

		/// <summary>
		/// Stores the instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.StoreInstruction(Context ctx)
		{
			if (ctx.Operand1.StackType == StackTypeCode.Int64)
				ExpandStore(ctx);
		}

		/// <summary>
		/// Us the div instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.UDivInstruction(Context ctx)
		{
			if (ctx.Operand1.StackType == StackTypeCode.Int64)
				ExpandUDiv(ctx);
		}

		/// <summary>
		/// Us the rem instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.URemInstruction(Context ctx)
		{
			if (ctx.Operand1.StackType == StackTypeCode.Int64)
				ExpandURem(ctx);
		}

		/// <summary>
		/// Zeroes the extended move instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.ZeroExtendedMoveInstruction(Context ctx)
		{
			if (ctx.Result.StackType == StackTypeCode.Int64)
				ExpandUnsignedMove(ctx);
		}

		#endregion // IIRVisitor

		#region IIRVisitor - Unused

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.AddressOfInstruction"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.AddressOfInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.BranchInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.BranchInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.CallInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.CallInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.EpilogueInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.EpilogueInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.FloatingPointCompareInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.FloatingPointCompareInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.FloatingPointToIntegerConversionInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.FloatingPointToIntegerConversionInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.IntegerToFloatingPointConversionInstruction"/> instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.IntegerToFloatingPointConversionInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.JmpInstruction"/> instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.JmpInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.LiteralInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.LiteralInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.PhiInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.PhiInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.PrologueInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.PrologueInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.ReturnInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.ReturnInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.NopInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.NopInstruction(Context context) { }

		#endregion // IIRVisitor - Unused

		#region ICILVisitor

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.UnaryBranch"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.UnaryBranch(Context ctx)
		{
			if (ctx.Operand1.StackType == StackTypeCode.Int64)
				ExpandUnaryBranch(ctx);
		}

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.BinaryBranch"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.BinaryBranch(Context ctx)
		{
			if (ctx.Operand1.StackType == StackTypeCode.Int64)
				ExpandBinaryBranch(ctx);
		}

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Neg"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Neg(Context ctx)
		{
			if (ctx.Operand1.StackType == StackTypeCode.Int64)
				ExpandNeg(ctx);
		}

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Not"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Not(Context ctx)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.UnaryArithmetic"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.UnaryArithmetic(Context ctx)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.ArithmeticOverflow"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.ArithmeticOverflow(Context ctx)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Add"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Add(Context ctx)
		{
			if (ctx.Operand1.StackType == StackTypeCode.Int64)
				ExpandAdd(ctx);
		}

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Sub"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Sub(Context ctx)
		{
			if (ctx.Operand1.StackType == StackTypeCode.Int64)
				ExpandSub(ctx);
		}

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Mul"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Mul(Context ctx)
		{
			if (ctx.Operand1.StackType == StackTypeCode.Int64)
				ExpandMul(ctx);
		}

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Div"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Div(Context ctx)
		{
			if (ctx.Operand1.StackType == StackTypeCode.Int64)
				ExpandDiv(ctx);
		}

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Rem"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Rem(Context ctx)
		{
			if (ctx.Operand1.StackType == StackTypeCode.Int64)
				ExpandRem(ctx);
		}

		#endregion // Members

		#region ICILVisitor - Unused

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Nop"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Nop(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Break"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Break(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Ldarg"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Ldarg(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Ldarga"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Ldarga(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Ldloc"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Ldloc(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Ldloca"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Ldloca(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Ldc"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Ldc(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Ldobj"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Ldobj(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Ldstr"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Ldstr(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Ldfld"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Ldfld(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Ldflda"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Ldflda(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Ldsfld"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Ldsfld(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Ldsflda"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Ldsflda(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Ldftn"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Ldftn(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Ldvirtftn"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Ldvirtftn(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Ldtoken"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Ldtoken(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Stloc"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Stloc(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Starg"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Starg(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Stobj"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Stobj(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Stfld"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Stfld(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Stsfld"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Stsfld(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Dup"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Dup(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Pop"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Pop(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Jmp"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Jmp(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Call"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Call(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Calli"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Calli(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Ret"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Ret(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Branch"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Branch(Context ctx) { }


		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Switch"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Switch(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.BinaryLogic"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.BinaryLogic(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Shift"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Shift(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Conversion"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Conversion(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Callvirt"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Callvirt(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Cpobj"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Cpobj(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Newobj"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Newobj(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Castclass"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Castclass(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Isinst"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Isinst(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Unbox"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Unbox(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Throw"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Throw(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Box"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Box(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Newarr"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Newarr(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Ldlen"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Ldlen(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Ldelema"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Ldelema(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Ldelem"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Ldelem(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Stelem"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Stelem(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.UnboxAny"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.UnboxAny(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Refanyval"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Refanyval(Context ctx) { }

		/// <summary>
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Mkrefany(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Endfinally"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Endfinally(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Leave"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Leave(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Arglist"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Arglist(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.BinaryComparison"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.BinaryComparison(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Localalloc"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Localalloc(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Endfilter"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Endfilter(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.InitObj"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.InitObj(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Cpblk"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Cpblk(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Initblk"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Initblk(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Prefix"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Prefix(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Rethrow"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Rethrow(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Sizeof"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Sizeof(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Refanytype"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Refanytype(Context ctx) { }

		#endregion // ICILVisitor - Unused

	}
}
