// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.Tweak;

/// <summary>
/// Cmp32
/// </summary>
[Transform("x64.Tweak")]
public sealed class Cmp32 : BaseTransform
{
	public Cmp32() : base(X64.Cmp32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		return context.Operand1.IsConstant;
	}

	public override void Transform(Context context, Transform transform)
	{
		var left = context.Operand1;

		var v1 = transform.VirtualRegisters.Allocate(left);

		context.InsertBefore().AppendInstruction(X64.Mov32, v1, left);
		context.Operand1 = v1;
	}
}
