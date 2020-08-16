// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transform.Auto.IR.ConstantFolding
{
	/// <summary>
	/// MulUnsigned32x2
	/// </summary>
	public sealed class MulUnsigned32x2 : BaseTransformation
	{
		public MulUnsigned32x2() : base(IRInstruction.MulUnsigned32)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand1.IsVirtualRegister)
				return false;

			if (context.Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Instruction != IRInstruction.MulUnsigned32)
				return false;

			if (!IsResolvedConstant(context.Operand1.Definitions[0].Operand2))
				return false;

			if (!IsResolvedConstant(context.Operand2))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1.Definitions[0].Operand1;
			var t2 = context.Operand1.Definitions[0].Operand2;
			var t3 = context.Operand2;

			var e1 = transformContext.CreateConstant(MulUnsigned32(To32(t2), To32(t3)));

			context.SetInstruction(IRInstruction.MulUnsigned32, result, t1, e1);
		}
	}

	/// <summary>
	/// MulUnsigned32x2v1
	/// </summary>
	public sealed class MulUnsigned32x2v1 : BaseTransformation
	{
		public MulUnsigned32x2v1() : base(IRInstruction.MulUnsigned32)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand2.IsVirtualRegister)
				return false;

			if (context.Operand2.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Instruction != IRInstruction.MulUnsigned32)
				return false;

			if (!IsResolvedConstant(context.Operand2.Definitions[0].Operand2))
				return false;

			if (!IsResolvedConstant(context.Operand1))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1;
			var t2 = context.Operand2.Definitions[0].Operand1;
			var t3 = context.Operand2.Definitions[0].Operand2;

			var e1 = transformContext.CreateConstant(MulUnsigned32(To32(t3), To32(t1)));

			context.SetInstruction(IRInstruction.MulUnsigned32, result, t2, e1);
		}
	}

	/// <summary>
	/// MulUnsigned32x2v2
	/// </summary>
	public sealed class MulUnsigned32x2v2 : BaseTransformation
	{
		public MulUnsigned32x2v2() : base(IRInstruction.MulUnsigned32)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand1.IsVirtualRegister)
				return false;

			if (context.Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Instruction != IRInstruction.MulUnsigned32)
				return false;

			if (!IsResolvedConstant(context.Operand1.Definitions[0].Operand1))
				return false;

			if (!IsResolvedConstant(context.Operand2))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1.Definitions[0].Operand1;
			var t2 = context.Operand1.Definitions[0].Operand2;
			var t3 = context.Operand2;

			var e1 = transformContext.CreateConstant(MulUnsigned32(To32(t1), To32(t3)));

			context.SetInstruction(IRInstruction.MulUnsigned32, result, t2, e1);
		}
	}

	/// <summary>
	/// MulUnsigned32x2v3
	/// </summary>
	public sealed class MulUnsigned32x2v3 : BaseTransformation
	{
		public MulUnsigned32x2v3() : base(IRInstruction.MulUnsigned32)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand2.IsVirtualRegister)
				return false;

			if (context.Operand2.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Instruction != IRInstruction.MulUnsigned32)
				return false;

			if (!IsResolvedConstant(context.Operand2.Definitions[0].Operand1))
				return false;

			if (!IsResolvedConstant(context.Operand1))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1;
			var t2 = context.Operand2.Definitions[0].Operand1;
			var t3 = context.Operand2.Definitions[0].Operand2;

			var e1 = transformContext.CreateConstant(MulUnsigned32(To32(t2), To32(t1)));

			context.SetInstruction(IRInstruction.MulUnsigned32, result, t3, e1);
		}
	}
}
