// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transform.Auto.IR.StrengthReduction
{
	/// <summary>
	/// UnlessXor64ShiftRight64
	/// </summary>
	public sealed class UnlessXor64ShiftRight64 : BaseTransformation
	{
		public UnlessXor64ShiftRight64() : base(IRInstruction.ShiftRight64, true)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand1.IsVirtualRegister)
				return false;

			if (context.Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Instruction != IRInstruction.Xor64)
				return false;

			if (!IsConstant(context.Operand1.Definitions[0].Operand2))
				return false;

			if (!IsConstant(context.Operand2))
				return false;

			if (!IsLessThanOrEqual(GetHighestBitSet(To64(context.Operand1.Definitions[0].Operand2)), To64(context.Operand2)))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1.Definitions[0].Operand1;
			var t2 = context.Operand2;

			context.SetInstruction(IRInstruction.ShiftRight64, result, t1, t2);
		}
	}

	/// <summary>
	/// UnlessXor64ShiftRight64_v1
	/// </summary>
	public sealed class UnlessXor64ShiftRight64_v1 : BaseTransformation
	{
		public UnlessXor64ShiftRight64_v1() : base(IRInstruction.ShiftRight64, true)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand1.IsVirtualRegister)
				return false;

			if (context.Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Instruction != IRInstruction.Xor64)
				return false;

			if (!IsConstant(context.Operand1.Definitions[0].Operand1))
				return false;

			if (!IsConstant(context.Operand2))
				return false;

			if (!IsLessThanOrEqual(GetHighestBitSet(To64(context.Operand1.Definitions[0].Operand1)), To64(context.Operand2)))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1.Definitions[0].Operand2;
			var t2 = context.Operand2;

			context.SetInstruction(IRInstruction.ShiftRight64, result, t1, t2);
		}
	}
}
