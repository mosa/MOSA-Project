/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Diagnostics;

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.CompilerFramework.Operands;

namespace Mosa.Platform.X86.Constraints
{
	/// <summary>
	/// A generic register constraint implementation for x86 two operand instructions
	/// using only general purpose registers.
	/// </summary>
	public class GPRConstraint : IRegisterConstraint
	{
		#region IRegisterConstraint Members

		/// <summary>
		/// Determines if this is a valid operand of the instruction.
		/// </summary>
		/// <param name="opIdx">The operand index to check.</param>
		/// <param name="op">The operand in use.</param>
		/// <returns>
		/// True if the used operand is valid or false, if it is not valid.
		/// </returns>
		public virtual bool IsValidOperand(int opIdx, Operand op)
		{
			/*
			 * We support memory, register and constant operands as the source
			 * of an instruction. Specific instruction, which don't support this
			 * scenario must create a derived class and specialize it.
			 * 
			 */
			Debug.Assert(opIdx == 0, @"More than one operand is not supported.");
			if (opIdx > 0)
				throw new ArgumentOutOfRangeException(@"opIdx", opIdx, @"More than one operand is not supported.");

			if (op.StackType == StackTypeCode.F || op.StackType == StackTypeCode.Int64)
				return false;

			return (op is MemoryOperand || op is RegisterOperand || op is ConstantOperand);
		}

		/// <summary>
		/// Determines if this is a valid result operand of the instruction.
		/// </summary>
		/// <param name="resIdx">The result operand index to check.</param>
		/// <param name="op">The operand in use.</param>
		/// <returns>
		/// True if the used operand is valid or false, if it is not valid.
		/// </returns>
		public virtual bool IsValidResult(int resIdx, Operand op)
		{
			/*
			 * The scheme employed by this class forces register allocators to defer
			 * loads. This allows us to remove temporary loads. However results of
			 * these instructions go into a register. This facilitates easy use of
			 * mul r/m32 and similar instructions, where we don't force a memory operand 
			 * into a register.
			 */
			Debug.Assert(resIdx == 0, @"More than one result operand is not supported.");
			if (resIdx > 0)
				throw new ArgumentOutOfRangeException(@"resIdx", resIdx, @"More than one result operand is not supported.");

			if (op.StackType == StackTypeCode.F || op.StackType == StackTypeCode.Int64)
				return false;

			return (op is RegisterOperand);
		}

		/// <summary>
		/// Returns an array of registers, that are valid for the specified operand of the instruction.
		/// </summary>
		/// <param name="opIdx">The operand index to check.</param>
		/// <returns></returns>
		public virtual Register[] GetRegistersForOperand(int opIdx)
		{
			Debug.Assert(opIdx == 0, @"More than one operand is not supported.");
			if (opIdx > 0)
				throw new ArgumentOutOfRangeException(@"opIdx", opIdx, @"More than one operand is not supported.");

			return GetGeneralPurposeRegisters();
		}

		/// <summary>
		/// Returns an array of registers, that are valid for the specified result operand of the instruction.
		/// </summary>
		/// <param name="resIdx">The result operand index to check.</param>
		/// <returns></returns>
		public virtual Register[] GetRegistersForResult(int resIdx)
		{
			Debug.Assert(resIdx == 0, @"More than one result operand is not supported.");
			if (resIdx > 0)
				throw new ArgumentOutOfRangeException(@"resIdx", resIdx, @"More than one result operand is not supported.");

			return GetGeneralPurposeRegisters();
		}

		/// <summary>
		/// Retrieves an array of registers used by this instruction. This function is
		/// required if an instruction invalidates registers, which are not operands. It allows
		/// the register allocator to perform proper register spilling, if a used register also
		/// hosts a variable.
		/// </summary>
		/// <returns>
		/// Null to indicate no additional registers used by the instruction.
		/// </returns>
		public virtual Register[] GetRegistersUsed()
		{
			return null;
		}

		#endregion // IRegisterConstraint Members

		#region Internals

		/// <summary>
		/// Gets the general purpose registers.
		/// </summary>
		/// <returns>An array holding all general purpose registers.</returns>
		protected Register[] GetGeneralPurposeRegisters()
		{
			return new Register[] {
				X86.GeneralPurposeRegister.ESI,
				X86.GeneralPurposeRegister.EDI,
				X86.GeneralPurposeRegister.EBX, 
				X86.GeneralPurposeRegister.EAX, 
				X86.GeneralPurposeRegister.ECX,
				X86.GeneralPurposeRegister.EDX,
			};
		}

		#endregion // Internals
	}
}
