// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Useless;

/// <summary>
/// Store16ZeroExtend16x32
/// </summary>
[Transform("IR.Optimizations.Manual.Useless")]
public sealed class Store16ZeroExtend16x32 : BaseTransform
{
	public Store16ZeroExtend16x32() : base(Framework.IR.Store16, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override int Priority => 85;

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand3.IsVirtualRegister)
			return false;

		if (!context.Operand3.IsDefinedOnce)
			return false;

		if (context.Operand3.Definitions[0].Instruction != Framework.IR.ZeroExtend16x32)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var t1 = context.Operand1;
		var t2 = context.Operand2;
		var t3 = context.Operand3.Definitions[0].Operand1;

		context.SetInstruction(Framework.IR.Store16, null, t1, t2, t3);
	}
}
