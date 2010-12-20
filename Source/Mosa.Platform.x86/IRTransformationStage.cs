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
using Mosa.Runtime.CompilerFramework.IR;
using Mosa.Runtime.CompilerFramework.Operands;
using Mosa.Runtime.Linker;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Vm;
using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Platform.x86
{
	/// <summary>
	/// Transforms IR instructions into their appropriate X86.
	/// </summary>
	/// <remarks>
	/// This transformation stage transforms IR instructions into their equivalent X86 sequences.
	/// </remarks>
	public sealed class IRTransformationStage : BaseTransformationStage, IR.IIRVisitor, IMethodCompilerStage, IPlatformStage, IPipelineStage
	{
		#region IPipelineStage Members
		bool exceptionHandlingCompiled = false;
		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		string IPipelineStage.Name { get { return @"X86.IRTransformationStage"; } }

		#endregion // IMethodCompilerStage Members

		#region IIRVisitor

		/// <summary>
		/// Visitation function for AddSInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.AddSInstruction(Context context)
		{
			HandleCommutativeOperation(context, CPUx86.Instruction.AddInstruction);
		}

		/// <summary>
		/// Visitation function for AddUInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.AddUInstruction(Context context)
		{
			HandleCommutativeOperation(context, CPUx86.Instruction.AddInstruction);
		}

		/// <summary>
		/// Visitation function for AddFInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.AddFInstruction(Context context)
		{
			HandleCommutativeOperation(context, CPUx86.Instruction.SseAddInstruction);
			ExtendToR8(context);
		}

		/// <summary>
		/// Visitation function for DivFInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.DivFInstruction(Context context)
		{
			HandleNonCommutativeOperation(context, CPUx86.Instruction.SseDivInstruction);
			ExtendToR8(context);
		}

		/// <summary>
		/// Visitation function for DivSInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.DivSInstruction(Context context)
		{
			HandleNonCommutativeOperation(context, CPUx86.Instruction.DivInstruction);
		}

		/// <summary>
		/// Addresses the of instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.AddressOfInstruction(Context context)
		{
			Operand opRes = context.Result;
			RegisterOperand eax = new RegisterOperand(opRes.Type, GeneralPurposeRegister.EAX);
			context.Result = eax;
			context.ReplaceInstructionOnly(CPUx86.Instruction.LeaInstruction);
			//context.Ignore = true;
			context.AppendInstruction(CPUx86.Instruction.MovInstruction, opRes, eax);
		}

		/// <summary>
		/// Arithmetics the shift right instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.ArithmeticShiftRightInstruction(Context context)
		{
			HandleShiftOperation(context, CPUx86.Instruction.SarInstruction);
		}

		/// <summary>
		/// Visitation function for EpilogueInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.EpilogueInstruction(Context context)
		{
			SigType I = new SigType(CilElementType.I);
			RegisterOperand ebx = new RegisterOperand(I, GeneralPurposeRegister.EBX);
			RegisterOperand edx = new RegisterOperand(I, GeneralPurposeRegister.EDX);
			RegisterOperand ebp = new RegisterOperand(I, GeneralPurposeRegister.EBP);
			RegisterOperand esp = new RegisterOperand(I, GeneralPurposeRegister.ESP);
			int stackSize = (int)context.Other;

			// Pop callee's EIP that has been used for instruction handling
			context.SetInstruction(CPUx86.Instruction.PopInstruction, ebx);

			if (methodCompiler.Method.Signature.ReturnType.Type == CilElementType.I8
				|| methodCompiler.Method.Signature.ReturnType.Type == CilElementType.U8)
			{
				// pop ebx
				context.AppendInstruction(CPUx86.Instruction.PopInstruction, ebx);
			}
			else
			{
				// pop edx
				context.AppendInstruction(CPUx86.Instruction.PopInstruction, edx);
				// pop ebx
				context.AppendInstruction(CPUx86.Instruction.PopInstruction, ebx);
			}

			// add esp, -localsSize
			context.AppendInstruction(CPUx86.Instruction.AddInstruction, esp, new ConstantOperand(I, -stackSize));
			// pop ebp
			context.AppendInstruction(CPUx86.Instruction.PopInstruction, ebp);
			// ret
			context.AppendInstruction(CPUx86.Instruction.RetInstruction);
		}

		/// <summary>
		/// Floatings the point compare instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.FloatingPointCompareInstruction(Context context)
		{
			Operand resultOperand = context.Result;
			Operand left = EmitConstant(context.Operand1);
			Operand right = EmitConstant(context.Operand2);
			//context.Remove();
			context.Operand1 = left;
			context.Operand2 = right;


			// Swap the operands if necessary...
			if (left is MemoryOperand && right is RegisterOperand)
			{
				SwapComparisonOperands(context);
				left = context.Operand1;
				right = context.Operand2;
			}

			IR.ConditionCode setp = IR.ConditionCode.Parity;
			IR.ConditionCode setnp = IR.ConditionCode.NoParity;
			IR.ConditionCode setnc = IR.ConditionCode.NoCarry;
			IR.ConditionCode setc = IR.ConditionCode.Carry;
			IR.ConditionCode setnz = IR.ConditionCode.NoZero;
			IR.ConditionCode setz = IR.ConditionCode.Zero;
			IR.ConditionCode code = context.ConditionCode;

			context.SetInstruction(CPUx86.Instruction.NopInstruction);

			// x86 is messed up :(
			switch (code)
			{
				case IR.ConditionCode.Equal: break;
				case IR.ConditionCode.NotEqual: break;
				case IR.ConditionCode.UnsignedGreaterOrEqual: code = IR.ConditionCode.GreaterOrEqual; break;
				case IR.ConditionCode.UnsignedGreaterThan: code = IR.ConditionCode.GreaterThan; break;
				case IR.ConditionCode.UnsignedLessOrEqual: code = IR.ConditionCode.LessOrEqual; break;
				case IR.ConditionCode.UnsignedLessThan: code = IR.ConditionCode.LessThan; break;
			}

			if (!(left is RegisterOperand))
			{
				RegisterOperand xmm2 = new RegisterOperand(left.Type, SSE2Register.XMM2);
				if (left.Type.Type == CilElementType.R4)
					context.AppendInstruction(CPUx86.Instruction.MovssInstruction, xmm2, left);
				else
					context.AppendInstruction(CPUx86.Instruction.MovsdInstruction, xmm2, left);
				left = xmm2;
			}

			// Compare using the smallest precision
			if (left.Type.Type == CilElementType.R4 && right.Type.Type == CilElementType.R8)
			{
				RegisterOperand rop = new RegisterOperand(new SigType(CilElementType.R4), SSE2Register.XMM4);
				context.AppendInstruction(CPUx86.Instruction.Cvtsd2ssInstruction, rop, right);
				right = rop;
			}
			if (left.Type.Type == CilElementType.R8 && right.Type.Type == CilElementType.R4)
			{
				RegisterOperand rop = new RegisterOperand(new SigType(CilElementType.R4), SSE2Register.XMM3);
				context.AppendInstruction(CPUx86.Instruction.Cvtsd2ssInstruction, rop, left);
				left = rop;
			}

			if (left.Type.Type == CilElementType.R4)
			{
				switch (code)
				{
					case IR.ConditionCode.Equal:
						context.AppendInstruction(CPUx86.Instruction.ComissInstruction, left, right);
						break;
					case IR.ConditionCode.NotEqual: goto case IR.ConditionCode.Equal;
					case IR.ConditionCode.UnsignedGreaterOrEqual: goto case IR.ConditionCode.Equal;
					case IR.ConditionCode.UnsignedGreaterThan: goto case IR.ConditionCode.Equal;
					case IR.ConditionCode.UnsignedLessOrEqual: goto case IR.ConditionCode.Equal;
					case IR.ConditionCode.UnsignedLessThan: goto case IR.ConditionCode.Equal;
					case IR.ConditionCode.GreaterOrEqual:
						context.AppendInstruction(CPUx86.Instruction.ComissInstruction, left, right);
						break;
					case IR.ConditionCode.GreaterThan: goto case IR.ConditionCode.GreaterOrEqual;
					case IR.ConditionCode.LessOrEqual: goto case IR.ConditionCode.GreaterOrEqual;
					case IR.ConditionCode.LessThan: goto case IR.ConditionCode.GreaterOrEqual;
				}
			}
			else
			{
				switch (code)
				{
					case IR.ConditionCode.Equal:
						context.AppendInstruction(CPUx86.Instruction.ComisdInstruction, left, right);
						break;
					case IR.ConditionCode.NotEqual: goto case IR.ConditionCode.Equal;
					case IR.ConditionCode.UnsignedGreaterOrEqual: goto case IR.ConditionCode.Equal;
					case IR.ConditionCode.UnsignedGreaterThan: goto case IR.ConditionCode.Equal;
					case IR.ConditionCode.UnsignedLessOrEqual: goto case IR.ConditionCode.Equal;
					case IR.ConditionCode.UnsignedLessThan: goto case IR.ConditionCode.Equal;
					case IR.ConditionCode.GreaterOrEqual:
						context.AppendInstruction(CPUx86.Instruction.ComisdInstruction, left, right);
						break;
					case IR.ConditionCode.GreaterThan: goto case IR.ConditionCode.GreaterOrEqual;
					case IR.ConditionCode.LessOrEqual: goto case IR.ConditionCode.GreaterOrEqual;
					case IR.ConditionCode.LessThan: goto case IR.ConditionCode.GreaterOrEqual;
				}
			}

			// Determine the result
			RegisterOperand eax = new RegisterOperand(BuiltInSigType.Byte, GeneralPurposeRegister.EAX);
			RegisterOperand ebx = new RegisterOperand(BuiltInSigType.Byte, GeneralPurposeRegister.EBX);
			RegisterOperand ecx = new RegisterOperand(BuiltInSigType.Byte, GeneralPurposeRegister.ECX);
			RegisterOperand edx = new RegisterOperand(BuiltInSigType.Byte, GeneralPurposeRegister.EDX);

			if (code == IR.ConditionCode.Equal)
			{
				context.AppendInstruction(CPUx86.Instruction.SetccInstruction, setz, eax);
				context.AppendInstruction(CPUx86.Instruction.SetccInstruction, setnp, ebx);
				context.AppendInstruction(CPUx86.Instruction.SetccInstruction, setnc, ecx);
				context.AppendInstruction(CPUx86.Instruction.AndInstruction, eax, ebx);
				context.AppendInstruction(CPUx86.Instruction.AndInstruction, eax, ecx);
			}
			else if (code == IR.ConditionCode.NotEqual)
			{
				context.AppendInstruction(CPUx86.Instruction.SetccInstruction, setz, eax);
				context.AppendInstruction(CPUx86.Instruction.SetccInstruction, setnp, ebx);
				context.AppendInstruction(CPUx86.Instruction.SetccInstruction, setnc, ecx);
				context.AppendInstruction(CPUx86.Instruction.AndInstruction, eax, ebx);
				context.AppendInstruction(CPUx86.Instruction.AndInstruction, eax, ecx);
				context.AppendInstruction(CPUx86.Instruction.NotInstruction, eax, eax);
				context.AppendInstruction(CPUx86.Instruction.AndInstruction, eax, new ConstantOperand(new SigType(CilElementType.I4), (int)1));
				context.AppendInstruction(CPUx86.Instruction.SetccInstruction, setz, ebx);
				context.AppendInstruction(CPUx86.Instruction.SetccInstruction, setp, ecx);
				context.AppendInstruction(CPUx86.Instruction.SetccInstruction, setc, edx);
				context.AppendInstruction(CPUx86.Instruction.AndInstruction, ebx, ecx);
				context.AppendInstruction(CPUx86.Instruction.AndInstruction, ebx, edx);
				context.AppendInstruction(CPUx86.Instruction.OrInstruction, eax, ebx);
			}
			else if (code == IR.ConditionCode.GreaterThan || code == IR.ConditionCode.GreaterOrEqual)
			{
				context.AppendInstruction(CPUx86.Instruction.SetccInstruction, setnz, eax);
				context.AppendInstruction(CPUx86.Instruction.SetccInstruction, setnp, ebx);
				context.AppendInstruction(CPUx86.Instruction.SetccInstruction, setnc, ecx);
				context.AppendInstruction(CPUx86.Instruction.AndInstruction, eax, ebx);
				context.AppendInstruction(CPUx86.Instruction.AndInstruction, eax, ecx);
			}
			else if (code == IR.ConditionCode.LessThan || code == IR.ConditionCode.LessOrEqual)
			{
				context.AppendInstruction(CPUx86.Instruction.SetccInstruction, setnz, eax);
				context.AppendInstruction(CPUx86.Instruction.SetccInstruction, setnp, ebx);
				context.AppendInstruction(CPUx86.Instruction.SetccInstruction, setc, ecx);
				context.AppendInstruction(CPUx86.Instruction.AndInstruction, eax, ebx);
				context.AppendInstruction(CPUx86.Instruction.AndInstruction, eax, ecx);
			}
			if (code == IR.ConditionCode.GreaterOrEqual || code == IR.ConditionCode.LessOrEqual)
			{
				context.AppendInstruction(CPUx86.Instruction.SetccInstruction, setz, edx);
				context.AppendInstruction(CPUx86.Instruction.OrInstruction, eax, edx);
			}
			context.AppendInstruction(CPUx86.Instruction.MovzxInstruction, resultOperand, eax);
		}

		/// <summary>
		/// Visitation function for IntegerCompareInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.IntegerCompareInstruction(Context context)
		{
			EmitOperandConstants(context);

			IR.ConditionCode condition = context.ConditionCode;
			Operand resultOperand = context.Result;

			context.SetInstruction(CPUx86.Instruction.CmpInstruction, context.Operand1, context.Operand2);

			if (resultOperand != null)
			{
				RegisterOperand eax = new RegisterOperand(BuiltInSigType.Byte, GeneralPurposeRegister.EAX);

				if (IsUnsigned(resultOperand))
					context.AppendInstruction(CPUx86.Instruction.SetccInstruction, GetUnsignedConditionCode(condition), eax);
				else
					context.AppendInstruction(CPUx86.Instruction.SetccInstruction, condition, eax);

				context.AppendInstruction(CPUx86.Instruction.MovzxInstruction, resultOperand, eax);
			}
		}

		/// <summary>
		/// Visitation function for JmpInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.JmpInstruction(Context context)
		{
			context.ReplaceInstructionOnly(CPUx86.Instruction.JmpInstruction);
		}

		/// <summary>
		/// Visitation function for LoadInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.LoadInstruction(Context context)
		{
			RegisterOperand eax = new RegisterOperand(context.Operand1.Type, GeneralPurposeRegister.EAX);
			Operand result = context.Result;
			Operand operand = context.Operand1;
			Operand offset = context.Operand2;
			ConstantOperand constantOffset = offset as ConstantOperand;
			IntPtr offsetPtr = IntPtr.Zero;

			context.SetInstruction(CPUx86.Instruction.MovInstruction, eax, operand);
			if (constantOffset != null)
			{
				offsetPtr = new IntPtr(Convert.ToInt64(constantOffset.Value));
			}
			else
			{
				context.AppendInstruction(CPUx86.Instruction.AddInstruction, eax, offset);
			}

			context.AppendInstruction(CPUx86.Instruction.MovInstruction, result, new MemoryOperand(eax.Type, GeneralPurposeRegister.EAX, offsetPtr));
		}

		/// <summary>
		/// Visitation function for LogicalAndInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.LogicalAndInstruction(Context context)
		{
			context.ReplaceInstructionOnly(CPUx86.Instruction.AndInstruction);
		}

		/// <summary>
		/// Visitation function for LogicalOrInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.LogicalOrInstruction(Context context)
		{
			context.ReplaceInstructionOnly(CPUx86.Instruction.OrInstruction);
		}

		/// <summary>
		/// Visitation function for LogicalXorInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.LogicalXorInstruction(Context context)
		{
			context.ReplaceInstructionOnly(CPUx86.Instruction.XorInstruction);
		}

		/// <summary>
		/// Visitation function for LogicalNotInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.LogicalNotInstruction(Context context)
		{
			Operand dest = context.Result;

			context.SetInstruction(CPUx86.Instruction.MovInstruction, context.Result, context.Operand1);
			if (dest.Type.Type == CilElementType.U1)
				context.AppendInstruction(CPUx86.Instruction.XorInstruction, dest, new ConstantOperand(new SigType(CilElementType.U4), (uint)0xFF));
			else if (dest.Type.Type == CilElementType.U2)
				context.AppendInstruction(CPUx86.Instruction.XorInstruction, dest, new ConstantOperand(new SigType(CilElementType.U4), (uint)0xFFFF));
			else
				context.AppendInstruction(CPUx86.Instruction.NotInstruction, dest);

		}

		/// <summary>
		/// Visitation function for MoveInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.MoveInstruction(Context context)
		{
			Operand result = context.Result;
			Operand operand = context.Operand1;
			context.Operand1 = EmitConstant(context.Operand1);

			if (context.Result.StackType == StackTypeCode.F)
			{
				Debug.Assert(context.Operand1.StackType == StackTypeCode.F, @"Move can't convert to floating point type.");
				if (context.Result.Type.Type == context.Operand1.Type.Type)
				{
					if (context.Result.Type.Type == CilElementType.R4)
						MoveFloatingPoint(context, CPUx86.Instruction.MovssInstruction);
					else if (context.Result.Type.Type == CilElementType.R8)
						MoveFloatingPoint(context, CPUx86.Instruction.MovsdInstruction);
				}
				else if (context.Result.Type.Type == CilElementType.R8)
				{
					context.SetInstruction(CPUx86.Instruction.Cvtss2sdInstruction, context.Result, context.Operand1, context.Operand2);
				}
				else if (context.Result.Type.Type == CilElementType.R4)
				{
					context.SetInstruction(CPUx86.Instruction.Cvtsd2ssInstruction, context.Result, context.Operand1, context.Operand2);
				}
			}
			else
			{
				if (context.Result is MemoryOperand && context.Operand1 is MemoryOperand)
				{
					RegisterOperand load = new RegisterOperand(new SigType(CilElementType.I), GeneralPurposeRegister.EDX);
					RegisterOperand store = new RegisterOperand(operand.Type, GeneralPurposeRegister.EDX);

					if (!Is32Bit(operand) && IsSigned(operand))
						context.SetInstruction(CPUx86.Instruction.MovsxInstruction, load, operand);
					else if (!Is32Bit(operand) && IsUnsigned(operand))
						context.SetInstruction(CPUx86.Instruction.MovzxInstruction, load, operand);
					else
						context.SetInstruction(CPUx86.Instruction.MovInstruction, load, operand);

					context.AppendInstruction(CPUx86.Instruction.MovInstruction, result, store);
				}
				else
					context.ReplaceInstructionOnly(CPUx86.Instruction.MovInstruction);
			}
		}

		private void MoveFloatingPoint(Context context, CPUx86.BaseInstruction instruction)
		{
			RegisterOperand xmm0 = new RegisterOperand(context.Result.Type, SSE2Register.XMM0);
			Operand result = context.Result;
			Operand operand = context.Operand1;
			context.SetInstruction(instruction, xmm0, operand);
			context.AppendInstruction(instruction, result, xmm0);
		}

		/// <summary>
		/// Visitation function for PrologueInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.PrologueInstruction(Context context)
		{
			SigType I = new SigType(CilElementType.I4);
			RegisterOperand eax = new RegisterOperand(I, GeneralPurposeRegister.EAX);
			RegisterOperand ebx = new RegisterOperand(I, GeneralPurposeRegister.EBX);
			RegisterOperand ecx = new RegisterOperand(I, GeneralPurposeRegister.ECX);
			RegisterOperand ebp = new RegisterOperand(I, GeneralPurposeRegister.EBP);
			RegisterOperand esp = new RegisterOperand(I, GeneralPurposeRegister.ESP);
			RegisterOperand edi = new RegisterOperand(I, GeneralPurposeRegister.EDI);
			int stackSize = (int)context.Other;
			Debug.Assert((stackSize % 4) == 0, @"Stack size of method can't be divided by 4!!");

			/* If you want to stop at the _header of an emitted function, just uncomment
			 * the following line. It will issue a breakpoint instruction. Note that if
			 * you debug using visual studio you must enable unmanaged code debugging, 
			 * otherwise the function will never return and the breakpoint will never
			 * appear.
			 */
			// int 3
			//context.SetInstruction(CPUx86.Instruction.BreakInstruction);
			//context.AppendInstruction(CPUx86.Instruction.PushInstruction, null, ebp);
			//context.AppendInstruction(CPUx86.Instruction.NopInstruction);

			// Uncomment this line to enable breakpoints within Bochs
			//context.XXX(CPUx86.Instruction.BochsDebug);

			// push ebp
			context.SetInstruction(CPUx86.Instruction.PushInstruction, null, ebp);

			// mov ebp, esp
			context.AppendInstruction(CPUx86.Instruction.MovInstruction, ebp, esp);
			// sub esp, localsSize
			context.AppendInstruction(CPUx86.Instruction.SubInstruction, esp, new ConstantOperand(I, -stackSize));
			context.AppendInstruction(CPUx86.Instruction.PushInstruction, null, ebx);

			// Initialize all locals to zero
			context.AppendInstruction(CPUx86.Instruction.PushInstruction, null, edi);
			context.AppendInstruction(CPUx86.Instruction.MovInstruction, edi, esp);
			context.AppendInstruction(CPUx86.Instruction.PushInstruction, null, ecx);
			context.AppendInstruction(CPUx86.Instruction.AddInstruction, edi, new ConstantOperand(I, 8));
			context.AppendInstruction(CPUx86.Instruction.MovInstruction, ecx, new ConstantOperand(I, -(int)(stackSize >> 2)));
			context.AppendInstruction(CPUx86.Instruction.XorInstruction, eax, eax);
			context.AppendInstruction(CPUx86.Instruction.RepInstruction);
			context.AppendInstruction(CPUx86.Instruction.StosdInstruction);
			context.AppendInstruction(CPUx86.Instruction.PopInstruction, ecx);
			context.AppendInstruction(CPUx86.Instruction.PopInstruction, edi);
			/*
			 * This move adds the runtime method identification token onto the stack. This
			 * allows us to perform call stack identification and gives the garbage collector 
			 * the possibility to identify roots into the managed heap. 
			 */
			// mov [ebp-4], token
			context.AppendInstruction(CPUx86.Instruction.MovInstruction, new MemoryOperand(I, GeneralPurposeRegister.EBP, new IntPtr(-4)), new ConstantOperand(I, methodCompiler.Method.Token));

			// Do not save EDX for non-int64 return values
			if (methodCompiler.Method.Signature.ReturnType.Type != CilElementType.I8 &&
				methodCompiler.Method.Signature.ReturnType.Type != CilElementType.U8)
			{
				// push edx
				context.AppendInstruction(CPUx86.Instruction.PushInstruction, null, new RegisterOperand(I, GeneralPurposeRegister.EDX));
			}

			// Save callee's EIP to the stack for exception handling
			context.AppendInstruction(CPUx86.Instruction.PushInstruction, null, new ConstantOperand(I, (-stackSize) + 0x0C));

			//context.AppendInstruction(CPUx86.Instruction.BreakInstruction);
		}

		/// <summary>
		/// Visitation function for ReturnInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.ReturnInstruction(Context context)
		{
			Operand op = context.Operand1;

			if (op != null)
			{
				callingConvention.MoveReturnValue(context, op);
				context.AppendInstruction(CPUx86.Instruction.JmpInstruction);
				context.SetBranch(Int32.MaxValue);
			}
			else
			{
				context.SetInstruction(CPUx86.Instruction.JmpInstruction);
				context.SetBranch(Int32.MaxValue);
			}
		}

		/// <summary>
		/// Visitation function for ShiftLeftInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.ShiftLeftInstruction(Context context)
		{
			HandleShiftOperation(context, CPUx86.Instruction.ShlInstruction);
		}

		/// <summary>
		/// Visitation function for ShiftRightInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.ShiftRightInstruction(Context context)
		{
			HandleShiftOperation(context, CPUx86.Instruction.ShrInstruction);
		}

		/// <summary>
		/// Visitation function for StoreInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.StoreInstruction(Context context)
		{
			Operand destination = context.Result;
			Operand offset = context.Operand1;
			Operand value = context.Operand2;

			ConstantOperand constantOffset = offset as ConstantOperand;

			RegisterOperand eax = new RegisterOperand(destination.Type, GeneralPurposeRegister.EAX);
			RegisterOperand edx = new RegisterOperand(value.Type, GeneralPurposeRegister.EDX);

			context.SetInstruction(CPUx86.Instruction.MovInstruction, eax, destination);
			context.AppendInstruction(CPUx86.Instruction.MovInstruction, edx, value);

			IntPtr offsetPtr = IntPtr.Zero;
			if (constantOffset != null)
			{
				offsetPtr = new IntPtr(Convert.ToInt64(constantOffset.Value));
			}
			else
			{
				context.AppendInstruction(CPUx86.Instruction.AddInstruction, eax, offset);
			}

			context.AppendInstruction(CPUx86.Instruction.MovInstruction, new MemoryOperand(value.Type, GeneralPurposeRegister.EAX, offsetPtr), edx);
		}

		/// <summary>
		/// Visitation function for DivUInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.DivUInstruction(Context context)
		{
			context.ReplaceInstructionOnly(CPUx86.Instruction.UDivInstruction);
		}

		/// <summary>
		/// Visitation function for MulSInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.MulSInstruction(Context context)
		{
			HandleCommutativeOperation(context, CPUx86.Instruction.MulInstruction);
		}

		/// <summary>
		/// Visitation function for MulFInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.MulFInstruction(Context context)
		{
			HandleCommutativeOperation(context, CPUx86.Instruction.SseMulInstruction);
			ExtendToR8(context);
		}

		/// <summary>
		/// Visitation function for MulUInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.MulUInstruction(Context context)
		{
			HandleCommutativeOperation(context, CPUx86.Instruction.MulInstruction);
		}

		/// <summary>
		/// Visitation function for SubFInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.SubFInstruction(Context context)
		{
			HandleNonCommutativeOperation(context, CPUx86.Instruction.SseSubInstruction);
			ExtendToR8(context);
		}

		/// <summary>
		/// Visitation function for SubSInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.SubSInstruction(Context context)
		{
			HandleNonCommutativeOperation(context, CPUx86.Instruction.SubInstruction);
		}

		/// <summary>
		/// Visitation function for SubUInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.SubUInstruction(Context context)
		{
			HandleNonCommutativeOperation(context, CPUx86.Instruction.SubInstruction);
		}

		/// <summary>
		/// Visitation function for RemFInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.RemFInstruction(Context context)
		{
			HandleNonCommutativeOperation(context, CPUx86.Instruction.SseDivInstruction);
			ExtendToR8(context);

			Operand destination = context.Result;
			Operand source = context.Operand1;

			Context[] newBlocks = CreateEmptyBlockContexts(context.Label, 3);
			Context nextBlock = SplitContext(context, false);

			RegisterOperand xmm5 = new RegisterOperand(new SigType(CilElementType.R8), SSE2Register.XMM5);
			RegisterOperand xmm6 = new RegisterOperand(new SigType(CilElementType.R8), SSE2Register.XMM6);
			RegisterOperand eax = new RegisterOperand(new SigType(CilElementType.I4), GeneralPurposeRegister.EAX);
			RegisterOperand edx = new RegisterOperand(new SigType(CilElementType.I4), GeneralPurposeRegister.EDX);
			RegisterOperand uedx = new RegisterOperand(new SigType(CilElementType.U4), GeneralPurposeRegister.EDX);
			Context before = context.InsertBefore();

			context.SetInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[0].BasicBlock);
			LinkBlocks(context, newBlocks[0]);

			newBlocks[0].SetInstruction(CPUx86.Instruction.MovsdInstruction, xmm5, source);
			newBlocks[0].AppendInstruction(CPUx86.Instruction.MovsdInstruction, xmm6, destination);

			newBlocks[0].AppendInstruction(CPUx86.Instruction.SseDivInstruction, destination, source);
			newBlocks[0].AppendInstruction(CPUx86.Instruction.Cvttsd2siInstruction, edx, destination);

			newBlocks[0].AppendInstruction(CPUx86.Instruction.CmpInstruction, edx, new ConstantOperand(new SigType(CilElementType.I4), 0));
			newBlocks[0].AppendInstruction(CPUx86.Instruction.BranchInstruction, IR.ConditionCode.Equal, newBlocks[2].BasicBlock);
			newBlocks[0].AppendInstruction(CPUx86.Instruction.JmpInstruction, newBlocks[1].BasicBlock);
			LinkBlocks(newBlocks[0], newBlocks[1], newBlocks[2]);

			newBlocks[1].AppendInstruction(CPUx86.Instruction.Cvtsi2sdInstruction, destination, edx);
			newBlocks[1].AppendInstruction(CPUx86.Instruction.SseMulInstruction, destination, xmm5);
			newBlocks[1].AppendInstruction(CPUx86.Instruction.SseSubInstruction, xmm6, destination);
			newBlocks[1].AppendInstruction(CPUx86.Instruction.MovsdInstruction, destination, xmm6);
			newBlocks[1].AppendInstruction(CPUx86.Instruction.JmpInstruction, nextBlock.BasicBlock);
			LinkBlocks(newBlocks[1], nextBlock);

			newBlocks[2].SetInstruction(CPUx86.Instruction.MovsdInstruction, destination, xmm6);
			newBlocks[2].AppendInstruction(CPUx86.Instruction.JmpInstruction, nextBlock.BasicBlock);
			LinkBlocks(newBlocks[2], nextBlock);
		}

		/// <summary>
		/// Visitation function for RemSInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.RemSInstruction(Context context)
		{
			Operand result = context.Result;
			Operand operand = context.Operand1;
			RegisterOperand eax = new RegisterOperand(new SigType(CilElementType.I4), GeneralPurposeRegister.EAX);
			RegisterOperand ecx = new RegisterOperand(new SigType(CilElementType.I4), GeneralPurposeRegister.ECX);
			RegisterOperand eaxSource = new RegisterOperand(result.Type, GeneralPurposeRegister.EAX);
			RegisterOperand ecxSource = new RegisterOperand(operand.Type, GeneralPurposeRegister.ECX);

			context.SetInstruction(CPUx86.Instruction.MovInstruction, eaxSource, result);
			context.AppendInstruction(IR.Instruction.SignExtendedMoveInstruction, eax, eaxSource);
			context.AppendInstruction(CPUx86.Instruction.MovInstruction, ecxSource, operand);
			context.AppendInstruction(IR.Instruction.SignExtendedMoveInstruction, ecx, ecxSource);
			context.AppendInstruction(CPUx86.Instruction.DivInstruction, eax, ecx);
			context.AppendInstruction(CPUx86.Instruction.MovInstruction, result, new RegisterOperand(new SigType(CilElementType.I4), GeneralPurposeRegister.EDX));
		}

		/// <summary>
		/// Visitation function for RemUInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.RemUInstruction(Context context)
		{
			Operand result = context.Result;
			Operand operand = context.Operand1;
			RegisterOperand eax = new RegisterOperand(new SigType(CilElementType.U4), GeneralPurposeRegister.EAX);
			RegisterOperand ecx = new RegisterOperand(new SigType(CilElementType.U4), GeneralPurposeRegister.ECX);
			RegisterOperand eaxSource = new RegisterOperand(result.Type, GeneralPurposeRegister.EAX);
			RegisterOperand ecxSource = new RegisterOperand(operand.Type, GeneralPurposeRegister.ECX);

			context.SetInstruction(CPUx86.Instruction.MovInstruction, eaxSource, result);
			context.AppendInstruction(IR.Instruction.ZeroExtendedMoveInstruction, eax, eaxSource);
			context.AppendInstruction(CPUx86.Instruction.MovInstruction, ecxSource, operand);
			context.AppendInstruction(IR.Instruction.ZeroExtendedMoveInstruction, ecx, ecxSource);
			context.AppendInstruction(CPUx86.Instruction.UDivInstruction, eax, ecx);
			context.AppendInstruction(CPUx86.Instruction.MovInstruction, result, new RegisterOperand(new SigType(CilElementType.U4), GeneralPurposeRegister.EDX));
		}

		/// <summary>
		/// Visitation function for SwitchInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.SwitchInstruction(Context context)
		{
			IBranch branch = context.Branch;
			Operand operand = context.Operand1;

			context.Remove();

			for (int i = 0; i < branch.Targets.Length - 1; ++i)
			{
				context.AppendInstruction(CPUx86.Instruction.CmpInstruction, operand, new ConstantOperand(new SigType(CilElementType.I), i));
				context.AppendInstruction(CPUx86.Instruction.BranchInstruction, IR.ConditionCode.Equal);
				context.SetBranch(branch.Targets[i]);
			}
		}

		/// <summary>
		/// Visitation function for BreakInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.BreakInstruction(Context context)
		{
			context.SetInstruction(CPUx86.Instruction.BreakInstruction);
		}

		/// <summary>
		/// Visitation function for NopInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.NopInstruction(Context context)
		{
			context.SetInstruction(CPUx86.Instruction.NopInstruction);
		}

		/// <summary>
		/// Visitation function for SignExtendedMoveInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.SignExtendedMoveInstruction(Context context)
		{
			Operand offset = context.Operand2;
			if (offset != null)
			{
				RegisterOperand eax = new RegisterOperand(BuiltInSigType.Int32, GeneralPurposeRegister.EAX);
				Operand destination = context.Result;
				MemoryOperand source = (MemoryOperand)context.Operand1;
				SigType elementType = GetElementType(source.Type);
				ConstantOperand constantOffset = offset as ConstantOperand;
				IntPtr offsetPtr = IntPtr.Zero;

				context.SetInstruction(CPUx86.Instruction.MovInstruction, eax, source);
				if (constantOffset != null)
				{
					offsetPtr = new IntPtr(Convert.ToInt64(constantOffset.Value));
				}
				else
				{
					context.AppendInstruction(CPUx86.Instruction.AddInstruction, eax, offset);
				}

				context.AppendInstruction(CPUx86.Instruction.MovsxInstruction, destination, new MemoryOperand(elementType, GeneralPurposeRegister.EAX, offsetPtr));
			}
			else
			{
				context.ReplaceInstructionOnly(CPUx86.Instruction.MovsxInstruction);
			}
		}

		private static SigType GetElementType(SigType sigType)
		{
			PtrSigType pointerType = sigType as PtrSigType;
			if (pointerType != null)
			{
				return pointerType.ElementType;
			}

			RefSigType referenceType = sigType as RefSigType;
			if (referenceType != null)
			{
				return referenceType.ElementType;
			}

			return sigType;
		}

		/// <summary>
		/// Visitation function for CallInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.CallInstruction(Context context)
		{
			callingConvention.MakeCall(context);
		}

		/// <summary>
		/// Visitation function for ZeroExtendedMoveInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.ZeroExtendedMoveInstruction(Context context)
		{
			Operand offset = context.Operand2;
			if (offset != null)
			{
				RegisterOperand eax = new RegisterOperand(context.Operand1.Type, GeneralPurposeRegister.EAX);
				Operand result = context.Result;
				Operand source = context.Operand1;
				SigType elementType = GetElementType(source.Type);
				ConstantOperand constantOffset = offset as ConstantOperand;
				IntPtr offsetPtr = IntPtr.Zero;

				context.SetInstruction(CPUx86.Instruction.MovInstruction, eax, source);
				if (constantOffset != null)
				{
					offsetPtr = new IntPtr(Convert.ToInt64(constantOffset.Value));
				}

				context.AppendInstruction(CPUx86.Instruction.AddInstruction, eax, offset);
				context.AppendInstruction(CPUx86.Instruction.MovzxInstruction, result, new MemoryOperand(elementType, GeneralPurposeRegister.EAX, offsetPtr));
			}
			else
			{
				context.ReplaceInstructionOnly(CPUx86.Instruction.MovzxInstruction);
			}
		}

		/// <summary>
		/// Visitation function for BranchInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.BranchInstruction(Context context)
		{
			context.ReplaceInstructionOnly(CPUx86.Instruction.BranchInstruction);
		}

		/// <summary>
		/// Visitation function for FloatingPointToIntegerConversionInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.FloatingPointToIntegerConversionInstruction(Context context)
		{
			Operand source = context.Operand1;
			Operand destination = context.Result;
			switch (destination.Type.Type)
			{
				case CilElementType.I1: goto case CilElementType.I4;
				case CilElementType.I2: goto case CilElementType.I4;
				case CilElementType.I4:
					if (source.Type.Type == CilElementType.R8)
						context.ReplaceInstructionOnly(CPUx86.Instruction.Cvttsd2siInstruction);
					else
						context.ReplaceInstructionOnly(CPUx86.Instruction.Cvttss2siInstruction);
					break;
				case CilElementType.I8: throw new NotSupportedException();
				case CilElementType.U1: goto case CilElementType.U4;
				case CilElementType.U2: goto case CilElementType.U4;
				case CilElementType.U4: throw new NotSupportedException();
				case CilElementType.U8: throw new NotSupportedException();
				case CilElementType.I: goto case CilElementType.I4;
				case CilElementType.U: goto case CilElementType.U4;
			}
		}

		/// <summary>
		/// Visitation function for PopInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.PopInstruction(Context context)
		{
			context.ReplaceInstructionOnly(CPUx86.Instruction.PopInstruction);
		}

		/// <summary>
		/// Visitation function for PushInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.PushInstruction(Context context)
		{
			context.ReplaceInstructionOnly(CPUx86.Instruction.PushInstruction);
		}

		/// <summary>
		/// Visitation function for ThrowInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.ThrowInstruction(Context context)
		{
			int pointOfThrow = 4;
			int exceptionObject = 12;
			SigType u4 = new SigType(CilElementType.U4);

			RuntimeType runtimeType = typeSystem.GetType(@"Mosa.Platforms.x86.ExceptionEngine, Mosa.Platforms.x86");
			RuntimeMethod runtimeMethod = runtimeType.FindMethod(@"ThrowException");
			SymbolOperand method = SymbolOperand.FromMethod(runtimeMethod);

			Operand callTarget = context.Result;

			MemoryOperand pointOfThrowOperand = new MemoryOperand(u4, GeneralPurposeRegister.ESP, new IntPtr(pointOfThrow));
			MemoryOperand exceptionObjectOperand = new MemoryOperand(u4, GeneralPurposeRegister.ESP, new IntPtr(exceptionObject));

			RegisterOperand esp = new RegisterOperand(u4, GeneralPurposeRegister.ESP);
			RegisterOperand eax = new RegisterOperand(u4, GeneralPurposeRegister.EAX);

			// Save current stack
			context.SetInstruction(CPUx86.Instruction.PushInstruction, null, new RegisterOperand(u4, GeneralPurposeRegister.ESP));
			// Save point of call
			context.AppendInstruction(CPUx86.Instruction.PushInstruction, null, pointOfThrowOperand);
			// Pass thrown exception
			context.AppendInstruction(CPUx86.Instruction.PushInstruction, null, exceptionObjectOperand);
			// Save registers
			context.AppendInstruction(CPUx86.Instruction.PushInstruction, null, new RegisterOperand(u4, GeneralPurposeRegister.EBP));
			context.AppendInstruction(CPUx86.Instruction.PushInstruction, null, new RegisterOperand(u4, GeneralPurposeRegister.EDI));
			context.AppendInstruction(CPUx86.Instruction.PushInstruction, null, new RegisterOperand(u4, GeneralPurposeRegister.ESI));
			context.AppendInstruction(CPUx86.Instruction.PushInstruction, null, new RegisterOperand(u4, GeneralPurposeRegister.EBX));
			context.AppendInstruction(CPUx86.Instruction.PushInstruction, null, new RegisterOperand(u4, GeneralPurposeRegister.EDX));
			context.AppendInstruction(CPUx86.Instruction.PushInstruction, null, new RegisterOperand(u4, GeneralPurposeRegister.ECX));
			context.AppendInstruction(CPUx86.Instruction.PushInstruction, null, new RegisterOperand(u4, GeneralPurposeRegister.EAX));

			// Pass them to the exception handling routine as parameters
			context.AppendInstruction(CPUx86.Instruction.SubInstruction, esp, new ConstantOperand(esp.Type, 40));
			context.AppendInstruction(CPUx86.Instruction.MovInstruction, new RegisterOperand(u4, GeneralPurposeRegister.EDX), esp);

			for (int i = 0; i < 10; ++i)
			{
				context.AppendInstruction(CPUx86.Instruction.PopInstruction, eax);
				context.AppendInstruction(CPUx86.Instruction.MovInstruction, new MemoryOperand(u4, GeneralPurposeRegister.EDX, new IntPtr(i * 4)), eax);
			}

			// Call exception handling
			context.AppendInstruction(CPUx86.Instruction.CallInstruction, null, method);

			// Compile exception handling if neccessary
			if (!exceptionHandlingCompiled)
			{
				this.methodCompiler.Scheduler.ScheduleTypeForCompilation(typeSystem.GetType(@"Mosa.Platforms.x86.RegisterContext, Mosa.Platforms.x86"));
				this.methodCompiler.Scheduler.ScheduleTypeForCompilation(typeSystem.GetType(@"Mosa.Platforms.x86.ExceptionEngine, Mosa.Platforms.x86"));
				(this.typeLayout as TypeLayoutStage).CreateSequentialLayout(typeSystem.GetType(@"Mosa.Platforms.x86.RegisterContext, Mosa.Platforms.x86"));
				(this.typeLayout as TypeLayoutStage).CreateSequentialLayout(typeSystem.GetType(@"Mosa.Platforms.x86.ExceptionEngine, Mosa.Platforms.x86"));
				(this.typeLayout as TypeLayoutStage).BuildMethodTable(typeSystem.GetType(@"Mosa.Platforms.x86.RegisterContext, Mosa.Platforms.x86"));
				(this.typeLayout as TypeLayoutStage).BuildMethodTable(typeSystem.GetType(@"Mosa.Platforms.x86.ExceptionEngine, Mosa.Platforms.x86"));
				exceptionHandlingCompiled = true;
			}
		}

		#endregion //  IIRVisitor

		#region IIRVisitor - Unused

		/// <summary>
		/// Visitation function for IntegerToFloatingPointConversionInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.IntegerToFloatingPointConversionInstruction(Context context) { }

		/// <summary>
		/// Visitation function for LiteralInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.LiteralInstruction(Context context) { }

		/// <summary>
		/// Visitation function for PhiInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.PhiInstruction(Context context) { }

		#endregion // IIRVisitor - Unused

		#region Internals

		/// <summary>
		/// Extends to r8.
		/// </summary>
		/// <param name="context">The context.</param>
		private static void ExtendToR8(Context context)
		{
			RegisterOperand xmm5 = new RegisterOperand(new SigType(CilElementType.R8), SSE2Register.XMM5);
			RegisterOperand xmm6 = new RegisterOperand(new SigType(CilElementType.R8), SSE2Register.XMM6);
			Context before = context.InsertBefore();

			if (context.Result.Type.Type == CilElementType.R4)
			{
				before.SetInstruction(CPUx86.Instruction.Cvtss2sdInstruction, xmm5, context.Result);
				context.Result = xmm5;
			}

			if (context.Operand1.Type.Type == CilElementType.R4)
			{
				before.SetInstruction(CPUx86.Instruction.Cvtss2sdInstruction, xmm6, context.Operand1);
				context.Operand1 = xmm6;
			}
		}

		/// <summary>
		/// Special handling for commutative operations.
		/// </summary>
		/// <param name="context">The transformation context.</param>
		/// <param name="instruction">The instruction.</param>
		/// <remarks>
		/// Commutative operations are reordered by moving the constant to the second operand,
		/// which allows the instruction selection in the code generator to use a instruction
		/// format with an immediate operand.
		/// </remarks>
		private void HandleCommutativeOperation(Context context, IInstruction instruction)
		{
			EmitOperandConstants(context);
			context.ReplaceInstructionOnly(instruction);
		}

		/// <summary>
		/// Handles the non commutative operation.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="instruction">The instruction.</param>
		private void HandleNonCommutativeOperation(Context context, IInstruction instruction)
		{
			EmitResultConstants(context);
			EmitOperandConstants(context);
			context.ReplaceInstructionOnly(instruction);
		}

		/// <summary>
		/// Special handling for shift operations, which require the shift amount in the ECX or as a constant register.
		/// </summary>
		/// <param name="context">The transformation context.</param>
		/// <param name="instruction">The instruction to transform.</param>
		private void HandleShiftOperation(Context context, IInstruction instruction)
		{
			EmitOperandConstants(context);
			context.ReplaceInstructionOnly(instruction);
		}

		/// <summary>
		/// Swaps the comparison operands.
		/// </summary>
		/// <param name="context">The context.</param>
		private static void SwapComparisonOperands(Context context)
		{
			Operand op1 = context.Operand1;
			context.Operand1 = context.Operand2;
			context.Operand2 = op1;

			// Negate the condition code if necessary...
			switch (context.ConditionCode)
			{
				case IR.ConditionCode.Equal: break;
				case IR.ConditionCode.GreaterOrEqual: context.ConditionCode = IR.ConditionCode.LessThan; break;
				case IR.ConditionCode.GreaterThan: context.ConditionCode = IR.ConditionCode.LessOrEqual; break;
				case IR.ConditionCode.LessOrEqual: context.ConditionCode = IR.ConditionCode.GreaterThan; break;
				case IR.ConditionCode.LessThan: context.ConditionCode = IR.ConditionCode.GreaterOrEqual; break;
				case IR.ConditionCode.NotEqual: break;
				case IR.ConditionCode.UnsignedGreaterOrEqual: context.ConditionCode = IR.ConditionCode.UnsignedLessThan; break;
				case IR.ConditionCode.UnsignedGreaterThan: context.ConditionCode = IR.ConditionCode.UnsignedLessOrEqual; break;
				case IR.ConditionCode.UnsignedLessOrEqual: context.ConditionCode = IR.ConditionCode.UnsignedGreaterThan; break;
				case IR.ConditionCode.UnsignedLessThan: context.ConditionCode = IR.ConditionCode.UnsignedGreaterOrEqual; break;
			}
		}

		#endregion // Internals
	}
}
