﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.Optimizations.Manual.Special
{
	public sealed class Mov64ConstantReuse : BaseTransform
	{
		public Mov64ConstantReuse() : base(X64.Mov64, TransformType.Manual | TransformType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			if (!context.Operand1.IsResolvedConstant)
				return false;

			if (context.Operand1.ConstantUnsigned32 == 0)
				return false;

			if (!context.Result.IsCPURegister)
				return false;

			if (context.Result.Register != CPURegister.RSP)
				return false;

			var previous = GetPreviousNode(context);

			if (previous == null || previous.Instruction != X64.Mov64)
				return false;

			if (previous.Result.Register != CPURegister.RSP)
				return false;

			if (!previous.Operand1.IsResolvedConstant)
				return false;

			if (!previous.Result.IsCPURegister)
				return false;

			if (context.Operand1.ConstantUnsigned64 != previous.Operand1.ConstantUnsigned64)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var previous = GetPreviousNode(context);

			context.Operand1 = previous.Result;
		}
	}
}