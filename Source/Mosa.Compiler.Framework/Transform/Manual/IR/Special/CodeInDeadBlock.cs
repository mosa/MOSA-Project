// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transform.IR.Special
{
	public sealed class CodeInDeadBlock : BaseTransformation
	{
		public CodeInDeadBlock()
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			return
					context.Block.PreviousBlocks.Count == 0
				&& !context.Block.IsHeadBlock
				&& !context.Block.IsEpilogue
				&& !context.Block.IsHandlerHeadBlock
				&& context.Instruction != IRInstruction.BlockStart
				&& context.Instruction != IRInstruction.BlockEnd;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			context.SetInstruction(IRInstruction.Nop);
		}
	}
}
