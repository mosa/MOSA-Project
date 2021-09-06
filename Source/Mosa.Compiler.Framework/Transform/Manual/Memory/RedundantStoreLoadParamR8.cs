// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.Memory
{
	public sealed class RedundantStoreLoadParamR8 : BaseTransformation
	{
		public RedundantStoreLoadParamR8() : base(IRInstruction.StoreParamR8)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			var previous = GetPreviousNode(context);

			if (previous == null)
				return false;

			if (previous.Instruction != IRInstruction.LoadParamR8)
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
