// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.ConstantMove
{
	/// <summary>
	/// MulR4
	/// </summary>
	public sealed class MulR4 : BaseTransformation
	{
		public MulR4() : base(IRInstruction.MulR4, TransformationType.Auto | TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			if (!IsResolvedConstant(context.Operand1))
				return false;

			if (IsResolvedConstant(context.Operand2))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var result = context.Result;

			var t1 = context.Operand1;
			var t2 = context.Operand2;

			context.SetInstruction(IRInstruction.MulR4, result, t2, t1);
		}
	}
}
