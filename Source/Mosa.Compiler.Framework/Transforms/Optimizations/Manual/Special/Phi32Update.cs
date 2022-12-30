﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Special
{
	public sealed class Phi32Update : BaseTransformation
	{
		public Phi32Update() : base(IRInstruction.Phi32, TransformationType.Manual | TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return context.OperandCount != context.Block.PreviousBlocks.Count;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			TransformContext.UpdatePhi(context);
		}
	}
}
