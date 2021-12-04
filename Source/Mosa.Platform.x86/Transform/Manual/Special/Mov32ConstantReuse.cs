﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transform;
using Mosa.Platform.Intel;

namespace Mosa.Platform.x86.Transform.Manual.Special
{
	public sealed class Mov32ConstantReuse : BaseTransformation
	{
		public Mov32ConstantReuse() : base(X86.Mov32)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand1.IsResolvedConstant)
				return false;

			if (context.Operand1.ConstantUnsigned32 == 0)
				return false;

			if (!context.Result.IsCPURegister)
				return false;

			if (context.Result.Register != GeneralPurposeRegister.ESP)
				return false;

			var previous = GetPreviousNode(context);

			if (previous == null || previous.Instruction != X86.Mov32)
				return false;

			if (previous.Result.Register != GeneralPurposeRegister.ESP)
				return false;

			if (!previous.Operand1.IsResolvedConstant)
				return false;

			if (!previous.Result.IsCPURegister)
				return false;

			if (context.Operand1.ConstantUnsigned32 != previous.Operand1.ConstantUnsigned32)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var previous = GetPreviousNode(context);

			context.Operand1 = previous.Result;
		}
	}
}
