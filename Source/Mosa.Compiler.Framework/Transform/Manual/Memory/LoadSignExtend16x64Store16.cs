﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.Memory
{
	public sealed class LoadSignExtend16x64Store16 : BaseTransformation
	{
		public LoadSignExtend16x64Store16() : base(IRInstruction.LoadSignExtend16x64)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand2.IsResolvedConstant)
				return false;

			var previous = GetPreviousNodeUntil(context, IRInstruction.Store16, out bool immediatePrevious);

			if (previous == null)
				return false;

			if (!immediatePrevious && !IsSSAForm(previous.Operand3))
				return false;

			if (!previous.Operand2.IsResolvedConstant)
				return false;

			if (previous.Operand1 != context.Operand1)
				return false;

			if (previous.Operand2.ConstantUnsigned64 != context.Operand2.ConstantUnsigned64)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var previous = GetPreviousNodeUntil(context, IRInstruction.Store16, out _);

			context.SetInstruction(IRInstruction.SignExtend16x64, context.Result, previous.Operand3);
		}
	}
}
