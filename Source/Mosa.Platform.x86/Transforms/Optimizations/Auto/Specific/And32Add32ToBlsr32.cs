// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.Optimizations.Auto.Specific
{
	/// <summary>
	/// And32Add32ToBlsr32
	/// </summary>
	public sealed class And32Add32ToBlsr32 : BaseTransform
	{
		public And32Add32ToBlsr32() : base(X86.And32, TransformType.Auto | TransformType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			if (!context.Operand2.IsVirtualRegister)
				return false;

			if (context.Operand2.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Instruction != X86.Add32)
				return false;

			if (!context.Operand2.Definitions[0].Operand2.IsResolvedConstant)
				return false;

			if (context.Operand2.Definitions[0].Operand2.ConstantUnsigned64 != 18446744073709551615)
				return false;

			if (!AreSame(context.Operand1, context.Operand2.Definitions[0].Operand1))
				return false;

			if (!IsVirtualRegister(context.Operand1))
				return false;

			if (IsCarryFlagUsed(context))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var result = context.Result;

			var t1 = context.Operand1;

			context.SetInstruction(X86.Blsr32, result, t1);
		}
	}

	/// <summary>
	/// And32Add32ToBlsr32_v1
	/// </summary>
	public sealed class And32Add32ToBlsr32_v1 : BaseTransform
	{
		public And32Add32ToBlsr32_v1() : base(X86.And32, TransformType.Auto | TransformType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			if (!context.Operand1.IsVirtualRegister)
				return false;

			if (context.Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Instruction != X86.Add32)
				return false;

			if (!context.Operand1.Definitions[0].Operand2.IsResolvedConstant)
				return false;

			if (context.Operand1.Definitions[0].Operand2.ConstantUnsigned64 != 18446744073709551615)
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand1, context.Operand2))
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

			context.SetInstruction(X86.Blsr32, result, t1);
		}
	}

	/// <summary>
	/// And32Add32ToBlsr32_v2
	/// </summary>
	public sealed class And32Add32ToBlsr32_v2 : BaseTransform
	{
		public And32Add32ToBlsr32_v2() : base(X86.And32, TransformType.Auto | TransformType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			if (!context.Operand2.IsVirtualRegister)
				return false;

			if (context.Operand2.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Instruction != X86.Add32)
				return false;

			if (!context.Operand2.Definitions[0].Operand1.IsResolvedConstant)
				return false;

			if (context.Operand2.Definitions[0].Operand1.ConstantUnsigned64 != 18446744073709551615)
				return false;

			if (!AreSame(context.Operand1, context.Operand2.Definitions[0].Operand2))
				return false;

			if (!IsVirtualRegister(context.Operand1))
				return false;

			if (IsCarryFlagUsed(context))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var result = context.Result;

			var t1 = context.Operand1;

			context.SetInstruction(X86.Blsr32, result, t1);
		}
	}

	/// <summary>
	/// And32Add32ToBlsr32_v3
	/// </summary>
	public sealed class And32Add32ToBlsr32_v3 : BaseTransform
	{
		public And32Add32ToBlsr32_v3() : base(X86.And32, TransformType.Auto | TransformType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			if (!context.Operand1.IsVirtualRegister)
				return false;

			if (context.Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Instruction != X86.Add32)
				return false;

			if (!context.Operand1.Definitions[0].Operand1.IsResolvedConstant)
				return false;

			if (context.Operand1.Definitions[0].Operand1.ConstantUnsigned64 != 18446744073709551615)
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand2, context.Operand2))
				return false;

			if (!IsVirtualRegister(context.Operand1.Definitions[0].Operand2))
				return false;

			if (IsCarryFlagUsed(context))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var result = context.Result;

			var t1 = context.Operand1.Definitions[0].Operand2;

			context.SetInstruction(X86.Blsr32, result, t1);
		}
	}
}
