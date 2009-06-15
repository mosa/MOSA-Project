/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 *  Simon Wollwage (<mailto:kintaro@think-in-co.de>)
 *  Scott Balmos (<mailto:sbalmos@fastmail.fm>)
 */

using System;
using System.Collections.Generic;
using System.Text;

using Mosa.Runtime.CompilerFramework;
using IL = Mosa.Runtime.CompilerFramework.IL;
using IR = Mosa.Runtime.CompilerFramework.IR;
using Mosa.Platforms.x86.Instructions;
using Mosa.Platforms.x86.Instructions.Intrinsics;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Metadata;
using System.Diagnostics;

namespace Mosa.Platforms.x86
{
	/// <summary>
	/// Transforms 64-bit arithmetic to 32-bit operations.
	/// </summary>
	/// <remarks>
	/// This stage translates all 64-bit operations to appropriate 32-bit operations on
	/// architectures without appropriate 64-bit integral operations.
	/// </remarks>
	public sealed class LongOperandTransformationStage :
		CodeTransformationStage,
		IR.IIRVisitor<CodeTransformationStage.Context>,
		IL.IILVisitor<CodeTransformationStage.Context>,
		IX86InstructionVisitor<CodeTransformationStage.Context>
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LongOperandTransformationStage"/> class.
		/// </summary>
		public LongOperandTransformationStage()
		{
		}

		#endregion // Construction

		#region IMethodCompilerStage Members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		public sealed override string Name
		{
			get { return @"LongArithmeticTransformationStage"; }
		}

		#endregion // IMethodCompilerStage Members

		#region Utility Methods

		/// <summary>
		/// Splits the long operand into its high and low parts.
		/// </summary>
		/// <param name="op">The operand to split.</param>
		/// <param name="opL">The low operand.</param>
		/// <param name="opH">The high operand.</param>
		/// <exception cref="T:System.ArgumentException"><paramref name="op"/> is not a ConstantOperand and not a MemoryOperand.</exception>
		public static void SplitLongOperand(Operand op, out Operand opL, out Operand opH)
		{
			Debug.Assert(op is MemoryOperand || op is ConstantOperand, @"Long operand not memory or constant.");

			SigType I4 = new SigType(CilElementType.I4);
			SigType U4 = new SigType(CilElementType.U4);

			// Is it a constant operand?
			ConstantOperand cop = op as ConstantOperand;
			if (cop != null) {
				long value = (long)cop.Value;
				opL = new ConstantOperand(I4, (int)(value & 0xFFFFFFFF));
				opH = new ConstantOperand(I4, (int)((value >> 32) & 0xFFFFFFFF));
			}
			else {
				// No, could be a member or a plain memory operand
				MemberOperand memberOp = op as MemberOperand;
				if (memberOp != null) {
					// We need to keep the member reference, otherwise the linker can't fixup
					// the member address.
					opL = new MemberOperand(memberOp.Member, I4, memberOp.Offset);
					opH = new MemberOperand(memberOp.Member, I4, new IntPtr(memberOp.Offset.ToInt64() + 4));
				}
				else {
					// Plain memory, we can handle it here
					MemoryOperand mop = (MemoryOperand)op;
					opL = new MemoryOperand(I4, mop.Base, mop.Offset);
					opH = new MemoryOperand(I4, mop.Base, new IntPtr(mop.Offset.ToInt64() + 4));
				}
			}
		}

		/// <summary>
		/// Expands the add instruction for 64-bit operands.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="instruction">The instruction.</param>
		private void ExpandAdd(Context ctx, IL.AddInstruction instruction)
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
			SplitLongOperand(instruction.First, out op1L, out op1H);
			SplitLongOperand(instruction.Second, out op2L, out op2H);
			SplitLongOperand(instruction.Results[0], out resL, out resH);

			Instruction[] result = new Instruction[] {
                new Instructions.MoveInstruction(eaxL, op1L),
                new Instructions.AddInstruction(eaxL, op2L),
                new Instructions.MoveInstruction(resL, eaxL),
                new Instructions.MoveInstruction(eaxH, op1H),
                new Instructions.AdcInstruction(eaxH, op2H),
                new Instructions.MoveInstruction(resH, eaxH),
            };
			Replace(ctx, result);
		}

		/// <summary>
		/// Expands the sub instruction for 64-bit operands.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="instruction">The instruction.</param>
		private void ExpandSub(Context ctx, IL.SubInstruction instruction)
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
			SplitLongOperand(instruction.First, out op1L, out op1H);
			SplitLongOperand(instruction.Second, out op2L, out op2H);
			SplitLongOperand(instruction.Results[0], out resL, out resH);

			Instruction[] result = new Instruction[] {
                new Instructions.MoveInstruction(eaxL, op1L),
                new Instructions.SubInstruction(eaxL, op2L),
                new Instructions.MoveInstruction(resL, eaxL),
                new Instructions.MoveInstruction(eaxH, op1H),
                new Instructions.SbbInstruction(eaxH, op2H),
                new Instructions.MoveInstruction(resH, eaxH),
            };
			Replace(ctx, result);
		}

		/// <summary>
		/// Expands the mul instruction for 64-bit operands.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="instruction">The instruction.</param>
		private void ExpandMul(Context ctx, IL.MulInstruction instruction)
		{
			MemoryOperand op0 = instruction.Results[0] as MemoryOperand;
			MemoryOperand op1 = instruction.Operands[0] as MemoryOperand;
			MemoryOperand op2 = instruction.Operands[1] as MemoryOperand;
			Debug.Assert(op0 != null && op1 != null && op2 != null, @"Operands to 64 bit multiplication are not MemoryOperands.");

			SigType I4 = new SigType(CilElementType.I4);
			SigType U4 = new SigType(CilElementType.U4);
			Operand op0H, op1H, op2H, op0L, op1L, op2L;
			SplitLongOperand(instruction.Results[0], out op0L, out op0H);
			SplitLongOperand(instruction.First, out op1L, out op1H);
			SplitLongOperand(instruction.Second, out op2L, out op2H);

			// op0 = EDX:EAX, op1 = A, op2 = B
			RegisterOperand edx = new RegisterOperand(I4, GeneralPurposeRegister.EDX);
			Replace(ctx, new Instruction[] {
                new IL.MulInstruction(IL.OpCode.Mul, op0H, op1H, op2L),
                new IL.MulInstruction(IL.OpCode.Mul, op0L, op1L, op2H),
                new IL.AddInstruction(IL.OpCode.Add, op0H, op0H, op0L),
                new IL.MulInstruction(IL.OpCode.Mul, op0L, op1L, op2L),
                new AddInstruction(op0H, edx)
            });
		}

		/// <summary>
		/// Expands the div.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="instruction">The instruction.</param>
		private void ExpandDiv(Context ctx, IL.DivInstruction instruction)
		{
			BasicBlock[] blocks = CreateEmptyBlocks(7);
			BasicBlock nextBlock = SplitBlock(ctx, instruction, blocks[0]);

			SigType I4 = new SigType(CilElementType.I4);
			SigType U4 = new SigType(CilElementType.U4);
			SigType I1 = new SigType(CilElementType.I1);
			SigType U1 = new SigType(CilElementType.U1);

			Operand op0H, op1H, op2H, op0L, op1L, op2L;
			SplitLongOperand(instruction.Results[0], out op0L, out op0H);
			SplitLongOperand(instruction.First, out op1L, out op1H);
			SplitLongOperand(instruction.Second, out op2L, out op2H);
			RegisterOperand eax = new RegisterOperand(I4, GeneralPurposeRegister.EAX);
			RegisterOperand ebx = new RegisterOperand(I4, GeneralPurposeRegister.EBX);
			RegisterOperand edx = new RegisterOperand(I4, GeneralPurposeRegister.EDX);
			RegisterOperand ecx = new RegisterOperand(I4, GeneralPurposeRegister.ECX);
			RegisterOperand edi = new RegisterOperand(I4, GeneralPurposeRegister.EDI);
			RegisterOperand esi = new RegisterOperand(I4, GeneralPurposeRegister.ESI);

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
			Replace(ctx, new Instruction[] {
                new IR.PushInstruction(edi),
                new IR.PushInstruction(esi),
                new IR.PushInstruction(ebx),
                new Instructions.LogicalXorInstruction(edi, edi),
                new Instructions.MoveInstruction(eax, op1H),
                new Instructions.LogicalOrInstruction(eax, eax),
                new IR.BranchInstruction(IR.ConditionCode.GreaterOrEqual, blocks[0].Label),
                new Instructions.IncInstruction(edi),
                new Instructions.MoveInstruction(edx, op1L),
                new Instructions.NegInstruction(eax),
                new Instructions.NegInstruction(edx),
                new Instructions.SbbInstruction(eax, new ConstantOperand(I4, 0)),
                new Instructions.MoveInstruction(op1H, eax),
                new Instructions.MoveInstruction(op1L, edx),
            });

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
			blocks[0].Instructions.AddRange(new Instruction[] {
                new Instructions.MoveInstruction(eax, op2H),
                new Instructions.LogicalOrInstruction(eax, eax),
                new IR.BranchInstruction(IR.ConditionCode.GreaterOrEqual, blocks[1].Label),
                new Instructions.IncInstruction(edi),
                new Instructions.MoveInstruction(edx, op2L),
                new Instructions.NegInstruction(eax),
                new Instructions.NegInstruction(edx),
                new Instructions.SbbInstruction(eax, new ConstantOperand(I4, 0)),
                new Instructions.MoveInstruction(op2H, eax),
                new Instructions.MoveInstruction(op2L, edx),
				new IR.JmpInstruction(blocks[1].Label),
            });

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
			blocks[1].Instructions.AddRange(new Instruction[] {
                new Instructions.LogicalOrInstruction(eax, eax),
                new IR.BranchInstruction(IR.ConditionCode.NotEqual, blocks[2].Label),
                new Instructions.MoveInstruction(ecx, op2L),
                new Instructions.MoveInstruction(eax, op1H),
                new Instructions.LogicalXorInstruction(edx, edx),
                new Instructions.UDivInstruction(eax, ecx),
                new Instructions.MoveInstruction(ebx, eax),
                new Instructions.MoveInstruction(eax, op1L),
                new Instructions.UDivInstruction(eax, ecx),
                new Instructions.MoveInstruction(edx, ebx),
                new IR.JmpInstruction(blocks[6].Label)
            });
			;
			// Here we do it the hard way.  Remember, eax contains the high word of DVSR
			//
			// L3:
			//        mov     ebx,eax         ; ebx:ecx <- divisor
			//        mov     ecx,LOWORD(DVSR)
			//        mov     edx,HIWORD(DVND) ; edx:eax <- dividend
			//        mov     eax,LOWORD(DVND)
			blocks[2].Instructions.AddRange(new Instruction[] {
                new Instructions.MoveInstruction(ebx, eax),
                new Instructions.MoveInstruction(ecx, op2L),
                new Instructions.MoveInstruction(edx, op1H),
                new Instructions.MoveInstruction(eax, op1L),
            });

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
			blocks[3].Instructions.AddRange(new Instruction[] {
                new Instructions.ShrInstruction(ebx, new ConstantOperand(U1, 1)),
                new Instructions.RcrInstruction(ecx, new ConstantOperand(U1, 1)), // RCR
                new Instructions.ShrInstruction(edx, new ConstantOperand(U1, 1)),
                new Instructions.RcrInstruction(eax, new ConstantOperand(U1, 1)),
                new Instructions.LogicalOrInstruction(ebx, ebx),
                new IR.BranchInstruction(IR.ConditionCode.NotEqual, blocks[3].Label),
                new Instructions.UDivInstruction(eax, ecx),
                new Instructions.MoveInstruction(esi, eax),
                new x86.Instructions.MulInstruction(eax, op2H),
                //new IL.MulInstruction(IL.OpCode.Mul, eax, eax, op2H),
                new Instructions.MoveInstruction(ecx, eax),
                new Instructions.MoveInstruction(eax, op2L),
                new x86.Instructions.MulInstruction(eax, esi),
                //new IL.MulInstruction(IL.OpCode.Mul, eax, eax, esi),
                new Instructions.AddInstruction(edx, ecx),
                new IR.BranchInstruction(IR.ConditionCode.UnsignedLessThan, blocks[4].Label),
                new Instructions.CmpInstruction(edx, op1H),
                new IR.BranchInstruction(IR.ConditionCode.UnsignedGreaterThan, blocks[4].Label),
                new IR.BranchInstruction(IR.ConditionCode.UnsignedLessThan, blocks[5].Label),
                new Instructions.CmpInstruction(eax, op1L),
                new IR.BranchInstruction(IR.ConditionCode.UnsignedLessOrEqual, blocks[5].Label),
            });

			// L6:
			blocks[4].Instructions.AddRange(new Instruction[] {
                new Instructions.DecInstruction(esi),
				new IR.JmpInstruction(blocks[5].Label),
            });

			// L7:
			blocks[5].Instructions.AddRange(new Instruction[] {
                new Instructions.LogicalXorInstruction(edx, edx),
                new Instructions.MoveInstruction(eax, esi),
				new IR.JmpInstruction(blocks[6].Label),
            });

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
			blocks[6].Instructions.InsertRange(0, new Instruction[] {
                new Instructions.DecInstruction(edi),
                new IR.BranchInstruction(IR.ConditionCode.NotEqual, nextBlock.Label),
                new Instructions.NegInstruction(edx),
                new Instructions.NegInstruction(eax),
                new Instructions.SbbInstruction(edx, new ConstantOperand(I4, 0)),
				new IR.JmpInstruction(nextBlock.Label),
            });

			nextBlock.Instructions.InsertRange(0, new Instruction[] {
                new MoveInstruction(op0L, eax),
                new MoveInstruction(op0H, edx),
                new IR.PopInstruction(ebx),
                new IR.PopInstruction(esi),
                new IR.PopInstruction(edi),
            });

			// Link the created blocks together
			LinkBlocks(ctx.Block, blocks[0]);
			LinkBlocks(blocks[0], blocks[1]);
			LinkBlocks(blocks[1], blocks[6]);
			LinkBlocks(blocks[1], blocks[2]);
			LinkBlocks(blocks[2], blocks[3]);
			LinkBlocks(blocks[3], blocks[3]);
			LinkBlocks(blocks[3], blocks[4]);
			LinkBlocks(blocks[3], blocks[5]);
			LinkBlocks(blocks[4], blocks[5]);
			LinkBlocks(blocks[5], blocks[6]);
			LinkBlocks(blocks[6], nextBlock);
		}

		/// <summary>
		/// Expands the rem.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="instruction">The instruction.</param>
		private void ExpandRem(Context ctx, IL.RemInstruction instruction)
		{
			BasicBlock[] blocks = CreateEmptyBlocks(7);
			BasicBlock nextBlock = SplitBlock(ctx, instruction, blocks[0]);

			SigType I4 = new SigType(CilElementType.I4);
			SigType U4 = new SigType(CilElementType.U4);
			SigType U1 = new SigType(CilElementType.U1);

			Operand op0H, op1H, op2H, op0L, op1L, op2L;
			SplitLongOperand(instruction.Results[0], out op0L, out op0H);
			SplitLongOperand(instruction.First, out op1L, out op1H);
			SplitLongOperand(instruction.Second, out op2L, out op2H);
			RegisterOperand eax = new RegisterOperand(I4, GeneralPurposeRegister.EAX);
			RegisterOperand ebx = new RegisterOperand(I4, GeneralPurposeRegister.EBX);
			RegisterOperand edx = new RegisterOperand(U4, GeneralPurposeRegister.EDX);
			RegisterOperand ecx = new RegisterOperand(U4, GeneralPurposeRegister.ECX);
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
			Replace(ctx, new Instruction[] {
                new IR.PushInstruction(edi),
                new IR.PushInstruction(esi),
                new IR.PushInstruction(ebx),
                new Instructions.LogicalXorInstruction(edi, edi),
                new Instructions.MoveInstruction(eax, op1H),
                new Instructions.LogicalOrInstruction(eax, eax),
                new IR.BranchInstruction(IR.ConditionCode.GreaterOrEqual, blocks[0].Label),
                new Instructions.IncInstruction(edi),
                new Instructions.MoveInstruction(edx, op1L),
                new Instructions.NegInstruction(eax),
                new Instructions.NegInstruction(edx),
                new Instructions.SbbInstruction(eax, new ConstantOperand(I4, 0)),
                new Instructions.MoveInstruction(op1H, eax),
                new Instructions.MoveInstruction(op1L, edx),								
            });

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
			blocks[0].Instructions.AddRange(new Instruction[] {
                new Instructions.MoveInstruction(eax, op2H),
                new Instructions.LogicalOrInstruction(eax, eax),
                new IR.BranchInstruction(IR.ConditionCode.GreaterOrEqual, blocks[1].Label),
                new Instructions.MoveInstruction(edx, op2L),
                new Instructions.NegInstruction(eax),
                new Instructions.NegInstruction(edx),
                new Instructions.SbbInstruction(eax, new ConstantOperand(I4, 0)),
                new Instructions.MoveInstruction(op2H, eax),
                new Instructions.MoveInstruction(op2L, edx),
				new IR.JmpInstruction(blocks[1].Label),
			});

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
			blocks[1].Instructions.AddRange(new Instruction[] {
                new Instructions.LogicalOrInstruction(eax, eax),
                new IR.BranchInstruction(IR.ConditionCode.NotEqual, blocks[2].Label),
                new Instructions.MoveInstruction(ecx, op2L),
                new Instructions.MoveInstruction(eax, op1H),
                new Instructions.LogicalXorInstruction(edx, edx),
                new Instructions.UDivInstruction(eax, ecx),
                new Instructions.MoveInstruction(eax, op1L),
                new Instructions.UDivInstruction(eax, ecx),
                new Instructions.MoveInstruction(eax, edx),
                new Instructions.LogicalXorInstruction(edx, edx),
                new Instructions.DecInstruction(edi),
                new Instructions.JnsBranchInstruction(blocks[6].Label),
                new IR.JmpInstruction(nextBlock.Label),
            });
			;
			// Here we do it the hard way.  Remember, eax contains the high word of DVSR
			//
			// L3:
			//        mov     ebx,eax         ; ebx:ecx <- divisor
			//        mov     ecx,LOWORD(DVSR)
			//        mov     edx,HIWORD(DVND) ; edx:eax <- dividend
			//        mov     eax,LOWORD(DVND)
			blocks[2].Instructions.AddRange(new Instruction[] {
                new Instructions.MoveInstruction(ebx, eax),
                new Instructions.MoveInstruction(ecx, op2L),
                new Instructions.MoveInstruction(edx, op1H),
                new Instructions.MoveInstruction(eax, op1L),
				new IR.JmpInstruction(blocks[3].Label),
            });

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

			blocks[3].Instructions.AddRange(new Instruction[] {
                new Instructions.ShrInstruction(ebx, new ConstantOperand(U1, 1)),
                new Instructions.RcrInstruction(ecx, new ConstantOperand(U1, 1)), // RCR
                new Instructions.ShrInstruction(edx, new ConstantOperand(U1, 1)),
                new Instructions.RcrInstruction(eax, new ConstantOperand(U1, 1)),
                new Instructions.LogicalOrInstruction(ebx, ebx),
                new IR.BranchInstruction(IR.ConditionCode.NotEqual, blocks[3].Label),
                new Instructions.UDivInstruction(eax, ecx),
                new Instructions.MoveInstruction(ecx, eax),
                new IL.MulInstruction(IL.OpCode.Mul, eax, eax, op2H),
                new Instructions.Intrinsics.XchgInstruction(ecx, eax),
                new IL.MulInstruction(IL.OpCode.Mul, eax, eax, op2L),
                new Instructions.AddInstruction(edx, ecx),
                new IR.BranchInstruction(IR.ConditionCode.UnsignedLessThan, blocks[4].Label),
                new Instructions.CmpInstruction(edx, op1H),
                new IR.BranchInstruction(IR.ConditionCode.UnsignedGreaterThan, blocks[4].Label),
                new IR.BranchInstruction(IR.ConditionCode.UnsignedLessThan, blocks[5].Label),
                new Instructions.CmpInstruction(eax, op1L),
                new IR.BranchInstruction(IR.ConditionCode.UnsignedLessOrEqual, blocks[5].Label),
                new IR.JmpInstruction(blocks[4].Label),
            });

			// L6:
			blocks[4].Instructions.AddRange(new Instruction[] {
                new Instructions.SubInstruction(eax, op2L),
                new Instructions.SbbInstruction(edx, op2H),
                new IR.JmpInstruction(blocks[5].Label),
            });

			// L7:
			//
			// Calculate remainder by subtracting the result from the original dividend.
			// Since the result is already in a register, we will do the subtract in the
			// opposite direction and negate the result if necessary.
			//
			blocks[5].Instructions.AddRange(new Instruction[] {
                new Instructions.SubInstruction(eax, op1L),
                new Instructions.SbbInstruction(edx, op1H),
                new Instructions.DecInstruction(edi),
                new Instructions.JnsBranchInstruction(nextBlock.Label),
                new IR.JmpInstruction(blocks[6].Label),
            });

			// L4:
			//        neg     edx             ; otherwise, negate the result
			//        neg     eax
			//        sbb     edx,0
			blocks[6].Instructions.InsertRange(0, new Instruction[] {
                new Instructions.NegInstruction(edx),
                new Instructions.NegInstruction(eax),
                new Instructions.SbbInstruction(edx, new ConstantOperand(I4, 0)),
                new IR.JmpInstruction(nextBlock.Label),
            });

			nextBlock.Instructions.InsertRange(0, new Instruction[] {
                new MoveInstruction(op0L, eax),
                new MoveInstruction(op0H, edx),
                new IR.PopInstruction(ebx),
                new IR.PopInstruction(esi),
                new IR.PopInstruction(edi),
            });

			// Link the created blocks together
			LinkBlocks(ctx.Block, blocks[0]);
			LinkBlocks(blocks[0], blocks[1]);
			LinkBlocks(blocks[1], blocks[6]);
			LinkBlocks(blocks[1], nextBlock);
			LinkBlocks(blocks[1], blocks[2]);
			LinkBlocks(blocks[2], blocks[3]);
			LinkBlocks(blocks[3], blocks[3]);
			LinkBlocks(blocks[3], blocks[4]);
			LinkBlocks(blocks[3], blocks[5]);
			LinkBlocks(blocks[4], blocks[5]);
			LinkBlocks(blocks[5], blocks[6]);
			LinkBlocks(blocks[6], nextBlock);
		}

		/// <summary>
		/// Expands the udiv instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="instruction">The instruction.</param>
		private void ExpandUDiv(Context ctx, IR.UDivInstruction instruction)
		{
			BasicBlock[] blocks = CreateEmptyBlocks(7);
			BasicBlock nextBlock = SplitBlock(ctx, instruction, blocks[0]);

			SigType I4 = new SigType(CilElementType.I4);
			SigType U4 = new SigType(CilElementType.U4);
			SigType U1 = new SigType(CilElementType.U1);

			Operand op0H, op1H, op2H, op0L, op1L, op2L;
			SplitLongOperand(instruction.Results[0], out op0L, out op0H);
			SplitLongOperand(instruction.Operand1, out op1L, out op1H);
			SplitLongOperand(instruction.Operand2, out op2L, out op2H);
			RegisterOperand eax = new RegisterOperand(U4, GeneralPurposeRegister.EAX);
			RegisterOperand ebx = new RegisterOperand(U4, GeneralPurposeRegister.EBX);
			RegisterOperand edx = new RegisterOperand(U4, GeneralPurposeRegister.EDX);
			RegisterOperand ecx = new RegisterOperand(U4, GeneralPurposeRegister.ECX);
			RegisterOperand edi = new RegisterOperand(U4, GeneralPurposeRegister.EDI);
			RegisterOperand esi = new RegisterOperand(U4, GeneralPurposeRegister.ESI);

			Replace(ctx, new Instruction[] {
                new IR.PushInstruction(edi),
                new IR.PushInstruction(esi),
                new IR.PushInstruction(ebx),
                new Instructions.MoveInstruction(eax, op2H),
                new Instructions.LogicalOrInstruction(eax, eax),
                new IR.BranchInstruction(IR.ConditionCode.NotEqual, blocks[0].Label),
                new Instructions.MoveInstruction(ecx, op2L),
                new Instructions.MoveInstruction(eax, op1H),
                new Instructions.LogicalXorInstruction(edx, edx),
                new Instructions.UDivInstruction(eax, ecx),
                new Instructions.MoveInstruction(ebx, eax),
                new Instructions.MoveInstruction(eax, op1L),
                new Instructions.UDivInstruction(eax, ecx),
                new Instructions.MoveInstruction(edx, ebx),
                new IR.JmpInstruction(nextBlock.Label),
            });

			// L1
			blocks[0].Instructions.AddRange(new Instruction[] {
                new Instructions.MoveInstruction(ecx, eax),
                new Instructions.MoveInstruction(ebx, op2L),
                new Instructions.MoveInstruction(edx, op1H),
                new Instructions.MoveInstruction(eax, op1L),
				new IR.JmpInstruction(blocks[2].Label),
			});

			// L3
			blocks[1].Instructions.AddRange(new Instruction[] {
                new Instructions.ShrInstruction(ecx, new ConstantOperand(U1, 1)),
                new Instructions.RcrInstruction(ebx, new ConstantOperand(U1, 1)), // RCR
                new Instructions.ShrInstruction(edx, new ConstantOperand(U1, 1)),
                new Instructions.RcrInstruction(eax, new ConstantOperand(U1, 1)),
                new Instructions.LogicalOrInstruction(ecx, ecx),
                new IR.BranchInstruction(IR.ConditionCode.NotEqual, blocks[1].Label),
                new Instructions.UDivInstruction(eax, ebx),
                new Instructions.MoveInstruction(esi, eax),
                new IL.MulInstruction(IL.OpCode.Mul, eax, eax, op2H),
                new Instructions.MoveInstruction(ecx, eax),
                new Instructions.MoveInstruction(eax, op2L),
                new IL.MulInstruction(IL.OpCode.Mul, eax, eax, esi),
                new Instructions.AddInstruction(edx, ecx),
                new IR.BranchInstruction(IR.ConditionCode.UnsignedLessThan, blocks[2].Label),
                new Instructions.CmpInstruction(edx, op1H),
                new IR.BranchInstruction(IR.ConditionCode.UnsignedGreaterThan, blocks[2].Label),
                new IR.BranchInstruction(IR.ConditionCode.UnsignedLessThan, blocks[3].Label),
                new Instructions.CmpInstruction(eax, op1L),
                new IR.BranchInstruction(IR.ConditionCode.UnsignedLessOrEqual, blocks[3].Label),
				new IR.JmpInstruction(blocks[4].Label),
            });

			// L4:
			blocks[2].Instructions.AddRange(new Instruction[] {
                new Instructions.DecInstruction(esi),
				new IR.JmpInstruction(blocks[3].Label),
			});

			// L5
			blocks[3].Instructions.AddRange(new Instruction[] {
                new Instructions.LogicalXorInstruction(edx, edx),
                new Instructions.MoveInstruction(eax, esi),
				new IR.JmpInstruction(nextBlock.Label),
			});

			// L2
			nextBlock.Instructions.InsertRange(0, new Instruction[] {
                new MoveInstruction(op0L, eax),
                new MoveInstruction(op0H, edx),
                new IR.PopInstruction(ebx),
                new IR.PopInstruction(esi),
                new IR.PopInstruction(edi),
            });

			// Link the created blocks together
			LinkBlocks(ctx.Block, blocks[0]);
			LinkBlocks(ctx.Block, nextBlock);
			LinkBlocks(blocks[0], blocks[1]);
			LinkBlocks(blocks[1], blocks[1]);
			LinkBlocks(blocks[1], blocks[2]);
			LinkBlocks(blocks[1], blocks[3]);
			LinkBlocks(blocks[2], blocks[3]);
			LinkBlocks(blocks[3], nextBlock);
		}

		/// <summary>
		/// Expands the urem instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="instruction">The instruction.</param>
		private void ExpandURem(Context ctx, IR.URemInstruction instruction)
		{
			BasicBlock[] blocks = CreateEmptyBlocks(3);
			BasicBlock nextBlock = SplitBlock(ctx, instruction, blocks[0]);

			SigType I4 = new SigType(CilElementType.I4);
			SigType U4 = new SigType(CilElementType.U4);
			SigType U1 = new SigType(CilElementType.U1);

			Operand op0H, op1H, op2H, op0L, op1L, op2L;
			SplitLongOperand(instruction.Results[0], out op0L, out op0H);
			SplitLongOperand(instruction.Operand1, out op1L, out op1H);
			SplitLongOperand(instruction.Operand2, out op2L, out op2H);
			RegisterOperand eax = new RegisterOperand(U4, GeneralPurposeRegister.EAX);
			RegisterOperand ebx = new RegisterOperand(U4, GeneralPurposeRegister.EBX);
			RegisterOperand edx = new RegisterOperand(U4, GeneralPurposeRegister.EDX);
			RegisterOperand ecx = new RegisterOperand(U4, GeneralPurposeRegister.ECX);
			RegisterOperand edi = new RegisterOperand(U4, GeneralPurposeRegister.EDI);
			RegisterOperand esi = new RegisterOperand(U4, GeneralPurposeRegister.ESI);

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
			Replace(ctx, new Instruction[] {
                new IR.PushInstruction(edi),
                new IR.PushInstruction(esi),
                new IR.PushInstruction(ebx),
                new Instructions.MoveInstruction(eax, op2H),
                new Instructions.LogicalOrInstruction(eax, eax),
                new IR.BranchInstruction(IR.ConditionCode.NotEqual, blocks[0].Label),
                new Instructions.MoveInstruction(ecx, op2L),
                new Instructions.MoveInstruction(eax, op1H),
                new Instructions.LogicalXorInstruction(edx, edx),
                new Instructions.UDivInstruction(eax, ecx),
                new Instructions.MoveInstruction(eax, op1L),
                new Instructions.UDivInstruction(eax, ecx),
                new Instructions.MoveInstruction(eax, edx),
                new Instructions.LogicalXorInstruction(edx, edx),
            });

			// L1:
			blocks[0].Instructions.AddRange(new Instruction[] {
                new Instructions.MoveInstruction(ecx, eax),
                new Instructions.MoveInstruction(ebx, op2L),
                new Instructions.MoveInstruction(edx, op1H),
                new Instructions.MoveInstruction(eax, op1L),
				new IR.JmpInstruction(blocks[1].Label),
			});

			// L3:
			blocks[1].Instructions.AddRange(new Instruction[] {
                new Instructions.ShrInstruction(ecx, new ConstantOperand(U1, 1)),
                new Instructions.RcrInstruction(ebx, new ConstantOperand(U1, 1)), // RCR
                new Instructions.ShrInstruction(edx, new ConstantOperand(U1, 1)),
                new Instructions.RcrInstruction(eax, new ConstantOperand(U1, 1)),
                new Instructions.LogicalOrInstruction(ecx, ecx),
                new IR.BranchInstruction(IR.ConditionCode.NotEqual, blocks[1].Label),
                new Instructions.UDivInstruction(eax, ebx),
                new Instructions.MoveInstruction(ecx, eax),
                new Instructions.MulInstruction(eax, op2H),
                new Instructions.Intrinsics.XchgInstruction(ecx, eax),
                new Instructions.MulInstruction(eax, op2L),
                new Instructions.AddInstruction(edx, ecx),
                new IR.BranchInstruction(IR.ConditionCode.UnsignedLessThan, blocks[2].Label),
                new Instructions.CmpInstruction(edx, op1H),
                new IR.BranchInstruction(IR.ConditionCode.UnsignedGreaterThan, blocks[2].Label),
                new IR.BranchInstruction(IR.ConditionCode.UnsignedLessThan, blocks[3].Label),
                new Instructions.CmpInstruction(eax, op1L),
                new IR.BranchInstruction(IR.ConditionCode.UnsignedLessOrEqual, blocks[3].Label),
				new IR.JmpInstruction(blocks[2].Label),
            });

			// L4:
			blocks[2].Instructions.AddRange(new Instruction[] {
                new Instructions.SubInstruction(eax, op2L),
                new Instructions.SbbInstruction(edx, op2H),
				new IR.JmpInstruction(blocks[3].Label),
            });

			// L5:
			blocks[3].Instructions.AddRange(new Instruction[] {
                new Instructions.SubInstruction(eax, op1L),
                new Instructions.SbbInstruction(edx, op1H),
                new Instructions.NegInstruction(edx),
                new Instructions.NegInstruction(eax),
                new Instructions.SbbInstruction(edx, new ConstantOperand(U4, (int)0)),
				new IR.JmpInstruction(nextBlock.Label),
            });

			nextBlock.Instructions.InsertRange(0, new Instruction[] {
                new MoveInstruction(op0L, eax),
                new MoveInstruction(op0H, edx),
                new IR.PopInstruction(ebx),
                new IR.PopInstruction(esi),
                new IR.PopInstruction(edi),
            });

			// Link the created blocks together
			LinkBlocks(ctx.Block, blocks[0]);
			LinkBlocks(ctx.Block, nextBlock);
			LinkBlocks(blocks[0], blocks[1]);
			LinkBlocks(blocks[1], blocks[1]);
			LinkBlocks(blocks[1], blocks[2]);
			LinkBlocks(blocks[1], blocks[3]);
			LinkBlocks(blocks[2], blocks[3]);
			LinkBlocks(blocks[3], nextBlock);
		}

		/// <summary>
		/// Expands the arithmetic shift right instruction for 64-bits.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="instruction">The instruction.</param>
		private void ExpandArithmeticShiftRight(Context ctx, IR.ArithmeticShiftRightInstruction instruction)
		{
			BasicBlock[] blocks = CreateEmptyBlocks(3);
			BasicBlock nextBlock = SplitBlock(ctx, instruction, blocks[0]);

			SigType I4 = new SigType(CilElementType.I4);
			MemoryOperand op0 = instruction.Operand0 as MemoryOperand;
			MemoryOperand op1 = instruction.Operand1 as MemoryOperand;
			Operand count = instruction.Operand2;
			Debug.Assert(op0 != null && op1 != null, @"Shl Int64 operands not in memory.");

			Operand op0H, op1H, op0L, op1L;
			SplitLongOperand(instruction.Operand0, out op0L, out op0H);
			SplitLongOperand(instruction.Operand1, out op1L, out op1H);
			RegisterOperand eax = new RegisterOperand(I4, GeneralPurposeRegister.EAX);
			RegisterOperand edx = new RegisterOperand(I4, GeneralPurposeRegister.EDX);
			RegisterOperand ecx = new RegisterOperand(I4, GeneralPurposeRegister.ECX);

			// ; existing code from the block
			// cmp count, 64
			// jae sign
			Replace(ctx, new Instruction[] {
                new IR.PushInstruction(ecx),
                new Instructions.MoveInstruction(ecx, count),
                new Instructions.MoveInstruction(edx, op1H),
                new Instructions.MoveInstruction(eax, op1L),
                new Instructions.CmpInstruction(ecx, new ConstantOperand(I4, 64)),
                new IR.BranchInstruction(IR.ConditionCode.UnsignedGreaterOrEqual, blocks[2].Label),
				new IR.JmpInstruction(blocks[0].Label),
            });

			// small_shift:
			// cmp count, 32
			// jae large_shift
			// shrd eax,edx,cl
			// sar edx,cl
			// jmp done
			blocks[0].Instructions.AddRange(new Instruction[] {
                new Instructions.CmpInstruction(ecx, new ConstantOperand(I4, 32)),
                new IR.BranchInstruction(IR.ConditionCode.UnsignedGreaterOrEqual, blocks[1].Label),
                new Instructions.ShrdInstruction(eax, edx, ecx),
                new Instructions.SarInstruction(edx, ecx),
                new IR.JmpInstruction(nextBlock.Label)
            });

			// large_shift:
			// mov     eax,edx
			// sar     edx,31
			// and     count,31
			// sar eax,count
			// jmp done
			blocks[1].Instructions.AddRange(new Instruction[] {
                new Instructions.MoveInstruction(eax, edx),
                new Instructions.SarInstruction(edx, new ConstantOperand(new SigType(CilElementType.I1), 0x1F)),
                new Instructions.LogicalAndInstruction(ecx, new ConstantOperand(I4, 0x1F)),
                new Instructions.SarInstruction(eax, ecx),
                new IR.JmpInstruction(nextBlock.Label)
            });

			// clear:
			// mov opH, 0
			// mov opL, 0
			blocks[2].Instructions.AddRange(new Instruction[] {
                new Instructions.SarInstruction(edx, new ConstantOperand(new SigType(CilElementType.I1), 31)),
                new Instructions.MoveInstruction(eax, edx),
				new IR.JmpInstruction(nextBlock.Label),
            });

			// done:
			// ; remaining code from current basic block
			nextBlock.Instructions.InsertRange(0, new Instruction[] {
                new Instructions.MoveInstruction(op0H, edx),
                new Instructions.MoveInstruction(op0L, eax),
                new IR.PopInstruction(ecx)
            });

			// Link the created blocks together
			LinkBlocks(ctx.Block, blocks[0]);
			LinkBlocks(ctx.Block, blocks[2]);
			LinkBlocks(blocks[0], blocks[1]);
			LinkBlocks(blocks[0], nextBlock);
			LinkBlocks(blocks[1], nextBlock);
			LinkBlocks(blocks[2], nextBlock);
		}

		/// <summary>
		/// Expands the shift left instruction for 64-bits.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="instruction">The instruction.</param>
		private void ExpandShiftLeft(Context ctx, IR.ShiftLeftInstruction instruction)
		{
			BasicBlock[] blocks = CreateEmptyBlocks(3);
			BasicBlock nextBlock = SplitBlock(ctx, instruction, blocks[0]);

			SigType I4 = new SigType(CilElementType.I4);
			MemoryOperand op0 = instruction.Operand0 as MemoryOperand;
			MemoryOperand op1 = instruction.Operand1 as MemoryOperand;
			Operand count = instruction.Operand2;
			Debug.Assert(op0 != null && op1 != null, @"Shl Int64 operands not in memory.");

			Operand op0H, op1H, op0L, op1L;
			SplitLongOperand(instruction.Operand0, out op0L, out op0H);
			SplitLongOperand(instruction.Operand1, out op1L, out op1H);
			RegisterOperand eax = new RegisterOperand(I4, GeneralPurposeRegister.EAX);
			RegisterOperand edx = new RegisterOperand(I4, GeneralPurposeRegister.EDX);
			RegisterOperand ecx = new RegisterOperand(I4, GeneralPurposeRegister.ECX);

			// ; existing code from the block
			// cmp count, 64
			// jae clear
			Replace(ctx, new Instruction[] {
                new IR.PushInstruction(ecx),
                new Instructions.MoveInstruction(ecx, count),
                new Instructions.MoveInstruction(edx, op1H),
                new Instructions.MoveInstruction(eax, op1L),
                new Instructions.CmpInstruction(ecx, new ConstantOperand(I4, 64)),
                new IR.BranchInstruction(IR.ConditionCode.UnsignedGreaterOrEqual, blocks[2].Label),
				new IR.JmpInstruction(blocks[0].Label),
            });

			// small_shift:
			// cmp count, 32
			// jae mov_shift
			// shld
			// shl
			// jmp done
			blocks[0].Instructions.AddRange(new Instruction[] {
                new Instructions.CmpInstruction(ecx, new ConstantOperand(I4, 32)),
                new IR.BranchInstruction(IR.ConditionCode.UnsignedGreaterOrEqual, blocks[1].Label),
                new Instructions.ShldInstruction(edx, eax, ecx),
                new Instructions.ShlInstruction(eax, ecx),
                new IR.JmpInstruction(nextBlock.Label)
            });

			// mov_shift:
			// mov opH, opL
			// xor opL, opL
			// and count, 0x1F
			// shl opH, count
			// jmp done
			blocks[1].Instructions.AddRange(new Instruction[] {
                new Instructions.MoveInstruction(edx, eax),
                new Instructions.LogicalXorInstruction(eax, eax),
                new Instructions.LogicalAndInstruction(ecx, new ConstantOperand(I4, 0x1F)),
                new Instructions.ShlInstruction(edx, ecx),
                new IR.JmpInstruction(nextBlock.Label)
            });

			// clear:
			// mov opH, 0
			// mov opL, 0
			blocks[2].Instructions.AddRange(new Instruction[] {
                new Instructions.LogicalXorInstruction(edx, edx),
                new Instructions.LogicalXorInstruction(eax, eax),
				new IR.JmpInstruction(nextBlock.Label),
            });

			// done:
			// ; remaining code from current basic block
			nextBlock.Instructions.InsertRange(0, new Instruction[] {
                new Instructions.MoveInstruction(op0H, edx),
                new Instructions.MoveInstruction(op0L, eax),
                new IR.PopInstruction(ecx)
            });

			// Link the created blocks together
			LinkBlocks(ctx.Block, blocks[0]);
			LinkBlocks(ctx.Block, blocks[2]);
			LinkBlocks(blocks[0], blocks[1]);
			LinkBlocks(blocks[0], nextBlock);
			LinkBlocks(blocks[1], nextBlock);
			LinkBlocks(blocks[2], nextBlock);
		}

		/// <summary>
		/// Expands the shift right instruction for 64-bits.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="instruction">The instruction.</param>
		private void ExpandShiftRight(Context ctx, IR.ShiftRightInstruction instruction)
		{
			BasicBlock[] blocks = CreateEmptyBlocks(3);
			BasicBlock nextBlock = SplitBlock(ctx, instruction, blocks[0]);

			SigType I4 = new SigType(CilElementType.I4);
			MemoryOperand op0 = instruction.Operand0 as MemoryOperand;
			MemoryOperand op1 = instruction.Operand1 as MemoryOperand;
			Operand count = instruction.Operand2;
			Debug.Assert(op0 != null && op1 != null, @"Shl Int64 operands not in memory.");

			Operand op0H, op1H, op0L, op1L;
			SplitLongOperand(instruction.Operand0, out op0L, out op0H);
			SplitLongOperand(instruction.Operand1, out op1L, out op1H);
			RegisterOperand eax = new RegisterOperand(I4, GeneralPurposeRegister.EAX);
			RegisterOperand edx = new RegisterOperand(I4, GeneralPurposeRegister.EDX);
			RegisterOperand ecx = new RegisterOperand(I4, GeneralPurposeRegister.ECX);

			// ; existing code from the block
			// cmp count, 64
			// jae clear
			Replace(ctx, new Instruction[] {
                new IR.PushInstruction(ecx),
                new Instructions.MoveInstruction(ecx, count),
                new Instructions.MoveInstruction(edx, op1H),
                new Instructions.MoveInstruction(eax, op1L),
                new Instructions.CmpInstruction(ecx, new ConstantOperand(I4, 64)),
                new IR.BranchInstruction(IR.ConditionCode.UnsignedGreaterOrEqual, blocks[2].Label)
            });

			// small_shift:
			// cmp count, 32
			// jae large_shift
			// shrd eax,edx,cl
			// sar edx,cl
			// jmp done
			blocks[0].Instructions.AddRange(new Instruction[] {
                new Instructions.CmpInstruction(ecx, new ConstantOperand(I4, 32)),
                new IR.BranchInstruction(IR.ConditionCode.UnsignedGreaterOrEqual, blocks[1].Label),
                new Instructions.ShrdInstruction(eax, edx, ecx),
                new Instructions.ShrInstruction(edx, ecx),
                new IR.JmpInstruction(nextBlock.Label)
            });

			// large_shift:
			// mov     eax,edx
			// sar     edx,31
			// and     count,31
			// sar eax,count
			// jmp done
			blocks[1].Instructions.AddRange(new Instruction[] {
                new Instructions.MoveInstruction(eax, edx),
                new Instructions.LogicalXorInstruction(edx, edx),
                new Instructions.LogicalAndInstruction(ecx, new ConstantOperand(I4, 0x1F)),
                new Instructions.ShrInstruction(eax, ecx),
                new IR.JmpInstruction(nextBlock.Label)
            });

			// clear:
			// mov opH, 0
			// mov opL, 0
			blocks[2].Instructions.AddRange(new Instruction[] {
                new Instructions.LogicalXorInstruction(edx, edx),
                new Instructions.LogicalXorInstruction(eax, eax),
				new IR.JmpInstruction(nextBlock.Label),
            });

			// done:
			// ; remaining code from current basic block
			nextBlock.Instructions.InsertRange(0, new Instruction[] {
                new Instructions.MoveInstruction(op0H, edx),
                new Instructions.MoveInstruction(op0L, eax),
                new IR.PopInstruction(ecx)
            });

			// Link the created blocks together
			LinkBlocks(ctx.Block, blocks[0]);
			LinkBlocks(ctx.Block, blocks[2]);
			LinkBlocks(blocks[0], blocks[1]);
			LinkBlocks(blocks[0], nextBlock);
			LinkBlocks(blocks[1], nextBlock);
			LinkBlocks(blocks[2], nextBlock);
		}

		/// <summary>
		/// Expands the neg instruction for 64-bits.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="instruction">The instruction.</param>
		private void ExpandNeg(Context ctx, IL.NegInstruction instruction)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Expands the not instruction for 64-bits.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="instruction">The instruction.</param>
		private void ExpandNot(Context ctx, IR.LogicalNotInstruction instruction)
		{
			Operand op0H, op1H, op0L, op1L;
			SplitLongOperand(instruction.Operand0, out op0L, out op0H);
			SplitLongOperand(instruction.Operand1, out op1L, out op1H);

			Replace(ctx, new Instruction[] {
                new IR.LogicalNotInstruction(op0H, op1H),
                new IR.LogicalNotInstruction(op0L, op1L),
            });
		}

		/// <summary>
		/// Expands the and instruction for 64-bits.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="instruction">The instruction.</param>
		private void ExpandAnd(Context ctx, IR.LogicalAndInstruction instruction)
		{
			Operand op0H, op1H, op2H, op0L, op1L, op2L;
			SplitLongOperand(instruction.Operand0, out op0L, out op0H);
			SplitLongOperand(instruction.Operand1, out op1L, out op1H);
			SplitLongOperand(instruction.Operand2, out op2L, out op2H);

			Replace(ctx, new Instruction[] {
                new IR.LogicalAndInstruction(op0H, op1H, op2H),
                new IR.LogicalAndInstruction(op0L, op1L, op2L),
            });
		}

		/// <summary>
		/// Expands the or instruction for 64-bits.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="instruction">The instruction.</param>
		private void ExpandOr(Context ctx, IR.LogicalOrInstruction instruction)
		{
			Operand op0H, op1H, op2H, op0L, op1L, op2L;
			SplitLongOperand(instruction.Operand0, out op0L, out op0H);
			SplitLongOperand(instruction.Operand1, out op1L, out op1H);
			SplitLongOperand(instruction.Operand2, out op2L, out op2H);

			Replace(ctx, new Instruction[] {
                new IR.LogicalOrInstruction(op0H, op1H, op2H),
                new IR.LogicalOrInstruction(op0L, op1L, op2L),
            });
		}

		/// <summary>
		/// Expands the neg instruction for 64-bits.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="instruction">The instruction.</param>
		private void ExpandXor(Context ctx, IR.LogicalXorInstruction instruction)
		{
			Operand op0H, op1H, op2H, op0L, op1L, op2L;
			SplitLongOperand(instruction.Operand0, out op0L, out op0H);
			SplitLongOperand(instruction.Operand1, out op1L, out op1H);
			SplitLongOperand(instruction.Operand2, out op2L, out op2H);

			Replace(ctx, new Instruction[] {
                new IR.LogicalXorInstruction(op0H, op1H, op2H),
                new IR.LogicalXorInstruction(op0L, op1L, op2L),
            });
		}

		/// <summary>
		/// Expands the move instruction for 64-bits.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="instruction">The instruction.</param>
		private void ExpandMove(Context ctx, IR.MoveInstruction instruction)
		{
			Operand op0L, op0H, op1L, op1H;
			SplitLongOperand(instruction.Operand0, out op0L, out op0H);
			SplitLongOperand(instruction.Operand1, out op1L, out op1H);

			Replace(ctx, new Instruction[] {
                new IR.MoveInstruction(op0L, op1L),
                new IR.MoveInstruction(op0H, op1H)
            });
		}

		/// <summary>
		/// Expands the unsigned move instruction for 64-bits.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="instruction">The instruction.</param>
		private void ExpandUnsignedMove(Context ctx, IR.ZeroExtendedMoveInstruction instruction)
		{
			MemoryOperand op0 = instruction.Operand0 as MemoryOperand;
			Operand op1 = instruction.Operand1;
			Debug.Assert(op0 != null, @"I8 not in a memory operand!");
			Instruction[] instructions = null;
			SigType U4 = new SigType(CilElementType.U4);
			MemoryOperand op0L = new MemoryOperand(U4, op0.Base, op0.Offset);
			MemoryOperand op0H = new MemoryOperand(U4, op0.Base, new IntPtr(op0.Offset.ToInt64() + 4));
			RegisterOperand eax = new RegisterOperand(U4, GeneralPurposeRegister.EAX);
			RegisterOperand edx = new RegisterOperand(U4, GeneralPurposeRegister.EDX);

			switch (op1.Type.Type) {
				case CilElementType.Boolean:
					instructions = new Instruction[] {
                        new IR.ZeroExtendedMoveInstruction(op0L, op1),
                        new IR.LogicalXorInstruction(op0H, op0H, op0H)
                    };
					break;

				case CilElementType.U1:
					instructions = new Instruction[] {
                        new IR.ZeroExtendedMoveInstruction(eax, op1),
                        new CdqInstruction(),
                        new MoveInstruction(op0L, eax),
                        new IR.LogicalXorInstruction(op0H, op0H, op0H)
                    };
					break;

				case CilElementType.U2: goto case CilElementType.U1;

				case CilElementType.U4:
					instructions = new Instruction[] {
                        new IR.MoveInstruction(eax, op1),
                        new CdqInstruction(),
                        new MoveInstruction(op0L, eax),
                        new MoveInstruction(op0H, edx)
                    };
					break;

				case CilElementType.U8:
					Replace(ctx, new MoveInstruction(op0, op1));
					break;

				case CilElementType.R4:
					throw new NotSupportedException();

				case CilElementType.R8:
					throw new NotSupportedException();

				default:
					throw new NotSupportedException();
			}

			Replace(ctx, instructions);
		}

		/// <summary>
		/// Expands the signed move instruction for 64-bits.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="instruction">The instruction.</param>
		private void ExpandSignedMove(Context ctx, IR.SignExtendedMoveInstruction instruction)
		{
			MemoryOperand op0 = instruction.Operand0 as MemoryOperand;
			Operand op1 = instruction.Operand1;
			Debug.Assert(op0 != null, @"I8 not in a memory operand!");
			Instruction[] instructions = null;
			SigType I4 = new SigType(CilElementType.I4);
			MemoryOperand op0L = new MemoryOperand(I4, op0.Base, op0.Offset);
			MemoryOperand op0H = new MemoryOperand(I4, op0.Base, new IntPtr(op0.Offset.ToInt64() + 4));
			RegisterOperand eax = new RegisterOperand(I4, GeneralPurposeRegister.EAX);
			RegisterOperand edx = new RegisterOperand(I4, GeneralPurposeRegister.EDX);

			switch (op1.Type.Type) {
				case CilElementType.Boolean:
					instructions = new Instruction[] {
                        new IR.ZeroExtendedMoveInstruction(op0L, op1),
                        new IR.LogicalXorInstruction(op0H, op0H, op0H)
                    };
					break;

				case CilElementType.I1:
					instructions = new Instruction[] {
                        new IR.SignExtendedMoveInstruction(eax, op1),
                        new CdqInstruction(),
                        new MoveInstruction(op0L, eax),
                        new MoveInstruction(op0H, edx)
                    };
					break;

				case CilElementType.I2: goto case CilElementType.I1;

				case CilElementType.I4:
					instructions = new Instruction[] {
                        new IR.MoveInstruction(eax, op1),
                        new CdqInstruction(),
                        new MoveInstruction(op0L, eax),
                        new MoveInstruction(op0H, edx)
                    };
					break;

				case CilElementType.I8:
					Replace(ctx, new MoveInstruction(op0, op1));
					break;

				case CilElementType.U1:
					instructions = new Instruction[] {
                        new IR.ZeroExtendedMoveInstruction(eax, op1),
                        new CdqInstruction(),
                        new MoveInstruction(op0L, eax),
                        new IR.LogicalXorInstruction(op0H, op0H, op0H)
                    };
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

			Replace(ctx, instructions);
		}

		/// <summary>
		/// Expands the load instruction for 64-bits.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="instruction">The instruction.</param>
		private void ExpandLoad(Context ctx, IR.LoadInstruction instruction)
		{
			MemoryOperand op0 = instruction.Operand0 as MemoryOperand;
			MemoryOperand op1 = instruction.Operand1 as MemoryOperand;
			Debug.Assert(op0 != null && op1 != null, @"Operands to I8 LoadInstruction are not MemoryOperand.");

			SigType I4 = new SigType(CilElementType.I4);
			MemoryOperand op0L = new MemoryOperand(I4, op0.Base, op0.Offset);
			MemoryOperand op0H = new MemoryOperand(I4, op0.Base, new IntPtr(op0.Offset.ToInt64() + 4));
			RegisterOperand eax = new RegisterOperand(I4, GeneralPurposeRegister.EAX);
			RegisterOperand edx = new RegisterOperand(I4, GeneralPurposeRegister.EDX);

			Replace(ctx, new Instruction[] {
                new x86.Instructions.MoveInstruction(eax, op1),
                new x86.Instructions.MoveInstruction(edx, new MemoryOperand(instruction.Results[0].Type, GeneralPurposeRegister.EAX, IntPtr.Zero)),
                new x86.Instructions.MoveInstruction(op0L, edx),
                new x86.Instructions.MoveInstruction(edx, new MemoryOperand(instruction.Results[0].Type, GeneralPurposeRegister.EAX, new IntPtr(4))),
                new x86.Instructions.MoveInstruction(op0H, edx)
            });
		}

		/// <summary>
		/// Expands the store instruction for 64-bits.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="instruction">The instruction.</param>
		private void ExpandStore(Context ctx, IR.StoreInstruction instruction)
		{
			MemoryOperand op0 = instruction.Operand0 as MemoryOperand;
			MemoryOperand op1 = instruction.Operand1 as MemoryOperand;
			Debug.Assert(op0 != null && op1 != null, @"Operands to I8 LoadInstruction are not MemoryOperand.");

			SigType I4 = new SigType(CilElementType.I4);
			MemoryOperand op1L = new MemoryOperand(I4, op1.Base, op1.Offset);
			MemoryOperand op1H = new MemoryOperand(I4, op1.Base, new IntPtr(op1.Offset.ToInt64() + 4));
			RegisterOperand eax = new RegisterOperand(I4, GeneralPurposeRegister.EAX);
			RegisterOperand edx = new RegisterOperand(I4, GeneralPurposeRegister.EDX);

			Replace(ctx, new Instruction[] {
                new x86.Instructions.MoveInstruction(edx, op0),
                new x86.Instructions.MoveInstruction(eax, op1L),
                new x86.Instructions.MoveInstruction(new MemoryOperand(I4, GeneralPurposeRegister.EDX, IntPtr.Zero), eax),
                new x86.Instructions.MoveInstruction(eax, op1H),
                new x86.Instructions.MoveInstruction(new MemoryOperand(I4, GeneralPurposeRegister.EDX, new IntPtr(4)), eax),
            });
		}

		/// <summary>
		/// Expands the pop instruction for 64-bits.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="instruction">The instruction.</param>
		private void ExpandPop(Context ctx, IR.PopInstruction instruction)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Expands the push instruction for 64-bits.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="instruction">The instruction.</param>
		private void ExpandPush(Context ctx, IR.PushInstruction instruction)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Expands the unary branch instruction for 64-bits.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="instruction">The instruction.</param>
		private void ExpandUnaryBranch(Context ctx, IL.UnaryBranchInstruction instruction)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Create an empty block.
		/// </summary>
		/// <returns></returns>
		private BasicBlock CreateEmptyBlock()
		{
			BasicBlock block = new BasicBlock(_blocks.Count + 0x10000000);
			block.Index = _blocks.Count;
			_blocks.Add(block);
			return block;
		}

		/// <summary>
		/// Creates empty blocks.
		/// </summary>
		/// <param name="blocks">The blocks.</param>
		/// <returns></returns>
		private BasicBlock[] CreateEmptyBlocks(int blocks)
		{
			// Allocate the block array
			BasicBlock[] result = new BasicBlock[blocks];

			for (int index = 0; index < blocks; index++)
				result[index] = CreateEmptyBlock();

			return result;
		}

		/// <summary>
		/// Splits the block.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="instruction">The instruction index to split on.</param>
		/// <param name="insert">The insert to be called after the split.</param>
		/// <returns></returns>
		private BasicBlock SplitBlock(Context ctx, Instruction instruction, BasicBlock insert)
		{
			int label = _blocks.Count + 0x10000000;

			BasicBlock nextBlock = ctx.Block.Split(ctx.Index + 1, label);
			nextBlock.Index = _blocks.Count - 1;
			_blocks.Add(nextBlock);

			foreach (BasicBlock block in ctx.Block.NextBlocks)
				nextBlock.NextBlocks.Add(block);

			ctx.Block.NextBlocks.Clear();

			if (insert != null) {
				ctx.Block.NextBlocks.Add(insert);
				insert.PreviousBlocks.Add(ctx.Block);
				ctx.Block.Instructions.Add(new IR.JmpInstruction(insert.Label));
			}
			else {
				ctx.Block.NextBlocks.Add(nextBlock);
				nextBlock.PreviousBlocks.Add(ctx.Block);
				ctx.Block.Instructions.Add(new IR.JmpInstruction(label));
			}

			return nextBlock;
		}

		/// <summary>
		/// Expands the binary branch instruction for 64-bits.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="instruction">The instruction.</param>
		private void ExpandBinaryBranch(Context ctx, IL.BinaryBranchInstruction instruction)
		{
			BasicBlock[] blocks = CreateEmptyBlocks(4);
			BasicBlock nextBlock = SplitBlock(ctx, instruction, blocks[0]);

			SigType I4 = new SigType(CilElementType.I4);
			SigType U4 = new SigType(CilElementType.U4);
			SigType U1 = new SigType(CilElementType.U1);

			Operand op1H, op1L, op2H, op2L;
			SplitLongOperand(instruction.Operands[0], out op1L, out op1H);
			SplitLongOperand(instruction.Operands[1], out op2L, out op2H);

			int[] targets = instruction.BranchTargets;

			IR.ConditionCode code;

			switch (instruction.Code) {
				// Signed
				case IL.OpCode.Beq_s: code = IR.ConditionCode.Equal; break;
				case IL.OpCode.Bge_s: code = IR.ConditionCode.GreaterOrEqual; break;
				case IL.OpCode.Bgt_s: code = IR.ConditionCode.GreaterThan; break;
				case IL.OpCode.Ble_s: code = IR.ConditionCode.LessOrEqual; break;
				case IL.OpCode.Blt_s: code = IR.ConditionCode.LessThan; break;

				// Unsigned
				case IL.OpCode.Bne_un_s: code = IR.ConditionCode.NotEqual; break;
				case IL.OpCode.Bge_un_s: code = IR.ConditionCode.UnsignedGreaterOrEqual; break;
				case IL.OpCode.Bgt_un_s: code = IR.ConditionCode.UnsignedGreaterThan; break;
				case IL.OpCode.Ble_un_s: code = IR.ConditionCode.UnsignedLessOrEqual; break;
				case IL.OpCode.Blt_un_s: code = IR.ConditionCode.UnsignedLessThan; break;

				// Long form signed
				case IL.OpCode.Beq: goto case IL.OpCode.Beq_s;
				case IL.OpCode.Bge: goto case IL.OpCode.Bge_s;
				case IL.OpCode.Bgt: goto case IL.OpCode.Bgt_s;
				case IL.OpCode.Ble: goto case IL.OpCode.Ble_s;
				case IL.OpCode.Blt: goto case IL.OpCode.Blt_s;

				// Long form unsigned
				case IL.OpCode.Bne_un: goto case IL.OpCode.Bne_un_s;
				case IL.OpCode.Bge_un: goto case IL.OpCode.Bge_un_s;
				case IL.OpCode.Bgt_un: goto case IL.OpCode.Bgt_un_s;
				case IL.OpCode.Ble_un: goto case IL.OpCode.Ble_un_s;
				case IL.OpCode.Blt_un: goto case IL.OpCode.Blt_un_s;

				default:
					throw new NotImplementedException();
			}

			IR.ConditionCode conditionHigh = GetHighCondition(code);

			// I8 is stored LO-HI in x86 LE
			if (code != Mosa.Runtime.CompilerFramework.IR.ConditionCode.Equal) {
				blocks[0].Instructions.AddRange(new Instruction[] {
                    // Compare high dwords
                    new Instructions.CmpInstruction(op1H, op2H),
                    new IR.BranchInstruction(IR.ConditionCode.Equal, blocks[1].Label),
                    // Branch if check already gave results
                    new IR.BranchInstruction(GetHighCondition(code), blocks[3].Label),
					new IR.JmpInstruction(blocks[1].Label),
                });
				blocks[1].Instructions.AddRange(new Instruction[] {
                    // Compare low dwords
                    new Instructions.CmpInstruction(op1L, op2L),
                    // Set the unsigned result...
                    new IR.BranchInstruction(code, blocks[2].Label),
                    new IR.JmpInstruction(blocks[3].Label),
                });
				blocks[2].Instructions.AddRange(new Instruction[] {
                    new IR.JmpInstruction(targets[0]),
                });
				blocks[3].Instructions.AddRange(new Instruction[] {
                    new IR.JmpInstruction(targets[1]),
                });
			}
			else {
				blocks[0].Instructions.AddRange(new Instruction[] {
                    // Compare high dwords
                    new Instructions.CmpInstruction(op1H, op2H),
                    // Branch if check already gave results
                    new IR.BranchInstruction(GetHighCondition(code), blocks[3].Label),
                });
				blocks[1].Instructions.AddRange(new Instruction[] {
                    // Compare low dwords
                    new Instructions.CmpInstruction(op1L, op2L),
                    // Set the unsigned result...
                    new IR.BranchInstruction(code, blocks[2].Label),
                    new IR.JmpInstruction(blocks[3].Label),
                });
				blocks[2].Instructions.AddRange(new Instruction[] {
                    new IR.JmpInstruction(targets[0]),
                });
				blocks[3].Instructions.AddRange(new Instruction[] {
                    new IR.JmpInstruction(targets[1]),
                });
			}

			Remove(ctx);

			LinkBlocks(blocks[1], blocks[2]);
			LinkBlocks(blocks[1], blocks[3]);
			LinkBlocks(blocks[2], blocks[3]);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
		private Mosa.Runtime.CompilerFramework.IR.ConditionCode GetHighCondition(Mosa.Runtime.CompilerFramework.IR.ConditionCode code)
		{
			switch (code) {
				case Mosa.Runtime.CompilerFramework.IR.ConditionCode.Equal: return Mosa.Runtime.CompilerFramework.IR.ConditionCode.NotEqual;
				case Mosa.Runtime.CompilerFramework.IR.ConditionCode.GreaterOrEqual: return Mosa.Runtime.CompilerFramework.IR.ConditionCode.LessThan;
				case Mosa.Runtime.CompilerFramework.IR.ConditionCode.GreaterThan: return Mosa.Runtime.CompilerFramework.IR.ConditionCode.LessThan;
				case Mosa.Runtime.CompilerFramework.IR.ConditionCode.LessOrEqual: return Mosa.Runtime.CompilerFramework.IR.ConditionCode.GreaterThan;
				case Mosa.Runtime.CompilerFramework.IR.ConditionCode.LessThan: return Mosa.Runtime.CompilerFramework.IR.ConditionCode.GreaterThan;
				case Mosa.Runtime.CompilerFramework.IR.ConditionCode.NotEqual: return Mosa.Runtime.CompilerFramework.IR.ConditionCode.Equal;
				default: return code;
			}
		}



		/// <summary>
		/// Expands the binary comparison instruction for 64-bits.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="instruction">The instruction.</param>
		private void ExpandComparison(Context ctx, IR.IntegerCompareInstruction instruction)
		{
			Operand op0 = instruction.Operand0;
			Operand op1 = instruction.Operand1;
			Operand op2 = instruction.Operand2;

			Debug.Assert(op1 != null && op2 != null, @"IntegerCompareInstruction operand not memory!");
			Debug.Assert(op0 is MemoryOperand || op0 is RegisterOperand, @"IntegerCompareInstruction result not memory and not register!");

			SigType I4 = new SigType(CilElementType.I4), U4 = new SigType(CilElementType.U4);
			/*MemoryOperand op1L = new MemoryOperand(U4, op1.Base, op1.Offset);
			MemoryOperand op2L = new MemoryOperand(U4, op2.Base, op2.Offset);
			MemoryOperand op1H = new MemoryOperand(I4, op1.Base, new IntPtr(op1.Offset.ToInt64() + 4));
			MemoryOperand op2H = new MemoryOperand(I4, op2.Base, new IntPtr(op2.Offset.ToInt64() + 4));*/

			Operand op1L, op1H, op2L, op2H;
			SplitLongOperand(op1, out op1L, out op1H);
			SplitLongOperand(op2, out op2L, out op2H);

			// Create an additional block to split the comparison
			BasicBlock[] blocks = CreateEmptyBlocks(3);
			BasicBlock nextBlock = SplitBlock(ctx, instruction, blocks[1]);

			Debug.Assert(nextBlock != null, @"No follower block?");
			IR.BranchInstruction branch = new IR.BranchInstruction(IR.ConditionCode.NotEqual, nextBlock.Label);

			// I8 is stored LO-HI in x86 LE
			Instruction[] instructions = new Instruction[] {
                // Compare high dwords
                new Instructions.CmpInstruction(op1H, op2H),
                // Set the condition code
                new SetccInstruction(op0, instruction.ConditionCode),
                // Branch if check already gave results
                branch,
				new IR.JmpInstruction(blocks[0].Label),
            };
			Replace(ctx, instructions);

			blocks[0].Instructions.AddRange(new Instruction[] {
                // Compare low dwords
                new Instructions.CmpInstruction(op1L, op2L),
                // Set the unsigned result...
                new SetccInstruction(op0, GetUnsignedConditionCode(instruction.ConditionCode)),
				new IR.JmpInstruction(nextBlock.Label),
            });

			LinkBlocks(ctx.Block, nextBlock);
			LinkBlocks(ctx.Block, blocks[0]);
			LinkBlocks(blocks[0], nextBlock);
		}

		/// <summary>
		/// Gets the unsigned condition code.
		/// </summary>
		/// <param name="conditionCode">The condition code to get an unsigned form from.</param>
		/// <returns>The unsigned form of the given condition code.</returns>
		private IR.ConditionCode GetUnsignedConditionCode(IR.ConditionCode conditionCode)
		{
			IR.ConditionCode cc = conditionCode;
			switch (conditionCode) {
				case IR.ConditionCode.Equal: break;
				case IR.ConditionCode.NotEqual: break;
				case IR.ConditionCode.GreaterOrEqual: cc = IR.ConditionCode.UnsignedGreaterOrEqual; break;
				case IR.ConditionCode.GreaterThan: cc = IR.ConditionCode.UnsignedGreaterThan; break;
				case IR.ConditionCode.LessOrEqual: cc = IR.ConditionCode.UnsignedLessOrEqual; break;
				case IR.ConditionCode.LessThan: cc = IR.ConditionCode.UnsignedLessThan; break;
				case IR.ConditionCode.UnsignedGreaterOrEqual: break;
				case IR.ConditionCode.UnsignedGreaterThan: break;
				case IR.ConditionCode.UnsignedLessOrEqual: break;
				case IR.ConditionCode.UnsignedLessThan: break;
				default:
					throw new NotSupportedException();
			}
			return cc;
		}

		/// <summary>
		/// Links the blocks.
		/// </summary>
		/// <param name="from">The block issuing the jump.</param>
		/// <param name="to">The block, where from is jumping to.</param>
		private void LinkBlocks(BasicBlock from, BasicBlock to)
		{
			Debug.Assert(false == from.NextBlocks.Contains(to), @"A link already exists?");
			Debug.Assert(false == to.PreviousBlocks.Contains(from), @"A link already exists?");
			from.NextBlocks.Add(to);
			to.PreviousBlocks.Add(from);
		}

		/// <summary>
		/// Unlinks the blocks.
		/// </summary>
		/// <param name="from">The block, who used to issue the jump.</param>
		/// <param name="to">The block, where the jump was going to.</param>
		private void UnlinkBlocks(BasicBlock from, BasicBlock to)
		{
			Debug.Assert(true == from.NextBlocks.Contains(to), @"Link doesn't exist?");
			Debug.Assert(true == to.PreviousBlocks.Contains(from), @"Link doesn't exist?");
			from.NextBlocks.Remove(to);
			to.PreviousBlocks.Remove(from);
		}

		/// <summary>
		/// Clears the int64 to zero.
		/// </summary>
		/// <param name="block">The basic block to add the clear instructions to.</param>
		/// <param name="opL">The 64-bit memory operand for the lower dword.</param>
		/// <param name="opH">The 64-bit memory operand for the higher dword.</param>
		private void ClearInt64(BasicBlock block, MemoryOperand opL, MemoryOperand opH)
		{
			ConstantOperand zero = new ConstantOperand(new SigType(CilElementType.I4), 0);
			block.Instructions.AddRange(new Instruction[] {
                new MoveInstruction(opL, zero),
                new MoveInstruction(opH, zero)
            });
		}

		#endregion // Utility Methods

		#region IIRVisitor<Context> Members

		void IR.IIRVisitor<Context>.Visit(IR.AddressOfInstruction instruction, Context arg)
		{
		}

		void IR.IIRVisitor<Context>.Visit(IR.ArithmeticShiftRightInstruction instruction, Context arg)
		{
			Operand op0 = instruction.Operands[0];
			if (op0.StackType == StackTypeCode.Int64)
				ExpandArithmeticShiftRight(arg, instruction);
		}

		void IR.IIRVisitor<Context>.Visit(IR.BranchInstruction instruction, Context arg)
		{
		}

		void IR.IIRVisitor<Context>.Visit(IR.CallInstruction instruction, Context arg)
		{
		}

		void IR.IIRVisitor<Context>.Visit(IR.EpilogueInstruction instruction, Context arg)
		{
		}

		void IR.IIRVisitor<Context>.Visit(IR.IntegerCompareInstruction instruction, Context arg)
		{
			Operand op0 = instruction.Operands[0];
			if (op0.StackType == StackTypeCode.Int64)
				ExpandComparison(arg, instruction);
		}

		void IR.IIRVisitor<Context>.Visit(IR.FloatingPointCompareInstruction instruction, Context arg)
		{
		}

		void IR.IIRVisitor<Context>.Visit(IR.FloatingPointToIntegerConversionInstruction instruction, Context arg)
		{
		}

		void IR.IIRVisitor<Context>.Visit(IR.IntegerToFloatingPointConversionInstruction instruction, Context arg)
		{
		}

		void IR.IIRVisitor<Context>.Visit(IR.JmpInstruction instruction, Context arg)
		{
		}

		void IR.IIRVisitor<Context>.Visit(IR.LiteralInstruction instruction, Context arg)
		{
		}

		void IR.IIRVisitor<Context>.Visit(IR.LoadInstruction instruction, Context arg)
		{
			Operand op0 = instruction.Operand0;
			if (op0.StackType == StackTypeCode.Int64)
				ExpandLoad(arg, instruction);
		}

		void IR.IIRVisitor<Context>.Visit(IR.LogicalAndInstruction instruction, Context arg)
		{
			Operand op0 = instruction.Operands[0];
			if (op0.StackType == StackTypeCode.Int64)
				ExpandAnd(arg, instruction);
		}

		void IR.IIRVisitor<Context>.Visit(IR.LogicalOrInstruction instruction, Context arg)
		{
			Operand op0 = instruction.Operands[0];
			if (op0.StackType == StackTypeCode.Int64)
				ExpandOr(arg, instruction);
		}

		void IR.IIRVisitor<Context>.Visit(IR.LogicalXorInstruction instruction, Context arg)
		{
			Operand op0 = instruction.Operands[0];
			if (op0.StackType == StackTypeCode.Int64)
				ExpandXor(arg, instruction);
		}

		void IR.IIRVisitor<Context>.Visit(IR.LogicalNotInstruction instruction, Context arg)
		{
			Operand op0 = instruction.Operands[0];
			if (op0.StackType == StackTypeCode.Int64)
				ExpandNot(arg, instruction);
		}

		void IR.IIRVisitor<Context>.Visit(IR.MoveInstruction instruction, Context arg)
		{
			Operand op0 = instruction.Operands[0];
			if (op0.StackType == StackTypeCode.Int64)
				ExpandMove(arg, instruction);
		}

		void IR.IIRVisitor<Context>.Visit(IR.PhiInstruction instruction, Context arg)
		{
		}

		void IR.IIRVisitor<Context>.Visit(IR.PopInstruction instruction, Context arg)
		{
			Operand op0 = instruction.Results[0];
			if (op0.StackType == StackTypeCode.Int64)
				ExpandPop(arg, instruction);
		}

		void IR.IIRVisitor<Context>.Visit(IR.PrologueInstruction instruction, Context arg)
		{
		}

		void IR.IIRVisitor<Context>.Visit(IR.PushInstruction instruction, Context arg)
		{
			Operand op0 = instruction.Operands[0];
			if (op0.StackType == StackTypeCode.Int64)
				ExpandPush(arg, instruction);
		}

		void IR.IIRVisitor<Context>.Visit(IR.ReturnInstruction instruction, Context arg)
		{
		}

		void IR.IIRVisitor<Context>.Visit(IR.ShiftLeftInstruction instruction, Context arg)
		{
			Operand op0 = instruction.Operands[0];
			if (op0.StackType == StackTypeCode.Int64)
				ExpandShiftLeft(arg, instruction);
		}

		void IR.IIRVisitor<Context>.Visit(IR.ShiftRightInstruction instruction, Context arg)
		{
			Operand op0 = instruction.Operands[0];
			if (op0.StackType == StackTypeCode.Int64)
				ExpandShiftRight(arg, instruction);
		}

		void IR.IIRVisitor<Context>.Visit(IR.SignExtendedMoveInstruction instruction, Context arg)
		{
			Operand op0 = instruction.Operand0;
			if (op0.StackType == StackTypeCode.Int64)
				ExpandSignedMove(arg, instruction);
		}

		void IR.IIRVisitor<Context>.Visit(IR.StoreInstruction instruction, Context arg)
		{
			Operand op0 = instruction.Operands[0];
			if (op0.StackType == StackTypeCode.Int64)
				ExpandStore(arg, instruction);
		}

		void IR.IIRVisitor<Context>.Visit(IR.UDivInstruction instruction, Context arg)
		{
			Operand op0 = instruction.Operands[0];
			if (op0.StackType == StackTypeCode.Int64)
				ExpandUDiv(arg, instruction);
		}

		void IR.IIRVisitor<Context>.Visit(IR.URemInstruction instruction, Context arg)
		{
			Operand op0 = instruction.Operands[0];
			if (op0.StackType == StackTypeCode.Int64)
				ExpandURem(arg, instruction);
		}

		void IR.IIRVisitor<Context>.Visit(IR.ZeroExtendedMoveInstruction instruction, Context arg)
		{
			Operand op0 = instruction.Operand0;
			if (op0.StackType == StackTypeCode.Int64)
				ExpandUnsignedMove(arg, instruction);
		}

		#endregion // IIRVisitor<Context> Members

		#region IILVisitor<Context> Members

		void IL.IILVisitor<Context>.Nop(IL.NopInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.Break(IL.BreakInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.Ldarg(IL.LdargInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.Ldarga(IL.LdargaInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.Ldloc(IL.LdlocInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.Ldloca(IL.LdlocaInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.Ldc(IL.LdcInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.Ldobj(IL.LdobjInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.Ldstr(IL.LdstrInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.Ldfld(IL.LdfldInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.Ldflda(IL.LdfldaInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.Ldsfld(IL.LdsfldInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.Ldsflda(IL.LdsfldaInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.Ldftn(IL.LdftnInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.Ldvirtftn(IL.LdvirtftnInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.Ldtoken(IL.LdtokenInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.Stloc(IL.StlocInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.Starg(IL.StargInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.Stobj(IL.StobjInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.Stfld(IL.StfldInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.Stsfld(IL.StsfldInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.Dup(IL.DupInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.Pop(IL.PopInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.Jmp(IL.JumpInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.Call(IL.CallInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.Calli(IL.CalliInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.Ret(IL.ReturnInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.Branch(IL.BranchInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.UnaryBranch(IL.UnaryBranchInstruction instruction, Context arg)
		{
			Operand op0 = instruction.Operands[0];
			if (op0.StackType == StackTypeCode.Int64)
				ExpandUnaryBranch(arg, instruction);
		}

		void IL.IILVisitor<Context>.BinaryBranch(IL.BinaryBranchInstruction instruction, Context arg)
		{
			Operand op0 = instruction.Operands[0];
			if (op0.StackType == StackTypeCode.Int64)
				ExpandBinaryBranch(arg, instruction);
		}

		void IL.IILVisitor<Context>.Switch(IL.SwitchInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.BinaryLogic(IL.BinaryLogicInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.Shift(IL.ShiftInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.Neg(IL.NegInstruction instruction, Context arg)
		{
			Operand op0 = instruction.Operands[0];
			if (op0.StackType == StackTypeCode.Int64)
				ExpandNeg(arg, instruction);
		}

		void IL.IILVisitor<Context>.Not(IL.NotInstruction instruction, Context arg)
		{
			throw new NotSupportedException();
		}

		void IL.IILVisitor<Context>.Conversion(IL.ConversionInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.Callvirt(IL.CallvirtInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.Cpobj(IL.CpobjInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.Newobj(IL.NewobjInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.Castclass(IL.CastclassInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.Isinst(IL.IsInstInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.Unbox(IL.UnboxInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.Throw(IL.ThrowInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.Box(IL.BoxInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.Newarr(IL.NewarrInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.Ldlen(IL.LdlenInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.Ldelema(IL.LdelemaInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.Ldelem(IL.LdelemInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.Stelem(IL.StelemInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.UnboxAny(IL.UnboxAnyInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.Refanyval(IL.RefanyvalInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.UnaryArithmetic(IL.UnaryArithmeticInstruction instruction, Context arg)
		{
			throw new NotSupportedException();
		}

		void IL.IILVisitor<Context>.Mkrefany(IL.MkrefanyInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.ArithmeticOverflow(IL.ArithmeticOverflowInstruction instruction, Context arg)
		{
			throw new NotSupportedException();
		}

		void IL.IILVisitor<Context>.Endfinally(IL.EndfinallyInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.Leave(IL.LeaveInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.Arglist(IL.ArglistInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.BinaryComparison(IL.BinaryComparisonInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.Localalloc(IL.LocalallocInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.Endfilter(IL.EndfilterInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.InitObj(IL.InitObjInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.Cpblk(IL.CpblkInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.Initblk(IL.InitblkInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.Prefix(IL.PrefixInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.Rethrow(IL.RethrowInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.Sizeof(IL.SizeofInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.Refanytype(IL.RefanytypeInstruction instruction, Context arg)
		{
		}

		void IL.IILVisitor<Context>.Add(IL.AddInstruction instruction, Context arg)
		{
			Operand op0 = instruction.Operands[0];
			if (op0.StackType == StackTypeCode.Int64) {
				ExpandAdd(arg, instruction);
			}
		}

		void IL.IILVisitor<Context>.Sub(IL.SubInstruction instruction, Context arg)
		{
			Operand op0 = instruction.Operands[0];
			if (op0.StackType == StackTypeCode.Int64) {
				ExpandSub(arg, instruction);
			}
		}

		void IL.IILVisitor<Context>.Mul(IL.MulInstruction instruction, Context arg)
		{
			Operand op0 = instruction.Operands[0];
			if (op0.StackType == StackTypeCode.Int64) {
				ExpandMul(arg, instruction);
			}
		}

		void IL.IILVisitor<Context>.Div(IL.DivInstruction instruction, Context arg)
		{
			Operand op0 = instruction.Operands[0];
			if (op0.StackType == StackTypeCode.Int64) {
				ExpandDiv(arg, instruction);
			}
		}

		void IL.IILVisitor<Context>.Rem(IL.RemInstruction instruction, Context arg)
		{
			Operand op0 = instruction.Operands[0];
			if (op0.StackType == StackTypeCode.Int64) {
				ExpandRem(arg, instruction);
			}
		}

		#endregion // IILVisitor<Context> Members

		#region IX86InstructionVisitor<Context> Members

		void IX86InstructionVisitor<Context>.Add(AddInstruction addInstruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.Adc(AdcInstruction adcInstruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.And(LogicalAndInstruction andInstruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.Or(LogicalOrInstruction orInstruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.Xor(LogicalXorInstruction xorInstruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.Sub(SubInstruction subInstruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.Sbb(SbbInstruction sbbInstruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.Mul(MulInstruction mulInstruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.Div(DivInstruction divInstruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.SseAdd(SseAddInstruction addInstruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.SseSub(SseSubInstruction subInstruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.SseMul(SseMulInstruction mulInstruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.SseDiv(SseDivInstruction mulInstruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.Sar(SarInstruction shiftInstruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.Shl(ShlInstruction shiftInstruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.Shld(ShldInstruction shiftInstruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.Shr(ShrInstruction shiftInstruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.Rcr(RcrInstruction rotateInstruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.Shrd(ShrdInstruction shiftInstruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.Cvtsi2ss(Cvtsi2ssInstruction instruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.Cvtsi2sd(Cvtsi2sdInstruction instruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.Cvtsd2ss(Cvtsd2ssInstruction instruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.Cvtss2sd(Cvtss2sdInstruction instruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.Cmp(CmpInstruction instruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.Setcc(SetccInstruction instruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.Cli(CliInstruction instruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.Cld(CldInstruction instruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.Cdq(CdqInstruction instruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.CmpXchg(CmpXchgInstruction instruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.Hlt(HltInstruction instruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.In(InInstruction instruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.Int(IntInstruction instruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.Jns(JnsBranchInstruction instruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.Iretd(IretdInstruction instruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.Lgdt(LgdtInstruction instruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.Lidt(LidtInstruction instruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.Lock(LockIntruction instruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.Dec(DecInstruction instruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.Inc(IncInstruction instruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.Neg(NegInstruction instruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.Out(OutInstruction instruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.Pause(PauseInstruction instruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.Pop(PopInstruction instruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.Popad(PopadInstruction instruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.Popfd(PopfdInstruction instruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.Push(PushInstruction instruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.Pushad(PushadInstruction instruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.Pushfd(PushfdInstruction instruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.Rdmsr(RdmsrInstruction instruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.Rdpmc(RdpmcInstruction instruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.Rdtsc(RdtscInstruction instruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.Rep(RepInstruction instruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.Sti(StiInstruction instruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.Stosb(StosbInstruction instruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.Stosd(StosdInstruction instruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.UDiv(UDivInstruction divInstruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.Xchg(XchgInstruction instruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.Comisd(ComisdInstruction instruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.Comiss(ComissInstruction instruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.Ucomisd(UcomisdInstruction instruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.Ucomiss(UcomissInstruction instruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.CpuId(CpuIdInstruction instruction, Context arg)
		{
		}

		void IX86InstructionVisitor<Context>.BochsDebug(BochsDebug instruction, Context arg)
		{
		}

		#endregion // IX86InstructionVisitor<Context> Members
	}
}
