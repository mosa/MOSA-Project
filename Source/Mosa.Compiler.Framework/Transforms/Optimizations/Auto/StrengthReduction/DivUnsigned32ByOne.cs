// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.StrengthReduction
{
	/// <summary>
	/// DivUnsigned32ByOne
	/// </summary>
	public sealed class DivUnsigned32ByOne : BaseTransform
	{
		public DivUnsigned32ByOne() : base(IRInstruction.DivUnsigned32, TransformType.Auto | TransformType.Optimization)
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

			context.SetInstruction(IRInstruction.Move32, result, t1);
		}
	}
}
