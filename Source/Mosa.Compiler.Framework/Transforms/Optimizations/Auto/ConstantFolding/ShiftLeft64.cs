// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.ConstantFolding
{
	/// <summary>
	/// ShiftLeft64
	/// </summary>
	public sealed class ShiftLeft64 : BaseTransformation
	{
		public ShiftLeft64() : base(IRInstruction.ShiftLeft64, TransformationType.Auto | TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			if (!IsResolvedConstant(context.Operand1))
				return false;

			if (!IsResolvedConstant(context.Operand2))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var result = context.Result;

			var t1 = context.Operand1;
			var t2 = context.Operand2;

			var e1 = transform.CreateConstant(ShiftLeft64(To64(t1), ToSigned64(t2)));

			context.SetInstruction(IRInstruction.Move64, result, e1);
		}
	}
}
