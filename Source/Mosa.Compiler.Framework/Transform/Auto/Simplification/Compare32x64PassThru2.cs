// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.Transform.Auto.Simplification
{
	/// <summary>
	/// Compare32x64PassThru2
	/// </summary>
	public sealed class Compare32x64PassThru2 : BaseTransformation
	{
		public Compare32x64PassThru2() : base(IRInstruction.Compare32x64, TransformationType.Auto| TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (context.ConditionCode != ConditionCode.Equal)
				return false;

			if (!context.Operand1.IsVirtualRegister)
				return false;

			if (!context.Operand2.IsResolvedConstant)
				return false;

			if (context.Operand2.ConstantUnsigned64 != 1)
				return false;

			if (context.Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Instruction != IRInstruction.And64)
				return false;

			if (!context.Operand1.Definitions[0].Operand2.IsResolvedConstant)
				return false;

			if (context.Operand1.Definitions[0].Operand2.ConstantUnsigned64 != 1)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1.Definitions[0].Operand1;

			var c1 = transformContext.CreateConstant(1);

			context.SetInstruction(IRInstruction.And64, result, t1, c1);
		}
	}

	/// <summary>
	/// Compare32x64PassThru2_v1
	/// </summary>
	public sealed class Compare32x64PassThru2_v1 : BaseTransformation
	{
		public Compare32x64PassThru2_v1() : base(IRInstruction.Compare32x64, TransformationType.Auto| TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (context.ConditionCode != ConditionCode.Equal)
				return false;

			if (!context.Operand1.IsVirtualRegister)
				return false;

			if (!context.Operand2.IsResolvedConstant)
				return false;

			if (context.Operand2.ConstantUnsigned64 != 1)
				return false;

			if (context.Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Instruction != IRInstruction.And64)
				return false;

			if (!context.Operand1.Definitions[0].Operand1.IsResolvedConstant)
				return false;

			if (context.Operand1.Definitions[0].Operand1.ConstantUnsigned64 != 1)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1.Definitions[0].Operand2;

			var c1 = transformContext.CreateConstant(1);

			context.SetInstruction(IRInstruction.And64, result, t1, c1);
		}
	}
}
