// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transform.Auto.IR.StrengthReduction
{
	/// <summary>
	/// Or64And64ClearAndSet
	/// </summary>
	public sealed class Or64And64ClearAndSet : BaseTransformation
	{
		public Or64And64ClearAndSet() : base(IRInstruction.Or64, true)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand1.IsVirtualRegister)
				return false;

			if (context.Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Instruction != IRInstruction.And64)
				return false;

			if (!IsResolvedConstant(context.Operand1.Definitions[0].Operand2))
				return false;

			if (!IsResolvedConstant(context.Operand2))
				return false;

			if (!IsZero(Not64(Or64(To64(context.Operand1.Definitions[0].Operand2), To64(context.Operand2)))))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1.Definitions[0].Operand1;
			var t2 = context.Operand1.Definitions[0].Operand2;

			context.SetInstruction(IRInstruction.Or64, result, t2, t1);
		}
	}

	/// <summary>
	/// Or64And64ClearAndSet_v1
	/// </summary>
	public sealed class Or64And64ClearAndSet_v1 : BaseTransformation
	{
		public Or64And64ClearAndSet_v1() : base(IRInstruction.Or64, true)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand2.IsVirtualRegister)
				return false;

			if (context.Operand2.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Instruction != IRInstruction.And64)
				return false;

			if (!IsResolvedConstant(context.Operand2.Definitions[0].Operand2))
				return false;

			if (!IsResolvedConstant(context.Operand1))
				return false;

			if (!IsZero(Not64(Or64(To64(context.Operand2.Definitions[0].Operand2), To64(context.Operand1)))))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand2.Definitions[0].Operand1;
			var t2 = context.Operand2.Definitions[0].Operand2;

			context.SetInstruction(IRInstruction.Or64, result, t2, t1);
		}
	}

	/// <summary>
	/// Or64And64ClearAndSet_v2
	/// </summary>
	public sealed class Or64And64ClearAndSet_v2 : BaseTransformation
	{
		public Or64And64ClearAndSet_v2() : base(IRInstruction.Or64, true)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand1.IsVirtualRegister)
				return false;

			if (context.Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Instruction != IRInstruction.And64)
				return false;

			if (!IsResolvedConstant(context.Operand1.Definitions[0].Operand1))
				return false;

			if (!IsResolvedConstant(context.Operand2))
				return false;

			if (!IsZero(Not64(Or64(To64(context.Operand1.Definitions[0].Operand1), To64(context.Operand2)))))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1.Definitions[0].Operand1;
			var t2 = context.Operand1.Definitions[0].Operand2;

			context.SetInstruction(IRInstruction.Or64, result, t1, t2);
		}
	}

	/// <summary>
	/// Or64And64ClearAndSet_v3
	/// </summary>
	public sealed class Or64And64ClearAndSet_v3 : BaseTransformation
	{
		public Or64And64ClearAndSet_v3() : base(IRInstruction.Or64, true)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand2.IsVirtualRegister)
				return false;

			if (context.Operand2.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Instruction != IRInstruction.And64)
				return false;

			if (!IsResolvedConstant(context.Operand2.Definitions[0].Operand1))
				return false;

			if (!IsResolvedConstant(context.Operand1))
				return false;

			if (!IsZero(Not64(Or64(To64(context.Operand2.Definitions[0].Operand1), To64(context.Operand1)))))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand2.Definitions[0].Operand1;
			var t2 = context.Operand2.Definitions[0].Operand2;

			context.SetInstruction(IRInstruction.Or64, result, t1, t2);
		}
	}
}
