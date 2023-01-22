// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.x86.Transforms.IR
{
	/// <summary>
	/// Call
	/// </summary>
	public sealed class Call : BaseTransform
	{
		public Call() : base(IRInstruction.Call, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			if (context.Result?.IsInteger64 == true)
			{
				transform.SplitLongOperand(context.Result, out _, out _);
			}

			foreach (var operand in context.Operands)
			{
				if (operand.IsInteger64)
				{
					transform.SplitLongOperand(operand, out _, out _);
				}
			}
		}
	}
}
