// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.Memory
{
	public sealed class RedundantLoadStoreParamR8 : BaseTransformation
	{
		public RedundantLoadStoreParamR8() : base(IRInstruction.LoadParamR8)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			var previous = GetPreviousNode(context);

			if (previous == null)
				return false;

			if (previous.Instruction != IRInstruction.StoreParamR8)
				return false;

			if (previous.Operand1 != context.Operand1)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var previous = GetPreviousNode(context);

			context.SetInstruction(IRInstruction.MoveR8, context.Result, previous.Operand2);
		}
	}
}
