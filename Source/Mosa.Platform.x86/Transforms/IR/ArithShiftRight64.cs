
using System.Diagnostics;

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR
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

			var count = context.Operand2;

			var v1 = transform.AllocateVirtualRegister32();

			context.SetInstruction(X86.Sar32, v1, op1H, count);
			context.AppendInstruction(X86.Shrd32, resultLow, op1L, op1H, count);
			context.AppendInstruction(X86.Sar32, resultHigh, resultHigh, transform.Constant32_32);

			/// Optimized when shift value is a constant
			if (count.IsResolvedConstant)
			{
				if (count.ConstantUnsigned32 == 32)
				{
					context.AppendInstruction(X86.Mov32, resultHigh, v1);
				}
				else
				{
					context.AppendInstruction(X86.Mov32, resultLow, v1);
				}

				return;
			}

			var v2 = transform.AllocateVirtualRegister32();

			context.AppendInstruction(X86.Mov32, v2, count);
			context.AppendInstruction(X86.Test32, null, v2, transform.Constant32_32);
			context.AppendInstruction(X86.CMov32, ConditionCode.NotEqual, resultLow, resultLow, v1);
			context.AppendInstruction(X86.CMov32, ConditionCode.Equal, resultHigh, resultHigh, v1);
		}
	}
}
