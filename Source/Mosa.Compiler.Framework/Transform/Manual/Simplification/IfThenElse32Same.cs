// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.Simplification
{
	public sealed class IfThenElse32Same : BaseTransformation
	{
		public IfThenElse32Same() : base(IRInstruction.IfThenElse32)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			return AreSame(context.Operand2, context.Operand3);
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			context.SetInstruction(IRInstruction.Move32, context.Result, context.Operand1);
		}
	}
}
