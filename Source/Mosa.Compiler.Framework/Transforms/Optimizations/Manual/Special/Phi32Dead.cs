﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Special
{
	public sealed class Phi32Dead : BaseTransform
	{
		public Phi32Dead() : base(IRInstruction.Phi32, TransformType.Manual | TransformType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			if (context.ResultCount == 0)
				return true;

			var result = context.Result;
			var node = context.Node;

			foreach (var use in result.Uses)
			{
				if (use != node)
					return false;
			}

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			context.SetNop();
		}
	}
}
