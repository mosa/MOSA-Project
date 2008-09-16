/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Text;

using Mosa.Runtime.CompilerFramework;
using IL = Mosa.Runtime.CompilerFramework.IL;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Metadata;

namespace Mosa.Platforms.x86
{
    /// <summary>
    /// Implements the CIL default calling convention for x86.
    /// </summary>
    sealed class DefaultCallingConvention : ICallingConvention
    {
        #region Static data members

        /// <summary>
        /// Holds the single instance of the default calling convention.
        /// </summary>
        public static readonly DefaultCallingConvention Instance = new DefaultCallingConvention();

        #endregion // Static data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultCallingConvention"/>.
        /// </summary>
        private DefaultCallingConvention()
        { 
        }

        #endregion // Construction

        #region ICallingConvention Members

        /// <summary>
        /// Expands the given invoke instruction to perform the method call.
        /// </summary>
        /// <param name="architecture">The architecture supplying instructions.</param>
        /// <param name="instruction">The invoke instruction to expand.</param>
        /// <returns>
        /// A single instruction or an array of instructions, which appropriately represent the method call.
        /// </returns>
        object ICallingConvention.Expand(IArchitecture architecture, IL.InvokeInstruction instruction)
        {
            /*
             * Calling convention is right-to-left, pushed on the stack. Return value in EAX for integral
             * types 4 bytes or less, XMM0 for floating point and EAX:EDX for 64-bit. If this is a method
             * of a type, the this argument is moved to ECX right before the call.
             * 
             */

            List<Instruction> instructions = new List<Instruction>();
            SigType I = new SigType(CilElementType.I);
            RegisterOperand esp = new RegisterOperand(I, GeneralPurposeRegister.ESP);
            bool moveThis = instruction.InvokeTarget.Signature.HasThis;
            int stackSize = CalculateStackSizeForParameters(instruction, moveThis);
            if (0 != stackSize)
            {
                instructions.Add(architecture.CreateInstruction(typeof(x86.Instructions.SubInstruction), esp, new ConstantOperand(I, stackSize)));
                instructions.Add(architecture.CreateInstruction(typeof(x86.Instructions.MoveInstruction), new RegisterOperand(architecture.NativeType, GeneralPurposeRegister.EDX), esp));

                Stack<Operand> ops = new Stack<Operand>(instruction.Operands.Length);
                int thisArg = 1;
                foreach (Operand op in instruction.Operands)
                {
                    if (true == moveThis && 1 == thisArg)
                    {
                        thisArg = 0;
                        continue;
                    }
                    ops.Push(op);
                }

                int space = stackSize, size, alignment;
                while (0 != ops.Count)
                {
                    Operand op = ops.Pop();
                    GetStackRequirements(op.Type, out size, out alignment);
                    space -= size;
                    Push(instructions, architecture, op, space);
                }
            }

            if (true == moveThis)
            {
                RegisterOperand ecx = new RegisterOperand(I, GeneralPurposeRegister.ECX);
                instructions.Add(architecture.CreateInstruction(typeof(Instructions.MoveInstruction), ecx, instruction.Operands[0]));
            }
            instructions.Add(architecture.CreateInstruction(typeof(x86.Instructions.CallInstruction), instruction.InvokeTarget));
            if (0 != stackSize)
            {
                instructions.Add(architecture.CreateInstruction(typeof(x86.Instructions.AddInstruction), esp, new ConstantOperand(I, stackSize)));
            }

            return instructions;
        }

        /// <summary>
        /// Pushes the specified instructions.
        /// </summary>
        /// <param name="instructions">The instructions.</param>
        /// <param name="arch">The arch.</param>
        /// <param name="op">The op.</param>
        /// <param name="stackSize">Size of the stack.</param>
        private void Push(List<Instruction> instructions, IArchitecture arch, Operand op, int stackSize)
        {
            if (op is MemoryOperand)
            {
                RegisterOperand rop;
                switch (op.StackType)
                {
                    case StackTypeCode.O: goto case StackTypeCode.N;
                    case StackTypeCode.Ptr: goto case StackTypeCode.N;
                    case StackTypeCode.Int32: goto case StackTypeCode.N;
                    case StackTypeCode.N:
                        rop = new RegisterOperand(op.Type, GeneralPurposeRegister.EAX);
                        break;

                    case StackTypeCode.F:
                        rop = new RegisterOperand(op.Type, SSE2Register.XMM0);
                        break;

                    case StackTypeCode.Int64:
                        throw new NotSupportedException(@"Calling methods with Int64 args is not supported right now.");
                        
                    default:
                        throw new NotSupportedException();
                }
                instructions.Add(arch.CreateInstruction(typeof(Mosa.Runtime.CompilerFramework.IR.MoveInstruction), rop, op));
                op = rop;
            }

            instructions.Add(arch.CreateInstruction(typeof(x86.Instructions.MoveInstruction), new MemoryOperand(op.Type, GeneralPurposeRegister.EDX, new IntPtr(stackSize)), op));
        }

        /// <summary>
        /// Calculates the stack size for parameters.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="hasThis">if set to <c>true</c> [has this].</param>
        /// <returns></returns>
        private int CalculateStackSizeForParameters(IL.InvokeInstruction instruction, bool hasThis)
        {
            int result = (true == hasThis ? -4 : 0);
            int size, alignment;
            // FIXME: This will not work for an instance method with the first arg being a fp value
            foreach (Operand op in instruction.Operands)
            {
                GetStackRequirements(op.Type, out size, out alignment);
                result += size;
            }
            return result;
        }

        /// <summary>
        /// Requests the calling convention to create an appropriate move instruction to populate the return
        /// value of a method.
        /// </summary>
        /// <param name="architecture">The architecture to emit the instruction for.</param>
        /// <param name="operand">The operand, that's holding the return value.</param>
        /// <returns>
        /// An instruction, which represents the appropriate move.
        /// </returns>
        Instruction[] ICallingConvention.MoveReturnValue(IArchitecture architecture, Operand operand)
        {
            int size, alignment;
            GetStackRequirements(operand.Type, out size, out alignment);

            // FIXME: Do not issue a move, if the operand is already the destination register
            if (4 == size)
            {
                return new Instruction[] { architecture.CreateInstruction(typeof(Instructions.MoveInstruction), new RegisterOperand(operand.Type, GeneralPurposeRegister.EAX), operand) };
            }
            else if (8 == size && (operand.Type.Type == CilElementType.R4 || operand.Type.Type == CilElementType.R8))
            {
                return new Instruction[] { architecture.CreateInstruction(typeof(Instructions.MoveInstruction), new RegisterOperand(operand.Type, SSE2Register.XMM0), operand) };
            }
            else
            {
                throw new NotSupportedException();
            }

        }

        void ICallingConvention.GetStackRequirements(StackOperand stackOperand, out int size, out int alignment)
        {
            // Special treatment for some stack types
            // FIXME: Handle the size and alignment requirements of value types
            GetStackRequirements(stackOperand.Type, out size, out alignment);
        }

        private void GetStackRequirements(SigType sigType, out int size, out int alignment)
        {
            switch (sigType.Type)
            {
                    // TODO ROOTNODE
                case CilElementType.R4:
                    size = alignment = 4;
                    break;
                case CilElementType.R8:
                    // Default alignment and size are 4
                    size = alignment = 8;
                    break;

                case CilElementType.I8: goto case CilElementType.U8;
                case CilElementType.U8:
                    size = alignment = 8;
                    break;

                case CilElementType.ValueType:
                    throw new NotSupportedException();

                default:
                    size = alignment = 4;
                    break;
            }
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
