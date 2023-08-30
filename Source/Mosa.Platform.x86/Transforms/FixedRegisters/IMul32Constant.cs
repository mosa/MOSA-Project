// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.FixedRegisters;

/// <summary>
/// IMul32Constant
/// </summary>
[Transform("x86.FixedRegisters")]
public sealed class IMul32Constant : BaseTransform
{
	public IMul32Constant() : base(X86.IMul32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return context.Operand2.IsConstant;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var operand2 = context.Operand2;

		var v1 = transform.VirtualRegisters.Allocate32();

		context.InsertBefore().AppendInstruction(X86.Mov32, v1, operand2);
		context.Operand2 = v1;
	}
}
