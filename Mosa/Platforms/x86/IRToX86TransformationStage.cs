/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 *  Scott Balmos (<mailto:sbalmos@fastmail.fm>)
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Linker;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;
using CIL = Mosa.Runtime.CompilerFramework.CIL;
using IR2 = Mosa.Runtime.CompilerFramework.IR2;
using CPUx86 = Mosa.Platforms.x86.CPUx86;

namespace Mosa.Platforms.x86
{
	/// <summary>
	/// Transforms CIL instructions into their appropriate IR2.
	/// </summary>
	/// <remarks>
	/// This transformation stage transforms CIL instructions into their equivalent IR sequences.
	/// </remarks>
	public sealed class IRToX86TransformationStage :
		IR2.IRCombinedWithCILStage,
		IPlatformTransformationStage
	{
		private readonly System.DataConverter LittleEndianBitConverter = System.DataConverter.LittleEndian;

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="IRToX86TransformationStage"/>.
		/// </summary>
		public IRToX86TransformationStage()
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
			get { return @"IRToX86TransformationStage"; }
		}

		/// <summary>
		/// Adds this stage to the given pipeline.
		/// </summary>
		/// <param name="pipeline">The pipeline to add this stage to.</param>
		public override void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline)
		{
		}

		#endregion // IMethodCompilerStage Members

		#region Members

		/// <summary>
		/// Visitation function for <see cref="Call"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void Call(Context ctx)
		{
			HandleInvokeInstruction(ctx);
		}

		/// <summary>
		/// Visitation function for <see cref="Calli"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void Calli(Context ctx)
		{
			HandleInvokeInstruction(ctx);
		}

		/// <summary>
		/// Visitation function for <see cref="Callvirt"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void Callvirt(Context ctx)
		{
			HandleInvokeInstruction(ctx);
		}

		/// <summary>
		/// Visitation function for <see cref="BinaryComparison"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void BinaryComparison(Context ctx)
		{
			throw new NotSupportedException();
			//HandleComparisonInstruction(ctx, instruction);
		}

		/// <summary>
		/// Visitation function for <see cref="Add"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void Add(Context ctx)
		{
			if (ctx.Operand1.StackType == StackTypeCode.F || ctx.Operand2.StackType == StackTypeCode.F)
				HandleCommutativeOperation(ctx, CPUx86.Instruction.SseAddInstruction);
			else
				HandleCommutativeOperation(ctx, CPUx86.Instruction.AddInstruction);
		}

		/// <summary>
		/// Visitation function for <see cref="Sub"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void Sub(Context ctx)
		{
			if (ctx.Operand1.StackType == StackTypeCode.F || ctx.Operand2.StackType == StackTypeCode.F)
				HandleNonCommutativeOperation(ctx, CPUx86.Instruction.SseSubInstruction);
			else
				HandleNonCommutativeOperation(ctx, CPUx86.Instruction.SubInstruction);

		}

		/// <summary>
		/// Visitation function for <see cref="Mul"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void Mul(Context ctx)
		{
			if (ctx.Operand1.StackType == StackTypeCode.F)
				HandleCommutativeOperation(ctx, CPUx86.Instruction.SseMulInstruction);
			else
				HandleCommutativeOperation(ctx, CPUx86.Instruction.MulInstruction);
		}

		/// <summary>
		/// Visitation function for <see cref="Div"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void Div(Context ctx)
		{
			if (X86.IsUnsigned(ctx.Operand1) || X86.IsUnsigned(ctx.Operand2))
				HandleCommutativeOperation(ctx, CPUx86.Instruction.UDivInstruction);
			else if (ctx.Operand1.StackType == StackTypeCode.F)
				HandleCommutativeOperation(ctx, CPUx86.Instruction.SseDivInstruction);
			else
				HandleCommutativeOperation(ctx, CPUx86.Instruction.DivInstruction);
		}

		/// <summary>
		/// Visitation function for <see cref="Rem"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void Rem(Context ctx)
		{
			ctx.InsertInstructionAfter(CPUx86.Instruction.MoveInstruction, new RegisterOperand(ctx.Operand1.Type, GeneralPurposeRegister.EAX), ctx.Operand1);

			if (X86.IsUnsigned(ctx.Operand1))
				ctx.InsertInstructionAfter(IR2.Instruction.ZeroExtendedMoveInstruction, new RegisterOperand(ctx.Operand1.Type, GeneralPurposeRegister.EAX), new RegisterOperand(ctx.Operand1.Type, GeneralPurposeRegister.EAX));
			else
				ctx.InsertInstructionAfter(IR2.Instruction.SignExtendedMoveInstruction, new RegisterOperand(ctx.Operand1.Type, GeneralPurposeRegister.EAX), new RegisterOperand(ctx.Operand1.Type, GeneralPurposeRegister.EAX));

			if (X86.IsUnsigned(ctx.Operand1) && X86.IsUnsigned(ctx.Operand2))
				ctx.InsertInstructionAfter(CPUx86.Instruction.UDivInstruction, ctx.Operand1, ctx.Operand2);
			else
				ctx.InsertInstructionAfter(CPUx86.Instruction.DivInstruction, ctx.Operand1, ctx.Operand2);

			ctx.InsertInstructionAfter(CPUx86.Instruction.MoveInstruction, ctx.Result, new RegisterOperand(ctx.Operand1.Type, GeneralPurposeRegister.EDX));
		}

		#endregion // Members

		#region Members

		/// <summary>
		/// Addresses the of instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void AddressOfInstruction(Context ctx)
		{
			Operand opRes = ctx.Operand1;
			RegisterOperand eax = new RegisterOperand(opRes.Type, GeneralPurposeRegister.EAX);

			ctx.Result = eax;

			ctx.InsertInstructionAfter(CPUx86.Instruction.MoveInstruction, opRes, eax);
		}

		/// <summary>
		/// Arithmetics the shift right instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void ArithmeticShiftRightInstruction(Context ctx)
		{
			HandleShiftOperation(ctx, CPUx86.Instruction.SarInstruction);
		}

		/// <summary>
		/// Epilogues the instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void EpilogueInstruction(Context ctx)
		{
			SigType I = new SigType(CilElementType.I);
			RegisterOperand ebp = new RegisterOperand(I, GeneralPurposeRegister.EBP);
			RegisterOperand esp = new RegisterOperand(I, GeneralPurposeRegister.ESP);
			int stackSize = (int)ctx.Other;

			if (Compiler.Method.Signature.ReturnType.Type == CilElementType.I8 ||
				Compiler.Method.Signature.ReturnType.Type == CilElementType.U8) {

				// add esp, -localsSize
				ctx.SetInstruction(CPUx86.Instruction.AddInstruction, esp, new ConstantOperand(I, -stackSize));
				// pop ebp
				ctx.InsertInstructionAfter(IR2.Instruction.PopInstruction, ebp);
				// ret
				ctx.InsertInstructionAfter(IR2.Instruction.ReturnInstruction);
			}
			else {
				// pop edx
				ctx.SetInstruction(IR2.Instruction.PopInstruction, new RegisterOperand(I, GeneralPurposeRegister.EDX));
				// add esp, -localsSize
				ctx.InsertInstructionAfter(CPUx86.Instruction.AddInstruction, esp, new ConstantOperand(I, -stackSize));
				// pop ebp
				ctx.InsertInstructionAfter(IR2.Instruction.PopInstruction, ebp);
				// ret
				ctx.InsertInstructionAfter(IR2.Instruction.ReturnInstruction);
			}
		}

		/// <summary>
		/// Floatings the point compare instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void FloatingPointCompareInstruction(Context ctx)
		{
			Operand op0 = ctx.Operand1;
			Operand source = EmitConstant(ctx.Operand2);
			Operand destination = EmitConstant(ctx.Operand3);
			IR2.ConditionCode setcc = IR2.ConditionCode.Equal;

			ctx.Remove();

			// Swap the operands if necessary...
			if (source is MemoryOperand && destination is RegisterOperand) {
				SwapComparisonOperands(ctx, source, destination);
				source = ctx.Operand1;
				destination = ctx.Operand2;
			}
			else if (source is MemoryOperand && destination is MemoryOperand) {
				RegisterOperand xmm2 = new RegisterOperand(source.Type, SSE2Register.XMM2);
				ctx.InsertInstructionAfter(CPUx86.Instruction.MoveInstruction, xmm2, source);
				source = xmm2;
			}

			// x86 is messed up :(
			switch (ctx.ConditionCode) {
				case IR2.ConditionCode.Equal: break;
				case IR2.ConditionCode.NotEqual: break;
				case IR2.ConditionCode.UnsignedGreaterOrEqual: setcc = IR2.ConditionCode.GreaterOrEqual; break;
				case IR2.ConditionCode.UnsignedGreaterThan: setcc = IR2.ConditionCode.GreaterThan; break;
				case IR2.ConditionCode.UnsignedLessOrEqual: setcc = IR2.ConditionCode.LessOrEqual; break;
				case IR2.ConditionCode.UnsignedLessThan: setcc = IR2.ConditionCode.LessThan; break;
				case IR2.ConditionCode.GreaterOrEqual: setcc = IR2.ConditionCode.UnsignedGreaterOrEqual; break;
				case IR2.ConditionCode.GreaterThan: setcc = IR2.ConditionCode.UnsignedGreaterThan; break;
				case IR2.ConditionCode.LessOrEqual: setcc = IR2.ConditionCode.UnsignedLessOrEqual; break;
				case IR2.ConditionCode.LessThan: setcc = IR2.ConditionCode.UnsignedLessThan; break;
			}

			// Compare using the smallest precision
			if (source.Type.Type == CilElementType.R4 && destination.Type.Type == CilElementType.R8) {
				RegisterOperand rop = new RegisterOperand(new SigType(CilElementType.R4), SSE2Register.XMM1);
				ctx.InsertInstructionAfter(CPUx86.Instruction.Cvtsd2ssInstruction, rop, destination);
				destination = rop;
			}
			if (source.Type.Type == CilElementType.R8 && destination.Type.Type == CilElementType.R4) {
				RegisterOperand rop = new RegisterOperand(new SigType(CilElementType.R4), SSE2Register.XMM0);
				ctx.InsertInstructionAfter(CPUx86.Instruction.Cvtsd2ssInstruction, rop, source);
				source = rop;
			}

			if (source.Type.Type == CilElementType.R4) {
				switch (ctx.ConditionCode) {
					case IR2.ConditionCode.Equal:
						ctx.InsertInstructionAfter(CPUx86.Instruction.UcomissInstruction, source, destination);
						break;
					case IR2.ConditionCode.NotEqual: goto case IR2.ConditionCode.Equal;
					case IR2.ConditionCode.UnsignedGreaterOrEqual: goto case IR2.ConditionCode.Equal;
					case IR2.ConditionCode.UnsignedGreaterThan: goto case IR2.ConditionCode.Equal;
					case IR2.ConditionCode.UnsignedLessOrEqual: goto case IR2.ConditionCode.Equal;
					case IR2.ConditionCode.UnsignedLessThan: goto case IR2.ConditionCode.Equal;
					case IR2.ConditionCode.GreaterOrEqual:
						ctx.InsertInstructionAfter(CPUx86.Instruction.ComissInstruction, source, destination);
						break;
					case IR2.ConditionCode.GreaterThan: goto case IR2.ConditionCode.GreaterOrEqual;
					case IR2.ConditionCode.LessOrEqual: goto case IR2.ConditionCode.GreaterOrEqual;
					case IR2.ConditionCode.LessThan: goto case IR2.ConditionCode.GreaterOrEqual;
				}
			}
			else {
				switch (ctx.ConditionCode) {
					case IR2.ConditionCode.Equal:
						ctx.InsertInstructionAfter(CPUx86.Instruction.UcomisdInstruction, source, destination);
						break;
					case IR2.ConditionCode.NotEqual: goto case IR2.ConditionCode.Equal;
					case IR2.ConditionCode.UnsignedGreaterOrEqual: goto case IR2.ConditionCode.Equal;
					case IR2.ConditionCode.UnsignedGreaterThan: goto case IR2.ConditionCode.Equal;
					case IR2.ConditionCode.UnsignedLessOrEqual: goto case IR2.ConditionCode.Equal;
					case IR2.ConditionCode.UnsignedLessThan: goto case IR2.ConditionCode.Equal;
					case IR2.ConditionCode.GreaterOrEqual:
						ctx.InsertInstructionAfter(CPUx86.Instruction.ComisdInstruction, source, destination);
						break;
					case IR2.ConditionCode.GreaterThan: goto case IR2.ConditionCode.GreaterOrEqual;
					case IR2.ConditionCode.LessOrEqual: goto case IR2.ConditionCode.GreaterOrEqual;
					case IR2.ConditionCode.LessThan: goto case IR2.ConditionCode.GreaterOrEqual;
				}
			}

			// Determine the result
			ctx.InsertInstructionAfter(CPUx86.Instruction.SetccInstruction, op0);
			ctx.ConditionCode = setcc;

			// Extend this to the full register, if we're storing it in a register
			if (op0 is RegisterOperand) {
				RegisterOperand rop = new RegisterOperand(new SigType(CilElementType.U1), ((RegisterOperand)op0).Register);
				ctx.InsertInstructionAfter(IR2.Instruction.ZeroExtendedMoveInstruction, op0, rop);
			}

		}

		/// <summary>
		/// Integers the compare instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void IntegerCompareInstruction(Context ctx)
		{
			HandleComparisonInstruction(ctx);
		}

		/// <summary>
		/// Loads the instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void LoadInstruction(Context ctx)
		{
			//RegisterOperand eax = new RegisterOperand(Architecture.NativeType, GeneralPurposeRegister.EAX);
			RegisterOperand eax = new RegisterOperand(ctx.Operand1.Type, GeneralPurposeRegister.EAX);
			Replace(ctx, new LegacyInstruction[] {
                new x86.Instructions.MoveInstruction(eax, ctx.Operand1),
                new x86.Instructions.MoveInstruction(eax, new MemoryOperand(ctx.Result.Type, GeneralPurposeRegister.EAX, IntPtr.Zero)),
                new x86.Instructions.MoveInstruction(ctx.Result, eax)
            });
		}

		/// <summary>
		/// Logicals the and instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void LogicalAndInstruction(Context ctx)
		{
			ThreeTwoAddressConversion(ctx);
		}

		/// <summary>
		/// Logicals the or instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void LogicalOrInstruction(Context ctx)
		{
			ThreeTwoAddressConversion(ctx);
		}

		/// <summary>
		/// Logicals the xor instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void LogicalXorInstruction(Context ctx)
		{
			ThreeTwoAddressConversion(ctx);
		}

		/// <summary>
		/// Logicals the not instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void LogicalNotInstruction(Context ctx)
		{
			TwoOneAddressConversion(ctx);
		}

		/// <summary>
		/// Moves the instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void MoveInstruction(Context ctx)
		{
			// We need to replace ourselves in case of a Memory -> Memory transfer
			Operand op0 = ctx.Operand1;
			Operand op1 = ctx.Operand2;
			op1 = EmitConstant(op1);

			if (!(op0 is MemoryOperand) || !(op1 is MemoryOperand)) return;

			List<LegacyInstruction> replacements = new List<LegacyInstruction>();
			RegisterOperand rop;
			if (op0.StackType == StackTypeCode.F || op1.StackType == StackTypeCode.F) {
				rop = new RegisterOperand(op0.Type, SSE2Register.XMM0);
			}
			else if (op0.StackType == StackTypeCode.Int64) {
				rop = new RegisterOperand(op0.Type, SSE2Register.XMM0);
			}
			else {
				rop = new RegisterOperand(op0.Type, GeneralPurposeRegister.EAX);
			}

			replacements.AddRange(new LegacyInstruction[] {
                                                        new Instructions.MoveInstruction(rop, op1),
                                                        new Instructions.MoveInstruction(op0, rop)
                                                    });

			Replace(ctx, replacements.ToArray());
		}

		/// <summary>
		/// Prologues the instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void PrologueInstruction(Context ctx)
		{
			SigType I = new SigType(CilElementType.I4);
			RegisterOperand eax = new RegisterOperand(I, GeneralPurposeRegister.EAX);
			RegisterOperand ecx = new RegisterOperand(I, GeneralPurposeRegister.ECX);
			RegisterOperand ebp = new RegisterOperand(I, GeneralPurposeRegister.EBP);
			RegisterOperand esp = new RegisterOperand(I, GeneralPurposeRegister.ESP);
			RegisterOperand edi = new RegisterOperand(I, GeneralPurposeRegister.EDI);
			int stackSize = (int)ctx.Other;
			Debug.Assert((stackSize % 4) == 0, @"Stack size of method can't be divided by 4!!");

			/* If you want to stop at the _header of an emitted function, just uncomment
                 * the following line. It will issue a breakpoint instruction. Note that if
                 * you debug using visual studio you must enable unmanaged code debugging, 
                 * otherwise the function will never return and the breakpoint will never
                 * appear.
                 */
			// int 3
			//Architecture.CreateInstruction(typeof(Instructions.IntInstruction), new ConstantOperand(new SigType(CilElementType.U1), (byte)3)),

			// Uncomment this line to enable breakpoints within Bochs
			ctx.SetInstruction(CPUx86.Instruction.BochsDebug);
			// push ebp
			ctx.InsertInstructionAfter(IR2.Instruction.PushInstruction, ebp);
			// mov ebp, esp
			ctx.InsertInstructionAfter(IR2.Instruction.MoveInstruction, ebp, esp);
			// sub esp, localsSize
			ctx.InsertInstructionAfter(CPUx86.Instruction.SubInstruction, esp, new ConstantOperand(I, -stackSize));
			// Initialize all locals to zero
			ctx.InsertInstructionAfter(IR2.Instruction.PushInstruction, edi);
			ctx.InsertInstructionAfter(IR2.Instruction.MoveInstruction, edi, esp);
			ctx.InsertInstructionAfter(IR2.Instruction.PushInstruction, ecx);
			ctx.InsertInstructionAfter(CPUx86.Instruction.AddInstruction, edi, new ConstantOperand(I, 4));
			ctx.InsertInstructionAfter(IR2.Instruction.MoveInstruction, ecx, new ConstantOperand(I, (-stackSize) / 4));
			ctx.InsertInstructionAfter(CPUx86.Instruction.LogicalXorInstruction, eax, eax);
			ctx.InsertInstructionAfter(CPUx86.Instruction.RepInstruction);
			ctx.InsertInstructionAfter(CPUx86.Instruction.StosdInstruction);
			ctx.InsertInstructionAfter(IR2.Instruction.PopInstruction, ecx);
			ctx.InsertInstructionAfter(IR2.Instruction.PopInstruction, edi);
			/*
			 * This move adds the runtime method identification token onto the stack. This
			 * allows us to perform call stack identification and gives the garbage collector 
			 * the possibility to identify roots into the managed heap. 
			 */
			// mov [ebp-4], token
			ctx.InsertInstructionAfter(IR2.Instruction.MoveInstruction, new MemoryOperand(I, GeneralPurposeRegister.EBP, new IntPtr(-4)), new ConstantOperand(I, Compiler.Method.Token));

			// Do not save EDX for non-int64 return values
			if (Compiler.Method.Signature.ReturnType.Type != CilElementType.I8 &&
				Compiler.Method.Signature.ReturnType.Type != CilElementType.U8) {
				// push edx
				ctx.InsertInstructionAfter(IR2.Instruction.PushInstruction, new RegisterOperand(I, GeneralPurposeRegister.EDX));
			}
		}

		/// <summary>
		/// Returns the instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void ReturnInstruction(Context ctx)
		{
			Operand op1 = ctx.Operand1;

			ctx.SetInstruction(CIL.Instruction.Get(CIL.OpCode.Br));
			ctx.Branch.Targets[0] = Int32.MaxValue;

			if (op1 != null) {
				ICallingConvention cc = Architecture.GetCallingConvention(Compiler.Method.Signature.CallingConvention);
				cc.MoveReturnValue(ctx.Next, op1);
			}
		}

		/// <summary>
		/// Shifts the left instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void ShiftLeftInstruction(Context ctx)
		{
			HandleShiftOperation(ctx);
		}

		/// <summary>
		/// Shifts the right instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void ShiftRightInstruction(Context ctx)
		{
			HandleShiftOperation(ctx);
		}

		/// <summary>
		/// Stores the instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void StoreInstruction(Context ctx)
		{
			RegisterOperand eax = new RegisterOperand(ctx.Result.Type, GeneralPurposeRegister.EAX);
			RegisterOperand edx = new RegisterOperand(ctx.Operand1.Type, GeneralPurposeRegister.EDX);
			Replace(ctx, new LegacyInstruction[] {
                new x86.Instructions.MoveInstruction(eax, ctx.Result),
                new x86.Instructions.MoveInstruction(edx, ctx.Operand1),
                new x86.Instructions.MoveInstruction(new MemoryOperand(ctx.Operand1.Type, GeneralPurposeRegister.EAX, IntPtr.Zero), edx)
            });
		}

		/// <summary>
		/// Us the div instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void UDivInstruction(Context ctx)
		{
			ThreeTwoAddressConversion(ctx);
		}

		/// <summary>
		/// Us the rem instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void URemInstruction(Context ctx)
		{
			Replace(ctx, new LegacyInstruction[] {
                new x86.Instructions.MoveInstruction(new RegisterOperand(ctx.Operand2.Type, GeneralPurposeRegister.EAX), ctx.Operand2),
                new x86.Instructions.UDivInstruction(ctx.Operand2, ctx.Operand3),
                new x86.Instructions.MoveInstruction(ctx.Operand1, new RegisterOperand(ctx.Operand1.Type, GeneralPurposeRegister.EDX))
            });
		}

		#endregion //  Members

		#region Members

		/// <summary>
		/// CMPs the instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void CmpInstruction(Context ctx)
		{
			Operand op0 = ctx.Operand1;
			Operand op1 = ctx.Operand2;

			if (((!(op0 is MemoryOperand) || !(op1 is MemoryOperand)) &&
				 (!(op0 is ConstantOperand) || !(op1 is ConstantOperand))) && !(op1 is ConstantOperand))
				return;

			RegisterOperand eax = new RegisterOperand(op0.Type, GeneralPurposeRegister.EAX);

			ctx.Result = eax;

			Context start = ctx.InsertBefore();
			start.SetInstruction(IR2.Instruction.PushInstruction, eax);

			if (X86.IsSigned(op0))
				start.InsertInstructionAfter(IR2.Instruction.SignExtendedMoveInstruction, eax, op0);
			else
				start.InsertInstructionAfter(IR2.Instruction.MoveInstruction, eax, op0);

			ctx.InsertInstructionAfter(IR2.Instruction.PopInstruction, eax);
		}

		/// <summary>
		/// CVTSS2SDs the instruction.
		/// </summary>
		/// <param name="ctx">The CTX.</param>
		public void Cvtss2sdInstruction(Context ctx)
		{
			if (ctx.Operand2 is ConstantOperand) {
				LegacyInstruction[] insts = new LegacyInstruction[] { new Instructions.Cvtss2sdInstruction(ctx.Operand1, EmitConstant(ctx.Operand2)) };
				Replace(ctx, insts);
			}
		}

		/// <summary>
		/// Muls the instruction.
		/// </summary>
		/// <param name="ctx">The CTX.</param>
		public void MulInstruction(Context ctx)
		{
			/*
				if (ctx.Operand1.StackType == StackTypeCode.F || ctx.Operand2.StackType == StackTypeCode.F)
				{
					Replace(ctx, Architecture.CreateInstruction(typeof(x86.SseMulInstruction), IL.OpCode.Mul, new Operand[] { ctx.Operand1, ctx.Operand2, ctx.Result }));
				}
				// FIXME
				// Waiting for ConstantPropagation to get shift/optimization to work.
				else if (ctx.Operand2 is ConstantOperand)
				{
					int x = (int)(ctx.Operand2 as ConstantOperand).Value;
					// Check if it's a power of two
					if ((x & (x - 1)) == 0)
					{
						ConstantOperand shift = new ConstantOperand(new Mosa.Runtime.Metadata.Signatures.SigType(CilElementType.I4), (int)System.Math.Log(x, 2));
						Replace(ctx, Architecture.CreateInstruction(typeof(x86.ShiftInstruction), IL.OpCode.Shl, new Operand[] { ctx.Operand1, shift }));
					}
				}
			 */
		}

		/// <summary>
		/// Sses the sub instruction.
		/// </summary>
		/// <param name="ctx">The CTX.</param>
		public void SseSubInstruction(Context ctx)
		{
			EmitOperandConstants(ctx);
			ThreeTwoAddressConversion(ctx);
		}

		#endregion // Members

		#region Internals

		/// <summary>
		/// Emits the constant operands.
		/// </summary>
		/// <param name="ctx">The context.</param>
		private void EmitOperandConstants(Context ctx)
		{
			if (ctx.OperandCount > 0)
				ctx.Operand1 = EmitConstant(ctx.Operand1);
			else if (ctx.OperandCount > 1)
				ctx.Operand2 = EmitConstant(ctx.Operand2);
			else if (ctx.OperandCount > 2)
				ctx.Operand3 = EmitConstant(ctx.Operand3);
		}

		/// <summary>
		/// Emits the constant operands.
		/// </summary>
		/// <param name="ctx">The context.</param>
		private void EmitResultConstants(Context ctx)
		{
			if (ctx.ResultCount > 0)
				ctx.Result = EmitConstant(ctx.Result);
			else if (ctx.OperandCount > 1)
				ctx.Result2 = EmitConstant(ctx.Result2);
		}

		/// <summary>
		/// This function emits a constant variable into the read-only data section.
		/// </summary>
		/// <param name="op">The operand to emit as a constant.</param>
		/// <returns>An operand, which represents the reference to the read-only constant.</returns>
		/// <remarks>
		/// This function checks if the given operand needs to be moved into the read-only data
		/// section of the executable. For x86 this concerns only floating point operands, as these
		/// can't be specified in inline assembly.<para/>
		/// This function checks if the given operand needs to be moved and rewires the operand, if
		/// it must be moved.
		/// </remarks>
		private Operand EmitConstant(Operand op)
		{
			ConstantOperand cop = op as ConstantOperand;
			if (cop != null && cop.StackType == StackTypeCode.F) {
				int size, alignment;
				this.Architecture.GetTypeRequirements(cop.Type, out size, out alignment);

				string name = String.Format("C_{0}", Guid.NewGuid());
				using (Stream stream = this.Compiler.Linker.Allocate(name, SectionKind.ROData, size, alignment)) {
					byte[] buffer;

					switch (cop.Type.Type) {
						case CilElementType.R4:
							buffer = LittleEndianBitConverter.GetBytes((float)cop.Value);
							break;

						case CilElementType.R8:
							buffer = LittleEndianBitConverter.GetBytes((double)cop.Value);
							break;

						default:
							throw new NotSupportedException();
					}

					stream.Write(buffer, 0, buffer.Length);
				}

				// FIXME: Attach the label operand to the linker symbol
				// FIXME: Rename the LabelOperand to SymbolOperand
				// FIXME: Use the provided name to link
				LabelOperand lop = new LabelOperand(cop.Type, name);
				op = lop;
			}
			return op;
		}

		/// <summary>
		/// Special handling for commutative operations.
		/// </summary>
		/// <param name="ctx">The transformation context.</param>
		/// <param name="instruction">The instruction.</param>
		/// <remarks>
		/// Commutative operations are reordered by moving the constant to the second operand,
		/// which allows the instruction selection in the code generator to use a instruction
		/// format with an immediate operand.
		/// </remarks>
		private void HandleCommutativeOperation(Context ctx, IInstruction instruction)
		{
			EmitOperandConstants(ctx);

			// If the first operand is a constant, move it to the second operand
			if (ctx.Operand1 is ConstantOperand) {
				// Yes, swap the operands...
				Operand t = ctx.Operand1;
				ctx.Operand1 = ctx.Operand2;
				ctx.Operand2 = t;
			}

			// In order for mul to work out, the first operand must be equal to the destination operand -
			// if it is not (e.g. c = a + b) then transform it to c = a, c = c + b.
			ThreeTwoAddressConversion(ctx, instruction);
		}

		/// <summary>
		/// Handles the non commutative operation.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="instruction">The instruction.</param>
		private void HandleNonCommutativeOperation(Context ctx, IInstruction instruction)
		{
			EmitResultConstants(ctx);
			EmitOperandConstants(ctx);

			// In order for mul to work out, the first operand must be equal to the destination operand -
			// if it is not (e.g. c = a + b) then transform it to c = a, c = c + b.
			ThreeTwoAddressConversion(ctx, instruction);
		}

		/// <summary>
		/// Special handling for shift operations, which require the shift amount in the ECX or as a constant register.
		/// </summary>
		/// <param name="ctx">The transformation context.</param>
		private void HandleShiftOperation(Context ctx)
		{
			HandleShiftOperation(ctx, ctx.Instruction);
		}

		/// <summary>
		/// Special handling for shift operations, which require the shift amount in the ECX or as a constant register.
		/// </summary>
		/// <param name="ctx">The transformation context.</param>
		/// <param name="instruction">The instruction to transform.</param>
		private void HandleShiftOperation(Context ctx, IInstruction instruction)
		{
			Operand opRes = ctx.Result;
			Operand op1 = ctx.Operand1;
			Operand op2 = ctx.Operand2;
			EmitOperandConstants(ctx);

			RegisterOperand ecx = new RegisterOperand(op2.Type, GeneralPurposeRegister.ECX);

			ctx.SetInstruction(CPUx86.Instruction.MoveInstruction, ecx, op2);
			ctx.InsertInstructionAfter(CPUx86.Instruction.MoveInstruction, opRes, op1);
			ctx.InsertInstructionAfter(instruction, opRes, ecx);


			// FIXME
			// Commented part causes an access violation!
			/*
			if (ops[1] is ConstantOperand)
			{
				Replace(ctx, new Instruction[] {
					Architecture.CreateInstruction(typeof(Instructions.MoveInstruction), opRes, ops[0]),
					Architecture.CreateInstruction(replacementType, opRes, ops[1])
				});
			}
			else
			{*/

			//if (ops[0].Type.Type == CilElementType.Char) {
			//    RegisterOperand ecx = new RegisterOperand(ops[1].Type, GeneralPurposeRegister.ECX);
			//    Replace(ctx, new LegacyInstruction[] {
			//        Architecture.CreateInstruction(typeof(Instructions.MoveInstruction), ecx, ops[1]),
			//        Architecture.CreateInstruction(typeof(Instructions.MoveInstruction), opRes, ops[0]),
			//        Architecture.CreateInstruction(replacementType, opRes, ecx),
			//    });
			//}
			//else {
			//    RegisterOperand ecx = new RegisterOperand(ops[1].Type, GeneralPurposeRegister.ECX);
			//    Replace(ctx, new LegacyInstruction[] {
			//        Architecture.CreateInstruction(typeof(Instructions.MoveInstruction), ecx, ops[1]),
			//        Architecture.CreateInstruction(typeof(Instructions.MoveInstruction), opRes, ops[0]),
			//        Architecture.CreateInstruction(replacementType, opRes, ecx),
			//    });
			//}
		}

		/// <summary>
		/// Processes a method call instruction.
		/// </summary>
		/// <param name="ctx">The transformation context.</param>
		private void HandleInvokeInstruction(Context ctx)
		{
			ICallingConvention cc = Architecture.GetCallingConvention(ctx.InvokeTarget.Signature.CallingConvention);
			Debug.Assert(null != cc, @"Failed to retrieve the calling convention.");
			object result = cc.Expand(ctx);
			if (result is List<LegacyInstruction>) {
				// Replace the single instruction with the set
				List<LegacyInstruction> insts = (List<LegacyInstruction>)result;
				Replace(ctx, insts.ToArray());
			}
			else if (result is LegacyInstruction) {
				// Save the replacement instruction
				Replace(ctx, (LegacyInstruction)result);
			}
		}

		/// <summary>
		/// Handles the comparison instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		private void HandleComparisonInstruction(Context ctx)
		{
			Operand op1 = ctx.Operand1;
			Operand op2 = ctx.Operand2;
			EmitOperandConstants(ctx);

			if (op1 is MemoryOperand && op2 is RegisterOperand) {
				SwapComparisonOperands(ctx, op1, op2);
			}
			else if (op1 is MemoryOperand && op2 is MemoryOperand) {
				RegisterOperand eax = new RegisterOperand(op1.Type, GeneralPurposeRegister.EAX);

				ctx.InsertBefore().SetInstruction(CPUx86.Instruction.MoveInstruction, eax, op1);
				ctx.Operand1 = eax;
			}

			ThreeTwoAddressConversion(ctx, null);
		}

		/// <summary>
		/// Swaps the comparison operands.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="op1">The op1.</param>
		/// <param name="op2">The op2.</param>
		private void SwapComparisonOperands(Context ctx, Operand op1, Operand op2)
		{
			// Swap the operands
			ctx.Operand1 = op2;
			ctx.Operand2 = op1;

			// Negate the condition code if necessary...
			switch (ctx.ConditionCode) {
				case IR2.ConditionCode.Equal:
					break;

				case IR2.ConditionCode.GreaterOrEqual:
					ctx.ConditionCode = IR2.ConditionCode.LessThan;
					break;

				case IR2.ConditionCode.GreaterThan:
					ctx.ConditionCode = IR2.ConditionCode.LessOrEqual;
					break;

				case IR2.ConditionCode.LessOrEqual:
					ctx.ConditionCode = IR2.ConditionCode.GreaterThan;
					break;

				case IR2.ConditionCode.LessThan:
					ctx.ConditionCode = IR2.ConditionCode.GreaterOrEqual;
					break;

				case IR2.ConditionCode.NotEqual:
					break;

				case IR2.ConditionCode.UnsignedGreaterOrEqual:
					ctx.ConditionCode = IR2.ConditionCode.UnsignedLessThan;
					break;

				case IR2.ConditionCode.UnsignedGreaterThan:
					ctx.ConditionCode = IR2.ConditionCode.UnsignedLessOrEqual;
					break;

				case IR2.ConditionCode.UnsignedLessOrEqual:
					ctx.ConditionCode = IR2.ConditionCode.UnsignedGreaterThan;
					break;

				case IR2.ConditionCode.UnsignedLessThan:
					ctx.ConditionCode = IR2.ConditionCode.UnsignedGreaterOrEqual;
					break;
			}
		}

		/// <summary>
		/// Converts the given instruction from two address format to a one address format.
		/// </summary>
		/// <param name="ctx">The conversion context.</param>
		private void TwoOneAddressConversion(Context ctx)
		{
			Operand opRes = ctx.Result;
			RegisterOperand eax = new RegisterOperand(opRes.Type, GeneralPurposeRegister.EAX);
			ctx.Operand1 = EmitConstant(ctx.Operand1);
			ctx.Result = eax;

			ctx.InsertBefore().SetInstruction(IR2.Instruction.MoveInstruction, eax, ctx.Operand2);
			ctx.InsertInstructionAfter(IR2.Instruction.MoveInstruction, opRes, eax);

		}
		/// <summary>
		/// Converts the given instruction from three address format to a two address format.
		/// </summary>
		/// <param name="ctx">The conversion context.</param>
		private void ThreeTwoAddressConversion(Context ctx)
		{
			ThreeTwoAddressConversion(ctx, ctx.Instruction);
		}

		/// <summary>
		/// Converts the given instruction from three address format to a two address format.
		/// </summary>
		/// <param name="ctx">The conversion context.</param>
		/// <param name="instruction">The instruction.</param>
		private void ThreeTwoAddressConversion(Context ctx, IInstruction instruction)
		{
			Operand opRes = ctx.Result;
			Operand op1 = ctx.Operand1;
			Operand op2 = ctx.Operand2;

			// Create registers for different data types
			RegisterOperand eax = new RegisterOperand(opRes.Type, opRes.StackType == StackTypeCode.F ? (Register)SSE2Register.XMM0 : (Register)GeneralPurposeRegister.EAX);
			RegisterOperand eaxL = new RegisterOperand(op1.Type, GeneralPurposeRegister.EAX);
			RegisterOperand eaxS = new RegisterOperand(opRes.Type, GeneralPurposeRegister.EAX);

			if (instruction != null)
				ctx.SetInstruction(instruction, eax, op2);
			else {
				ctx.Result = eax;
				ctx.Operand1 = eax;
			}

			// Check if we have to sign-extend the operand that's being loaded
			if (X86.IsSigned(op1) && !(op1 is ConstantOperand)) {
				// Signextend it
				ctx.InsertBefore().SetInstruction(IR2.Instruction.SignExtendedMoveInstruction, eaxL, op1);
			}
			// Check if the operand has to be zero-extended
			else if (X86.IsUnsigned(op1) && !(op1 is ConstantOperand) && op1.StackType != StackTypeCode.F) {
				ctx.InsertBefore().SetInstruction(IR2.Instruction.ZeroExtendedMoveInstruction, eaxL, op1);
			}
			// In any other case: Just load it
			else
				ctx.InsertBefore().SetInstruction(IR2.Instruction.MoveInstruction, eax, op1);

			ctx.InsertInstructionAfter(IR2.Instruction.MoveInstruction, opRes, eax);
		}
		#endregion // Internals
	}
}
