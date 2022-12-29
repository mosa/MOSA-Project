// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.Memory
{
	public sealed class DoubleStoreParam64 : BaseTransformation
	{
		public DoubleStoreParam64() : base(IRInstruction.StoreParam64, TransformationType.Manual | TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			var next = GetNextNodeUntil(context, IRInstruction.StoreParam64, transformContext.Window);

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
