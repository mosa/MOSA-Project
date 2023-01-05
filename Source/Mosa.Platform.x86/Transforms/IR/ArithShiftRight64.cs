// Copyright (c) MOSA Project. Licensed under the New BSD License.

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
			return !(context.Operand2.IsResolvedConstant && (context.Operand2.IsInteger32 & 32 == 0));
		}

		public override void Transform(Context context, TransformContext transform)
		{
			transform.SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			transform.SplitLongOperand(context.Operand1, out var op1L, out var op1H);

			var count = context.Operand2;

			var v1_eax = transform.AllocateVirtualRegister32();
			var v2_edx = transform.AllocateVirtualRegister32();
			var v3_ecx = transform.AllocateVirtualRegister32();

			var newBlocks = transform.CreateNewBlockContexts(2, context.Label);
			var nextBlock = transform.Split(context);

			context.SetInstruction(X86.Shrd32, v1_eax, op1L, op1H, count);
			context.AppendInstruction(X86.Sar32, v2_edx, op1H, count);
			context.AppendInstruction(X86.Mov32, v3_ecx, count);
			context.AppendInstruction(X86.Test32, null, v3_ecx, transform.Constant32_32);
			context.AppendInstruction(X86.Branch, ConditionCode.Equal, newBlocks[1].Block);
			context.AppendInstruction(X86.Jmp, newBlocks[0].Block);

			newBlocks[0].AppendInstruction(X86.Mov32, resultLow, v2_edx);
			newBlocks[0].AppendInstruction(X86.Sar32, resultHigh, v2_edx, transform.Constant32_31);
			newBlocks[0].AppendInstruction(X86.Jmp, nextBlock.Block);

			newBlocks[1].AppendInstruction(X86.Mov32, resultLow, v1_eax);
			newBlocks[1].AppendInstruction(X86.Mov32, resultHigh, v2_edx);
			newBlocks[1].AppendInstruction(X86.Jmp, nextBlock.Block);
		}
	}
}
