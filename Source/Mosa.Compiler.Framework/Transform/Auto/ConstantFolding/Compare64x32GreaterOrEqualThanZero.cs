// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.Transform.Auto.ConstantFolding
{
	/// <summary>
	/// Compare64x32GreaterOrEqualThanZero
	/// </summary>
	public sealed class Compare64x32GreaterOrEqualThanZero : BaseTransformation
	{
		public Compare64x32GreaterOrEqualThanZero() : base(IRInstruction.Compare64x32, TransformationType.Auto | TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (context.ConditionCode != ConditionCode.UnsignedGreaterOrEqual)
				return false;

			if (!context.Operand2.IsResolvedConstant)
				return false;

			if (context.Operand2.ConstantUnsigned64 != 0)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var c1 = transformContext.CreateConstant(1);

			context.SetInstruction(IRInstruction.Move32, result, c1);
		}
	}
}
