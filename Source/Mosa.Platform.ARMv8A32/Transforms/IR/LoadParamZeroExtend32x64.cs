// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.ARMv8A32.Transforms.IR
{
	/// <summary>
	/// LoadParamZeroExtend32x64
	/// </summary>
	public sealed class LoadParamZeroExtend32x64 : BaseTransform
	{
		public LoadParamZeroExtend32x64() : base(IRInstruction.LoadParamZeroExtend32x64, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			transform.SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			transform.SplitLongOperand(context.Operand1, out var lowOffset, out _);

			ARMv8A32TransformHelper.TransformLoad(transform, context, ARMv8A32.Ldr32, resultLow, transform.StackFrame, lowOffset);
			context.AppendInstruction(ARMv8A32.Mov, resultHigh, transform.Constant32_0);
		}
	}
}
