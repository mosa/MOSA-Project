// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.StrengthReduction
{
	/// <summary>
	/// Compare64x64DivUnsignedRange
	/// </summary>
	public sealed class Compare64x64DivUnsignedRange : BaseTransform
	{
		public Compare64x64DivUnsignedRange() : base(IRInstruction.Compare64x64, TransformType.Auto | TransformType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			if (context.ConditionCode != ConditionCode.Equal)
				return false;

			if (!context.Operand1.IsVirtualRegister)
				return false;

			if (context.Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Instruction != IRInstruction.DivUnsigned64)
				return false;

			if (!IsResolvedConstant(context.Operand2))
				return false;

			if (!IsResolvedConstant(context.Operand1.Definitions[0].Operand2))
				return false;

			if (IsZero(context.Operand1.Definitions[0].Operand2))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var result = context.Result;

			var t1 = context.Operand1.Definitions[0].Operand1;
			var t2 = context.Operand1.Definitions[0].Operand2;
			var t3 = context.Operand2;

			var v1 = transform.AllocateVirtualRegister(transform.I8);

			var e1 = transform.CreateConstant(MulUnsigned64(To64(t2), To64(t3)));

			context.SetInstruction(IRInstruction.Sub64, v1, t1, e1);
			context.AppendInstruction(IRInstruction.Compare64x64, ConditionCode.UnsignedLess, result, v1, t2);
		}
	}

	/// <summary>
	/// Compare64x64DivUnsignedRange_v1
	/// </summary>
	public sealed class Compare64x64DivUnsignedRange_v1 : BaseTransform
	{
		public Compare64x64DivUnsignedRange_v1() : base(IRInstruction.Compare64x64, TransformType.Auto | TransformType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			if (context.ConditionCode != ConditionCode.Equal)
				return false;

			if (!context.Operand2.IsVirtualRegister)
				return false;

			if (context.Operand2.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Instruction != IRInstruction.DivUnsigned64)
				return false;

			if (!IsResolvedConstant(context.Operand1))
				return false;

			if (!IsResolvedConstant(context.Operand2.Definitions[0].Operand2))
				return false;

			if (IsZero(context.Operand2.Definitions[0].Operand2))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var result = context.Result;

			var t1 = context.Operand1;
			var t2 = context.Operand2.Definitions[0].Operand1;
			var t3 = context.Operand2.Definitions[0].Operand2;

			var v1 = transform.AllocateVirtualRegister(transform.I8);

			var e1 = transform.CreateConstant(MulUnsigned64(To64(t3), To64(t1)));

			context.SetInstruction(IRInstruction.Sub64, v1, t2, e1);
			context.AppendInstruction(IRInstruction.Compare64x64, ConditionCode.UnsignedLess, result, v1, t3);
		}
	}
}
