// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.StrengthReduction
{
	/// <summary>
	/// And32Max
	/// </summary>
	public sealed class And32Max : BaseTransform
	{
		public And32Max() : base(IRInstruction.And32, TransformType.Auto | TransformType.Optimization)
		{
		}

		public override int Priority => 80;

		public override bool Match(Context context, TransformContext transform)
		{
			if (!context.Operand2.IsResolvedConstant)
				return false;

			if (context.Operand2.ConstantUnsigned64 != 0xFFFFFFFF)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var result = context.Result;

			var t1 = context.Operand1;

			context.SetInstruction(IRInstruction.Move32, result, t1);
		}
	}

	/// <summary>
	/// And32Max_v1
	/// </summary>
	public sealed class And32Max_v1 : BaseTransform
	{
		public And32Max_v1() : base(IRInstruction.And32, TransformType.Auto | TransformType.Optimization)
		{
		}

		public override int Priority => 80;

		public override bool Match(Context context, TransformContext transform)
		{
			if (!context.Operand1.IsResolvedConstant)
				return false;

			if (context.Operand1.ConstantUnsigned64 != 0xFFFFFFFF)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var result = context.Result;

			var t1 = context.Operand2;

			context.SetInstruction(IRInstruction.Move32, result, t1);
		}
	}
}