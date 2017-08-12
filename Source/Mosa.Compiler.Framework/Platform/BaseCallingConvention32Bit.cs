// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Platform
{
	/// <summary>
	/// Implements the default 32-bit calling convention.
	/// </summary>
	public class BaseCallingConvention32Bit : BaseCallingConventionExtended
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseCallingConvention32Bit"/>.
		/// </summary>
		/// <param name="architecture">The architecture of the calling convention.</param>
		public BaseCallingConvention32Bit(BaseArchitecture architecture)
			: base(architecture)
		{
		}

		#endregion Construction

		#region Data members

		protected Register scratchRegister;
		protected Register return32BitRegister;
		protected Register return64BitRegister;
		protected Register returnFloatingPointRegister;

		#endregion Data members

		#region Properties

		//The first parameter is offset by 8 bytes from the start of the stack frame.
		//- holds the return stack frame, which was pushed by the prologue instruction.
		//- holds the return address, which was pushed by the call instruction.

		public override int OffsetOfFirstLocal { get { return 0; } }

		public override int OffsetOfFirstParameter { get { return 8; } }

		#endregion Properties

		#region Members

		/// <summary>
		/// Expands method call instruction represented by the context to perform the method call.
		/// </summary>
		/// <param name="compiler">The compiler.</param>
		/// <param name="context">The context.</param>
		public override void MakeCall(BaseMethodCompiler compiler, Context context)
		{
			/*
			 * Calling convention is right-to-left, pushed on the stack. Return value in EAX for integral
			 * types 4 bytes or less, XMM0 for floating point and EAX:EDX for 64-bit. If this is a method
			 * of a type, the this argument is moved to ECX right before the call.
			 * If return value is value type, a stack local is allocated as a hidden parameter in caller
			 * stack, the callee will then store the return value in the allocated space
			 * The return value is the first parameter (even before this)
			 * The callee will place the address of the return value into EAX and the caller will then
			 * either retrieve the return value using compound move or will let one of the callers higher
			 * in the caller chain handle the retrieval of the return value using compound move.
			 */

			Operand target = context.Operand1;
			Operand result = context.Result;
			MosaMethod method = context.InvokeMethod;

			//Debug.Assert(method != null, context.ToString());

			Operand scratch = Operand.CreateCPURegister(compiler.TypeSystem.BuiltIn.Pointer, scratchRegister);

			var operands = BuildOperands(context);

			int stackSize = 0;
			int returnSize = 0;

			stackSize = compiler.TypeLayout.GetMethodParameterStackSize(method);
			returnSize = CalculateReturnSize(compiler, method);

			context.Empty();

			int totalStack = returnSize + stackSize;

			if (totalStack != 0)
			{
				ReserveStackSizeForCall(compiler, context, totalStack, scratch);
				PushOperands(compiler, context, method, operands, totalStack, scratch);
			}

			// the mov/call two-instructions combo is to help facilitate the register allocator
			architecture.InsertMoveInstruction(context, scratch, target);
			architecture.InsertCallInstruction(context, scratch);

			CleanupReturnValue(compiler, context, result);
			FreeStackAfterCall(compiler, context, totalStack);
		}

		private static int CalculateReturnSize(BaseMethodCompiler compiler, MosaMethod method)
		{
			if (MosaTypeLayout.IsStoredOnStack(method.Signature.ReturnType))
			{
				return compiler.TypeLayout.GetTypeSize(method.Signature.ReturnType);
			}

			return 0;
		}

		/// <summary>
		/// Reserves the stack size for call.
		/// </summary>
		/// <param name="compiler">The compiler.</param>
		/// <param name="context">The context.</param>
		/// <param name="stackSize">Size of the stack.</param>
		/// <param name="scratch">The scratch.</param>
		private void ReserveStackSizeForCall(BaseMethodCompiler compiler, Context context, int stackSize, Operand scratch)
		{
			if (stackSize == 0)
				return;

			var stackPointer = Operand.CreateCPURegister(compiler.TypeSystem.BuiltIn.Pointer, architecture.StackPointerRegister);

			architecture.InsertSubInstruction(context, stackPointer, stackPointer, Operand.CreateConstant(compiler.TypeSystem.BuiltIn.I4, stackSize));
			architecture.InsertMoveInstruction(context, scratch, stackPointer);
		}

		/// <summary>
		/// Frees the stack after call.
		/// </summary>
		/// <param name="compiler">The compiler.</param>
		/// <param name="context">The context.</param>
		/// <param name="stackSize">Size of the stack.</param>
		private void FreeStackAfterCall(BaseMethodCompiler compiler, Context context, int stackSize)
		{
			if (stackSize == 0)
				return;

			var stackPointer = Operand.CreateCPURegister(compiler.TypeSystem.BuiltIn.Pointer, architecture.StackPointerRegister);
			architecture.InsertAddInstruction(context, stackPointer, stackPointer, Operand.CreateConstant(compiler.TypeSystem.BuiltIn.I4, stackSize));
		}

		/// <summary>
		/// Cleanups the return value.
		/// </summary>
		/// <param name="compiler">The compiler.</param>
		/// <param name="context">The context.</param>
		/// <param name="result">The result.</param>
		private void CleanupReturnValue(BaseMethodCompiler compiler, Context context, Operand result)
		{
			if (result == null)
				return;

			if (result.IsR)
			{
				var returnFP = Operand.CreateCPURegister(result.Type, returnFloatingPointRegister);
				context.AppendInstruction(IRInstruction.Gen, returnFP);
				architecture.InsertMoveInstruction(context, result, returnFP);
			}
			else if (result.Is64BitInteger)
			{
				var returnLow = Operand.CreateCPURegister(result.Type, return32BitRegister);
				var returnHigh = Operand.CreateCPURegister(compiler.TypeSystem.BuiltIn.U4, return64BitRegister);
				context.AppendInstruction(IRInstruction.Gen, returnLow);
				context.AppendInstruction(IRInstruction.Gen, returnHigh);
				architecture.InsertMoveInstruction(context, result.Low, returnLow);
				architecture.InsertMoveInstruction(context, result.High, returnHigh);
			}
			else if (MosaTypeLayout.IsStoredOnStack(result.Type))
			{
				Debug.Assert(result.IsStackLocal);
				int size = compiler.TypeLayout.GetTypeSize(result.Type);
				var stackPointerRegister = Operand.CreateCPURegister(compiler.TypeSystem.BuiltIn.Pointer, architecture.StackPointerRegister);
				architecture.InsertCompoundCopy(compiler, context, compiler.StackFrame, result, stackPointerRegister, compiler.ConstantZero, size);
			}
			else
			{
				var returnLow = Operand.CreateCPURegister(result.Type, return32BitRegister);
				context.AppendInstruction(IRInstruction.Gen, returnLow);
				architecture.InsertMoveInstruction(context, result, returnLow);
			}
		}

		/// <summary>
		/// Calculates the remaining space.
		/// </summary>
		/// <param name="compiler">The compiler.</param>
		/// <param name="context">The context.</param>
		/// <param name="method">The method.</param>
		/// <param name="operands">The operand stack.</param>
		/// <param name="space">The space.</param>
		/// <param name="scratch">The scratch.</param>
		private void PushOperands(BaseMethodCompiler compiler, Context context, MosaMethod method, List<Operand> operands, int space, Operand scratch)
		{
			Debug.Assert((method.Signature.Parameters.Count + (method.HasThis ? 1 : 0) == operands.Count)
						|| (method.DeclaringType.IsDelegate && method.Signature.Parameters.Count == operands.Count));

			for (int index = operands.Count - 1; index >= 0; index--)
			{
				var operand = operands[index];

				architecture.GetTypeRequirements(compiler.TypeLayout, operand.Type, out int size, out int alignment);

				size = Alignment.AlignUp(size, alignment);

				space -= size;

				Push(compiler, context, operand, space, size, scratch);
			}
		}

		/// <summary>
		/// Pushes the specified instructions.
		/// </summary>
		/// <param name="compiler">The compiler.</param>
		/// <param name="context">The context.</param>
		/// <param name="operand">The op.</param>
		/// <param name="offset">Size of the stack.</param>
		/// <param name="size">Size of the parameter.</param>
		/// <param name="scratch">The scratch.</param>
		private void Push(BaseMethodCompiler compiler, Context context, Operand operand, int offset, int size, Operand scratch)
		{
			var offsetOperand = Operand.CreateConstant(compiler.TypeSystem, offset);

			if (operand.Is64BitInteger)
			{
				var offset4Operand = Operand.CreateConstant(compiler.TypeSystem, offset + 4);

				architecture.InsertStoreInstruction(context, scratch, offsetOperand, operand.Low);
				architecture.InsertStoreInstruction(context, scratch, offset4Operand, operand.High);
			}
			else if (operand.IsR)
			{
				if (operand.IsR8 && size == 4)
				{
					var op2 = Operand.CreateCPURegister(compiler.TypeSystem.BuiltIn.R4, returnFloatingPointRegister);
					architecture.InsertMoveInstruction(context, op2, operand);
					architecture.InsertStoreInstruction(context, scratch, offsetOperand, op2);
				}
				else
				{
					architecture.InsertStoreInstruction(context, scratch, offsetOperand, operand);
				}
			}
			else if (MosaTypeLayout.IsStoredOnStack(operand.Type))
			{
				var offset2 = Operand.CreateConstant(compiler.TypeSystem, offset);
				architecture.InsertCompoundCopy(compiler, context, scratch, offset2, compiler.StackFrame, operand, compiler.TypeLayout.GetTypeSize(operand.Type));
			}
			else
			{
				architecture.InsertStoreInstruction(context, scratch, offsetOperand, operand);
			}
		}

		/// <summary>
		/// Requests the calling convention to create an appropriate move instruction to populate the return
		/// value of a method.
		/// </summary>
		/// <param name="compiler">The compiler.</param>
		/// <param name="context">The context.</param>
		/// <param name="operand">The operand, that's holding the return value.</param>
		public override void SetReturnValue(BaseMethodCompiler compiler, Context context, Operand operand)
		{
			architecture.GetTypeRequirements(compiler.TypeLayout, operand.Type, out int size, out int alignment);

			size = Alignment.AlignUp(size, alignment);

			if (operand.IsR4)
			{
				architecture.InsertMoveInstruction(context, Operand.CreateCPURegister(operand.Type, returnFloatingPointRegister), operand);
			}
			else if (operand.IsR8)
			{
				architecture.InsertMoveInstruction(context, Operand.CreateCPURegister(operand.Type, returnFloatingPointRegister), operand);
			}
			else if (operand.IsLong)
			{
				var highType = (operand.IsI8) ? compiler.TypeSystem.BuiltIn.I4 : compiler.TypeSystem.BuiltIn.U4;

				architecture.InsertMoveInstruction(context, Operand.CreateCPURegister(compiler.TypeSystem.BuiltIn.U4, return32BitRegister), operand.Low);
				architecture.InsertMoveInstruction(context, Operand.CreateCPURegister(highType, return64BitRegister), operand.High);
			}
			else if (MosaTypeLayout.IsStoredOnStack(operand.Type))
			{
				int size2 = compiler.TypeLayout.GetTypeSize(operand.Type);
				var OffsetOfFirstParameterOperand = Operand.CreateConstant(compiler.TypeSystem, OffsetOfFirstParameter);
				architecture.InsertCompoundCopy(compiler, context, compiler.StackFrame, OffsetOfFirstParameterOperand, compiler.StackFrame, operand, size2);
			}
			else
			{
				architecture.InsertMoveInstruction(context, Operand.CreateCPURegister(operand.Type, return32BitRegister), operand);
			}
		}

		#endregion Members
	}
}
