/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;

namespace Mosa.Runtime.CompilerFramework.IR
{
	/// <summary>
	/// Visitor interface for instructions of the intermediate representation.
	/// </summary>
	public interface IIRVisitor : IVisitor
	{
		/// <summary>
		/// Visitation function for <see cref="AddressOfInstruction"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void AddressOfInstruction(Context context);

		/// <summary>
		/// Visitation function for <see cref="ArithmeticShiftRightInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void ArithmeticShiftRightInstruction(Context context);

		/// <summary>
		/// Visitation function for <see cref="BranchInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void BranchInstruction(Context context);

		/// <summary>
		/// Visitation function for <see cref="CallInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CallInstruction(Context context);

		/// <summary>
		/// Visitation function for <see cref="EpilogueInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void EpilogueInstruction(Context context);

		/// <summary>
		/// Visitation function for <see cref="FloatingPointCompareInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void FloatingPointCompareInstruction(Context context);

		/// <summary>
		/// Visitation function for <see cref="FloatingPointToIntegerConversionInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void FloatingPointToIntegerConversionInstruction(Context context);

		/// <summary>
		/// Visitation function for <see cref="IntegerCompareInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IntegerCompareInstruction(Context context);

		/// <summary>
		/// Visitation function for <see cref="IntegerToFloatingPointConversionInstruction"/> instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IntegerToFloatingPointConversionInstruction(Context context);

		/// <summary>
		/// Visitation function for <see cref="JmpInstruction"/> instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void JmpInstruction(Context context);

		/// <summary>
		/// Visitation function for <see cref="LiteralInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void LiteralInstruction(Context context);

		/// <summary>
		/// Visitation function for <see cref="LoadInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void LoadInstruction(Context context);

		/// <summary>
		/// Visitation function for <see cref="LogicalAndInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void LogicalAndInstruction(Context context);

		/// <summary>
		/// Visitation function for <see cref="LogicalOrInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void LogicalOrInstruction(Context context);

		/// <summary>
		/// Visitation function for <see cref="LogicalXorInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void LogicalXorInstruction(Context context);

		/// <summary>
		/// Visitation function for <see cref="LogicalNotInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void LogicalNotInstruction(Context context);

		/// <summary>
		/// Visitation function for <see cref="MoveInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void MoveInstruction(Context context);

		/// <summary>
		/// Visitation function for <see cref="PhiInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void PhiInstruction(Context context);

		/// <summary>
		/// Visitation function for <see cref="PopInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void PopInstruction(Context context);

		/// <summary>
		/// Visitation function for <see cref="PrologueInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void PrologueInstruction(Context context);

		/// <summary>
		/// Visitation function for <see cref="PushInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void PushInstruction(Context context);

		/// <summary>
		/// Visitation function for <see cref="ReturnInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void ReturnInstruction(Context context);

		/// <summary>
		/// Visitation function for <see cref="ShiftLeftInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void ShiftLeftInstruction(Context context);

		/// <summary>
		/// Visitation function for <see cref="ShiftRightInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void ShiftRightInstruction(Context context);

		/// <summary>
		/// Visitation function for <see cref="SignExtendedMoveInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void SignExtendedMoveInstruction(Context context);

		/// <summary>
		/// Visitation function for <see cref="StoreInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void StoreInstruction(Context context);

		/// <summary>
		/// Visitation function for <see cref="ZeroExtendedMoveInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void ZeroExtendedMoveInstruction(Context context);

		/// <summary>
		/// Visitation function for <see cref="NopInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void NopInstruction(Context context);

		/// <summary>
		/// Visitation function for <see cref="AddSInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void AddSInstruction(Context context);

		/// <summary>
		/// Visitation function for <see cref="AddUInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void AddUInstruction(Context context);

		/// <summary>
		/// Visitation function for <see cref="AddFInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void AddFInstruction(Context context);

		/// <summary>
		/// Visitation function for <see cref="DivFInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void DivFInstruction(Context context);

		/// <summary>
		/// Visitation function for <see cref="DivSInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void DivSInstruction(Context context);

		/// <summary>
		/// Visitation function for <see cref="DivUInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void DivUInstruction(Context context);

		/// <summary>
		/// Visitation function for <see cref="MulSInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void MulSInstruction(Context context);

		/// <summary>
		/// Visitation function for <see cref="MulFInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void MulFInstruction(Context context);

		/// <summary>
		/// Visitation function for <see cref="MulUInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void MulUInstruction(Context context);

		/// <summary>
		/// Visitation function for <see cref="SubFInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void SubFInstruction(Context context);

		/// <summary>
		/// Visitation function for <see cref="SubSInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void SubSInstruction(Context context);

		/// <summary>
		/// Visitation function for <see cref="SubUInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void SubUInstruction(Context context);

		/// <summary>
		/// Visitation function for <see cref="RemFInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void RemFInstruction(Context context);

		/// <summary>
		/// Visitation function for <see cref="RemSInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void RemSInstruction(Context context);

		/// <summary>
		/// Visitation function for <see cref="RemUInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void RemUInstruction(Context context);

		/// <summary>
		/// Visitation function for <see cref="SwitchInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void SwitchInstruction(Context context);

		/// <summary>
		/// Visitation function for <see cref="BreakInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void BreakInstruction(Context context);

		/// <summary>
		/// Visitation function for <see cref="ThrowInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void ThrowInstruction(Context context);
	}
}
