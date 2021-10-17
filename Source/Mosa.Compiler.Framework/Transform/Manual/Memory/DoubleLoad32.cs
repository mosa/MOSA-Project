// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.Memory
{
	public sealed class DoubleLoad32 : BaseTransformation
	{
		public DoubleLoad32() : base(IRInstruction.Load32)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand2.IsResolvedConstant)
				return false;

			var previous = GetPreviousNodeUntil(context, IRInstruction.Load32, out _, context.Result);

			if (previous == null)
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
			var previous = GetPreviousNodeUntil(context, IRInstruction.Load32, out _);

			context.SetInstruction(IRInstruction.Move32, context.Result, previous.Result);
		}
	}
}
