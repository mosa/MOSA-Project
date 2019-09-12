// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transform.IR.Special
{
	public sealed class Deadcode : BaseTransformation
	{
		public Deadcode()
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (context.ResultCount == 0 || context.ResultCount > 2)
				return false;

			if (!IsSSAForm(context.Result))
				return false;

			if (context.Result.Uses.Count != 0)
				return false;

			if (context.ResultCount == 2 && !IsSSAForm(context.Result2))
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
