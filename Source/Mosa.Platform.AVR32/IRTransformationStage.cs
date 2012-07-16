/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Pascal Delprat (pdelprat) <pascal.delprat@online.fr> 
 */

using System;
using System.Diagnostics;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Framework.Platform;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Platform.AVR32
{
	/// <summary>
	/// Transforms IR instructions into their appropriate AVR32.
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
		}

		/// <summary>
		/// Visitation function for AddUInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.AddUnsigned(Context context)
		{
			// FIXME: Float or Int64 not supported
			Operand result = context.Result;
			Operand operand = context.Operand1;

			if ((result.IsRegister) && (operand.IsConstant))
			{
				context.SetInstruction(AVR32.Add, result, operand);
			}
			else
				if ((result.IsMemoryAddress) && (operand.IsConstant))
				{

				}
				else
					if ((result.IsRegister) && (operand.IsMemoryAddress))
					{

					}
					else
						if ((result.IsRegister) && (operand.IsRegister))
						{

						}
						else
							if ((result.IsMemoryAddress) && (operand.IsRegister))
							{

							}
							else
								if ((result.IsMemoryAddress) && (context.Operand1.IsMemoryAddress))
								{
									Operand r8 = Operand.CreateCPURegister(BuiltInSigType.Int32, GeneralPurposeRegister.R8);
									Operand r9 = Operand.CreateCPURegister(BuiltInSigType.Int32, GeneralPurposeRegister.R9);

									context.SetInstruction(AVR32.Ld, r8, result);
									context.AppendInstruction(AVR32.Ld, r9, operand);
									context.AppendInstruction(AVR32.Add, r8, r9);
									context.AppendInstruction(AVR32.St, result, r8);
								}
		}

		/// <summary>
		/// Visitation function for AddFInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.AddFloat(Context context)
		{
		}

		/// <summary>
		/// Visitation function for DivFInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.DivFloat(Context context)
		{
		}

		/// <summary>
		/// Visitation function for DivSInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.DivSigned(Context context)
		{
		}

		/// <summary>
		/// Addresses the of instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.AddressOf(Context context)
		{
		}

		/// <summary>
		/// Arithmetic the shift right instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.ArithmeticShiftRight(Context context)
		{
		}

		/// <summary>
		/// Floating point compare instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.FloatCompare(Context context)
		{
		}

		/// <summary>
		/// Visitation function for IntegerCompareBranchInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.IntegerCompareBranch(Context context)
		{
			int target = context.BranchTargets[0];
			var condition = context.ConditionCode;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			context.SetInstruction(AVR32.Cp, operand1, operand2);
			context.AppendInstruction(AVR32.Branch, condition);
			context.SetBranch(target);
		}

		/// <summary>
		/// Visitation function for IntegerCompareInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.IntegerCompare(Context context)
		{
			var condition = context.ConditionCode;
			var resultOperand = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			context.SetInstruction(AVR32.Cp, operand1, operand2);

			if (resultOperand != null)
			{
				Operand r8 = Operand.CreateCPURegister(BuiltInSigType.Byte, GeneralPurposeRegister.R8);

				//if (IsUnsigned(resultOperand))
				//context.AppendInstruction(Instruction.Setcc, GetUnsignedConditionCode(condition), r8);
				//else
				//  context.AppendInstruction(Instruction.Setcc, condition, r8);

				//context.AppendInstruction(Instruction.Movzx, resultOperand, r8);
			}
		}

		/// <summary>
		/// Visitation function for JmpInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.Jmp(Context context)
		{
			context.ReplaceInstructionOnly(AVR32.Rjmp);
		}

		/// <summary>
		/// Visitation function for LoadInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.Load(Context context)
		{
			Operand r8 = Operand.CreateCPURegister(context.Operand1.Type, GeneralPurposeRegister.R8);
			Operand result = context.Result;
			Operand operand = context.Operand1;
			Operand offset = context.Operand2;
			IntPtr offsetPtr = IntPtr.Zero;

			context.SetInstruction(AVR32.Ld, r8, operand);
			if (offset.IsConstant)
			{
				offsetPtr = new IntPtr(Convert.ToInt64(offset.Value));
			}
			else
			{
				context.AppendInstruction(AVR32.Mov, r8, offset);
				context.AppendInstruction(AVR32.Add, r8, r8);
			}

			context.AppendInstruction(AVR32.Ld, result, Operand.CreateMemoryAddress(r8.Type, GeneralPurposeRegister.R8, offsetPtr));
		}

		/// <summary>
		/// Visitation function for Load Zero Extended.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.LoadZeroExtended(Context context)
		{
			// TODO
		}

		/// <summary>
		/// Visitation function for Load Sign Extended.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.LoadSignExtended(Context context)
		{
			// TODO
		}

		/// <summary>
		/// Visitation function for LogicalAndInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.LogicalAnd(Context context)
		{
			// FIXME: Float or Int64 not supported
			Operand result = context.Result;
			Operand operand = context.Operand1;

			if ((result.IsRegister) && (operand.IsConstant))
			{
				context.SetInstruction(AVR32.And, result, operand);
			}
			else
				if ((result.IsMemoryAddress) && (operand.IsConstant))
				{
					Operand r8 = Operand.CreateCPURegister(BuiltInSigType.Int32, GeneralPurposeRegister.R8);
					Operand r9 = Operand.CreateCPURegister(BuiltInSigType.Int32, GeneralPurposeRegister.R9);

					context.SetInstruction(AVR32.Ld, r8, result);
					context.SetInstruction(AVR32.Mov, r9, operand);
					context.AppendInstruction(AVR32.And, r8, r9);
					context.AppendInstruction(AVR32.St, result, r8);
				}
				else
					if ((result.IsRegister) && (operand.IsMemoryAddress))
					{

					}
					else
						if ((result.IsRegister) && (operand.IsRegister))
						{

						}
						else
							if ((result.IsMemoryAddress) && (operand.IsRegister))
							{

							}
							else
								if ((result.IsMemoryAddress) && (context.Operand1.IsMemoryAddress))
								{
									Operand r8 = Operand.CreateCPURegister(BuiltInSigType.Int32, GeneralPurposeRegister.R8);
									Operand r9 = Operand.CreateCPURegister(BuiltInSigType.Int32, GeneralPurposeRegister.R9);

									context.SetInstruction(AVR32.Ld, r8, result);
									context.AppendInstruction(AVR32.Ld, r9, operand);
									context.AppendInstruction(AVR32.And, r8, r9);
									context.AppendInstruction(AVR32.St, result, r8);
								}
		}

		/// <summary>
		/// Visitation function for LogicalOrInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.LogicalOr(Context context)
		{
		}

		/// <summary>
		/// Visitation function for LogicalXorInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.LogicalXor(Context context)
		{
		}

		/// <summary>
		/// Visitation function for LogicalNotInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.LogicalNot(Context context)
		{
		}

		/// <summary>
		/// Visitation function for MoveInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.Move(Context context)
		{
			Operand result = context.Result;
			Operand operand = context.Operand1;
			//context.Operand1 = EmitConstant(context.Operand1);

			if (context.Result.StackType == StackTypeCode.F)
			{
				// TODO:
			}
			else if (context.Result.IsMemoryAddress && context.Operand1.IsMemoryAddress)
			{
				Operand load = Operand.CreateCPURegister(BuiltInSigType.IntPtr, GeneralPurposeRegister.R9);

				context.SetInstruction(AVR32.Ld, load, operand);
				context.AppendInstruction(AVR32.St, result, load);

				//if (!Is32Bit(operand) && IsSigned(operand))
				//    context.SetInstruction(Instruction.Movsx, load, operand);
				//else if (!Is32Bit(operand) && IsUnsigned(operand))
				//    context.SetInstruction(Instruction.Movzx, load, operand);
				//else
				//    context.SetInstruction(Instruction.Mov, load, operand);

				//context.AppendInstruction(Instruction.Mov, result, store);
			}
			else if (context.Result.IsRegister && context.Operand1.IsMemoryAddress)
			{
				context.ReplaceInstructionOnly(AVR32.Ld);
			}
			else if (context.Result.IsMemoryAddress && context.Operand1.IsRegister)
			{
				context.SetInstruction(AVR32.St, result, operand);
			}
			else if (context.Result.IsRegister && context.Operand1.IsRegister)
			{
				context.ReplaceInstructionOnly(AVR32.Mov);
			}
			else if (context.Result.IsMemoryAddress && context.Operand1.IsConstant)
			{
				Operand load = Operand.CreateCPURegister(BuiltInSigType.IntPtr, GeneralPurposeRegister.R9);

				context.SetInstruction(AVR32.Mov, load, operand);
				context.AppendInstruction(AVR32.St, result, load);
			}
			else if (context.Result.IsMemoryAddress && context.Operand1.IsSymbol)
			{
				//context.SetInstruction(Instruction.St, result, operand);
			}


		}

		/// <summary>
		/// Visitation function for PrologueInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.Prologue(Context context)
		{
			SigType I = BuiltInSigType.Int32;
			Operand r8 = Operand.CreateCPURegister(I, GeneralPurposeRegister.R8);
			Operand r12 = Operand.CreateCPURegister(I, GeneralPurposeRegister.R12);
			Operand r10 = Operand.CreateCPURegister(I, GeneralPurposeRegister.R10);
			Operand r11 = Operand.CreateCPURegister(I, GeneralPurposeRegister.R11);
			Operand sp = Operand.CreateCPURegister(I, GeneralPurposeRegister.SP);
			Operand r7 = Operand.CreateCPURegister(I, GeneralPurposeRegister.R7);
			Operand r6 = Operand.CreateCPURegister(I, GeneralPurposeRegister.R6);

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
				// TODO:
				//context.SetInstruction(Instruction.Break);
				context.AppendInstruction(AVR32.Nop);

				// Uncomment this line to enable breakpoints within Bochs
				//context.AppendInstruction(CPUx86.Instruction.BochsDebug);
			}

			// push ebp
			context.SetInstruction(AVR32.Push, null, r11);
			// mov ebp, esp
			context.AppendInstruction(AVR32.Mov, r11, sp);
			// sub esp, localsSize
			context.AppendInstruction(AVR32.Sub, sp, Operand.CreateConstant(I, -stackSize));
			// push ebx
			context.AppendInstruction(AVR32.Push, null, r12);

			// Initialize all locals to zero
			context.AppendInstruction(AVR32.Push, null, r7);
			context.AppendInstruction(AVR32.Mov, r7, sp);
			context.AppendInstruction(AVR32.Push, null, r10);

			//context.AppendInstruction(Instruction.Add, r7, Operand.CreateConstant(I, 8));
			context.AppendInstruction(AVR32.Mov, r6, Operand.CreateConstant(I, 8));
			context.AppendInstruction(AVR32.Add, r7, r6);
			context.AppendInstruction(AVR32.Mov, r10, Operand.CreateConstant(I, -(int)(stackSize >> 2)));
			context.AppendInstruction(AVR32.Eor, r8, r8);
			// TODO:
			//context.AppendInstruction(Instruction.Rep);
			//context.AppendInstruction(Instruction.Stosd);
			context.AppendInstruction(AVR32.Pop, r10);
			context.AppendInstruction(AVR32.Pop, r7);

			// Save EDX for int32 return values (or do not save EDX for non-int64 return values)
			if (methodCompiler.Method.Signature.ReturnType.Type != CilElementType.I8 &&
				methodCompiler.Method.Signature.ReturnType.Type != CilElementType.U8)
			{
				// push edx
				context.AppendInstruction(AVR32.Push, null, Operand.CreateCPURegister(I, GeneralPurposeRegister.R9));
			}
		}

		/// <summary>
		/// Visitation function for EpilogueInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.Epilogue(Context context)
		{
			SigType I = BuiltInSigType.IntPtr;
			Operand r12 = Operand.CreateCPURegister(I, GeneralPurposeRegister.R12);
			Operand r9 = Operand.CreateCPURegister(I, GeneralPurposeRegister.R9);
			Operand r11 = Operand.CreateCPURegister(I, GeneralPurposeRegister.R11);
			Operand sp = Operand.CreateCPURegister(I, GeneralPurposeRegister.SP);
			Operand r7 = Operand.CreateCPURegister(I, GeneralPurposeRegister.R7);

			// Load EDX for int32 return values
			if (methodCompiler.Method.Signature.ReturnType.Type != CilElementType.I8 &&
				methodCompiler.Method.Signature.ReturnType.Type != CilElementType.U8)
			{
				// pop edx
				context.SetInstruction(AVR32.Pop, r9);
				context.AppendInstruction(AVR32.Nop);
			}

			// pop ebx
			context.SetInstruction(AVR32.Pop, r12);
			// add esp, -localsSize
			context.AppendInstruction(AVR32.Mov, r7, Operand.CreateConstant(I, -stackSize));
			context.AppendInstruction(AVR32.Add, sp, r7);
			// pop ebp
			context.AppendInstruction(AVR32.Pop, r11);
			// ret
			context.AppendInstruction(AVR32.Ret);
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
				context.SetInstruction(AVR32.Ret);
				return;
			}

			if (context.Operand1 != null)
			{
				callingConvention.MoveReturnValue(context, context.Operand1);
				context.AppendInstruction(AVR32.Rjmp);
				context.SetBranch(Int32.MaxValue);
			}
			else
			{
				context.SetInstruction(AVR32.Jmp);
				context.SetBranch(Int32.MaxValue);
			}
		}

		/// <summary>
		/// Visitation function for ShiftLeftInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.ShiftLeft(Context context)
		{
		}

		/// <summary>
		/// Visitation function for ShiftRightInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.ShiftRight(Context context)
		{
		}

		/// <summary>
		/// Visitation function for StoreInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.Store(Context context)
		{
			Operand destination = context.Result;
			Operand offset = context.Operand2;
			Operand value = context.Operand3;

			Operand r8 = Operand.CreateCPURegister(destination.Type, GeneralPurposeRegister.R8);
			Operand r9 = Operand.CreateCPURegister(value.Type, GeneralPurposeRegister.R9);

			context.SetInstruction(AVR32.Ld, r8, destination);
			context.AppendInstruction(AVR32.Ld, r9, value);

			IntPtr offsetPtr = IntPtr.Zero;
			if (offset.IsConstant)
			{
				offsetPtr = new IntPtr(Convert.ToInt64(offset.Value));
			}
			else
			{
				context.AppendInstruction(AVR32.Mov, r8, offset);
				context.AppendInstruction(AVR32.Add, r8, r8);
			}

			context.AppendInstruction(AVR32.Ld, Operand.CreateMemoryAddress(value.Type, GeneralPurposeRegister.R8, offsetPtr), r9);
		}

		/// <summary>
		/// Visitation function for DivUInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.DivUnsigned(Context context)
		{
		}

		/// <summary>
		/// Visitation function for MulSInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.MulSigned(Context context)
		{
		}

		/// <summary>
		/// Visitation function for MulFInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.MulFloat(Context context)
		{
		}

		/// <summary>
		/// Visitation function for MulUInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.MulUnsigned(Context context)
		{
		}

		/// <summary>
		/// Visitation function for SubFInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.SubFloat(Context context)
		{
		}

		/// <summary>
		/// Visitation function for SubSInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.SubSigned(Context context)
		{
		}

		/// <summary>
		/// Visitation function for SubUInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.SubUnsigned(Context context)
		{
		}

		/// <summary>
		/// Visitation function for RemFInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.RemFloat(Context context)
		{
		}

		/// <summary>
		/// Visitation function for RemSInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.RemSigned(Context context)
		{
		}

		/// <summary>
		/// Visitation function for RemUInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.RemUnsigned(Context context)
		{
		}

		/// <summary>
		/// Visitation function for SwitchInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.Switch(Context context)
		{
		}

		/// <summary>
		/// Visitation function for BreakInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.Break(Context context)
		{
		}

		/// <summary>
		/// Visitation function for NopInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.Nop(Context context)
		{
			context.SetInstruction(AVR32.Nop);
		}

		/// <summary>
		/// Visitation function for SignExtendedMoveInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.SignExtendedMove(Context context)
		{
			context.ReplaceInstructionOnly(AVR32.Lds);
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
		/// Visitation function for CallInstruction instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.Call(Context context)
		{
			if (context.OperandCount == 0 && context.BranchTargets != null)
			{
				// inter-method call; usually for exception processing
				context.ReplaceInstructionOnly(AVR32.Rcall);
			}
			else
			{
				callingConvention.MakeCall(context);
			}
		}

		/// <summary>
		/// Visitation function for intrinsic the method call.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.IntrinsicMethodCall(Context context)
		{
			// TODO
		}

		/// <summary>
		/// Visitation function for ZeroExtendedMoveInstruction instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.ZeroExtendedMove(Context context)
		{
			//context.ReplaceInstructionOnly(AVR32.Mov);
		}

		/// <summary>
		/// Visitation function for FloatingPointToIntegerConversionInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.FloatToIntegerConversion(Context context)
		{
		}

		/// <summary>
		/// Visitation function for ThrowInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.Throw(Context context)
		{
		}

		/// <summary>
		/// Visitation function for ExceptionPrologueInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.ExceptionPrologue(Context context)
		{
		}

		#endregion //  IIRVisitor

		#region IIRVisitor - Unused

		/// <summary>
		/// Visitation function for IntegerToFloatingPointConversion.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.IntegerToFloatConversion(Context context) { }

		/// <summary>
		/// Visitation function for Phi.
		/// </summary>
		/// <param name="context">The context.</param>
		void IIRVisitor.Phi(Context context) { }

		#endregion // IIRVisitor - Unused

	}
}