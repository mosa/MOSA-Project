// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.Transform.Auto.ConstantFolding
{
	/// <summary>
	/// Compare32x64LessThanZero
	/// </summary>
	public sealed class Compare32x64LessThanZero : BaseTransformation
	{
		public Compare32x64LessThanZero() : base(IRInstruction.Compare32x64)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (context.ConditionCode != ConditionCode.UnsignedGreater)
				return false;

			if (!context.Operand1.IsResolvedConstant)
				return false;

			if (context.Operand1.ConstantUnsigned64 != 0)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var c1 = transformContext.CreateConstant(0);

			context.SetInstruction(IRInstruction.Move64, result, c1);
		}
	}
}
