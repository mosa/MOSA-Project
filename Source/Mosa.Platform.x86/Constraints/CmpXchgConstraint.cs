/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.CompilerFramework.Operands;

namespace Mosa.Platform.x86.Constraints
{
	/// <summary>
	/// Provides register constraints for the <see cref="Mosa.Platform.x86.CPUx86.CmpXchgInstruction"/>.
	/// </summary>
	sealed class CmpXchgConstraint : GPRConstraint
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="CmpXchgConstraint"/> class.
		/// </summary>
		public CmpXchgConstraint()
		{
		}

		#endregion // Construction

		#region GPRConstraint Overrides

		/// <summary>
		/// Determines if this is a valid operand of the instruction.
		/// </summary>
		/// <param name="opIdx">The operand index to check.</param>
		/// <param name="op">The operand in use.</param>
		/// <returns>
		/// True if the used operand is valid or false, if it is not valid.
		/// </returns>
		public override bool IsValidOperand(int opIdx, Operand op)
		{
			if (op.StackType == StackTypeCode.F || op.StackType == StackTypeCode.Int64)
				return false;

			switch (opIdx)
			{
				case 0:
					return (op is RegisterOperand || op is MemoryOperand);

				case 1:
					return (op is RegisterOperand && ((RegisterOperand)op).Register == GeneralPurposeRegister.EAX);

				case 2:
					return (op is RegisterOperand);
			}

			return false;
		}

		/// <summary>
		/// Returns an array of registers, that are valid for the specified operand of the instruction.
		/// </summary>
		/// <param name="opIdx">The operand index to check.</param>
		/// <returns></returns>
		public override Register[] GetRegistersForOperand(int opIdx)
		{
			switch (opIdx)
			{
				case 0:
					return GetGeneralPurposeRegisters();

				case 1:
					return new Register[] { GeneralPurposeRegister.EAX };

				case 2:
					return GetGeneralPurposeRegisters();
			}

			return null;
		}

		#endregion // GPRConstraint Overrides
	}
}
