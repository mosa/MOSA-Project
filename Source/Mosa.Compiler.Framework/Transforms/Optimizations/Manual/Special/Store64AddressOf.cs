﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Special;

public sealed class Store64AddressOf : BaseTransform
{
	public Store64AddressOf() : base(IRInstruction.Store64, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand2.IsConstantZero)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != IRInstruction.AddressOf)
			return false;

		if (!context.Operand1.Definitions[0].Operand1.IsLocalStack)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetInstruction(IRInstruction.Store64, null, transform.StackFrame, context.Operand1.Definitions[0].Operand1, context.Operand3);
	}
}
