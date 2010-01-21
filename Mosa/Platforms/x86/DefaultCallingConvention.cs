/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.CompilerFramework.Operands;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Metadata;
using CPUx86 = Mosa.Platforms.x86.CPUx86;
using CIL = Mosa.Runtime.CompilerFramework.CIL;
using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Platforms.x86
{
	/// <summary>
	/// Implements the CIL default calling convention for x86.
	/// </summary>
	sealed class DefaultCallingConvention : ICallingConvention
	{
		#region Data members

		/// <summary>
		/// Holds the architecture of the calling convention.
		/// </summary>
		private IArchitecture architecture;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="DefaultCallingConvention"/>.
		/// </summary>
		/// <param name="architecture">The architecture of the calling convention.</param>
		public DefaultCallingConvention(IArchitecture architecture)
		{
			if (null == architecture)
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
		void ICallingConvention.Expand(Context ctx, IMetadataProvider metadata)
		{
			/*
			 * Calling convention is right-to-left, pushed on the stack. Return value in EAX for integral
			 * types 4 bytes or less, XMM0 for floating point and EAX:EDX for 64-bit. If this is a method
			 * of a type, the this argument is moved to ECX right before the call.
			 * 
			 */

			Mosa.Runtime.Vm.RuntimeMethod invokeTarget = ctx.InvokeTarget;
			Operand result = ctx.Result;
			Operand operand1 = ctx.Operand1;

			List<Operand> operands = new List<Operand>();
			operands.AddRange(ctx.Operands);

			int resultCount = ctx.ResultCount;
			int operandCount = ctx.OperandCount;

			SigType I = new SigType(CilElementType.I);
			RegisterOperand esp = new RegisterOperand(I, GeneralPurposeRegister.ESP);
			int stackSize = CalculateStackSizeForParameters(operands, invokeTarget.Signature.HasThis, metadata);

			ctx.SetInstruction(CPUx86.Instruction.NopInstruction);
			if (stackSize != 0) 
            {
				ctx.AppendInstruction(CPUx86.Instruction.SubInstruction, esp, new ConstantOperand(I, stackSize));
				ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, new RegisterOperand(architecture.NativeType, GeneralPurposeRegister.EDX), esp);

				Stack<Operand> operandStack = GetOperandStackFromInstruction(operands, operandCount, invokeTarget.Signature.HasThis);

				int space = stackSize;
				CalculateRemainingSpace(ctx, operandStack, ref space, metadata);
			}

			if (invokeTarget.Signature.HasThis) 
            {
				RegisterOperand ecx = new RegisterOperand(I, GeneralPurposeRegister.ECX);
                RegisterOperand eax = new RegisterOperand(I, GeneralPurposeRegister.EAX);
				//ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, eax, operand1);
                //ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, new MemoryOperand (new SigType(CilElementType.Ptr), GeneralPurposeRegister.EDX, new IntPtr(0)), eax);
			}

			ctx.AppendInstruction(IR.Instruction.CallInstruction, invokeTarget);

			if (stackSize != 0)
				ctx.AppendInstruction(CPUx86.Instruction.AddInstruction, esp, new ConstantOperand(I, stackSize));

			if (resultCount > 0)
				if (result.StackType == StackTypeCode.Int64)
					MoveReturnValueTo64Bit(result, ctx);
				else
					MoveReturnValueTo32Bit(result, ctx);

			//ctx.Remove();
		}

		/// <summary>
		/// Gets the operand stack from instruction.
		/// </summary>
		/// <param name="operands"></param>
		/// <param name="operandCount"></param>
		/// <param name="moveThis"></param>
		/// <returns></returns>
		private Stack<Operand> GetOperandStackFromInstruction(IEnumerable<Operand> operands, int operandCount, bool moveThis)
		{
			Stack<Operand> operandStack = new Stack<Operand>(operandCount);
			int thisArg = 1;

			foreach (Operand operand in operands) 
            {
				if (moveThis && 1 == thisArg) 
                {
					thisArg = 0;
					//continue;
				}
				operandStack.Push(operand);
			}

			return operandStack;
		}

		/// <summary>
		/// Calculates the remaining space.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="operandStack">The operand stack.</param>
		/// <param name="space">The space.</param>
		private void CalculateRemainingSpace(Context ctx, Stack<Operand> operandStack, ref int space, IMetadataProvider metadata)
		{
			while (operandStack.Count != 0) {
				Operand operand = operandStack.Pop();
				int size, alignment;

				architecture.GetTypeRequirements(operand.Type, out size, out alignment);

                if (operand.Type.Type == CilElementType.ValueType)
                    size = ObjectModelUtility.ComputeTypeSize((operand.Type as ValueTypeSigType).Token, metadata, architecture);

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
			RegisterOperand eax = new RegisterOperand(resultOperand.Type, GeneralPurposeRegister.EAX);
			ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, resultOperand, eax);
		}

		/// <summary>
		/// Moves the return value to 64 bit.
		/// </summary>
		/// <param name="resultOperand">The result operand.</param>
		/// <param name="ctx">The context.</param>
		private void MoveReturnValueTo64Bit(Operand resultOperand, Context ctx)
		{
			SigType I4 = new SigType(CilElementType.I4);
			SigType U4 = new SigType(CilElementType.U4);
			MemoryOperand memoryOperand = resultOperand as MemoryOperand;

			if (memoryOperand == null) return;
			Operand opL, opH;
			LongOperandTransformationStage.SplitLongOperand(memoryOperand, out opL, out opH);
			//MemoryOperand opL = new MemoryOperand(U4, memoryOperand.Base, memoryOperand.Offset);
			//MemoryOperand opH = new MemoryOperand(I4, memoryOperand.Base, new IntPtr(memoryOperand.Offset.ToInt64() + 4));
			RegisterOperand eax = new RegisterOperand(U4, GeneralPurposeRegister.EAX);
			RegisterOperand edx = new RegisterOperand(I4, GeneralPurposeRegister.EDX);

			ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, opL, eax);
			ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, opH, edx);
		}

		/// <summary>
		/// Pushes the specified instructions.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="op">The op.</param>
		/// <param name="stackSize">Size of the stack.</param>
		private void Push(Context ctx, Operand op, int stackSize, int parameterSize)
		{
			if (op is MemoryOperand) {
                if (op.Type.Type == CilElementType.ValueType)
                {
                    for (int i = 0; i < parameterSize; i += 4)
                        ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, new MemoryOperand(op.Type, GeneralPurposeRegister.EDX, new IntPtr(stackSize + i)), new MemoryOperand(op.Type, (op as MemoryOperand).Base, new IntPtr((op as MemoryOperand).Offset.ToInt64() + i)));

                    return;
                }

				RegisterOperand rop;
				switch (op.StackType) {
					case StackTypeCode.O: goto case StackTypeCode.N;
					case StackTypeCode.Ptr: goto case StackTypeCode.N;
					case StackTypeCode.Int32: goto case StackTypeCode.N;
					case StackTypeCode.N:
						rop = new RegisterOperand(op.Type, GeneralPurposeRegister.EAX);
						break;

					case StackTypeCode.F:
						rop = new RegisterOperand(op.Type, SSE2Register.XMM0);
						break;

					case StackTypeCode.Int64: {
							SigType I4 = new SigType(CilElementType.I4);
							MemoryOperand mop = op as MemoryOperand;
							Debug.Assert(null != mop, @"I8/U8 arg is not in a memory operand.");
							RegisterOperand eax = new RegisterOperand(I4, GeneralPurposeRegister.EAX);
							Operand opL, opH;
							LongOperandTransformationStage.SplitLongOperand(mop, out opL, out opH);
							//MemoryOperand opL = new MemoryOperand(I4, mop.Base, mop.Offset);
							//MemoryOperand opH = new MemoryOperand(I4, mop.Base, new IntPtr(mop.Offset.ToInt64() + 4));

							ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, eax, opL);
							ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, new MemoryOperand(op.Type, GeneralPurposeRegister.EDX, new IntPtr(stackSize)), eax);
							ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, eax, opH);
							ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, new MemoryOperand(op.Type, GeneralPurposeRegister.EDX, new IntPtr(stackSize + 4)), eax);
						}
						return;

					default:
						throw new NotSupportedException();
				}

				ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, rop, op);
				op = rop;
			}
			else if (op is ConstantOperand && op.StackType == StackTypeCode.Int64) {
				Operand opL, opH;
				SigType I4 = new SigType(CilElementType.I4);
				RegisterOperand eax = new RegisterOperand(I4, GeneralPurposeRegister.EAX);
				LongOperandTransformationStage.SplitLongOperand(op, out opL, out opH);

				ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, eax, opL);
				ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, new MemoryOperand(I4, GeneralPurposeRegister.EDX, new IntPtr(stackSize)), eax);
				ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, eax, opH);
				ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, new MemoryOperand(I4, GeneralPurposeRegister.EDX, new IntPtr(stackSize + 4)), eax);

				return;
			}

			ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, new MemoryOperand(op.Type, GeneralPurposeRegister.EDX, new IntPtr(stackSize)), op);
		}

		/// <summary>
		/// Calculates the stack size for parameters.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <returns></returns>
		private int CalculateStackSizeForParameters(Context ctx, IMetadataProvider metadata)
		{
            return CalculateStackSizeForParameters(ctx.Operands, ctx.InvokeTarget.Signature.HasThis, metadata);
		}

		/// <summary>
		/// Calculates the stack size for parameters.
		/// </summary>
		/// <param name="operands"></param>
		/// <param name="hasThis"></param>
		/// <returns></returns>
		private int CalculateStackSizeForParameters(IEnumerable<Operand> operands, bool hasThis, IMetadataProvider metadata)
		{
			int result = (hasThis ? 0 : 0);
			int size, alignment;

			foreach (Operand op in operands) {
				this.architecture.GetTypeRequirements(op.Type, out size, out alignment);

                if (op.Type.Type == CilElementType.ValueType)
                {
                    size = ObjectModelUtility.ComputeTypeSize((op.Type as Runtime.Metadata.Signatures.ValueTypeSigType).Token, metadata, architecture);
                }

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
			this.architecture.GetTypeRequirements(operand.Type, out size, out alignment);

			// FIXME: Do not issue a move, if the operand is already the destination register
			if (4 == size || 2 == size || 1 == size) {
				ctx.SetInstruction(CPUx86.Instruction.MovInstruction, new RegisterOperand(operand.Type, GeneralPurposeRegister.EAX), operand);
				return;
			}
			else if (8 == size && (operand.Type.Type == CilElementType.R4 || operand.Type.Type == CilElementType.R8)) {
				ctx.SetInstruction(CPUx86.Instruction.MovInstruction, new RegisterOperand(operand.Type, SSE2Register.XMM0), operand);
				return;
			}
			else if (8 == size && (operand.Type.Type == CilElementType.I8 || operand.Type.Type == CilElementType.U8)) {
                SigType HighType = (operand.Type.Type == CilElementType.I8) ? new SigType(CilElementType.I4) : new SigType(CilElementType.U4);
				SigType U4 = new SigType(CilElementType.U4);

				Operand opL, opH;
				LongOperandTransformationStage.SplitLongOperand(operand, out opL, out opH);

				// Like Win32: EDX:EAX
				ctx.SetInstruction(CPUx86.Instruction.MovInstruction, new RegisterOperand(U4, GeneralPurposeRegister.EAX), opL);
				ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, new RegisterOperand(HighType, GeneralPurposeRegister.EDX), opH);

				return;
			}
			else {
				throw new NotSupportedException();
			}
		}

		void ICallingConvention.GetStackRequirements(StackOperand stackOperand, out int size, out int alignment)
		{
			// Special treatment for some stack types
			// FIXME: Handle the size and alignment requirements of value types
			this.architecture.GetTypeRequirements(stackOperand.Type, out size, out alignment);
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
				 * The first 4 bytes hold the method token, so that the GC can
				 * retrieve the method GC map and that we can do smart stack traces.
				 * 
				 * The second 4 bytes are used to hold the start of the method,
				 * so that we can embed floating point constants in our PIC.
				 * 
				 */
				return -8;
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

		#endregion // ICallingConvention Members
	}
}
