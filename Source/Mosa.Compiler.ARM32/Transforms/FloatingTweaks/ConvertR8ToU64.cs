// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.FloatingTweaks;

/// <summary>
/// ConvertR8ToI64
/// </summary>
public sealed class ConvertR8ToU64 : BaseTransform
{
	public ConvertR8ToU64() : base(IR.ConvertR8ToU64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;

		transform.SplitOperand(result, out _, out _);

		var v1 = transform.VirtualRegisters.Allocate32();

		context.SetInstruction(IR.ConvertR8ToI32, v1, operand1);
		context.AppendInstruction(IR.To64, result, Operand.Constant32_0, v1);
	}
}
