// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transform;

namespace Mosa.Platform.x86.Transform.Auto.StrengthReduction
{
	/// <summary>
	/// Add32ByZero
	/// </summary>
	public sealed class Add32ByZero : BaseTransformation
	{
		public Add32ByZero() : base(X86.Add32, TransformationType.Auto| TransformationType.Optimization)
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

			context.SetInstruction(X86.Mov32, result, t1);
		}
	}
}
