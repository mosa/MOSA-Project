/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Operands;

namespace Mosa.Compiler.Framework.Platform
{

	/// <summary>
	/// This interface is used to present opcode constraints to the register allocator.
	/// </summary>
	public interface IOpcodeRegisterUsage
	{
		
		/// <summary>
		/// Gets the available result registers.
		/// </summary>
		/// <returns>Returns list of available (primary) registers for result </returns>
		Register[] GetAvailableResultRegisters();

		/// <summary>
		/// Gets the available result registers.
		/// </summary>
		/// <param name="addressMode">The address mode.</param>
		/// <returns>
		/// Returns list of available (primary) registers for result, given opcode address mode
		/// </returns>
		Register[] GetAvailableResultRegisters(OperandAddressMode addressMode);

		/// <summary>
		/// Gets the available operand1 registers.
		/// </summary>
		/// <returns>
		/// Returns list of available (primary) registers for operand 1
		/// </returns>
		Register[] GetAvailableOperand1Registers();

		/// <summary>
		/// Gets the available operand1 registers.
		/// </summary>
		/// <param name="addressMode">The address mode.</param>
		/// <returns>
		/// Returns list of available (primary) registers for operand 1, given opcode address mode
		/// </returns>
		Register[] GetAvailableOperand1Registers(OperandAddressMode addressMode);

		/// <summary>
		/// Gets the modified registers.
		/// </summary>
		/// <param name="register">The register.</param>
		/// <returns>
		/// Returns modified register(s), given the result register. It may *not* contain the result register.
		/// </returns>
		Register[] GetModifiedRegisters(Register register);

		/// <summary>
		/// Determines whether the specified results and operands are valid.
		/// </summary>
		/// <param name="result">The results.</param>
		/// <param name="resultAddressMode">The result address mode.</param>
		/// <param name="operand1">The operand1.</param>
		/// <param name="operand1AddressMode">The operand1 address mode.</param>
		/// <param name="operand2">The operand2.</param>
		/// <param name="operand2AddressMode">The operand2 address mode.</param>
		/// <returns>
		///   <c>true</c> if the specified results1 is valid; otherwise, <c>false</c>.
		/// </returns>
		bool IsValid(Register result, OperandAddressMode resultAddressMode, Register operand1, OperandAddressMode operand1AddressMode, Register operand2, OperandAddressMode operand2AddressMode);

		/// <summary>
		/// Gets the unspecified source registers. An operand is unspecified when it is not included in the context 
		/// operand list (for whatever reason). This method assists with opcode/register analysis that need to know
		/// this detail.
		/// </summary>
		/// <param name="register">The register.</param>
		/// <returns>
		/// Returns unspecified operand register(s), given result register.
		/// </returns>
		Register[] GetUnspecifiedSourceRegisters(Register register);

	}
}
