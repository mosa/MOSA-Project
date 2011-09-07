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
	public class SwitchInstruction : BaseInstruction
	{
		public override void Visit(IIRVisitor visitor, Context context)
		{
			visitor.SwitchInstruction(context);
		}
	}
}
