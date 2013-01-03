/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */


namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// </summary>
	public sealed class BlockEnd : BaseIRInstruction
	{
		
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="Phi" /> class.
		/// </summary>
        public BlockEnd()
			: base(0, 0)
		{
		}

		#endregion // Construction

		#region Methods

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IIRVisitor visitor, Context context)
		{
			// None
		}

		#endregion // Methods
	}
}
