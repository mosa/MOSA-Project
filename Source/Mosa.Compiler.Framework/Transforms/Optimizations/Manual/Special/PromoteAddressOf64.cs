// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Special;

public sealed class PromoteAddressOf64 : BaseTransform
{
	public PromoteAddressOf64() : base(IRInstruction.AddressOf, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		if (!context.Operand1.IsLocalStack)
			return false;

		if (context.Operand1.Uses.Count != 2)
			return false;

		if (context.Result.Uses.Count != 1)
			return false;

		if (!IsSSAForm(context.Result))
			return false;

		var store = context.Result.Uses[0];

		if (store.Instruction != IRInstruction.Store64)
			return false;

		if (store.Operand1 != context.Result)
			return false;

		if (!store.Operand2.IsConstantZero)
			return false;

		var load = context.Operand1.Uses[0] != context.Node
			? context.Operand1.Uses[0]
			: context.Operand1.Uses[1];

		if (load.Instruction != IRInstruction.Load64)
			return false;

		if (load.Operand1 != transform.StackFrame)
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var store = context.Result.Uses[0];

		var load = context.Operand1.Uses[0] != context.Node
			? context.Operand1.Uses[0]
			: context.Operand1.Uses[1];

		load.SetInstruction(IRInstruction.MoveManagedPointer, load.Result, store.Operand3);
		context.SetNop();
		store.SetNop();
	}
}
