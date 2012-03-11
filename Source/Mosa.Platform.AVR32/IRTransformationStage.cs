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
using Mosa.Compiler.Framework.Operands;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;
using IR = Mosa.Compiler.Framework.IR;

namespace Mosa.Platform.AVR32
{
	/// <summary>
	/// Transforms IR instructions into their appropriate AVR32.
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
		/// <param name="compiler">The compiler context to perform processing in.</param>
		void IMethodCompilerStage.Setup(IMethodCompiler compiler)
		{
			base.Setup(compiler);

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
		}

		/// <summary>
		/// Visitation function for AddUInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.AddUInstruction(Context context)
		{
            // FIXME: Float or Int64 not supported
            Operand result = context.Result;
            Operand operand = context.Operand1;

            if ((result is RegisterOperand) && (operand is ConstantOperand))
			{
				context.SetInstruction(Instruction.AddInstruction, result, operand);
			}
			else
                if ((result is MemoryOperand) && (operand is ConstantOperand))
				{

				}
				else
                    if ((result is RegisterOperand) && (operand is MemoryOperand))
					{

					}
					else
                        if ((result is RegisterOperand) && (operand is RegisterOperand))
						{

						}
						else
                            if ((result is MemoryOperand) && (operand is RegisterOperand))
							{

							}
                            else
                            if ((result is MemoryOperand) && (context.Operand1 is MemoryOperand))
                            {
                                RegisterOperand r8 = new RegisterOperand(BuiltInSigType.Int32, GeneralPurposeRegister.R8);
                                RegisterOperand r9 = new RegisterOperand(BuiltInSigType.Int32, GeneralPurposeRegister.R9);

                                context.SetInstruction(Instruction.LdInstruction, r8, result);
                                context.AppendInstruction(Instruction.LdInstruction, r9, operand);
                                context.AppendInstruction(Instruction.AddInstruction, r8, r9);
                                context.AppendInstruction(Instruction.StInstruction, result, r8);
                            }
		}

		/// <summary>
		/// Visitation function for AddFInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.AddFInstruction(Context context)
		{
		}

		/// <summary>
		/// Visitation function for DivFInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.DivFInstruction(Context context)
		{
		}

		/// <summary>
		/// Visitation function for DivSInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.DivSInstruction(Context context)
		{
		}

		/// <summary>
		/// Addresses the of instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.AddressOfInstruction(Context context)
		{
		}

		/// <summary>
		/// Arithmetic the shift right instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.ArithmeticShiftRightInstruction(Context context)
		{
		}

		/// <summary>
		/// Floating point compare instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.FloatingPointCompareInstruction(Context context)
		{
		}

		/// <summary>
		/// Visitation function for IntegerCompareBranchInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.IntegerCompareBranchInstruction(Context context)
		{
			IBranch branch = context.Branch;
			var condition = context.ConditionCode;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			context.SetInstruction(Instruction.CpInstruction, operand1, operand2);
			context.AppendInstruction(Instruction.BranchInstruction, condition);
			context.SetBranch(branch.Targets[0]);
		}

		/// <summary>
		/// Visitation function for IntegerCompareInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.IntegerCompareInstruction(Context context)
		{
			var condition = context.ConditionCode;
			var resultOperand = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			context.SetInstruction(Instruction.CpInstruction, operand1, operand2);

			if (resultOperand != null)
			{
				RegisterOperand r8 = new RegisterOperand(BuiltInSigType.Byte, GeneralPurposeRegister.R8);

				//if (IsUnsigned(resultOperand))
					//context.AppendInstruction(Instruction.SetccInstruction, GetUnsignedConditionCode(condition), r8);
				//else
				  //  context.AppendInstruction(Instruction.SetccInstruction, condition, r8);

				//context.AppendInstruction(Instruction.MovzxInstruction, resultOperand, r8);
			}
		}

		/// <summary>
		/// Visitation function for JmpInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.JmpInstruction(Context context)
		{
			context.ReplaceInstructionOnly(Instruction.RjmpInstruction);
		}

		/// <summary>
		/// Visitation function for LoadInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.LoadInstruction(Context context)
		{
			RegisterOperand r8 = new RegisterOperand(context.Operand1.Type, GeneralPurposeRegister.R8);
			Operand result = context.Result;
			Operand operand = context.Operand1;
			Operand offset = context.Operand2;
			ConstantOperand constantOffset = offset as ConstantOperand;
			IntPtr offsetPtr = IntPtr.Zero;

			context.SetInstruction(Instruction.LdInstruction, r8, operand);
			if (constantOffset != null)
			{
				offsetPtr = new IntPtr(Convert.ToInt64(constantOffset.Value));
			}
			else
			{
				context.AppendInstruction(Instruction.MovInstruction, r8, offset);
				context.AppendInstruction(Instruction.AddInstruction, r8, r8);
			}

			context.AppendInstruction(Instruction.LdInstruction, result, new MemoryOperand(r8.Type, GeneralPurposeRegister.R8, offsetPtr));
		}

		/// <summary>
		/// Visitation function for LogicalAndInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.LogicalAndInstruction(Context context)
		{
            // FIXME: Float or Int64 not supported
            Operand result = context.Result;
            Operand operand = context.Operand1;

            if ((result is RegisterOperand) && (operand is ConstantOperand))
            {
                context.SetInstruction(Instruction.AndInstruction, result, operand);
            }
            else
                if ((result is MemoryOperand) && (operand is ConstantOperand))
                {
                    RegisterOperand r8 = new RegisterOperand(BuiltInSigType.Int32, GeneralPurposeRegister.R8);
                    RegisterOperand r9 = new RegisterOperand(BuiltInSigType.Int32, GeneralPurposeRegister.R9);

                    context.SetInstruction(Instruction.LdInstruction, r8, result);
                    context.SetInstruction(Instruction.MovInstruction, r9, operand);
                    context.AppendInstruction(Instruction.AndInstruction, r8, r9);
                    context.AppendInstruction(Instruction.StInstruction, result, r8);
                }
                else
                    if ((result is RegisterOperand) && (operand is MemoryOperand))
                    {

                    }
                    else
                        if ((result is RegisterOperand) && (operand is RegisterOperand))
                        {

                        }
                        else
                            if ((result is MemoryOperand) && (operand is RegisterOperand))
                            {

                            }
                            else
                                if ((result is MemoryOperand) && (context.Operand1 is MemoryOperand))
                                {
                                    RegisterOperand r8 = new RegisterOperand(BuiltInSigType.Int32, GeneralPurposeRegister.R8);
                                    RegisterOperand r9 = new RegisterOperand(BuiltInSigType.Int32, GeneralPurposeRegister.R9);

                                    context.SetInstruction(Instruction.LdInstruction, r8, result);
                                    context.AppendInstruction(Instruction.LdInstruction, r9, operand);
                                    context.AppendInstruction(Instruction.AndInstruction, r8, r9);
                                    context.AppendInstruction(Instruction.StInstruction, result, r8);
                                }
		}

		/// <summary>
		/// Visitation function for LogicalOrInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.LogicalOrInstruction(Context context)
		{
		}

		/// <summary>
		/// Visitation function for LogicalXorInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.LogicalXorInstruction(Context context)
		{
		}

		/// <summary>
		/// Visitation function for LogicalNotInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.LogicalNotInstruction(Context context)
		{
		}

		/// <summary>
		/// Visitation function for MoveInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.MoveInstruction(Context context)
		{
			Operand result = context.Result;
			Operand operand = context.Operand1;
			//context.Operand1 = EmitConstant(context.Operand1);

			if (context.Result.StackType == StackTypeCode.F)
			{
				// TODO:
			}
			else
			{
				if (context.Result is MemoryOperand && context.Operand1 is MemoryOperand)
				{
					RegisterOperand load = new RegisterOperand(BuiltInSigType.IntPtr, GeneralPurposeRegister.R9);

					context.SetInstruction(Instruction.LdInstruction, load, operand);
					context.AppendInstruction(Instruction.StInstruction, result, load);

					//if (!Is32Bit(operand) && IsSigned(operand))
					//    context.SetInstruction(Instruction.MovsxInstruction, load, operand);
					//else if (!Is32Bit(operand) && IsUnsigned(operand))
					//    context.SetInstruction(Instruction.MovzxInstruction, load, operand);
					//else
					//    context.SetInstruction(Instruction.MovInstruction, load, operand);

					//context.AppendInstruction(Instruction.MovInstruction, result, store);
				}
				else
					if (context.Result is RegisterOperand && context.Operand1 is MemoryOperand)
					{
						context.ReplaceInstructionOnly(Instruction.LdInstruction);
					}
					else
						if (context.Result is MemoryOperand && context.Operand1 is RegisterOperand)
						{
							context.SetInstruction(Instruction.StInstruction, result, operand);
						}
						else
							if (context.Result is RegisterOperand && context.Operand1 is RegisterOperand)
							{
								context.ReplaceInstructionOnly(Instruction.MovInstruction);
							}
							else
								if (context.Result is MemoryOperand && context.Operand1 is ConstantOperand)
								{
									RegisterOperand load = new RegisterOperand(BuiltInSigType.IntPtr, GeneralPurposeRegister.R9);

									context.SetInstruction(Instruction.MovInstruction, load, operand);
									context.AppendInstruction(Instruction.StInstruction, result, load);
								}
								else
									if (context.Result is MemoryOperand && context.Operand1 is SymbolOperand)
									{
										//context.SetInstruction(Instruction.StInstruction, result, operand);
									}
									else
										if (context.Result is MemoryOperand && context.Operand1 is LabelOperand)
										{
											//context.SetInstruction(Instruction.StInstruction, result, operand);
										}

			}
		}

		/// <summary>
		/// Visitation function for PrologueInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.PrologueInstruction(Context context)
		{
			SigType I = BuiltInSigType.Int32;
			RegisterOperand r8 = new RegisterOperand(I, GeneralPurposeRegister.R8);
			RegisterOperand r12 = new RegisterOperand(I, GeneralPurposeRegister.R12);
			RegisterOperand r10 = new RegisterOperand(I, GeneralPurposeRegister.R10);
			RegisterOperand r11 = new RegisterOperand(I, GeneralPurposeRegister.R11);
			RegisterOperand sp = new RegisterOperand(I, GeneralPurposeRegister.SP);
			RegisterOperand r7 = new RegisterOperand(I, GeneralPurposeRegister.R7);
			RegisterOperand r6 = new RegisterOperand(I, GeneralPurposeRegister.R6);

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
				//context.SetInstruction(Instruction.BreakInstruction);
				context.AppendInstruction(Instruction.NopInstruction);

				// Uncomment this line to enable breakpoints within Bochs
				//context.AppendInstruction(CPUx86.Instruction.BochsDebug);
			}

			// push ebp
			context.SetInstruction(Instruction.PushInstruction, null, r11);
			// mov ebp, esp
			context.AppendInstruction(Instruction.MovInstruction, r11, sp);
			// sub esp, localsSize
			context.AppendInstruction(Instruction.SubInstruction, sp, new ConstantOperand(I, -stackSize));
			// push ebx
			context.AppendInstruction(Instruction.PushInstruction, null, r12);

			// Initialize all locals to zero
			context.AppendInstruction(Instruction.PushInstruction, null, r7);
			context.AppendInstruction(Instruction.MovInstruction, r7, sp);
			context.AppendInstruction(Instruction.PushInstruction, null, r10);

			//context.AppendInstruction(Instruction.AddInstruction, r7, new ConstantOperand(I, 8));
			context.AppendInstruction(Instruction.MovInstruction, r6, new ConstantOperand(I, 8));
			context.AppendInstruction(Instruction.AddInstruction, r7, r6);
			context.AppendInstruction(Instruction.MovInstruction, r10, new ConstantOperand(I, -(int)(stackSize >> 2)));
			context.AppendInstruction(Instruction.EorInstruction, r8, r8);
			// TODO:
			//context.AppendInstruction(Instruction.RepInstruction);
			//context.AppendInstruction(Instruction.StosdInstruction);
			context.AppendInstruction(Instruction.PopInstruction, r10);
			context.AppendInstruction(Instruction.PopInstruction, r7);

			// Save EDX for int32 return values (or do not save EDX for non-int64 return values)
			if (methodCompiler.Method.Signature.ReturnType.Type != CilElementType.I8 &&
				methodCompiler.Method.Signature.ReturnType.Type != CilElementType.U8)
			{
				// push edx
				context.AppendInstruction(Instruction.PushInstruction, null, new RegisterOperand(I, GeneralPurposeRegister.R9));
			}
		}

		/// <summary>
		/// Visitation function for EpilogueInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.EpilogueInstruction(Context context)
		{
			SigType I = BuiltInSigType.IntPtr;
			RegisterOperand r12 = new RegisterOperand(I, GeneralPurposeRegister.R12);
			RegisterOperand r9 = new RegisterOperand(I, GeneralPurposeRegister.R9);
			RegisterOperand r11 = new RegisterOperand(I, GeneralPurposeRegister.R11);
			RegisterOperand sp = new RegisterOperand(I, GeneralPurposeRegister.SP);
			RegisterOperand r7 = new RegisterOperand(I, GeneralPurposeRegister.R7);

			// Load EDX for int32 return values
			if (methodCompiler.Method.Signature.ReturnType.Type != CilElementType.I8 &&
				methodCompiler.Method.Signature.ReturnType.Type != CilElementType.U8)
			{
				// pop edx
				context.SetInstruction(Instruction.PopInstruction, r9);
				context.AppendInstruction(Instruction.NopInstruction);
			}

			// pop ebx
			context.SetInstruction(Instruction.PopInstruction, r12);
			// add esp, -localsSize
			context.AppendInstruction(Instruction.MovInstruction, r7, new ConstantOperand(I, -stackSize));
			context.AppendInstruction(Instruction.AddInstruction, sp, r7);
			// pop ebp
			context.AppendInstruction(Instruction.PopInstruction, r11);
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
				context.AppendInstruction(Instruction.RjmpInstruction);
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
		}

		/// <summary>
		/// Visitation function for ShiftRightInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.ShiftRightInstruction(Context context)
		{
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

			RegisterOperand r8 = new RegisterOperand(destination.Type, GeneralPurposeRegister.R8);
			RegisterOperand r9 = new RegisterOperand(value.Type, GeneralPurposeRegister.R9);

			context.SetInstruction(Instruction.LdInstruction, r8, destination);
			context.AppendInstruction(Instruction.LdInstruction, r9, value);

			IntPtr offsetPtr = IntPtr.Zero;
			if (constantOffset != null)
			{
				offsetPtr = new IntPtr(Convert.ToInt64(constantOffset.Value));
			}
			else
			{
				context.AppendInstruction(Instruction.MovInstruction, r8, offset);
				context.AppendInstruction(Instruction.AddInstruction, r8, r8);
			}

			context.AppendInstruction(Instruction.LdInstruction, new MemoryOperand(value.Type, GeneralPurposeRegister.R8, offsetPtr), r9);
		}

		/// <summary>
		/// Visitation function for DivUInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.DivUInstruction(Context context)
		{
		}

		/// <summary>
		/// Visitation function for MulSInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.MulSInstruction(Context context)
		{
		}

		/// <summary>
		/// Visitation function for MulFInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.MulFInstruction(Context context)
		{
		}

		/// <summary>
		/// Visitation function for MulUInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.MulUInstruction(Context context)
		{
		}

		/// <summary>
		/// Visitation function for SubFInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.SubFInstruction(Context context)
		{
		}

		/// <summary>
		/// Visitation function for SubSInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.SubSInstruction(Context context)
		{
		}

		/// <summary>
		/// Visitation function for SubUInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.SubUInstruction(Context context)
		{
		}

		/// <summary>
		/// Visitation function for RemFInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.RemFInstruction(Context context)
		{
		}

		/// <summary>
		/// Visitation function for RemSInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.RemSInstruction(Context context)
		{
		}

		/// <summary>
		/// Visitation function for RemUInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.RemUInstruction(Context context)
		{
		}

		/// <summary>
		/// Visitation function for SwitchInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.SwitchInstruction(Context context)
		{
		}

		/// <summary>
		/// Visitation function for BreakInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.BreakInstruction(Context context)
		{
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
				var r8 = new RegisterOperand(BuiltInSigType.Int32, GeneralPurposeRegister.R8);
				var destination = context.Result;
				var source = context.Operand1 as MemoryOperand;
				var elementType = type == null ? GetElementType(source.Type) : GetElementType(type);
				var constantOffset = offset as ConstantOperand;
				var offsetPtr = IntPtr.Zero;

				context.SetInstruction(Instruction.LdInstruction, r8, source);
				if (constantOffset != null)
				{
					offsetPtr = new IntPtr(Convert.ToInt64(constantOffset.Value));
				}
				else
				{
					context.AppendInstruction(Instruction.MovInstruction, r8, offset);
					context.AppendInstruction(Instruction.AddInstruction, r8, r8);
				}

				context.AppendInstruction(Instruction.LdsInstruction, destination, new MemoryOperand(elementType, GeneralPurposeRegister.R8, offsetPtr));
			}
			else
			{
				context.ReplaceInstructionOnly(Instruction.LdsInstruction);
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
				context.ReplaceInstructionOnly(Instruction.RcallInstruction);
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
				RegisterOperand r8 = new RegisterOperand(context.Operand1.Type, GeneralPurposeRegister.R8);
				Operand result = context.Result;
				Operand source = context.Operand1;
				SigType elementType = GetElementType(source.Type);
				ConstantOperand constantOffset = offset as ConstantOperand;
				IntPtr offsetPtr = IntPtr.Zero;

				context.SetInstruction(Instruction.MovInstruction, r8, source);
				if (constantOffset != null)
				{
					offsetPtr = new IntPtr(Convert.ToInt64(constantOffset.Value));
				}

				if (elementType.Type == CilElementType.Char ||
					elementType.Type == CilElementType.U1 ||
					elementType.Type == CilElementType.U2)
				{
					context.AppendInstruction(Instruction.LdInstruction, r8, offset);
					context.AppendInstruction(Instruction.AddInstruction, r8, r8);
				}
				//context.AppendInstruction(Instruction.MovzxInstruction, result, new MemoryOperand(elementType, GeneralPurposeRegister.R8, offsetPtr));
			}
			else
			{
				//context.ReplaceInstructionOnly(Instruction.MovzxInstruction);
			}
		}

		/// <summary>
		/// Visitation function for FloatingPointToIntegerConversionInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.FloatingPointToIntegerConversionInstruction(Context context)
		{
		}

		/// <summary>
		/// Visitation function for PopInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.PopInstruction(Context context)
		{
		}

		/// <summary>
		/// Visitation function for PushInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.PushInstruction(Context context)
		{
		}

		/// <summary>
		/// Visitation function for ThrowInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.ThrowInstruction(Context context)
		{
		}

		/// <summary>
		/// Visitation function for ExceptionPrologueInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.ExceptionPrologueInstruction(Context context)
		{
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

	}
}