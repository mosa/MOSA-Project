// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transform.Auto.IR.StrengthReduction
{
	/// <summary>
	/// MulSigned64ByPowerOfTwo
	/// </summary>
	public sealed class MulSigned64ByPowerOfTwo : BaseTransformation
	{
		public MulSigned64ByPowerOfTwo() : base(IRInstruction.MulSigned64)
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

			context.SetInstruction(IRInstruction.ShiftLeft64, result, t1, e1);
		}
	}

	/// <summary>
	/// MulSigned64ByPowerOfTwov1
	/// </summary>
	public sealed class MulSigned64ByPowerOfTwov1 : BaseTransformation
	{
		public MulSigned64ByPowerOfTwov1() : base(IRInstruction.MulSigned64)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!IsResolvedConstant(context.Operand1))
				return false;

			if (!IsPowerOfTwo64(context.Operand1))
				return false;

			if (IsZero(context.Operand1))
				return false;

			if (IsOne(context.Operand1))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1;
			var t2 = context.Operand2;

			var e1 = transformContext.CreateConstant(GetPowerOfTwo(To32(t1)));

			context.SetInstruction(IRInstruction.ShiftLeft64, result, t2, e1);
		}
	}
}
