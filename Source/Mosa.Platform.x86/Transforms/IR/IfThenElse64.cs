// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR
{
	/// <summary>
	/// IfThenElse64
	/// </summary>
	public sealed class IfThenElse64 : BaseTransform
	{
		public IfThenElse64() : base(IRInstruction.IfThenElse64, TransformType.Manual | TransformType.Transform)
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
			transform.SplitLongOperand(context.Operand3, out var op3L, out var op3H);

			var v1 = transform.AllocateVirtualRegister32();

			context.SetInstruction(X86.Or32, v1, op1L, op1H);
			context.AppendInstruction(X86.Cmp32, null, v1, transform.Constant32_0);
			context.AppendInstruction(X86.CMov32, ConditionCode.NotEqual, resultLow, resultLow, op2L);     // true
			context.AppendInstruction(X86.CMov32, ConditionCode.NotEqual, resultHigh, resultHigh, op2H);   // true
			context.AppendInstruction(X86.CMov32, ConditionCode.Equal, resultLow, resultLow, op3L);        // false
			context.AppendInstruction(X86.CMov32, ConditionCode.Equal, resultHigh, resultHigh, op3H);      // false
		}
	}
}
