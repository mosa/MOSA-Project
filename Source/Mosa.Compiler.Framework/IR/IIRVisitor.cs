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
	/// Visitor interface for s of the intermediate representation.
	/// </summary>
	public interface IIRVisitor : IVisitor
	{
		/// <summary>
		/// Visitation function for AddressOf.
		/// </summary>
		/// <param name="context">The context.</param>
		void AddressOf(Context context);

		/// <summary>
		/// Visitation function for ArithmeticShiftRight.
		/// </summary>
		/// <param name="context">The context.</param>
		void ArithmeticShiftRight(Context context);

		/// <summary>
		/// Visitation function for Call.
		/// </summary>
		/// <param name="context">The context.</param>
		void Call(Context context);

		/// <summary>
		/// Visitation function for Epilogue.
		/// </summary>
		/// <param name="context">The context.</param>
		void Epilogue(Context context);

		/// <summary>
		/// Visitation function for FloatingPointCompare.
		/// </summary>
		/// <param name="context">The context.</param>
		void FloatCompare(Context context);

		/// <summary>
		/// Visitation function for FloatingPointToIntegerConversion.
		/// </summary>
		/// <param name="context">The context.</param>
		void FloatToIntegerConversion(Context context);

		/// <summary>
		/// Visitation function for IntegerCompare.
		/// </summary>
		/// <param name="context">The context.</param>
		void IntegerCompareBranch(Context context);

		/// <summary>
		/// Visitation function for IntegerCompare.
		/// </summary>
		/// <param name="context">The context.</param>
		void IntegerCompare(Context context);

		/// <summary>
		/// Visitation function for IntegerToFloatingPointConversion.
		/// </summary>
		/// <param name="context">The context.</param>
		void IntegerToFloatConversion(Context context);

		/// <summary>
		/// Visitation function for Jmp> .
		/// </summary>
		/// <param name="context">The context.</param>
		void Jmp(Context context);

		/// <summary>
		/// Visitation function for Load.
		/// </summary>
		/// <param name="context">The context.</param>
		void Load(Context context);

		/// <summary>
		/// Visitation function for Load Zero Extended.
		/// </summary>
		/// <param name="context">The context.</param>
		void LoadZeroExtended(Context context);

		/// <summary>
		/// Visitation function for Load Sign Extended.
		/// </summary>
		/// <param name="context">The context.</param>
		void LoadSignExtended(Context context);

		/// <summary>
		/// Visitation function for LogicalAnd.
		/// </summary>
		/// <param name="context">The context.</param>
		void LogicalAnd(Context context);

		/// <summary>
		/// Visitation function for LogicalOr.
		/// </summary>
		/// <param name="context">The context.</param>
		void LogicalOr(Context context);

		/// <summary>
		/// Visitation function for LogicalXor.
		/// </summary>
		/// <param name="context">The context.</param>
		void LogicalXor(Context context);

		/// <summary>
		/// Visitation function for LogicalNot.
		/// </summary>
		/// <param name="context">The context.</param>
		void LogicalNot(Context context);

		/// <summary>
		/// Visitation function for Move.
		/// </summary>
		/// <param name="context">The context.</param>
		void Move(Context context);

		/// <summary>
		/// Visitation function for Phi.
		/// </summary>
		/// <param name="context">The context.</param>
		void Phi(Context context);

		/// <summary>
		/// Visitation function for Prologue.
		/// </summary>
		/// <param name="context">The context.</param>
		void Prologue(Context context);

		/// <summary>
		/// Visitation function for Return.
		/// </summary>
		/// <param name="context">The context.</param>
		void Return(Context context);

		/// <summary>
		/// Visitation function for ShiftLeft.
		/// </summary>
		/// <param name="context">The context.</param>
		void ShiftLeft(Context context);

		/// <summary>
		/// Visitation function for ShiftRight.
		/// </summary>
		/// <param name="context">The context.</param>
		void ShiftRight(Context context);

		/// <summary>
		/// Visitation function for SignExtendedMove.
		/// </summary>
		/// <param name="context">The context.</param>
		void SignExtendedMove(Context context);

		/// <summary>
		/// Visitation function for Store.
		/// </summary>
		/// <param name="context">The context.</param>
		void Store(Context context);

		/// <summary>
		/// Visitation function for ZeroExtendedMove.
		/// </summary>
		/// <param name="context">The context.</param>
		void ZeroExtendedMove(Context context);

		/// <summary>
		/// Visitation function for Nop.
		/// </summary>
		/// <param name="context">The context.</param>
		void Nop(Context context);

		/// <summary>
		/// Visitation function for AddSigned.
		/// </summary>
		/// <param name="context">The context.</param>
		void AddSigned(Context context);

		/// <summary>
		/// Visitation function for AddUnsigned.
		/// </summary>
		/// <param name="context">The context.</param>
		void AddUnsigned(Context context);

		/// <summary>
		/// Visitation function for AddF.
		/// </summary>
		/// <param name="context">The context.</param>
		void AddFloat(Context context);

		/// <summary>
		/// Visitation function for DivF.
		/// </summary>
		/// <param name="context">The context.</param>
		void DivFloat(Context context);

		/// <summary>
		/// Visitation function for DivS.
		/// </summary>
		/// <param name="context">The context.</param>
		void DivSigned(Context context);

		/// <summary>
		/// Visitation function for DivU.
		/// </summary>
		/// <param name="context">The context.</param>
		void DivUnsigned(Context context);

		/// <summary>
		/// Visitation function for MulS.
		/// </summary>
		/// <param name="context">The context.</param>
		void MulSigned(Context context);

		/// <summary>
		/// Visitation function for MulF.
		/// </summary>
		/// <param name="context">The context.</param>
		void MulFloat(Context context);

		/// <summary>
		/// Visitation function for MulU.
		/// </summary>
		/// <param name="context">The context.</param>
		void MulUnsigned(Context context);

		/// <summary>
		/// Visitation function for SubF.
		/// </summary>
		/// <param name="context">The context.</param>
		void SubFloat(Context context);

		/// <summary>
		/// Visitation function for SubS.
		/// </summary>
		/// <param name="context">The context.</param>
		void SubSigned(Context context);

		/// <summary>
		/// Visitation function for SubU.
		/// </summary>
		/// <param name="context">The context.</param>
		void SubUnsigned(Context context);

		/// <summary>
		/// Visitation function for RemF.
		/// </summary>
		/// <param name="context">The context.</param>
		void RemFloat(Context context);

		/// <summary>
		/// Visitation function for RemS.
		/// </summary>
		/// <param name="context">The context.</param>
		void RemSigned(Context context);

		/// <summary>
		/// Visitation function for RemU.
		/// </summary>
		/// <param name="context">The context.</param>
		void RemUnsigned(Context context);

		/// <summary>
		/// Visitation function for Switch.
		/// </summary>
		/// <param name="context">The context.</param>
		void Switch(Context context);

		/// <summary>
		/// Visitation function for Break.
		/// </summary>
		/// <param name="context">The context.</param>
		void Break(Context context);

		/// <summary>
		/// Visitation function for Throw.
		/// </summary>
		/// <param name="context">The context.</param>
		void Throw(Context context);

		/// <summary>
		/// Visitation function for ExceptionPrologue.
		/// </summary>
		/// <param name="context">The context.</param>
		void ExceptionPrologue(Context context);

		/// <summary>
		/// Visitation function for intrinsic the method call.
		/// </summary>
		/// <param name="context">The context.</param>
		void IntrinsicMethodCall(Context context);
	}
}
