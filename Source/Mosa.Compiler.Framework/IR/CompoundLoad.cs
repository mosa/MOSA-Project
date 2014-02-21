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
	public sealed class CompoundLoad : ThreeOperandInstruction
	{
		public CompoundLoad()
		{
		}

		public override void Visit(IIRVisitor visitor, Context context)
		{
			visitor.CompoundLoad(context);
		}
	}
}