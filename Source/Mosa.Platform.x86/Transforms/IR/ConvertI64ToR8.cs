// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR
{
	/// <summary>
	/// ConvertI64ToR8
	/// </summary>
	public sealed class ConvertI64ToR8 : BaseTransform
	{
		public ConvertI64ToR8() : base(IRInstruction.ConvertI64ToR8, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			transform.SplitLongOperand(context.Result, out var op1Low, out _);

			context.SetInstruction(X86.Cvtsi2sd32, context.Result, op1Low);
		}
	}
}
