// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.Transform.Auto.StrengthReduction
{
	/// <summary>
	/// DivSigned64ByPowerOfTwo
	/// </summary>
	public sealed class DivSigned64ByPowerOfTwo : BaseTransformation
	{
		public DivSigned64ByPowerOfTwo() : base(IRInstruction.DivSigned64, TransformationType.Auto| TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!IsResolvedConstant(context.Operand2))
				return false;

			if (!IsPowerOfTwo64(context.Operand2))
				return false;

			if (IsZero(context.Operand2))
				return false;

			if (IsOne(context.Operand2))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1;
			var t2 = context.Operand2;

			var e1 = transformContext.CreateConstant(GetPowerOfTwo(To32(t2)));

			context.SetInstruction(IRInstruction.ShiftRight64, result, t1, e1);
		}
	}
}
