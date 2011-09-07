/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Operands;

namespace Mosa.Platform.x86.Constraints
{
	/// <summary>
	/// 
	/// </summary>
	public class MoveConstraint : IRegisterConstraint
	{
		/// <summary>
		/// Determines if this is a valid operand of the instruction.
		/// </summary>
		/// <param name="opIdx">The operand index to check.</param>
		/// <param name="op">The operand in use.</param>
		/// <returns>True if the used operand is valid or false, if it is not valid.</returns>
		public bool IsValidOperand(int opIdx, Operand op)
		{
			if (!(op.StackType == StackTypeCode.Int32 || op.StackType == StackTypeCode.Int64 || op.StackType == StackTypeCode.N
				|| op.StackType == StackTypeCode.Ptr))
				return false;

			if (opIdx == 0)
				return (op is MemoryOperand || op is RegisterOperand);
			if (opIdx == 1)
				return (op is MemoryOperand || op is RegisterOperand || op is ConstantOperand);

			return false;
		}

		/// <summary>
		/// Determines if this is a valid result operand of the instruction.
		/// </summary>
		/// <param name="resIdx">The result operand index to check.</param>
		/// <param name="op">The operand in use.</param>
		/// <returns>True if the used operand is valid or false, if it is not valid.</returns>
		public bool IsValidResult(int resIdx, Operand op)
		{
			if (!(op.StackType == StackTypeCode.Int32 || op.StackType == StackTypeCode.Int64 || op.StackType == StackTypeCode.N
				|| op.StackType == StackTypeCode.Ptr))
				return false;

			return (op is RegisterOperand || op is MemoryOperand);
		}

		/// <summary>
		/// Returns an array of registers, that are valid for the specified operand of the instruction.
		/// </summary>
		/// <param name="opIdx">The operand index to check.</param>
		public Register[] GetRegistersForOperand(int opIdx)
		{
			Register[] valid = { 
				x86.GeneralPurposeRegister.EAX, 
				x86.GeneralPurposeRegister.EBX, 
				x86.GeneralPurposeRegister.ECX, 
				x86.GeneralPurposeRegister.EDX,
				x86.GeneralPurposeRegister.EBP, 
				x86.GeneralPurposeRegister.EDI,
				x86.GeneralPurposeRegister.ESI, 
				x86.GeneralPurposeRegister.ESP 
			};

			return valid;
		}

		/// <summary>
		/// Returns an array of registers, that are valid for the specified result operand of the instruction.
		/// </summary>
		/// <param name="resIdx">The result operand index to check.</param>
		public Register[] GetRegistersForResult(int resIdx)
		{
			return GetRegistersForOperand(0);
		}

		/// <summary>
		/// Retrieves an array of registers used by this instruction. This function is
		/// required if an instruction invalidates registers, which are not operands. It allows
		/// the register allocator to perform proper register spilling, if a used register also
		/// hosts a variable.
		/// </summary>
		/// <returns>An array of registers used by the instruction.</returns>
		public Register[] GetRegistersUsed()
		{
			Register[] valid = { };
			return valid;
		}
	}
}
