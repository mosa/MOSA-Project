// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.x86.Transforms.IR
{
	/// <summary>
	/// ZeroExtend32x64
	/// </summary>
	public sealed class ZeroExtend32x64 : BaseTransform
	{
		public ZeroExtend32x64() : base(IRInstruction.ZeroExtend32x64, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			transform.SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			transform.SplitLongOperand(context.Operand1, out var op1L, out _);

			context.SetInstruction(X86.Mov32, resultLow, op1L);
			context.AppendInstruction(X86.Mov32, resultHigh, transform.Constant32_0);
		}
	}
}
