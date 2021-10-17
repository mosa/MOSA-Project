// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.Memory
{
	public sealed class LoadSignExtend8x32Store8 : BaseTransformation
	{
		public LoadSignExtend8x32Store8() : base(IRInstruction.LoadSignExtend8x32)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand2.IsResolvedConstant)
				return false;

			var previous = GetPreviousNodeUntil(context, IRInstruction.Store8, out bool immediate);

			if (previous == null)
				return false;

			if (!immediate && !IsSSAForm(previous.Operand3))
				return false;

			if (!previous.Operand2.IsResolvedConstant)
				return false;

			if (previous.Operand1 != context.Operand1)
				return false;

			if (previous.Operand2.ConstantUnsigned32 != context.Operand2.ConstantUnsigned32)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var previous = GetPreviousNodeUntil(context, IRInstruction.Store8, out _);

			context.SetInstruction(IRInstruction.SignExtend8x32, context.Result, previous.Operand3);
		}
	}
}
