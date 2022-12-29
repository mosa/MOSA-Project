// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.Memory
{
	public sealed class DoubleStoreParamR4 : BaseTransformation
	{
		public DoubleStoreParamR4() : base(IRInstruction.StoreParamR4, TransformationType.Manual | TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			var next = GetNextNodeUntil(context, IRInstruction.StoreParamR4, transformContext.Window);

			if (next == null)
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
