// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transform.Auto.IR.StrengthReduction
{
	/// <summary>
	/// UselessAnd32ShiftRight32
	/// </summary>
	public sealed class UselessAnd32ShiftRight32 : BaseTransformation
	{
		public UselessAnd32ShiftRight32() : base(IRInstruction.ShiftRight32)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand1.IsVirtualRegister)
				return false;

			if (context.Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Instruction != IRInstruction.And32)
				return false;

			if (!IsConstant(context.Operand1.Definitions[0].Operand2))
				return false;

			if (!IsConstant(context.Operand2))
				return false;

			if (IsZero(context.Operand2))
				return false;

			if (!IsLessOrEqual(GetHighestSetBit(To32(context.Operand1.Definitions[0].Operand2)), To32(context.Operand2)))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1.Definitions[0].Operand1;
			var t2 = context.Operand2;

			context.SetInstruction(IRInstruction.ShiftRight32, result, t1, t2);
		}
	}

	/// <summary>
	/// UselessAnd32ShiftRight32_v1
	/// </summary>
	public sealed class UselessAnd32ShiftRight32_v1 : BaseTransformation
	{
		public UselessAnd32ShiftRight32_v1() : base(IRInstruction.ShiftRight32)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand1.IsVirtualRegister)
				return false;

			if (context.Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Instruction != IRInstruction.And32)
				return false;

			if (!IsConstant(context.Operand1.Definitions[0].Operand1))
				return false;

			if (!IsConstant(context.Operand2))
				return false;

			if (IsZero(context.Operand2))
				return false;

			if (!IsLessOrEqual(GetHighestSetBit(To32(context.Operand1.Definitions[0].Operand1)), To32(context.Operand2)))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1.Definitions[0].Operand2;
			var t2 = context.Operand2;

			context.SetInstruction(IRInstruction.ShiftRight32, result, t1, t2);
		}
	}
}
