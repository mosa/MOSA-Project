// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Special;

public sealed class StoreLoad64 : BaseTransform
{
	public StoreLoad64() : base(Framework.IR.Store64, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand2.IsLocalStack)
			return false;

		if (!context.Operand1.IsPhysicalRegister)
			return false;

		if (context.Operand1 != transform.StackFrame)
			return false;

		if (context.Operand2.Uses.Count != 2)   // FUTURE: traverse all uses
			return false;

		if (!(context.Operand3.IsConstant || context.Operand3.IsDefinedOnce))
			return false;

		if (!context.Operand3.IsDefinedOnce)
			return false;

		var load = context.Operand2.Uses[0] != context.Node
			? context.Operand2.Uses[0]
			: context.Operand2.Uses[1];

		if (load.Instruction != Framework.IR.Load64)
			return false;

		if (!(context.Operand3.IsConstant || context.Operand3.IsDefinedOnce))
			return false;

		if (load.Operand1 != transform.StackFrame)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var load = context.Operand2.Uses[0] != context.Node
			? context.Operand2.Uses[0]
			: context.Operand2.Uses[1];

		context.SetInstruction(Framework.IR.Move64, load.Result, context.Operand3);
		load.SetNop();
	}
}
