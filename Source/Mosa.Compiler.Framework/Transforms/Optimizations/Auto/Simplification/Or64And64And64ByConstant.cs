// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.Simplification;

/// <summary>
/// Or64And64And64ByConstant
/// </summary>
[Transform("IR.Optimizations.Auto.Simplification")]
public sealed class Or64And64And64ByConstant : BaseTransform
{
	public Or64And64And64ByConstant() : base(Framework.IR.Or64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != Framework.IR.And64)
			return false;

		if (!context.Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Instruction != Framework.IR.And64)
			return false;

		if (!AreSame(context.Operand1.Definitions[0].Operand2, context.Operand2.Definitions[0].Operand2))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand1;
		var t2 = context.Operand1.Definitions[0].Operand2;
		var t3 = context.Operand2.Definitions[0].Operand1;

		var v1 = transform.VirtualRegisters.Allocate64();

		context.SetInstruction(Framework.IR.Or64, v1, t1, t3);
		context.AppendInstruction(Framework.IR.And64, result, v1, t2);
	}
}

/// <summary>
/// Or64And64And64ByConstant_v1
/// </summary>
[Transform("IR.Optimizations.Auto.Simplification")]
public sealed class Or64And64And64ByConstant_v1 : BaseTransform
{
	public Or64And64And64ByConstant_v1() : base(Framework.IR.Or64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != Framework.IR.And64)
			return false;

		if (!context.Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Instruction != Framework.IR.And64)
			return false;

		if (!AreSame(context.Operand1.Definitions[0].Operand2, context.Operand2.Definitions[0].Operand2))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand1;
		var t2 = context.Operand1.Definitions[0].Operand2;
		var t3 = context.Operand2.Definitions[0].Operand1;

		var v1 = transform.VirtualRegisters.Allocate64();

		context.SetInstruction(Framework.IR.Or64, v1, t3, t1);
		context.AppendInstruction(Framework.IR.And64, result, v1, t2);
	}
}

/// <summary>
/// Or64And64And64ByConstant_v2
/// </summary>
[Transform("IR.Optimizations.Auto.Simplification")]
public sealed class Or64And64And64ByConstant_v2 : BaseTransform
{
	public Or64And64And64ByConstant_v2() : base(Framework.IR.Or64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != Framework.IR.And64)
			return false;

		if (!context.Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Instruction != Framework.IR.And64)
			return false;

		if (!AreSame(context.Operand1.Definitions[0].Operand2, context.Operand2.Definitions[0].Operand1))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand1;
		var t2 = context.Operand1.Definitions[0].Operand2;
		var t3 = context.Operand2.Definitions[0].Operand2;

		var v1 = transform.VirtualRegisters.Allocate64();

		context.SetInstruction(Framework.IR.Or64, v1, t1, t3);
		context.AppendInstruction(Framework.IR.And64, result, v1, t2);
	}
}

/// <summary>
/// Or64And64And64ByConstant_v3
/// </summary>
[Transform("IR.Optimizations.Auto.Simplification")]
public sealed class Or64And64And64ByConstant_v3 : BaseTransform
{
	public Or64And64And64ByConstant_v3() : base(Framework.IR.Or64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != Framework.IR.And64)
			return false;

		if (!context.Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Instruction != Framework.IR.And64)
			return false;

		if (!AreSame(context.Operand1.Definitions[0].Operand1, context.Operand2.Definitions[0].Operand2))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand1;
		var t2 = context.Operand1.Definitions[0].Operand2;
		var t3 = context.Operand2.Definitions[0].Operand1;

		var v1 = transform.VirtualRegisters.Allocate64();

		context.SetInstruction(Framework.IR.Or64, v1, t3, t2);
		context.AppendInstruction(Framework.IR.And64, result, v1, t1);
	}
}

/// <summary>
/// Or64And64And64ByConstant_v4
/// </summary>
[Transform("IR.Optimizations.Auto.Simplification")]
public sealed class Or64And64And64ByConstant_v4 : BaseTransform
{
	public Or64And64And64ByConstant_v4() : base(Framework.IR.Or64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != Framework.IR.And64)
			return false;

		if (!context.Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Instruction != Framework.IR.And64)
			return false;

		if (!AreSame(context.Operand1.Definitions[0].Operand1, context.Operand2.Definitions[0].Operand2))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand1;
		var t2 = context.Operand1.Definitions[0].Operand2;
		var t3 = context.Operand2.Definitions[0].Operand1;

		var v1 = transform.VirtualRegisters.Allocate64();

		context.SetInstruction(Framework.IR.Or64, v1, t2, t3);
		context.AppendInstruction(Framework.IR.And64, result, v1, t1);
	}
}

/// <summary>
/// Or64And64And64ByConstant_v5
/// </summary>
[Transform("IR.Optimizations.Auto.Simplification")]
public sealed class Or64And64And64ByConstant_v5 : BaseTransform
{
	public Or64And64And64ByConstant_v5() : base(Framework.IR.Or64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != Framework.IR.And64)
			return false;

		if (!context.Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Instruction != Framework.IR.And64)
			return false;

		if (!AreSame(context.Operand1.Definitions[0].Operand2, context.Operand2.Definitions[0].Operand1))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand1;
		var t2 = context.Operand1.Definitions[0].Operand2;
		var t3 = context.Operand2.Definitions[0].Operand2;

		var v1 = transform.VirtualRegisters.Allocate64();

		context.SetInstruction(Framework.IR.Or64, v1, t3, t1);
		context.AppendInstruction(Framework.IR.And64, result, v1, t2);
	}
}

/// <summary>
/// Or64And64And64ByConstant_v6
/// </summary>
[Transform("IR.Optimizations.Auto.Simplification")]
public sealed class Or64And64And64ByConstant_v6 : BaseTransform
{
	public Or64And64And64ByConstant_v6() : base(Framework.IR.Or64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != Framework.IR.And64)
			return false;

		if (!context.Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Instruction != Framework.IR.And64)
			return false;

		if (!AreSame(context.Operand1.Definitions[0].Operand1, context.Operand2.Definitions[0].Operand1))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand1;
		var t2 = context.Operand1.Definitions[0].Operand2;
		var t3 = context.Operand2.Definitions[0].Operand2;

		var v1 = transform.VirtualRegisters.Allocate64();

		context.SetInstruction(Framework.IR.Or64, v1, t2, t3);
		context.AppendInstruction(Framework.IR.And64, result, v1, t1);
	}
}

/// <summary>
/// Or64And64And64ByConstant_v7
/// </summary>
[Transform("IR.Optimizations.Auto.Simplification")]
public sealed class Or64And64And64ByConstant_v7 : BaseTransform
{
	public Or64And64And64ByConstant_v7() : base(Framework.IR.Or64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != Framework.IR.And64)
			return false;

		if (!context.Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Instruction != Framework.IR.And64)
			return false;

		if (!AreSame(context.Operand1.Definitions[0].Operand1, context.Operand2.Definitions[0].Operand1))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand1;
		var t2 = context.Operand1.Definitions[0].Operand2;
		var t3 = context.Operand2.Definitions[0].Operand2;

		var v1 = transform.VirtualRegisters.Allocate64();

		context.SetInstruction(Framework.IR.Or64, v1, t3, t2);
		context.AppendInstruction(Framework.IR.And64, result, v1, t1);
	}
}
