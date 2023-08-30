// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.FixedRegisters;

/// <summary>
/// IMul32Constant
/// </summary>
[Transform("x64.FixedRegisters")]
public sealed class IMul64Constant : BaseTransform
{
	public IMul64Constant() : base(X64.IMul64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return context.Operand2.IsConstant;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var operand2 = context.Operand2;

		var v1 = transform.VirtualRegisters.Allocate64();

		context.InsertBefore().AppendInstruction(X64.Mov64, v1, operand2);
		context.Operand2 = v1;
	}
}
