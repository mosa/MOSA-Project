// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.StrengthReduction
{
	/// <summary>
	/// MulUnsigned32ByZero
	/// </summary>
	public sealed class MulUnsigned32ByZero : BaseTransform
	{
		public MulUnsigned32ByZero() : base(IRInstruction.MulUnsigned32, TransformType.Auto | TransformType.Optimization)
		{
		}

		public override int Priority => 80;

		public override bool Match(Context context, TransformContext transform)
		{
			if (!context.Operand2.IsResolvedConstant)
				return false;

			if (context.Operand2.ConstantUnsigned64 != 0)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var result = context.Result;

			var e1 = transform.CreateConstant(To32(0));

			context.SetInstruction(IRInstruction.Move32, result, e1);
		}
	}

	/// <summary>
	/// MulUnsigned32ByZero_v1
	/// </summary>
	public sealed class MulUnsigned32ByZero_v1 : BaseTransform
	{
		public MulUnsigned32ByZero_v1() : base(IRInstruction.MulUnsigned32, TransformType.Auto | TransformType.Optimization)
		{
		}

		public override int Priority => 80;

		public override bool Match(Context context, TransformContext transform)
		{
			if (!context.Operand1.IsResolvedConstant)
				return false;

			if (context.Operand1.ConstantUnsigned64 != 0)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var result = context.Result;

			var e1 = transform.CreateConstant(To32(0));

			context.SetInstruction(IRInstruction.Move32, result, e1);
		}
	}
}
