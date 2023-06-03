// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Special;

public sealed class StoreLoad32 : BaseTransform
{
	public StoreLoad32() : base(IRInstruction.Store32, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		if (!context.Operand2.IsLocalStack)
			return false;

		if (!context.Operand1.IsCPURegister)
			return false;

		if (context.Operand1 != transform.StackFrame)
			return false;

		if (context.Operand2.Uses.Count != 2)
			return false;

		if (!(context.Operand3.IsConstant || context.Operand3.IsDefinedOnce))
			return false;

		var load = context.Operand2.Uses[0] != context.Node
			? context.Operand2.Uses[0]
			: context.Operand2.Uses[1];

		if (load.Instruction != IRInstruction.Load32)
			return false;

		if (!(context.Operand3.IsConstant || context.Operand3.IsDefinedOnce))
			return false;

		if (load.Operand1 != transform.StackFrame)
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var load = context.Operand2.Uses[0] != context.Node
			? context.Operand2.Uses[0]
			: context.Operand2.Uses[1];

		context.SetInstruction(IRInstruction.Move32, load.Result, context.Operand3);
		load.SetNop();
	}
}
