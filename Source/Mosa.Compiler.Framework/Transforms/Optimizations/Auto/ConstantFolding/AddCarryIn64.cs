// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.ConstantFolding
{
	/// <summary>
	/// AddCarryIn64
	/// </summary>
	public sealed class AddCarryIn64 : BaseTransform
	{
		public AddCarryIn64() : base(IRInstruction.AddCarryIn64, TransformType.Auto | TransformType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			if (!IsResolvedConstant(context.Operand1))
				return false;

			if (!IsResolvedConstant(context.Operand2))
				return false;

			if (!IsResolvedConstant(context.Operand3))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var result = context.Result;

			var t1 = context.Operand1;
			var t2 = context.Operand2;
			var t3 = context.Operand3;

			var e1 = transform.CreateConstant(Add64(Add64(To64(t1), To64(t2)), BoolTo64(To64(t3))));

			context.SetInstruction(IRInstruction.Move64, result, e1);
		}
	}
}
