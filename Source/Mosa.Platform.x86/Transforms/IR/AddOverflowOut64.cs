
using System.Diagnostics;

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR
{
	/// <summary>
	/// AddOverflowOut64
	/// </summary>
	public sealed class AddOverflowOut64 : BaseTransformation
	{
		public AddOverflowOut64() : base(IRInstruction.AddOverflowOut64, TransformationType.Manual | TransformationType.Transform)
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
			var result2 = context.Result2;

			var v1 = transform.AllocateVirtualRegister32();

			context.SetInstruction(X86.Add32, resultLow, op1L, op2L);
			context.AppendInstruction(X86.Adc32, resultHigh, op1H, op2H);
			context.AppendInstruction(X86.Setcc, ConditionCode.Overflow, v1);
			context.AppendInstruction(X86.Movzx8To32, result2, v1);
		}
	}
}
