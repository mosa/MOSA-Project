// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transform.Manual.IR.Simplification
{
	public sealed class SubCarryOut32CarryNotUsed : BaseTransformation
	{
		public SubCarryOut32CarryNotUsed() : base(IRInstruction.SubCarryOut32)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			return (context.Result2.Uses.Count == 0);
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			context.SetInstruction(IRInstruction.Sub32, context.Result, context.Operand1, context.Operand2);
		}
	}
}
