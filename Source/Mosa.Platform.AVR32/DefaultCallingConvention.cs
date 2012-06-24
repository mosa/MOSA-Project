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
using System.Collections.Generic;
using System.Diagnostics;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Platform.AVR32
{
	/// <summary>
	/// Implements the CIL default calling convention for AVR32.
	/// </summary>
	sealed class DefaultCallingConvention : ICallingConvention
	{
		#region Data members

		/// <summary>
		/// Holds the architecture of the calling convention.
		/// </summary>
		private IArchitecture architecture;

		private static readonly Register[] ReturnVoidRegisters = new Register[] { };
		private static readonly Register[] Return32BitRegisters = new Register[] { GeneralPurposeRegister.R8 };
		private static readonly Register[] Return64BitRegisters = new Register[] { GeneralPurposeRegister.R8, GeneralPurposeRegister.R9 };
		private static readonly Register[] ReturnFPRegisters = new Register[] { /* TODO */ };
		private static readonly Register[] CalleeSavedRegisters = new Register[] { /* TODO */ };

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="DefaultCallingConvention"/>.
		/// </summary>
		/// <param name="architecture">The architecture of the calling convention.</param>
		/// <param name="typeLayout">The type layout.</param>
		public DefaultCallingConvention(IArchitecture architecture)
		{
			if (architecture == null)
				throw new ArgumentNullException(@"architecture");

			this.architecture = architecture;
		}

		#endregion // Construction

		#region ICallingConvention Members

		/// <summary>
		/// Expands the given invoke instruction to perform the method call.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <returns>
		/// A single instruction or an array of instructions, which appropriately represent the method call.
		/// </returns>
		void ICallingConvention.MakeCall(Context ctx)
		{
			/*
			 * Calling convention is right-to-left, pushed on the stack. Return value in R9 for integral
			 * types 4 bytes or less. R8:R9 for 64-bit. If this is a method
			 * of a type, the this argument is moved to R10 right before the call.
			 */

			Operand invokeTarget = ctx.Operand1;
			Operand result = ctx.Result;

			Stack<Operand> operands = BuildOperandStack(ctx);

			int stackSize = CalculateStackSizeForParameters(operands);

			ctx.SetInstruction(AVR32.Nop);

			ReserveStackSizeForCall(ctx, stackSize);

			if (stackSize != 0)
			{
				PushOperands(ctx, operands, stackSize);
			}

			ctx.AppendInstruction(AVR32.Call, null, invokeTarget);

			if (stackSize != 0)
			{
				FreeStackAfterCall(ctx, stackSize);
			}

			CleanupReturnValue(ctx, result);
		}

		private Stack<Operand> BuildOperandStack(Context ctx)
		{
			Stack<Operand> operandStack = new Stack<Operand>(ctx.OperandCount);
			int index = 0;

			foreach (Operand operand in ctx.Operands)
			{
				if (index++ > 0)
				{
					operandStack.Push(operand);
				}
			}

			return operandStack;
		}

		private void ReserveStackSizeForCall(Context ctx, int stackSize)
		{
			if (stackSize != 0)
			{
				Operand sp = Operand.CreateCPURegister(BuiltInSigType.IntPtr, GeneralPurposeRegister.SP);

				ctx.AppendInstruction(AVR32.Sub, sp, Operand.CreateConstant(sp.Type, stackSize));
				ctx.AppendInstruction(AVR32.Mov, Operand.CreateCPURegister(architecture.NativeType, GeneralPurposeRegister.R9), sp);
			}
		}

		private void FreeStackAfterCall(Context ctx, int stackSize)
		{
			Operand sp = Operand.CreateCPURegister(BuiltInSigType.IntPtr, GeneralPurposeRegister.SP);
			Operand r7 = Operand.CreateCPURegister(BuiltInSigType.IntPtr, GeneralPurposeRegister.R7);
			if (stackSize != 0)
			{
				ctx.AppendInstruction(AVR32.Mov, r7, Operand.CreateConstant(BuiltInSigType.IntPtr, stackSize));
				ctx.AppendInstruction(AVR32.Add, sp, r7);
			}
		}

		private void CleanupReturnValue(Context ctx, Operand result)
		{
			if (result != null)
			{
				if (result.StackType == StackTypeCode.Int64)
					MoveReturnValueTo64Bit(result, ctx);
				else
					MoveReturnValueTo32Bit(result, ctx);
			}
		}


		/// <summary>
		/// Calculates the remaining space.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="operandStack">The operand stack.</param>
		/// <param name="space">The space.</param>
		private void PushOperands(Context ctx, Stack<Operand> operandStack, int space)
		{
			while (operandStack.Count != 0)
			{
				Operand operand = operandStack.Pop();

				int size, alignment;
				architecture.GetTypeRequirements(operand.Type, out size, out alignment);

				space -= size;
				Push(ctx, operand, space, size);
			}
		}

		/// <summary>
		/// Moves the return value to 32 bit.
		/// </summary>
		/// <param name="resultOperand">The result operand.</param>
		/// <param name="ctx">The context.</param>
		private void MoveReturnValueTo32Bit(Operand resultOperand, Context ctx)
		{
			Operand r8 = Operand.CreateCPURegister(resultOperand.Type, GeneralPurposeRegister.R8);
			ctx.AppendInstruction(AVR32.Mov, resultOperand, r8);
		}

		/// <summary>
		/// Moves the return value to 64 bit.
		/// </summary>
		/// <param name="resultOperand">The result operand.</param>
		/// <param name="ctx">The context.</param>
		private void MoveReturnValueTo64Bit(Operand resultOperand, Context ctx)
		{
			//Operand opL, opH;
			//LongOperandTransformationStage.SplitLongOperand(resultOperand, out opL, out opH);

			//RegisterOperand r8 = new RegisterOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.R8);
			//RegisterOperand r9 = new RegisterOperand(BuiltInSigType.Int32, GeneralPurposeRegister.R9);

			//ctx.AppendInstruction(Instruction.Mov, opL, r8);
			//ctx.AppendInstruction(Instruction.Mov, opH, r9);
		}

		/// <summary>
		/// Pushes the specified instructions.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="op">The op.</param>
		/// <param name="stackSize">Size of the stack.</param>
		/// <param name="parameterSize">Size of the parameter.</param>
		private void Push(Context ctx, Operand op, int stackSize, int parameterSize)
		{
			if (op.IsMemoryAddress)
			{
				if (op.Type.Type == CilElementType.ValueType)
				{
					for (int i = 0; i < parameterSize; i += 4)
						ctx.AppendInstruction(AVR32.Mov, Operand.CreateMemoryAddress(op.Type, GeneralPurposeRegister.R9, new IntPtr(stackSize + i)), Operand.CreateMemoryAddress(op.Type, op.Base, new IntPtr(op.Offset.ToInt64() + i)));

					return;
				}

				Operand rop;
				switch (op.StackType)
				{
					case StackTypeCode.O: goto case StackTypeCode.N;
					case StackTypeCode.Ptr: goto case StackTypeCode.N;
					case StackTypeCode.Int32: goto case StackTypeCode.N;
					case StackTypeCode.N:
						rop = Operand.CreateCPURegister(op.Type, GeneralPurposeRegister.R8);
						break;

					case StackTypeCode.F:
						// TODO:
						//rop = new RegisterOperand(op.Type, SSE2Register.XMM0);
						rop = null;
						break;

					case StackTypeCode.Int64:
						{
							Debug.Assert(op.IsMemoryAddress, @"I8/U8 arg is not in a memory operand.");
							//Operand r8 = Operand.CreateCPURegister(BuiltInSigType.Int32, GeneralPurposeRegister.R8);

							//Operand opL, opH;
							//LongOperandTransformationStage.SplitLongOperand(op, out opL, out opH);

							//ctx.AppendInstruction(Instruction.Mov, r8, opL);
							//ctx.AppendInstruction(Instruction.Mov, Operand.CreateMemoryAddress(op.Type, GeneralPurposeRegister.R9, new IntPtr(stackSize)), r8);
							//ctx.AppendInstruction(Instruction.Mov, r8, opH);
							//ctx.AppendInstruction(Instruction.Mov, Operand.CreateMemoryAddress(op.Type, GeneralPurposeRegister.R9, new IntPtr(stackSize + 4)), r8);
						}
						return;

					default:
						throw new NotSupportedException();
				}

				ctx.AppendInstruction(AVR32.Mov, rop, op);
				op = rop;
			}
			else if (op.IsConstant && op.StackType == StackTypeCode.Int64)
			{
				//Operand opL, opH;
				//RegisterOperand r8 = new RegisterOperand(BuiltInSigType.Int32, GeneralPurposeRegister.R8);
				//LongOperandTransformationStage.SplitLongOperand(op, out opL, out opH);

				//ctx.AppendInstruction(Instruction.Mov, r8, opL);
				//ctx.AppendInstruction(Instruction.Mov, Operand.CreateMemoryAddress(BuiltInSigType.Int32, GeneralPurposeRegister.R9, new IntPtr(stackSize)), r8);
				//ctx.AppendInstruction(Instruction.Mov, r8, opH);
				//ctx.AppendInstruction(Instruction.Mov, Operand.CreateMemoryAddress(BuiltInSigType.Int32, GeneralPurposeRegister.R9, new IntPtr(stackSize + 4)), r8);

				return;
			}

			ctx.AppendInstruction(AVR32.Mov, Operand.CreateMemoryAddress(op.Type, GeneralPurposeRegister.R9, new IntPtr(stackSize)), op);
		}

		/// <summary>
		/// Calculates the stack size for parameters.
		/// </summary>
		/// <param name="operands">The operands.</param>
		/// <returns></returns>
		private int CalculateStackSizeForParameters(IEnumerable<Operand> operands)
		{
			int result = 0;

			foreach (Operand op in operands)
			{
				int size, alignment;
				architecture.GetTypeRequirements(op.Type, out size, out alignment);

				result += size;
			}
			return result;
		}

		/// <summary>
		/// Requests the calling convention to create an appropriate move instruction to populate the return
		/// value of a method.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="operand">The operand, that's holding the return value.</param>
		void ICallingConvention.MoveReturnValue(Context ctx, Operand operand)
		{
			int size, alignment;
			architecture.GetTypeRequirements(operand.Type, out size, out alignment);

			// FIXME: Do not issue a move, if the operand is already the destination register
			if (size == 4 || size == 2 || size == 1)
			{
				ctx.SetInstruction(AVR32.Mov, Operand.CreateCPURegister(operand.Type, GeneralPurposeRegister.R8), operand);
				return;
			}
			else if (size == 8 && (operand.Type.Type == CilElementType.R4 || operand.Type.Type == CilElementType.R8))
			{

				if (!(operand.IsMemoryAddress))
				{
					// Move the operand to memory by prepending an instruction
				}

				// BUG: Return values are in FP0, not XMM#0
				// TODO:
				//ctx.SetInstruction(Instruction.Mov, new RegisterOperand(operand.Type, SSE2Register.XMM0), operand);
				return;
			}
			else if (size == 8 && (operand.Type.Type == CilElementType.I8 || operand.Type.Type == CilElementType.U8))
			{
				SigType HighType = (operand.Type.Type == CilElementType.I8) ? BuiltInSigType.Int32 : BuiltInSigType.UInt32;

				//Operand opL, opH;
				//LongOperandTransformationStage.SplitLongOperand(operand, out opL, out opH);

				//// Like Win32: EDX:EAX
				//ctx.SetInstruction(Instruction.Mov, new RegisterOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.R8), opL);
				//ctx.AppendInstruction(Instruction.Mov, new RegisterOperand(HighType, GeneralPurposeRegister.R9), opH);

				return;
			}
			else
			{
				throw new NotSupportedException();
			}
		}

		void ICallingConvention.GetStackRequirements(Operand stackOperand, out int size, out int alignment)
		{
			// Special treatment for some stack types
			// FIXME: Handle the size and alignment requirements of value types
			architecture.GetTypeRequirements(stackOperand.Type, out size, out alignment);
		}

		int ICallingConvention.OffsetOfFirstLocal
		{
			get
			{
				/*
				 * The first local variable is offset by 8 bytes from the start of
				 * the stack frame. [EBP-08h] (The first stack slot available for
				 * locals is [EBP], so we're reserving two 32-bit ints for
				 * system/compiler use as described below.
				 * 
				 * The first 4 bytes are used to hold the start of the method,
				 * so that we can embed floating point constants in our PIC.
				 * 
				 */
				return -4;
			}
		}

		int ICallingConvention.OffsetOfFirstParameter
		{
			get
			{
				/*
				 * The first parameter is offset by 8 bytes from the start of
				 * the stack frame. [EBP+08h].
				 * 
				 * - [EBP+04h] holds the EDX register, which was pushed by the prologue instruction.
				 * - [EBP+08h] holds the return address, which was pushed by the call instruction.
				 * 
				 */
				return 8;
			}
		}

		/// <summary>
		/// Gets the callee saved registers.
		/// </summary>
		Register[] ICallingConvention.CalleeSavedRegisters
		{
			get { return CalleeSavedRegisters; }
		}

		/// <summary>
		/// Gets the return registers.
		/// </summary>
		/// <param name="returnType">Type of the return.</param>
		/// <returns></returns>
		Register[] ICallingConvention.GetReturnRegisters(CilElementType returnType)
		{
			if (returnType == CilElementType.Void)
				return ReturnVoidRegisters;

			if (returnType == CilElementType.R4 || returnType == CilElementType.R8)
				return ReturnFPRegisters;

			if (returnType == CilElementType.I8 || returnType == CilElementType.U8)
				return Return64BitRegisters;

			return Return32BitRegisters;
		}
		#endregion // ICallingConvention Members
	}
}
