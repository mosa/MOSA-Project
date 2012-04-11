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
	/// This interface is used to present opcode constraints to the register allocator.
	/// </summary>
	public interface IOpcodeRegisterUsage
	{
		/// <summary>
		/// Gets the allowable registers for result.
		/// </summary>
		/// <returns>Returns list of allowable (primary) registers for result </returns>
		Register[] GetAllowableResultRegisters();

		/// <summary>
		/// Gets the allowable result registers.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>Returns list of allowable (primary) registers for result, given opcode operand type</returns>
		Register[] GetAllowableResultRegisters(OpcodeOperandType type);

		/// <summary>
		/// Gets the used registers.
		/// </summary>
		/// <param name="register">The register.</param>
		/// <returns>
		/// Returns modified register(s), given (primary) result register
		/// </returns>
		Register[] GetUsedRegisters(Register register);

		/// <summary>
		/// Determines whether the specified results1 is valid.
		/// </summary>
		/// <param name="results1">The results1.</param>
		/// <param name="resultType1">The result type1.</param>
		/// <param name="results2">The results2.</param>
		/// <param name="resultType2">The result type2.</param>
		/// <param name="operand1">The operand1.</param>
		/// <param name="operandType1">The operand type1.</param>
		/// <param name="operand2">The operand2.</param>
		/// <param name="operandType2">The operand type2.</param>
		/// <param name="operand3">The operand3.</param>
		/// <param name="operandType3">The operand type3.</param>
		/// <returns>
		///   <c>true</c> if the specified results1 is valid; otherwise, <c>false</c>.
		/// </returns>
		bool IsValid(Register results1, OpcodeOperandType resultType1, Register operand1, OpcodeOperandType operandType1, Register operand2, OpcodeOperandType operandType2, Register operand3, OpcodeOperandType operandType3);

		/// <summary>
		/// Gets the unspecified registers.
		/// </summary>
		/// <param name="register">The register.</param>
		/// <returns>
		/// Returns unspecified register(s), given (primary) result register
		/// </returns>
		Register[] GetUnspecifiedRegisters(Register register);

	}
}
