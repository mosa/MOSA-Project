// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.Memory
{
	public sealed class DoubleStoreParamR4 : BaseTransformation
	{
		public DoubleStoreParamR4() : base(IRInstruction.StoreParamR4)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			var next = GetNextNode(context);

			if (next == null)
				return false;

			if (next.Instruction != IRInstruction.StoreParamR4)
				return false;

			if (next.Operand1 != context.Operand1)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			context.SetInstruction(IRInstruction.Nop);
		}
	}
}
