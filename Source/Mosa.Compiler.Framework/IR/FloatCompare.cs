// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// Represents a floating point comparison context.
	/// </summary>
	public sealed class FloatCompare : ThreeOperandInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="FloatCompare"/> class.
		/// </summary>
		public FloatCompare()
		{
		}

		#endregion Construction

		#region ThreeOperandInstruction Overrides

		/// <summary>
		/// Abstract visitor method for intermediate representation visitors.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IIRVisitor visitor, Context context)
		{
			visitor.FloatCompare(context);
		}

		#endregion ThreeOperandInstruction Overrides
	}
}
