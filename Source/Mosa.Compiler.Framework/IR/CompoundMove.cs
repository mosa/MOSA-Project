/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Ki (kiootic) <kiootic@gmail.com>
 */

namespace Mosa.Compiler.Framework.IR
{
	public sealed class CompoundMove : TwoOperandInstruction
	{
		public CompoundMove()
		{
		}

		public override void Visit(IIRVisitor visitor, Context context)
		{
			visitor.CompoundMove(context);
		}
	}
}