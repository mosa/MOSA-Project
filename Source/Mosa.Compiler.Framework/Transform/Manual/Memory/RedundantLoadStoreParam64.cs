// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.Memory
{
	public sealed class RedundantLoadStoreParam64 : BaseTransformation
	{
		public RedundantLoadStoreParam64() : base(IRInstruction.LoadParam64)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			var previous = GetPreviousNode(context);

			if (previous == null)
				return false;

			if (previous.Instruction != IRInstruction.LoadParam64)
				return false;

			if (previous.Operand1 != context.Operand1)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var previous = GetPreviousNode(context);

			context.SetInstruction(IRInstruction.Move64, context.Result, previous.Operand2);
		}
	}
}
