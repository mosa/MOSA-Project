using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Mosa.Runtime.CompilerFramework.IR2
{
	/// <summary>
	/// Intermediate representation of a floating point to integral conversion operation.
	/// </summary>
	public sealed class FloatingPointToIntegerConversionInstruction : TwoOperandInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="FloatingPointToIntegerConversionInstruction"/> class.
		/// </summary>
		public FloatingPointToIntegerConversionInstruction()
		{
		}

		#endregion // Construction

		#region TwoOperandInstruction Overrides

		/// <summary>
		/// Returns a string representation of the context.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString(Context context)
		{
			return String.Format(@"IR.fptoi {0}, {1} ; {0} = (integer){1}", context.Operand1, context.Operand2);
		}

		/// <summary>
		/// Abstract visitor method for intermediate representation visitors.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IIRVisitor visitor, Context context)
		{
			visitor.FloatingPointToIntegerConversionInstruction(context);
		}

		#endregion // TwoOperandInstruction Overrides
	}
}
