// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.Transform.Auto.Rewrite
{
	/// <summary>
	/// IfThenElse64v3
	/// </summary>
	public sealed class IfThenElse64v3 : BaseTransformation
	{
		public IfThenElse64v3() : base(IRInstruction.IfThenElse64)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand1.IsVirtualRegister)
				return false;

			if (context.Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Instruction != IRInstruction.Compare64x64)
				return false;

			if (context.Operand1.Definitions[0].ConditionCode != ConditionCode.NotEqual)
				return false;

			if (!context.Operand1.Definitions[0].Operand1.IsResolvedConstant)
				return false;

			if (context.Operand1.Definitions[0].Operand1.ConstantUnsigned64 != 0)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1.Definitions[0].Operand2;
			var t2 = context.Operand2;
			var t3 = context.Operand3;

			context.SetInstruction(IRInstruction.IfThenElse64, result, t1, t2, t3);
		}
	}

	/// <summary>
	/// IfThenElse64v3_v1
	/// </summary>
	public sealed class IfThenElse64v3_v1 : BaseTransformation
	{
		public IfThenElse64v3_v1() : base(IRInstruction.IfThenElse64)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand1.IsVirtualRegister)
				return false;

			if (context.Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Instruction != IRInstruction.Compare64x64)
				return false;

			if (context.Operand1.Definitions[0].ConditionCode != ConditionCode.NotEqual)
				return false;

			if (!context.Operand1.Definitions[0].Operand2.IsResolvedConstant)
				return false;

			if (context.Operand1.Definitions[0].Operand2.ConstantUnsigned64 != 0)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1.Definitions[0].Operand1;
			var t2 = context.Operand2;
			var t3 = context.Operand3;

			context.SetInstruction(IRInstruction.IfThenElse64, result, t1, t2, t3);
		}
	}
}
