// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// Intermediate representation of an integral to floating point conversion operation.
	/// </summary>
	public sealed class IntegerToFloatConversion : TwoOperandInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="IntegerToFloatConversion"/> class.
		/// </summary>
		public IntegerToFloatConversion()
		{
		}

		#endregion Construction

		#region TwoOperandInstruction Overrides

		/// <summary>
		/// Abstract visitor method for intermediate representation visitors.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IIRVisitor visitor, Context context)
		{
			visitor.IntegerToFloatConversion(context);
		}

		#endregion TwoOperandInstruction Overrides
	}
}