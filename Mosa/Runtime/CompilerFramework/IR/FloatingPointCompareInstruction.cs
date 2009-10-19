/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.CompilerFramework.IR
{
	/// <summary>
	/// Represents a floating point comparison context.
	/// </summary>
	public sealed class FloatingPointCompareInstruction : ThreeOperandInstruction
	{

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="FloatingPointCompareInstruction"/> class.
		/// </summary>
		public FloatingPointCompareInstruction()
		{
		}

		#endregion // Construction

		#region ThreeOperandInstruction Overrides

		/// <summary>
		/// Returns a string representation of the context.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString(Context context)
		{
			string cc;
			switch ((ConditionCode)context.Other) {
				case ConditionCode.Equal: cc = @"=="; break;
				case ConditionCode.GreaterOrEqual: cc = @">="; break;
				case ConditionCode.GreaterThan: cc = @">"; break;
				case ConditionCode.LessOrEqual: cc = @"<="; break;
				case ConditionCode.LessThan: cc = @"<"; break;
				case ConditionCode.NotEqual: cc = @"!="; break;
				case ConditionCode.UnsignedGreaterOrEqual: cc = @">= (U)"; break;
				case ConditionCode.UnsignedGreaterThan: cc = @"> (U)"; break;
				case ConditionCode.UnsignedLessOrEqual: cc = @"<= (U)"; break;
				case ConditionCode.UnsignedLessThan: cc = @"< (U)"; break;
				default:
					throw new NotSupportedException();
			}
			return String.Format(@"IR.fcmp {0} = {1} {2} {3}", context.Operand1, context.Operand2, cc, context.Operand3);
		}

		/// <summary>
		/// Abstract visitor method for intermediate representation visitors.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IIRVisitor visitor, Context context)
		{
			visitor.FloatingPointCompareInstruction(context);
		}

		#endregion // ThreeOperandInstruction Overrides
	}
}
