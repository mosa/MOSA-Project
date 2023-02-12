// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.IR;

public sealed class CheckArrayBounds : BaseTransform
{
	public CheckArrayBounds() : base(IRInstruction.CheckArrayBounds, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var array = context.Operand1;
		var index = context.Operand2;

		// First create new block and split current block
		var newBlocks = transform.CreateNewBlockContexts(1, context.Label);
		var nextBlock = transform.Split(context);

		// Get array length
		var v1_length = transform.AllocateVirtualRegister32();

		// Now compare length with index
		// If index is greater than or equal to the length then jump to exception block, otherwise jump to next block
		if (transform.Is32BitPlatform)
		{
			context.SetInstruction(IRInstruction.Load32, v1_length, array, transform.Constant32_0);
			context.AppendInstruction(IRInstruction.Branch32, ConditionCode.UnsignedGreaterOrEqual, null, index, v1_length, newBlocks[0].Block);
		}
		else
		{
			context.SetInstruction(IRInstruction.Load64, v1_length, array, transform.Constant64_0);
			context.AppendInstruction(IRInstruction.Branch64, ConditionCode.UnsignedGreaterOrEqual, null, index, v1_length, newBlocks[0].Block);
		}

		context.AppendInstruction(IRInstruction.Jmp, nextBlock.Block);

		newBlocks[0].AppendInstruction(IRInstruction.ThrowIndexOutOfRange);
	}
}
