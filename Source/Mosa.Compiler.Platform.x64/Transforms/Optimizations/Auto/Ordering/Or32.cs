// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.x64.Transforms.Optimizations.Auto.Ordering
{
	/// <summary>
	/// Or32
	/// </summary>
	public sealed class Or32 : BaseTransform
	{
		public Or32() : base(X64.Or32, TransformType.Auto | TransformType.Optimization)
		{
		}

		public override int Priority => 10;

		public override bool Match(Context context, TransformContext transform)
		{
			if (!IsVirtualRegister(context.Operand1))
				return false;

			if (!IsVirtualRegister(context.Operand2))
				return false;

			if (!IsGreater(UseCount(context.Operand1), UseCount(context.Operand2)))
				return false;

			if (IsResultAndOperand1Same(context))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var result = context.Result;

			var t1 = context.Operand1;
			var t2 = context.Operand2;

			context.SetInstruction(X64.Or32, result, t2, t1);
		}
	}
}
