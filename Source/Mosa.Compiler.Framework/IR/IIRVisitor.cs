/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */


namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// Visitor interface for instructions of the intermediate representation.
	/// </summary>
	public interface IIRVisitor : IVisitor
	{
		/// <summary>
		/// Visitation function for AddressOfInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void AddressOfInstruction(Context context);

		/// <summary>
		/// Visitation function for ArithmeticShiftRightInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void ArithmeticShiftRightInstruction(Context context);

		/// <summary>
		/// Visitation function for CallInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CallInstruction(Context context);

		/// <summary>
		/// Visitation function for EpilogueInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void EpilogueInstruction(Context context);

		/// <summary>
		/// Visitation function for FloatingPointCompareInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void FloatingPointCompareInstruction(Context context);

		/// <summary>
		/// Visitation function for FloatingPointToIntegerConversionInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void FloatingPointToIntegerConversionInstruction(Context context);

		/// <summary>
		/// Visitation function for IntegerCompareInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IntegerCompareBranchInstruction(Context context);

		/// <summary>
		/// Visitation function for IntegerCompareInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IntegerCompareInstruction(Context context);

		/// <summary>
		/// Visitation function for IntegerToFloatingPointConversionInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IntegerToFloatingPointConversionInstruction(Context context);

		/// <summary>
		/// Visitation function for JmpInstruction"/> instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void JmpInstruction(Context context);

		/// <summary>
		/// Visitation function for LoadInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void LoadInstruction(Context context);

		/// <summary>
		/// Visitation function for LogicalAndInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void LogicalAndInstruction(Context context);

		/// <summary>
		/// Visitation function for LogicalOrInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void LogicalOrInstruction(Context context);

		/// <summary>
		/// Visitation function for LogicalXorInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void LogicalXorInstruction(Context context);

		/// <summary>
		/// Visitation function for LogicalNotInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void LogicalNotInstruction(Context context);

		/// <summary>
		/// Visitation function for MoveInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void MoveInstruction(Context context);

		/// <summary>
		/// Visitation function for PhiInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void PhiInstruction(Context context);

		/// <summary>
		/// Visitation function for PrologueInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void PrologueInstruction(Context context);

		/// <summary>
		/// Visitation function for ReturnInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void ReturnInstruction(Context context);

		/// <summary>
		/// Visitation function for ShiftLeftInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void ShiftLeftInstruction(Context context);

		/// <summary>
		/// Visitation function for ShiftRightInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void ShiftRightInstruction(Context context);

		/// <summary>
		/// Visitation function for SignExtendedMoveInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void SignExtendedMoveInstruction(Context context);

		/// <summary>
		/// Visitation function for StoreInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void StoreInstruction(Context context);

		/// <summary>
		/// Visitation function for ZeroExtendedMoveInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void ZeroExtendedMoveInstruction(Context context);

		/// <summary>
		/// Visitation function for NopInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void NopInstruction(Context context);

		/// <summary>
		/// Visitation function for AddSInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void AddSInstruction(Context context);

		/// <summary>
		/// Visitation function for AddUInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void AddUInstruction(Context context);

		/// <summary>
		/// Visitation function for AddFInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void AddFInstruction(Context context);

		/// <summary>
		/// Visitation function for DivFInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void DivFInstruction(Context context);

		/// <summary>
		/// Visitation function for DivSInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void DivSInstruction(Context context);

		/// <summary>
		/// Visitation function for DivUInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void DivUInstruction(Context context);

		/// <summary>
		/// Visitation function for MulSInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void MulSInstruction(Context context);

		/// <summary>
		/// Visitation function for MulFInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void MulFInstruction(Context context);

		/// <summary>
		/// Visitation function for MulUInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void MulUInstruction(Context context);

		/// <summary>
		/// Visitation function for SubFInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void SubFInstruction(Context context);

		/// <summary>
		/// Visitation function for SubSInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void SubSInstruction(Context context);

		/// <summary>
		/// Visitation function for SubUInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void SubUInstruction(Context context);

		/// <summary>
		/// Visitation function for RemFInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void RemFInstruction(Context context);

		/// <summary>
		/// Visitation function for RemSInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void RemSInstruction(Context context);

		/// <summary>
		/// Visitation function for RemUInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void RemUInstruction(Context context);

		/// <summary>
		/// Visitation function for SwitchInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void SwitchInstruction(Context context);

		/// <summary>
		/// Visitation function for BreakInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void BreakInstruction(Context context);

		/// <summary>
		/// Visitation function for ThrowInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void ThrowInstruction(Context context);

		/// <summary>
		/// Visitation function for ExceptionPrologueInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void ExceptionPrologueInstruction(Context context);
	}
}
