// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transform.Auto.IR.StrengthReduction
{
	/// <summary>
	/// Compare32x32Add32UnsignedNegative
	/// </summary>
	public sealed class Compare32x32Add32UnsignedNegative : BaseTransformation
	{
		public Compare32x32Add32UnsignedNegative() : base(IRInstruction.Compare32x32)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (context.ConditionCode != ConditionCode.UnsignedLessOrEqual)
				return false;

			if (!context.Operand1.IsVirtualRegister)
				return false;

			if (context.Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Instruction != IRInstruction.Add32)
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand1, context.Operand2))
				return false;

			if (!IsUnsignedIntegerPositive(context.Operand1.Definitions[0].Operand2))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var c1 = transformContext.CreateConstant(0);

			context.SetInstruction(IRInstruction.Move32, result, c1);
		}
	}

	/// <summary>
	/// Compare32x32Add32UnsignedNegative_v1
	/// </summary>
	public sealed class Compare32x32Add32UnsignedNegative_v1 : BaseTransformation
	{
		public Compare32x32Add32UnsignedNegative_v1() : base(IRInstruction.Compare32x32)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (context.ConditionCode != ConditionCode.UnsignedLessOrEqual)
				return false;

			if (!context.Operand1.IsVirtualRegister)
				return false;

			if (context.Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Instruction != IRInstruction.Add32)
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand2, context.Operand2))
				return false;

			if (!IsUnsignedIntegerPositive(context.Operand1.Definitions[0].Operand1))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var c1 = transformContext.CreateConstant(0);

			context.SetInstruction(IRInstruction.Move32, result, c1);
		}
	}

	/// <summary>
	/// Compare32x32Add32UnsignedNegative_v2
	/// </summary>
	public sealed class Compare32x32Add32UnsignedNegative_v2 : BaseTransformation
	{
		public Compare32x32Add32UnsignedNegative_v2() : base(IRInstruction.Compare32x32)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (context.ConditionCode != ConditionCode.UnsignedLessOrEqual)
				return false;

			if (!context.Operand1.IsVirtualRegister)
				return false;

			if (context.Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Instruction != IRInstruction.Add32)
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand1, context.Operand2))
				return false;

			if (!IsUnsignedIntegerPositive(context.Operand1.Definitions[0].Operand2))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var c1 = transformContext.CreateConstant(0);

			context.SetInstruction(IRInstruction.Move32, result, c1);
		}
	}

	/// <summary>
	/// Compare32x32Add32UnsignedNegative_v3
	/// </summary>
	public sealed class Compare32x32Add32UnsignedNegative_v3 : BaseTransformation
	{
		public Compare32x32Add32UnsignedNegative_v3() : base(IRInstruction.Compare32x32)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (context.ConditionCode != ConditionCode.UnsignedLessOrEqual)
				return false;

			if (!context.Operand1.IsVirtualRegister)
				return false;

			if (context.Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Instruction != IRInstruction.Add32)
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand2, context.Operand2))
				return false;

			if (!IsUnsignedIntegerPositive(context.Operand1.Definitions[0].Operand1))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var c1 = transformContext.CreateConstant(0);

			context.SetInstruction(IRInstruction.Move32, result, c1);
		}
	}
}
