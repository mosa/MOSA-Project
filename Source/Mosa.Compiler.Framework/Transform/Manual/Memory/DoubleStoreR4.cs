﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.Memory
{
	public sealed class DoubleStoreR4 : BaseTransformation
	{
		public DoubleStoreR4() : base(IRInstruction.StoreR4)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand2.IsResolvedConstant)
				return false;

			var next = GetNextNode(context);

			if (next == null)
				return false;

			if (next.Instruction != IRInstruction.StoreR4)
				return false;

			if (!next.Operand2.IsResolvedConstant)
				return false;

			if (next.Operand1 != context.Operand1)
				return false;

			if (next.Operand2.ConstantUnsigned64 != context.Operand2.ConstantUnsigned64)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			context.SetNop();
		}
	}
}
