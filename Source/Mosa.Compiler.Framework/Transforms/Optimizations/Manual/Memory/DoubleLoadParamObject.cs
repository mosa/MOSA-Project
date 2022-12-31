// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Memory
{
	public sealed class DoubleLoadParamObject : BaseTransform
	{
		public DoubleLoadParamObject() : base(IRInstruction.LoadParamObject, TransformType.Manual | TransformType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			var previous = GetPreviousNodeUntil(context, IRInstruction.LoadParamObject, transform.Window, context.Result);

			if (previous == null)
				return false;

			if (previous.Operand1 != context.Operand1)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var previous = GetPreviousNodeUntil(context, IRInstruction.LoadParamObject, transform.Window);

			context.SetInstruction(IRInstruction.MoveObject, context.Result, previous.Result);
		}
	}
}
