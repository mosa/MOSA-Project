// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.Algebraic
{
	/// <summary>
	/// Signed64AAPlusBBMinus2AB
	/// </summary>
	public sealed class Signed64AAPlusBBMinus2AB : BaseTransformation
	{
		public Signed64AAPlusBBMinus2AB() : base(IRInstruction.Sub64, TransformationType.Auto | TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			if (!context.Operand1.IsVirtualRegister)
				return false;

			if (!context.Operand2.IsVirtualRegister)
				return false;

			if (context.Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Instruction != IRInstruction.Add64)
				return false;

			if (!context.Operand1.Definitions[0].Operand1.IsVirtualRegister)
				return false;

			if (!context.Operand1.Definitions[0].Operand2.IsVirtualRegister)
				return false;

			if (context.Operand1.Definitions[0].Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Operand1.Definitions[0].Instruction != IRInstruction.MulSigned64)
				return false;

			if (context.Operand1.Definitions[0].Operand2.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Operand2.Definitions[0].Instruction != IRInstruction.MulSigned64)
				return false;

			if (context.Operand2.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Instruction != IRInstruction.ShiftLeft64)
				return false;

			if (!context.Operand2.Definitions[0].Operand1.IsVirtualRegister)
				return false;

			if (!context.Operand2.Definitions[0].Operand2.IsResolvedConstant)
				return false;

			if (context.Operand2.Definitions[0].Operand2.ConstantUnsigned64 != 1)
				return false;

			if (context.Operand2.Definitions[0].Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Operand1.Definitions[0].Instruction != IRInstruction.MulSigned64)
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1, context.Operand1.Definitions[0].Operand1.Definitions[0].Operand2))
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1, context.Operand2.Definitions[0].Operand1.Definitions[0].Operand1))
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand2.Definitions[0].Operand1, context.Operand1.Definitions[0].Operand2.Definitions[0].Operand2))
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand2.Definitions[0].Operand1, context.Operand2.Definitions[0].Operand1.Definitions[0].Operand2))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var result = context.Result;

			var t1 = context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1;
			var t2 = context.Operand1.Definitions[0].Operand2.Definitions[0].Operand1;

			var v1 = transform.AllocateVirtualRegister(transform.I8);
			var v2 = transform.AllocateVirtualRegister(transform.I8);

			context.SetInstruction(IRInstruction.Sub64, v1, t1, t2);
			context.AppendInstruction(IRInstruction.Sub64, v2, t1, t2);
			context.AppendInstruction(IRInstruction.MulSigned64, result, v2, v1);
		}
	}

	/// <summary>
	/// Signed64AAPlusBBMinus2AB_v1
	/// </summary>
	public sealed class Signed64AAPlusBBMinus2AB_v1 : BaseTransformation
	{
		public Signed64AAPlusBBMinus2AB_v1() : base(IRInstruction.Sub64, TransformationType.Auto | TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			if (!context.Operand1.IsVirtualRegister)
				return false;

			if (!context.Operand2.IsVirtualRegister)
				return false;

			if (context.Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Instruction != IRInstruction.Add64)
				return false;

			if (!context.Operand1.Definitions[0].Operand1.IsVirtualRegister)
				return false;

			if (!context.Operand1.Definitions[0].Operand2.IsVirtualRegister)
				return false;

			if (context.Operand1.Definitions[0].Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Operand1.Definitions[0].Instruction != IRInstruction.MulSigned64)
				return false;

			if (context.Operand1.Definitions[0].Operand2.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Operand2.Definitions[0].Instruction != IRInstruction.MulSigned64)
				return false;

			if (context.Operand2.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Instruction != IRInstruction.ShiftLeft64)
				return false;

			if (!context.Operand2.Definitions[0].Operand1.IsVirtualRegister)
				return false;

			if (!context.Operand2.Definitions[0].Operand2.IsResolvedConstant)
				return false;

			if (context.Operand2.Definitions[0].Operand2.ConstantUnsigned64 != 1)
				return false;

			if (context.Operand2.Definitions[0].Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Operand1.Definitions[0].Instruction != IRInstruction.MulSigned64)
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1, context.Operand1.Definitions[0].Operand1.Definitions[0].Operand2))
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1, context.Operand2.Definitions[0].Operand1.Definitions[0].Operand2))
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand2.Definitions[0].Operand1, context.Operand1.Definitions[0].Operand2.Definitions[0].Operand2))
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand2.Definitions[0].Operand1, context.Operand2.Definitions[0].Operand1.Definitions[0].Operand1))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var result = context.Result;

			var t1 = context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1;
			var t2 = context.Operand1.Definitions[0].Operand2.Definitions[0].Operand1;

			var v1 = transform.AllocateVirtualRegister(transform.I8);
			var v2 = transform.AllocateVirtualRegister(transform.I8);

			context.SetInstruction(IRInstruction.Sub64, v1, t1, t2);
			context.AppendInstruction(IRInstruction.Sub64, v2, t1, t2);
			context.AppendInstruction(IRInstruction.MulSigned64, result, v2, v1);
		}
	}

	/// <summary>
	/// Signed64AAPlusBBMinus2AB_v2
	/// </summary>
	public sealed class Signed64AAPlusBBMinus2AB_v2 : BaseTransformation
	{
		public Signed64AAPlusBBMinus2AB_v2() : base(IRInstruction.Sub64, TransformationType.Auto | TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			if (!context.Operand1.IsVirtualRegister)
				return false;

			if (!context.Operand2.IsVirtualRegister)
				return false;

			if (context.Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Instruction != IRInstruction.Add64)
				return false;

			if (!context.Operand1.Definitions[0].Operand1.IsVirtualRegister)
				return false;

			if (!context.Operand1.Definitions[0].Operand2.IsVirtualRegister)
				return false;

			if (context.Operand1.Definitions[0].Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Operand1.Definitions[0].Instruction != IRInstruction.MulSigned64)
				return false;

			if (context.Operand1.Definitions[0].Operand2.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Operand2.Definitions[0].Instruction != IRInstruction.MulSigned64)
				return false;

			if (context.Operand2.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Instruction != IRInstruction.ShiftLeft64)
				return false;

			if (!context.Operand2.Definitions[0].Operand1.IsVirtualRegister)
				return false;

			if (!context.Operand2.Definitions[0].Operand2.IsResolvedConstant)
				return false;

			if (context.Operand2.Definitions[0].Operand2.ConstantUnsigned64 != 1)
				return false;

			if (context.Operand2.Definitions[0].Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Operand1.Definitions[0].Instruction != IRInstruction.MulSigned64)
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1, context.Operand1.Definitions[0].Operand1.Definitions[0].Operand2))
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1, context.Operand2.Definitions[0].Operand1.Definitions[0].Operand2))
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand2.Definitions[0].Operand1, context.Operand1.Definitions[0].Operand2.Definitions[0].Operand2))
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand2.Definitions[0].Operand1, context.Operand2.Definitions[0].Operand1.Definitions[0].Operand1))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var result = context.Result;

			var t1 = context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1;
			var t2 = context.Operand1.Definitions[0].Operand2.Definitions[0].Operand1;

			var v1 = transform.AllocateVirtualRegister(transform.I8);
			var v2 = transform.AllocateVirtualRegister(transform.I8);

			context.SetInstruction(IRInstruction.Sub64, v1, t2, t1);
			context.AppendInstruction(IRInstruction.Sub64, v2, t2, t1);
			context.AppendInstruction(IRInstruction.MulSigned64, result, v2, v1);
		}
	}

	/// <summary>
	/// Signed64AAPlusBBMinus2AB_v3
	/// </summary>
	public sealed class Signed64AAPlusBBMinus2AB_v3 : BaseTransformation
	{
		public Signed64AAPlusBBMinus2AB_v3() : base(IRInstruction.Sub64, TransformationType.Auto | TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			if (!context.Operand1.IsVirtualRegister)
				return false;

			if (!context.Operand2.IsVirtualRegister)
				return false;

			if (context.Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Instruction != IRInstruction.Add64)
				return false;

			if (!context.Operand1.Definitions[0].Operand1.IsVirtualRegister)
				return false;

			if (!context.Operand1.Definitions[0].Operand2.IsVirtualRegister)
				return false;

			if (context.Operand1.Definitions[0].Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Operand1.Definitions[0].Instruction != IRInstruction.MulSigned64)
				return false;

			if (context.Operand1.Definitions[0].Operand2.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Operand2.Definitions[0].Instruction != IRInstruction.MulSigned64)
				return false;

			if (context.Operand2.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Instruction != IRInstruction.ShiftLeft64)
				return false;

			if (!context.Operand2.Definitions[0].Operand1.IsVirtualRegister)
				return false;

			if (!context.Operand2.Definitions[0].Operand2.IsResolvedConstant)
				return false;

			if (context.Operand2.Definitions[0].Operand2.ConstantUnsigned64 != 1)
				return false;

			if (context.Operand2.Definitions[0].Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Operand1.Definitions[0].Instruction != IRInstruction.MulSigned64)
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1, context.Operand1.Definitions[0].Operand1.Definitions[0].Operand2))
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1, context.Operand2.Definitions[0].Operand1.Definitions[0].Operand1))
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand2.Definitions[0].Operand1, context.Operand1.Definitions[0].Operand2.Definitions[0].Operand2))
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand2.Definitions[0].Operand1, context.Operand2.Definitions[0].Operand1.Definitions[0].Operand2))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var result = context.Result;

			var t1 = context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1;
			var t2 = context.Operand1.Definitions[0].Operand2.Definitions[0].Operand1;

			var v1 = transform.AllocateVirtualRegister(transform.I8);
			var v2 = transform.AllocateVirtualRegister(transform.I8);

			context.SetInstruction(IRInstruction.Sub64, v1, t2, t1);
			context.AppendInstruction(IRInstruction.Sub64, v2, t2, t1);
			context.AppendInstruction(IRInstruction.MulSigned64, result, v2, v1);
		}
	}
}
