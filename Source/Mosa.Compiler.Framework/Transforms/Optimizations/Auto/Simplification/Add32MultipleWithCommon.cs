// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.Simplification
{
	/// <summary>
	/// Add32MultipleWithCommon
	/// </summary>
	public sealed class Add32MultipleWithCommon : BaseTransform
	{
		public Add32MultipleWithCommon() : base(IRInstruction.Add32, TransformType.Auto | TransformType.Optimization)
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

			if (context.Operand1.Definitions[0].Instruction != IRInstruction.MulUnsigned32)
				return false;

			if (context.Operand2.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Instruction != IRInstruction.MulUnsigned32)
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand1, context.Operand2.Definitions[0].Operand1))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var result = context.Result;

			var t1 = context.Operand1.Definitions[0].Operand1;
			var t2 = context.Operand1.Definitions[0].Operand2;
			var t3 = context.Operand2.Definitions[0].Operand2;

			var v1 = transform.AllocateVirtualRegister(transform.I4);

			context.SetInstruction(IRInstruction.Add32, v1, t2, t3);
			context.AppendInstruction(IRInstruction.MulUnsigned32, result, t1, v1);
		}
	}

	/// <summary>
	/// Add32MultipleWithCommon_v1
	/// </summary>
	public sealed class Add32MultipleWithCommon_v1 : BaseTransform
	{
		public Add32MultipleWithCommon_v1() : base(IRInstruction.Add32, TransformType.Auto | TransformType.Optimization)
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

			if (context.Operand1.Definitions[0].Instruction != IRInstruction.MulUnsigned32)
				return false;

			if (context.Operand2.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Instruction != IRInstruction.MulUnsigned32)
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand1, context.Operand2.Definitions[0].Operand1))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var result = context.Result;

			var t1 = context.Operand1.Definitions[0].Operand1;
			var t2 = context.Operand1.Definitions[0].Operand2;
			var t3 = context.Operand2.Definitions[0].Operand2;

			var v1 = transform.AllocateVirtualRegister(transform.I4);

			context.SetInstruction(IRInstruction.Add32, v1, t3, t2);
			context.AppendInstruction(IRInstruction.MulUnsigned32, result, t1, v1);
		}
	}

	/// <summary>
	/// Add32MultipleWithCommon_v2
	/// </summary>
	public sealed class Add32MultipleWithCommon_v2 : BaseTransform
	{
		public Add32MultipleWithCommon_v2() : base(IRInstruction.Add32, TransformType.Auto | TransformType.Optimization)
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

			if (context.Operand1.Definitions[0].Instruction != IRInstruction.MulUnsigned32)
				return false;

			if (context.Operand2.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Instruction != IRInstruction.MulUnsigned32)
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand1, context.Operand2.Definitions[0].Operand2))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var result = context.Result;

			var t1 = context.Operand1.Definitions[0].Operand1;
			var t2 = context.Operand1.Definitions[0].Operand2;
			var t3 = context.Operand2.Definitions[0].Operand1;

			var v1 = transform.AllocateVirtualRegister(transform.I4);

			context.SetInstruction(IRInstruction.Add32, v1, t2, t3);
			context.AppendInstruction(IRInstruction.MulUnsigned32, result, t1, v1);
		}
	}

	/// <summary>
	/// Add32MultipleWithCommon_v3
	/// </summary>
	public sealed class Add32MultipleWithCommon_v3 : BaseTransform
	{
		public Add32MultipleWithCommon_v3() : base(IRInstruction.Add32, TransformType.Auto | TransformType.Optimization)
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

			if (context.Operand1.Definitions[0].Instruction != IRInstruction.MulUnsigned32)
				return false;

			if (context.Operand2.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Instruction != IRInstruction.MulUnsigned32)
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand2, context.Operand2.Definitions[0].Operand1))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var result = context.Result;

			var t1 = context.Operand1.Definitions[0].Operand1;
			var t2 = context.Operand1.Definitions[0].Operand2;
			var t3 = context.Operand2.Definitions[0].Operand2;

			var v1 = transform.AllocateVirtualRegister(transform.I4);

			context.SetInstruction(IRInstruction.Add32, v1, t3, t1);
			context.AppendInstruction(IRInstruction.MulUnsigned32, result, t2, v1);
		}
	}

	/// <summary>
	/// Add32MultipleWithCommon_v4
	/// </summary>
	public sealed class Add32MultipleWithCommon_v4 : BaseTransform
	{
		public Add32MultipleWithCommon_v4() : base(IRInstruction.Add32, TransformType.Auto | TransformType.Optimization)
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

			if (context.Operand1.Definitions[0].Instruction != IRInstruction.MulUnsigned32)
				return false;

			if (context.Operand2.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Instruction != IRInstruction.MulUnsigned32)
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand2, context.Operand2.Definitions[0].Operand1))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var result = context.Result;

			var t1 = context.Operand1.Definitions[0].Operand1;
			var t2 = context.Operand1.Definitions[0].Operand2;
			var t3 = context.Operand2.Definitions[0].Operand2;

			var v1 = transform.AllocateVirtualRegister(transform.I4);

			context.SetInstruction(IRInstruction.Add32, v1, t1, t3);
			context.AppendInstruction(IRInstruction.MulUnsigned32, result, t2, v1);
		}
	}

	/// <summary>
	/// Add32MultipleWithCommon_v5
	/// </summary>
	public sealed class Add32MultipleWithCommon_v5 : BaseTransform
	{
		public Add32MultipleWithCommon_v5() : base(IRInstruction.Add32, TransformType.Auto | TransformType.Optimization)
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

			if (context.Operand1.Definitions[0].Instruction != IRInstruction.MulUnsigned32)
				return false;

			if (context.Operand2.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Instruction != IRInstruction.MulUnsigned32)
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand1, context.Operand2.Definitions[0].Operand2))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var result = context.Result;

			var t1 = context.Operand1.Definitions[0].Operand1;
			var t2 = context.Operand1.Definitions[0].Operand2;
			var t3 = context.Operand2.Definitions[0].Operand1;

			var v1 = transform.AllocateVirtualRegister(transform.I4);

			context.SetInstruction(IRInstruction.Add32, v1, t3, t2);
			context.AppendInstruction(IRInstruction.MulUnsigned32, result, t1, v1);
		}
	}

	/// <summary>
	/// Add32MultipleWithCommon_v6
	/// </summary>
	public sealed class Add32MultipleWithCommon_v6 : BaseTransform
	{
		public Add32MultipleWithCommon_v6() : base(IRInstruction.Add32, TransformType.Auto | TransformType.Optimization)
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

			if (context.Operand1.Definitions[0].Instruction != IRInstruction.MulUnsigned32)
				return false;

			if (context.Operand2.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Instruction != IRInstruction.MulUnsigned32)
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand2, context.Operand2.Definitions[0].Operand2))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var result = context.Result;

			var t1 = context.Operand1.Definitions[0].Operand1;
			var t2 = context.Operand1.Definitions[0].Operand2;
			var t3 = context.Operand2.Definitions[0].Operand1;

			var v1 = transform.AllocateVirtualRegister(transform.I4);

			context.SetInstruction(IRInstruction.Add32, v1, t1, t3);
			context.AppendInstruction(IRInstruction.MulUnsigned32, result, t2, v1);
		}
	}

	/// <summary>
	/// Add32MultipleWithCommon_v7
	/// </summary>
	public sealed class Add32MultipleWithCommon_v7 : BaseTransform
	{
		public Add32MultipleWithCommon_v7() : base(IRInstruction.Add32, TransformType.Auto | TransformType.Optimization)
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

			if (context.Operand1.Definitions[0].Instruction != IRInstruction.MulUnsigned32)
				return false;

			if (context.Operand2.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Instruction != IRInstruction.MulUnsigned32)
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand2, context.Operand2.Definitions[0].Operand2))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var result = context.Result;

			var t1 = context.Operand1.Definitions[0].Operand1;
			var t2 = context.Operand1.Definitions[0].Operand2;
			var t3 = context.Operand2.Definitions[0].Operand1;

			var v1 = transform.AllocateVirtualRegister(transform.I4);

			context.SetInstruction(IRInstruction.Add32, v1, t3, t1);
			context.AppendInstruction(IRInstruction.MulUnsigned32, result, t2, v1);
		}
	}
}
