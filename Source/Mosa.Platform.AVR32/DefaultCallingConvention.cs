/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Pascal Delprat (pdelprat) <pascal.delprat@online.fr>
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.Metadata.Signatures;
using System;
using System.Collections.Generic;

namespace Mosa.Platform.AVR32
{
	/// <summary>
	/// Implements the default calling convention for AVR32.
	/// </summary>
	internal sealed class DefaultCallingConvention : BaseCallingConventionExtended
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="DefaultCallingConvention"/>.
		/// </summary>
		/// <param name="architecture">The architecture of the calling convention.</param>
		/// <param name="typeLayout">The type layout.</param>
		public DefaultCallingConvention(BaseArchitecture architecture)
			: base(architecture)
		{
		}

		#endregion Construction

		#region BaseCallingConvention Members

		/// <summary>
		/// Expands the given invoke instruction to perform the method call.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <returns>
		/// A single instruction or an array of instructions, which appropriately represent the method call.
		/// </returns>
		public override void MakeCall(Context ctx)
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

		private void FreeStackAfterCall(Context context, int stackSize)
		{
			Operand sp = Operand.CreateCPURegister(BuiltInSigType.IntPtr, GeneralPurposeRegister.SP);
			Operand r7 = Operand.CreateCPURegister(BuiltInSigType.IntPtr, GeneralPurposeRegister.R7);
			if (stackSize != 0)
			{
				context.AppendInstruction(AVR32.Mov, r7, Operand.CreateConstantIntPtr(stackSize));
				context.AppendInstruction(AVR32.Add, sp, r7);
			}
		}

		private void CleanupReturnValue(Context context, Operand result)
		{
			if (result != null)
			{
				if (result.Is64BitInteger)
					MoveReturnValueTo64Bit(result, context);
				else
					MoveReturnValueTo32Bit(result, context);
			}
		}

		/// <summary>
		/// Calculates the remaining space.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="operandStack">The operand stack.</param>
		/// <param name="space">The space.</param>
		private void PushOperands(Context context, Stack<Operand> operandStack, int space)
		{
			while (operandStack.Count != 0)
			{
				Operand operand = operandStack.Pop();

				int size, alignment;
				architecture.GetTypeRequirements(operand.Type, out size, out alignment);

				space -= size;
				Push(context, operand, space, size);
			}
		}

		/// <summary>
		/// Moves the return value to 32 bit.
		/// </summary>
		/// <param name="resultOperand">The result operand.</param>
		/// <param name="context">The context.</param>
		private void MoveReturnValueTo32Bit(Operand resultOperand, Context context)
		{
			Operand r8 = Operand.CreateCPURegister(resultOperand.Type, GeneralPurposeRegister.R8);
			context.AppendInstruction(AVR32.Mov, resultOperand, r8);
		}

		/// <summary>
		/// Moves the return value to 64 bit.
		/// </summary>
		/// <param name="resultOperand">The result operand.</param>
		/// <param name="context">The context.</param>
		private void MoveReturnValueTo64Bit(Operand resultOperand, Context context)
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
		/// <param name="context">The context.</param>
		/// <param name="op">The op.</param>
		/// <param name="stackSize">Size of the stack.</param>
		/// <param name="parameterSize">Size of the parameter.</param>
		private void Push(Context context, Operand op, int stackSize, int parameterSize)
		{
			// TODO
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
		/// <param name="context">The context.</param>
		/// <param name="operand">The operand, that's holding the return value.</param>
		public override void SetReturnValue(Context context, Operand operand)
		{
			int size, alignment;
			architecture.GetTypeRequirements(operand.Type, out size, out alignment);

			// FIXME: Do not issue a move, if the operand is already the destination register
			if (size == 4 || size == 2 || size == 1)
			{
				context.SetInstruction(AVR32.Mov, Operand.CreateCPURegister(operand.Type, GeneralPurposeRegister.R8), operand);
				return;
			}
			else if (size == 8 && (operand.IsFloat || operand.IsDouble))
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
			else if (size == 8 && (operand.IsSignedLong || operand.IsUnsignedLong))
			{
				SigType HighType = (operand.IsSignedLong) ? BuiltInSigType.Int32 : BuiltInSigType.UInt32;

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

		public override void GetStackRequirements(Operand stackOperand, out int size, out int alignment)
		{
			// Special treatment for some stack types
			// FIXME: Handle the size and alignment requirements of value types
			architecture.GetTypeRequirements(stackOperand.Type, out size, out alignment);
		}

		public override int OffsetOfFirstLocal
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

		public override int OffsetOfFirstParameter
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

		#endregion BaseCallingConvention Members
	}
}