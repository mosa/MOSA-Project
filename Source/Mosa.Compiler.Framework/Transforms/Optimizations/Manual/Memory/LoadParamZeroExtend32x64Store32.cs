// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Memory
{
	public sealed class LoadParamZeroExtend32x64Store32 : BaseTransform
	{
		public LoadParamZeroExtend32x64Store32() : base(IRInstruction.LoadParamZeroExtend32x64, TransformType.Manual | TransformType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			var previous = GetPreviousNodeUntil(context, IRInstruction.StoreParam32, transform.Window, out bool immediate);

			if (previous == null)
				return false;

			if (!immediate && !IsSSAForm(previous.Operand2))
				return false;

			if (previous.Operand1 != context.Operand1)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var previous = GetPreviousNodeUntil(context, IRInstruction.StoreParam32, transform.Window);

			context.SetInstruction(IRInstruction.ZeroExtend32x64, context.Result, previous.Operand2);
		}
	}
}
