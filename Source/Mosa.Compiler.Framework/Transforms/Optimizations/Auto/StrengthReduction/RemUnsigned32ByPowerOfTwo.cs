// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.StrengthReduction
{
	/// <summary>
	/// RemUnsigned32ByPowerOfTwo
	/// </summary>
	public sealed class RemUnsigned32ByPowerOfTwo : BaseTransform
	{
		public RemUnsigned32ByPowerOfTwo() : base(IRInstruction.RemUnsigned32, TransformType.Auto | TransformType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			if (!IsResolvedConstant(context.Operand2))
				return false;

			if (!IsPowerOfTwo32(context.Operand2))
				return false;

			if (IsZero(context.Operand2))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var result = context.Result;

			var t1 = context.Operand1;
			var t2 = context.Operand2;

			var e1 = transform.CreateConstant(Sub32(ShiftLeft32(1, And32(GetPowerOfTwo(To32(t2)), Sub32(32, 1))), 1));

			context.SetInstruction(IRInstruction.And32, result, t1, e1);
		}
	}
}
