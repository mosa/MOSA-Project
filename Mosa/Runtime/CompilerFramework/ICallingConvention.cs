/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using Mosa.Runtime.CompilerFramework.IL;

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// This interface provides support to emit calling convention 
    /// specific code.
    /// </summary>
    public interface ICallingConvention
    {
        /// <summary>
        /// Expands the given invoke instruction to perform the method call.
        /// </summary>
        /// <param name="instruction">The invoke instruction to expand.</param>
        /// <returns>A single instruction or an array of instructions, which appropriately represent the method call.</returns>
        object Expand(InvokeInstruction instruction);

        /// <summary>
        /// Retrieves the stack requirements of a stack operand.
        /// </summary>
        /// <param name="stackOperand">The operand to calculate the stack requirements for.</param>
        /// <param name="size">Receives the size of the operand in bytes.</param>
        /// <param name="alignment">Receives the alignment requirements of the operand in bytes.</param>
        /// <remarks>
        /// A stack operand is a parameter or a local variable. This function is used to properly build stack
        /// frame offsets for either type of stack operand.
        /// </remarks>
        void GetStackRequirements(StackOperand stackOperand, out int size, out int alignment);

        /// <summary>
        /// Requests the calling convention to create an appropriate move instruction to populate the return 
        /// value of a method.
        /// </summary>
        /// <param name="operand">The operand, that's holding the return value.</param>
        /// <returns>An instruction, which represents the appropriate move.</returns>
        Instruction[] MoveReturnValue(Operand operand);

        /// <summary>
        /// Retrieves the offset of the first local variable From the stack frame start.
        /// </summary>
        int OffsetOfFirstLocal { get; }

        /// <summary>
        /// Retrieves the offset of the first parameter From the stack frame start.
        /// </summary>
        int OffsetOfFirstParameter { get; }
    }
}
