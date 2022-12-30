﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Simplification
{
	public sealed class SubCarryOut64CarryNotUsed : BaseTransformation
	{
		public SubCarryOut64CarryNotUsed() : base(IRInstruction.SubCarryOut64, TransformationType.Manual | TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return context.Result2.Uses.Count == 0;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			context.SetInstruction(IRInstruction.Sub64, context.Result, context.Operand1, context.Operand2);
		}
	}
}
