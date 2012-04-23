/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using Mosa.Compiler.Framework.Operands;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// This interface provides support to emit calling convention 
	/// specific code.
	/// </summary>
	public interface ICallingConvention
	{
		/// <summary>
		/// Expands method call instruction represented by the context to perform the method call.
		/// </summary>
		/// <param name="context">The context.</param>
		void MakeCall(Context context);

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
		/// <param name="context">The context.</param>
		/// <param name="operand">The operand, that's holding the return value.</param>
		void MoveReturnValue(Context context, Operand operand);

		/// <summary>
		/// Retrieves the offset of the first local variable from the stack frame start.
		/// </summary>
		int OffsetOfFirstLocal { get; }

		/// <summary>
		/// Retrieves the offset of the first parameter From the stack frame start.
		/// </summary>
		int OffsetOfFirstParameter { get; }
	}
}
