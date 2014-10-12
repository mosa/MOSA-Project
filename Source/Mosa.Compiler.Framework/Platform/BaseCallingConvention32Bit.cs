/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

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

		#region Members

		/// <summary>
		/// Expands method call instruction represented by the context to perform the method call.
		/// </summary>
		/// <param name="typeLayout">The type layouts.</param>
		/// <param name="context">The context.</param>
		public override void MakeCall(BaseMethodCompiler compiler, MosaTypeLayout typeLayout, Context context)
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
			MosaMethod method = context.MosaMethod;

			Debug.Assert(method != null, context.ToString());

			Operand scratch = Operand.CreateCPURegister(typeLayout.TypeSystem.BuiltIn.Pointer, scratchRegister);

			List<Operand> operands = BuildOperands(context);

			int stackSize = CalculateStackSizeForParameters(typeLayout, architecture, operands, method);

			context.Remove();

			int returnSize = 0;
			if (typeLayout.IsCompoundType(method.Signature.ReturnType))
			{
				returnSize = typeLayout.GetTypeSize(method.Signature.ReturnType);
			}

			if (stackSize != 0 || returnSize != 0)
			{
				ReserveStackSizeForCall(typeLayout.TypeSystem, context, returnSize + stackSize, scratch);
				PushOperands(compiler, typeLayout, context, method, operands, returnSize + stackSize, scratch);
			}

			// the mov/call two-instructions combo is to help facilitate the register allocator
			architecture.InsertMoveInstruction(context, scratch, target);
			architecture.InsertCallInstruction(context, scratch);

			CleanupReturnValue(compiler, typeLayout, context, result);
			FreeStackAfterCall(typeLayout.TypeSystem, context, returnSize + stackSize);
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

			Operand stackPointer = Operand.CreateCPURegister(typeSystem.BuiltIn.Pointer, architecture.StackPointerRegister);

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

			Operand stackPointer = Operand.CreateCPURegister(typeSystem.BuiltIn.Pointer, architecture.StackPointerRegister);
			architecture.InsertAddInstruction(context, stackPointer, stackPointer, Operand.CreateConstant(typeSystem.BuiltIn.I4, stackSize));
		}

		/// <summary>
		/// Cleanups the return value.
		/// </summary>
		/// <param name="compiler">The compiler.</param>
		/// <param name="typeLayout">The type layouts.</param>
		/// <param name="context">The context.</param>
		/// <param name="result">The result.</param>
		private void CleanupReturnValue(BaseMethodCompiler compiler, MosaTypeLayout typeLayout, Context context, Operand result)
		{
			if (result == null)
				return;

			if (result.IsR)
			{
				Operand returnFP = Operand.CreateCPURegister(result.Type, returnFloatingPointRegister);
				context.AppendInstruction(IRInstruction.Gen, returnFP);
				architecture.InsertMoveInstruction(context, result, returnFP);
			}
			else if (result.Is64BitInteger)
			{
				Operand returnLow = Operand.CreateCPURegister(result.Type, return32BitRegister);
				Operand returnHigh = Operand.CreateCPURegister(typeLayout.TypeSystem.BuiltIn.U4, return64BitRegister);
				context.AppendInstruction(IRInstruction.Gen, returnLow);
				context.AppendInstruction(IRInstruction.Gen, returnHigh);
				architecture.InsertMoveInstruction(context, result.Low, returnLow);
				architecture.InsertMoveInstruction(context, result.High, returnHigh);
			}
			else if (typeLayout.IsCompoundType(result.Type))
			{
				int size = typeLayout.GetTypeSize(result.Type);
				Operand returnLow = Operand.CreateCPURegister(typeLayout.TypeSystem.BuiltIn.Pointer, return32BitRegister);
				context.AppendInstruction(IRInstruction.Gen, returnLow);
				architecture.InsertCompoundMoveInstruction(compiler, context, result, Operand.CreateMemoryAddress(result.Type, returnLow, 0), size);
			}
			else
			{
				Operand returnLow = Operand.CreateCPURegister(result.Type, return32BitRegister);
				context.AppendInstruction(IRInstruction.Gen, returnLow);
				architecture.InsertMoveInstruction(context, result, returnLow);
			}
		}

		/// <summary>
		/// Calculates the remaining space.
		/// </summary>
		/// <param name="compiler">The compiler.</param>
		/// <param name="typeLayout">The type layouts.</param>
		/// <param name="context">The context.</param>
		/// <param name="method">The method.</param>
		/// <param name="operands">The operand stack.</param>
		/// <param name="space">The space.</param>
		/// <param name="scratch">The scratch.</param>
		private void PushOperands(BaseMethodCompiler compiler, MosaTypeLayout typeLayout, Context context, MosaMethod method, List<Operand> operands, int space, Operand scratch)
		{
			Debug.Assert((method.Signature.Parameters.Count + (method.HasThis ? 1 : 0) == operands.Count) ||
						(method.DeclaringType.IsDelegate && method.Signature.Parameters.Count == operands.Count));

			int offset = method.Signature.Parameters.Count - operands.Count;

			for (int index = operands.Count - 1; index >= 0; index--)
			{
				Operand operand = operands[index];

				MosaType param = (index + offset >= 0) ? method.Signature.Parameters[index + offset].ParameterType : null;

				int size, alignment;

				if (param != null && operand.IsR8 && param.IsR4)
				{
					architecture.GetTypeRequirements(typeLayout, param, out size, out alignment);
				}
				else
				{
					architecture.GetTypeRequirements(typeLayout, operand.Type, out size, out alignment);
				}

				size = Alignment.AlignUp(size, alignment);

				space -= size;

				Push(compiler, typeLayout, context, operand, space, size, scratch);
			}
		}

		/// <summary>
		/// Pushes the specified instructions.
		/// </summary>
		/// <param name="compiler">The compiler.</param>
		/// <param name="typeLayout">The type layout.</param>
		/// <param name="context">The context.</param>
		/// <param name="op">The op.</param>
		/// <param name="stackSize">Size of the stack.</param>
		/// <param name="parameterSize">Size of the parameter.</param>
		/// <param name="scratch">The scratch.</param>
		private void Push(BaseMethodCompiler compiler, MosaTypeLayout typeLayout, Context context, Operand op, int stackSize, int parameterSize, Operand scratch)
		{
			if (op.Is64BitInteger)
			{
				architecture.InsertMoveInstruction(context, Operand.CreateMemoryAddress(typeLayout.TypeSystem.BuiltIn.I4, scratch, stackSize), op.Low);
				architecture.InsertMoveInstruction(context, Operand.CreateMemoryAddress(typeLayout.TypeSystem.BuiltIn.I4, scratch, stackSize + 4), op.High);
			}
			else if (op.IsR)
			{
				if (op.IsR8 && parameterSize == 4)
				{
					Operand op2 = Operand.CreateCPURegister(typeLayout.TypeSystem.BuiltIn.R4, returnFloatingPointRegister);
					architecture.InsertMoveInstruction(context, op2, op);
					architecture.InsertMoveInstruction(context, Operand.CreateMemoryAddress(scratch.Type, scratch, stackSize), op2);
				}
				else
				{
					architecture.InsertMoveInstruction(context, Operand.CreateMemoryAddress(scratch.Type, scratch, stackSize), op);
				}
			}
			else if (typeLayout.IsCompoundType(op.Type))
			{
				architecture.InsertCompoundMoveInstruction(compiler, context, Operand.CreateMemoryAddress(op.Type, scratch, stackSize), op, typeLayout.GetTypeSize(op.Type));
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
		/// <param name="compiler">The compiler.</param>
		/// <param name="typeLayout">The type layouts.</param>
		/// <param name="context">The context.</param>
		/// <param name="operand">The operand, that's holding the return value.</param>
		public override void SetReturnValue(BaseMethodCompiler compiler, MosaTypeLayout typeLayout, Context context, Operand operand)
		{
			int size, alignment;
			architecture.GetTypeRequirements(typeLayout, operand.Type, out size, out alignment);
			size = Alignment.AlignUp(size, alignment);

			if (size == 4 || size == 2 || size == 1)
			{
				architecture.InsertMoveInstruction(context, Operand.CreateCPURegister(operand.Type, return32BitRegister), operand);
			}
			else if (operand.IsR4)
			{
				architecture.InsertMoveInstruction(context, Operand.CreateCPURegister(operand.Type, returnFloatingPointRegister), operand);
			}
			else if (operand.IsR8)
			{
				architecture.InsertMoveInstruction(context, Operand.CreateCPURegister(operand.Type, returnFloatingPointRegister), operand);
			}
			else if (operand.IsLong)
			{
				MosaType highType = (operand.IsI8) ? typeLayout.TypeSystem.BuiltIn.I4 : typeLayout.TypeSystem.BuiltIn.U4;

				architecture.InsertMoveInstruction(context, Operand.CreateCPURegister(typeLayout.TypeSystem.BuiltIn.U4, return32BitRegister), operand.Low);
				architecture.InsertMoveInstruction(context, Operand.CreateCPURegister(highType, return64BitRegister), operand.High);
			}
			else if (typeLayout.IsCompoundType(operand.Type))
			{
				architecture.InsertAddressOfInstruction(context, Operand.CreateCPURegister(operand.Type, return32BitRegister), operand);
			}
		}

		public override int OffsetOfFirstLocal { get { return 0; } }

		//The first parameter is offset by 8 bytes from the start of the stack frame.
		//- holds the return stack frame, which was pushed by the prologue instruction.
		//- holds the return address, which was pushed by the call instruction.
		public override int OffsetOfFirstParameter { get { return 8; } }

		#endregion Members
	}
}