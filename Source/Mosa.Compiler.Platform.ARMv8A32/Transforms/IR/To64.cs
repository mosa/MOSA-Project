// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.ARMv8A32.Transforms.IR
{
	/// <summary>
	/// To64
	/// </summary>
	public sealed class To64 : BaseTransform
	{
		public To64() : base(IRInstruction.To64, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			transform.SplitLongOperand(context.Result, out var resultLow, out var resultHigh);

			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			operand1 = ARMv8A32TransformHelper.MoveConstantToRegisterOrImmediate(transform, context, operand1);
			operand2 = ARMv8A32TransformHelper.MoveConstantToRegisterOrImmediate(transform, context, operand2);

			context.SetInstruction(ARMv8A32.Mov, resultLow, operand1);
			context.AppendInstruction(ARMv8A32.Mov, resultHigh, operand2);
		}
	}
}
