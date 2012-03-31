/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using Mosa.Compiler.Framework.Operands;

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Constraints
{
	/// <summary>
	/// This interface is used to present register constraints by native
	/// instructions to the register allocator.
	/// </summary>
	public interface IRegisterConstraint
	{
		/// <summary>
		/// Determines if this is a valid operand of the instruction.
		/// </summary>
		/// <param name="opIdx">The operand index to check.</param>
		/// <param name="op">The operand in use.</param>
		/// <returns>True if the used operand is valid or false, if it is not valid.</returns>
		bool IsValidOperand(int opIdx, Operand op);

		/// <summary>
		/// Determines if this is a valid result operand of the instruction.
		/// </summary>
		/// <param name="resIdx">The result operand index to check.</param>
		/// <param name="op">The operand in use.</param>
		/// <returns>True if the used operand is valid or false, if it is not valid.</returns>
		bool IsValidResult(int resIdx, Operand op);

		/// <summary>
		/// Returns an array of registers, that are valid for the specified operand of the instruction.
		/// </summary>
		/// <param name="opIdx">The operand index to check.</param>
		Register[] GetRegistersForOperand(int opIdx);

		/// <summary>
		/// Returns an array of registers, that are valid for the specified result operand of the instruction.
		/// </summary>
		/// <param name="resIdx">The result operand index to check.</param>
		Register[] GetRegistersForResult(int resIdx);

		/// <summary>
		/// Retrieves an array of registers used by this instruction. This function is
		/// required if an instruction invalidates registers, which are not operands. It allows
		/// the register allocator to perform proper register spilling, if a used register also
		/// hosts a variable.
		/// </summary>
		/// <returns>An array of registers used by the instruction.</returns>
		Register[] GetRegistersUsed();
	}
}
