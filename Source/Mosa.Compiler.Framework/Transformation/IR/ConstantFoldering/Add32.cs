// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transformation.IR.Lower
{
	public class Add32 : BaseTransformation
	{
		public override BaseInstruction Instruction { get { return IRInstruction.Add32; } }

		public override bool Match(Context context, TransformContext transformContext)
		{
			return context.Operand1.IsResolvedConstant && context.Operand2.IsResolvedConstant;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;
			var op1 = context.Operand1;
			var op2 = context.Operand2;

			var constant = ConstantOperand.Create(result.Type, (op1.ConstantUnsignedLongInteger + op2.ConstantUnsignedLongInteger) & 0xFFFFFFFF);

			context.SetInstruction(IRInstruction.MoveInt32, result, constant);
		}
	}
}
