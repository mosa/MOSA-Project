// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.x86.Transforms.IR
{
	/// <summary>
	/// LoadParam64
	/// </summary>
	public sealed class LoadParam64 : BaseTransform
	{
		public LoadParam64() : base(IRInstruction.LoadParam64, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			transform.SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			transform.SplitLongOperand(context.Operand1, out var lowOffset, out var highOffset);

			context.SetInstruction(X86.MovLoad32, resultLow, transform.StackFrame, lowOffset);
			context.AppendInstruction(X86.MovLoad32, resultHigh, transform.StackFrame, highOffset);
		}
	}
}
