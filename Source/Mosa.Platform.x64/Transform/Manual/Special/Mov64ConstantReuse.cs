// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transform;

namespace Mosa.Platform.x64.Transform.Manual.Special
{
	public sealed class Mov64ConstantReuse : BaseTransformation
	{
		public Mov64ConstantReuse() : base(X64.Mov64, TransformationType.Manual | TransformationType.Optimization)
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

		public override void Transform(Context context, TransformContext transformContext)
		{
			var previous = GetPreviousNode(context);

			context.Operand1 = previous.Result;
		}
	}
}
