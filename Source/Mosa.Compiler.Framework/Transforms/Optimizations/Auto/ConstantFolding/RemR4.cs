// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.ConstantFolding
{
	/// <summary>
	/// RemR4
	/// </summary>
	public sealed class RemR4 : BaseTransform
	{
		public RemR4() : base(IRInstruction.RemR4, TransformType.Auto | TransformType.Optimization)
		{
		}

		public override int Priority => 100;

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

			var e1 = transform.CreateConstant(RemR4(ToR4(t1), ToR4(t2)));

			context.SetInstruction(IRInstruction.MoveR4, result, e1);
		}
	}
}
