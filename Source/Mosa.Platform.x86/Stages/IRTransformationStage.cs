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
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Framework.Platform;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.TypeSystem;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	/// Transforms IR instructions into their appropriate X86.
	/// </summary>
	/// <remarks>
	/// This transformation stage transforms IR instructions into their equivalent X86 sequences.
	/// </remarks>
	public sealed class IRTransformationStage : BaseTransformationStage, IIRVisitor, IMethodCompilerStage, IPlatformStage
	{

		private int stackSize;

		#region IMethodCompilerStage

		/// <summary>
		/// Setup stage specific processing on the compiler context.
		/// </summary>
		/// <param name="methodCompiler">The compiler context to perform processing in.</param>
		void IMethodCompilerStage.Setup(BaseMethodCompiler methodCompiler)
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
		void IIRVisitor.AddSigned(Context context)
		{
			HandleCommutativeOperation(context, X86.Add);
		}

		/// <summary>
		/// Visitation function for AddUInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.AddUnsigned(Context context)
		{
			HandleCommutativeOperation(context, X86.Add);
		}

		/// <summary>
		/// Visitation function for AddFInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.AddFloat(Context context)
		{
			if (context.Result.Type.Type == CilElementType.R4)
				HandleCommutativeOperation(context, X86.AddSS);
			else
				HandleCommutativeOperation(context, X86.AddSD);

			ExtendToR8(context);
		}

		/// <summary>
		/// Visitation function for DivFInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.DivFloat(Context context)
		{
			if (context.Result.Type.Type == CilElementType.R4)
				HandleCommutativeOperation(context, X86.DivSS);
			else
				HandleCommutativeOperation(context, X86.DivSD);

			ExtendToR8(context);
		}

		/// <summary>
		/// Visitation function for DivSInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.DivSigned(Context context)
		{
			HandleNonCommutativeOperation(context, X86.IDiv);
		}

		/// <summary>
		/// Addresses the of instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.AddressOf(Context context)
		{
			var opRes = context.Result;

			Operand register = Operand.CreateCPURegister(opRes.Type, GeneralPurposeRegister.EAX);
			//VirtualRegisterOperand register = AllocateVirtualRegister(opRes.Type);

			context.Result = register;
			context.ReplaceInstructionOnly(X86.Lea);
			context.AppendInstruction(X86.Mov, opRes, register);
		}

		/// <summary>
		/// Arithmetic the shift right instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.ArithmeticShiftRight(Context context)
		{
			HandleShiftOperation(context, X86.Sar);
		}

		/// <summary>
		/// Floating point compare instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.FloatCompare(Context context)
		{
			Operand resultOperand = context.Result;
			Operand left = EmitConstant(context.Operand1);
			Operand right = EmitConstant(context.Operand2);

			context.Operand1 = left;
			context.Operand2 = right;

			// Swap the operands if necessary...
			if (left.IsMemoryAddress && right.IsRegister)
			{
				SwapComparisonOperands(context);
				left = context.Operand1;
				right = context.Operand2;
			}

			ConditionCode setnp = ConditionCode.NoParity;
			ConditionCode setnc = ConditionCode.NoCarry;
			ConditionCode setc = ConditionCode.Carry;
			ConditionCode setnz = ConditionCode.NoZero;
			ConditionCode setz = ConditionCode.Zero;
			ConditionCode code = context.ConditionCode;

			context.SetInstruction(X86.Nop);

			// x86 is messed up :(
			switch (code)
			{
				case ConditionCode.Equal: break;
				case ConditionCode.NotEqual: break;
				case ConditionCode.UnsignedGreaterOrEqual: code = ConditionCode.GreaterOrEqual; break;
				case ConditionCode.UnsignedGreaterThan: code = ConditionCode.GreaterThan; break;
				case ConditionCode.UnsignedLessOrEqual: code = ConditionCode.LessOrEqual; break;
				case ConditionCode.UnsignedLessThan: code = ConditionCode.LessThan; break;
			}

			if (!(left.IsRegister))
			{
				Operand xmm2 = Operand.CreateCPURegister(left.Type, SSE2Register.XMM2);
				if (left.Type.Type == CilElementType.R4)
					context.AppendInstruction(X86.Movss, xmm2, left);
				else
					context.AppendInstruction(X86.Movsd, xmm2, left);
				left = xmm2;
			}

			// Compare using the smallest precision
			if (left.Type.Type == CilElementType.R4 && right.Type.Type == CilElementType.R8)
			{
				Operand rop = Operand.CreateCPURegister(BuiltInSigType.Single, SSE2Register.XMM4);
				context.AppendInstruction(X86.Cvtsd2ss, rop, right);
				right = rop;
			}
			if (left.Type.Type == CilElementType.R8 && right.Type.Type == CilElementType.R4)
			{
				Operand rop = Operand.CreateCPURegister(BuiltInSigType.Single, SSE2Register.XMM3);
				context.AppendInstruction(X86.Cvtsd2ss, rop, left);
				left = rop;
			}

			if (left.Type.Type == CilElementType.R4)
			{
				switch (code)
				{
					case ConditionCode.Equal:
						context.AppendInstruction(X86.Comiss, null, left, right);
						break;
					case ConditionCode.NotEqual: goto case ConditionCode.Equal;
					case ConditionCode.UnsignedGreaterOrEqual: goto case ConditionCode.Equal;
					case ConditionCode.UnsignedGreaterThan: goto case ConditionCode.Equal;
					case ConditionCode.UnsignedLessOrEqual: goto case ConditionCode.Equal;
					case ConditionCode.UnsignedLessThan: goto case ConditionCode.Equal;
					case ConditionCode.GreaterOrEqual:
						context.AppendInstruction(X86.Comiss, null, left, right);
						break;
					case ConditionCode.GreaterThan: goto case ConditionCode.GreaterOrEqual;
					case ConditionCode.LessOrEqual: goto case ConditionCode.GreaterOrEqual;
					case ConditionCode.LessThan: goto case ConditionCode.GreaterOrEqual;
				}
			}
			else
			{
				switch (code)
				{
					case ConditionCode.Equal:
						context.AppendInstruction(X86.Comisd, null, left, right);
						break;
					case ConditionCode.NotEqual: goto case ConditionCode.Equal;
					case ConditionCode.UnsignedGreaterOrEqual: goto case ConditionCode.Equal;
					case ConditionCode.UnsignedGreaterThan: goto case ConditionCode.Equal;
					case ConditionCode.UnsignedLessOrEqual: goto case ConditionCode.Equal;
					case ConditionCode.UnsignedLessThan: goto case ConditionCode.Equal;
					case ConditionCode.GreaterOrEqual:
						context.AppendInstruction(X86.Comisd, null, left, right);
						break;
					case ConditionCode.GreaterThan: goto case ConditionCode.GreaterOrEqual;
					case ConditionCode.LessOrEqual: goto case ConditionCode.GreaterOrEqual;
					case ConditionCode.LessThan: goto case ConditionCode.GreaterOrEqual;
				}
			}

			// Determine the result
			Operand eax = Operand.CreateCPURegister(BuiltInSigType.Byte, GeneralPurposeRegister.EAX);
			Operand ebx = Operand.CreateCPURegister(BuiltInSigType.Byte, GeneralPurposeRegister.EBX);
			Operand ecx = Operand.CreateCPURegister(BuiltInSigType.Byte, GeneralPurposeRegister.ECX);
			Operand edx = Operand.CreateCPURegister(BuiltInSigType.Byte, GeneralPurposeRegister.EDX);
			//VirtualRegisterOperand eax = AllocateVirtualRegister(BuiltInSigType.Byte);
			//VirtualRegisterOperand ebx = AllocateVirtualRegister(BuiltInSigType.Byte);
			//VirtualRegisterOperand ecx = AllocateVirtualRegister(BuiltInSigType.Byte);
			//VirtualRegisterOperand edx = AllocateVirtualRegister(BuiltInSigType.Byte); 

			context.AppendInstruction(X86.Pushfd);

			if (code == ConditionCode.Equal)
			{
				context.AppendInstruction(X86.Setcc, setz, eax);
				context.AppendInstruction(X86.Setcc, setnp, ebx);
				context.AppendInstruction(X86.Setcc, setnc, ecx);
				context.AppendInstruction(X86.Setcc, setnz, edx);
				context.AppendInstruction(X86.And, eax, ebx);
				context.AppendInstruction(X86.And, eax, ecx);
				context.AppendInstruction(X86.Or, ebx, ecx);
				context.AppendInstruction(X86.Or, ebx, edx);
				context.AppendInstruction(X86.And, eax, ebx);
				context.AppendInstruction(X86.And, eax, Operand.CreateConstant(BuiltInSigType.Int32, (int)1));
			}
			else if (code == ConditionCode.NotEqual)
			{
				context.AppendInstruction(X86.Setcc, setz, eax);
				context.AppendInstruction(X86.Setcc, setnp, ebx);
				context.AppendInstruction(X86.Setcc, setnc, ecx);
				context.AppendInstruction(X86.Setcc, setnz, edx);
				context.AppendInstruction(X86.And, eax, ebx);
				context.AppendInstruction(X86.And, eax, ecx);
				context.AppendInstruction(X86.Or, ebx, ecx);
				context.AppendInstruction(X86.Or, ebx, edx);
				context.AppendInstruction(X86.Not, ebx, ebx);
				context.AppendInstruction(X86.Or, eax, ebx);
				context.AppendInstruction(X86.And, eax, Operand.CreateConstant(BuiltInSigType.Int32, (int)1));
			}
			else if (code == ConditionCode.GreaterThan)
			{
				context.AppendInstruction(X86.Setcc, setnz, eax);
				context.AppendInstruction(X86.Setcc, setnp, ebx);
				context.AppendInstruction(X86.Setcc, setnc, ecx);
				context.AppendInstruction(X86.And, eax, ebx);
				context.AppendInstruction(X86.And, eax, ecx);
			}
			else if (code == ConditionCode.LessThan)
			{
				context.AppendInstruction(X86.Setcc, setnz, eax);
				context.AppendInstruction(X86.Setcc, setnp, ebx);
				context.AppendInstruction(X86.Setcc, setc, ecx);
				context.AppendInstruction(X86.And, eax, ebx);
				context.AppendInstruction(X86.And, eax, ecx);
			}
			else if (code == ConditionCode.GreaterOrEqual)
			{
				context.AppendInstruction(X86.Setcc, setnz, eax);
				context.AppendInstruction(X86.Setcc, setnp, ebx);
				context.AppendInstruction(X86.Setcc, setnc, ecx);
				context.AppendInstruction(X86.Setcc, setz, edx);
				context.AppendInstruction(X86.And, eax, ebx);
				context.AppendInstruction(X86.And, eax, ecx);
				context.AppendInstruction(X86.Or, eax, edx);
			}
			else if (code == ConditionCode.LessOrEqual)
			{
				context.AppendInstruction(X86.Setcc, setnz, eax);
				context.AppendInstruction(X86.Setcc, setnp, ebx);
				context.AppendInstruction(X86.Setcc, setc, ecx);
				context.AppendInstruction(X86.Setcc, setz, edx);
				context.AppendInstruction(X86.And, eax, ebx);
				context.AppendInstruction(X86.And, eax, ecx);
				context.AppendInstruction(X86.Or, eax, edx);
			}
			context.AppendInstruction(X86.Movzx, resultOperand, eax);
			context.AppendInstruction(X86.Popfd);
		}

		/// <summary>
		/// Visitation function for IntegerCompareBranchInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.IntegerCompareBranch(Context context)
		{
			EmitOperandConstants(context);

			int target = context.BranchTargets[0];
			var condition = context.ConditionCode;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			context.SetInstruction(X86.Cmp, null, operand1, operand2);
			context.AppendInstruction(X86.Branch, condition);
			context.SetBranch(target);
		}

		/// <summary>
		/// Visitation function for IntegerCompareInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.IntegerCompare(Context context)
		{
			EmitOperandConstants(context);

			var condition = context.ConditionCode;
			var resultOperand = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			context.SetInstruction(X86.Cmp, null, operand1, operand2);

			if (resultOperand != null)
			{
				Operand eax = Operand.CreateCPURegister(BuiltInSigType.Byte, GeneralPurposeRegister.EAX);
				//VirtualRegisterOperand eax = AllocateVirtualRegister(BuiltInSigType.Byte); 

				if (IsUnsigned(resultOperand))
					context.AppendInstruction(X86.Setcc, GetUnsignedConditionCode(condition), eax);
				else
					context.AppendInstruction(X86.Setcc, condition, eax);

				context.AppendInstruction(X86.Movzx, resultOperand, eax);
			}
		}

		/// <summary>
		/// Visitation function for JmpInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.Jmp(Context context)
		{
			context.ReplaceInstructionOnly(X86.Jmp);
		}

		/// <summary>
		/// Visitation function for LoadInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.Load(Context context)
		{
			Operand result = context.Result;
			Operand operand = context.Operand1;
			Operand offset = context.Operand2;
			IntPtr offsetPtr = IntPtr.Zero;

			Operand eax = Operand.CreateCPURegister(operand.Type, GeneralPurposeRegister.EAX);

			context.SetInstruction(X86.Mov, eax, operand);

			if (offset.IsConstant)
			{
				offsetPtr = new IntPtr(Convert.ToInt64(offset.Value));
			}
			else
			{
				context.AppendInstruction(X86.Add, eax, offset);
			}

			context.AppendInstruction(X86.Mov, result, Operand.CreateMemoryAddress(eax.Type, GeneralPurposeRegister.EAX, offsetPtr));
		}

		/// <summary>
		/// Visitation function for Load Sign Extended.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.LoadSignExtended(Context context)
		{
			var destination = context.Result;
			var source = context.Operand1;
			var type = context.SigType;
			var offset = context.Operand2;

			var eax = Operand.CreateCPURegister(BuiltInSigType.Int32, GeneralPurposeRegister.EAX);
			var elementType = GetElementType(type);
			var offsetPtr = IntPtr.Zero;

			context.SetInstruction(X86.Mov, eax, source);

			if (offset.IsConstant)
			{
				offsetPtr = new IntPtr(Convert.ToInt64(offset.Value));
			}
			else
			{
				context.AppendInstruction(X86.Add, eax, offset);
			}

			context.AppendInstruction(X86.Movsx, destination, Operand.CreateMemoryAddress(elementType, GeneralPurposeRegister.EAX, offsetPtr));
		}

		/// <summary>
		/// Visitation function for Load Zero Extended.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.LoadZeroExtended(Context context)
		{
			var destination = context.Result;
			var source = context.Operand1;
			var type = context.SigType;
			var offset = context.Operand2;

			Debug.Assert(offset != null);

			Operand eax = Operand.CreateCPURegister(source.Type, GeneralPurposeRegister.EAX);
			SigType elementType = GetElementType(source.Type);
			IntPtr offsetPtr = IntPtr.Zero;

			context.SetInstruction(X86.Mov, eax, source);

			if (offset.IsConstant)
			{
				offsetPtr = new IntPtr(Convert.ToInt64(offset.Value));
			}

			if (elementType.Type == CilElementType.Char ||
				elementType.Type == CilElementType.U1 ||
				elementType.Type == CilElementType.U2)
			{
				context.AppendInstruction(X86.Add, eax, offset);
			}
			context.AppendInstruction(X86.Movzx, destination, Operand.CreateMemoryAddress(elementType, GeneralPurposeRegister.EAX, offsetPtr));
		}

		/// <summary>
		/// Visitation function for LogicalAndInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.LogicalAnd(Context context)
		{
			context.ReplaceInstructionOnly(X86.And);
		}

		/// <summary>
		/// Visitation function for LogicalOrInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.LogicalOr(Context context)
		{
			context.ReplaceInstructionOnly(X86.Or);
		}

		/// <summary>
		/// Visitation function for LogicalXorInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.LogicalXor(Context context)
		{
			context.ReplaceInstructionOnly(X86.Xor);
		}

		/// <summary>
		/// Visitation function for LogicalNotInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.LogicalNot(Context context)
		{
			Operand dest = context.Result;

			context.SetInstruction(X86.Mov, context.Result, context.Operand1);
			if (dest.Type.Type == CilElementType.U1)
				context.AppendInstruction(X86.Xor, dest, Operand.CreateConstant(BuiltInSigType.UInt32, (uint)0xFF));
			else if (dest.Type.Type == CilElementType.U2)
				context.AppendInstruction(X86.Xor, dest, Operand.CreateConstant(BuiltInSigType.UInt32, (uint)0xFFFF));
			else
				context.AppendInstruction(X86.Not, dest, dest);
		}

		/// <summary>
		/// Visitation function for MoveInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.Move(Context context)
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
						MoveFloatingPoint(context, X86.Movss);
					else if (context.Result.Type.Type == CilElementType.R8)
						MoveFloatingPoint(context, X86.Movsd);
				}
				else if (context.Result.Type.Type == CilElementType.R8)
				{
					context.SetInstruction(X86.Cvtss2sd, context.Result, context.Operand1);
				}
				else if (context.Result.Type.Type == CilElementType.R4)
				{
					context.SetInstruction(X86.Cvtsd2ss, context.Result, context.Operand1);
				}
			}
			else
			{
				if (context.Result.IsMemoryAddress && context.Operand1.IsMemoryAddress)
				{
					Operand load = Operand.CreateCPURegister(BuiltInSigType.IntPtr, GeneralPurposeRegister.EDX);
					Operand store = Operand.CreateCPURegister(operand.Type, GeneralPurposeRegister.EDX);

					if (!Is32Bit(operand) && IsSigned(operand))
						context.SetInstruction(X86.Movsx, load, operand);
					else if (!Is32Bit(operand) && IsUnsigned(operand))
						context.SetInstruction(X86.Movzx, load, operand);
					else
						context.SetInstruction(X86.Mov, load, operand);

					context.AppendInstruction(X86.Mov, result, store);
				}
				else
					context.ReplaceInstructionOnly(X86.Mov);
			}
		}

		private void MoveFloatingPoint(Context context, X86Instruction instruction)
		{
			Operand xmm0 = Operand.CreateCPURegister(context.Result.Type, SSE2Register.XMM0);
			Operand result = context.Result;
			Operand operand = context.Operand1;
			context.SetInstruction(instruction, xmm0, operand);
			context.AppendInstruction(instruction, result, xmm0);
		}

		/// <summary>
		/// Visitation function for PrologueInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.Prologue(Context context)
		{
			Operand eax = Operand.CreateCPURegister(BuiltInSigType.Int32, GeneralPurposeRegister.EAX);
			Operand ebx = Operand.CreateCPURegister(BuiltInSigType.Int32, GeneralPurposeRegister.EBX);
			Operand ecx = Operand.CreateCPURegister(BuiltInSigType.Int32, GeneralPurposeRegister.ECX);
			Operand ebp = Operand.CreateCPURegister(BuiltInSigType.Int32, GeneralPurposeRegister.EBP);
			Operand esp = Operand.CreateCPURegister(BuiltInSigType.Int32, GeneralPurposeRegister.ESP);
			Operand edi = Operand.CreateCPURegister(BuiltInSigType.Int32, GeneralPurposeRegister.EDI);
			Operand edx = Operand.CreateCPURegister(BuiltInSigType.Int32, GeneralPurposeRegister.EDX);

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
				context.SetInstruction(X86.Break);
				context.AppendInstruction(X86.Nop);

				// Uncomment this line to enable breakpoints within Bochs
				//context.AppendInstruction(CPUx86.Instruction.BochsDebug);
			}

			// push ebp
			context.SetInstruction(X86.Push, null, ebp);
			// mov ebp, esp
			context.AppendInstruction(X86.Mov, ebp, esp);
			// sub esp, localsSize
			context.AppendInstruction(X86.Sub, esp, Operand.CreateConstant(BuiltInSigType.Int32, -stackSize));
			// push ebx
			context.AppendInstruction(X86.Push, null, ebx);

			// Initialize all locals to zero
			context.AppendInstruction(X86.Push, null, edi);
			context.AppendInstruction(X86.Push, null, ecx);
			context.AppendInstruction(X86.Mov, edi, esp);
			context.AppendInstruction(X86.Add, edi, Operand.CreateConstant(BuiltInSigType.Int32, 3 * 4)); // 4 bytes per push above
			context.AppendInstruction(X86.Mov, ecx, Operand.CreateConstant(BuiltInSigType.Int32, -(int)(stackSize >> 2)));
			context.AppendInstruction(X86.Xor, eax, eax);
			context.AppendInstruction(X86.Rep);
			context.AppendInstruction(X86.Stos);

			// Save EDX for int32 return values (or do not save EDX for non-int64 return values)
			if (methodCompiler.Method.Signature.ReturnType.Type != CilElementType.I8 &&
				methodCompiler.Method.Signature.ReturnType.Type != CilElementType.U8)
			{
				// push edx
				context.AppendInstruction(X86.Push, null, edx);
			}

		}

		/// <summary>
		/// Visitation function for EpilogueInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.Epilogue(Context context)
		{
			Operand ebx = Operand.CreateCPURegister(BuiltInSigType.Int32, GeneralPurposeRegister.EBX);
			Operand edx = Operand.CreateCPURegister(BuiltInSigType.Int32, GeneralPurposeRegister.EDX);
			Operand ebp = Operand.CreateCPURegister(BuiltInSigType.Int32, GeneralPurposeRegister.EBP);
			Operand esp = Operand.CreateCPURegister(BuiltInSigType.Int32, GeneralPurposeRegister.ESP);
			Operand ecx = Operand.CreateCPURegister(BuiltInSigType.Int32, GeneralPurposeRegister.ECX);
			Operand edi = Operand.CreateCPURegister(BuiltInSigType.Int32, GeneralPurposeRegister.EDI);

			// Load EDX for int32 return values
			if (methodCompiler.Method.Signature.ReturnType.Type != CilElementType.I8 &&
				methodCompiler.Method.Signature.ReturnType.Type != CilElementType.U8)
			{
				// pop edx
				context.SetInstruction(X86.Pop, edx);
				context.AppendInstruction(X86.Nop);
			}

			// pop ecx
			context.SetInstruction(X86.Pop, ecx);
			// pop edi
			context.AppendInstruction(X86.Pop, edi);
			// pop ebx
			context.AppendInstruction(X86.Pop, ebx);
			// add esp, -localsSize
			context.AppendInstruction(X86.Add, esp, Operand.CreateConstant(BuiltInSigType.IntPtr, -stackSize));
			// pop ebp
			context.AppendInstruction(X86.Pop, ebp);
			// ret
			context.AppendInstruction(X86.Ret);
		}

		/// <summary>
		/// Visitation function for ReturnInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.Return(Context context)
		{
			if (context.BranchTargets == null)
			{
				// To return from an internal method call (usually from within a finally or exception clause)
				context.SetInstruction(X86.Ret);
				return;
			}

			if (context.Operand1 != null)
			{
				callingConvention.MoveReturnValue(context, context.Operand1);
				context.AppendInstruction(X86.Jmp);
				context.SetBranch(Int32.MaxValue);
			}
			else
			{
				context.SetInstruction(X86.Jmp);
				context.SetBranch(Int32.MaxValue);
			}
		}

		/// <summary>
		/// Visitation function for ShiftLeftInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.ShiftLeft(Context context)
		{
			HandleShiftOperation(context, X86.Shl);
		}

		/// <summary>
		/// Visitation function for ShiftRightInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.ShiftRight(Context context)
		{
			HandleShiftOperation(context, X86.Shr);
		}

		/// <summary>
		/// Visitation function for StoreInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.Store(Context context)
		{
			Operand destination = context.Operand1;
			Operand offset = context.Operand2;
			Operand value = context.Operand3;

			Operand eax = Operand.CreateCPURegister(destination.Type, GeneralPurposeRegister.EAX);
			Operand edx = Operand.CreateCPURegister(value.Type, GeneralPurposeRegister.EDX);

			context.SetInstruction(X86.Mov, eax, destination);
			context.AppendInstruction(X86.Mov, edx, value);

			IntPtr offsetPtr = IntPtr.Zero;
			if (offset.IsConstant)
			{
				offsetPtr = new IntPtr(Convert.ToInt64(offset.Value));
			}
			else
			{
				context.AppendInstruction(X86.Add, eax, offset);
			}

			context.AppendInstruction(X86.Mov, Operand.CreateMemoryAddress(value.Type, GeneralPurposeRegister.EAX, offsetPtr), edx);
		}

		/// <summary>
		/// Visitation function for DivUInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.DivUnsigned(Context context)
		{
			context.ReplaceInstructionOnly(X86.Div);

			Operand edx = Operand.CreateCPURegister(BuiltInSigType.IntPtr, GeneralPurposeRegister.EDX);
			Context before = context.InsertBefore();
			before.SetInstruction(X86.Xor, edx, edx);

			if (context.Operand1.IsConstant)
			{
				Operand ecx = Operand.CreateCPURegister(context.Operand1.Type, GeneralPurposeRegister.ECX);
				before.AppendInstruction(X86.Mov, ecx, context.Operand1);
				context.Operand1 = ecx;
			}
		}

		/// <summary>
		/// Visitation function for MulSInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.MulSigned(Context context)
		{
			HandleCommutativeOperation(context, X86.Mul);
		}

		/// <summary>
		/// Visitation function for MulFInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.MulFloat(Context context)
		{
			if (context.Result.Type.Type == CilElementType.R4)
				HandleCommutativeOperation(context, X86.MulSS);
			else
				HandleCommutativeOperation(context, X86.MulSD);

			ExtendToR8(context);
		}

		/// <summary>
		/// Visitation function for MulUInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.MulUnsigned(Context context)
		{
			HandleCommutativeOperation(context, X86.Mul);
		}

		/// <summary>
		/// Visitation function for SubFInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.SubFloat(Context context)
		{
			if (context.Result.Type.Type == CilElementType.R4)
				HandleCommutativeOperation(context, X86.SubSS);
			else
				HandleCommutativeOperation(context, X86.SubSD);

			ExtendToR8(context);
		}

		/// <summary>
		/// Visitation function for SubSInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.SubSigned(Context context)
		{
			HandleNonCommutativeOperation(context, X86.Sub);
		}

		/// <summary>
		/// Visitation function for SubUInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.SubUnsigned(Context context)
		{
			HandleNonCommutativeOperation(context, X86.Sub);
		}

		/// <summary>
		/// Visitation function for RemFInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.RemFloat(Context context)
		{
			if (context.Result.Type.Type == CilElementType.R4)
				HandleCommutativeOperation(context, X86.DivSS);
			else
				HandleCommutativeOperation(context, X86.DivSD);

			ExtendToR8(context);

			Operand destination = context.Result;
			Operand source = context.Operand1;

			Context[] newBlocks = CreateEmptyBlockContexts(context.Label, 3);
			Context nextBlock = SplitContext(context, false);

			Operand xmm5 = Operand.CreateCPURegister(BuiltInSigType.Double, SSE2Register.XMM5);
			Operand xmm6 = Operand.CreateCPURegister(BuiltInSigType.Double, SSE2Register.XMM6);
			Operand edx = Operand.CreateCPURegister(BuiltInSigType.Int32, GeneralPurposeRegister.EDX);

			context.SetInstruction(X86.Jmp, newBlocks[0].BasicBlock);
			LinkBlocks(context, newBlocks[0]);

			newBlocks[0].SetInstruction(X86.Movsd, xmm5, source);
			newBlocks[0].AppendInstruction(X86.Movsd, xmm6, destination);

			if (source.Type.Type == CilElementType.R4)
				newBlocks[0].AppendInstruction(X86.DivSS, destination, source);
			else
				newBlocks[0].AppendInstruction(X86.DivSD, destination, source);

			newBlocks[0].AppendInstruction(X86.Cvttsd2si, edx, destination);

			newBlocks[0].AppendInstruction(X86.Cmp, null, edx, Operand.CreateConstant(BuiltInSigType.Int32, (int)0));
			newBlocks[0].AppendInstruction(X86.Branch, ConditionCode.Equal, newBlocks[2].BasicBlock);
			newBlocks[0].AppendInstruction(X86.Jmp, newBlocks[1].BasicBlock);
			LinkBlocks(newBlocks[0], newBlocks[1], newBlocks[2]);

			newBlocks[1].AppendInstruction(X86.Cvtsi2sd, destination, edx);

			if (xmm5.Type.Type == CilElementType.R4)
				newBlocks[1].AppendInstruction(X86.MulSS, destination, xmm5);
			else
				newBlocks[1].AppendInstruction(X86.MulSD, destination, xmm5);

			if (destination.Type.Type == CilElementType.R4)
				newBlocks[1].AppendInstruction(X86.SubSS, xmm6, destination);
			else
				newBlocks[1].AppendInstruction(X86.SubSD, xmm6, destination);

			newBlocks[1].AppendInstruction(X86.Movsd, destination, xmm6);
			newBlocks[1].AppendInstruction(X86.Jmp, nextBlock.BasicBlock);
			LinkBlocks(newBlocks[1], nextBlock);

			newBlocks[2].SetInstruction(X86.Movsd, destination, xmm6);
			newBlocks[2].AppendInstruction(X86.Jmp, nextBlock.BasicBlock);
			LinkBlocks(newBlocks[2], nextBlock);
		}

		/// <summary>
		/// Visitation function for RemSInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.RemSigned(Context context)
		{
			Operand result = context.Result;
			Operand operand = context.Operand1;
			Operand eax = Operand.CreateCPURegister(BuiltInSigType.Int32, GeneralPurposeRegister.EAX);
			Operand ecx = Operand.CreateCPURegister(BuiltInSigType.Int32, GeneralPurposeRegister.ECX);
			Operand edx = Operand.CreateCPURegister(BuiltInSigType.Int32, GeneralPurposeRegister.EDX);
			Operand eaxSource = Operand.CreateCPURegister(result.Type, GeneralPurposeRegister.EAX);
			Operand ecxSource = Operand.CreateCPURegister(operand.Type, GeneralPurposeRegister.ECX);

			context.SetInstruction(X86.Mov, eaxSource, result);
			context.AppendInstruction(IRInstruction.SignExtendedMove, eax, eaxSource);
			context.AppendInstruction(X86.Mov, ecxSource, operand);
			context.AppendInstruction(IRInstruction.SignExtendedMove, ecx, ecxSource);
			context.AppendInstruction(X86.IDiv, eax, ecx);
			context.AppendInstruction(X86.Mov, result, edx);
		}

		/// <summary>
		/// Visitation function for RemUInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.RemUnsigned(Context context)
		{
			Operand result = context.Result;
			Operand operand = context.Operand1;
			Operand eax = Operand.CreateCPURegister(BuiltInSigType.UInt32, GeneralPurposeRegister.EAX);
			Operand ecx = Operand.CreateCPURegister(BuiltInSigType.UInt32, GeneralPurposeRegister.ECX);
			Operand edx = Operand.CreateCPURegister(BuiltInSigType.IntPtr, GeneralPurposeRegister.EDX);
			Operand eaxSource = Operand.CreateCPURegister(result.Type, GeneralPurposeRegister.EAX);
			Operand ecxSource = Operand.CreateCPURegister(operand.Type, GeneralPurposeRegister.ECX);

			context.SetInstruction(X86.Mov, eaxSource, result);
			context.AppendInstruction(IRInstruction.ZeroExtendedMove, eax, eaxSource);
			context.AppendInstruction(X86.Mov, ecxSource, operand);
			context.AppendInstruction(IRInstruction.ZeroExtendedMove, ecx, ecxSource);

			context.AppendInstruction(X86.Xor, edx, edx);
			context.AppendInstruction(X86.Div, eax, ecx);

			context.AppendInstruction(X86.Mov, result, Operand.CreateCPURegister(BuiltInSigType.UInt32, GeneralPurposeRegister.EDX));
		}

		/// <summary>
		/// Visitation function for SwitchInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.Switch(Context context)
		{
			int[] targets = context.BranchTargets;
			Operand operand = context.Operand1;

			context.Remove();

			for (int i = 0; i < targets.Length - 1; ++i)
			{
				context.AppendInstruction(X86.Cmp, null, operand, Operand.CreateConstant(BuiltInSigType.IntPtr, i));
				context.AppendInstruction(X86.Branch, ConditionCode.Equal);
				context.SetBranch(targets[i]);
			}
		}

		/// <summary>
		/// Visitation function for BreakInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.Break(Context context)
		{
			context.SetInstruction(X86.Break);
		}

		/// <summary>
		/// Visitation function for NopInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.Nop(Context context)
		{
			context.SetInstruction(X86.Nop);
		}

		/// <summary>
		/// Visitation function for SignExtendedMoveInstruction instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.SignExtendedMove(Context context)
		{
			context.ReplaceInstructionOnly(X86.Movsx);
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
		/// Visitation function for Call.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.Call(Context context)
		{
			if (context.OperandCount == 0 && context.BranchTargets != null)
			{
				// inter-method call; usually for exception processing
				context.ReplaceInstructionOnly(X86.Call);
			}
			else
			{
				callingConvention.MakeCall(context);
			}
		}

		/// <summary>
		/// Visitation function for ZeroExtendedMoveInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.ZeroExtendedMove(Context context)
		{
			context.ReplaceInstructionOnly(X86.Movzx);
		}

		/// <summary>
		/// Visitation function for FloatingPointToIntegerConversionInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.FloatToIntegerConversion(Context context)
		{
			Operand source = context.Operand1;
			Operand destination = context.Result;
			switch (destination.Type.Type)
			{
				case CilElementType.I1: goto case CilElementType.I4;
				case CilElementType.I2: goto case CilElementType.I4;
				case CilElementType.I4:
					if (source.Type.Type == CilElementType.R8)
						context.ReplaceInstructionOnly(X86.Cvttsd2si);
					else
						context.ReplaceInstructionOnly(X86.Cvttss2si);
					break;
				case CilElementType.I8: return; // FIXME: throw new NotSupportedException();
				case CilElementType.U1: goto case CilElementType.U4;
				case CilElementType.U2: goto case CilElementType.U4;
				case CilElementType.U4: return; // FIXME: throw new NotSupportedException();
				case CilElementType.U8: return; // FIXME: throw new NotSupportedException();
				case CilElementType.I: goto case CilElementType.I4;
				case CilElementType.U: goto case CilElementType.U4;
			}
		}

		/// <summary>
		/// Visitation function for ThrowInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.Throw(Context context)
		{
			RuntimeType runtimeType = typeSystem.GetType(@"Mosa.Internal.ExceptionEngine");
			RuntimeMethod runtimeMethod = runtimeType.FindMethod(@"ThrowException");
			Operand throwMethod = Operand.CreateSymbolFromMethod(runtimeMethod);

			// Push exception object onto stack
			context.SetInstruction(X86.Push, null, context.Operand1);
			// Save entire CPU context onto stack
			context.AppendInstruction(X86.Pushad);
			// Call exception handling
			context.AppendInstruction(X86.Call, null, throwMethod);
		}

		/// <summary>
		/// Visitation function for ExceptionPrologueInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.ExceptionPrologue(Context context)
		{
			// Exception Handler will pass the exception object in the register - EDX was choosen
			context.SetInstruction(X86.Mov, context.Result, Operand.CreateCPURegister(BuiltInSigType.Object, GeneralPurposeRegister.EDX));

			// Alternative method is to pop it off the stack instead, going passing via register for now
			//context.SetInstruction(CPUx86.Instruction.PopInstruction, context.Result);
		}

		/// <summary>
		/// Visitation function for IntegerToFloatingPointConversion.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.IntegerToFloatConversion(Context context)
		{
			if (context.Result.Type.Type == CilElementType.R4)
				context.ReplaceInstructionOnly(X86.Cvtsi2ss);
			else if (context.Result.Type.Type == CilElementType.R8)
				context.ReplaceInstructionOnly(X86.Cvtsi2sd);
			else
				throw new NotSupportedException();
		}

		#endregion //  IIRVisitor

		#region IIRVisitor - Unused

		/// <summary>
		/// Visitation function for PhiInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.Phi(Context context) { }

		/// <summary>
		/// Visitation function for intrinsic the method call.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.IntrinsicMethodCall(Context context) { }

		#endregion // IIRVisitor - Unused

		#region Internals

		/// <summary>
		/// Extends to r8.
		/// </summary>
		/// <param name="context">The context.</param>
		private static void ExtendToR8(Context context)
		{
			Operand xmm5 = Operand.CreateCPURegister(BuiltInSigType.Double, SSE2Register.XMM5);
			Operand xmm6 = Operand.CreateCPURegister(BuiltInSigType.Double, SSE2Register.XMM6);
			Context before = context.InsertBefore();

			if (context.Result.Type.Type == CilElementType.R4)
			{
				before.SetInstruction(X86.Cvtss2sd, xmm5, context.Result);
				context.Result = xmm5;
			}

			if (context.Operand1.Type.Type == CilElementType.R4)
			{
				before.SetInstruction(X86.Cvtss2sd, xmm6, context.Operand1);
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
		private void HandleCommutativeOperation(Context context, BaseInstruction instruction)
		{
			EmitOperandConstants(context);
			context.ReplaceInstructionOnly(instruction);
		}

		/// <summary>
		/// Handles the non commutative operation.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="instruction">The instruction.</param>
		private void HandleNonCommutativeOperation(Context context, BaseInstruction instruction)
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
		private void HandleShiftOperation(Context context, BaseInstruction instruction)
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

			// Negate the condition code if necessary.
			switch (context.ConditionCode)
			{
				case ConditionCode.Equal: break;
				case ConditionCode.GreaterOrEqual: context.ConditionCode = ConditionCode.LessThan; break;
				case ConditionCode.GreaterThan: context.ConditionCode = ConditionCode.LessOrEqual; break;
				case ConditionCode.LessOrEqual: context.ConditionCode = ConditionCode.GreaterThan; break;
				case ConditionCode.LessThan: context.ConditionCode = ConditionCode.GreaterOrEqual; break;
				case ConditionCode.NotEqual: break;
				case ConditionCode.UnsignedGreaterOrEqual: context.ConditionCode = ConditionCode.UnsignedLessThan; break;
				case ConditionCode.UnsignedGreaterThan: context.ConditionCode = ConditionCode.UnsignedLessOrEqual; break;
				case ConditionCode.UnsignedLessOrEqual: context.ConditionCode = ConditionCode.UnsignedGreaterThan; break;
				case ConditionCode.UnsignedLessThan: context.ConditionCode = ConditionCode.UnsignedGreaterOrEqual; break;
			}
		}

		#endregion // Internals
	}
}
