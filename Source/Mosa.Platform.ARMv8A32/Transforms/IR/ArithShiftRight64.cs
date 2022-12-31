// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.ARMv8A32.Transforms.IR
{
	/// <summary>
	/// ArithShiftRight64
	/// </summary>
	public sealed class ArithShiftRight64 : BaseTransform
	{
		public ArithShiftRight64() : base(IRInstruction.ArithShiftRight64, TransformType.Manual | TransformType.Transform)
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

			op1L = ARMv8A32TransformHelper.MoveConstantToRegister(transform, context, op1L);
			op1H = ARMv8A32TransformHelper.MoveConstantToRegister(transform, context, op1H);
			op2L = ARMv8A32TransformHelper.MoveConstantToRegister(transform, context, op2L);

			var v1 = transform.AllocateVirtualRegister32();
			var v2 = transform.AllocateVirtualRegister32();

			context.SetInstruction(ARMv8A32.Asr, resultHigh, op1H, op2L);
			context.AppendInstruction(ARMv8A32.Sub, StatusRegister.Set, v1, op2L, transform.Constant32_0);
			context.AppendInstruction(ARMv8A32.Lsr, resultLow, op1L, op2L);
			context.AppendInstruction(ARMv8A32.Rsb, v2, op2L, transform.Constant32_0);
			context.AppendInstruction(ARMv8A32.Asr, ConditionCode.Positive, resultHigh, op1H, transform.Constant32_31);
			context.AppendInstruction(ARMv8A32.OrrRegShift, resultLow, resultLow, op1H, v2, transform.Constant32_0 /* LSL */);
			context.AppendInstruction(ARMv8A32.Asr, ConditionCode.Positive, resultLow, op1H, v1);
		}
	}
}
