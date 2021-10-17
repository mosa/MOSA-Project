// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.Memory
{
	public sealed class DoubleStoreParamObject : BaseTransformation
	{
		public DoubleStoreParamObject() : base(IRInstruction.StoreParamObject)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			var next = GetNextNodeUntil(context, IRInstruction.StoreParamObject, transformContext.Window);

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
