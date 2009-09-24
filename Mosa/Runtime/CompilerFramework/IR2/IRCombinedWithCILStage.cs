/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using Mosa.Runtime.CompilerFramework;

namespace Mosa.Runtime.CompilerFramework.IR2
{
	/// <summary>
	/// Visitor interface for instructions of the intermediate representation.
	/// </summary>
	public class IRCombinedWithCILStage : CIL.CILStage, IIRVisitor, IVisitor
	{

		#region Methods

		/// <summary>
		/// Visitation function for <see cref="AddressOfInstruction"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void AddressOfInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="ArithmeticShiftRightInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void ArithmeticShiftRightInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="BranchInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void BranchInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CallInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void CallInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="EpilogueInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void EpilogueInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="FloatingPointCompareInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void FloatingPointCompareInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="FloatingPointToIntegerConversionInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void FloatingPointToIntegerConversionInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="IntegerCompareInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void IntegerCompareInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="IntegerToFloatingPointConversionInstruction"/> instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void IntegerToFloatingPointConversionInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="JmpInstruction"/> instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void JmpInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="LiteralInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void LiteralInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="LoadInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void LoadInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="LogicalAndInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void LogicalAndInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="LogicalOrInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void LogicalOrInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="LogicalXorInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void LogicalXorInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="LogicalNotInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void LogicalNotInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="MoveInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void MoveInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="PhiInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void PhiInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="PopInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void PopInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="PrologueInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void PrologueInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="PushInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void PushInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="ReturnInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void ReturnInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="ShiftLeftInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void ShiftLeftInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="ShiftRightInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void ShiftRightInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="SignExtendedMoveInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void SignExtendedMoveInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="StoreInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void StoreInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="UDivInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void UDivInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="URemInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void URemInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="ZeroExtendedMoveInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public virtual void ZeroExtendedMoveInstruction(Context context) { }

		#endregion // Methods

	}
}
