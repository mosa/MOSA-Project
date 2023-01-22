// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.ARMv8A32.Transforms.IR
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

			op1L = ARMv8A32TransformHelper.MoveConstantToRegisterOrImmediate(transform, context, op1L);

			context.SetInstruction(ARMv8A32.Mov, resultLow, op1L);
			context.AppendInstruction(ARMv8A32.Mov, resultHigh, transform.Constant32_0);
		}
	}
}
