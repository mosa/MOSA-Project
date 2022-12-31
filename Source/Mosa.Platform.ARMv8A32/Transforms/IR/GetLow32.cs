// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.ARMv8A32.Transforms.IR
{
	/// <summary>
	/// GetLow32
	/// </summary>
	public sealed class GetLow32 : BaseTransform
	{
		public GetLow32() : base(IRInstruction.GetLow32, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			transform.SplitLongOperand(context.Result, out var resultLow, out _);
			transform.SplitLongOperand(context.Operand1, out var op1L, out _);

			op1L = ARMv8A32TransformHelper.MoveConstantToRegisterOrImmediate(transform, context, op1L);

			context.SetInstruction(ARMv8A32.Mov, resultLow, op1L);
		}
	}
}
