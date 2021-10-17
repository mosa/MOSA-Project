// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.Memory
{
	public sealed class DoubleLoadParam32 : BaseTransformation
	{
		public DoubleLoadParam32() : base(IRInstruction.LoadParam32)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			var previous = GetPreviousNodeUntil(context, IRInstruction.LoadParam32, out _, context.Result);

			if (previous == null)
				return false;

			if (previous.Operand1 != context.Operand1)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var previous = GetPreviousNodeUntil(context, IRInstruction.LoadParam32, out _);

			context.SetInstruction(IRInstruction.Move32, context.Result, previous.Result);
		}
	}
}
