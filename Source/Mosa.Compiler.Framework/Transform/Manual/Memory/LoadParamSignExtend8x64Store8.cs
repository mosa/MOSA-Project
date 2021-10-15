// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.Memory
{
	public sealed class LoadParamSignExtend8x64Store8 : BaseTransformation
	{
		public LoadParamSignExtend8x64Store8() : base(IRInstruction.LoadParamSignExtend8x64)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			var previous = GetPreviousNodeUntil(context, IRInstruction.StoreParam8, out bool immediatePrevious);

			if (previous == null)
				return false;

			if (!immediatePrevious && !IsSSAForm(previous.Operand2))
				return false;

			if (previous.Operand1 != context.Operand1)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var previous = GetPreviousNodeUntil(context, IRInstruction.StoreParam8, out _);

			context.SetInstruction(IRInstruction.SignExtend8x64, context.Result, previous.Operand2);
		}
	}
}
