// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.Transform.Auto.StrengthReduction
{
	/// <summary>
	/// IsolateAndFlipLeastSignificantBit32
	/// </summary>
	public sealed class IsolateAndFlipLeastSignificantBit32 : BaseTransformation
	{
		public IsolateAndFlipLeastSignificantBit32() : base(IRInstruction.Add32, TransformationType.Auto| TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand1.IsVirtualRegister)
				return false;

			if (!context.Operand2.IsResolvedConstant)
				return false;

			if (context.Operand2.ConstantUnsigned64 != 1)
				return false;

			if (context.Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Instruction != IRInstruction.ShiftRight32)
				return false;

			if (!context.Operand1.Definitions[0].Operand1.IsVirtualRegister)
				return false;

			if (!context.Operand1.Definitions[0].Operand2.IsResolvedConstant)
				return false;

			if (context.Operand1.Definitions[0].Operand2.ConstantUnsigned64 != 31)
				return false;

			if (context.Operand1.Definitions[0].Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Operand1.Definitions[0].Instruction != IRInstruction.ShiftLeft32)
				return false;

			if (!context.Operand1.Definitions[0].Operand1.Definitions[0].Operand2.IsResolvedConstant)
				return false;

			if (context.Operand1.Definitions[0].Operand1.Definitions[0].Operand2.ConstantUnsigned64 != 31)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1;

			var v1 = transformContext.AllocateVirtualRegister(transformContext.I4);

			var e1 = transformContext.CreateConstant(To32(1));

			context.SetInstruction(IRInstruction.Not32, v1, t1);
			context.AppendInstruction(IRInstruction.And32, result, v1, e1);
		}
	}

	/// <summary>
	/// IsolateAndFlipLeastSignificantBit32_v1
	/// </summary>
	public sealed class IsolateAndFlipLeastSignificantBit32_v1 : BaseTransformation
	{
		public IsolateAndFlipLeastSignificantBit32_v1() : base(IRInstruction.Add32, TransformationType.Auto| TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand1.IsResolvedConstant)
				return false;

			if (context.Operand1.ConstantUnsigned64 != 1)
				return false;

			if (!context.Operand2.IsVirtualRegister)
				return false;

			if (context.Operand2.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Instruction != IRInstruction.ShiftRight32)
				return false;

			if (!context.Operand2.Definitions[0].Operand1.IsVirtualRegister)
				return false;

			if (!context.Operand2.Definitions[0].Operand2.IsResolvedConstant)
				return false;

			if (context.Operand2.Definitions[0].Operand2.ConstantUnsigned64 != 31)
				return false;

			if (context.Operand2.Definitions[0].Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Operand1.Definitions[0].Instruction != IRInstruction.ShiftLeft32)
				return false;

			if (!context.Operand2.Definitions[0].Operand1.Definitions[0].Operand2.IsResolvedConstant)
				return false;

			if (context.Operand2.Definitions[0].Operand1.Definitions[0].Operand2.ConstantUnsigned64 != 31)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand2.Definitions[0].Operand1.Definitions[0].Operand1;

			var v1 = transformContext.AllocateVirtualRegister(transformContext.I4);

			var e1 = transformContext.CreateConstant(To32(1));

			context.SetInstruction(IRInstruction.Not32, v1, t1);
			context.AppendInstruction(IRInstruction.And32, result, v1, e1);
		}
	}
}
