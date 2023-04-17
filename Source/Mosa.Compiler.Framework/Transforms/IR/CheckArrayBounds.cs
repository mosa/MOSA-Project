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
		var newBlock = transform.CreateNewBlockContexts(1, context.Label)[0];
		var nextBlock = transform.Split(context);

		// Get array length
		var v1_length = transform.VirtualRegisters.Allocate32();

		// Now compare length with index
		// If index is greater than or equal to the length then jump to exception block, otherwise jump to next block
		context.SetInstruction(transform.LoadInstruction, v1_length, array, transform.Constant32_0);
		context.AppendInstruction(transform.BranchInstruction, ConditionCode.UnsignedGreaterOrEqual, null, index, v1_length, newBlock.Block);
		context.AppendInstruction(IRInstruction.Jmp, nextBlock.Block);

		newBlock.AppendInstruction(IRInstruction.ThrowIndexOutOfRange);
	}
}
