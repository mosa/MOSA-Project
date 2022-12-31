// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.ARMv8A32.Transforms.IR
{
	/// <summary>
	/// SubCarryIn32
	/// </summary>
	public sealed class SubCarryIn32 : BaseTransform
	{
		public SubCarryIn32() : base(IRInstruction.SubCarryIn32, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var operand3 = context.Operand3;

			operand1 = ARMv8A32TransformHelper.MoveConstantToRegister(transform, context, operand1);
			operand2 = ARMv8A32TransformHelper.MoveConstantToRegisterOrImmediate(transform, context, operand2);
			operand3 = ARMv8A32TransformHelper.MoveConstantToRegisterOrImmediate(transform, context, operand3);

			var v1 = transform.AllocateVirtualRegister32();

			context.SetInstruction(ARMv8A32.Sub, v1, operand1, operand2);
			context.AppendInstruction(ARMv8A32.Sub, result, v1, operand3);
		}
	}
}
