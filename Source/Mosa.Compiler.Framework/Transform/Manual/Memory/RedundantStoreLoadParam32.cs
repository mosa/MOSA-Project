// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.Memory
{
	public sealed class RedundantStoreLoadParam32 : BaseTransformation
	{
		public RedundantStoreLoadParam32() : base(IRInstruction.StoreParam32)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			var previous = GetPreviousNode(context);

			if (previous == null)
				return false;

			if (previous.Instruction != IRInstruction.LoadParam32)
				return false;

			if (previous.Operand1 != context.Operand1)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			context.SetInstruction(IRInstruction.Nop);
		}
	}
}
