// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Memory
{
	public sealed class StoreLoadParamObject : BaseTransformation
	{
		public StoreLoadParamObject() : base(IRInstruction.StoreParamObject, TransformationType.Manual | TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			var previous = GetPreviousNodeUntil(context, IRInstruction.LoadParamObject, transform.Window, context.Operand2);

			if (previous == null)
				return false;

			if (previous.Operand1 != context.Operand1)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			context.SetNop();
		}
	}
}
