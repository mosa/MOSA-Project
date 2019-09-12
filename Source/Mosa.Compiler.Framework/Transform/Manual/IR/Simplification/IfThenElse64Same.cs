// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transform.Manual.IR.Simplification
{
	public sealed class IfThenElse64Same : BaseTransformation
	{
		public IfThenElse64Same() : base(IRInstruction.IfThenElse64)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			return AreSame(context.Operand2, context.Operand3);
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			context.SetInstruction(IRInstruction.Move64, context.Result, context.Operand1);
		}
	}
}
