// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.IR.LowerTo32
{
	public sealed class Truncate64x32 : BaseTransformation
	{
		public Truncate64x32() : base(IRInstruction.Truncate64x32)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			return transformContext.LowerTo32;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			transformContext.SetGetLow64(context, context.Result, context.Operand1);
		}
	}
}
