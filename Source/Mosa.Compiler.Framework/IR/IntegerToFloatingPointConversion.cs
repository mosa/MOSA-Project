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
	/// Intermediate representation of an integral to floating point conversion operation.
	/// </summary>
	public sealed class IntegerToFloatingPointConversion : TwoOperandInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="IntegerToFloatingPointConversion"/> class.
		/// </summary>
		public IntegerToFloatingPointConversion()
		{
		}

		#endregion // Construction

		#region TwoOperandInstruction Overrides

		#endregion // TwoOperandInstruction Overrides
	}
}
