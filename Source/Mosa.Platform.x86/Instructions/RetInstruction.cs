/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Platform;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class RetInstruction : X86Instruction, IOpcodeRegisterUsage
	{

		#region Methods

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(Context context, MachineCodeEmitter emitter)
		{
			emitter.WriteByte(0xC3);
		}

		#endregion // Methods

		#region IOpcodeRegisterUsage

		/// <summary>
		/// Gets the available result registers.
		/// </summary>
		/// <returns>Returns list of available (primary) registers for result </returns>
		Register[] IOpcodeRegisterUsage.GetAvailableResultRegisters()
		{
			return null;
		}

		/// <summary>
		/// Gets the available result registers.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>Returns list of available (primary) registers for result, given opcode address mode</returns>
		Register[] IOpcodeRegisterUsage.GetAvailableResultRegisters(OperandAddressMode addressMode)
		{
			return null;
		}

		/// <summary>
		/// Gets the available operand1 registers.
		/// </summary>
		/// <returns>
		/// Returns list of available (primary) registers for operand 1
		/// </returns>
		Register[] IOpcodeRegisterUsage.GetAvailableOperand1Registers()
		{
			return null;
		}

		/// <summary>
		/// Gets the available operand1 registers.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>
		/// Returns list of available (primary) registers for operand 1, given opcode address mode
		/// </returns>
		Register[] IOpcodeRegisterUsage.GetAvailableOperand1Registers(OperandAddressMode addressMode)
		{
			return null;
		}

		/// <summary>
		/// Gets the modified registers.
		/// </summary>
		/// <param name="register">The register.</param>
		/// <returns>
		/// Returns modified register(s), given the result register. It may *not* return the result register.
		/// </returns>
		Register[] IOpcodeRegisterUsage.GetModifiedRegisters(Register register)
		{
			return null;
		}

		/// <summary>
		/// Determines whether the specified results and operands are valid.
		/// </summary>
		/// <param name="result">The results.</param>
		/// <param name="resultType">Type of the result.</param>
		/// <param name="operand1">The operand1.</param>
		/// <param name="operand1AddressMode">The operand1 address mode.</param>
		/// <param name="operand2">The operand2.</param>
		/// <param name="operand2AddressMode">The operand2 address mode.</param>
		/// <returns>
		///   <c>true</c> if the specified results1 is valid; otherwise, <c>false</c>.
		/// </returns>
		bool IOpcodeRegisterUsage.IsValid(Register result, OperandAddressMode resultAddressMode, Register operand1, OperandAddressMode operand1AddressMode, Register operand2, OperandAddressMode operand2AddressMode)
		{
			return (result == null && operand1 == null && operand2 == null);
		}

		/// <summary>
		/// Gets the unspecified source registers. An operand is unspecified when it is not included in the context 
		/// operand list (for whatever reason). This method assists with opcode/register analysis that need to know
		/// this detail.
		/// </summary>
		/// <param name="register">The register.</param>
		/// <returns>
		/// Returns unspecified operand register(s), given result register.
		/// </returns>
		Register[] IOpcodeRegisterUsage.GetUnspecifiedSourceRegisters(Register register)
		{
			return null;
		}

		#endregion
	}
}
