// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.StrengthReduction
{
	/// <summary>
	/// DivUnsigned64ByOne
	/// </summary>
	public sealed class DivUnsigned64ByOne : BaseTransform
	{
		public DivUnsigned64ByOne() : base(IRInstruction.DivUnsigned64, TransformType.Auto | TransformType.Optimization)
		{
		}

		public override int Priority => 80;

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
}
