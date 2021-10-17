// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.Memory
{
	public sealed class LoadStoreParamR4 : BaseTransformation
	{
		public LoadStoreParamR4() : base(IRInstruction.LoadParamR4)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			var previous = GetPreviousNodeUntil(context, IRInstruction.StoreParamR4, out bool immediate);

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
			var previous = GetPreviousNodeUntil(context, IRInstruction.StoreParamR4, out _);

			context.SetInstruction(IRInstruction.MoveR4, context.Result, previous.Operand2);
		}
	}
}
