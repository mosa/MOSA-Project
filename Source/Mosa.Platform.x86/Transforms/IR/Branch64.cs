
using System.Diagnostics;

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR
{
	/// <summary>
	/// Branch64
	/// </summary>
	public sealed class Branch64 : BaseTransformation
	{
		public Branch64() : base(IRInstruction.Branch64, TransformationType.Manual | TransformationType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			transform.SplitLongOperand(context.Operand1, out var op1L, out var op1H);
			transform.SplitLongOperand(context.Operand2, out var op2L, out var op2H);

			var target = context.BranchTargets[0];
			var condition = context.ConditionCode;

			var nextBlock = transform.Split(context);
			var newBlocks = transform.CreateNewBlockContexts(2, context.Label);

			if (op1H.IsResolvedConstant && op2H.IsResolvedConstant)
			{
				// high dwords are constants
				context.SetInstruction(X86.Jmp, op1H.ConstantUnsigned32 == op2H.ConstantUnsigned32 ? newBlocks[1].Block : newBlocks[0].Block);
			}
			else
			{
				// Compare high dwords
				context.SetInstruction(X86.Cmp32, null, op1H, op2H);
				context.AppendInstruction(X86.Branch, ConditionCode.Equal, newBlocks[1].Block);
				context.AppendInstruction(X86.Jmp, newBlocks[0].Block);
			}

			// Branch if check already gave results
			newBlocks[0].AppendInstruction(X86.Branch, condition, target);
			newBlocks[0].AppendInstruction(X86.Jmp, nextBlock.Block);

			// Compare low dwords
			newBlocks[1].AppendInstruction(X86.Cmp32, null, op1L, op2L);
			newBlocks[1].AppendInstruction(X86.Branch, condition.GetUnsigned(), target);
			newBlocks[1].AppendInstruction(X86.Jmp, nextBlock.Block);
		}
	}
}
