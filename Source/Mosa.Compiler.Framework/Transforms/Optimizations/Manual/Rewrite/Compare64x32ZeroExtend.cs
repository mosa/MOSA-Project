// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Rewrite
{
	/// <summary>
	/// Compare64x32ZeroExtend
	/// </summary>
	public sealed class Compare64x32ZeroExtend : BaseTransformation
	{
		public Compare64x32ZeroExtend() : base(IRInstruction.Compare64x32, TransformationType.Manual | TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			if (!context.Operand1.IsVirtualRegister)
				return false;

			if (!context.Operand2.IsVirtualRegister)
				return false;

			if (context.Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Instruction != IRInstruction.ZeroExtend32x64)
				return false;

			if (context.Operand2.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Instruction != IRInstruction.ZeroExtend32x64)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var result = context.Result;

			var t1 = context.Operand1.Definitions[0].Operand1;
			var t2 = context.Operand2.Definitions[0].Operand1;

			var cond = context.ConditionCode;

			context.SetInstruction(IRInstruction.Compare32x32, cond, result, t1, t2);
		}
	}
}
