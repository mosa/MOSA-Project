// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.StrengthReduction
{
	/// <summary>
	/// DivSigned64ByZero
	/// </summary>
	public sealed class DivSigned64ByZero : BaseTransformation
	{
		public DivSigned64ByZero() : base(IRInstruction.DivSigned64, TransformationType.Auto | TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			if (!context.Operand1.IsResolvedConstant)
				return false;

			if (context.Operand1.ConstantUnsigned64 != 0)
				return false;

			if (!IsResolvedConstant(context.Operand2))
				return false;

			if (IsZero(context.Operand2))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var result = context.Result;

			var e1 = transform.CreateConstant(To64(0));

			context.SetInstruction(IRInstruction.Move64, result, e1);
		}
	}
}
