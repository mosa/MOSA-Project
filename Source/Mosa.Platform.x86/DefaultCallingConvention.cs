/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.TypeSystem;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Platform.x86
{
	/// <summary>
	/// Implements the CIL default calling convention for x86.
	/// </summary>
	internal sealed class DefaultCallingConvention : ICallingConvention
	{
		#region Data members

		/// <summary>
		/// Holds the architecture of the calling convention.
		/// </summary>
		private IArchitecture architecture;

		#endregion Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="DefaultCallingConvention"/>.
		/// </summary>
		/// <param name="architecture">The architecture of the calling convention.</param>
		public DefaultCallingConvention(IArchitecture architecture)
		{
			if (architecture == null)
				throw new ArgumentNullException(@"architecture");

			this.architecture = architecture;
		}

		#endregion Construction

		#region ICallingConvention Members

		/// <summary>
		/// Expands the given invoke instruction to perform the method call.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns>
		/// A single instruction or an array of instructions, which appropriately represent the method call.
		/// </returns>
		void ICallingConvention.MakeCall(Context context)
		{
			/*
			 * Calling convention is right-to-left, pushed on the stack. Return value in EAX for integral
			 * types 4 bytes or less, XMM0 for floating point and EAX:EDX for 64-bit. If this is a method
			 * of a type, the this argument is moved to ECX right before the call.
			 */

			Operand target = context.Operand1;
			Operand result = context.Result;
			RuntimeMethod method = context.InvokeMethod;

			Debug.Assert(method != null);

			Operand edx = Operand.CreateCPURegister(BuiltInSigType.IntPtr, GeneralPurposeRegister.EDX);

			List<Operand> operands = BuildOperands(context);

			int stackSize = CalculateStackSizeForParameters(operands, method);

			context.Remove();

			if (stackSize != 0)
			{
				ReserveStackSizeForCall(context, stackSize, edx);
				PushOperands(context, method, operands, stackSize, edx);
			}

			// the mov/call two-instructions combo is to help facilitate the register allocator
			context.AppendInstruction(X86.Mov, edx, target);
			context.AppendInstruction(X86.Call, null, edx);

			FreeStackAfterCall(context, stackSize);
			CleanupReturnValue(context, result);
		}

		private List<Operand> BuildOperands(Context context)
		{
			List<Operand> operands = new List<Operand>(context.OperandCount - 1);
			int index = 0;

			foreach (Operand operand in context.Operands)
			{
				if (index++ > 0)
				{
					operands.Add(operand);
				}
			}

			return operands;
		}

		private void ReserveStackSizeForCall(Context context, int stackSize, Operand edx)
		{
			if (stackSize == 0)
				return;

			Operand esp = Operand.CreateCPURegister(BuiltInSigType.IntPtr, GeneralPurposeRegister.ESP);

			context.AppendInstruction(X86.Sub, esp, esp, Operand.CreateConstant(BuiltInSigType.IntPtr, stackSize));
			context.AppendInstruction(X86.Mov, edx, esp);
		}

		private void FreeStackAfterCall(Context context, int stackSize)
		{
			if (stackSize == 0)
				return;

			Operand esp = Operand.CreateCPURegister(BuiltInSigType.IntPtr, GeneralPurposeRegister.ESP);
			context.AppendInstruction(X86.Add, esp, esp, Operand.CreateConstant(BuiltInSigType.IntPtr, stackSize));
		}

		private void CleanupReturnValue(Context context, Operand result)
		{
			if (result == null)
				return;

			if (result.StackType == StackTypeCode.F)
			{
				Operand xmm0 = Operand.CreateCPURegister(result.Type, SSE2Register.XMM0);
				context.AppendInstruction(X86.Movsd, result, xmm0);
			}
			else if (result.StackType == StackTypeCode.Int64)
			{
				Operand eax = Operand.CreateCPURegister(result.Type, GeneralPurposeRegister.EAX);
				Operand edx = Operand.CreateCPURegister(BuiltInSigType.Int32, GeneralPurposeRegister.EDX);

				context.AppendInstruction(X86.Mov, result.Low, eax);
				context.AppendInstruction(X86.Mov, result.High, edx);
			}
			else
			{
				Operand eax = Operand.CreateCPURegister(result.Type, GeneralPurposeRegister.EAX);
				context.AppendInstruction(X86.Mov, result, eax);
			}
		}

		/// <summary>
		/// Calculates the remaining space.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="method">The method.</param>
		/// <param name="operands">The operand stack.</param>
		/// <param name="space">The space.</param>
		/// <param name="edx">The edx.</param>
		private void PushOperands(Context context, RuntimeMethod method, List<Operand> operands, int space, Operand edx)
		{
			Debug.Assert((method.Parameters.Count + (method.HasThis ? 1 : 0) == operands.Count) ||
						(method.DeclaringType.IsDelegate && method.Parameters.Count == operands.Count));

			int offset = method.Parameters.Count - operands.Count;

			for (int index = operands.Count - 1; index >= 0; index--)
			{
				Operand operand = operands[index];

				SigType param = (index + offset >= 0) ? method.SigParameters[index + offset] : null;

				int size, alignment;
				architecture.GetTypeRequirements(operand.Type, out size, out alignment);

				if (param != null && operand.Type.Type == CilElementType.R8 && param.Type == CilElementType.R4)
				{
					size = 4; alignment = 4;
				}

				space -= size;
				Push(context, operand, space, size, edx);
			}
		}

		/// <summary>
		/// Pushes the specified instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="op">The op.</param>
		/// <param name="stackSize">Size of the stack.</param>
		/// <param name="parameterSize">Size of the parameter.</param>
		/// <param name="edx">The edx.</param>
		/// <exception cref="System.NotSupportedException"></exception>
		private void Push(Context context, Operand op, int stackSize, int parameterSize, Operand edx)
		{
			Debug.Assert(!op.IsMemoryAddress);

			if (op.StackType == StackTypeCode.Int64)
			{
				context.AppendInstruction(X86.Mov, Operand.CreateMemoryAddress(BuiltInSigType.Int32, edx, stackSize), op.Low);
				context.AppendInstruction(X86.Mov, Operand.CreateMemoryAddress(BuiltInSigType.Int32, edx, stackSize + 4), op.High);

				return;
			}

			if (op.StackType == StackTypeCode.F)
			{
				if (op.Type.Type == CilElementType.R8 && parameterSize == 4)
				{
					Operand xmm1 = Operand.CreateCPURegister(BuiltInSigType.Single, SSE2Register.XMM0);
					context.AppendInstruction(X86.Cvtsd2ss, xmm1, op);
					architecture.InsertMove(context, Operand.CreateMemoryAddress(edx.Type, edx, stackSize), xmm1);
				}
				else
				{
					architecture.InsertMove(context, Operand.CreateMemoryAddress(edx.Type, edx, stackSize), op);
				}
				return;
			}

			architecture.InsertMove(context, Operand.CreateMemoryAddress(op.Type, edx, stackSize), op);
		}

		/// <summary>
		/// Calculates the stack size for parameters.
		/// </summary>
		/// <param name="operands">The operands.</param>
		/// <param name="method">The method.</param>
		/// <returns></returns>
		private int CalculateStackSizeForParameters(List<Operand> operands, RuntimeMethod method)
		{
			Debug.Assert((method.Parameters.Count + (method.HasThis ? 1 : 0) == operands.Count) ||
			(method.DeclaringType.IsDelegate && method.Parameters.Count == operands.Count));

			int offset = method.Parameters.Count - operands.Count;
			int result = 0;

			for (int index = operands.Count - 1; index >= 0; index--)
			{
				Operand operand = operands[index];

				int size, alignment;
				architecture.GetTypeRequirements(operand.Type, out size, out alignment);

				SigType param = (index + offset >= 0) ? method.SigParameters[index + offset] : null;

				if (param != null && operand.Type.Type == CilElementType.R8 && param.Type == CilElementType.R4)
				{
					size = 4; alignment = 4;
				}

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
		void ICallingConvention.MoveReturnValue(Context context, Operand operand)
		{
			int size, alignment;
			architecture.GetTypeRequirements(operand.Type, out size, out alignment);

			if (size == 4 || size == 2 || size == 1)
			{
				context.SetInstruction(X86.Mov, Operand.CreateCPURegister(operand.Type, GeneralPurposeRegister.EAX), operand);
				return;
			}
			else if (operand.Type.Type == CilElementType.R4)
			{
				context.SetInstruction(X86.Movss, Operand.CreateCPURegister(operand.Type, SSE2Register.XMM0), operand);
				return;
			}
			else if (operand.Type.Type == CilElementType.R8)
			{
				context.SetInstruction(X86.Movsd, Operand.CreateCPURegister(operand.Type, SSE2Register.XMM0), operand);

				return;
			}
			else if (operand.Type.Type == CilElementType.I8 || operand.Type.Type == CilElementType.U8)
			{
				SigType HighType = (operand.Type.Type == CilElementType.I8) ? BuiltInSigType.Int32 : BuiltInSigType.UInt32;

				Operand opL = operand.Low;
				Operand opH = operand.High;

				context.SetInstruction(X86.Mov, Operand.CreateCPURegister(BuiltInSigType.UInt32, GeneralPurposeRegister.EAX), opL);
				context.AppendInstruction(X86.Mov, Operand.CreateCPURegister(HighType, GeneralPurposeRegister.EDX), opH);

				return;
			}

			throw new NotSupportedException();
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
				return 0;
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

		#endregion ICallingConvention Members
	}
}