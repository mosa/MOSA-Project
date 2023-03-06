// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR;

/// <summary>
/// MulUnsigned32
/// </summary>
public sealed class MulUnsigned32 : BaseIRTransform
{
	public MulUnsigned32() : base(IRInstruction.MulUnsigned32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var v1 = transform.AllocateVirtualRegister32();
		context.SetInstruction2(X86.Mul32, v1, context.Result, context.Operand1, context.Operand2);
	}
}
