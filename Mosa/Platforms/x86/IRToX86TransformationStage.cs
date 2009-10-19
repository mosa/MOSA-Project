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

using System;
using System.Diagnostics;
using System.IO;

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Linker;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;
using CIL = Mosa.Runtime.CompilerFramework.CIL;
using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Platforms.x86
{
	/// <summary>
	/// Transforms CIL instructions into their appropriate IR.
	/// </summary>
	/// <remarks>
	/// This transformation stage transforms CIL instructions into their equivalent IR sequences.
	/// </remarks>
	public sealed class IRToX86TransformationStage :
		CodeTransformationStage, 
		IR.IIRVisitor, 
		CPUx86.IX86Visitor,
		IMethodCompilerStage,
		IPlatformTransformationStage
	{
		private readonly DataConverter LittleEndianBitConverter = DataConverter.LittleEndian;

		#region IMethodCompilerStage Members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		public override string Name
		{
			get { return @"IRToX86TransformationStage"; }
		}

		/// <summary>
		/// Adds this stage to the given pipeline.
		/// </summary>
		/// <param name="pipeline">The pipeline to add this stage to.</param>
		public override void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline)
		{
			pipeline.InsertBefore<FinalX86TransformationStage>(this);
		}

		#endregion // IMethodCompilerStage Members

		#region IIRVisitor

		/// <summary>
		/// Addresses the of instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.AddressOfInstruction(Context ctx)
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
		void IR.IIRVisitor.ArithmeticShiftRightInstruction(Context ctx)
		{
			HandleShiftOperation(ctx, CPUx86.Instruction.SarInstruction);
		}

		/// <summary>
		/// Epilogues the instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.EpilogueInstruction(Context ctx)
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
				ctx.InsertInstructionAfter(IR.Instruction.PopInstruction, ebp);
				// ret
				ctx.InsertInstructionAfter(IR.Instruction.ReturnInstruction);
			}
			else {
				// pop edx
				ctx.SetInstruction(IR.Instruction.PopInstruction, new RegisterOperand(I, GeneralPurposeRegister.EDX));
				// add esp, -localsSize
				ctx.InsertInstructionAfter(CPUx86.Instruction.AddInstruction, esp, new ConstantOperand(I, -stackSize));
				// pop ebp
				ctx.InsertInstructionAfter(IR.Instruction.PopInstruction, ebp);
				// ret
				ctx.InsertInstructionAfter(IR.Instruction.ReturnInstruction);
			}
		}

		/// <summary>
		/// Floatings the point compare instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.FloatingPointCompareInstruction(Context ctx)
		{
			Operand op0 = ctx.Operand1;
			Operand source = EmitConstant(ctx.Operand2);
			Operand destination = EmitConstant(ctx.Operand3);
			IR.ConditionCode setcc = IR.ConditionCode.Equal;

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
				case IR.ConditionCode.Equal: break;
				case IR.ConditionCode.NotEqual: break;
				case IR.ConditionCode.UnsignedGreaterOrEqual: setcc = IR.ConditionCode.GreaterOrEqual; break;
				case IR.ConditionCode.UnsignedGreaterThan: setcc = IR.ConditionCode.GreaterThan; break;
				case IR.ConditionCode.UnsignedLessOrEqual: setcc = IR.ConditionCode.LessOrEqual; break;
				case IR.ConditionCode.UnsignedLessThan: setcc = IR.ConditionCode.LessThan; break;
				case IR.ConditionCode.GreaterOrEqual: setcc = IR.ConditionCode.UnsignedGreaterOrEqual; break;
				case IR.ConditionCode.GreaterThan: setcc = IR.ConditionCode.UnsignedGreaterThan; break;
				case IR.ConditionCode.LessOrEqual: setcc = IR.ConditionCode.UnsignedLessOrEqual; break;
				case IR.ConditionCode.LessThan: setcc = IR.ConditionCode.UnsignedLessThan; break;
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
					case IR.ConditionCode.Equal:
						ctx.InsertInstructionAfter(CPUx86.Instruction.UcomissInstruction, source, destination);
						break;
					case IR.ConditionCode.NotEqual: goto case IR.ConditionCode.Equal;
					case IR.ConditionCode.UnsignedGreaterOrEqual: goto case IR.ConditionCode.Equal;
					case IR.ConditionCode.UnsignedGreaterThan: goto case IR.ConditionCode.Equal;
					case IR.ConditionCode.UnsignedLessOrEqual: goto case IR.ConditionCode.Equal;
					case IR.ConditionCode.UnsignedLessThan: goto case IR.ConditionCode.Equal;
					case IR.ConditionCode.GreaterOrEqual:
						ctx.InsertInstructionAfter(CPUx86.Instruction.ComissInstruction, source, destination);
						break;
					case IR.ConditionCode.GreaterThan: goto case IR.ConditionCode.GreaterOrEqual;
					case IR.ConditionCode.LessOrEqual: goto case IR.ConditionCode.GreaterOrEqual;
					case IR.ConditionCode.LessThan: goto case IR.ConditionCode.GreaterOrEqual;
				}
			}
			else {
				switch (ctx.ConditionCode) {
					case IR.ConditionCode.Equal:
						ctx.InsertInstructionAfter(CPUx86.Instruction.UcomisdInstruction, source, destination);
						break;
					case IR.ConditionCode.NotEqual: goto case IR.ConditionCode.Equal;
					case IR.ConditionCode.UnsignedGreaterOrEqual: goto case IR.ConditionCode.Equal;
					case IR.ConditionCode.UnsignedGreaterThan: goto case IR.ConditionCode.Equal;
					case IR.ConditionCode.UnsignedLessOrEqual: goto case IR.ConditionCode.Equal;
					case IR.ConditionCode.UnsignedLessThan: goto case IR.ConditionCode.Equal;
					case IR.ConditionCode.GreaterOrEqual:
						ctx.InsertInstructionAfter(CPUx86.Instruction.ComisdInstruction, source, destination);
						break;
					case IR.ConditionCode.GreaterThan: goto case IR.ConditionCode.GreaterOrEqual;
					case IR.ConditionCode.LessOrEqual: goto case IR.ConditionCode.GreaterOrEqual;
					case IR.ConditionCode.LessThan: goto case IR.ConditionCode.GreaterOrEqual;
				}
			}

			// Determine the result
			ctx.InsertInstructionAfter(CPUx86.Instruction.SetccInstruction, op0);
			ctx.ConditionCode = setcc;

			// Extend this to the full register, if we're storing it in a register
			if (op0 is RegisterOperand) {
				RegisterOperand rop = new RegisterOperand(new SigType(CilElementType.U1), ((RegisterOperand)op0).Register);
				ctx.InsertInstructionAfter(IR.Instruction.ZeroExtendedMoveInstruction, op0, rop);
			}

		}

		/// <summary>
		/// Integers the compare instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.IntegerCompareInstruction(Context ctx)
		{
			HandleComparisonInstruction(ctx);
		}

		/// <summary>
		/// Loads the instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.LoadInstruction(Context ctx)
		{
			//RegisterOperand eax = new RegisterOperand(Architecture.NativeType, GeneralPurposeRegister.EAX);
			RegisterOperand eax = new RegisterOperand(ctx.Operand1.Type, GeneralPurposeRegister.EAX);
			ctx.SetInstruction(CPUx86.Instruction.MoveInstruction, eax, ctx.Operand1);
			ctx.InsertInstructionAfter(CPUx86.Instruction.MoveInstruction, eax, new MemoryOperand(ctx.Result.Type, GeneralPurposeRegister.EAX, IntPtr.Zero));
			ctx.InsertInstructionAfter(CPUx86.Instruction.MoveInstruction, ctx.Result, eax);
		}

		/// <summary>
		/// Logicals the and instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.LogicalAndInstruction(Context ctx)
		{
			ThreeTwoAddressConversion(ctx);
		}

		/// <summary>
		/// Logicals the or instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.LogicalOrInstruction(Context ctx)
		{
			ThreeTwoAddressConversion(ctx);
		}

		/// <summary>
		/// Logicals the xor instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.LogicalXorInstruction(Context ctx)
		{
			ThreeTwoAddressConversion(ctx);
		}

		/// <summary>
		/// Logicals the not instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.LogicalNotInstruction(Context ctx)
		{
			TwoOneAddressConversion(ctx);
		}

		/// <summary>
		/// Moves the instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.MoveInstruction(Context ctx)
		{
			// We need to replace ourselves in case of a Memory -> Memory transfer
			Operand op0 = ctx.Operand1;
			Operand op1 = ctx.Operand2;
			op1 = EmitConstant(op1);

			if (!(op0 is MemoryOperand) || !(op1 is MemoryOperand)) return;

			RegisterOperand rop;
			if (op0.StackType == StackTypeCode.F || op1.StackType == StackTypeCode.F)
				rop = new RegisterOperand(op0.Type, SSE2Register.XMM0);
			else if (op0.StackType == StackTypeCode.Int64)
				rop = new RegisterOperand(op0.Type, SSE2Register.XMM0);
			else
				rop = new RegisterOperand(op0.Type, GeneralPurposeRegister.EAX);

			ctx.SetInstruction(CPUx86.Instruction.MoveInstruction, rop, op1);
			ctx.InsertInstructionAfter(CPUx86.Instruction.MoveInstruction, op0, rop);
		}

		/// <summary>
		/// Prologues the instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.PrologueInstruction(Context ctx)
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
			ctx.SetInstruction(CPUx86.Intrinsics.Instruction.BochsDebug);
			// push ebp
			ctx.InsertInstructionAfter(IR.Instruction.PushInstruction, null, ebp);
			// mov ebp, esp
			ctx.InsertInstructionAfter(IR.Instruction.MoveInstruction, ebp, esp);
			// sub esp, localsSize
			ctx.InsertInstructionAfter(CPUx86.Instruction.SubInstruction, esp, new ConstantOperand(I, -stackSize));
			// Initialize all locals to zero
			ctx.InsertInstructionAfter(IR.Instruction.PushInstruction, null, edi);
			ctx.InsertInstructionAfter(IR.Instruction.MoveInstruction, edi, esp);
			ctx.InsertInstructionAfter(IR.Instruction.PushInstruction, null, ecx);
			ctx.InsertInstructionAfter(CPUx86.Instruction.AddInstruction, edi, new ConstantOperand(I, 4));
			ctx.InsertInstructionAfter(IR.Instruction.MoveInstruction, ecx, new ConstantOperand(I, (-stackSize) / 4));
			ctx.InsertInstructionAfter(CPUx86.Instruction.LogicalXorInstruction, eax, eax);
			ctx.InsertInstructionAfter(CPUx86.Intrinsics.Instruction.RepInstruction);
			ctx.InsertInstructionAfter(CPUx86.Intrinsics.Instruction.StosdInstruction);
			ctx.InsertInstructionAfter(IR.Instruction.PopInstruction, ecx);
			ctx.InsertInstructionAfter(IR.Instruction.PopInstruction, edi);
			/*
			 * This move adds the runtime method identification token onto the stack. This
			 * allows us to perform call stack identification and gives the garbage collector 
			 * the possibility to identify roots into the managed heap. 
			 */
			// mov [ebp-4], token
			ctx.InsertInstructionAfter(IR.Instruction.MoveInstruction, new MemoryOperand(I, GeneralPurposeRegister.EBP, new IntPtr(-4)), new ConstantOperand(I, Compiler.Method.Token));

			// Do not save EDX for non-int64 return values
			if (Compiler.Method.Signature.ReturnType.Type != CilElementType.I8 &&
				Compiler.Method.Signature.ReturnType.Type != CilElementType.U8) {
				// push edx
				ctx.InsertInstructionAfter(IR.Instruction.PushInstruction, null, new RegisterOperand(I, GeneralPurposeRegister.EDX));
			}
		}

		/// <summary>
		/// Returns the instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.ReturnInstruction(Context ctx)
		{
			Operand op1 = ctx.Operand1;

			ctx.SetInstruction(CIL.Instruction.Get(CIL.OpCode.Br));
			ctx.SetBranch(Int32.MaxValue);

			if (op1 != null) {
				ICallingConvention cc = Architecture.GetCallingConvention(Compiler.Method.Signature.CallingConvention);
				cc.MoveReturnValue(ctx.Next, op1);
			}
		}

		/// <summary>
		/// Shifts the left instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.ShiftLeftInstruction(Context ctx)
		{
			HandleShiftOperation(ctx);
		}

		/// <summary>
		/// Shifts the right instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.ShiftRightInstruction(Context ctx)
		{
			HandleShiftOperation(ctx);
		}

		/// <summary>
		/// Stores the instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.StoreInstruction(Context ctx)
		{
			Operand operand1 = ctx.Operand1;
			Operand result = ctx.Result;

			RegisterOperand eax = new RegisterOperand(result.Type, GeneralPurposeRegister.EAX);
			RegisterOperand edx = new RegisterOperand(operand1.Type, GeneralPurposeRegister.EDX);
			ctx.SetInstruction(CPUx86.Instruction.MoveInstruction, eax, result);
			ctx.InsertInstructionAfter(CPUx86.Instruction.MoveInstruction, edx, operand1);
			ctx.InsertInstructionAfter(CPUx86.Instruction.MoveInstruction, new MemoryOperand(operand1.Type, GeneralPurposeRegister.EAX, IntPtr.Zero), edx);
		}

		/// <summary>
		/// UDivInstruction instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.UDivInstruction(Context ctx)
		{
			ThreeTwoAddressConversion(ctx);
		}

		/// <summary>
		/// URemInstruction instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.URemInstruction(Context ctx)
		{
			Operand operand1 = ctx.Operand1;
			Operand operand2 = ctx.Operand2;
			Operand operand3 = ctx.Operand3;

			ctx.SetInstruction(CPUx86.Instruction.MoveInstruction, new RegisterOperand(operand2.Type, GeneralPurposeRegister.EAX), operand2);
			ctx.InsertInstructionAfter(IR.Instruction.UDivInstruction, operand2, operand3);
			ctx.InsertInstructionAfter(CPUx86.Instruction.MoveInstruction, operand1, new RegisterOperand(operand1.Type, GeneralPurposeRegister.EDX));
		}

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.NopInstruction"/> instructions.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.NopInstruction(Context ctx)
		{
			ctx.SetInstruction(CPUx86.Instruction.NopInstruction);
		}

		#endregion //  Members

		#region IIRVisitor - Unused

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
		/// Visitation function for <see cref="IR.IIRVisitor.PopInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.PopInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.PushInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.PushInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.SignExtendedMoveInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.SignExtendedMoveInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.ZeroExtendedMoveInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.ZeroExtendedMoveInstruction(Context context) { }

		#endregion // IIRVisitor

		// FIXME PG - somes of these seem to be X86 to IR (wrong transformation direction)
		#region IX86Visitor

		/// <summary>
		/// Cmp the instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CPUx86.IX86Visitor.Cmp(Context ctx)
		{
			Operand op0 = ctx.Operand1;
			Operand op1 = ctx.Operand2;

			if (((!(op0 is MemoryOperand) || !(op1 is MemoryOperand)) &&
				 (!(op0 is ConstantOperand) || !(op1 is ConstantOperand))) && !(op1 is ConstantOperand))
				return;

			RegisterOperand eax = new RegisterOperand(op0.Type, GeneralPurposeRegister.EAX);

			ctx.Result = eax;

			Context start = ctx.InsertBefore();
			start.SetInstruction(IR.Instruction.PushInstruction, null, eax);

			if (X86.IsSigned(op0))
				start.InsertInstructionAfter(IR.Instruction.SignExtendedMoveInstruction, eax, op0);
			else
				start.InsertInstructionAfter(IR.Instruction.MoveInstruction, eax, op0);

			ctx.InsertInstructionAfter(IR.Instruction.PopInstruction, eax);
		}

		/// <summary>
		/// Cvtss2sdInstruction instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CPUx86.IX86Visitor.Cvtss2sd(Context ctx)
		{
			if (ctx.Operand2 is ConstantOperand)
				ctx.SetInstruction(CPUx86.Instruction.Cvtss2sdInstruction, ctx.Operand1, EmitConstant(ctx.Operand2));
		}

		/// <summary>
		/// MulInstruction instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CPUx86.IX86Visitor.Mul(Context ctx)
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
		/// SseSubInstruction instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CPUx86.IX86Visitor.SseSub(Context ctx)
		{
			EmitOperandConstants(ctx);
			ThreeTwoAddressConversion(ctx);
		}

		#endregion // Members

		#region IX86Visitor - Unused

		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Add"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Add(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Adc"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Adc(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.And"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.And(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Or"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Or(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Xor"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Xor(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Sub"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Sub(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Sbb"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Sbb(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.DirectMultiplication"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.DirectMultiplication(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.DirectDivision"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.DirectDivision(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Div"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Div(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.UDiv"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.UDiv(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.SseAdd"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.SseAdd(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.SseMul"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.SseMul(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.SseMul"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.SseDiv(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Sar"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Sar(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Sal"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Sal(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Shl"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Shl(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Shr"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Shr(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Rcr"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Rcr(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Cvtsi2ss"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Cvtsi2ss(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Cvtsi2sd"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Cvtsi2sd(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Cvtsd2ss"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Cvtsd2ss(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Setcc"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Setcc(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Cdq"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Cdq(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Shld"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Shld(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Shrd"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Shrd(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Comisd"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Comisd(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Comiss"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Comiss(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Ucomisd"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Ucomisd(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Ucomiss"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Ucomiss(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Jns"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Jns(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.BochsDebug"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.BochsDebug(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Cli"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Cli(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Cld"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Cld(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.CmpXchg"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.CmpXchg(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.CpuId"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.CpuId(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.CpuIdEax"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.CpuIdEax(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.CpuIdEbx"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.CpuIdEbx(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.CpuIdEcx"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.CpuIdEcx(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.CpuIdEdx"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.CpuIdEdx(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Hlt"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Hlt(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Invlpg"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Invlpg(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.In"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.In(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Inc"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Inc(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Dec"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Dec(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Int"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Int(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Iretd"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Iretd(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Lgdt"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Lgdt(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Lidt"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Lidt(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Lock"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Lock(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Neg"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Neg(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Nop"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Nop(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Out"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Out(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Pause"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Pause(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Pop"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Pop(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Popad"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Popad(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Popfd"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Popfd(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Push"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Push(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Pushad"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Pushad(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Pushfd"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Pushfd(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Rdmsr"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Rdmsr(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Rdpmc"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Rdpmc(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Rdtsc"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Rdtsc(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Rep"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Rep(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Sti"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Sti(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Stosb"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Stosb(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Stosd"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Stosd(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Xchg"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Xchg(Context context) { }

		#endregion // IX86Visitor - Unused

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
				Architecture.GetTypeRequirements(cop.Type, out size, out alignment);

				string name = String.Format("C_{0}", Guid.NewGuid());
				using (Stream stream = Compiler.Linker.Allocate(name, SectionKind.ROData, size, alignment)) {
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
		private static void SwapComparisonOperands(Context ctx, Operand op1, Operand op2)
		{
			// Swap the operands
			ctx.Operand1 = op2;
			ctx.Operand2 = op1;

			// Negate the condition code if necessary...
			switch (ctx.ConditionCode) {
				case IR.ConditionCode.Equal:
					break;

				case IR.ConditionCode.GreaterOrEqual:
					ctx.ConditionCode = IR.ConditionCode.LessThan;
					break;

				case IR.ConditionCode.GreaterThan:
					ctx.ConditionCode = IR.ConditionCode.LessOrEqual;
					break;

				case IR.ConditionCode.LessOrEqual:
					ctx.ConditionCode = IR.ConditionCode.GreaterThan;
					break;

				case IR.ConditionCode.LessThan:
					ctx.ConditionCode = IR.ConditionCode.GreaterOrEqual;
					break;

				case IR.ConditionCode.NotEqual:
					break;

				case IR.ConditionCode.UnsignedGreaterOrEqual:
					ctx.ConditionCode = IR.ConditionCode.UnsignedLessThan;
					break;

				case IR.ConditionCode.UnsignedGreaterThan:
					ctx.ConditionCode = IR.ConditionCode.UnsignedLessOrEqual;
					break;

				case IR.ConditionCode.UnsignedLessOrEqual:
					ctx.ConditionCode = IR.ConditionCode.UnsignedGreaterThan;
					break;

				case IR.ConditionCode.UnsignedLessThan:
					ctx.ConditionCode = IR.ConditionCode.UnsignedGreaterOrEqual;
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

			ctx.InsertBefore().SetInstruction(IR.Instruction.MoveInstruction, eax, ctx.Operand2);
			ctx.InsertInstructionAfter(IR.Instruction.MoveInstruction, opRes, eax);

		}
		/// <summary>
		/// Converts the given instruction from three address format to a two address format.
		/// </summary>
		/// <param name="ctx">The conversion context.</param>
		private static void ThreeTwoAddressConversion(Context ctx)
		{
			ThreeTwoAddressConversion(ctx, ctx.Instruction);
		}

		/// <summary>
		/// Converts the given instruction from three address format to a two address format.
		/// </summary>
		/// <param name="ctx">The conversion context.</param>
		/// <param name="instruction">The instruction.</param>
		private static void ThreeTwoAddressConversion(Context ctx, IInstruction instruction)
		{
			if (ctx.OperandCount != 2)
				return;

			Operand opRes = ctx.Result;
			Operand op1 = ctx.Operand1;
			Operand op2 = ctx.Operand2;

			// Create registers for different data types
			RegisterOperand eax = new RegisterOperand(opRes.Type, opRes.StackType == StackTypeCode.F ? (Register) SSE2Register.XMM0 : GeneralPurposeRegister.EAX);
			RegisterOperand eaxL = new RegisterOperand(op1.Type, GeneralPurposeRegister.EAX);

			if (instruction != null)
				ctx.SetInstruction(instruction, eax, op2);
			else {
				ctx.Result = eax;
				ctx.Operand1 = eax;
			}

			// Check if we have to sign-extend the operand that's being loaded
			if (X86.IsSigned(op1) && !(op1 is ConstantOperand)) {
				// Signextend it
				ctx.InsertBefore().SetInstruction(IR.Instruction.SignExtendedMoveInstruction, eaxL, op1);
			}
			// Check if the operand has to be zero-extended
			else if (X86.IsUnsigned(op1) && !(op1 is ConstantOperand) && op1.StackType != StackTypeCode.F) {
				ctx.InsertBefore().SetInstruction(IR.Instruction.ZeroExtendedMoveInstruction, eaxL, op1);
			}
			// In any other case: Just load it
			else
				ctx.InsertBefore().SetInstruction(IR.Instruction.MoveInstruction, eax, op1);

			ctx.InsertInstructionAfter(IR.Instruction.MoveInstruction, opRes, eax);
		}
		#endregion // Internals
	}
}
