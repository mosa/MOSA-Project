// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.x86.Transforms.IR
{
	/// <summary>
	/// Sub64
	/// </summary>
	public sealed class Sub64 : BaseTransform
	{
		public Sub64() : base(IRInstruction.Sub64, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			transform.SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			transform.SplitLongOperand(context.Operand1, out var op1L, out var op1H);
			transform.SplitLongOperand(context.Operand2, out var op2L, out var op2H);

			context.SetInstruction(X86.Sub32, resultLow, op1L, op2L);
			context.AppendInstruction(X86.Sbb32, resultHigh, op1H, op2H);
		}
	}
}
