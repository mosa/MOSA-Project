// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.Transform.Auto.StrengthReduction
{
	/// <summary>
	/// Or32And32ClearAndSet
	/// </summary>
	public sealed class Or32And32ClearAndSet : BaseTransformation
	{
		public Or32And32ClearAndSet() : base(IRInstruction.Or32, TransformationType.Auto | TransformationType.Optimization)
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

			if (!IsResolvedConstant(context.Operand2))
				return false;

			if (!IsResolvedConstant(context.Operand1.Definitions[0].Operand2))
				return false;

			if (!IsZero(Not32(Or32(To32(context.Operand1.Definitions[0].Operand2), To32(context.Operand2)))))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1.Definitions[0].Operand1;
			var t2 = context.Operand2;

			context.SetInstruction(IRInstruction.Or32, result, t1, t2);
		}
	}

	/// <summary>
	/// Or32And32ClearAndSet_v1
	/// </summary>
	public sealed class Or32And32ClearAndSet_v1 : BaseTransformation
	{
		public Or32And32ClearAndSet_v1() : base(IRInstruction.Or32, TransformationType.Auto | TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand2.IsVirtualRegister)
				return false;

			if (context.Operand2.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Instruction != IRInstruction.And32)
				return false;

			if (!IsResolvedConstant(context.Operand1))
				return false;

			if (!IsResolvedConstant(context.Operand2.Definitions[0].Operand2))
				return false;

			if (!IsZero(Not32(Or32(To32(context.Operand2.Definitions[0].Operand2), To32(context.Operand1)))))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1;
			var t2 = context.Operand2.Definitions[0].Operand1;

			context.SetInstruction(IRInstruction.Or32, result, t2, t1);
		}
	}

	/// <summary>
	/// Or32And32ClearAndSet_v2
	/// </summary>
	public sealed class Or32And32ClearAndSet_v2 : BaseTransformation
	{
		public Or32And32ClearAndSet_v2() : base(IRInstruction.Or32, TransformationType.Auto | TransformationType.Optimization)
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

			if (!IsResolvedConstant(context.Operand2))
				return false;

			if (!IsResolvedConstant(context.Operand1.Definitions[0].Operand1))
				return false;

			if (!IsZero(Not32(Or32(To32(context.Operand1.Definitions[0].Operand1), To32(context.Operand2)))))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1.Definitions[0].Operand2;
			var t2 = context.Operand2;

			context.SetInstruction(IRInstruction.Or32, result, t1, t2);
		}
	}

	/// <summary>
	/// Or32And32ClearAndSet_v3
	/// </summary>
	public sealed class Or32And32ClearAndSet_v3 : BaseTransformation
	{
		public Or32And32ClearAndSet_v3() : base(IRInstruction.Or32, TransformationType.Auto | TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand2.IsVirtualRegister)
				return false;

			if (context.Operand2.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Instruction != IRInstruction.And32)
				return false;

			if (!IsResolvedConstant(context.Operand1))
				return false;

			if (!IsResolvedConstant(context.Operand2.Definitions[0].Operand1))
				return false;

			if (!IsZero(Not32(Or32(To32(context.Operand2.Definitions[0].Operand1), To32(context.Operand1)))))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1;
			var t2 = context.Operand2.Definitions[0].Operand2;

			context.SetInstruction(IRInstruction.Or32, result, t2, t1);
		}
	}
}
