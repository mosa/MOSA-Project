// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.StrengthReduction
{
	/// <summary>
	/// MulUnsigned64ByOne
	/// </summary>
	public sealed class MulUnsigned64ByOne : BaseTransformation
	{
		public MulUnsigned64ByOne() : base(IRInstruction.MulUnsigned64, TransformationType.Auto | TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			if (!context.Operand2.IsResolvedConstant)
				return false;

			if (context.Operand2.ConstantUnsigned64 != 1)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var result = context.Result;

			var t1 = context.Operand1;

			context.SetInstruction(IRInstruction.Move64, result, t1);
		}
	}

	/// <summary>
	/// MulUnsigned64ByOne_v1
	/// </summary>
	public sealed class MulUnsigned64ByOne_v1 : BaseTransformation
	{
		public MulUnsigned64ByOne_v1() : base(IRInstruction.MulUnsigned64, TransformationType.Auto | TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			if (!context.Operand1.IsResolvedConstant)
				return false;

			if (context.Operand1.ConstantUnsigned64 != 1)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var result = context.Result;

			var t1 = context.Operand2;

			context.SetInstruction(IRInstruction.Move64, result, t1);
		}
	}
}
