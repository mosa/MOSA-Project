// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.Tweak;

/// <summary>
/// Cmp64
/// </summary>
public sealed class Cmp64 : BaseTransform
{
	public Cmp64() : base(X64.Cmp64, TransformType.Manual | TransformType.Transform)
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

		context.InsertBefore().AppendInstruction(X64.Mov64, v1, left);
		context.Operand1 = v1;
	}
}
