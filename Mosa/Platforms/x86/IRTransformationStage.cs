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
	public sealed class IRTransformationStage : BaseTransformationStage, IR.IIRVisitor, IMethodCompilerStage, IPlatformTransformationStage, IPipelineStage
	{

		#region IMethodCompilerStage Members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		string IPipelineStage.Name { get { return @"X86.IRTransformationStage"; } }

		/// <summary>
		/// Gets the pipeline stage order.
		/// </summary>
		/// <value>The pipeline stage order.</value>
		PipelineStageOrder[] IPipelineStage.PipelineStageOrder
		{
			get
			{
				return PipelineStageOrder.CreatePipelineOrder(typeof(CILTransformationStage), typeof(TweakTransformationStage));
			}
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
			ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, opRes, eax);
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
				ctx.AppendInstruction(CPUx86.Instruction.PopInstruction, ebp);
				// ret
				ctx.AppendInstruction(IR.Instruction.ReturnInstruction);
			}
			else {
				// pop edx
				ctx.SetInstruction(CPUx86.Instruction.PopInstruction, new RegisterOperand(I, GeneralPurposeRegister.EDX));
				// add esp, -localsSize
				ctx.AppendInstruction(CPUx86.Instruction.AddInstruction, esp, new ConstantOperand(I, -stackSize));
				// pop ebp
				ctx.AppendInstruction(CPUx86.Instruction.PopInstruction, ebp);
				// ret
				ctx.AppendInstruction(CPUx86.Instruction.RetInstruction); // Change to Return
			}
		}

		/// <summary>
		/// Floatings the point compare instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.FloatingPointCompareInstruction(Context ctx)
		{
			Operand op0 = ctx.Result;
			Operand source = EmitConstant(ctx.Operand1);
			Operand destination = EmitConstant(ctx.Operand2);
			IR.ConditionCode setcc = IR.ConditionCode.Equal;
			IR.ConditionCode code = ctx.ConditionCode;
			ctx.Remove();

			// Swap the operands if necessary...
			if (source is MemoryOperand && destination is RegisterOperand) {
				SwapComparisonOperands(ctx);
				source = ctx.Operand1;
				destination = ctx.Operand2;
			}

			// x86 is messed up :(
			switch (code) {
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
				ctx.AppendInstruction(CPUx86.Instruction.Cvtsd2ssInstruction, rop, destination);
				destination = rop;
			}
			if (source.Type.Type == CilElementType.R8 && destination.Type.Type == CilElementType.R4) {
				RegisterOperand rop = new RegisterOperand(new SigType(CilElementType.R4), SSE2Register.XMM0);
				ctx.AppendInstruction(CPUx86.Instruction.Cvtsd2ssInstruction, rop, source);
				source = rop;
			}

			if (source.Type.Type == CilElementType.R4) {
				switch (code) {
					case IR.ConditionCode.Equal:
						ctx.AppendInstruction(CPUx86.Instruction.UcomissInstruction, source, destination);
						break;
					case IR.ConditionCode.NotEqual: goto case IR.ConditionCode.Equal;
					case IR.ConditionCode.UnsignedGreaterOrEqual: goto case IR.ConditionCode.Equal;
					case IR.ConditionCode.UnsignedGreaterThan: goto case IR.ConditionCode.Equal;
					case IR.ConditionCode.UnsignedLessOrEqual: goto case IR.ConditionCode.Equal;
					case IR.ConditionCode.UnsignedLessThan: goto case IR.ConditionCode.Equal;
					case IR.ConditionCode.GreaterOrEqual:
						ctx.AppendInstruction(CPUx86.Instruction.ComissInstruction, source, destination);
						break;
					case IR.ConditionCode.GreaterThan: goto case IR.ConditionCode.GreaterOrEqual;
					case IR.ConditionCode.LessOrEqual: goto case IR.ConditionCode.GreaterOrEqual;
					case IR.ConditionCode.LessThan: goto case IR.ConditionCode.GreaterOrEqual;
				}
			}
			else {
				switch (code) {
					case IR.ConditionCode.Equal:
						ctx.AppendInstruction(CPUx86.Instruction.UcomisdInstruction, source, destination);
						break;
					case IR.ConditionCode.NotEqual: goto case IR.ConditionCode.Equal;
					case IR.ConditionCode.UnsignedGreaterOrEqual: goto case IR.ConditionCode.Equal;
					case IR.ConditionCode.UnsignedGreaterThan: goto case IR.ConditionCode.Equal;
					case IR.ConditionCode.UnsignedLessOrEqual: goto case IR.ConditionCode.Equal;
					case IR.ConditionCode.UnsignedLessThan: goto case IR.ConditionCode.Equal;
					case IR.ConditionCode.GreaterOrEqual:
						ctx.AppendInstruction(CPUx86.Instruction.ComisdInstruction, source, destination);
						break;
					case IR.ConditionCode.GreaterThan: goto case IR.ConditionCode.GreaterOrEqual;
					case IR.ConditionCode.LessOrEqual: goto case IR.ConditionCode.GreaterOrEqual;
					case IR.ConditionCode.LessThan: goto case IR.ConditionCode.GreaterOrEqual;
				}
			}

			// Determine the result
			ctx.AppendInstruction(CPUx86.Instruction.SetccInstruction, setcc, op0);

			// Extend this to the full register, if we're storing it in a register
			if (op0 is RegisterOperand) {
				RegisterOperand rop = new RegisterOperand(new SigType(CilElementType.U1), ((RegisterOperand)op0).Register);
				ctx.AppendInstruction(CPUx86.Instruction.MovzxInstruction, op0, rop);
			}

		}

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.IntegerCompareInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.IntegerCompareInstruction(Context ctx)
		{
			EmitOperandConstants(ctx);

			IR.ConditionCode condition = ctx.ConditionCode;

			ctx.SetInstruction(CPUx86.Instruction.CmpInstruction, ctx.Result, ctx.Operand1);

			if (IsUnsigned(ctx.Result))
				ctx.AppendInstruction(CPUx86.Instruction.SetccInstruction, GetUnsignedConditionCode(condition), ctx.Result);
			else
				ctx.AppendInstruction(CPUx86.Instruction.SetccInstruction, condition, ctx.Result);

			if (ctx.Result is RegisterOperand) {
				RegisterOperand rop = new RegisterOperand(new SigType(CilElementType.U1), ((RegisterOperand)ctx.Result).Register);
				ctx.AppendInstruction(CPUx86.Instruction.MovzxInstruction, rop, rop);
			}
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
			ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, eax, new MemoryOperand(ctx.Result.Type, GeneralPurposeRegister.EAX, IntPtr.Zero));
			ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, ctx.Result, eax);
		}

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.LogicalAndInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.LogicalAndInstruction(Context ctx)
		{
			ctx.ReplaceInstructionOnly(CPUx86.Instruction.AndInstruction);
		}

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.LogicalOrInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.LogicalOrInstruction(Context ctx)
		{
			ctx.ReplaceInstructionOnly(CPUx86.Instruction.OrInstruction);
		}

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.LogicalXorInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.LogicalXorInstruction(Context ctx)
		{
			ctx.ReplaceInstructionOnly(CPUx86.Instruction.XorInstruction);
		}

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.LogicalNotInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.LogicalNotInstruction(Context ctx)
		{
			Operand dest = ctx.Operand1;

			if (dest.Type.Type == CilElementType.U1)
				ctx.SetInstruction(CPUx86.Instruction.XorInstruction, dest, new ConstantOperand(new SigType(CilElementType.U4), (uint)0xFF));
			else if (dest.Type.Type == CilElementType.U2)
				ctx.SetInstruction(CPUx86.Instruction.XorInstruction, dest, new ConstantOperand(new SigType(CilElementType.U4), (uint)0xFFFF));
			else
				ctx.SetInstruction(CPUx86.Instruction.NotInstruction, dest);

		}

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.MoveInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.MoveInstruction(Context ctx)
		{
			EmitConstant(ctx.Operand1);
			ctx.ReplaceInstructionOnly(CPUx86.Instruction.MovInstruction);
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
			//ctx.SetInstruction(CPUx86.Instruction.DebugInstruction);
			//ctx.AppendInstruction(CPUx86.Instruction.PushInstruction, null, ebp);

			// Uncomment this line to enable breakpoints within Bochs
			//ctx.XXX(CPUx86.Instruction.BochsDebug);

			// push ebp
			ctx.SetInstruction(CPUx86.Instruction.PushInstruction, null, ebp);
			// mov ebp, esp
			ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, ebp, esp);
			// sub esp, localsSize
			ctx.AppendInstruction(CPUx86.Instruction.SubInstruction, esp, new ConstantOperand(I, -stackSize));
			// Initialize all locals to zero
			ctx.AppendInstruction(CPUx86.Instruction.PushInstruction, null, edi);
			ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, edi, esp);
			ctx.AppendInstruction(CPUx86.Instruction.PushInstruction, null, ecx);
			ctx.AppendInstruction(CPUx86.Instruction.AddInstruction, edi, new ConstantOperand(I, 4));
			ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, ecx, new ConstantOperand(I, (-stackSize) / 4));
			ctx.AppendInstruction(CPUx86.Instruction.XorInstruction, eax, eax);
			ctx.AppendInstruction(CPUx86.Instruction.RepInstruction);
			ctx.AppendInstruction(CPUx86.Instruction.StosdInstruction);
			ctx.AppendInstruction(CPUx86.Instruction.PopInstruction, ecx);
			ctx.AppendInstruction(CPUx86.Instruction.PopInstruction, edi);
			/*
			 * This move adds the runtime method identification token onto the stack. This
			 * allows us to perform call stack identification and gives the garbage collector 
			 * the possibility to identify roots into the managed heap. 
			 */
			// mov [ebp-4], token
			ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, new MemoryOperand(I, GeneralPurposeRegister.EBP, new IntPtr(-4)), new ConstantOperand(I, Compiler.Method.Token));

			// Do not save EDX for non-int64 return values
			if (Compiler.Method.Signature.ReturnType.Type != CilElementType.I8 &&
				Compiler.Method.Signature.ReturnType.Type != CilElementType.U8) {
				// push edx
				ctx.AppendInstruction(CPUx86.Instruction.PushInstruction, null, new RegisterOperand(I, GeneralPurposeRegister.EDX));
			}
		}

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.ReturnInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.ReturnInstruction(Context ctx)
		{
			Operand op = ctx.Operand1;

			//ctx.Remove();

			if (op != null) {
				ICallingConvention cc = Architecture.GetCallingConvention(Compiler.Method.Signature.CallingConvention);
				cc.MoveReturnValue(ctx, op);
			}

			ctx.AppendInstruction(CPUx86.Instruction.JmpInstruction);
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
			ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, edx, operand1);
			ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, new MemoryOperand(operand1.Type, GeneralPurposeRegister.EAX, IntPtr.Zero), edx);
		}

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.UDivInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.UDivInstruction(Context ctx)
		{
			//Operand op1 = ctx.Operand1;
			//Operand op2 = ctx.Operand2;

			//RegisterOperand edx = new RegisterOperand(new SigType(CilElementType.U4), GeneralPurposeRegister.EDX);
			//ctx.InsertInstructionAfter(CPUx86.Instruction.XorInstruction, edx, edx);
			//ctx.InsertInstructionAfter(CPUx86.Instruction.DivInstruction, op1, op2);
		}

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.URemInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.URemInstruction(Context ctx)
		{
			ctx.ReplaceInstructionOnly(IR.Instruction.UDivInstruction);
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
					break;
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

			if ((ctx.Result is RegisterOperand))
				return;

			Operand result = ctx.Result;
			Operand source = ctx.Operand1;
			RegisterOperand ebx = new RegisterOperand(result.Type, GeneralPurposeRegister.EBX);
			ctx.Result = ebx;

			ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, result, ebx);
		}

		#endregion //  IIRVisitor

		#region IIRVisitor - Unused

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.BranchInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.BranchInstruction(Context context)
		{
			switch (context.ConditionCode) {
				case IR.ConditionCode.Equal:
					context.ReplaceInstructionOnly(CPUx86.Instruction.JeInstruction);
					break;

				case IR.ConditionCode.GreaterOrEqual:
					context.ReplaceInstructionOnly(CPUx86.Instruction.JgeInstruction);
					break;

				case IR.ConditionCode.GreaterThan:
					context.ReplaceInstructionOnly(CPUx86.Instruction.JgInstruction);
					break;

				case IR.ConditionCode.LessOrEqual:
					context.ReplaceInstructionOnly(CPUx86.Instruction.JleInstruction);
					break;

				case IR.ConditionCode.LessThan:
					context.ReplaceInstructionOnly(CPUx86.Instruction.JlInstruction);
					break;

				case IR.ConditionCode.NotEqual:
					context.ReplaceInstructionOnly(CPUx86.Instruction.JneInstruction);
					break;

				case IR.ConditionCode.UnsignedGreaterOrEqual:
					context.ReplaceInstructionOnly(CPUx86.Instruction.JaeInstruction);
					break;

				case IR.ConditionCode.UnsignedGreaterThan:
					context.ReplaceInstructionOnly(CPUx86.Instruction.JaInstruction);
					break;

				case IR.ConditionCode.UnsignedLessOrEqual:
					context.ReplaceInstructionOnly(CPUx86.Instruction.JbeInstruction);
					break;

				case IR.ConditionCode.UnsignedLessThan:
					context.ReplaceInstructionOnly(CPUx86.Instruction.JbInstruction);
					break;

				default:
					throw new NotSupportedException();
			}
		}

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
		void IR.IIRVisitor.PopInstruction(Context context)
		{
			context.ReplaceInstructionOnly(CPUx86.Instruction.PopInstruction);
		}

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.PushInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.PushInstruction(Context context)
		{
			context.ReplaceInstructionOnly(CPUx86.Instruction.PushInstruction);
		}

		#endregion // IIRVisitor

		#region Internals

		/// <summary>
		/// Special handling for shift operations, which require the shift amount in the ECX or as a constant register.
		/// </summary>
		/// <param name="ctx">The transformation context.</param>
		/// <param name="instruction">The instruction to transform.</param>
		private void HandleShiftOperation(Context ctx, IInstruction instruction)
		{
			EmitOperandConstants(ctx);
			ctx.ReplaceInstructionOnly(instruction);
		}


		/// <summary>
		/// Swaps the comparison operands.
		/// </summary>
		/// <param name="ctx">The context.</param>
		private static void SwapComparisonOperands(Context ctx)
		{
			Operand op1 = ctx.Operand1;
			ctx.Operand2 = op1;
			ctx.Operand1 = ctx.Operand2;

			// Negate the condition code if necessary...
			switch (ctx.ConditionCode) {
				case IR.ConditionCode.Equal: break;
				case IR.ConditionCode.GreaterOrEqual: ctx.ConditionCode = IR.ConditionCode.LessThan; break;
				case IR.ConditionCode.GreaterThan: ctx.ConditionCode = IR.ConditionCode.LessOrEqual; break;
				case IR.ConditionCode.LessOrEqual: ctx.ConditionCode = IR.ConditionCode.GreaterThan; break;
				case IR.ConditionCode.LessThan: ctx.ConditionCode = IR.ConditionCode.GreaterOrEqual; break;
				case IR.ConditionCode.NotEqual: break;
				case IR.ConditionCode.UnsignedGreaterOrEqual: ctx.ConditionCode = IR.ConditionCode.UnsignedLessThan; break;
				case IR.ConditionCode.UnsignedGreaterThan: ctx.ConditionCode = IR.ConditionCode.UnsignedLessOrEqual; break;
				case IR.ConditionCode.UnsignedLessOrEqual: ctx.ConditionCode = IR.ConditionCode.UnsignedGreaterThan; break;
				case IR.ConditionCode.UnsignedLessThan: ctx.ConditionCode = IR.ConditionCode.UnsignedGreaterOrEqual; break;
			}
		}

		#endregion // Internals
	}
}
