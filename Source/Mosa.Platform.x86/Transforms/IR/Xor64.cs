// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR
{
	/// <summary>
	/// Xor64
	/// </summary>
	public sealed class Xor64 : BaseTransform
	{
		public Xor64() : base(IRInstruction.Xor64, TransformType.Manual | TransformType.Transform)
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

			context.SetInstruction(X86.Xor32, resultHigh, op1H, op2H);
			context.AppendInstruction(X86.Xor32, resultLow, op1L, op2L);
		}
	}
}
