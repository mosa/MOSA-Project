// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.Memory
{
	public sealed class DoubleStoreParamR8 : BaseTransformation
	{
		public DoubleStoreParamR8() : base(IRInstruction.StoreParamR8)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			var next = GetNextNode(context);

			if (next == null)
				return false;

			if (next.Instruction != IRInstruction.StoreParamR8)
				return false;

			if (next.Operand1 != context.Operand1)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			context.SetNop();
		}
	}
}
