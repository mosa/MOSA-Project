// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.Algebraic;

/// <summary>
/// Unsigned32AAPlusBBPlus2AB
/// </summary>
[Transform("IR.Optimizations.Auto.Algebraic")]
public sealed class Unsigned32AAPlusBBPlus2AB : BaseTransform
{
	public Unsigned32AAPlusBBPlus2AB() : base(IRInstruction.Add32, TransformType.Auto | TransformType.Optimization)
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

		if (context.Operand1.Definitions[0].Instruction != IRInstruction.Add32)
			return false;

		if (!context.Operand1.Definitions[0].Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand1.Definitions[0].Operand2.IsVirtualRegister)
			return false;

		if (context.Operand1.Definitions[0].Operand1.Definitions.Count != 1)
			return false;

		if (context.Operand1.Definitions[0].Operand1.Definitions[0].Instruction != IRInstruction.MulUnsigned32)
			return false;

		if (context.Operand1.Definitions[0].Operand2.Definitions.Count != 1)
			return false;

		if (context.Operand1.Definitions[0].Operand2.Definitions[0].Instruction != IRInstruction.MulUnsigned32)
			return false;

		if (context.Operand2.Definitions.Count != 1)
			return false;

		if (context.Operand2.Definitions[0].Instruction != IRInstruction.ShiftLeft32)
			return false;

		if (!context.Operand2.Definitions[0].Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand2.Definitions[0].Operand2.IsResolvedConstant)
			return false;

		if (context.Operand2.Definitions[0].Operand2.ConstantUnsigned64 != 1)
			return false;

		if (context.Operand2.Definitions[0].Operand1.Definitions.Count != 1)
			return false;

		if (context.Operand2.Definitions[0].Operand1.Definitions[0].Instruction != IRInstruction.MulSigned32)
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

		var v1 = transform.AllocateVirtualRegister32();
		var v2 = transform.AllocateVirtualRegister32();

		context.SetInstruction(IRInstruction.Add32, v1, t1, t2);
		context.AppendInstruction(IRInstruction.Add32, v2, t1, t2);
		context.AppendInstruction(IRInstruction.MulUnsigned32, result, v2, v1);
	}
}

/// <summary>
/// Unsigned32AAPlusBBPlus2AB_v1
/// </summary>
[Transform("IR.Optimizations.Auto.Algebraic")]
public sealed class Unsigned32AAPlusBBPlus2AB_v1 : BaseTransform
{
	public Unsigned32AAPlusBBPlus2AB_v1() : base(IRInstruction.Add32, TransformType.Auto | TransformType.Optimization)
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

		if (context.Operand1.Definitions[0].Instruction != IRInstruction.ShiftLeft32)
			return false;

		if (!context.Operand1.Definitions[0].Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand1.Definitions[0].Operand2.IsResolvedConstant)
			return false;

		if (context.Operand1.Definitions[0].Operand2.ConstantUnsigned64 != 1)
			return false;

		if (context.Operand1.Definitions[0].Operand1.Definitions.Count != 1)
			return false;

		if (context.Operand1.Definitions[0].Operand1.Definitions[0].Instruction != IRInstruction.MulSigned32)
			return false;

		if (context.Operand2.Definitions.Count != 1)
			return false;

		if (context.Operand2.Definitions[0].Instruction != IRInstruction.Add32)
			return false;

		if (!context.Operand2.Definitions[0].Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand2.Definitions[0].Operand2.IsVirtualRegister)
			return false;

		if (context.Operand2.Definitions[0].Operand1.Definitions.Count != 1)
			return false;

		if (context.Operand2.Definitions[0].Operand1.Definitions[0].Instruction != IRInstruction.MulUnsigned32)
			return false;

		if (context.Operand2.Definitions[0].Operand2.Definitions.Count != 1)
			return false;

		if (context.Operand2.Definitions[0].Operand2.Definitions[0].Instruction != IRInstruction.MulUnsigned32)
			return false;

		if (!AreSame(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1, context.Operand2.Definitions[0].Operand1.Definitions[0].Operand1))
			return false;

		if (!AreSame(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1, context.Operand2.Definitions[0].Operand1.Definitions[0].Operand2))
			return false;

		if (!AreSame(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand2, context.Operand2.Definitions[0].Operand2.Definitions[0].Operand1))
			return false;

		if (!AreSame(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand2, context.Operand2.Definitions[0].Operand2.Definitions[0].Operand2))
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1;
		var t2 = context.Operand1.Definitions[0].Operand1.Definitions[0].Operand2;

		var v1 = transform.AllocateVirtualRegister32();
		var v2 = transform.AllocateVirtualRegister32();

		context.SetInstruction(IRInstruction.Add32, v1, t1, t2);
		context.AppendInstruction(IRInstruction.Add32, v2, t1, t2);
		context.AppendInstruction(IRInstruction.MulUnsigned32, result, v2, v1);
	}
}

/// <summary>
/// Unsigned32AAPlusBBPlus2AB_v2
/// </summary>
[Transform("IR.Optimizations.Auto.Algebraic")]
public sealed class Unsigned32AAPlusBBPlus2AB_v2 : BaseTransform
{
	public Unsigned32AAPlusBBPlus2AB_v2() : base(IRInstruction.Add32, TransformType.Auto | TransformType.Optimization)
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

		if (context.Operand1.Definitions[0].Instruction != IRInstruction.Add32)
			return false;

		if (!context.Operand1.Definitions[0].Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand1.Definitions[0].Operand2.IsVirtualRegister)
			return false;

		if (context.Operand1.Definitions[0].Operand1.Definitions.Count != 1)
			return false;

		if (context.Operand1.Definitions[0].Operand1.Definitions[0].Instruction != IRInstruction.MulUnsigned32)
			return false;

		if (context.Operand1.Definitions[0].Operand2.Definitions.Count != 1)
			return false;

		if (context.Operand1.Definitions[0].Operand2.Definitions[0].Instruction != IRInstruction.MulUnsigned32)
			return false;

		if (context.Operand2.Definitions.Count != 1)
			return false;

		if (context.Operand2.Definitions[0].Instruction != IRInstruction.ShiftLeft32)
			return false;

		if (!context.Operand2.Definitions[0].Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand2.Definitions[0].Operand2.IsResolvedConstant)
			return false;

		if (context.Operand2.Definitions[0].Operand2.ConstantUnsigned64 != 1)
			return false;

		if (context.Operand2.Definitions[0].Operand1.Definitions.Count != 1)
			return false;

		if (context.Operand2.Definitions[0].Operand1.Definitions[0].Instruction != IRInstruction.MulSigned32)
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

		var v1 = transform.AllocateVirtualRegister32();
		var v2 = transform.AllocateVirtualRegister32();

		context.SetInstruction(IRInstruction.Add32, v1, t1, t2);
		context.AppendInstruction(IRInstruction.Add32, v2, t1, t2);
		context.AppendInstruction(IRInstruction.MulUnsigned32, result, v2, v1);
	}
}

/// <summary>
/// Unsigned32AAPlusBBPlus2AB_v3
/// </summary>
[Transform("IR.Optimizations.Auto.Algebraic")]
public sealed class Unsigned32AAPlusBBPlus2AB_v3 : BaseTransform
{
	public Unsigned32AAPlusBBPlus2AB_v3() : base(IRInstruction.Add32, TransformType.Auto | TransformType.Optimization)
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

		if (context.Operand1.Definitions[0].Instruction != IRInstruction.ShiftLeft32)
			return false;

		if (!context.Operand1.Definitions[0].Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand1.Definitions[0].Operand2.IsResolvedConstant)
			return false;

		if (context.Operand1.Definitions[0].Operand2.ConstantUnsigned64 != 1)
			return false;

		if (context.Operand1.Definitions[0].Operand1.Definitions.Count != 1)
			return false;

		if (context.Operand1.Definitions[0].Operand1.Definitions[0].Instruction != IRInstruction.MulSigned32)
			return false;

		if (context.Operand2.Definitions.Count != 1)
			return false;

		if (context.Operand2.Definitions[0].Instruction != IRInstruction.Add32)
			return false;

		if (!context.Operand2.Definitions[0].Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand2.Definitions[0].Operand2.IsVirtualRegister)
			return false;

		if (context.Operand2.Definitions[0].Operand1.Definitions.Count != 1)
			return false;

		if (context.Operand2.Definitions[0].Operand1.Definitions[0].Instruction != IRInstruction.MulUnsigned32)
			return false;

		if (context.Operand2.Definitions[0].Operand2.Definitions.Count != 1)
			return false;

		if (context.Operand2.Definitions[0].Operand2.Definitions[0].Instruction != IRInstruction.MulUnsigned32)
			return false;

		if (!AreSame(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1, context.Operand2.Definitions[0].Operand2.Definitions[0].Operand1))
			return false;

		if (!AreSame(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1, context.Operand2.Definitions[0].Operand2.Definitions[0].Operand2))
			return false;

		if (!AreSame(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand2, context.Operand2.Definitions[0].Operand1.Definitions[0].Operand1))
			return false;

		if (!AreSame(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand2, context.Operand2.Definitions[0].Operand1.Definitions[0].Operand2))
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1;
		var t2 = context.Operand1.Definitions[0].Operand1.Definitions[0].Operand2;

		var v1 = transform.AllocateVirtualRegister32();
		var v2 = transform.AllocateVirtualRegister32();

		context.SetInstruction(IRInstruction.Add32, v1, t2, t1);
		context.AppendInstruction(IRInstruction.Add32, v2, t2, t1);
		context.AppendInstruction(IRInstruction.MulUnsigned32, result, v2, v1);
	}
}

/// <summary>
/// Unsigned32AAPlusBBPlus2AB_v4
/// </summary>
[Transform("IR.Optimizations.Auto.Algebraic")]
public sealed class Unsigned32AAPlusBBPlus2AB_v4 : BaseTransform
{
	public Unsigned32AAPlusBBPlus2AB_v4() : base(IRInstruction.Add32, TransformType.Auto | TransformType.Optimization)
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

		if (context.Operand1.Definitions[0].Instruction != IRInstruction.Add32)
			return false;

		if (!context.Operand1.Definitions[0].Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand1.Definitions[0].Operand2.IsVirtualRegister)
			return false;

		if (context.Operand1.Definitions[0].Operand1.Definitions.Count != 1)
			return false;

		if (context.Operand1.Definitions[0].Operand1.Definitions[0].Instruction != IRInstruction.MulUnsigned32)
			return false;

		if (context.Operand1.Definitions[0].Operand2.Definitions.Count != 1)
			return false;

		if (context.Operand1.Definitions[0].Operand2.Definitions[0].Instruction != IRInstruction.MulUnsigned32)
			return false;

		if (context.Operand2.Definitions.Count != 1)
			return false;

		if (context.Operand2.Definitions[0].Instruction != IRInstruction.ShiftLeft32)
			return false;

		if (!context.Operand2.Definitions[0].Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand2.Definitions[0].Operand2.IsResolvedConstant)
			return false;

		if (context.Operand2.Definitions[0].Operand2.ConstantUnsigned64 != 1)
			return false;

		if (context.Operand2.Definitions[0].Operand1.Definitions.Count != 1)
			return false;

		if (context.Operand2.Definitions[0].Operand1.Definitions[0].Instruction != IRInstruction.MulSigned32)
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

		var v1 = transform.AllocateVirtualRegister32();
		var v2 = transform.AllocateVirtualRegister32();

		context.SetInstruction(IRInstruction.Add32, v1, t2, t1);
		context.AppendInstruction(IRInstruction.Add32, v2, t2, t1);
		context.AppendInstruction(IRInstruction.MulUnsigned32, result, v2, v1);
	}
}

/// <summary>
/// Unsigned32AAPlusBBPlus2AB_v5
/// </summary>
[Transform("IR.Optimizations.Auto.Algebraic")]
public sealed class Unsigned32AAPlusBBPlus2AB_v5 : BaseTransform
{
	public Unsigned32AAPlusBBPlus2AB_v5() : base(IRInstruction.Add32, TransformType.Auto | TransformType.Optimization)
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

		if (context.Operand1.Definitions[0].Instruction != IRInstruction.ShiftLeft32)
			return false;

		if (!context.Operand1.Definitions[0].Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand1.Definitions[0].Operand2.IsResolvedConstant)
			return false;

		if (context.Operand1.Definitions[0].Operand2.ConstantUnsigned64 != 1)
			return false;

		if (context.Operand1.Definitions[0].Operand1.Definitions.Count != 1)
			return false;

		if (context.Operand1.Definitions[0].Operand1.Definitions[0].Instruction != IRInstruction.MulSigned32)
			return false;

		if (context.Operand2.Definitions.Count != 1)
			return false;

		if (context.Operand2.Definitions[0].Instruction != IRInstruction.Add32)
			return false;

		if (!context.Operand2.Definitions[0].Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand2.Definitions[0].Operand2.IsVirtualRegister)
			return false;

		if (context.Operand2.Definitions[0].Operand1.Definitions.Count != 1)
			return false;

		if (context.Operand2.Definitions[0].Operand1.Definitions[0].Instruction != IRInstruction.MulUnsigned32)
			return false;

		if (context.Operand2.Definitions[0].Operand2.Definitions.Count != 1)
			return false;

		if (context.Operand2.Definitions[0].Operand2.Definitions[0].Instruction != IRInstruction.MulUnsigned32)
			return false;

		if (!AreSame(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1, context.Operand2.Definitions[0].Operand2.Definitions[0].Operand1))
			return false;

		if (!AreSame(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1, context.Operand2.Definitions[0].Operand2.Definitions[0].Operand2))
			return false;

		if (!AreSame(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand2, context.Operand2.Definitions[0].Operand1.Definitions[0].Operand1))
			return false;

		if (!AreSame(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand2, context.Operand2.Definitions[0].Operand1.Definitions[0].Operand2))
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1;
		var t2 = context.Operand1.Definitions[0].Operand1.Definitions[0].Operand2;

		var v1 = transform.AllocateVirtualRegister32();
		var v2 = transform.AllocateVirtualRegister32();

		context.SetInstruction(IRInstruction.Add32, v1, t1, t2);
		context.AppendInstruction(IRInstruction.Add32, v2, t1, t2);
		context.AppendInstruction(IRInstruction.MulUnsigned32, result, v2, v1);
	}
}

/// <summary>
/// Unsigned32AAPlusBBPlus2AB_v6
/// </summary>
[Transform("IR.Optimizations.Auto.Algebraic")]
public sealed class Unsigned32AAPlusBBPlus2AB_v6 : BaseTransform
{
	public Unsigned32AAPlusBBPlus2AB_v6() : base(IRInstruction.Add32, TransformType.Auto | TransformType.Optimization)
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

		if (context.Operand1.Definitions[0].Instruction != IRInstruction.Add32)
			return false;

		if (!context.Operand1.Definitions[0].Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand1.Definitions[0].Operand2.IsVirtualRegister)
			return false;

		if (context.Operand1.Definitions[0].Operand1.Definitions.Count != 1)
			return false;

		if (context.Operand1.Definitions[0].Operand1.Definitions[0].Instruction != IRInstruction.MulUnsigned32)
			return false;

		if (context.Operand1.Definitions[0].Operand2.Definitions.Count != 1)
			return false;

		if (context.Operand1.Definitions[0].Operand2.Definitions[0].Instruction != IRInstruction.MulUnsigned32)
			return false;

		if (context.Operand2.Definitions.Count != 1)
			return false;

		if (context.Operand2.Definitions[0].Instruction != IRInstruction.ShiftLeft32)
			return false;

		if (!context.Operand2.Definitions[0].Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand2.Definitions[0].Operand2.IsResolvedConstant)
			return false;

		if (context.Operand2.Definitions[0].Operand2.ConstantUnsigned64 != 1)
			return false;

		if (context.Operand2.Definitions[0].Operand1.Definitions.Count != 1)
			return false;

		if (context.Operand2.Definitions[0].Operand1.Definitions[0].Instruction != IRInstruction.MulSigned32)
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

		var v1 = transform.AllocateVirtualRegister32();
		var v2 = transform.AllocateVirtualRegister32();

		context.SetInstruction(IRInstruction.Add32, v1, t2, t1);
		context.AppendInstruction(IRInstruction.Add32, v2, t2, t1);
		context.AppendInstruction(IRInstruction.MulUnsigned32, result, v2, v1);
	}
}

/// <summary>
/// Unsigned32AAPlusBBPlus2AB_v7
/// </summary>
[Transform("IR.Optimizations.Auto.Algebraic")]
public sealed class Unsigned32AAPlusBBPlus2AB_v7 : BaseTransform
{
	public Unsigned32AAPlusBBPlus2AB_v7() : base(IRInstruction.Add32, TransformType.Auto | TransformType.Optimization)
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

		if (context.Operand1.Definitions[0].Instruction != IRInstruction.ShiftLeft32)
			return false;

		if (!context.Operand1.Definitions[0].Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand1.Definitions[0].Operand2.IsResolvedConstant)
			return false;

		if (context.Operand1.Definitions[0].Operand2.ConstantUnsigned64 != 1)
			return false;

		if (context.Operand1.Definitions[0].Operand1.Definitions.Count != 1)
			return false;

		if (context.Operand1.Definitions[0].Operand1.Definitions[0].Instruction != IRInstruction.MulSigned32)
			return false;

		if (context.Operand2.Definitions.Count != 1)
			return false;

		if (context.Operand2.Definitions[0].Instruction != IRInstruction.Add32)
			return false;

		if (!context.Operand2.Definitions[0].Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand2.Definitions[0].Operand2.IsVirtualRegister)
			return false;

		if (context.Operand2.Definitions[0].Operand1.Definitions.Count != 1)
			return false;

		if (context.Operand2.Definitions[0].Operand1.Definitions[0].Instruction != IRInstruction.MulUnsigned32)
			return false;

		if (context.Operand2.Definitions[0].Operand2.Definitions.Count != 1)
			return false;

		if (context.Operand2.Definitions[0].Operand2.Definitions[0].Instruction != IRInstruction.MulUnsigned32)
			return false;

		if (!AreSame(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1, context.Operand2.Definitions[0].Operand1.Definitions[0].Operand1))
			return false;

		if (!AreSame(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1, context.Operand2.Definitions[0].Operand1.Definitions[0].Operand2))
			return false;

		if (!AreSame(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand2, context.Operand2.Definitions[0].Operand2.Definitions[0].Operand1))
			return false;

		if (!AreSame(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand2, context.Operand2.Definitions[0].Operand2.Definitions[0].Operand2))
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1;
		var t2 = context.Operand1.Definitions[0].Operand1.Definitions[0].Operand2;

		var v1 = transform.AllocateVirtualRegister32();
		var v2 = transform.AllocateVirtualRegister32();

		context.SetInstruction(IRInstruction.Add32, v1, t2, t1);
		context.AppendInstruction(IRInstruction.Add32, v2, t2, t1);
		context.AppendInstruction(IRInstruction.MulUnsigned32, result, v2, v1);
	}
}
