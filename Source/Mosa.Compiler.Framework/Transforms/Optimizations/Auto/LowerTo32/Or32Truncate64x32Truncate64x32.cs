// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.LowerTo32;

/// <summary>
/// Or32Truncate64x32Truncate64x32
/// </summary>
[Transform("IR.Optimizations.Auto.LowerTo32")]
public sealed class Or32Truncate64x32Truncate64x32 : BaseTransform
{
	public Or32Truncate64x32Truncate64x32() : base(Framework.IR.Or32, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!transform.Is32BitPlatform)
			return false;

		if (!transform.IsLowerTo32)
			return false;

		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != Framework.IR.Truncate64x32)
			return false;

		if (!context.Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Instruction != Framework.IR.Truncate64x32)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand1;
		var t2 = context.Operand2.Definitions[0].Operand1;

		var v1 = transform.VirtualRegisters.Allocate64();

		context.SetInstruction(Framework.IR.Or64, v1, t1, t2);
		context.AppendInstruction(Framework.IR.GetLow32, result, v1);
	}
}
