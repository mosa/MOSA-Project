// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Platform.x64;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transform;

namespace Mosa.Platform.x64.Transform.Auto.StrengthReduction
{
	/// <summary>
	/// Sub64ByZero
	/// </summary>
	public sealed class Sub64ByZero : BaseTransformation
	{
		public Sub64ByZero() : base(X64.Sub64)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand2.IsResolvedConstant)
				return false;

			if (context.Operand2.ConstantUnsigned64 != 0)
				return false;

			if (AreStatusFlagUsed(context))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1;

			context.SetInstruction(X64.Mov64, result, t1);
		}
	}
}
