// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transform.Auto.IR.StrengthReduction
{
	/// <summary>
	/// Compare32x64Add64UnsignedRange
	/// </summary>
	public sealed class Compare32x64Add64UnsignedRange : BaseTransformation
	{
		public Compare32x64Add64UnsignedRange() : base(IRInstruction.Compare32x64, true)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
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

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1.Definitions[0].Operand1;
			var t2 = context.Operand1.Definitions[0].Operand2;
			var t3 = context.Operand2;

			var v1 = transformContext.AllocateVirtualRegister(transformContext.I8);

			var e1 = transformContext.CreateConstant(MulUnsigned64(To64(t2), To64(t3)));

			context.SetInstruction(IRInstruction.Sub64, v1, t1, e1);
			context.AppendInstruction(IRInstruction.Compare32x64, ConditionCode.UnsignedLess, result, v1, t2);
		}
	}

	/// <summary>
	/// Compare32x64Add64UnsignedRange_v1
	/// </summary>
	public sealed class Compare32x64Add64UnsignedRange_v1 : BaseTransformation
	{
		public Compare32x64Add64UnsignedRange_v1() : base(IRInstruction.Compare32x64, true)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
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

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1;
			var t2 = context.Operand2.Definitions[0].Operand1;
			var t3 = context.Operand2.Definitions[0].Operand2;

			var v1 = transformContext.AllocateVirtualRegister(transformContext.I8);

			var e1 = transformContext.CreateConstant(MulUnsigned64(To64(t3), To64(t1)));

			context.SetInstruction(IRInstruction.Sub64, v1, t2, e1);
			context.AppendInstruction(IRInstruction.Compare32x64, ConditionCode.UnsignedLess, result, v1, t3);
		}
	}
}
