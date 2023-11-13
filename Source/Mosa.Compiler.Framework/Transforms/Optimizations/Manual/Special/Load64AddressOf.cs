// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Special;

public sealed class Load64AddressOf : BaseTransform
{
	public Load64AddressOf() : base(Framework.IR.Load64, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand2.IsConstantZero)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != Framework.IR.AddressOf)
			return false;

		if (!context.Operand1.Definitions[0].Operand1.IsLocalStack)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetInstruction(Framework.IR.Load64, context.Result, transform.StackFrame, context.Operand1.Definitions[0].Operand1);
	}
}
