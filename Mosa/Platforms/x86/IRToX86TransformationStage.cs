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
using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Platforms.x86
{
	/// <summary>
	/// Transforms IR instructions into their appropriate X86.
	/// </summary>
	/// <remarks>
	/// This transformation stage transforms IR instructions into their equivalent X86 sequences.
	/// </remarks>
	public sealed class IRToX86TransformationStage :
		BaseX86TransformationStage,
		IR.IIRVisitor,
		IMethodCompilerStage,
		IPlatformTransformationStage
	{

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
			pipeline.InsertAfter<CILToX86TransformationStage>(this);
		}

		#endregion // IMethodCompilerStage Members

		#region IIRVisitor

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.AddressOfInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.AddressOfInstruction(Context ctx)
		{
			Operand opRes = ctx.Operand1;
			RegisterOperand eax = new RegisterOperand(opRes.Type, GeneralPurposeRegister.EAX);
			ctx.Result = eax;
			ctx.InsertInstructionAfter(CPUx86.Instruction.MovInstruction, opRes, eax);
		}

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.ArithmeticShiftRightInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.ArithmeticShiftRightInstruction(Context ctx)
		{
			HandleShiftOperation(ctx, CPUx86.Instruction.SarInstruction);
		}

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.EpilogueInstruction"/> instruction.
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
				ctx.InsertInstructionAfter(CPUx86.Instruction.PopInstruction, ebp);
				// ret
				ctx.InsertInstructionAfter(IR.Instruction.ReturnInstruction);
			}
			else {
				// pop edx
				ctx.SetInstruction(CPUx86.Instruction.PopInstruction, new RegisterOperand(I, GeneralPurposeRegister.EDX));
				// add esp, -localsSize
				ctx.InsertInstructionAfter(CPUx86.Instruction.AddInstruction, esp, new ConstantOperand(I, -stackSize));
				// pop ebp
				ctx.InsertInstructionAfter(CPUx86.Instruction.PopInstruction, ebp);
				// ret
				ctx.InsertInstructionAfter(CPUx86.Instruction.RetInstruction); // Change to Return
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
				ctx.InsertInstructionAfter(CPUx86.Instruction.MovInstruction, xmm2, source);
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
				ctx.InsertInstructionAfter(CPUx86.Instruction.MovzxInstruction, op0, rop);
			}

		}

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.IntegerCompareInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.IntegerCompareInstruction(Context ctx)
		{
			HandleComparisonInstruction(ctx);
		}

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.JmpInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.JmpInstruction(Context ctx)
		{
			ctx.ReplaceInstructionOnly(CPUx86.Instruction.JmpInstruction);
		}

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.LoadInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.LoadInstruction(Context ctx)
		{
			//RegisterOperand eax = new RegisterOperand(Architecture.NativeType, GeneralPurposeRegister.EAX);
			RegisterOperand eax = new RegisterOperand(ctx.Operand1.Type, GeneralPurposeRegister.EAX);
			ctx.SetInstruction(CPUx86.Instruction.MovInstruction, eax, ctx.Operand1);
			ctx.InsertInstructionAfter(CPUx86.Instruction.MovInstruction, eax, new MemoryOperand(ctx.Result.Type, GeneralPurposeRegister.EAX, IntPtr.Zero));
			ctx.InsertInstructionAfter(CPUx86.Instruction.MovInstruction, ctx.Result, eax);
		}

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.LogicalAndInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.LogicalAndInstruction(Context ctx)
		{
			ThreeTwoAddressConversion(ctx, CPUx86.Instruction.AndInstruction);
		}

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.LogicalOrInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.LogicalOrInstruction(Context ctx)
		{
			ThreeTwoAddressConversion(ctx, CPUx86.Instruction.OrInstruction);
		}

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.LogicalXorInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.LogicalXorInstruction(Context ctx)
		{
			ThreeTwoAddressConversion(ctx, CPUx86.Instruction.XorInstruction);
		}

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.LogicalNotInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.LogicalNotInstruction(Context ctx)
		{
			ThreeTwoAddressConversion(ctx, CPUx86.Instruction.NotInstruction);
		}

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.MoveInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.MoveInstruction(Context ctx)
		{
			// We need to replace ourselves in case of a Memory -> Memory transfer
			Operand op0 = ctx.Result;
			Operand op1 = ctx.Operand1;

			op1 = EmitConstant(op1);

			if (!(op0 is MemoryOperand && op1 is MemoryOperand)) {
				ctx.ReplaceInstructionOnly(CPUx86.Instruction.MovInstruction);
				return;
			}

			RegisterOperand rop;
			if (op0.StackType == StackTypeCode.F || op1.StackType == StackTypeCode.F)
				rop = new RegisterOperand(op0.Type, SSE2Register.XMM0);
			else if (op0.StackType == StackTypeCode.Int64)
				rop = new RegisterOperand(op0.Type, SSE2Register.XMM0);
			else
				rop = new RegisterOperand(op0.Type, GeneralPurposeRegister.EAX);

			ctx.SetInstruction(CPUx86.Instruction.MovInstruction, rop, op1);
			ctx.InsertInstructionAfter(CPUx86.Instruction.MovInstruction, op0, rop);
		}

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.PrologueInstruction"/> instruction.
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
			// ctx.XXX(CPUx86.IntInstruction, new ConstantOperand(new SigType(CilElementType.U1), (byte)3));

			// Uncomment this line to enable breakpoints within Bochs
			//ctx.XXX(CPUx86.Instruction.BochsDebug);

			// push ebp
			ctx.SetInstruction(CPUx86.Instruction.PushInstruction, null, ebp);
			// mov ebp, esp
			ctx.InsertInstructionAfter(CPUx86.Instruction.MovInstruction, ebp, esp);
			// sub esp, localsSize
			ctx.InsertInstructionAfter(CPUx86.Instruction.SubInstruction, esp, new ConstantOperand(I, -stackSize));
			// Initialize all locals to zero
			ctx.InsertInstructionAfter(CPUx86.Instruction.PushInstruction, null, edi);
			ctx.InsertInstructionAfter(CPUx86.Instruction.MovInstruction, edi, esp);
			ctx.InsertInstructionAfter(CPUx86.Instruction.PushInstruction, null, ecx);
			ctx.InsertInstructionAfter(CPUx86.Instruction.AddInstruction, edi, new ConstantOperand(I, 4));
			ctx.InsertInstructionAfter(CPUx86.Instruction.MovInstruction, ecx, new ConstantOperand(I, (-stackSize) / 4));
			ctx.InsertInstructionAfter(CPUx86.Instruction.XorInstruction, eax, eax);
			ctx.InsertInstructionAfter(CPUx86.Instruction.RepInstruction);
			ctx.InsertInstructionAfter(CPUx86.Instruction.StosdInstruction);
			ctx.InsertInstructionAfter(CPUx86.Instruction.PopInstruction, ecx);
			ctx.InsertInstructionAfter(CPUx86.Instruction.PopInstruction, edi);
			/*
			 * This move adds the runtime method identification token onto the stack. This
			 * allows us to perform call stack identification and gives the garbage collector 
			 * the possibility to identify roots into the managed heap. 
			 */
			// mov [ebp-4], token
			ctx.InsertInstructionAfter(CPUx86.Instruction.MovInstruction, new MemoryOperand(I, GeneralPurposeRegister.EBP, new IntPtr(-4)), new ConstantOperand(I, Compiler.Method.Token));

			// Do not save EDX for non-int64 return values
			if (Compiler.Method.Signature.ReturnType.Type != CilElementType.I8 &&
				Compiler.Method.Signature.ReturnType.Type != CilElementType.U8) {
				// push edx
				ctx.InsertInstructionAfter(CPUx86.Instruction.PushInstruction, null, new RegisterOperand(I, GeneralPurposeRegister.EDX));
			}
		}

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.ReturnInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.ReturnInstruction(Context ctx)
		{
			Operand op = ctx.Operand1;

			ctx.Remove();

			if (op != null) {
				ICallingConvention cc = Architecture.GetCallingConvention(Compiler.Method.Signature.CallingConvention);
				cc.MoveReturnValue(ctx, op);
			}

			ctx.InsertInstructionAfter(CPUx86.Instruction.JmpInstruction);
			ctx.SetBranch(Int32.MaxValue);
		}

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.ShiftLeftInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.ShiftLeftInstruction(Context ctx)
		{
			HandleShiftOperation(ctx, CPUx86.Instruction.ShlInstruction);
		}

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.ShiftRightInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.ShiftRightInstruction(Context ctx)
		{
			HandleShiftOperation(ctx, CPUx86.Instruction.ShrInstruction);
		}

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.StoreInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.StoreInstruction(Context ctx)
		{
			Operand operand1 = ctx.Operand1;
			Operand result = ctx.Result;

			RegisterOperand eax = new RegisterOperand(result.Type, GeneralPurposeRegister.EAX);
			RegisterOperand edx = new RegisterOperand(operand1.Type, GeneralPurposeRegister.EDX);
			ctx.SetInstruction(CPUx86.Instruction.MovInstruction, eax, result);
			ctx.InsertInstructionAfter(CPUx86.Instruction.MovInstruction, edx, operand1);
			ctx.InsertInstructionAfter(CPUx86.Instruction.MovInstruction, new MemoryOperand(operand1.Type, GeneralPurposeRegister.EAX, IntPtr.Zero), edx);
		}

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.UDivInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.UDivInstruction(Context ctx)
		{
			ThreeTwoAddressConversion(ctx, ctx.Instruction);

			//RegisterOperand edx = new RegisterOperand(new SigType(CilElementType.U4), GeneralPurposeRegister.EDX);
			//_codeEmitter.Xor(edx, edx);
			//_codeEmitter.Div(instruction.Operand0, instruction.Operand1);

		}

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.URemInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.URemInstruction(Context ctx)
		{
			Operand operand1 = ctx.Operand1;
			Operand operand2 = ctx.Operand2;
			Operand operand3 = ctx.Operand3;

			ctx.SetInstruction(CPUx86.Instruction.MovInstruction, new RegisterOperand(operand2.Type, GeneralPurposeRegister.EAX), operand2);
			ctx.InsertInstructionAfter(IR.Instruction.UDivInstruction, operand2, operand3);
			ctx.InsertInstructionAfter(CPUx86.Instruction.MovInstruction, operand1, new RegisterOperand(operand1.Type, GeneralPurposeRegister.EDX));
		}

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.NopInstruction"/> instructions.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.NopInstruction(Context ctx)
		{
			ctx.ReplaceInstructionOnly(CPUx86.Instruction.NopInstruction);
		}

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.SignExtendedMoveInstruction"/> instructions.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.SignExtendedMoveInstruction(Context ctx)
		{
			ctx.ReplaceInstructionOnly(CPUx86.Instruction.MovsxInstruction);
		}

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.CallInstruction"/> instructions.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.CallInstruction(Context ctx)
		{
			ctx.ReplaceInstructionOnly(CPUx86.Instruction.CallInstruction);
		}

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.ZeroExtendedMoveInstruction"/> instructions.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.ZeroExtendedMoveInstruction(Context ctx)
		{
			switch (ctx.Operand1.Type.Type) {
				case CilElementType.I1:
					ctx.ReplaceInstructionOnly(CPUx86.Instruction.MovzxInstruction);
					return;
				case CilElementType.I2: goto case CilElementType.I1;
				case CilElementType.I4: goto case CilElementType.I1;
				case CilElementType.I8: throw new NotSupportedException();
				case CilElementType.U1: goto case CilElementType.I1;
				case CilElementType.U2: goto case CilElementType.I1;
				case CilElementType.U4: goto case CilElementType.I1;
				case CilElementType.U8: goto case CilElementType.I8;
				case CilElementType.Char: goto case CilElementType.I2;
				default: throw new NotSupportedException();
			}
		}

		#endregion //  Members

		#region IIRVisitor - Unused

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.BranchInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.BranchInstruction(Context context) { }

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

		#endregion // IIRVisitor

		#region Internals

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

			RegisterOperand ecx = new RegisterOperand(new SigType(CilElementType.I1), GeneralPurposeRegister.ECX);

			ctx.SetInstruction(CPUx86.Instruction.MovInstruction, ecx, op2);
			ctx.InsertInstructionAfter(CPUx86.Instruction.MovInstruction, opRes, op1);
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

				ctx.InsertBefore().SetInstruction(CPUx86.Instruction.MovInstruction, eax, op1);
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

			ctx.InsertBefore().SetInstruction(CPUx86.Instruction.MovInstruction, eax, ctx.Operand2);
			ctx.InsertInstructionAfter(CPUx86.Instruction.MovInstruction, opRes, eax);

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
			RegisterOperand eax = new RegisterOperand(opRes.Type, opRes.StackType == StackTypeCode.F ? (Register)SSE2Register.XMM0 : GeneralPurposeRegister.EAX);
			RegisterOperand eaxL = new RegisterOperand(op1.Type, GeneralPurposeRegister.EAX);

			if (instruction != null)
				ctx.SetInstruction(instruction, eax, op2);
			else {
				ctx.Result = eax;
				ctx.Operand1 = eax;
			}

			// Check if we have to sign-extend the operand that's being loaded
			if (IsSigned(op1) && !(op1 is ConstantOperand)) {
				// Sign extend it
				ctx.InsertBefore().SetInstruction(CPUx86.Instruction.MovsxInstruction, eaxL, op1);
			}
			// Check if the operand has to be zero-extended
			else if (IsUnsigned(op1) && !(op1 is ConstantOperand) && op1.StackType != StackTypeCode.F) {
				ctx.InsertBefore().SetInstruction(CPUx86.Instruction.MovzxInstruction, eaxL, op1);
			}
			// In any other case: Just load it
			else
				ctx.InsertBefore().SetInstruction(CPUx86.Instruction.MovInstruction, eax, op1);

			ctx.InsertInstructionAfter(CPUx86.Instruction.MovInstruction, opRes, eax);
		}
		#endregion // Internals
	}
}
