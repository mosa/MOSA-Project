// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.Rewrite
{
	/// <summary>
	/// CompareObjectGreaterThanZero
	/// </summary>
	public sealed class CompareObjectGreaterThanZero : BaseTransformation
	{
		public CompareObjectGreaterThanZero() : base(IRInstruction.CompareObject, TransformationType.Auto | TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			if (context.ConditionCode != ConditionCode.UnsignedGreater)
				return false;

			if (!IsZero(context.Operand2))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var result = context.Result;

			var t1 = context.Operand1;
			var t2 = context.Operand2;

			context.SetInstruction(IRInstruction.CompareObject, ConditionCode.NotEqual, result, t1, t2);
		}
	}

	/// <summary>
	/// CompareObjectGreaterThanZero_v1
	/// </summary>
	public sealed class CompareObjectGreaterThanZero_v1 : BaseTransformation
	{
		public CompareObjectGreaterThanZero_v1() : base(IRInstruction.CompareObject, TransformationType.Auto | TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			if (context.ConditionCode != ConditionCode.UnsignedLess)
				return false;

			if (!IsZero(context.Operand1))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var result = context.Result;

			var t1 = context.Operand1;
			var t2 = context.Operand2;

			context.SetInstruction(IRInstruction.CompareObject, ConditionCode.NotEqual, result, t2, t1);
		}
	}
}
