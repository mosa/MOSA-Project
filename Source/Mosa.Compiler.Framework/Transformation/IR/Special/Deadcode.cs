// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transformation.IR.Special
{
	public class Deadcode : BaseTransformation
	{
		public override BaseInstruction Instruction { get { return null; } }

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (context.ResultCount == 0 || context.ResultCount > 2)
				return false;

			if (!ValidateSSAForm(context.Result))
				return false;

			// special case - phi instruction references itself - this can be caused by optimizations
			if (context.Instruction == IRInstruction.Phi && context.Result == context.Operand1)
				return true;

			if (context.Result.Uses.Count != 0)
				return false;

			if (context.ResultCount == 2 && !ValidateSSAForm(context.Result2))
				return false;

			if (context.ResultCount == 2 && context.Result2.Uses.Count != 0)
				return false;

			if (!(context.Instruction is BaseIRInstruction))
				return false;

			if (context.Instruction == IRInstruction.CallDynamic
				|| context.Instruction == IRInstruction.CallInterface
				|| context.Instruction == IRInstruction.CallDirect
				|| context.Instruction == IRInstruction.CallStatic
				|| context.Instruction == IRInstruction.CallVirtual
				|| context.Instruction == IRInstruction.NewObject
				|| context.Instruction == IRInstruction.SetReturn32
				|| context.Instruction == IRInstruction.SetReturn64
				|| context.Instruction == IRInstruction.SetReturnR4
				|| context.Instruction == IRInstruction.SetReturnR8
				|| context.Instruction == IRInstruction.SetReturnCompound
				|| context.Instruction == IRInstruction.IntrinsicMethodCall)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			context.SetInstruction(IRInstruction.Nop);
		}
	}
}
