// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.Algebraic;

/// <summary>
/// Unsigned64PerfectSquareFormula
/// </summary>
[Transform("IR.Optimizations.Auto.Algebraic")]
public sealed class Unsigned64PerfectSquareFormula : BaseTransform
{
	public Unsigned64PerfectSquareFormula() : base(IRInstruction.Add64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != IRInstruction.Add64)
			return false;

		if (!context.Operand1.Definitions[0].Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand1.Definitions[0].Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand1.Definitions[0].Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Operand1.Definitions[0].Instruction != IRInstruction.MulUnsigned64)
			return false;

		if (!context.Operand1.Definitions[0].Operand2.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Operand2.Definitions[0].Instruction != IRInstruction.MulUnsigned64)
			return false;

		if (!AreSame(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1, context.Operand1.Definitions[0].Operand1.Definitions[0].Operand2))
			return false;

		if (!AreSame(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1, context.Operand1.Definitions[0].Operand2.Definitions[0].Operand2))
			return false;

		if (!IsResolvedConstant(context.Operand2))
			return false;

		if (!IsResolvedConstant(context.Operand1.Definitions[0].Operand2.Definitions[0].Operand1))
			return false;

		if (!IsEvenInteger(context.Operand1.Definitions[0].Operand2.Definitions[0].Operand1))
			return false;

		if (!IsEqual(To64(context.Operand2), Square64(DivUnsigned64(To64(context.Operand1.Definitions[0].Operand2.Definitions[0].Operand1), 2))))
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1;
		var t2 = context.Operand1.Definitions[0].Operand2.Definitions[0].Operand1;

		var v1 = transform.VirtualRegisters.Allocate64();
		var v2 = transform.VirtualRegisters.Allocate64();

		var e1 = Operand.CreateConstant(DivUnsigned64(To64(t2), 2));

		context.SetInstruction(IRInstruction.Add64, v1, t1, e1);
		context.AppendInstruction(IRInstruction.Add64, v2, t1, e1);
		context.AppendInstruction(IRInstruction.MulUnsigned64, result, v2, v1);
	}
}

/// <summary>
/// Unsigned64PerfectSquareFormula_v1
/// </summary>
[Transform("IR.Optimizations.Auto.Algebraic")]
public sealed class Unsigned64PerfectSquareFormula_v1 : BaseTransform
{
	public Unsigned64PerfectSquareFormula_v1() : base(IRInstruction.Add64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		if (!context.Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Instruction != IRInstruction.Add64)
			return false;

		if (!context.Operand2.Definitions[0].Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand2.Definitions[0].Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand2.Definitions[0].Operand1.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Operand1.Definitions[0].Instruction != IRInstruction.MulUnsigned64)
			return false;

		if (!context.Operand2.Definitions[0].Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Operand2.Definitions[0].Instruction != IRInstruction.MulUnsigned64)
			return false;

		if (!AreSame(context.Operand2.Definitions[0].Operand1.Definitions[0].Operand1, context.Operand2.Definitions[0].Operand1.Definitions[0].Operand2))
			return false;

		if (!AreSame(context.Operand2.Definitions[0].Operand1.Definitions[0].Operand1, context.Operand2.Definitions[0].Operand2.Definitions[0].Operand2))
			return false;

		if (!IsResolvedConstant(context.Operand1))
			return false;

		if (!IsResolvedConstant(context.Operand2.Definitions[0].Operand2.Definitions[0].Operand1))
			return false;

		if (!IsEvenInteger(context.Operand2.Definitions[0].Operand2.Definitions[0].Operand1))
			return false;

		if (!IsEqual(To64(context.Operand1), Square64(DivUnsigned64(To64(context.Operand2.Definitions[0].Operand2.Definitions[0].Operand1), 2))))
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;

		var t1 = context.Operand2.Definitions[0].Operand1.Definitions[0].Operand1;
		var t2 = context.Operand2.Definitions[0].Operand2.Definitions[0].Operand1;

		var v1 = transform.VirtualRegisters.Allocate64();
		var v2 = transform.VirtualRegisters.Allocate64();

		var e1 = Operand.CreateConstant(DivUnsigned64(To64(t2), 2));

		context.SetInstruction(IRInstruction.Add64, v1, t1, e1);
		context.AppendInstruction(IRInstruction.Add64, v2, t1, e1);
		context.AppendInstruction(IRInstruction.MulUnsigned64, result, v2, v1);
	}
}

/// <summary>
/// Unsigned64PerfectSquareFormula_v2
/// </summary>
[Transform("IR.Optimizations.Auto.Algebraic")]
public sealed class Unsigned64PerfectSquareFormula_v2 : BaseTransform
{
	public Unsigned64PerfectSquareFormula_v2() : base(IRInstruction.Add64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != IRInstruction.Add64)
			return false;

		if (!context.Operand1.Definitions[0].Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand1.Definitions[0].Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand1.Definitions[0].Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Operand1.Definitions[0].Instruction != IRInstruction.MulUnsigned64)
			return false;

		if (!context.Operand1.Definitions[0].Operand2.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Operand2.Definitions[0].Instruction != IRInstruction.MulUnsigned64)
			return false;

		if (!AreSame(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand2, context.Operand1.Definitions[0].Operand2.Definitions[0].Operand1))
			return false;

		if (!AreSame(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand2, context.Operand1.Definitions[0].Operand2.Definitions[0].Operand2))
			return false;

		if (!IsResolvedConstant(context.Operand2))
			return false;

		if (!IsResolvedConstant(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1))
			return false;

		if (!IsEvenInteger(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1))
			return false;

		if (!IsEqual(To64(context.Operand2), Square64(DivUnsigned64(To64(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1), 2))))
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1;
		var t2 = context.Operand1.Definitions[0].Operand1.Definitions[0].Operand2;

		var v1 = transform.VirtualRegisters.Allocate64();
		var v2 = transform.VirtualRegisters.Allocate64();

		var e1 = Operand.CreateConstant(DivUnsigned64(To64(t1), 2));

		context.SetInstruction(IRInstruction.Add64, v1, t2, e1);
		context.AppendInstruction(IRInstruction.Add64, v2, t2, e1);
		context.AppendInstruction(IRInstruction.MulUnsigned64, result, v2, v1);
	}
}

/// <summary>
/// Unsigned64PerfectSquareFormula_v3
/// </summary>
[Transform("IR.Optimizations.Auto.Algebraic")]
public sealed class Unsigned64PerfectSquareFormula_v3 : BaseTransform
{
	public Unsigned64PerfectSquareFormula_v3() : base(IRInstruction.Add64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		if (!context.Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Instruction != IRInstruction.Add64)
			return false;

		if (!context.Operand2.Definitions[0].Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand2.Definitions[0].Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand2.Definitions[0].Operand1.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Operand1.Definitions[0].Instruction != IRInstruction.MulUnsigned64)
			return false;

		if (!context.Operand2.Definitions[0].Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Operand2.Definitions[0].Instruction != IRInstruction.MulUnsigned64)
			return false;

		if (!AreSame(context.Operand2.Definitions[0].Operand1.Definitions[0].Operand2, context.Operand2.Definitions[0].Operand2.Definitions[0].Operand1))
			return false;

		if (!AreSame(context.Operand2.Definitions[0].Operand1.Definitions[0].Operand2, context.Operand2.Definitions[0].Operand2.Definitions[0].Operand2))
			return false;

		if (!IsResolvedConstant(context.Operand1))
			return false;

		if (!IsResolvedConstant(context.Operand2.Definitions[0].Operand1.Definitions[0].Operand1))
			return false;

		if (!IsEvenInteger(context.Operand2.Definitions[0].Operand1.Definitions[0].Operand1))
			return false;

		if (!IsEqual(To64(context.Operand1), Square64(DivUnsigned64(To64(context.Operand2.Definitions[0].Operand1.Definitions[0].Operand1), 2))))
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;

		var t1 = context.Operand2.Definitions[0].Operand1.Definitions[0].Operand1;
		var t2 = context.Operand2.Definitions[0].Operand1.Definitions[0].Operand2;

		var v1 = transform.VirtualRegisters.Allocate64();
		var v2 = transform.VirtualRegisters.Allocate64();

		var e1 = Operand.CreateConstant(DivUnsigned64(To64(t1), 2));

		context.SetInstruction(IRInstruction.Add64, v1, t2, e1);
		context.AppendInstruction(IRInstruction.Add64, v2, t2, e1);
		context.AppendInstruction(IRInstruction.MulUnsigned64, result, v2, v1);
	}
}

/// <summary>
/// Unsigned64PerfectSquareFormula_v4
/// </summary>
[Transform("IR.Optimizations.Auto.Algebraic")]
public sealed class Unsigned64PerfectSquareFormula_v4 : BaseTransform
{
	public Unsigned64PerfectSquareFormula_v4() : base(IRInstruction.Add64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != IRInstruction.Add64)
			return false;

		if (!context.Operand1.Definitions[0].Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand1.Definitions[0].Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand1.Definitions[0].Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Operand1.Definitions[0].Instruction != IRInstruction.MulUnsigned64)
			return false;

		if (!context.Operand1.Definitions[0].Operand2.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Operand2.Definitions[0].Instruction != IRInstruction.MulUnsigned64)
			return false;

		if (!AreSame(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1, context.Operand1.Definitions[0].Operand1.Definitions[0].Operand2))
			return false;

		if (!AreSame(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1, context.Operand1.Definitions[0].Operand2.Definitions[0].Operand1))
			return false;

		if (!IsResolvedConstant(context.Operand2))
			return false;

		if (!IsResolvedConstant(context.Operand1.Definitions[0].Operand2.Definitions[0].Operand2))
			return false;

		if (!IsEvenInteger(context.Operand1.Definitions[0].Operand2.Definitions[0].Operand2))
			return false;

		if (!IsEqual(To64(context.Operand2), Square64(DivUnsigned64(To64(context.Operand1.Definitions[0].Operand2.Definitions[0].Operand2), 2))))
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1;
		var t2 = context.Operand1.Definitions[0].Operand2.Definitions[0].Operand2;

		var v1 = transform.VirtualRegisters.Allocate64();
		var v2 = transform.VirtualRegisters.Allocate64();

		var e1 = Operand.CreateConstant(DivUnsigned64(To64(t2), 2));

		context.SetInstruction(IRInstruction.Add64, v1, t1, e1);
		context.AppendInstruction(IRInstruction.Add64, v2, t1, e1);
		context.AppendInstruction(IRInstruction.MulUnsigned64, result, v2, v1);
	}
}

/// <summary>
/// Unsigned64PerfectSquareFormula_v5
/// </summary>
[Transform("IR.Optimizations.Auto.Algebraic")]
public sealed class Unsigned64PerfectSquareFormula_v5 : BaseTransform
{
	public Unsigned64PerfectSquareFormula_v5() : base(IRInstruction.Add64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		if (!context.Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Instruction != IRInstruction.Add64)
			return false;

		if (!context.Operand2.Definitions[0].Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand2.Definitions[0].Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand2.Definitions[0].Operand1.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Operand1.Definitions[0].Instruction != IRInstruction.MulUnsigned64)
			return false;

		if (!context.Operand2.Definitions[0].Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Operand2.Definitions[0].Instruction != IRInstruction.MulUnsigned64)
			return false;

		if (!AreSame(context.Operand2.Definitions[0].Operand1.Definitions[0].Operand1, context.Operand2.Definitions[0].Operand1.Definitions[0].Operand2))
			return false;

		if (!AreSame(context.Operand2.Definitions[0].Operand1.Definitions[0].Operand1, context.Operand2.Definitions[0].Operand2.Definitions[0].Operand1))
			return false;

		if (!IsResolvedConstant(context.Operand1))
			return false;

		if (!IsResolvedConstant(context.Operand2.Definitions[0].Operand2.Definitions[0].Operand2))
			return false;

		if (!IsEvenInteger(context.Operand2.Definitions[0].Operand2.Definitions[0].Operand2))
			return false;

		if (!IsEqual(To64(context.Operand1), Square64(DivUnsigned64(To64(context.Operand2.Definitions[0].Operand2.Definitions[0].Operand2), 2))))
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;

		var t1 = context.Operand2.Definitions[0].Operand1.Definitions[0].Operand1;
		var t2 = context.Operand2.Definitions[0].Operand2.Definitions[0].Operand2;

		var v1 = transform.VirtualRegisters.Allocate64();
		var v2 = transform.VirtualRegisters.Allocate64();

		var e1 = Operand.CreateConstant(DivUnsigned64(To64(t2), 2));

		context.SetInstruction(IRInstruction.Add64, v1, t1, e1);
		context.AppendInstruction(IRInstruction.Add64, v2, t1, e1);
		context.AppendInstruction(IRInstruction.MulUnsigned64, result, v2, v1);
	}
}

/// <summary>
/// Unsigned64PerfectSquareFormula_v6
/// </summary>
[Transform("IR.Optimizations.Auto.Algebraic")]
public sealed class Unsigned64PerfectSquareFormula_v6 : BaseTransform
{
	public Unsigned64PerfectSquareFormula_v6() : base(IRInstruction.Add64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != IRInstruction.Add64)
			return false;

		if (!context.Operand1.Definitions[0].Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand1.Definitions[0].Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand1.Definitions[0].Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Operand1.Definitions[0].Instruction != IRInstruction.MulUnsigned64)
			return false;

		if (!context.Operand1.Definitions[0].Operand2.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Operand2.Definitions[0].Instruction != IRInstruction.MulUnsigned64)
			return false;

		if (!AreSame(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1, context.Operand1.Definitions[0].Operand2.Definitions[0].Operand1))
			return false;

		if (!AreSame(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1, context.Operand1.Definitions[0].Operand2.Definitions[0].Operand2))
			return false;

		if (!IsResolvedConstant(context.Operand2))
			return false;

		if (!IsResolvedConstant(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand2))
			return false;

		if (!IsEvenInteger(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand2))
			return false;

		if (!IsEqual(To64(context.Operand2), Square64(DivUnsigned64(To64(context.Operand1.Definitions[0].Operand1.Definitions[0].Operand2), 2))))
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1;
		var t2 = context.Operand1.Definitions[0].Operand1.Definitions[0].Operand2;

		var v1 = transform.VirtualRegisters.Allocate64();
		var v2 = transform.VirtualRegisters.Allocate64();

		var e1 = Operand.CreateConstant(DivUnsigned64(To64(t2), 2));

		context.SetInstruction(IRInstruction.Add64, v1, t1, e1);
		context.AppendInstruction(IRInstruction.Add64, v2, t1, e1);
		context.AppendInstruction(IRInstruction.MulUnsigned64, result, v2, v1);
	}
}

/// <summary>
/// Unsigned64PerfectSquareFormula_v7
/// </summary>
[Transform("IR.Optimizations.Auto.Algebraic")]
public sealed class Unsigned64PerfectSquareFormula_v7 : BaseTransform
{
	public Unsigned64PerfectSquareFormula_v7() : base(IRInstruction.Add64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		if (!context.Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Instruction != IRInstruction.Add64)
			return false;

		if (!context.Operand2.Definitions[0].Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand2.Definitions[0].Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand2.Definitions[0].Operand1.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Operand1.Definitions[0].Instruction != IRInstruction.MulUnsigned64)
			return false;

		if (!context.Operand2.Definitions[0].Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Operand2.Definitions[0].Instruction != IRInstruction.MulUnsigned64)
			return false;

		if (!AreSame(context.Operand2.Definitions[0].Operand1.Definitions[0].Operand1, context.Operand2.Definitions[0].Operand2.Definitions[0].Operand1))
			return false;

		if (!AreSame(context.Operand2.Definitions[0].Operand1.Definitions[0].Operand1, context.Operand2.Definitions[0].Operand2.Definitions[0].Operand2))
			return false;

		if (!IsResolvedConstant(context.Operand1))
			return false;

		if (!IsResolvedConstant(context.Operand2.Definitions[0].Operand1.Definitions[0].Operand2))
			return false;

		if (!IsEvenInteger(context.Operand2.Definitions[0].Operand1.Definitions[0].Operand2))
			return false;

		if (!IsEqual(To64(context.Operand1), Square64(DivUnsigned64(To64(context.Operand2.Definitions[0].Operand1.Definitions[0].Operand2), 2))))
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;

		var t1 = context.Operand2.Definitions[0].Operand1.Definitions[0].Operand1;
		var t2 = context.Operand2.Definitions[0].Operand1.Definitions[0].Operand2;

		var v1 = transform.VirtualRegisters.Allocate64();
		var v2 = transform.VirtualRegisters.Allocate64();

		var e1 = Operand.CreateConstant(DivUnsigned64(To64(t2), 2));

		context.SetInstruction(IRInstruction.Add64, v1, t1, e1);
		context.AppendInstruction(IRInstruction.Add64, v2, t1, e1);
		context.AppendInstruction(IRInstruction.MulUnsigned64, result, v2, v1);
	}
}
