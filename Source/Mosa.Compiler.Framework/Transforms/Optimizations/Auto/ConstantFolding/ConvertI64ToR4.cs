// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.ConstantFolding
{
	/// <summary>
	/// ConvertI64ToR4
	/// </summary>
	public sealed class ConvertI64ToR4 : BaseTransform
	{
		public ConvertI64ToR4() : base(IRInstruction.ConvertI64ToR4, TransformType.Auto | TransformType.Optimization)
		{
		}

		public override int Priority => 100;

		public override bool Match(Context context, TransformContext transform)
		{
			if (!IsResolvedConstant(context.Operand1))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var result = context.Result;

			var t1 = context.Operand1;

			var e1 = transform.CreateConstant(ToR4(ToSigned64(t1)));

			context.SetInstruction(IRInstruction.MoveR4, result, e1);
		}
	}
}
