// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.ARMv8A32.Transforms.IR
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
			ARMv8A32TransformHelper.MoveConstantRightForComparison(context);

			var target = context.BranchTargets[0];
			var condition = context.ConditionCode;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			operand1 = ARMv8A32TransformHelper.MoveConstantToRegister(transform, context, operand1);
			operand2 = ARMv8A32TransformHelper.MoveConstantToRegisterOrImmediate(transform, context, operand2);

			context.SetInstruction(ARMv8A32.Cmp, null, operand1, operand2);
			context.AppendInstruction(ARMv8A32.B, condition, target);
		}
	}
}
