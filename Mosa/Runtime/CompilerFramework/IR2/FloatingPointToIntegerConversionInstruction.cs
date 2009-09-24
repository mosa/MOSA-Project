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
		/// Returns a string representation of the instruction.
		/// </summary>
		/// <returns>
		/// A string representation of the instruction in intermediate form.
		/// </returns>
		public override string ToString(ref InstructionData instruction)
		{
			return String.Format(@"IR.fptoi {0}, {1} ; {0} = (integer){1}", instruction.Operand1, instruction.Operand2);
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
