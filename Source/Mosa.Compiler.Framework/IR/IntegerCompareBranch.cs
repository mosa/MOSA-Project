/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 */

namespace Mosa.Compiler.Framework.IR
{
	public sealed class IntegerCompareBranch : BaseIRInstruction
	{
		#region Construction
	
		public IntegerCompareBranch() : base(2)
		{
		}

		#endregion // Construction
		
		#region Methods

		/// <summary>
		/// Abstract visitor method for intermediate representation visitors.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IIRVisitor visitor, Context context)
		{
			visitor.IntegerCompareBranch(context);
		}

		#endregion
	}
}
