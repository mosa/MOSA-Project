// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.ARMv8A32.Transforms.IR
{
	/// <summary>
	/// LoadParamSignExtend32x64
	/// </summary>
	public sealed class LoadParamSignExtend32x64 : BaseTransform
	{
		public LoadParamSignExtend32x64() : base(IRInstruction.LoadParamSignExtend32x64, TransformType.Manual | TransformType.Transform)
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

			ARMv8A32TransformHelper.TransformLoad(transform, context, ARMv8A32.Ldr32, resultLow, transform.StackFrame, lowOffset);
			context.AppendInstruction(ARMv8A32.Asr, resultHigh, resultLow, transform.Constant32_31);
		}
	}
}
