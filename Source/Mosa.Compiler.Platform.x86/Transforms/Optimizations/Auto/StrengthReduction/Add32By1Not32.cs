// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.x86.Transforms.Optimizations.Auto.StrengthReduction
{
	/// <summary>
	/// Add32By1Not32
	/// </summary>
	public sealed class Add32By1Not32 : BaseTransform
	{
		public Add32By1Not32() : base(X86.Add32, TransformType.Auto | TransformType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			if (!context.Operand1.IsVirtualRegister)
				return false;

			if (!context.Operand2.IsResolvedConstant)
				return false;

			if (context.Operand2.ConstantUnsigned64 != 1)
				return false;

			if (context.Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Instruction != X86.Not32)
				return false;

			if (!IsVirtualRegister(context.Operand1.Definitions[0].Operand1))
				return false;

			if (IsCarryFlagUsed(context))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var result = context.Result;

			var t1 = context.Operand1.Definitions[0].Operand1;

			context.SetInstruction(X86.Neg32, result, t1);
		}
	}

	/// <summary>
	/// Add32By1Not32_v1
	/// </summary>
	public sealed class Add32By1Not32_v1 : BaseTransform
	{
		public Add32By1Not32_v1() : base(X86.Add32, TransformType.Auto | TransformType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			if (!context.Operand1.IsResolvedConstant)
				return false;

			if (context.Operand1.ConstantUnsigned64 != 1)
				return false;

			if (!context.Operand2.IsVirtualRegister)
				return false;

			if (context.Operand2.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Instruction != X86.Not32)
				return false;

			if (!IsVirtualRegister(context.Operand2.Definitions[0].Operand1))
				return false;

			if (IsCarryFlagUsed(context))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var result = context.Result;

			var t1 = context.Operand2.Definitions[0].Operand1;

			context.SetInstruction(X86.Neg32, result, t1);
		}
	}
}
