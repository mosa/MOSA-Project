// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.ConstantFolding
{
	/// <summary>
	/// ConvertI32ToR4
	/// </summary>
	public sealed class ConvertI32ToR4 : BaseTransformation
	{
		public ConvertI32ToR4() : base(IRInstruction.ConvertI32ToR4, TransformationType.Auto | TransformationType.Optimization)
		{
		}

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

			var e1 = transform.CreateConstant(ToR4(ToSigned32(t1)));

			context.SetInstruction(IRInstruction.MoveR4, result, e1);
		}
	}
}
