// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.StrengthReduction
{
	/// <summary>
	/// DivUnsigned64ByPowerOfTwo
	/// </summary>
	public sealed class DivUnsigned64ByPowerOfTwo : BaseTransform
	{
		public DivUnsigned64ByPowerOfTwo() : base(IRInstruction.DivUnsigned64, TransformType.Auto | TransformType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			if (!IsResolvedConstant(context.Operand2))
				return false;

			if (!IsPowerOfTwo64(context.Operand2))
				return false;

			if (IsZero(context.Operand2))
				return false;

			if (IsOne(context.Operand2))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var result = context.Result;

			var t1 = context.Operand1;
			var t2 = context.Operand2;

			var e1 = transform.CreateConstant(GetPowerOfTwo(To32(t2)));

			context.SetInstruction(IRInstruction.ShiftRight64, result, t1, e1);
		}
	}
}
