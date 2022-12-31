// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.IR
{
	/// <summary>
	/// Branch32
	/// </summary>
	public sealed class Branch32 : BaseTransform
	{
		public Branch32() : base(IRInstruction.Branch32, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			transform.MoveConstantRight(context);

			var target = context.BranchTargets[0];
			var condition = context.ConditionCode;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			context.SetInstruction(X64.Cmp32, null, operand1, operand2);
			context.AppendInstruction(X64.Branch, condition, target);
		}
	}
}
