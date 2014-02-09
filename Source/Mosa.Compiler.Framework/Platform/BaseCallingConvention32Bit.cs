/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

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

		#region Members

		/// <summary>
		/// Expands the given invoke instruction to perform the method call.
		/// </summary>
		/// <param name="typeSystem">The type system.</param>
		/// <param name="context">The context.</param>
		public override void MakeCall(TypeSystem typeSystem, Context context)
		{
			/*
			 * Calling convention is right-to-left, pushed on the stack. Return value in EAX for integral
			 * types 4 bytes or less, XMM0 for floating point and EAX:EDX for 64-bit. If this is a method
			 * of a type, the this argument is moved to ECX right before the call.
			 */

			Operand target = context.Operand1;
			Operand result = context.Result;
			MosaMethod method = context.InvokeMethod;

			Debug.Assert(method != null);

			Operand scratch = Operand.CreateCPURegister(typeSystem.BuiltIn.Ptr, scratchRegister);

			List<Operand> operands = BuildOperands(context);

			int stackSize = CalculateStackSizeForParameters(architecture, operands, method);

			context.Remove();

			if (stackSize != 0)
			{
				ReserveStackSizeForCall(typeSystem, context, stackSize, scratch);
				PushOperands(typeSystem, context, method, operands, stackSize, scratch);
			}

			// the mov/call two-instructions combo is to help facilitate the register allocator
			architecture.InsertMoveInstruction(context, scratch, target);
			architecture.InsertCallInstruction(context, scratch);

			FreeStackAfterCall(typeSystem, context, stackSize);
			CleanupReturnValue(typeSystem, context, result);
		}

		/// <summary>
		/// Reserves the stack size for call.
		/// </summary>
		/// <param name="typeSystem">The type system.</param>
		/// <param name="context">The context.</param>
		/// <param name="stackSize">Size of the stack.</param>
		/// <param name="scratch">The scratch.</param>
		private void ReserveStackSizeForCall(TypeSystem typeSystem, Context context, int stackSize, Operand scratch)
		{
			if (stackSize == 0)
				return;

			Operand stackPointer = Operand.CreateCPURegister(typeSystem.BuiltIn.Ptr, architecture.StackPointerRegister);

			architecture.InsertSubInstruction(context, stackPointer, stackPointer, Operand.CreateConstant(typeSystem.BuiltIn.I4, stackSize));
			architecture.InsertMoveInstruction(context, scratch, stackPointer);
		}

		/// <summary>
		/// Frees the stack after call.
		/// </summary>
		/// <param name="typeSystem">The type system.</param>
		/// <param name="context">The context.</param>
		/// <param name="stackSize">Size of the stack.</param>
		private void FreeStackAfterCall(TypeSystem typeSystem, Context context, int stackSize)
		{
			if (stackSize == 0)
				return;

			Operand stackPointer = Operand.CreateCPURegister(typeSystem.BuiltIn.Ptr, architecture.StackPointerRegister);
			architecture.InsertAddInstruction(context, stackPointer, stackPointer, Operand.CreateConstant(typeSystem.BuiltIn.I4, stackSize));
		}

		/// <summary>
		/// Cleanups the return value.
		/// </summary>
		/// <param name="typeSystem">The type system.</param>
		/// <param name="context">The context.</param>
		/// <param name="result">The result.</param>
		private void CleanupReturnValue(TypeSystem typeSystem, Context context, Operand result)
		{
			if (result == null)
				return;

			if (result.IsFloatingPoint)
			{
				Operand returnFP = Operand.CreateCPURegister(result.Type, returnFloatingPointRegister);
				architecture.InsertMoveInstruction(context, result, returnFP);
			}
			else if (result.Is64BitInteger)
			{
				Operand returnLow = Operand.CreateCPURegister(result.Type, return32BitRegister);
				Operand returnHigh = Operand.CreateCPURegister(typeSystem.BuiltIn.U4, return64BitRegister);
				architecture.InsertMoveInstruction(context, result.Low, returnLow);
				architecture.InsertMoveInstruction(context, result.High, returnHigh);
			}
			else
			{
				Operand returnLow = Operand.CreateCPURegister(result.Type, return32BitRegister);
				architecture.InsertMoveInstruction(context, result, returnLow);
			}
		}

		/// <summary>
		/// Calculates the remaining space.
		/// </summary>
		/// <param name="typeSystem">The type system.</param>
		/// <param name="context">The context.</param>
		/// <param name="method">The method.</param>
		/// <param name="operands">The operand stack.</param>
		/// <param name="space">The space.</param>
		/// <param name="scratch">The scratch.</param>
		private void PushOperands(TypeSystem typeSystem, Context context, MosaMethod method, List<Operand> operands, int space, Operand scratch)
		{
			Debug.Assert((method.Parameters.Count + (method.HasThis ? 1 : 0) == operands.Count) ||
						(method.DeclaringType.IsDelegate && method.Parameters.Count == operands.Count));

			int offset = method.Parameters.Count - operands.Count;

			for (int index = operands.Count - 1; index >= 0; index--)
			{
				Operand operand = operands[index];

				MosaType param = (index + offset >= 0) ? method.Parameters[index + offset].Type : null;

				int size, alignment;
				architecture.GetTypeRequirements(operand.Type, out size, out alignment);

				if (param != null && operand.IsDouble && param.IsSingle)
				{
					size = 4; alignment = 4;
				}

				space -= size;
				Push(typeSystem, context, operand, space, size, scratch);
			}
		}

		/// <summary>
		/// Pushes the specified instructions.
		/// </summary>
		/// <param name="typeSystem">The type system.</param>
		/// <param name="context">The context.</param>
		/// <param name="op">The op.</param>
		/// <param name="stackSize">Size of the stack.</param>
		/// <param name="parameterSize">Size of the parameter.</param>
		/// <param name="scratch">The scratch.</param>
		private void Push(TypeSystem typeSystem, Context context, Operand op, int stackSize, int parameterSize, Operand scratch)
		{
			Debug.Assert(!op.IsMemoryAddress);

			if (op.Is64BitInteger)
			{
				architecture.InsertMoveInstruction(context, Operand.CreateMemoryAddress(typeSystem.BuiltIn.I4, scratch, stackSize), op.Low);
				architecture.InsertMoveInstruction(context, Operand.CreateMemoryAddress(typeSystem.BuiltIn.I4, scratch, stackSize + 4), op.High);
			}
			else if (op.IsFloatingPoint)
			{
				if (op.IsDouble && parameterSize == 4)
				{
					Operand op2 = Operand.CreateCPURegister(typeSystem.BuiltIn.R4, returnFloatingPointRegister);
					architecture.InsertMoveInstruction(context, op2, op);
					architecture.InsertMoveInstruction(context, Operand.CreateMemoryAddress(scratch.Type, scratch, stackSize), op2);
				}
				else
				{
					architecture.InsertMoveInstruction(context, Operand.CreateMemoryAddress(scratch.Type, scratch, stackSize), op);
				}
			}
			else
			{
				architecture.InsertMoveInstruction(context, Operand.CreateMemoryAddress(op.Type, scratch, stackSize), op);
			}
		}

		/// <summary>
		/// Requests the calling convention to create an appropriate move instruction to populate the return
		/// value of a method.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="operand">The operand, that's holding the return value.</param>
		public override void SetReturnValue(TypeSystem typeSystem, Context context, Operand operand)
		{
			int size, alignment;
			architecture.GetTypeRequirements(operand.Type, out size, out alignment);

			if (size == 4 || size == 2 || size == 1)
			{
				architecture.InsertMoveInstruction(context, Operand.CreateCPURegister(operand.Type, return32BitRegister), operand);
			}
			else if (operand.IsSingle)
			{
				architecture.InsertMoveInstruction(context, Operand.CreateCPURegister(operand.Type, returnFloatingPointRegister), operand);
			}
			else if (operand.IsDouble)
			{
				architecture.InsertMoveInstruction(context, Operand.CreateCPURegister(operand.Type, returnFloatingPointRegister), operand);
			}
			else if (operand.IsLong)
			{
				MosaType highType = (operand.IsSignedLong) ? typeSystem.BuiltIn.I4 : typeSystem.BuiltIn.U4;

				architecture.InsertMoveInstruction(context, Operand.CreateCPURegister(typeSystem.BuiltIn.U4, return32BitRegister), operand.Low);
				architecture.InsertMoveInstruction(context, Operand.CreateCPURegister(highType, return64BitRegister), operand.High);
			}
		}

		public override int OffsetOfFirstLocal { get { return 0; } }

		public override int OffsetOfFirstParameter
		{
			//The first parameter is offset by 8 bytes from the start of the stack frame.
			//- holds the return stack frame, which was pushed by the prologue instruction.
			//- holds the return address, which was pushed by the call instruction.
			get { return 8; }
		}

		#endregion Members
	}
}