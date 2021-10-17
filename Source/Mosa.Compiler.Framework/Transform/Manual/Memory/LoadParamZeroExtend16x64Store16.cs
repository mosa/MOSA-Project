// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.Memory
{
	public sealed class LoadParamZeroExtend16x64Store16 : BaseTransformation
	{
		public LoadParamZeroExtend16x64Store16() : base(IRInstruction.LoadParamZeroExtend16x64)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			var previous = GetPreviousNodeUntil(context, IRInstruction.StoreParam16, out bool immediate);

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
			var previous = GetPreviousNodeUntil(context, IRInstruction.StoreParam16, out _);

			context.SetInstruction(IRInstruction.ZeroExtend16x64, context.Result, previous.Operand2);
		}
	}
}
