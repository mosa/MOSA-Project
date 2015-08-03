// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// Intermediate representation of the unsigned division operation.
	/// </summary>
	public sealed class DivUnsigned : ThreeOperandInstruction
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DivUnsigned"/> class.
		/// </summary>
		public DivUnsigned()
		{
		}

		/// <summary>
		/// Abstract visitor method for intermediate representation visitors.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IIRVisitor visitor, Context context)
		{
			visitor.DivUnsigned(context);
		}
	}
}