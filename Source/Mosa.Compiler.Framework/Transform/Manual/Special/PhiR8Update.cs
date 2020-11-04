// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.Special
{
	public sealed class PhiR8Update : BaseTransformation
	{
		public PhiR8Update() : base(IRInstruction.Phi32)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			return context.OperandCount != context.Block.PreviousBlocks.Count;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			transformContext.UpdatePHI(context);
		}
	}
}
