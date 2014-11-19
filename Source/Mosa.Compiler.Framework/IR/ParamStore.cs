/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// Stores a value to the stack.
	/// </summary>
	/// <remarks>
	/// The store instruction stores the value passed the stack.
	/// </remarks>
	public sealed class ParamStore : BaseIRInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Store"/>.
		/// </summary>
		public ParamStore() :
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
			//visitor.ParamStore(context);
		}

		#endregion Methods
	}
}