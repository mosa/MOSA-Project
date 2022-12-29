// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.Memory
{
	public sealed class DoubleLoadParam64 : BaseTransformation
	{
		public DoubleLoadParam64() : base(IRInstruction.LoadParam64, TransformationType.Manual | TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			var previous = GetPreviousNodeUntil(context, IRInstruction.LoadParam64, transformContext.Window, context.Result);

			if (previous == null)
				return false;

			if (previous.Operand1 != context.Operand1)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var previous = GetPreviousNodeUntil(context, IRInstruction.LoadParam64, transformContext.Window);

			context.SetInstruction(IRInstruction.Move64, context.Result, previous.Result);
		}
	}
}
