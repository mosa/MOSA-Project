// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.ARMv8A32.Transforms.IR
{
	/// <summary>
	/// GetHigh32
	/// </summary>
	public sealed class GetHigh32 : BaseTransform
	{
		public GetHigh32() : base(IRInstruction.GetHigh32, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			transform.SplitLongOperand(context.Result, out var resultLow, out _);
			transform.SplitLongOperand(context.Operand1, out _, out var op1H);

			op1H = ARMv8A32TransformHelper.MoveConstantToRegisterOrImmediate(transform, context, op1H);

			context.SetInstruction(ARMv8A32.Mov, resultLow, op1H);
		}
	}
}
