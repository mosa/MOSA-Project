// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.Special
{
	public sealed class Phi32Update : BaseTransformation
	{
		public Phi32Update() : base(IRInstruction.Phi32, TransformationType.Manual | TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			return context.OperandCount != context.Block.PreviousBlocks.Count;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			TransformContext.UpdatePhi(context);
		}
	}
}
