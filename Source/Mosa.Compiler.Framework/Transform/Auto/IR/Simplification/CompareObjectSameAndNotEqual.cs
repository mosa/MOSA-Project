// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transform.Auto.IR.Simplification
{
	/// <summary>
	/// CompareObjectSameAndNotEqual
	/// </summary>
	public sealed class CompareObjectSameAndNotEqual : BaseTransformation
	{
		public CompareObjectSameAndNotEqual() : base(IRInstruction.CompareObject)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			var condition = context.ConditionCode;

			if (!(context.ConditionCode == ConditionCode.NotEqual || context.ConditionCode == ConditionCode.Greater || context.ConditionCode == ConditionCode.Less || context.ConditionCode == ConditionCode.UnsignedGreater || context.ConditionCode == ConditionCode.UnsignedLess))
				return false;

			if (!AreSame(context.Operand1, context.Operand2))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var e1 = transformContext.CreateConstant(To32(0));

			context.SetInstruction(IRInstruction.Move32, result, e1);
		}
	}
}
