// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// Stores a value to a memory pointer.
	/// </summary>
	/// <remarks>
	/// The store instruction stores the value in the given memory pointer with offset.
	/// The first operand is the memory base.
	/// The second operand is the memory base offset.
	/// The third is the value to store.
	/// </remarks>
	public sealed class Store : BaseIRInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Store"/>.
		/// </summary>
		public Store() :
			base(3, 0)
		{
		}

		#endregion Construction

		#region Methods

		/// <summary>
		/// Visitor method for intermediate representation visitors.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IIRVisitor visitor, Context context)
		{
			visitor.Store(context);
		}

		#endregion Methods
	}
}
