/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

namespace Mosa.Compiler.Framework.IR
{
	public class Switch : BaseIRInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="Switch" /> class.
		/// </summary>
		public Switch()
			: base(0, 0)
		{
		}

		#endregion Construction

		public override void Visit(IIRVisitor visitor, Context context)
		{
			visitor.Switch(context);
		}
	}
}