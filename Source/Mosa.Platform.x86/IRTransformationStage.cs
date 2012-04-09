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

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Operands;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.TypeSystem;
using IR = Mosa.Compiler.Framework.IR;

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

		private int stackSize;

		#region IMethodCompilerStage

		/// <summary>
		/// Setup stage specific processing on the compiler context.
		/// </summary>
		/// <param name="methodCompiler">The compiler context to perform processing in.</param>
		void IMethodCompilerStage.Setup(IMethodCompiler methodCompiler)
		{
			base.Setup(methodCompiler);

			IStackLayoutProvider stackLayoutProvider = methodCompiler.Pipeline.FindFirst<IStackLayoutProvider>();

			stackSize = (stackLayoutProvider == null) ? 0 : stackLayoutProvider.LocalsSize;

			Debug.Assert((stackSize % 4) == 0, @"Stack size of method can't be divided by 4!!");
		}

		#endregion // IMethodCompilerStage

		#region IIRVisitor

		/// <summary>
		/// Visitation function for AddSInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.AddSInstruction(Context context)
		{
			HandleCommutativeOperation(context, Instruction.AddInstruction);
		}

		/// <summary>
		/// Visitation function for AddUInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.AddUInstruction(Context context)
		{
			HandleCommutativeOperation(context, Instruction.AddInstruction);
		}

		/// <summary>
		/// Visitation function for AddFInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.AddFInstruction(Context context)
		{
			HandleCommutativeOperation(context, Instruction.SseAddInstruction);
			ExtendToR8(context);
		}

		/// <summary>
		/// Visitation function for DivFInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.DivFInstruction(Context context)
		{
			HandleNonCommutativeOperation(context, Instruction.SseDivInstruction);
			ExtendToR8(context);
		}

		/// <summary>
		/// Visitation function for DivSInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.DivSInstruction(Context context)
		{
			HandleNonCommutativeOperation(context, Instruction.DivInstruction);
		}

		/// <summary>
		/// Addresses the of instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.AddressOfInstruction(Context context)
		{
			var opRes = context.Result;
			
			RegisterOperand register = new RegisterOperand(opRes.Type, GeneralPurposeRegister.EAX);
			context.Result = register;
			context.ReplaceInstructionOnly(Instruction.LeaInstruction);
			//context.Ignore = true;
			context.AppendInstruction(Instruction.MovInstruction, opRes, register);
		}

		/// <summary>
		/// Arithmetic the shift right instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.ArithmeticShiftRightInstruction(Context context)
		{
			HandleShiftOperation(context, Instruction.SarInstruction);
		}

		/// <summary>
		/// Floating point compare instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.FloatingPointCompareInstruction(Context context)
		{
			Operand resultOperand = context.Result;
			Operand left = EmitConstant(context.Operand1);
			Operand right = EmitConstant(context.Operand2);

			context.Operand1 = left;
			context.Operand2 = right;

			// Swap the operands if necessary...
			if (left is MemoryOperand && right is RegisterOperand)
			{
				SwapComparisonOperands(context);
				left = context.Operand1;
				right = context.Operand2;
			}

			IR.ConditionCode setnp = IR.ConditionCode.NoParity;
			IR.ConditionCode setnc = IR.ConditionCode.NoCarry;
			IR.ConditionCode setc = IR.ConditionCode.Carry;
			IR.ConditionCode setnz = IR.ConditionCode.NoZero;
			IR.ConditionCode setz = IR.ConditionCode.Zero;
			IR.ConditionCode code = context.ConditionCode;

			context.SetInstruction(Instruction.NopInstruction);

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
					context.AppendInstruction(Instruction.MovssInstruction, xmm2, left);
				else
					context.AppendInstruction(Instruction.MovsdInstruction, xmm2, left);
				left = xmm2;
			}

			// Compare using the smallest precision
			if (left.Type.Type == CilElementType.R4 && right.Type.Type == CilElementType.R8)
			{
				RegisterOperand rop = new RegisterOperand(BuiltInSigType.Single, SSE2Register.XMM4);
				context.AppendInstruction(Instruction.Cvtsd2ssInstruction, rop, right);
				right = rop;
			}
			if (left.Type.Type == CilElementType.R8 && right.Type.Type == CilElementType.R4)
			{
				RegisterOperand rop = new RegisterOperand(BuiltInSigType.Single, SSE2Register.XMM3);
				context.AppendInstruction(Instruction.Cvtsd2ssInstruction, rop, left);
				left = rop;
			}

			if (left.Type.Type == CilElementType.R4)
			{
				switch (code)
				{
					case IR.ConditionCode.Equal:
						context.AppendInstruction(Instruction.ComissInstruction, left, right);
						break;
					case IR.ConditionCode.NotEqual: goto case IR.ConditionCode.Equal;
					case IR.ConditionCode.UnsignedGreaterOrEqual: goto case IR.ConditionCode.Equal;
					case IR.ConditionCode.UnsignedGreaterThan: goto case IR.ConditionCode.Equal;
					case IR.ConditionCode.UnsignedLessOrEqual: goto case IR.ConditionCode.Equal;
					case IR.ConditionCode.UnsignedLessThan: goto case IR.ConditionCode.Equal;
					case IR.ConditionCode.GreaterOrEqual:
						context.AppendInstruction(Instruction.ComissInstruction, left, right);
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
						context.AppendInstruction(Instruction.ComisdInstruction, left, right);
						break;
					case IR.ConditionCode.NotEqual: goto case IR.ConditionCode.Equal;
					case IR.ConditionCode.UnsignedGreaterOrEqual: goto case IR.ConditionCode.Equal;
					case IR.ConditionCode.UnsignedGreaterThan: goto case IR.ConditionCode.Equal;
					case IR.ConditionCode.UnsignedLessOrEqual: goto case IR.ConditionCode.Equal;
					case IR.ConditionCode.UnsignedLessThan: goto case IR.ConditionCode.Equal;
					case IR.ConditionCode.GreaterOrEqual:
						context.AppendInstruction(Instruction.ComisdInstruction, left, right);
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

			context.AppendInstruction(Instruction.PushfdInstruction);

			if (code == IR.ConditionCode.Equal)
			{
				context.AppendInstruction(Instruction.SetccInstruction, setz, eax);
				context.AppendInstruction(Instruction.SetccInstruction, setnp, ebx);
				context.AppendInstruction(Instruction.SetccInstruction, setnc, ecx);
				context.AppendInstruction(Instruction.SetccInstruction, setnz, edx);
				context.AppendInstruction(Instruction.AndInstruction, eax, ebx);
				context.AppendInstruction(Instruction.AndInstruction, eax, ecx);
				context.AppendInstruction(Instruction.OrInstruction, ebx, ecx);
				context.AppendInstruction(Instruction.OrInstruction, ebx, edx);
				context.AppendInstruction(Instruction.AndInstruction, eax, ebx);
				context.AppendInstruction(Instruction.AndInstruction, eax, new ConstantOperand(BuiltInSigType.Int32, (int)1));
			}
			else if (code == IR.ConditionCode.NotEqual)
			{
				context.AppendInstruction(Instruction.SetccInstruction, setz, eax);
				context.AppendInstruction(Instruction.SetccInstruction, setnp, ebx);
				context.AppendInstruction(Instruction.SetccInstruction, setnc, ecx);
				context.AppendInstruction(Instruction.SetccInstruction, setnz, edx);
				context.AppendInstruction(Instruction.AndInstruction, eax, ebx);
				context.AppendInstruction(Instruction.AndInstruction, eax, ecx);
				context.AppendInstruction(Instruction.OrInstruction, ebx, ecx);
				context.AppendInstruction(Instruction.OrInstruction, ebx, edx);
				context.AppendInstruction(Instruction.NotInstruction, ebx, ebx);
				context.AppendInstruction(Instruction.OrInstruction, eax, ebx);
				context.AppendInstruction(Instruction.AndInstruction, eax, new ConstantOperand(BuiltInSigType.Int32, (int)1));
			}
			else if (code == IR.ConditionCode.GreaterThan)
			{
				context.AppendInstruction(Instruction.SetccInstruction, setnz, eax);
				context.AppendInstruction(Instruction.SetccInstruction, setnp, ebx);
				context.AppendInstruction(Instruction.SetccInstruction, setnc, ecx);
				context.AppendInstruction(Instruction.AndInstruction, eax, ebx);
				context.AppendInstruction(Instruction.AndInstruction, eax, ecx);
			}
			else if (code == IR.ConditionCode.LessThan)
			{
				context.AppendInstruction(Instruction.SetccInstruction, setnz, eax);
				context.AppendInstruction(Instruction.SetccInstruction, setnp, ebx);
				context.AppendInstruction(Instruction.SetccInstruction, setc, ecx);
				context.AppendInstruction(Instruction.AndInstruction, eax, ebx);
				context.AppendInstruction(Instruction.AndInstruction, eax, ecx);
			}
			else if (code == IR.ConditionCode.GreaterOrEqual)
			{
				context.AppendInstruction(Instruction.SetccInstruction, setnz, eax);
				context.AppendInstruction(Instruction.SetccInstruction, setnp, ebx);
				context.AppendInstruction(Instruction.SetccInstruction, setnc, ecx);
				context.AppendInstruction(Instruction.SetccInstruction, setz, edx);
				context.AppendInstruction(Instruction.AndInstruction, eax, ebx);
				context.AppendInstruction(Instruction.AndInstruction, eax, ecx);
				context.AppendInstruction(Instruction.OrInstruction, eax, edx);
			}
			else if (code == IR.ConditionCode.LessOrEqual)
			{
				context.AppendInstruction(Instruction.SetccInstruction, setnz, eax);
				context.AppendInstruction(Instruction.SetccInstruction, setnp, ebx);
				context.AppendInstruction(Instruction.SetccInstruction, setc, ecx);
				context.AppendInstruction(Instruction.SetccInstruction, setz, edx);
				context.AppendInstruction(Instruction.AndInstruction, eax, ebx);
				context.AppendInstruction(Instruction.AndInstruction, eax, ecx);
				context.AppendInstruction(Instruction.OrInstruction, eax, edx);
			}
			context.AppendInstruction(Instruction.MovzxInstruction, resultOperand, eax);
			context.AppendInstruction(Instruction.PopfdInstruction);
		}

		/// <summary>
		/// Visitation function for IntegerCompareBranchInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.IntegerCompareBranchInstruction(Context context)
		{
			EmitOperandConstants(context);

			IBranch branch = context.Branch;
			var condition = context.ConditionCode;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			context.SetInstruction(Instruction.CmpInstruction, operand1, operand2);
			context.AppendInstruction(Instruction.BranchInstruction, condition);
			context.SetBranch(branch.Targets[0]);
		}

		/// <summary>
		/// Visitation function for IntegerCompareInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.IntegerCompareInstruction(Context context)
		{
			EmitOperandConstants(context);

			var condition = context.ConditionCode;
			var resultOperand = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			context.SetInstruction(Instruction.CmpInstruction, operand1, operand2);

			if (resultOperand != null)
			{
				RegisterOperand eax = new RegisterOperand(BuiltInSigType.Byte, GeneralPurposeRegister.EAX);

				if (IsUnsigned(resultOperand))
					context.AppendInstruction(Instruction.SetccInstruction, GetUnsignedConditionCode(condition), eax);
				else
					context.AppendInstruction(Instruction.SetccInstruction, condition, eax);

				context.AppendInstruction(Instruction.MovzxInstruction, resultOperand, eax);
			}
		}

		/// <summary>
		/// Visitation function for JmpInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.JmpInstruction(Context context)
		{
			context.ReplaceInstructionOnly(Instruction.JmpInstruction);
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

			context.SetInstruction(Instruction.MovInstruction, eax, operand);
			if (constantOffset != null)
			{
				offsetPtr = new IntPtr(Convert.ToInt64(constantOffset.Value));
			}
			else
			{
				context.AppendInstruction(Instruction.AddInstruction, eax, offset);
			}

			context.AppendInstruction(Instruction.MovInstruction, result, new MemoryOperand(eax.Type, GeneralPurposeRegister.EAX, offsetPtr));
		}

		/// <summary>
		/// Visitation function for LogicalAndInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.LogicalAndInstruction(Context context)
		{
			context.ReplaceInstructionOnly(Instruction.AndInstruction);
		}

		/// <summary>
		/// Visitation function for LogicalOrInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.LogicalOrInstruction(Context context)
		{
			context.ReplaceInstructionOnly(Instruction.OrInstruction);
		}

		/// <summary>
		/// Visitation function for LogicalXorInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.LogicalXorInstruction(Context context)
		{
			context.ReplaceInstructionOnly(Instruction.XorInstruction);
		}

		/// <summary>
		/// Visitation function for LogicalNotInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.LogicalNotInstruction(Context context)
		{
			Operand dest = context.Result;

			context.SetInstruction(Instruction.MovInstruction, context.Result, context.Operand1);
			if (dest.Type.Type == CilElementType.U1)
				context.AppendInstruction(Instruction.XorInstruction, dest, new ConstantOperand(BuiltInSigType.UInt32, (uint)0xFF));
			else if (dest.Type.Type == CilElementType.U2)
				context.AppendInstruction(Instruction.XorInstruction, dest, new ConstantOperand(BuiltInSigType.UInt32, (uint)0xFFFF));
			else
				context.AppendInstruction(Instruction.NotInstruction, dest);
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
						MoveFloatingPoint(context, Instruction.MovssInstruction);
					else if (context.Result.Type.Type == CilElementType.R8)
						MoveFloatingPoint(context, Instruction.MovsdInstruction);
				}
				else if (context.Result.Type.Type == CilElementType.R8)
				{
					context.SetInstruction(Instruction.Cvtss2sdInstruction, context.Result, context.Operand1, context.Operand2);
				}
				else if (context.Result.Type.Type == CilElementType.R4)
				{
					context.SetInstruction(Instruction.Cvtsd2ssInstruction, context.Result, context.Operand1, context.Operand2);
				}
			}
			else
			{
				if (context.Result is MemoryOperand && context.Operand1 is MemoryOperand)
				{
					RegisterOperand load = new RegisterOperand(BuiltInSigType.IntPtr, GeneralPurposeRegister.EDX);
					RegisterOperand store = new RegisterOperand(operand.Type, GeneralPurposeRegister.EDX);

					if (!Is32Bit(operand) && IsSigned(operand))
						context.SetInstruction(Instruction.MovsxInstruction, load, operand);
					else if (!Is32Bit(operand) && IsUnsigned(operand))
						context.SetInstruction(Instruction.MovzxInstruction, load, operand);
					else
						context.SetInstruction(Instruction.MovInstruction, load, operand);

					context.AppendInstruction(Instruction.MovInstruction, result, store);
				}
				else
					context.ReplaceInstructionOnly(Instruction.MovInstruction);
			}
		}

		private void MoveFloatingPoint(Context context, Instructions.X86Instruction instruction)
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
			SigType I = BuiltInSigType.Int32;
			RegisterOperand eax = new RegisterOperand(I, GeneralPurposeRegister.EAX);
			RegisterOperand ebx = new RegisterOperand(I, GeneralPurposeRegister.EBX);
			RegisterOperand ecx = new RegisterOperand(I, GeneralPurposeRegister.ECX);
			RegisterOperand ebp = new RegisterOperand(I, GeneralPurposeRegister.EBP);
			RegisterOperand esp = new RegisterOperand(I, GeneralPurposeRegister.ESP);
			RegisterOperand edi = new RegisterOperand(I, GeneralPurposeRegister.EDI);

			/* 
			 * If you want to stop at the header of an emitted function, just set breakFlag 
			 * to true in the following line. It will issue a breakpoint instruction. Note 
			 * that if you debug using visual studio you must enable unmanaged code 
			 * debugging, otherwise the function will never return and the breakpoint will 
			 * never appear. 
			 */
			bool breakFlag = false; // TODO: Turn this into a compiler option

			if (breakFlag)
			{
				// int 3
				context.SetInstruction(Instruction.BreakInstruction);
				context.AppendInstruction(Instruction.NopInstruction);

				// Uncomment this line to enable breakpoints within Bochs
				//context.AppendInstruction(CPUx86.Instruction.BochsDebug);
			}

			// push ebp
			context.SetInstruction(Instruction.PushInstruction, null, ebp);
			// mov ebp, esp
			context.AppendInstruction(Instruction.MovInstruction, ebp, esp);
			// sub esp, localsSize
			context.AppendInstruction(Instruction.SubInstruction, esp, new ConstantOperand(I, -stackSize));
			// push ebx
			context.AppendInstruction(Instruction.PushInstruction, null, ebx);

			// Initialize all locals to zero
			context.AppendInstruction(Instruction.PushInstruction, null, edi);
			context.AppendInstruction(Instruction.MovInstruction, edi, esp);
			context.AppendInstruction(Instruction.PushInstruction, null, ecx);
			context.AppendInstruction(Instruction.AddInstruction, edi, new ConstantOperand(I, 8));
			context.AppendInstruction(Instruction.MovInstruction, ecx, new ConstantOperand(I, -(int)(stackSize >> 2)));
			context.AppendInstruction(Instruction.XorInstruction, eax, eax);
			context.AppendInstruction(Instruction.RepInstruction);
			context.AppendInstruction(Instruction.StosdInstruction);
			context.AppendInstruction(Instruction.PopInstruction, ecx);
			context.AppendInstruction(Instruction.PopInstruction, edi);

			// Save EDX for int32 return values (or do not save EDX for non-int64 return values)
			if (methodCompiler.Method.Signature.ReturnType.Type != CilElementType.I8 &&
				methodCompiler.Method.Signature.ReturnType.Type != CilElementType.U8)
			{
				// push edx
				context.AppendInstruction(Instruction.PushInstruction, null, new RegisterOperand(I, GeneralPurposeRegister.EDX));
			}

		}

		/// <summary>
		/// Visitation function for EpilogueInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.EpilogueInstruction(Context context)
		{
			SigType I = BuiltInSigType.IntPtr;
			RegisterOperand ebx = new RegisterOperand(I, GeneralPurposeRegister.EBX);
			RegisterOperand edx = new RegisterOperand(I, GeneralPurposeRegister.EDX);
			RegisterOperand ebp = new RegisterOperand(I, GeneralPurposeRegister.EBP);
			RegisterOperand esp = new RegisterOperand(I, GeneralPurposeRegister.ESP);

			// Load EDX for int32 return values
			if (methodCompiler.Method.Signature.ReturnType.Type != CilElementType.I8 &&
				methodCompiler.Method.Signature.ReturnType.Type != CilElementType.U8)
			{
				// pop edx
				context.SetInstruction(Instruction.PopInstruction, edx);
				context.AppendInstruction(Instruction.NopInstruction);
			}

			// pop ebx
			context.SetInstruction(Instruction.PopInstruction, ebx);
			// add esp, -localsSize
			context.AppendInstruction(Instruction.AddInstruction, esp, new ConstantOperand(I, -stackSize));
			// pop ebp
			context.AppendInstruction(Instruction.PopInstruction, ebp);
			// ret
			context.AppendInstruction(Instruction.RetInstruction);
		}

		/// <summary>
		/// Visitation function for ReturnInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.ReturnInstruction(Context context)
		{
			if (context.Branch == null)
			{
				// To return from an internal method call (usually from within a finally or exception clause)
				context.SetInstruction(Instruction.RetInstruction);
				return;
			}

			if (context.Operand1 != null)
			{
				callingConvention.MoveReturnValue(context, context.Operand1);
				context.AppendInstruction(Instruction.JmpInstruction);
				context.SetBranch(Int32.MaxValue);
			}
			else
			{
				context.SetInstruction(Instruction.JmpInstruction);
				context.SetBranch(Int32.MaxValue);
			}
		}

		/// <summary>
		/// Visitation function for ShiftLeftInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.ShiftLeftInstruction(Context context)
		{
			HandleShiftOperation(context, Instruction.ShlInstruction);
		}

		/// <summary>
		/// Visitation function for ShiftRightInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.ShiftRightInstruction(Context context)
		{
			HandleShiftOperation(context, Instruction.ShrInstruction);
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

			context.SetInstruction(Instruction.MovInstruction, eax, destination);
			context.AppendInstruction(Instruction.MovInstruction, edx, value);

			IntPtr offsetPtr = IntPtr.Zero;
			if (constantOffset != null)
			{
				offsetPtr = new IntPtr(Convert.ToInt64(constantOffset.Value));
			}
			else
			{
				context.AppendInstruction(Instruction.AddInstruction, eax, offset);
			}

			context.AppendInstruction(Instruction.MovInstruction, new MemoryOperand(value.Type, GeneralPurposeRegister.EAX, offsetPtr), edx);
		}

		/// <summary>
		/// Visitation function for DivUInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.DivUInstruction(Context context)
		{
			context.ReplaceInstructionOnly(Instruction.UDivInstruction);
		}

		/// <summary>
		/// Visitation function for MulSInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.MulSInstruction(Context context)
		{
			HandleCommutativeOperation(context, Instruction.MulInstruction);
		}

		/// <summary>
		/// Visitation function for MulFInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.MulFInstruction(Context context)
		{
			HandleCommutativeOperation(context, Instruction.SseMulInstruction);
			ExtendToR8(context);
		}

		/// <summary>
		/// Visitation function for MulUInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.MulUInstruction(Context context)
		{
			HandleCommutativeOperation(context, Instruction.MulInstruction);
		}

		/// <summary>
		/// Visitation function for SubFInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.SubFInstruction(Context context)
		{
			HandleNonCommutativeOperation(context, Instruction.SseSubInstruction);
			ExtendToR8(context);
		}

		/// <summary>
		/// Visitation function for SubSInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.SubSInstruction(Context context)
		{
			HandleNonCommutativeOperation(context, Instruction.SubInstruction);
		}

		/// <summary>
		/// Visitation function for SubUInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.SubUInstruction(Context context)
		{
			HandleNonCommutativeOperation(context, Instruction.SubInstruction);
		}

		/// <summary>
		/// Visitation function for RemFInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.RemFInstruction(Context context)
		{
			HandleNonCommutativeOperation(context, Instruction.SseDivInstruction);
			ExtendToR8(context);

			Operand destination = context.Result;
			Operand source = context.Operand1;

			Context[] newBlocks = CreateEmptyBlockContexts(context.Label, 3);
			Context nextBlock = SplitContext(context, false);

			RegisterOperand xmm5 = new RegisterOperand(BuiltInSigType.Double, SSE2Register.XMM5);
			RegisterOperand xmm6 = new RegisterOperand(BuiltInSigType.Double, SSE2Register.XMM6);
			RegisterOperand edx = new RegisterOperand(BuiltInSigType.Int32, GeneralPurposeRegister.EDX);

			context.SetInstruction(Instruction.JmpInstruction, newBlocks[0].BasicBlock);
			LinkBlocks(context, newBlocks[0]);

			newBlocks[0].SetInstruction(Instruction.MovsdInstruction, xmm5, source);
			newBlocks[0].AppendInstruction(Instruction.MovsdInstruction, xmm6, destination);

			newBlocks[0].AppendInstruction(Instruction.SseDivInstruction, destination, source);
			newBlocks[0].AppendInstruction(Instruction.Cvttsd2siInstruction, edx, destination);

			newBlocks[0].AppendInstruction(Instruction.CmpInstruction, edx, new ConstantOperand(BuiltInSigType.Int32, 0));
			newBlocks[0].AppendInstruction(Instruction.BranchInstruction, IR.ConditionCode.Equal, newBlocks[2].BasicBlock);
			newBlocks[0].AppendInstruction(Instruction.JmpInstruction, newBlocks[1].BasicBlock);
			LinkBlocks(newBlocks[0], newBlocks[1], newBlocks[2]);

			newBlocks[1].AppendInstruction(Instruction.Cvtsi2sdInstruction, destination, edx);
			newBlocks[1].AppendInstruction(Instruction.SseMulInstruction, destination, xmm5);
			newBlocks[1].AppendInstruction(Instruction.SseSubInstruction, xmm6, destination);
			newBlocks[1].AppendInstruction(Instruction.MovsdInstruction, destination, xmm6);
			newBlocks[1].AppendInstruction(Instruction.JmpInstruction, nextBlock.BasicBlock);
			LinkBlocks(newBlocks[1], nextBlock);

			newBlocks[2].SetInstruction(Instruction.MovsdInstruction, destination, xmm6);
			newBlocks[2].AppendInstruction(Instruction.JmpInstruction, nextBlock.BasicBlock);
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
			RegisterOperand eax = new RegisterOperand(BuiltInSigType.Int32, GeneralPurposeRegister.EAX);
			RegisterOperand ecx = new RegisterOperand(BuiltInSigType.Int32, GeneralPurposeRegister.ECX);
			RegisterOperand edx = new RegisterOperand(BuiltInSigType.Int32, GeneralPurposeRegister.EDX);
			RegisterOperand eaxSource = new RegisterOperand(result.Type, GeneralPurposeRegister.EAX);
			RegisterOperand ecxSource = new RegisterOperand(operand.Type, GeneralPurposeRegister.ECX);

			context.SetInstruction(Instruction.MovInstruction, eaxSource, result);
			context.AppendInstruction(IR.Instruction.SignExtendedMoveInstruction, eax, eaxSource);
			context.AppendInstruction(Instruction.MovInstruction, ecxSource, operand);
			context.AppendInstruction(IR.Instruction.SignExtendedMoveInstruction, ecx, ecxSource);
			context.AppendInstruction(Instruction.DivInstruction, eax, ecx);
			context.AppendInstruction(Instruction.MovInstruction, result, edx);
		}

		/// <summary>
		/// Visitation function for RemUInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.RemUInstruction(Context context)
		{
			Operand result = context.Result;
			Operand operand = context.Operand1;
			RegisterOperand eax = new RegisterOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.EAX);
			RegisterOperand ecx = new RegisterOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.ECX);
			RegisterOperand eaxSource = new RegisterOperand(result.Type, GeneralPurposeRegister.EAX);
			RegisterOperand ecxSource = new RegisterOperand(operand.Type, GeneralPurposeRegister.ECX);

			context.SetInstruction(Instruction.MovInstruction, eaxSource, result);
			context.AppendInstruction(IR.Instruction.ZeroExtendedMoveInstruction, eax, eaxSource);
			context.AppendInstruction(Instruction.MovInstruction, ecxSource, operand);
			context.AppendInstruction(IR.Instruction.ZeroExtendedMoveInstruction, ecx, ecxSource);
			context.AppendInstruction(Instruction.UDivInstruction, eax, ecx);
			context.AppendInstruction(Instruction.MovInstruction, result, new RegisterOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.EDX));
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
				context.AppendInstruction(Instruction.CmpInstruction, operand, new ConstantOperand(BuiltInSigType.IntPtr, i));
				context.AppendInstruction(Instruction.BranchInstruction, IR.ConditionCode.Equal);
				context.SetBranch(branch.Targets[i]);
			}
		}

		/// <summary>
		/// Visitation function for BreakInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.BreakInstruction(Context context)
		{
			context.SetInstruction(Instruction.BreakInstruction);
		}

		/// <summary>
		/// Visitation function for NopInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.NopInstruction(Context context)
		{
			context.SetInstruction(Instruction.NopInstruction);
		}

		/// <summary>
		/// Visitation function for SignExtendedMoveInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.SignExtendedMoveInstruction(Context context)
		{
			var offset = context.Operand2;
			var type = context.Other as SigType;

			if (offset != null)
			{
				var eax = new RegisterOperand(BuiltInSigType.Int32, GeneralPurposeRegister.EAX);
				var destination = context.Result;
				var source = context.Operand1 as MemoryOperand;
				var elementType = type == null ? GetElementType(source.Type) : GetElementType(type);
				var constantOffset = offset as ConstantOperand;
				var offsetPtr = IntPtr.Zero;

				context.SetInstruction(Instruction.MovInstruction, eax, source);
				if (constantOffset != null)
				{
					offsetPtr = new IntPtr(Convert.ToInt64(constantOffset.Value));
				}
				else
				{
					context.AppendInstruction(Instruction.AddInstruction, eax, offset);
				}

				context.AppendInstruction(Instruction.MovsxInstruction, destination, new MemoryOperand(elementType, GeneralPurposeRegister.EAX, offsetPtr));
			}
			else
			{
				context.ReplaceInstructionOnly(Instruction.MovsxInstruction);
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
			if (context.OperandCount == 0 && context.Branch != null)
			{
				// inter-method call; usually for exception processing
				context.ReplaceInstructionOnly(Instruction.CallInstruction);
			}
			else
			{
				callingConvention.MakeCall(context);
			}
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

				context.SetInstruction(Instruction.MovInstruction, eax, source);
				if (constantOffset != null)
				{
					offsetPtr = new IntPtr(Convert.ToInt64(constantOffset.Value));
				}

				if (elementType.Type == CilElementType.Char ||
					elementType.Type == CilElementType.U1 ||
					elementType.Type == CilElementType.U2)
				{
					context.AppendInstruction(Instruction.AddInstruction, eax, offset);
				}
				context.AppendInstruction(Instruction.MovzxInstruction, result, new MemoryOperand(elementType, GeneralPurposeRegister.EAX, offsetPtr));
			}
			else
			{
				context.ReplaceInstructionOnly(Instruction.MovzxInstruction);
			}
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
						context.ReplaceInstructionOnly(Instruction.Cvttsd2siInstruction);
					else
						context.ReplaceInstructionOnly(Instruction.Cvttss2siInstruction);
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
		/// Visitation function for ThrowInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.ThrowInstruction(Context context)
		{
			RuntimeType runtimeType = typeSystem.GetType(@"Mosa.Internal.ExceptionEngine");
			RuntimeMethod runtimeMethod = runtimeType.FindMethod(@"ThrowException");
			SymbolOperand throwMethod = SymbolOperand.FromMethod(runtimeMethod);

			// Push exception object onto stack
			context.SetInstruction(Instruction.PushInstruction, null, context.Operand1);
			// Save entire CPU context onto stack
			context.AppendInstruction(Instruction.PushadInstruction);
			// Call exception handling
			context.AppendInstruction(Instruction.CallInstruction, null, throwMethod);
		}

		/// <summary>
		/// Visitation function for ExceptionPrologueInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.ExceptionPrologueInstruction(Context context)
		{
			// Exception Handler will pass the exception object in the register - EDX was choose
			context.SetInstruction(Instruction.MovInstruction, context.Result, new RegisterOperand(BuiltInSigType.Object, GeneralPurposeRegister.EDX));

			// Alternative method is to pop it off the stack instead, going passing via register for now
			//context.SetInstruction(CPUx86.Instruction.PopInstruction, context.Result);
		}

		#endregion //  IIRVisitor

		#region IIRVisitor - Unused

		/// <summary>
		/// Visitation function for IntegerToFloatingPointConversionInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.IntegerToFloatingPointConversionInstruction(Context context) { }

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
			RegisterOperand xmm5 = new RegisterOperand(BuiltInSigType.Double, SSE2Register.XMM5);
			RegisterOperand xmm6 = new RegisterOperand(BuiltInSigType.Double, SSE2Register.XMM6);
			Context before = context.InsertBefore();

			if (context.Result.Type.Type == CilElementType.R4)
			{
				before.SetInstruction(Instruction.Cvtss2sdInstruction, xmm5, context.Result);
				context.Result = xmm5;
			}

			if (context.Operand1.Type.Type == CilElementType.R4)
			{
				before.SetInstruction(Instruction.Cvtss2sdInstruction, xmm6, context.Operand1);
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