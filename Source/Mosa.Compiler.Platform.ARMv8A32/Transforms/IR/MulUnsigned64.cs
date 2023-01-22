// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.ARMv8A32.Transforms.IR
{
	/// <summary>
	/// MulUnsigned64
	/// </summary>
	public sealed class MulUnsigned64 : BaseTransform
	{
		public MulUnsigned64() : base(IRInstruction.MulUnsigned64, TransformType.Manual | TransformType.Transform)
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

			var v1 = transform.AllocateVirtualRegister32();
			var v2 = transform.AllocateVirtualRegister32();

			op1L = ARMv8A32TransformHelper.MoveConstantToRegister(transform, context, op1L);
			op1H = ARMv8A32TransformHelper.MoveConstantToRegister(transform, context, op1H);
			op2L = ARMv8A32TransformHelper.MoveConstantToRegister(transform, context, op2L);
			op2H = ARMv8A32TransformHelper.MoveConstantToRegister(transform, context, op2H);

			//umull		low, v1 <= op1l, op2l
			//mla		v2, <= op1l, op2h, v1
			//mla		high, <= op1h, op2l, v2

			context.SetInstruction2(ARMv8A32.UMull, v1, resultLow, op1L, op2L);
			context.AppendInstruction(ARMv8A32.Mla, v2, op1L, op2H, v1);
			context.AppendInstruction(ARMv8A32.Mla, resultHigh, op1H, op2L, v2);
		}
	}
}
