// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.Transform.Auto.Simplification
{
	/// <summary>
	/// Or32Double
	/// </summary>
	public sealed class Or32Double : BaseTransformation
	{
		public Or32Double() : base(IRInstruction.Or32, TransformationType.Auto| TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand1.IsVirtualRegister)
				return false;

			if (context.Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Instruction != IRInstruction.Or32)
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand1, context.Operand2))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1.Definitions[0].Operand1;
			var t2 = context.Operand1.Definitions[0].Operand2;

			context.SetInstruction(IRInstruction.Or32, result, t1, t2);
		}
	}

	/// <summary>
	/// Or32Double_v1
	/// </summary>
	public sealed class Or32Double_v1 : BaseTransformation
	{
		public Or32Double_v1() : base(IRInstruction.Or32, TransformationType.Auto| TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand2.IsVirtualRegister)
				return false;

			if (context.Operand2.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Instruction != IRInstruction.Or32)
				return false;

			if (!AreSame(context.Operand1, context.Operand2.Definitions[0].Operand1))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1;
			var t2 = context.Operand2.Definitions[0].Operand2;

			context.SetInstruction(IRInstruction.Or32, result, t1, t2);
		}
	}

	/// <summary>
	/// Or32Double_v2
	/// </summary>
	public sealed class Or32Double_v2 : BaseTransformation
	{
		public Or32Double_v2() : base(IRInstruction.Or32, TransformationType.Auto| TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand1.IsVirtualRegister)
				return false;

			if (context.Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Instruction != IRInstruction.Or32)
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand2, context.Operand2))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1.Definitions[0].Operand1;
			var t2 = context.Operand1.Definitions[0].Operand2;

			context.SetInstruction(IRInstruction.Or32, result, t2, t1);
		}
	}

	/// <summary>
	/// Or32Double_v3
	/// </summary>
	public sealed class Or32Double_v3 : BaseTransformation
	{
		public Or32Double_v3() : base(IRInstruction.Or32, TransformationType.Auto| TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand2.IsVirtualRegister)
				return false;

			if (context.Operand2.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Instruction != IRInstruction.Or32)
				return false;

			if (!AreSame(context.Operand1, context.Operand2.Definitions[0].Operand2))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1;
			var t2 = context.Operand2.Definitions[0].Operand1;

			context.SetInstruction(IRInstruction.Or32, result, t1, t2);
		}
	}
}
