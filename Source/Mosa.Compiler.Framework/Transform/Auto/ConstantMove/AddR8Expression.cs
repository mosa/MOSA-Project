// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.Transform.Auto.ConstantMove
{
	/// <summary>
	/// AddR8Expression
	/// </summary>
	public sealed class AddR8Expression : BaseTransformation
	{
		public AddR8Expression() : base(IRInstruction.AddR8, TransformationType.Auto | TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand2.IsVirtualRegister)
				return false;

			if (context.Operand2.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Instruction != IRInstruction.AddR8)
				return false;

			if (IsResolvedConstant(context.Operand1))
				return false;

			if (!IsResolvedConstant(context.Operand2.Definitions[0].Operand2))
				return false;

			if (IsResolvedConstant(context.Operand2.Definitions[0].Operand1))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1;
			var t2 = context.Operand2.Definitions[0].Operand1;
			var t3 = context.Operand2.Definitions[0].Operand2;

			var v1 = transformContext.AllocateVirtualRegister(transformContext.R8);

			context.SetInstruction(IRInstruction.AddR8, v1, t1, t2);
			context.AppendInstruction(IRInstruction.AddR8, result, v1, t3);
		}
	}

	/// <summary>
	/// AddR8Expression_v1
	/// </summary>
	public sealed class AddR8Expression_v1 : BaseTransformation
	{
		public AddR8Expression_v1() : base(IRInstruction.AddR8, TransformationType.Auto | TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand1.IsVirtualRegister)
				return false;

			if (context.Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Instruction != IRInstruction.AddR8)
				return false;

			if (IsResolvedConstant(context.Operand2))
				return false;

			if (!IsResolvedConstant(context.Operand1.Definitions[0].Operand2))
				return false;

			if (IsResolvedConstant(context.Operand1.Definitions[0].Operand1))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1.Definitions[0].Operand1;
			var t2 = context.Operand1.Definitions[0].Operand2;
			var t3 = context.Operand2;

			var v1 = transformContext.AllocateVirtualRegister(transformContext.R8);

			context.SetInstruction(IRInstruction.AddR8, v1, t3, t1);
			context.AppendInstruction(IRInstruction.AddR8, result, v1, t2);
		}
	}

	/// <summary>
	/// AddR8Expression_v2
	/// </summary>
	public sealed class AddR8Expression_v2 : BaseTransformation
	{
		public AddR8Expression_v2() : base(IRInstruction.AddR8, TransformationType.Auto | TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand2.IsVirtualRegister)
				return false;

			if (context.Operand2.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Instruction != IRInstruction.AddR8)
				return false;

			if (IsResolvedConstant(context.Operand1))
				return false;

			if (!IsResolvedConstant(context.Operand2.Definitions[0].Operand1))
				return false;

			if (IsResolvedConstant(context.Operand2.Definitions[0].Operand2))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1;
			var t2 = context.Operand2.Definitions[0].Operand1;
			var t3 = context.Operand2.Definitions[0].Operand2;

			var v1 = transformContext.AllocateVirtualRegister(transformContext.R8);

			context.SetInstruction(IRInstruction.AddR8, v1, t1, t3);
			context.AppendInstruction(IRInstruction.AddR8, result, v1, t2);
		}
	}

	/// <summary>
	/// AddR8Expression_v3
	/// </summary>
	public sealed class AddR8Expression_v3 : BaseTransformation
	{
		public AddR8Expression_v3() : base(IRInstruction.AddR8, TransformationType.Auto | TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand1.IsVirtualRegister)
				return false;

			if (context.Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Instruction != IRInstruction.AddR8)
				return false;

			if (IsResolvedConstant(context.Operand2))
				return false;

			if (!IsResolvedConstant(context.Operand1.Definitions[0].Operand1))
				return false;

			if (IsResolvedConstant(context.Operand1.Definitions[0].Operand2))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1.Definitions[0].Operand1;
			var t2 = context.Operand1.Definitions[0].Operand2;
			var t3 = context.Operand2;

			var v1 = transformContext.AllocateVirtualRegister(transformContext.R8);

			context.SetInstruction(IRInstruction.AddR8, v1, t3, t2);
			context.AppendInstruction(IRInstruction.AddR8, result, v1, t1);
		}
	}
}
