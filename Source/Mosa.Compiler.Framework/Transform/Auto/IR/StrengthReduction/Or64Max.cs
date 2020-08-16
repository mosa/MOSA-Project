// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transform.Auto.IR.StrengthReduction
{
	/// <summary>
	/// Or64Max
	/// </summary>
	public sealed class Or64Max : BaseTransformation
	{
		public Or64Max() : base(IRInstruction.Or64)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand2.IsResolvedConstant)
				return false;

			if (context.Operand2.ConstantUnsigned64 != 0xFFFFFFFFFFFFFFFF)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var c1 = transformContext.CreateConstant(0xFFFFFFFFFFFFFFFF);

			context.SetInstruction(IRInstruction.Move64, result, c1);
		}
	}

	/// <summary>
	/// Or64Maxv1
	/// </summary>
	public sealed class Or64Maxv1 : BaseTransformation
	{
		public Or64Maxv1() : base(IRInstruction.Or64)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand1.IsResolvedConstant)
				return false;

			if (context.Operand1.ConstantUnsigned64 != 0xFFFFFFFFFFFFFFFF)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var c1 = transformContext.CreateConstant(0xFFFFFFFFFFFFFFFF);

			context.SetInstruction(IRInstruction.Move64, result, c1);
		}
	}
}
