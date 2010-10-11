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
using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Platforms.x86
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

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		string IPipelineStage.Name { get { return @"X86.IRTransformationStage"; } }

		#endregion // IMethodCompilerStage Members

		#region IIRVisitor

		public void AddSInstruction(Context context)
		{
			this.HandleCommutativeOperation(context, CPUx86.Instruction.AddInstruction);
		}

		public void AddUInstruction(Context context)
		{
			this.HandleCommutativeOperation(context, CPUx86.Instruction.AddInstruction);
		}

		public void AddFInstruction(Context context)
		{
			this.HandleCommutativeOperation(context, CPUx86.Instruction.SseAddInstruction);
			ExtendToR8(context);
		}

		public void DivFInstruction(Context context)
		{
			this.HandleNonCommutativeOperation(context, CPUx86.Instruction.SseDivInstruction);
			ExtendToR8(context);
		}

		public void DivSInstruction(Context context)
		{
			this.HandleNonCommutativeOperation(context, CPUx86.Instruction.DivInstruction);
		}

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.AddressOfInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void AddressOfInstruction(Context ctx)
		{
			Operand opRes = ctx.Result;
			RegisterOperand eax = new RegisterOperand(opRes.Type, GeneralPurposeRegister.EAX);
			ctx.Result = eax;
			ctx.ReplaceInstructionOnly(CPUx86.Instruction.LeaInstruction);
			//ctx.Ignore = true;
			ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, opRes, eax);
		}

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.ArithmeticShiftRightInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void ArithmeticShiftRightInstruction(Context ctx)
		{
			HandleShiftOperation(ctx, CPUx86.Instruction.SarInstruction);
		}

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.EpilogueInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void EpilogueInstruction(Context ctx)
		{
			SigType I = new SigType(CilElementType.I);
			RegisterOperand ebx = new RegisterOperand(I, GeneralPurposeRegister.EBX);
			RegisterOperand edx = new RegisterOperand(I, GeneralPurposeRegister.EDX);
			RegisterOperand ebp = new RegisterOperand(I, GeneralPurposeRegister.EBP);
			RegisterOperand esp = new RegisterOperand(I, GeneralPurposeRegister.ESP);
			int stackSize = (int)ctx.Other;

			if (MethodCompiler.Method.Signature.ReturnType.Type == CilElementType.I8
				|| MethodCompiler.Method.Signature.ReturnType.Type == CilElementType.U8)
			{
				// pop ebx
				ctx.SetInstruction(CPUx86.Instruction.PopInstruction, ebx);
			}
			else
			{
				// pop edx
				ctx.SetInstruction(CPUx86.Instruction.PopInstruction, edx);
				// pop ebx
				ctx.AppendInstruction(CPUx86.Instruction.PopInstruction, ebx);
			}

			// add esp, -localsSize
			ctx.AppendInstruction(CPUx86.Instruction.AddInstruction, esp, new ConstantOperand(I, -stackSize));
			// pop ebp
			ctx.AppendInstruction(CPUx86.Instruction.PopInstruction, ebp);
			// ret
			ctx.AppendInstruction(CPUx86.Instruction.RetInstruction);
		}

		/// <summary>
		/// Floatings the point compare instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void FloatingPointCompareInstruction(Context ctx)
		{
			Operand resultOperand = ctx.Result;
			Operand left = EmitConstant(ctx.Operand1);
			Operand right = EmitConstant(ctx.Operand2);
			//ctx.Remove();
			ctx.Operand1 = left;
			ctx.Operand2 = right;


			// Swap the operands if necessary...
			if (left is MemoryOperand && right is RegisterOperand)
			{
				SwapComparisonOperands(ctx);
				left = ctx.Operand1;
				right = ctx.Operand2;
			}

            IR.ConditionCode setnp = IR.ConditionCode.NoParity;
            IR.ConditionCode setnc = IR.ConditionCode.NoCarry;
            IR.ConditionCode setc = IR.ConditionCode.Carry;
            IR.ConditionCode setnz = IR.ConditionCode.NoZero;
            IR.ConditionCode setz = IR.ConditionCode.Zero;
			IR.ConditionCode code = ctx.ConditionCode;

			ctx.SetInstruction(CPUx86.Instruction.NopInstruction);

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
					ctx.AppendInstruction(CPUx86.Instruction.MovssInstruction, xmm2, left);
				else
					ctx.AppendInstruction(CPUx86.Instruction.MovsdInstruction, xmm2, left);
				left = xmm2;
			}

			// Compare using the smallest precision
			if (left.Type.Type == CilElementType.R4 && right.Type.Type == CilElementType.R8)
			{
				RegisterOperand rop = new RegisterOperand(new SigType(CilElementType.R4), SSE2Register.XMM4);
				ctx.AppendInstruction(CPUx86.Instruction.Cvtsd2ssInstruction, rop, right);
				right = rop;
			}
			if (left.Type.Type == CilElementType.R8 && right.Type.Type == CilElementType.R4)
			{
				RegisterOperand rop = new RegisterOperand(new SigType(CilElementType.R4), SSE2Register.XMM3);
				ctx.AppendInstruction(CPUx86.Instruction.Cvtsd2ssInstruction, rop, left);
				left = rop;
			}

			if (left.Type.Type == CilElementType.R4)
			{
				switch (code)
				{
					case IR.ConditionCode.Equal:
						ctx.AppendInstruction(CPUx86.Instruction.ComissInstruction, left, right);
						break;
					case IR.ConditionCode.NotEqual: goto case IR.ConditionCode.Equal;
					case IR.ConditionCode.UnsignedGreaterOrEqual: goto case IR.ConditionCode.Equal;
					case IR.ConditionCode.UnsignedGreaterThan: goto case IR.ConditionCode.Equal;
					case IR.ConditionCode.UnsignedLessOrEqual: goto case IR.ConditionCode.Equal;
					case IR.ConditionCode.UnsignedLessThan: goto case IR.ConditionCode.Equal;
					case IR.ConditionCode.GreaterOrEqual:
						ctx.AppendInstruction(CPUx86.Instruction.ComissInstruction, left, right);
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
						ctx.AppendInstruction(CPUx86.Instruction.ComisdInstruction, left, right);
						break;
					case IR.ConditionCode.NotEqual: goto case IR.ConditionCode.Equal;
					case IR.ConditionCode.UnsignedGreaterOrEqual: goto case IR.ConditionCode.Equal;
					case IR.ConditionCode.UnsignedGreaterThan: goto case IR.ConditionCode.Equal;
					case IR.ConditionCode.UnsignedLessOrEqual: goto case IR.ConditionCode.Equal;
					case IR.ConditionCode.UnsignedLessThan: goto case IR.ConditionCode.Equal;
					case IR.ConditionCode.GreaterOrEqual:
						ctx.AppendInstruction(CPUx86.Instruction.ComisdInstruction, left, right);
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

            if (code == ConditionCode.Equal)
            {
                ctx.AppendInstruction(CPUx86.Instruction.SetccInstruction, setz, eax);
                ctx.AppendInstruction(CPUx86.Instruction.SetccInstruction, setnp, ebx);
                ctx.AppendInstruction(CPUx86.Instruction.SetccInstruction, setnc, ecx);
                ctx.AppendInstruction(CPUx86.Instruction.AndInstruction, eax, ebx);
                ctx.AppendInstruction(CPUx86.Instruction.AndInstruction, eax, ecx);
            }
            else if (code == ConditionCode.NotEqual)
            {
                ctx.AppendInstruction(CPUx86.Instruction.SetccInstruction, setz, eax);
                ctx.AppendInstruction(CPUx86.Instruction.SetccInstruction, setnp, ebx);
                ctx.AppendInstruction(CPUx86.Instruction.SetccInstruction, setnc, ecx);
                ctx.AppendInstruction(CPUx86.Instruction.AndInstruction, eax, ebx);
                ctx.AppendInstruction(CPUx86.Instruction.AndInstruction, eax, ecx);
                ctx.AppendInstruction(CPUx86.Instruction.NotInstruction, eax, eax);
                ctx.AppendInstruction(CPUx86.Instruction.AndInstruction, eax, new ConstantOperand(new SigType(CilElementType.I4), (int)1));
            }
            else if (code == ConditionCode.GreaterThan || code == ConditionCode.GreaterOrEqual)
            {
                ctx.AppendInstruction(CPUx86.Instruction.SetccInstruction, setnz, eax);
                ctx.AppendInstruction(CPUx86.Instruction.SetccInstruction, setnp, ebx);
                ctx.AppendInstruction(CPUx86.Instruction.SetccInstruction, setnc, ecx);
                ctx.AppendInstruction(CPUx86.Instruction.AndInstruction, eax, ebx);
                ctx.AppendInstruction(CPUx86.Instruction.AndInstruction, eax, ecx);
            }
            else if (code == ConditionCode.LessThan || code == ConditionCode.LessOrEqual)
            {
                ctx.AppendInstruction(CPUx86.Instruction.SetccInstruction, setnz, eax);
                ctx.AppendInstruction(CPUx86.Instruction.SetccInstruction, setnp, ebx);
                ctx.AppendInstruction(CPUx86.Instruction.SetccInstruction, setc, ecx);
                ctx.AppendInstruction(CPUx86.Instruction.AndInstruction, eax, ebx);
                ctx.AppendInstruction(CPUx86.Instruction.AndInstruction, eax, ecx);
            }
            if (code == ConditionCode.GreaterOrEqual || code == ConditionCode.LessOrEqual)
            {
                ctx.AppendInstruction(CPUx86.Instruction.SetccInstruction, setz, edx);
                ctx.AppendInstruction(CPUx86.Instruction.OrInstruction, eax, edx);
            }
			ctx.AppendInstruction(CPUx86.Instruction.MovzxInstruction, resultOperand, eax);
		}

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.IntegerCompareInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void IntegerCompareInstruction(Context ctx)
		{
			EmitOperandConstants(ctx);

			ConditionCode condition = ctx.ConditionCode;
			Operand resultOperand = ctx.Result;

			ctx.SetInstruction(CPUx86.Instruction.CmpInstruction, ctx.Operand1, ctx.Operand2);

			if (resultOperand != null)
			{
				RegisterOperand eax = new RegisterOperand(BuiltInSigType.Byte, GeneralPurposeRegister.EAX);

				if (IsUnsigned(resultOperand))
					ctx.AppendInstruction(CPUx86.Instruction.SetccInstruction, GetUnsignedConditionCode(condition), eax);
				else
					ctx.AppendInstruction(CPUx86.Instruction.SetccInstruction, condition, eax);

				ctx.AppendInstruction(CPUx86.Instruction.MovzxInstruction, resultOperand, eax);
			}
		}

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.JmpInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void JmpInstruction(Context ctx)
		{
			ctx.ReplaceInstructionOnly(CPUx86.Instruction.JmpInstruction);
		}

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.LoadInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void LoadInstruction(Context ctx)
		{
			RegisterOperand eax = new RegisterOperand(ctx.Operand1.Type, GeneralPurposeRegister.EAX);
			Operand result = ctx.Result;
			Operand operand = ctx.Operand1;
			Operand offset = ctx.Operand2;
			ConstantOperand constantOffset = offset as ConstantOperand;
			IntPtr offsetPtr = IntPtr.Zero;

			ctx.SetInstruction(CPUx86.Instruction.MovInstruction, eax, operand);
			if (constantOffset != null)
			{
				offsetPtr = new IntPtr(Convert.ToInt64(constantOffset.Value));
			}
			else
			{
				ctx.AppendInstruction(CPUx86.Instruction.AddInstruction, eax, offset);
			}

			ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, result, new MemoryOperand(eax.Type, GeneralPurposeRegister.EAX, offsetPtr));
		}

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.LogicalAndInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void LogicalAndInstruction(Context ctx)
		{
			ctx.ReplaceInstructionOnly(CPUx86.Instruction.AndInstruction);
		}

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.LogicalOrInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void LogicalOrInstruction(Context ctx)
		{
			ctx.ReplaceInstructionOnly(CPUx86.Instruction.OrInstruction);
		}

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.LogicalXorInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void LogicalXorInstruction(Context ctx)
		{
			ctx.ReplaceInstructionOnly(CPUx86.Instruction.XorInstruction);
		}

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.LogicalNotInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void LogicalNotInstruction(Context ctx)
		{
			Operand dest = ctx.Result;

			ctx.SetInstruction(CPUx86.Instruction.MovInstruction, ctx.Result, ctx.Operand1);
			if (dest.Type.Type == CilElementType.U1)
				ctx.AppendInstruction(CPUx86.Instruction.XorInstruction, dest, new ConstantOperand(new SigType(CilElementType.U4), (uint)0xFF));
			else if (dest.Type.Type == CilElementType.U2)
				ctx.AppendInstruction(CPUx86.Instruction.XorInstruction, dest, new ConstantOperand(new SigType(CilElementType.U4), (uint)0xFFFF));
			else
				ctx.AppendInstruction(CPUx86.Instruction.NotInstruction, dest);

		}

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.MoveInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void MoveInstruction(Context ctx)
		{
			Operand result = ctx.Result;
			Operand operand = ctx.Operand1;
			ctx.Operand1 = EmitConstant(ctx.Operand1);

			if (ctx.Result.StackType == StackTypeCode.F)
			{
				Debug.Assert(ctx.Operand1.StackType == StackTypeCode.F, @"Move can't convert to floating point type.");
				if (ctx.Result.Type.Type == ctx.Operand1.Type.Type)
				{
					if (ctx.Result.Type.Type == CilElementType.R4)
						MoveFloatingPoint(ctx, CPUx86.Instruction.MovssInstruction);
					else if (ctx.Result.Type.Type == CilElementType.R8)
						MoveFloatingPoint(ctx, CPUx86.Instruction.MovsdInstruction);
				}
				else if (ctx.Result.Type.Type == CilElementType.R8)
				{
					ctx.SetInstruction(CPUx86.Instruction.Cvtss2sdInstruction, ctx.Result, ctx.Operand1, ctx.Operand2);
				}
				else if (ctx.Result.Type.Type == CilElementType.R4)
				{
					ctx.SetInstruction(CPUx86.Instruction.Cvtsd2ssInstruction, ctx.Result, ctx.Operand1, ctx.Operand2);
				}
			}
			else
			{
				if (ctx.Result is MemoryOperand && ctx.Operand1 is MemoryOperand)
				{
					RegisterOperand load = new RegisterOperand(new SigType(CilElementType.I), GeneralPurposeRegister.EDX);
					RegisterOperand store = new RegisterOperand(operand.Type, GeneralPurposeRegister.EDX);

					if (!Is32Bit(operand) && IsSigned(operand))
						ctx.SetInstruction(CPUx86.Instruction.MovsxInstruction, load, operand);
					else if (!Is32Bit(operand) && IsUnsigned(operand))
						ctx.SetInstruction(CPUx86.Instruction.MovzxInstruction, load, operand);
					else
						ctx.SetInstruction(CPUx86.Instruction.MovInstruction, load, operand);

					ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, result, store);
				}
				else
					ctx.ReplaceInstructionOnly(CPUx86.Instruction.MovInstruction);
			}
		}

		private void MoveFloatingPoint(Context ctx, CPUx86.BaseInstruction instruction)
		{
			RegisterOperand xmm0 = new RegisterOperand(ctx.Result.Type, SSE2Register.XMM0);
			Operand result = ctx.Result;
			Operand operand = ctx.Operand1;
			ctx.SetInstruction(instruction, xmm0, operand);
			ctx.AppendInstruction(instruction, result, xmm0);
		}

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.PrologueInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void PrologueInstruction(Context ctx)
		{
			SigType I = new SigType(CilElementType.I4);
			RegisterOperand eax = new RegisterOperand(I, GeneralPurposeRegister.EAX);
			RegisterOperand ebx = new RegisterOperand(I, GeneralPurposeRegister.EBX);
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
			//ctx.SetInstruction(CPUx86.Instruction.BreakInstruction);
			//ctx.AppendInstruction(CPUx86.Instruction.PushInstruction, null, ebp);
			//ctx.AppendInstruction(CPUx86.Instruction.NopInstruction);

			// Uncomment this line to enable breakpoints within Bochs
			//ctx.XXX(CPUx86.Instruction.BochsDebug);

			// push ebp
			ctx.SetInstruction(CPUx86.Instruction.PushInstruction, null, ebp);

			// mov ebp, esp
			ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, ebp, esp);
			// sub esp, localsSize
			ctx.AppendInstruction(CPUx86.Instruction.SubInstruction, esp, new ConstantOperand(I, -stackSize));
			ctx.AppendInstruction(CPUx86.Instruction.PushInstruction, null, ebx);

			// Initialize all locals to zero
			ctx.AppendInstruction(CPUx86.Instruction.PushInstruction, null, edi);
			ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, edi, esp);
			ctx.AppendInstruction(CPUx86.Instruction.PushInstruction, null, ecx);
			ctx.AppendInstruction(CPUx86.Instruction.AddInstruction, edi, new ConstantOperand(I, 8));
			ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, ecx, new ConstantOperand(I, -(int)(stackSize >> 2)));
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
			ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, new MemoryOperand(I, GeneralPurposeRegister.EBP, new IntPtr(-4)), new ConstantOperand(I, MethodCompiler.Method.Token));

			// Do not save EDX for non-int64 return values
			if (MethodCompiler.Method.Signature.ReturnType.Type != CilElementType.I8 &&
				MethodCompiler.Method.Signature.ReturnType.Type != CilElementType.U8)
			{
				// push edx
				ctx.AppendInstruction(CPUx86.Instruction.PushInstruction, null, new RegisterOperand(I, GeneralPurposeRegister.EDX));
			}

			//ctx.AppendInstruction(CPUx86.Instruction.BreakInstruction);
		}

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.ReturnInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void ReturnInstruction(Context ctx)
		{
			Operand op = ctx.Operand1;

			if (op != null)
			{
				ICallingConvention cc = Architecture.GetCallingConvention();
				cc.MoveReturnValue(ctx, op);
				ctx.AppendInstruction(CPUx86.Instruction.JmpInstruction);
				ctx.SetBranch(Int32.MaxValue);
			}
			else
			{
				ctx.SetInstruction(CPUx86.Instruction.JmpInstruction);
				ctx.SetBranch(Int32.MaxValue);
			}
		}

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.ShiftLeftInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void ShiftLeftInstruction(Context ctx)
		{
			HandleShiftOperation(ctx, CPUx86.Instruction.ShlInstruction);
		}

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.ShiftRightInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void ShiftRightInstruction(Context ctx)
		{
			HandleShiftOperation(ctx, CPUx86.Instruction.ShrInstruction);
		}

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.StoreInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void StoreInstruction(Context ctx)
		{
			Operand destination = ctx.Result;
			Operand offset = ctx.Operand1;
			Operand value = ctx.Operand2;

			ConstantOperand constantOffset = offset as ConstantOperand;

			RegisterOperand eax = new RegisterOperand(destination.Type, GeneralPurposeRegister.EAX);
			RegisterOperand edx = new RegisterOperand(value.Type, GeneralPurposeRegister.EDX);

			ctx.SetInstruction(CPUx86.Instruction.MovInstruction, eax, destination);
			ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, edx, value);

			IntPtr offsetPtr = IntPtr.Zero;
			if (constantOffset != null)
			{
				offsetPtr = new IntPtr(Convert.ToInt64(constantOffset.Value));
			}
			else
			{
				ctx.AppendInstruction(CPUx86.Instruction.AddInstruction, eax, offset);
			}

			ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, new MemoryOperand(value.Type, GeneralPurposeRegister.EAX, offsetPtr), edx);
		}

		public void DivUInstruction(Context ctx)
		{
			ctx.ReplaceInstructionOnly(CPUx86.Instruction.UDivInstruction);
		}

		public void MulSInstruction(Context context)
		{
			HandleCommutativeOperation(context, CPUx86.Instruction.MulInstruction);
		}

		public void MulFInstruction(Context context)
		{
			HandleCommutativeOperation(context, CPUx86.Instruction.SseMulInstruction);
			ExtendToR8(context);
		}

		public void MulUInstruction(Context context)
		{
			HandleCommutativeOperation(context, CPUx86.Instruction.MulInstruction);
		}

		public void SubFInstruction(Context context)
		{
			HandleNonCommutativeOperation(context, CPUx86.Instruction.SseSubInstruction);
			ExtendToR8(context);
		}

		public void SubSInstruction(Context context)
		{
			HandleNonCommutativeOperation(context, CPUx86.Instruction.SubInstruction);
		}

		public void SubUInstruction(Context context)
		{
			HandleNonCommutativeOperation(context, CPUx86.Instruction.SubInstruction);
		}

		public void RemFInstruction(Context context)
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
            newBlocks[0].AppendInstruction(CPUx86.Instruction.BranchInstruction, ConditionCode.Equal, newBlocks[2].BasicBlock);
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

		public void RemSInstruction(Context context)
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
		/// Visitation function for <see cref="IIRVisitor.RemUInstruction"/> instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		public void RemUInstruction(Context context)
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

		public void SwitchInstruction(Context context)
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

		public void BreakInstruction(Context context)
		{
			context.SetInstruction(CPUx86.Instruction.BreakInstruction);
		}

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.NopInstruction"/> instructions.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void NopInstruction(Context ctx)
		{
			ctx.SetInstruction(CPUx86.Instruction.NopInstruction);
		}

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.SignExtendedMoveInstruction"/> instructions.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void SignExtendedMoveInstruction(Context ctx)
		{
			Operand offset = ctx.Operand2;
			if (offset != null)
			{
				RegisterOperand eax = new RegisterOperand(BuiltInSigType.Int32, GeneralPurposeRegister.EAX);
				Operand destination = ctx.Result;
				MemoryOperand source = (MemoryOperand)ctx.Operand1;
				SigType elementType = GetElementType(source.Type);
				ConstantOperand constantOffset = offset as ConstantOperand;
				IntPtr offsetPtr = IntPtr.Zero;

				ctx.SetInstruction(CPUx86.Instruction.MovInstruction, eax, source);
				if (constantOffset != null)
				{
					offsetPtr = new IntPtr(Convert.ToInt64(constantOffset.Value));
				}
				else
				{
					ctx.AppendInstruction(CPUx86.Instruction.AddInstruction, eax, offset);
				}

				ctx.AppendInstruction(CPUx86.Instruction.MovsxInstruction, destination, new MemoryOperand(elementType, GeneralPurposeRegister.EAX, offsetPtr));
			}
			else
			{
				ctx.ReplaceInstructionOnly(CPUx86.Instruction.MovsxInstruction);
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
		/// Visitation function for <see cref="IR.IIRVisitor.CallInstruction"/> instructions.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void CallInstruction(Context ctx)
		{
			ICallingConvention cc = Architecture.GetCallingConvention();
			Debug.Assert(null != cc, @"Failed to retrieve the calling convention.");
			cc.MakeCall(ctx, this.MethodCompiler.Method, this.MethodCompiler.TypeLayout);
		}

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.ZeroExtendedMoveInstruction"/> instructions.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void ZeroExtendedMoveInstruction(Context ctx)
		{
			Operand offset = ctx.Operand2;
			if (offset != null)
			{
				RegisterOperand eax = new RegisterOperand(ctx.Operand1.Type, GeneralPurposeRegister.EAX);
				Operand result = ctx.Result;
				Operand source = ctx.Operand1;
				SigType elementType = GetElementType(source.Type);
				ConstantOperand constantOffset = offset as ConstantOperand;
				IntPtr offsetPtr = IntPtr.Zero;

				ctx.SetInstruction(CPUx86.Instruction.MovInstruction, eax, source);
				if (constantOffset != null)
				{
					offsetPtr = new IntPtr(Convert.ToInt64(constantOffset.Value));
				}

				ctx.AppendInstruction(CPUx86.Instruction.AddInstruction, eax, offset);
				ctx.AppendInstruction(CPUx86.Instruction.MovzxInstruction, result, new MemoryOperand(elementType, GeneralPurposeRegister.EAX, offsetPtr));
			}
			else
			{
				ctx.ReplaceInstructionOnly(CPUx86.Instruction.MovzxInstruction);
			}
		}

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.BranchInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public void BranchInstruction(Context context)
		{
			context.ReplaceInstructionOnly(CPUx86.Instruction.BranchInstruction);
		}

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.FloatingPointToIntegerConversionInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public void FloatingPointToIntegerConversionInstruction(Context context)
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
		/// Visitation function for <see cref="IR.IIRVisitor.PopInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public void PopInstruction(Context context)
		{
			context.ReplaceInstructionOnly(CPUx86.Instruction.PopInstruction);
		}

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.PushInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public void PushInstruction(Context context)
		{
			context.ReplaceInstructionOnly(CPUx86.Instruction.PushInstruction);
		}

		#endregion //  IIRVisitor

		#region IIRVisitor - Unused

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

		#endregion // IIRVisitor - Unused

		#region Internals

		/// <summary>
		/// Extends to r8.
		/// </summary>
		/// <param name="ctx">The context.</param>
		private static void ExtendToR8(Context ctx)
		{
			RegisterOperand xmm5 = new RegisterOperand(new SigType(CilElementType.R8), SSE2Register.XMM5);
			RegisterOperand xmm6 = new RegisterOperand(new SigType(CilElementType.R8), SSE2Register.XMM6);
			Context before = ctx.InsertBefore();

			if (ctx.Result.Type.Type == CilElementType.R4)
			{
				before.SetInstruction(CPUx86.Instruction.Cvtss2sdInstruction, xmm5, ctx.Result);
				ctx.Result = xmm5;
			}

			if (ctx.Operand1.Type.Type == CilElementType.R4)
			{
				before.SetInstruction(CPUx86.Instruction.Cvtss2sdInstruction, xmm6, ctx.Operand1);
				ctx.Operand1 = xmm6;
			}
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
			ctx.ReplaceInstructionOnly(instruction);
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
			ctx.ReplaceInstructionOnly(instruction);
		}

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
			ctx.Operand1 = ctx.Operand2;
			ctx.Operand2 = op1;

			// Negate the condition code if necessary...
			switch (ctx.ConditionCode)
			{
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
