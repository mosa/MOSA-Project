// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Platform.x64;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transform;

namespace Mosa.Platform.x64.Transform.Auto.Specific
{
	/// <summary>
	/// And64Add64ToBlsr64
	/// </summary>
	public sealed class And64Add64ToBlsr64 : BaseTransformation
	{
		public And64Add64ToBlsr64() : base(x64.And64)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand2.IsVirtualRegister)
				return false;

			if (context.Operand2.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Instruction != x64.Add64)
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

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1;

			context.SetInstruction(x64.Blsr64, result, t1);
		}
	}

	/// <summary>
	/// And64Add64ToBlsr64_v1
	/// </summary>
	public sealed class And64Add64ToBlsr64_v1 : BaseTransformation
	{
		public And64Add64ToBlsr64_v1() : base(x64.And64)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand1.IsVirtualRegister)
				return false;

			if (context.Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Instruction != x64.Add64)
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

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1.Definitions[0].Operand1;

			context.SetInstruction(x64.Blsr64, result, t1);
		}
	}

	/// <summary>
	/// And64Add64ToBlsr64_v2
	/// </summary>
	public sealed class And64Add64ToBlsr64_v2 : BaseTransformation
	{
		public And64Add64ToBlsr64_v2() : base(x64.And64)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand2.IsVirtualRegister)
				return false;

			if (context.Operand2.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Instruction != x64.Add64)
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

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1;

			context.SetInstruction(x64.Blsr64, result, t1);
		}
	}

	/// <summary>
	/// And64Add64ToBlsr64_v3
	/// </summary>
	public sealed class And64Add64ToBlsr64_v3 : BaseTransformation
	{
		public And64Add64ToBlsr64_v3() : base(x64.And64)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand1.IsVirtualRegister)
				return false;

			if (context.Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Instruction != x64.Add64)
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

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1.Definitions[0].Operand2;

			context.SetInstruction(x64.Blsr64, result, t1);
		}
	}
}
