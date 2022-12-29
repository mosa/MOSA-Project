// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.Memory
{
	public sealed class LoadParamZeroExtend32x64Store32 : BaseTransformation
	{
		public LoadParamZeroExtend32x64Store32() : base(IRInstruction.LoadParamZeroExtend32x64, TransformationType.Manual | TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			var previous = GetPreviousNodeUntil(context, IRInstruction.StoreParam32, transformContext.Window, out bool immediate);

			if (previous == null)
				return false;

			if (!immediate && !IsSSAForm(previous.Operand2))
				return false;

			if (previous.Operand1 != context.Operand1)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var previous = GetPreviousNodeUntil(context, IRInstruction.StoreParam32, transformContext.Window);

			context.SetInstruction(IRInstruction.ZeroExtend32x64, context.Result, previous.Operand2);
		}
	}
}
