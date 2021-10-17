// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.Memory
{
	public sealed class LoadStoreR8 : BaseTransformation
	{
		public LoadStoreR8() : base(IRInstruction.LoadR8)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand2.IsResolvedConstant)
				return false;

			var previous = GetPreviousNodeUntil(context, IRInstruction.StoreR8, out bool immediate);

			if (previous == null)
				return false;

			if (!immediate && !IsSSAForm(previous.Operand3))
				return false;

			if (!previous.Operand2.IsResolvedConstant)
				return false;

			if (previous.Operand1 != context.Operand1)
				return false;

			if (previous.Operand2.ConstantUnsigned64 != context.Operand2.ConstantUnsigned64)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var previous = GetPreviousNodeUntil(context, IRInstruction.StoreR8, out _);

			context.SetInstruction(IRInstruction.Move32, context.Result, previous.Operand3);
		}
	}
}
