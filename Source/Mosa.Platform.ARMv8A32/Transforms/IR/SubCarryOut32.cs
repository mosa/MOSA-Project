// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.ARMv8A32.Transforms.IR
{
	/// <summary>
	/// SubCarryOut32
	/// </summary>
	public sealed class SubCarryOut32 : BaseTransform
	{
		public SubCarryOut32() : base(IRInstruction.SubCarryOut32, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var result = context.Result;
			var result2 = context.Result2;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			operand1 = ARMv8A32TransformHelper.MoveConstantToRegister(transform, context, operand1);
			operand2 = ARMv8A32TransformHelper.MoveConstantToRegisterOrImmediate(transform, context, operand2);

			context.SetInstruction(ARMv8A32.Sub, StatusRegister.Set, result, operand1, operand2);
			context.AppendInstruction(ARMv8A32.Mov, ConditionCode.Carry, result2, transform.Constant32_1);
			context.AppendInstruction(ARMv8A32.Mov, ConditionCode.NoCarry, result2, transform.Constant32_0);
		}
	}
}