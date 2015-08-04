// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.IR
{
	public sealed class CompoundStore : BaseIRInstruction
	{
		public CompoundStore()
			: base(3, 0)
		{
		}

		public override void Visit(IIRVisitor visitor, Context context)
		{
			visitor.CompoundStore(context);
		}
	}
}
