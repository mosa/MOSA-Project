// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.Algebraic;

public sealed class Unsigned32AAPlusBBPlus2AB : BaseTransform
{
	public Unsigned32AAPlusBBPlus2AB() : base(IR.Add32, TransformType.Auto | TransformType.Optimization, 75)
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

		if (context.Operand1.Definitions[0].Instruction != IR.Add32)
			return false;

		if (!context.Operand1.Definitions[0].Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand1.Definitions[0].Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand1.Definitions[0].Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Operand1.Definitions[0].Instruction != IR.MulUnsigned32)
			return false;

		if (!context.Operand1.Definitions[0].Operand2.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Operand2.Definitions[0].Instruction != IR.MulUnsigned32)
			return false;

		if (!context.Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Instruction != IR.ShiftLeft32)
			return false;

		if (!context.Operand2.Definitions[0].Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand2.Definitions[0].Operand2.IsConstantOne)
			return false;

		if (!context.Operand2.Definitions[0].Operand1.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Operand1.Definitions[0].Instruction != IR.MulSigned32)
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

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1;
		var t2 = context.Operand1.Definitions[0].Operand2.Definitions[0].Operand1;

		var v1 = transform.VirtualRegisters.Allocate32();
		var v2 = transform.VirtualRegisters.Allocate32();

		context.SetInstruction(IR.Add32, v1, t1, t2);
		context.AppendInstruction(IR.Add32, v2, t1, t2);
		context.AppendInstruction(IR.MulUnsigned32, result, v2, v1);
	}
}

public sealed class Unsigned32AAPlusBBPlus2AB_v1 : BaseTransform
{
	public Unsigned32AAPlusBBPlus2AB_v1() : base(IR.Add32, TransformType.Auto | TransformType.Optimization, 75)
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

		if (context.Operand1.Definitions[0].Instruction != IR.ShiftLeft32)
			return false;

		if (!context.Operand1.Definitions[0].Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand1.Definitions[0].Operand2.IsConstantOne)
			return false;

		if (!context.Operand1.Definitions[0].Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Operand1.Definitions[0].Instruction != IR.MulSigned32)
			return false;

		if (!context.Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Instruction != IR.Add32)
			return false;

		if (!context.Operand2.Definitions[0].Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand2.Definitions[0].Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand2.Definitions[0].Operand1.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Operand1.Definitions[0].Instruction != IR.MulUnsigned32)
			return false;

		if (!context.Operand2.Definitions[0].Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Operand2.Definitions[0].Instruction != IR.MulUnsigned32)
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

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1;
		var t2 = context.Operand1.Definitions[0].Operand1.Definitions[0].Operand2;

		var v1 = transform.VirtualRegisters.Allocate32();
		var v2 = transform.VirtualRegisters.Allocate32();

		context.SetInstruction(IR.Add32, v1, t1, t2);
		context.AppendInstruction(IR.Add32, v2, t1, t2);
		context.AppendInstruction(IR.MulUnsigned32, result, v2, v1);
	}
}

public sealed class Unsigned32AAPlusBBPlus2AB_v2 : BaseTransform
{
	public Unsigned32AAPlusBBPlus2AB_v2() : base(IR.Add32, TransformType.Auto | TransformType.Optimization, 75)
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

		if (context.Operand1.Definitions[0].Instruction != IR.Add32)
			return false;

		if (!context.Operand1.Definitions[0].Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand1.Definitions[0].Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand1.Definitions[0].Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Operand1.Definitions[0].Instruction != IR.MulUnsigned32)
			return false;

		if (!context.Operand1.Definitions[0].Operand2.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Operand2.Definitions[0].Instruction != IR.MulUnsigned32)
			return false;

		if (!context.Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Instruction != IR.ShiftLeft32)
			return false;

		if (!context.Operand2.Definitions[0].Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand2.Definitions[0].Operand2.IsConstantOne)
			return false;

		if (!context.Operand2.Definitions[0].Operand1.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Operand1.Definitions[0].Instruction != IR.MulSigned32)
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

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1;
		var t2 = context.Operand1.Definitions[0].Operand2.Definitions[0].Operand1;

		var v1 = transform.VirtualRegisters.Allocate32();
		var v2 = transform.VirtualRegisters.Allocate32();

		context.SetInstruction(IR.Add32, v1, t1, t2);
		context.AppendInstruction(IR.Add32, v2, t1, t2);
		context.AppendInstruction(IR.MulUnsigned32, result, v2, v1);
	}
}

public sealed class Unsigned32AAPlusBBPlus2AB_v3 : BaseTransform
{
	public Unsigned32AAPlusBBPlus2AB_v3() : base(IR.Add32, TransformType.Auto | TransformType.Optimization, 75)
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

		if (context.Operand1.Definitions[0].Instruction != IR.ShiftLeft32)
			return false;

		if (!context.Operand1.Definitions[0].Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand1.Definitions[0].Operand2.IsConstantOne)
			return false;

		if (!context.Operand1.Definitions[0].Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Operand1.Definitions[0].Instruction != IR.MulSigned32)
			return false;

		if (!context.Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Instruction != IR.Add32)
			return false;

		if (!context.Operand2.Definitions[0].Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand2.Definitions[0].Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand2.Definitions[0].Operand1.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Operand1.Definitions[0].Instruction != IR.MulUnsigned32)
			return false;

		if (!context.Operand2.Definitions[0].Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Operand2.Definitions[0].Instruction != IR.MulUnsigned32)
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

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1;
		var t2 = context.Operand1.Definitions[0].Operand1.Definitions[0].Operand2;

		var v1 = transform.VirtualRegisters.Allocate32();
		var v2 = transform.VirtualRegisters.Allocate32();

		context.SetInstruction(IR.Add32, v1, t2, t1);
		context.AppendInstruction(IR.Add32, v2, t2, t1);
		context.AppendInstruction(IR.MulUnsigned32, result, v2, v1);
	}
}

public sealed class Unsigned32AAPlusBBPlus2AB_v4 : BaseTransform
{
	public Unsigned32AAPlusBBPlus2AB_v4() : base(IR.Add32, TransformType.Auto | TransformType.Optimization, 75)
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

		if (context.Operand1.Definitions[0].Instruction != IR.Add32)
			return false;

		if (!context.Operand1.Definitions[0].Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand1.Definitions[0].Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand1.Definitions[0].Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Operand1.Definitions[0].Instruction != IR.MulUnsigned32)
			return false;

		if (!context.Operand1.Definitions[0].Operand2.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Operand2.Definitions[0].Instruction != IR.MulUnsigned32)
			return false;

		if (!context.Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Instruction != IR.ShiftLeft32)
			return false;

		if (!context.Operand2.Definitions[0].Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand2.Definitions[0].Operand2.IsConstantOne)
			return false;

		if (!context.Operand2.Definitions[0].Operand1.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Operand1.Definitions[0].Instruction != IR.MulSigned32)
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

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1;
		var t2 = context.Operand1.Definitions[0].Operand2.Definitions[0].Operand1;

		var v1 = transform.VirtualRegisters.Allocate32();
		var v2 = transform.VirtualRegisters.Allocate32();

		context.SetInstruction(IR.Add32, v1, t2, t1);
		context.AppendInstruction(IR.Add32, v2, t2, t1);
		context.AppendInstruction(IR.MulUnsigned32, result, v2, v1);
	}
}

public sealed class Unsigned32AAPlusBBPlus2AB_v5 : BaseTransform
{
	public Unsigned32AAPlusBBPlus2AB_v5() : base(IR.Add32, TransformType.Auto | TransformType.Optimization, 75)
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

		if (context.Operand1.Definitions[0].Instruction != IR.ShiftLeft32)
			return false;

		if (!context.Operand1.Definitions[0].Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand1.Definitions[0].Operand2.IsConstantOne)
			return false;

		if (!context.Operand1.Definitions[0].Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Operand1.Definitions[0].Instruction != IR.MulSigned32)
			return false;

		if (!context.Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Instruction != IR.Add32)
			return false;

		if (!context.Operand2.Definitions[0].Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand2.Definitions[0].Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand2.Definitions[0].Operand1.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Operand1.Definitions[0].Instruction != IR.MulUnsigned32)
			return false;

		if (!context.Operand2.Definitions[0].Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Operand2.Definitions[0].Instruction != IR.MulUnsigned32)
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

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1;
		var t2 = context.Operand1.Definitions[0].Operand1.Definitions[0].Operand2;

		var v1 = transform.VirtualRegisters.Allocate32();
		var v2 = transform.VirtualRegisters.Allocate32();

		context.SetInstruction(IR.Add32, v1, t1, t2);
		context.AppendInstruction(IR.Add32, v2, t1, t2);
		context.AppendInstruction(IR.MulUnsigned32, result, v2, v1);
	}
}

public sealed class Unsigned32AAPlusBBPlus2AB_v6 : BaseTransform
{
	public Unsigned32AAPlusBBPlus2AB_v6() : base(IR.Add32, TransformType.Auto | TransformType.Optimization, 75)
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

		if (context.Operand1.Definitions[0].Instruction != IR.Add32)
			return false;

		if (!context.Operand1.Definitions[0].Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand1.Definitions[0].Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand1.Definitions[0].Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Operand1.Definitions[0].Instruction != IR.MulUnsigned32)
			return false;

		if (!context.Operand1.Definitions[0].Operand2.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Operand2.Definitions[0].Instruction != IR.MulUnsigned32)
			return false;

		if (!context.Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Instruction != IR.ShiftLeft32)
			return false;

		if (!context.Operand2.Definitions[0].Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand2.Definitions[0].Operand2.IsConstantOne)
			return false;

		if (!context.Operand2.Definitions[0].Operand1.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Operand1.Definitions[0].Instruction != IR.MulSigned32)
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

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1;
		var t2 = context.Operand1.Definitions[0].Operand2.Definitions[0].Operand1;

		var v1 = transform.VirtualRegisters.Allocate32();
		var v2 = transform.VirtualRegisters.Allocate32();

		context.SetInstruction(IR.Add32, v1, t2, t1);
		context.AppendInstruction(IR.Add32, v2, t2, t1);
		context.AppendInstruction(IR.MulUnsigned32, result, v2, v1);
	}
}

public sealed class Unsigned32AAPlusBBPlus2AB_v7 : BaseTransform
{
	public Unsigned32AAPlusBBPlus2AB_v7() : base(IR.Add32, TransformType.Auto | TransformType.Optimization, 75)
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

		if (context.Operand1.Definitions[0].Instruction != IR.ShiftLeft32)
			return false;

		if (!context.Operand1.Definitions[0].Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand1.Definitions[0].Operand2.IsConstantOne)
			return false;

		if (!context.Operand1.Definitions[0].Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Operand1.Definitions[0].Instruction != IR.MulSigned32)
			return false;

		if (!context.Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Instruction != IR.Add32)
			return false;

		if (!context.Operand2.Definitions[0].Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand2.Definitions[0].Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand2.Definitions[0].Operand1.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Operand1.Definitions[0].Instruction != IR.MulUnsigned32)
			return false;

		if (!context.Operand2.Definitions[0].Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Operand2.Definitions[0].Instruction != IR.MulUnsigned32)
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

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1;
		var t2 = context.Operand1.Definitions[0].Operand1.Definitions[0].Operand2;

		var v1 = transform.VirtualRegisters.Allocate32();
		var v2 = transform.VirtualRegisters.Allocate32();

		context.SetInstruction(IR.Add32, v1, t2, t1);
		context.AppendInstruction(IR.Add32, v2, t2, t1);
		context.AppendInstruction(IR.MulUnsigned32, result, v2, v1);
	}
}
